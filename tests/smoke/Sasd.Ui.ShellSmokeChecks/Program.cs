using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.ShellSmokeChecks;

internal static class Program
{
    [STAThread]
    private static async Task<int> Main()
    {
        try
        {
            await ValidateCommandManagerAsync();
            await ValidateShortcutBindingAsync();
            ValidateStatusServiceAndBinding();
            ValidateShellComposition();
            ValidateNotificationHost();

            Console.WriteLine("SASD shell integration smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static async Task ValidateCommandManagerAsync()
    {
        var runner = new SasdCommandRunner();
        var manager = new SasdCommandManager(runner);
        int executions = 0;
        var save = new SasdCommand(
            "save",
            "Save",
            _ =>
            {
                executions++;
                return Task.CompletedTask;
            },
            shortcutKeys: Keys.Control | Keys.S);

        manager.Register(save);
        Ensure(manager.Count == 1, "Command manager did not retain the registered command.");
        Ensure(ReferenceEquals(manager.Runner, runner), "Command manager did not retain the supplied runner.");
        Ensure(ReferenceEquals(manager.Find("SAVE"), save), "Command lookup is not case-insensitive by stable id.");
        Ensure(ReferenceEquals(manager.FindByShortcut(Keys.Control | Keys.S), save), "Shortcut lookup returned the wrong command.");

        EnsureThrows<ArgumentException>(
            () => manager.Register(new SasdCommand("save", "Duplicate", _ => Task.CompletedTask)),
            "Command manager accepted a duplicate command id.");
        EnsureThrows<ArgumentException>(
            () => manager.Register(new SasdCommand(
                "save-copy",
                "Duplicate shortcut",
                _ => Task.CompletedTask,
                shortcutKeys: Keys.Control | Keys.S)),
            "Command manager accepted a duplicate global shortcut.");

        var unknown = await manager.ExecuteAsync("missing");
        Ensure(!unknown.Succeeded && unknown.ErrorCode == "COMMAND_NOT_REGISTERED",
            "Unknown command id was not reported as a recoverable result.");

        var success = await manager.ExecuteAsync("save");
        Ensure(success.Succeeded && executions == 1, "Registered command did not execute exactly once.");
        Ensure(manager.Remove("save") && manager.Count == 0, "Registered command could not be removed.");
    }

    private static async Task ValidateShortcutBindingAsync()
    {
        using var form = new TestForm { KeyPreview = false };
        var manager = new SasdCommandManager();
        int executions = 0;
        var command = new SasdCommand(
            "refresh",
            "Refresh",
            _ =>
            {
                executions++;
                return Task.CompletedTask;
            },
            shortcutKeys: Keys.Control | Keys.R);
        manager.Register(command);

        var completion = new TaskCompletionSource<SasdShortcutCommandCompletedEventArgs>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var binding = new SasdCommandShortcutBinding(form, manager);
        binding.CommandCompleted += (_, args) => completion.TrySetResult(args);

        Ensure(form.KeyPreview, "Shortcut binding did not enable KeyPreview.");
        KeyEventArgs keyEvent = form.RaiseKeyDown(Keys.Control | Keys.R);
        Ensure(keyEvent.Handled && keyEvent.SuppressKeyPress, "Claimed shortcut was not suppressed for child controls.");

        SasdShortcutCommandCompletedEventArgs completed = await completion.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Ensure(completed.Result.Succeeded && executions == 1, "Shortcut command did not execute successfully.");
        Ensure(ReferenceEquals(completed.Command, command), "Shortcut completion reported the wrong command.");

        command.Enabled = false;
        KeyEventArgs disabledEvent = form.RaiseKeyDown(Keys.Control | Keys.R);
        Ensure(!disabledEvent.Handled && !disabledEvent.SuppressKeyPress,
            "Disabled command incorrectly claimed its global shortcut.");

        binding.Dispose();
        Ensure(!form.KeyPreview, "Shortcut binding did not restore the form's original KeyPreview state.");
    }

    private static void ValidateStatusServiceAndBinding()
    {
        using var statusBar = new SasdStatusBar();
        statusBar.CreateControl();
        var service = new SasdStatusService();
        using var binding = new SasdStatusBinding(service, statusBar);

        service.Publish("Working", SasdStatusSeverity.Information, priority: 2);
        Ensure(statusBar.Items[0].Text == "Working", "Status binding did not display a published message.");

        service.Publish("Lower priority", SasdStatusSeverity.Information, priority: 1);
        Ensure(statusBar.Items[0].Text == "Working", "Lower-priority status overwrote the active message.");

        service.Clear();
        Ensure(statusBar.Items[0].Text == statusBar.IdleText, "Status clear did not restore idle text.");

        EnsureThrows<ArgumentException>(
            () => service.Publish(" "),
            "Status service accepted empty status text.");
        EnsureThrows<ArgumentOutOfRangeException>(
            () => service.Publish("Invalid lifetime", lifetime: TimeSpan.Zero),
            "Status service accepted a non-positive lifetime.");
    }

    private static void ValidateShellComposition()
    {
        using var shell = new SasdShellForm();
        Ensure(shell.CommandManager.Count == 0, "New shell unexpectedly contains application commands.");
        Ensure(shell.CommandBar.CommandCount == 0, "New shell unexpectedly contains command-bar entries.");
        Ensure(shell.NavigationHost.Dock == DockStyle.Fill, "Shell navigation host is not configured as the main content surface.");

        var toolbarCommand = new SasdCommand("new", "New", _ => Task.CompletedTask, shortcutKeys: Keys.Control | Keys.N);
        ToolStripButton? toolbarButton = shell.RegisterCommand(toolbarCommand);
        Ensure(toolbarButton is not null, "Visible shell command did not create a toolbar button.");
        Ensure(shell.CommandManager.Count == 1 && shell.CommandBar.CommandCount == 1,
            "Shell command registration did not update both global and toolbar surfaces.");

        var hiddenCommand = new SasdCommand("find", "Find", _ => Task.CompletedTask, shortcutKeys: Keys.Control | Keys.F);
        ToolStripButton? hiddenButton = shell.RegisterCommand(hiddenCommand, showOnCommandBar: false);
        Ensure(hiddenButton is null, "Shortcut-only shell command unexpectedly created a toolbar button.");
        Ensure(shell.CommandManager.Count == 2 && shell.CommandBar.CommandCount == 1,
            "Shortcut-only shell command was registered incorrectly.");

        Ensure(shell.UnregisterCommand("find"), "Shell could not unregister a shortcut-only command.");
        Ensure(shell.CommandManager.Count == 1, "Shell command registry retained an unregistered command.");
    }

    private static void ValidateNotificationHost()
    {
        using var host = new SasdNotificationHost();
        host.CreateControl();
        var service = new SasdNotificationService();
        int shown = 0;
        int dismissed = 0;
        host.NotificationShown += (_, _) => shown++;
        host.NotificationDismissed += (_, _) => dismissed++;
        host.Bind(service);

        service.Publish(
            "Saved successfully.",
            "Save",
            SasdNotificationSeverity.Success,
            TimeSpan.FromSeconds(30));

        SasdNotification? current = host.CurrentNotification;
        Ensure(current is not null, "Notification host did not receive a bound service publication.");
        Ensure(current?.Severity == SasdNotificationSeverity.Success,
            "Notification host changed notification severity.");
        Ensure(shown == 1, "Notification host did not report exactly one shown notification.");

        host.Dismiss();
        Ensure(host.CurrentNotification is null && dismissed == 1,
            "Notification host did not dismiss its current notification predictably.");

        host.Unbind();
        service.Publish("Detached notification");
        Ensure(shown == 1, "Notification host continued receiving events after Unbind.");
    }

    private static void EnsureThrows<TException>(Action action, string message)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed class TestForm : Form
    {
        public KeyEventArgs RaiseKeyDown(Keys keys)
        {
            var args = new KeyEventArgs(keys);
            OnKeyDown(args);
            return args;
        }
    }
}
