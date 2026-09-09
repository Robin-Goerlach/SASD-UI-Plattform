using System.Text;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.WinFormsSmokeChecks;

internal static class Program
{
    [STAThread]
    private static async Task<int> Main()
    {
        try
        {
            await ValidateCommandsAsync();
            await ValidateCsvAndGridStateAsync();
            await ValidateFormsAsync();
            await ValidateUiDispatcherAsync();
            ValidateShell();
            ValidateWindowsShellSafety();

            Console.WriteLine("SASD WinForms foundation smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static async Task ValidateCommandsAsync()
    {
        var runner = new SasdCommandRunner();
        int executions = 0;
        var command = new SasdCommand(
            "smoke.execute",
            "Execute",
            _ =>
            {
                executions++;
                return Task.CompletedTask;
            });

        var result = await runner.ExecuteAsync(command);
        Ensure(result.Succeeded && executions == 1, "Command did not execute exactly once.");

        command.Enabled = false;
        result = await runner.ExecuteAsync(command);
        Ensure(!result.Succeeded && result.ErrorCode == "COMMAND_DISABLED", "Disabled command was not rejected.");

        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var longCommand = new SasdCommand("smoke.long", "Long", _ => release.Task);
        var first = runner.ExecuteAsync(longCommand);
        var second = await runner.ExecuteAsync(longCommand);
        Ensure(!second.Succeeded && second.ErrorCode == "COMMAND_ALREADY_RUNNING", "Command re-entry was not blocked.");
        release.SetResult();
        Ensure((await first).Succeeded, "Initial long-running command did not complete.");
    }

    private static async Task ValidateCsvAndGridStateAsync()
    {
        using var grid = new SasdDataGrid
        {
            AutoGenerateColumns = false,
            FillAvailableWidth = false,
        };
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "Name", Width = 180 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "note", HeaderText = "Note", Width = 220 });
        grid.Rows.Add("Example AG", "Contains, comma and \"quote\"");

        var captured = SasdDataGridState.Capture(grid);
        grid.Columns[0].Width = 60;
        grid.Columns[1].Visible = false;
        captured.Restore(grid);
        Ensure(grid.Columns[0].Width == 180, "Grid width state was not restored.");
        Ensure(grid.Columns[1].Visible, "Grid visibility state was not restored.");

        await using var stream = new MemoryStream();
        await SasdCsvExporter.ExportAsync(grid, stream);
        string csv = Encoding.UTF8.GetString(stream.ToArray());
        Ensure(csv.Contains("Name,Note", StringComparison.Ordinal), "CSV header is missing.");
        Ensure(csv.Contains("\"Contains, comma and \"\"quote\"\"\"", StringComparison.Ordinal), "CSV escaping is incorrect.");

        var query = new SasdDataQuery(0, 100, [SasdSortDescriptor.Create("Name", SasdSortDirection.Ascending)], "example");
        Ensure(ReferenceEquals(query, query.Validate()), "Valid data query was unexpectedly replaced.");
    }

    private static async Task ValidateFormsAsync()
    {
        using var form = new Form();
        using var textBox = new TextBox();
        form.Controls.Add(textBox);
        using var coordinator = new SasdValidationCoordinator(form);
        coordinator.AddRequired("name", textBox, "Name");

        var invalid = await coordinator.ValidateAsync();
        Ensure(!invalid.IsValid && invalid.Messages.Count == 1, "Required-field validation did not fail.");

        textBox.Text = "Example";
        var valid = await coordinator.ValidateAsync();
        Ensure(valid.IsValid, "Required-field validation did not recover after input.");
    }

    private static async Task ValidateUiDispatcherAsync()
    {
        using var control = new Control();

        // A missing handle is ambiguous in WinForms: InvokeRequired can return false
        // even from a worker thread. The SASD dispatcher rejects this unsafe state.
        await EnsureThrowsAsync<InvalidOperationException>(
            () => SasdUiDispatcher.InvokeAsync(control, static () => { }),
            "Dispatcher accepted a control without a native handle.");

        control.CreateControl();
        bool executed = false;
        await SasdUiDispatcher.InvokeAsync(control, () => executed = true);
        Ensure(executed, "Dispatcher did not execute work on a created control.");

        control.Dispose();
        await EnsureThrowsAsync<ObjectDisposedException>(
            () => SasdUiDispatcher.InvokeAsync(control, static () => { }),
            "Dispatcher accepted a disposed control.");
    }

    private static void ValidateShell()
    {
        using var host = new SasdNavigationHost();
        host.RegisterPage("one", "One", static () => new Panel());
        host.RegisterPage("two", "Two", static () => new Panel());
        Ensure(host.Navigate("one"), "Registered shell page could not be opened.");
        Ensure(!host.Navigate("missing"), "Unknown shell page was accepted.");

        using var status = new SasdStatusBar();
        Ensure(status.ShowMessage(new SasdStatusMessage("Error", SasdStatusSeverity.Error, 10)), "High-priority status was rejected.");
        Ensure(!status.ShowMessage(new SasdStatusMessage("Ready", Priority: 0)), "Low-priority status overwrote an active error.");
        status.ClearMessage();
        Ensure(status.ShowMessage(new SasdStatusMessage("Ready", Priority: 0)), "Status did not accept a message after reset.");
    }

    private static void ValidateWindowsShellSafety()
    {
        var shell = new SasdShellService();

        var invalidPath = shell.OpenPath("\0");
        Ensure(!invalidPath.Succeeded && invalidPath.ErrorCode == "PATH_INVALID",
            "Invalid path input escaped the shell safety boundary.");

        var missingPath = shell.OpenPath(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        Ensure(!missingPath.Succeeded && missingPath.ErrorCode == "PATH_NOT_FOUND",
            "Missing path was not reported as a recoverable result.");

        var unsupportedUri = shell.OpenUri(new Uri("file:///C:/Windows/System32/cmd.exe"));
        Ensure(!unsupportedUri.Succeeded && unsupportedUri.ErrorCode == "URI_SCHEME_NOT_ALLOWED",
            "Unsupported URI scheme was accepted.");
    }

    private static async Task EnsureThrowsAsync<TException>(Func<Task> action, string message)
        where TException : Exception
    {
        try
        {
            await action();
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
}
