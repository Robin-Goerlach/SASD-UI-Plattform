using System.Text;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Theming;
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
            await ValidateCommandBarAsync();
            await ValidateCsvAndGridStateAsync();
            await ValidateFormsAsync();
            await ValidateUiDispatcherAsync();
            ValidateFilterBar();
            ValidateDialogForm();
            ValidateBreadcrumb();
            ValidateDocumentTabs();
            ValidateListAndTreeDefaults();
            ValidateIcons();
            ValidateNotifications();
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

    private static async Task ValidateCommandBarAsync()
    {
        using var commandBar = new SasdCommandBar();
        var completion = new TaskCompletionSource<SasdCommandBarCommandCompletedEventArgs>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        int executions = 0;
        var command = new SasdCommand(
            "smoke.command-bar",
            "Refresh",
            _ =>
            {
                executions++;
                return Task.CompletedTask;
            },
            "Refresh the sample.");

        commandBar.CommandCompleted += (_, args) => completion.TrySetResult(args);
        ToolStripButton button = commandBar.AddCommand(command);

        Ensure(commandBar.CommandCount == 1, "Command bar did not retain the added command.");
        Ensure(button.Text == "Refresh" && button.Enabled, "Command state was not projected to the command bar button.");

        command.Enabled = false;
        Ensure(!button.Enabled, "Command bar did not track disabled command state.");
        command.Enabled = true;

        button.PerformClick();
        SasdCommandBarCommandCompletedEventArgs completed = await completion.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Ensure(completed.Result.Succeeded && executions == 1, "Command bar did not execute the command successfully.");
        Ensure(ReferenceEquals(completed.Command, command), "Command bar completion reported the wrong command instance.");

        EnsureThrows<ArgumentException>(
            () => commandBar.AddCommand(command),
            "Command bar accepted a duplicate command id.");

        Ensure(commandBar.RemoveCommand("smoke.command-bar"), "Command bar could not remove an existing command.");
        Ensure(button.IsDisposed && commandBar.CommandCount == 0, "Removed command-bar resources were not disposed.");
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

        var query = new SasdDataQuery(
            0,
            100,
            [SasdSortDescriptor.Create("Name", SasdSortDirection.Ascending)],
            "example",
            [SasdFilterDescriptor.Create("Status", SasdFilterOperator.Equals, "Active")]);
        Ensure(ReferenceEquals(query, query.Validate()), "Valid data query was unexpectedly replaced.");
        Ensure(query.Filters is { Count: 1 }, "Filter descriptor was not retained by the data query.");
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

    private static void ValidateFilterBar()
    {
        using var filterBar = new SasdFilterBar();
        int changes = 0;
        filterBar.FiltersChanged += (_, _) => changes++;

        filterBar.AddOrUpdateFilter("status", "Status", "Active");
        filterBar.AddOrUpdateFilter("region", "Region", "EU");
        filterBar.AddOrUpdateFilter("STATUS", "Status", "Inactive");

        Ensure(filterBar.FilterCount == 2, "Updating an existing filter created a duplicate.");
        Ensure(filterBar.ActiveFilters[0].Value == "Inactive", "Existing filter was not replaced case-insensitively.");
        Ensure(filterBar.RemoveFilter("region"), "Existing filter could not be removed.");
        Ensure(!filterBar.RemoveFilter("missing"), "Missing filter was reported as removed.");
        filterBar.ClearFilters();
        Ensure(filterBar.FilterCount == 0, "Filter bar did not clear all filters.");
        Ensure(changes == 5, "Filter change notifications were not raised predictably.");
    }

    private static void ValidateDialogForm()
    {
        using var dialog = new SasdDialogForm();
        Ensure(dialog.AutoScaleMode == AutoScaleMode.Dpi, "Dialog form must retain DPI scaling.");
        Ensure(dialog.KeyPreview, "Dialog form must retain keyboard preview.");
        Ensure(!dialog.ShowInTaskbar, "Dialog form unexpectedly creates a taskbar entry.");
        Ensure(!dialog.MinimizeBox && !dialog.MaximizeBox, "Dialog form exposes inappropriate window commands.");
    }

    private static void ValidateBreadcrumb()
    {
        using var breadcrumb = new SasdBreadcrumb();
        breadcrumb.SetPath([
            SasdBreadcrumbItem.Create("customers", "Customers"),
            SasdBreadcrumbItem.Create("customer-42", "Example AG"),
        ]);

        Ensure(breadcrumb.ItemCount == 2, "Breadcrumb did not retain the complete path.");
        Ensure(breadcrumb.Items[1].Id == "customer-42", "Breadcrumb path order changed unexpectedly.");
        breadcrumb.ClearPath();
        Ensure(breadcrumb.ItemCount == 0, "Breadcrumb path did not clear.");
    }

    private static void ValidateDocumentTabs()
    {
        using var tabs = new SasdDocumentTabs();
        int opened = 0;
        int closed = 0;
        tabs.DocumentOpened += (_, _) => opened++;
        tabs.DocumentClosed += (_, _) => closed++;

        Control first = tabs.OpenOrSelect("one", "First", static () => new Panel());
        Control same = tabs.OpenOrSelect("ONE", "First renamed", static () => new Label());
        tabs.OpenOrSelect("two", "Second", static () => new Panel());

        Ensure(ReferenceEquals(first, same), "Opening the same document id created duplicate content.");
        Ensure(tabs.DocumentCount == 2 && opened == 2, "Document tab count or open notifications are incorrect.");
        Ensure(tabs.SelectDocument("one"), "Existing document could not be selected.");
        Ensure(string.Equals(tabs.SelectedDocumentId, "one", StringComparison.OrdinalIgnoreCase), "Selected document id is incorrect.");
        Ensure(tabs.SetDocumentTitle("two", "Second renamed"), "Document title could not be changed.");
        Ensure(tabs.CloseDocument("one"), "Existing document could not be closed.");
        Ensure(first.IsDisposed, "Closed document content was not disposed.");
        Ensure(closed == 1, "Document close notification was not raised once.");
        tabs.CloseAllDocuments();
        Ensure(tabs.DocumentCount == 0, "Document tabs did not close all documents.");
    }

    private static void ValidateListAndTreeDefaults()
    {
        using var list = new SasdListView();
        Ensure(list.View == View.Details && list.FullRowSelect, "ListView business defaults are incomplete.");
        Ensure(!list.HideSelection && !list.MultiSelect, "ListView selection defaults are unsafe or inconsistent.");

        using var tree = new SasdTreeView();
        Ensure(!tree.HideSelection && tree.ShowNodeToolTips, "TreeView selection or tooltip defaults are incorrect.");
    }

    private static void ValidateIcons()
    {
        var icons = new SasdSystemIconService();
        Image information = icons.GetImage(SasdSemanticIcon.Information);
        Image cachedInformation = icons.GetImage(SasdSemanticIcon.Information);

        Ensure(ReferenceEquals(information, cachedInformation), "Icon service did not reuse its service-owned cache.");
        Ensure(information.Width > 0 && information.Height > 0, "Icon service returned an invalid image.");

        icons.Dispose();
        EnsureThrows<ObjectDisposedException>(
            () => icons.GetImage(SasdSemanticIcon.Warning),
            "Disposed icon service accepted new requests.");
    }

    private static void ValidateNotifications()
    {
        var service = new SasdNotificationService();
        SasdNotification? received = null;
        int publications = 0;
        service.NotificationPublished += (_, args) =>
        {
            publications++;
            received = args.Notification;
        };

        service.Publish(
            "Saved successfully.",
            "Save",
            SasdNotificationSeverity.Success,
            TimeSpan.FromSeconds(5));

        Ensure(publications == 1 && received is not null, "Notification service did not publish exactly once.");
        Ensure(received.Severity == SasdNotificationSeverity.Success, "Notification severity changed during publication.");
        Ensure(received.Lifetime == TimeSpan.FromSeconds(5), "Notification lifetime changed during publication.");

        EnsureThrows<ArgumentException>(
            () => service.Publish(new SasdNotification(" ")),
            "Notification service accepted an empty message.");
        EnsureThrows<ArgumentOutOfRangeException>(
            () => service.Publish(new SasdNotification("Invalid lifetime", Lifetime: TimeSpan.Zero)),
            "Notification service accepted a non-positive lifetime.");
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
