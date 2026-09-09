using Sasd.Ui.Core;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.Sample.Utility;

/// <summary>
/// Small reference application for common Windows integration services.
/// </summary>
/// <remarks>
/// The sample keeps every potentially side-effecting Windows action behind the reusable SASD service
/// boundaries. It does not execute arbitrary command lines and it shows recoverable service failures as
/// normal application feedback rather than allowing them to escape through the UI event loop.
/// </remarks>
internal sealed class UtilitySampleForm : SasdForm
{
    private readonly SasdFileDialogService fileDialogs = new();
    private readonly SasdClipboardService clipboard = new();
    private readonly SasdShellService shell = new();
    private readonly SasdStatusBar statusBar = new();
    private readonly TextBox valueTextBox = new();
    private readonly ListBox historyList = new();

    /// <summary>Initialises the utility reference window.</summary>
    public UtilitySampleForm()
    {
        Text = "SASD UI Platform — Utility Reference";
        StateKey = "Sample.Utility.Main";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(720, 480);
        ClientSize = new Size(860, 560);

        valueTextBox.Dock = DockStyle.Top;
        valueTextBox.AccessibleName = "Selected path, URL or clipboard text";
        valueTextBox.PlaceholderText = "Selected path, URL or text appears here...";

        historyList.Dock = DockStyle.Fill;
        historyList.AccessibleName = "Operation history";

        Controls.Add(CreateContent());
        Controls.Add(statusBar);
        statusBar.Dock = DockStyle.Bottom;
        ShowResult("Utility reference application ready.");
    }

    private Control CreateContent()
    {
        var root = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            RowCount = 4,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        root.Controls.Add(new Label
        {
            AutoSize = true,
            MaximumSize = new Size(780, 0),
            Text = "This reference app exercises the SASD Windows-service boundaries. File/folder dialogs return paths only; " +
                   "clipboard failures are recoverable; shell access is constrained to existing paths and HTTP/HTTPS/mailto URIs.",
        }, 0, 0);

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 12, 0, 8),
            WrapContents = true,
        };
        actions.Controls.Add(CreateButton("Choose file", ChooseFile));
        actions.Controls.Add(CreateButton("Choose folder", ChooseFolder));
        actions.Controls.Add(CreateButton("Copy text", CopyText));
        actions.Controls.Add(CreateButton("Paste text", PasteText));
        actions.Controls.Add(CreateButton("Open path", OpenCurrentPath));
        actions.Controls.Add(CreateButton("Open SASD website", OpenWebsite));
        root.Controls.Add(actions, 0, 1);
        root.Controls.Add(valueTextBox, 0, 2);
        root.Controls.Add(historyList, 0, 3);
        return root;
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
        UiOperationResult result = clipboard.SetText(valueTextBox.Text);
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
        Record("Clipboard text", valueTextBox.Text);
        ShowResult(result.Value is null ? "Clipboard contains no text." : "Clipboard text loaded.",
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

    private void Record(string operation, string detail) =>
        historyList.Items.Insert(0, $"{DateTime.Now:T}  {operation}: {detail}");

    private void ShowResult(
        string message,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        int priority = 0) =>
        statusBar.ShowMessage(new SasdStatusMessage(message, severity, priority, TimeSpan.FromSeconds(5)));

    private static Button CreateButton(string text, Action action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 8, 8),
            Text = text,
        };
        button.Click += (_, _) => action();
        return button;
    }
}
