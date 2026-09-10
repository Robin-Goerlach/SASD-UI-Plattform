using Sasd.Ui.Core;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Theming;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Demonstrates constrained file, clipboard, shell, tray, icon and drag-and-drop services.</summary>
internal sealed class WindowsPage : UserControl
{
    private readonly ISasdFileDialogService fileDialogService;
    private readonly ISasdClipboardService clipboardService;
    private readonly SasdDragDropService dragDropService;
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly SasdShellService shellService = new();
    private readonly SasdTrayService trayService = new("SASD UI Platform showcase");
    private readonly SasdSystemIconService iconService = new();
    private readonly ListBox resultsList = new();
    private readonly SasdFileDropPolicy dropPolicy = new(
        AllowedExtensions: [".txt", ".md", ".json", ".log"],
        AllowDirectories: false,
        MaxItems: 10,
        MaxFileBytes: 5L * 1024L * 1024L);

    public WindowsPage(
        ISasdFileDialogService fileDialogService,
        ISasdClipboardService clipboardService,
        SasdDragDropService dragDropService,
        Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
        this.clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        this.dragDropService = dragDropService ?? throw new ArgumentNullException(nameof(dragDropService));
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        trayService.OpenRequested += OnTrayOpenRequested;
        trayService.ExitRequested += OnTrayExitRequested;

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(940, 0),
            Text =
                "Windows integration\r\n\r\n" +
                "The platform wraps common desktop operations behind small service boundaries. " +
                "The notification-area icon is opt-in, semantic icons remain service-owned, and drag-and-drop validates metadata before paths reach application logic; extensions are still not treated as trusted content.",
        };

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 10, 0, 10),
            WrapContents = true,
        };
        actions.Controls.Add(CreateButton("Open file dialog", OpenFile));
        actions.Controls.Add(CreateButton("Pick folder", PickFolder));
        actions.Controls.Add(CreateButton("Copy sample text", CopySampleText));
        actions.Controls.Add(CreateButton("Read clipboard", ReadClipboard));
        actions.Controls.Add(CreateButton("Open repository", OpenRepository));
        actions.Controls.Add(CreateButton("Show tray icon", ShowTrayIcon));
        actions.Controls.Add(CreateButton("Hide tray icon", HideTrayIcon));

        Control iconPreview = CreateIconPreview();

        var dropZone = new Panel
        {
            AllowDrop = true,
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 140,
            Padding = new Padding(12),
        };
        dropZone.Controls.Add(new Label
        {
            AccessibleName = "Validated file drop area",
            AutoSize = false,
            Dock = DockStyle.Fill,
            Text = "Drop up to 10 .txt, .md, .json or .log files here\r\nMaximum 5 MiB each — directories are rejected",
            TextAlign = ContentAlignment.MiddleCenter,
        });
        dropZone.DragEnter += (_, args) =>
        {
            args.Effect = dragDropService.CanAccept(args.Data, dropPolicy)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        };
        dropZone.DragDrop += (_, args) => ShowDropResult(args.Data);

        resultsList.Dock = DockStyle.Fill;
        resultsList.HorizontalScrollbar = true;

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 5,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(actions, 0, 1);
        layout.Controls.Add(iconPreview, 0, 2);
        layout.Controls.Add(dropZone, 0, 3);
        layout.Controls.Add(resultsList, 0, 4);
        Controls.Add(layout);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            trayService.OpenRequested -= OnTrayOpenRequested;
            trayService.ExitRequested -= OnTrayExitRequested;
            trayService.Dispose();
        }

        // Dispose child controls before releasing service-owned icon images referenced by
        // their PictureBoxes. PictureBox does not own images supplied through Image.
        base.Dispose(disposing);

        if (disposing)
        {
            iconService.Dispose();
        }
    }

    private Control CreateIconPreview()
    {
        var container = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 0, 0, 10),
            WrapContents = true,
        };
        container.Controls.Add(new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 12, 12, 0),
            Text = "Semantic icons:",
        });

        foreach (SasdSemanticIcon role in Enum.GetValues<SasdSemanticIcon>())
        {
            Image image = iconService.GetImage(role);
            var picture = new PictureBox
            {
                AccessibleName = $"{role} semantic icon",
                Height = 36,
                Image = image,
                Margin = new Padding(4, 0, 4, 0),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Width = 36,
            };
            var item = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 14, 0),
                WrapContents = false,
            };
            item.Controls.Add(picture);
            item.Controls.Add(new Label
            {
                AutoSize = true,
                Margin = new Padding(2, 10, 0, 0),
                Text = role.ToString(),
            });
            container.Controls.Add(item);
        }

        return container;
    }

    private void OpenFile()
    {
        string? path = fileDialogService.OpenFile(
            this,
            new SasdFileDialogOptions(
                Title: "Select a text-like file for the showcase",
                Filter: "Text-like files (*.txt;*.md;*.json;*.log)|*.txt;*.md;*.json;*.log|All files (*.*)|*.*"));

        resultsList.Items.Insert(0, path is null ? "Open-file dialog cancelled." : $"Selected file: {path}");
    }

    private void PickFolder()
    {
        string? path = fileDialogService.PickFolder(this, "Select an example folder");
        resultsList.Items.Insert(0, path is null ? "Folder dialog cancelled." : $"Selected folder: {path}");
    }

    private void CopySampleText()
    {
        UiOperationResult result = clipboardService.SetText("SASD UI Platform showcase clipboard test");
        ShowOperationResult(result, "Sample text copied to the clipboard.");
    }

    private void ReadClipboard()
    {
        UiOperationResult<string?> result = clipboardService.GetText();
        if (!result.Succeeded)
        {
            resultsList.Items.Insert(0, $"Clipboard read failed: {result.UserMessage ?? result.ErrorCode ?? "Unknown error"}");
            return;
        }

        resultsList.Items.Insert(0, result.Value is null
            ? "Clipboard currently contains no text."
            : $"Clipboard text: {result.Value}");
    }

    private void OpenRepository()
    {
        UiOperationResult result = shellService.OpenUri(new Uri("https://github.com/Robin-Goerlach/SASD-UI-Plattform"));
        ShowOperationResult(result, "Repository link sent to the Windows shell.");
    }

    private void ShowTrayIcon()
    {
        trayService.Show();
        resultsList.Items.Insert(0, "Notification-area icon shown. Double-click it or use its context menu to test events.");
        publishStatus("Tray icon shown explicitly.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    private void HideTrayIcon()
    {
        trayService.Hide();
        resultsList.Items.Insert(0, "Notification-area icon hidden.");
        publishStatus("Tray icon hidden.", SasdStatusSeverity.Information, TimeSpan.FromSeconds(3));
    }

    private void OnTrayOpenRequested(object? sender, SasdTrayRequestEventArgs e)
    {
        resultsList.Items.Insert(0, $"Tray Open requested (mouse button: {e.Button}).");
        FindForm()?.Activate();
        publishStatus("Tray Open event received.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    private void OnTrayExitRequested(object? sender, SasdTrayRequestEventArgs e)
    {
        // The tray service reports intent only. The showcase deliberately remains running so
        // users can observe the event without the demonstration unexpectedly closing itself.
        resultsList.Items.Insert(0, "Tray Exit requested; showcase remains open by design.");
        publishStatus("Tray Exit event received; no process exit performed.", SasdStatusSeverity.Information, TimeSpan.FromSeconds(5));
    }

    private void ShowDropResult(IDataObject? data)
    {
        SasdFileDropResult result = dragDropService.ReadFiles(data, dropPolicy);
        resultsList.Items.Clear();

        foreach (SasdFileDropItem item in result.Accepted)
        {
            resultsList.Items.Add($"ACCEPTED  {item.Path}  ({item.FileSizeBytes ?? 0:N0} bytes)");
        }

        foreach (SasdFileDropRejection rejection in result.Rejected)
        {
            resultsList.Items.Add($"REJECTED  [{rejection.ReasonCode}]  {rejection.Path ?? "<payload>"} — {rejection.Message}");
        }

        publishStatus(
            result.HasAcceptedItems
                ? $"Accepted {result.Accepted.Count} dropped file(s)."
                : "No dropped item passed the configured policy.",
            result.HasAcceptedItems ? SasdStatusSeverity.Success : SasdStatusSeverity.Warning,
            TimeSpan.FromSeconds(4));
    }

    private void ShowOperationResult(UiOperationResult result, string successMessage)
    {
        string message = result.Succeeded
            ? successMessage
            : result.UserMessage ?? result.ErrorCode ?? "The operation failed.";
        resultsList.Items.Insert(0, message);
        publishStatus(
            message,
            result.Succeeded ? SasdStatusSeverity.Success : SasdStatusSeverity.Warning,
            TimeSpan.FromSeconds(4));
    }

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
