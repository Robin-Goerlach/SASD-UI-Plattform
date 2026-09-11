using System.Reflection;
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
            NavigationHostLifecycleChecks.Run();
            ValidateBreadcrumbAccessibilityAndLifecycle();
            ValidateDocumentTabsKeyboardAndAccessibility();
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

    private static void ValidateBreadcrumbAccessibilityAndLifecycle()
    {
        using var form = new Form
        {
            ClientSize = new Size(640, 180),
            Location = new Point(-32000, -32000),
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Text = "SASD breadcrumb smoke",
        };
        using var breadcrumb = new SasdBreadcrumb { Dock = DockStyle.Top };
        form.Controls.Add(breadcrumb);

        // Host the breadcrumb in a real but off-screen WinForms window. This gives the
        // generated LinkLabel controls normal handle/focus semantics without SendKeys or
        // process-global input injection on a developer or CI desktop.
        form.Show();

        Ensure(breadcrumb.AccessibleRole == AccessibleRole.Grouping && breadcrumb.AccessibleName == "Breadcrumb",
            "Breadcrumb does not expose stable group-level accessibility semantics.");
        Ensure(breadcrumb.AccessibleDescription == "No breadcrumb locations.",
            "Empty breadcrumb does not expose an explicit accessible empty state.");

        breadcrumb.SetPath([
            SasdBreadcrumbItem.Create("root", "Customers"),
            SasdBreadcrumbItem.Create("customer-42", "Example AG"),
            SasdBreadcrumbItem.Create("contract-7", "Contract 7"),
        ]);

        FlowLayoutPanel host = breadcrumb.Controls.OfType<FlowLayoutPanel>().Single();
        Ensure(host.AccessibleRole == AccessibleRole.Grouping && host.AccessibleName == "Breadcrumb path" && !host.TabStop,
            "Breadcrumb path host does not expose a non-interactive accessible grouping.");
        Ensure(breadcrumb.AccessibleDescription?.Contains("3 breadcrumb locations", StringComparison.Ordinal) == true &&
               breadcrumb.AccessibleDescription.Contains("Current location: Contract 7", StringComparison.Ordinal),
            "Breadcrumb accessible state does not describe the path size/current location.");

        LinkLabel[] links = host.Controls.OfType<LinkLabel>().ToArray();
        Label current = host.Controls.OfType<Label>()
            .Single(label => label.AccessibleRole == AccessibleRole.StaticText);
        Label[] separators = host.Controls.OfType<Label>()
            .Where(label => label.AccessibleRole == AccessibleRole.Separator)
            .ToArray();

        Ensure(links.Length == 2 && links.All(static link =>
                link.AccessibleRole == AccessibleRole.Link &&
                link.TabStop &&
                !string.IsNullOrWhiteSpace(link.AccessibleDescription)),
            "Navigable breadcrumb locations do not expose selectable link semantics.");
        Ensure(separators.Length == 2 && separators.All(static separator => !separator.TabStop),
            "Decorative breadcrumb separators entered keyboard navigation.");
        Ensure(current.AccessibleName == "Current location Contract 7" && !current.TabStop,
            "Current breadcrumb location is not exposed as non-interactive static text.");

        links[0].Select();
        Ensure(links[0].Focused,
            "A breadcrumb navigation link could not receive focus in a real WinForms host.");

        SasdBreadcrumbItem? invokedItem = null;
        int invocations = 0;
        breadcrumb.ItemInvoked += (_, args) =>
        {
            invocations++;
            invokedItem = args.Item;
        };

        // LinkLabel owns its native keyboard/mouse activation behavior. Invoke the protected
        // LinkClicked boundary through reflection rather than adding a test-only public hook
        // to product code; this verifies that the generated link is wired to the SASD event
        // with the correct stable application-owned breadcrumb item.
        RaiseLinkClicked(links[0]);
        Ensure(invocations == 1 && invokedItem?.Id == "root",
            "Breadcrumb link invocation did not publish the expected stable path item exactly once.");

        Control[] generatedControls = host.Controls.Cast<Control>().ToArray();
        breadcrumb.SetPath([
            SasdBreadcrumbItem.Create("root", "Customers"),
            SasdBreadcrumbItem.Create("customer-99", "New Customer"),
        ]);
        Ensure(generatedControls.All(static control => control.IsDisposed),
            "Breadcrumb rebuild detached generated controls without disposing them.");
        Ensure(breadcrumb.AccessibleDescription?.Contains("2 breadcrumb locations", StringComparison.Ordinal) == true &&
               breadcrumb.AccessibleDescription.Contains("Current location: New Customer", StringComparison.Ordinal),
            "Breadcrumb accessible state did not refresh after path replacement.");

        breadcrumb.ClearPath();
        Ensure(breadcrumb.ItemCount == 0 && breadcrumb.AccessibleDescription == "No breadcrumb locations.",
            "Clearing the breadcrumb did not restore its accessible empty state.");

        form.Hide();
    }

    private static void ValidateDocumentTabsKeyboardAndAccessibility()
    {
        using var form = new Form
        {
            ClientSize = new Size(640, 360),
            Location = new Point(-32000, -32000),
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Text = "SASD document-tab smoke",
        };
        using var tabs = new TestDocumentTabs { Dock = DockStyle.Fill };
        form.Controls.Add(tabs);

        // Selection notifications originate from the native TabControl. Host it in a real,
        // off-screen window so the smoke exercises the same handle lifecycle as an application
        // rather than drawing conclusions from a handleless in-memory TabControl.
        form.Show();

        Ensure(tabs.AccessibleRole == AccessibleRole.Grouping && tabs.AccessibleName == "Documents",
            "Document host does not expose stable group-level accessibility semantics.");

        TabControl tabControl = tabs.Controls.OfType<TabControl>().Single();
        Ensure(tabControl.IsHandleCreated,
            "Document tab list did not receive a native handle from its host window.");
        Ensure(tabControl.AccessibleRole == AccessibleRole.PageTabList && tabControl.AccessibleName == "Document tabs",
            "Native document tab list does not expose explicit page-tab accessibility semantics.");

        Control first = tabs.OpenOrSelect("one", "First", static () => new Panel());
        Control same = tabs.OpenOrSelect("ONE", "First renamed", static () => new Label());
        tabs.OpenOrSelect("two", "Second", static () => new Panel());

        Ensure(ReferenceEquals(first, same),
            "Reopening an existing document id created replacement content.");
        Ensure(tabControl.TabPages[0].Text == "First renamed" &&
               tabControl.TabPages[0].AccessibleName == "First renamed",
            "Reopening an existing document with a new title left stale accessible tab text.");

        tabs.SelectDocument("one");
        int selectedEvents = 0;
        tabs.DocumentSelected += (_, _) => selectedEvents++;

        // SasdDocumentTabs guarantees the conventional Ctrl+Tab / Ctrl+Shift+Tab document
        // switching path itself. A test subclass exposes ProcessCmdKey without injecting input
        // into the process-global desktop session.
        Ensure(tabs.RaiseCommandKey(Keys.Control | Keys.Tab),
            "Ctrl+Tab was not handled while more than one document was open.");
        Ensure(string.Equals(tabs.SelectedDocumentId, "two", StringComparison.OrdinalIgnoreCase),
            "Ctrl+Tab did not select the next document.");

        Ensure(tabs.RaiseCommandKey(Keys.Control | Keys.Shift | Keys.Tab),
            "Ctrl+Shift+Tab was not handled while more than one document was open.");
        Ensure(string.Equals(tabs.SelectedDocumentId, "one", StringComparison.OrdinalIgnoreCase),
            "Ctrl+Shift+Tab did not select the previous document.");
        Ensure(selectedEvents == 2,
            $"Keyboard document switching published {selectedEvents} selection events instead of two.");

        form.Hide();
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

    private static void RaiseLinkClicked(LinkLabel link)
    {
        MethodInfo? onLinkClicked = typeof(LinkLabel).GetMethod(
            "OnLinkClicked",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (onLinkClicked is null)
        {
            throw new InvalidOperationException("Could not locate the protected LinkLabel.OnLinkClicked method.");
        }

        if (link.Links.Count == 0)
        {
            throw new InvalidOperationException("Generated breadcrumb LinkLabel does not expose a link area.");
        }

        onLinkClicked.Invoke(link, [new LinkLabelLinkClickedEventArgs(link.Links[0])]);
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

    private sealed class TestDocumentTabs : SasdDocumentTabs
    {
        public bool RaiseCommandKey(Keys keys)
        {
            Message message = default;
            return ProcessCmdKey(ref message, keys);
        }
    }
}
