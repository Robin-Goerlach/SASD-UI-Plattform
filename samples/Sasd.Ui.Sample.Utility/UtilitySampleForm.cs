using System.Globalization;
using System.Runtime.InteropServices;
using Sasd.Ui.Core;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.State;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.Sample.Utility;

/// <summary>
/// Reference application for common small-tool scenarios: native Windows services,
/// explicit UI-state persistence, tray lifecycle and cancellable background progress.
/// </summary>
/// <remarks>
/// <para>
/// Side-effecting Windows behavior stays behind SASD service boundaries. The sample
/// does not compose arbitrary command lines and treats clipboard/shell failures as
/// recoverable operation results rather than unhandled UI-thread exceptions.
/// </para>
/// <para>
/// UI state remains deliberately small. Paths and arbitrary clipboard text can be
/// sensitive application data, so this sample never persists them in the UI-state file.
/// </para>
/// </remarks>
internal sealed class UtilitySampleForm : SasdForm
{
    private const string SettingsStateKey = "utility:settings";

    private readonly SasdFileDialogService fileDialogs = new();
    private readonly SasdClipboardService clipboard = new();
    private readonly SasdShellService shell = new();
    private readonly SasdStatusBar statusBar = new();
    private readonly SasdTrayService trayService;
    private readonly SasdStateStore stateStore;
    private readonly SasdFormStateService formStateService;
    private readonly TextBox valueTextBox = new();
    private readonly ListBox historyList = new();
    private readonly CheckBox trayCheckBox = new();
    private bool updatingTrayCheckBox;

    /// <summary>Initialises the Utility reference application.</summary>
    public UtilitySampleForm()
    {
        Text = "SASD UI Platform — Utility Reference";
        StateKey = "Sample.Utility.Main";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(760, 560);
        ClientSize = new Size(940, 680);

        stateStore = new SasdStateStore(new SasdStateStoreOptions(
            "SASD-GmbH",
            "Sasd.Ui.Sample.Utility"));
        formStateService = new SasdFormStateService(stateStore);

        trayService = new SasdTrayService("SASD UI Utility Reference");
        trayService.OpenRequested += OnTrayOpenRequested;
        trayService.ExitRequested += OnTrayExitRequested;

        valueTextBox.Dock = DockStyle.Top;
        valueTextBox.AccessibleName = "Selected path, URL or clipboard text";
        valueTextBox.PlaceholderText = "Selected path, URL or text appears here...";

        historyList.Dock = DockStyle.Fill;
        historyList.AccessibleName = "Operation history";

        trayCheckBox.AutoSize = true;
        trayCheckBox.Text = "Show notification-area icon";
        trayCheckBox.AccessibleName = "Show notification-area icon";
        trayCheckBox.CheckedChanged += OnTrayCheckedChanged;

        Controls.Add(CreateContent());
        Controls.Add(statusBar);
        statusBar.Dock = DockStyle.Bottom;
        ShowResult("Utility reference application ready.");
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            trayCheckBox.CheckedChanged -= OnTrayCheckedChanged;
            trayService.OpenRequested -= OnTrayOpenRequested;
            trayService.ExitRequested -= OnTrayExitRequested;
            trayService.Dispose();

            // SasdStateStore.DisposeAsync currently only releases its synchronization
            // primitive. Completing that ValueTask synchronously is therefore safe in
            // a WinForms Dispose path and avoids an async-void disposal event.
            stateStore.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        base.Dispose(disposing);
    }

    private TableLayoutPanel CreateContent()
    {
        var root = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            RowCount = 8,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        root.Controls.Add(new Label
        {
            AutoSize = true,
            MaximumSize = new Size(860, 0),
            Text = "This reference app exercises Windows-service boundaries, explicit UI-state persistence, " +
                   "tray lifetime and a cancellable worker operation. Nothing runs as a hidden background service.",
        }, 0, 0);

        root.Controls.Add(CreateWindowsActions(), 0, 1);
        root.Controls.Add(valueTextBox, 0, 2);
        root.Controls.Add(CreateLifecycleActions(), 0, 3);
        root.Controls.Add(trayCheckBox, 0, 4);
        root.Controls.Add(CreateDiagnosticsBox(), 0, 5);
        root.Controls.Add(new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 12, 0, 4),
            Text = "Operation history",
        }, 0, 6);
        root.Controls.Add(historyList, 0, 7);
        return root;
    }

    private FlowLayoutPanel CreateWindowsActions()
    {
        var actions = CreateActionPanel();
        actions.Controls.Add(CreateButton("Choose file", ChooseFile));
        actions.Controls.Add(CreateButton("Choose folder", ChooseFolder));
        actions.Controls.Add(CreateButton("Copy text", CopyText));
        actions.Controls.Add(CreateButton("Paste text", PasteText));
        actions.Controls.Add(CreateButton("Open path", OpenCurrentPath));
        actions.Controls.Add(CreateButton("Open SASD website", OpenWebsite));
        return actions;
    }

    private FlowLayoutPanel CreateLifecycleActions()
    {
        var actions = CreateActionPanel();
        actions.Controls.Add(CreateAsyncButton("Run progress demo", RunProgressDemoAsync));
        actions.Controls.Add(CreateAsyncButton("Save UI state", SaveUiStateAsync));
        actions.Controls.Add(CreateAsyncButton("Restore UI state", RestoreUiStateAsync));
        actions.Controls.Add(CreateAsyncButton("Reset UI state", ResetUiStateAsync));
        return actions;
    }

    private GroupBox CreateDiagnosticsBox()
    {
        var diagnostics = new TextBox
        {
            AccessibleName = "Runtime diagnostics",
            BackColor = SystemColors.Window,
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Text = string.Join(Environment.NewLine,
            [
                $"Framework: {RuntimeInformation.FrameworkDescription}",
                $"OS: {RuntimeInformation.OSDescription}",
                $"Process architecture: {RuntimeInformation.ProcessArchitecture}",
                $"64-bit process: {Environment.Is64BitProcess}",
                $"UI-state file: {stateStore.StatePath}",
            ]),
        };

        var box = new GroupBox
        {
            Dock = DockStyle.Top,
            Height = 132,
            Padding = new Padding(8),
            Text = "Diagnostics",
        };
        box.Controls.Add(diagnostics);
        return box;
    }

    private void ChooseFile()
    {
        string? path = fileDialogs.OpenFile(this, new SasdFileDialogOptions(
            Title: "Choose a text or Markdown file",
            Filter: "Text and Markdown (*.txt;*.md)|*.txt;*.md|All files (*.*)|*.*"));
        if (path is null)
        {
            ShowResult("File selection cancelled.");
            return;
        }

        valueTextBox.Text = path;
        Record("Selected file", path);
        ShowResult("File path selected.", SasdStatusSeverity.Success);
    }

    private void ChooseFolder()
    {
        string? path = fileDialogs.PickFolder(this, "Choose a folder for the utility example");
        if (path is null)
        {
            ShowResult("Folder selection cancelled.");
            return;
        }

        valueTextBox.Text = path;
        Record("Selected folder", path);
        ShowResult("Folder path selected.", SasdStatusSeverity.Success);
    }

    private void CopyText()
    {
        string text = valueTextBox.Text;
        if (text.Length == 0)
        {
            ShowResult("Enter or paste text before copying it.", SasdStatusSeverity.Warning);
            return;
        }

        UiOperationResult result = clipboard.SetText(text);
        HandleResult(result, "Copied text to clipboard.", "Copy text");
    }

    private void PasteText()
    {
        UiOperationResult<string?> result = clipboard.GetText();
        if (!result.Succeeded)
        {
            HandleFailure(result.UserMessage, result.ErrorCode, "Paste text");
            return;
        }

        valueTextBox.Text = result.Value ?? string.Empty;
        Record("Clipboard text", result.Value is null ? "No text available" : "Text loaded");
        ShowResult(
            result.Value is null ? "Clipboard contains no text." : "Clipboard text loaded.",
            SasdStatusSeverity.Success);
    }

    private void OpenCurrentPath()
    {
        string path = valueTextBox.Text.Trim();
        if (path.Length == 0)
        {
            ShowResult("Choose or enter an existing path first.", SasdStatusSeverity.Warning);
            return;
        }

        UiOperationResult result = shell.OpenPath(path);
        HandleResult(result, "Windows was asked to open the selected path.", "Open path");
    }

    private void OpenWebsite()
    {
        UiOperationResult result = shell.OpenUri(new Uri("https://www.sasd.de/"));
        HandleResult(result, "Windows was asked to open the SASD website.", "Open URI");
    }

    private async Task RunProgressDemoAsync()
    {
        using var dialog = new SasdProgressDialog(
            "Utility progress example",
            "Starting worker operation...",
            allowCancellation: true);

        // Deliberately start the worker before ShowDialog creates the native handle.
        // This exercises the progress dialog's startup buffering contract in a real
        // consumer instead of only in the dedicated smoke test.
        Task<DialogResult> worker = RunProgressWorkerAsync(dialog);
        dialog.ShowDialog(this);
        DialogResult result = await worker;

        if (result == DialogResult.Cancel)
        {
            Record("Progress demo", "Cancelled");
            ShowResult("Progress operation cancelled.", SasdStatusSeverity.Warning);
        }
        else
        {
            Record("Progress demo", "Completed");
            ShowResult("Progress operation completed.", SasdStatusSeverity.Success);
        }
    }

    private static Task<DialogResult> RunProgressWorkerAsync(SasdProgressDialog dialog) => Task.Run(async () =>
    {
        try
        {
            for (int percent = 0; percent <= 100; percent += 5)
            {
                dialog.CancellationToken.ThrowIfCancellationRequested();
                dialog.Report(new SasdProgressUpdate($"Processing step {percent / 5 + 1} of 21...", percent));
                await Task.Delay(80, dialog.CancellationToken).ConfigureAwait(false);
            }

            dialog.Complete(DialogResult.OK);
            return DialogResult.OK;
        }
        catch (OperationCanceledException)
        {
            dialog.Complete(DialogResult.Cancel);
            return DialogResult.Cancel;
        }
    });

    private async Task SaveUiStateAsync()
    {
        await formStateService.SaveAsync(this);
        await stateStore.SaveAsync(SettingsStateKey, new UtilityUiSettings(trayService.IsVisible));
        Record("UI state", "Saved window placement and tray visibility");
        ShowResult("UI state saved.", SasdStatusSeverity.Success);
    }

    private async Task RestoreUiStateAsync()
    {
        await formStateService.RestoreAsync(this);
        UtilityUiSettings? settings = await stateStore.LoadAsync<UtilityUiSettings>(SettingsStateKey);
        ApplyTrayVisibility(settings?.TrayVisible ?? false, announce: false);
        Record("UI state", settings is null ? "No saved settings; defaults applied" : "Restored");
        ShowResult(settings is null ? "No saved UI settings were found." : "UI state restored.",
            settings is null ? SasdStatusSeverity.Information : SasdStatusSeverity.Success);
    }

    private async Task ResetUiStateAsync()
    {
        await stateStore.ResetAsync();
        ApplyTrayVisibility(false, announce: false);
        Record("UI state", "Reset");
        ShowResult("Persisted UI state reset. Business/user content was not touched.", SasdStatusSeverity.Success);
    }

    private void OnTrayCheckedChanged(object? sender, EventArgs e)
    {
        if (!updatingTrayCheckBox)
        {
            ApplyTrayVisibility(trayCheckBox.Checked, announce: true);
        }
    }

    private void ApplyTrayVisibility(bool visible, bool announce)
    {
        if (visible)
        {
            trayService.Show();
        }
        else
        {
            trayService.Hide();
        }

        updatingTrayCheckBox = true;
        try
        {
            trayCheckBox.Checked = visible;
        }
        finally
        {
            updatingTrayCheckBox = false;
        }

        if (announce)
        {
            Record("Tray icon", visible ? "Shown" : "Hidden");
            ShowResult(visible ? "Tray icon shown." : "Tray icon hidden.", SasdStatusSeverity.Success);
        }
    }

    private void OnTrayOpenRequested(object? sender, SasdTrayRequestEventArgs e)
    {
        Show();
        if (WindowState == FormWindowState.Minimized)
        {
            WindowState = FormWindowState.Normal;
        }

        Activate();
        Record("Tray request", "Open application");
    }

    private void OnTrayExitRequested(object? sender, SasdTrayRequestEventArgs e)
    {
        // The tray service itself never terminates a process. The application owns
        // the policy and explicitly maps its Exit request to normal Form.Close().
        Record("Tray request", "Exit application");
        Close();
    }

    private async Task ExecuteUiActionAsync(string operation, Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            Record(operation, $"Unexpected failure — {exception.GetType().Name}");
            ShowResult("The operation could not be completed.", SasdStatusSeverity.Error, priority: 10);
        }
    }

    private void HandleResult(UiOperationResult result, string successMessage, string operation)
    {
        if (!result.Succeeded)
        {
            HandleFailure(result.UserMessage, result.ErrorCode, operation);
            return;
        }

        Record(operation, "Succeeded");
        ShowResult(successMessage, SasdStatusSeverity.Success);
    }

    private void HandleFailure(string? userMessage, string? errorCode, string operation)
    {
        string message = userMessage ?? "The operation could not be completed.";
        Record(operation, $"Failed [{errorCode ?? "UNKNOWN"}] — {message}");
        ShowResult(message, SasdStatusSeverity.Error, priority: 10);
    }

    private void Record(string operation, string detail)
    {
        string time = DateTimeOffset.Now.ToString("T", CultureInfo.CurrentCulture);
        historyList.Items.Insert(0, $"{time}  {operation}: {detail}");
    }

    private void ShowResult(
        string message,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        int priority = 0) =>
        statusBar.ShowMessage(new SasdStatusMessage(message, severity, priority, TimeSpan.FromSeconds(5)));

    private static FlowLayoutPanel CreateActionPanel() => new()
    {
        AutoSize = true,
        Dock = DockStyle.Top,
        FlowDirection = FlowDirection.LeftToRight,
        Padding = new Padding(0, 12, 0, 8),
        WrapContents = true,
    };

    private static Button CreateButton(string text, Action action)
    {
        var button = CreateButtonBase(text);
        button.Click += (_, _) => action();
        return button;
    }

    private Button CreateAsyncButton(string text, Func<Task> action)
    {
        var button = CreateButtonBase(text);
        button.Click += async (_, _) => await ExecuteUiActionAsync(text, action);
        return button;
    }

    private static Button CreateButtonBase(string text) => new()
    {
        AutoSize = true,
        Margin = new Padding(0, 0, 8, 8),
        Text = text,
    };
}
