using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.ComponentGallery;

/// <summary>
/// Integration-oriented entry point for the Component Gallery.
/// </summary>
/// <remarks>
/// <para>
/// The existing <see cref="MainForm"/> remains the detailed catalog of individual controls.
/// This window demonstrates how the R1 shell services work together in a normal application:
/// global commands, keyboard shortcuts, navigation, status publication, notifications and
/// constrained file drag-and-drop.
/// </para>
/// <para>
/// The sample intentionally uses plain, readable event wiring. It is an executable reference,
/// not a framework inside the framework, and therefore avoids speculative abstraction or
/// performance tuning that would make the example harder to learn from.
/// </para>
/// </remarks>
internal sealed class GalleryShellForm : SasdShellForm
{
    private readonly SasdNotificationService notificationService = new();
    private readonly SasdDragDropService dragDropService = new();
    private readonly SasdFileDropPolicy dropPolicy = new(
        AllowedExtensions: [".txt", ".md", ".json"],
        AllowDirectories: false,
        MaxItems: 10,
        MaxFileBytes: 5L * 1024L * 1024L);

    /// <summary>Creates the integration gallery and registers its application-owned pages/commands.</summary>
    public GalleryShellForm()
    {
        Text = "SASD UI Platform — R1 Integration Gallery";
        StateKey = "ComponentGallery.IntegrationShell";
        MinimumSize = new Size(900, 620);
        ClientSize = new Size(1180, 760);
        StartPosition = FormStartPosition.CenterScreen;

        NavigationHost.CachePages = true;
        NavigationHost.NavigationWidth = 210;
        NavigationHost.RegisterPage("overview", "Overview", CreateOverviewPage);
        NavigationHost.RegisterPage("feedback", "Feedback", CreateFeedbackPage);
        NavigationHost.RegisterPage("windows", "Windows integration", CreateWindowsIntegrationPage);
        NavigationHost.Navigated += OnNavigated;

        RegisterGalleryCommands();
        NavigationHost.Navigate("overview");
        PublishStatus("R1 integration gallery ready.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    private void RegisterGalleryCommands()
    {
        RegisterCommand(new SasdCommand(
            "gallery.open-details",
            "Detailed gallery",
            _ =>
            {
                var detailGallery = new MainForm();
                detailGallery.Show(this);
                PublishStatus("Detailed component gallery opened.");
                return Task.CompletedTask;
            },
            "Open the existing detailed catalog of individual R1 components.",
            Keys.Control | Keys.D));

        RegisterCommand(new SasdCommand(
            "gallery.success-notification",
            "Success notice",
            _ =>
            {
                NavigationHost.Navigate("feedback");
                notificationService.Publish(
                    "The sample operation completed successfully.",
                    "Operation complete",
                    SasdNotificationSeverity.Success,
                    TimeSpan.FromSeconds(6));
                return Task.CompletedTask;
            },
            "Show a transient non-modal success notification.",
            Keys.Control | Keys.Shift | Keys.S));

        RegisterCommand(new SasdCommand(
            "gallery.warning-notification",
            "Warning notice",
            _ =>
            {
                NavigationHost.Navigate("feedback");
                notificationService.Publish(
                    "This warning remains non-modal because no immediate decision is required.",
                    "Attention",
                    SasdNotificationSeverity.Warning,
                    TimeSpan.FromSeconds(8));
                return Task.CompletedTask;
            },
            "Show a transient warning without replacing a required confirmation dialog.",
            Keys.Control | Keys.Shift | Keys.W));

        RegisterCommand(new SasdCommand(
            "gallery.windows-page",
            "Drop demo",
            _ =>
            {
                NavigationHost.Navigate("windows");
                return Task.CompletedTask;
            },
            "Open the safe drag-and-drop demonstration.",
            Keys.Control | Keys.Shift | Keys.D));
    }

    private static Control CreateOverviewPage()
    {
        var panel = CreatePagePanel();
        panel.Controls.Add(CreateTextLabel(
            "R1 integration shell\r\n\r\n" +
            "This window is composed from SasdShellForm rather than reproducing application plumbing. " +
            "The toolbar and keyboard shortcuts share one command registry/runner; navigation is application-owned; " +
            "status messages are published through ISasdStatusService; notifications use a presentation-neutral service; " +
            "and Windows drag-and-drop is validated before application code receives paths.\r\n\r\n" +
            "Useful shortcuts:\r\n" +
            "• Ctrl+D — open the detailed component gallery\r\n" +
            "• Ctrl+Shift+S — success notification\r\n" +
            "• Ctrl+Shift+W — warning notification\r\n" +
            "• Ctrl+Shift+D — safe drag-and-drop page\r\n\r\n" +
            "The sample deliberately keeps business policy out of the UI platform. Applications still decide what pages, commands, " +
            "files and operations mean."));
        return panel;
    }

    private Control CreateFeedbackPage()
    {
        var panel = CreatePagePanel();
        var host = new SasdNotificationHost
        {
            Dock = DockStyle.Top,
            DefaultLifetime = TimeSpan.FromSeconds(6),
            Margin = new Padding(0, 12, 0, 12),
        };
        host.Bind(notificationService);
        host.NotificationShown += (_, args) =>
            PublishStatus($"Notification shown: {args.Notification.Severity}", SasdStatusSeverity.Information);
        host.NotificationDismissed += (_, _) => PublishStatus("Notification dismissed.");

        var instructions = CreateTextLabel(
            "Non-modal feedback\r\n\r\n" +
            "Use the toolbar or keyboard shortcuts to publish notifications. R1 intentionally displays one notification at a time. " +
            "Critical errors, security warnings and decisions still belong on persistent or modal surfaces; this host is for transient feedback.");
        instructions.Dock = DockStyle.Top;

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 12, 0, 0),
        };
        actions.Controls.Add(CreateButton("Information", () =>
            notificationService.Publish("A normal informational message.", "Information", lifetime: TimeSpan.FromSeconds(6))));
        actions.Controls.Add(CreateButton("Success", () =>
            notificationService.Publish("Changes were saved.", "Saved", SasdNotificationSeverity.Success, TimeSpan.FromSeconds(6))));
        actions.Controls.Add(CreateButton("Warning", () =>
            notificationService.Publish("Review the highlighted condition when convenient.", "Warning", SasdNotificationSeverity.Warning, TimeSpan.FromSeconds(8))));
        actions.Controls.Add(CreateButton("Dismiss", host.Dismiss));

        panel.Controls.Add(actions);
        panel.Controls.Add(host);
        panel.Controls.Add(instructions);
        return panel;
    }

    private Control CreateWindowsIntegrationPage()
    {
        var page = CreatePagePanel();
        var results = new ListBox
        {
            Dock = DockStyle.Fill,
            HorizontalScrollbar = true,
            IntegralHeight = false,
        };

        var dropZone = new Panel
        {
            AllowDrop = true,
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 150,
            Padding = new Padding(16),
        };
        dropZone.Controls.Add(new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "Drop up to 10 .txt, .md or .json files here\r\nMaximum 5 MiB per file — directories are rejected",
            AccessibleName = "Safe file drop area",
        });
        dropZone.DragEnter += (_, args) =>
        {
            args.Effect = dragDropService.CanAccept(args.Data, dropPolicy)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        };
        dropZone.DragDrop += (_, args) => ShowDropResult(args.Data, results);

        var heading = CreateTextLabel(
            "Validated Windows drag-and-drop\r\n\r\n" +
            "The UI platform validates metadata and policy before paths reach application code. " +
            "An allowed extension is still not proof that file content is trusted; a real importer must parse and validate the content for its domain.");
        heading.Dock = DockStyle.Top;

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(dropZone, 0, 1);
        layout.Controls.Add(results, 0, 2);
        page.Controls.Add(layout);
        return page;
    }

    private void ShowDropResult(IDataObject? data, ListBox results)
    {
        SasdFileDropResult result = dragDropService.ReadFiles(data, dropPolicy);
        results.Items.Clear();

        foreach (SasdFileDropItem item in result.Accepted)
        {
            results.Items.Add($"ACCEPTED  {item.Path}  ({item.FileSizeBytes ?? 0:N0} bytes)");
        }

        foreach (SasdFileDropRejection rejection in result.Rejected)
        {
            results.Items.Add($"REJECTED  [{rejection.ReasonCode}]  {rejection.Path ?? "<payload>"} — {rejection.Message}");
        }

        if (result.HasAcceptedItems)
        {
            PublishStatus(
                $"Accepted {result.Accepted.Count} file(s); rejected {result.Rejected.Count} item(s).",
                result.Rejected.Count == 0 ? SasdStatusSeverity.Success : SasdStatusSeverity.Warning,
                TimeSpan.FromSeconds(5));
        }
        else
        {
            PublishStatus("No dropped item passed the configured policy.", SasdStatusSeverity.Warning, TimeSpan.FromSeconds(5));
        }
    }

    private void OnNavigated(object? sender, SasdNavigationEventArgs e) =>
        PublishStatus($"Page: {e.PageId}", SasdStatusSeverity.Information, TimeSpan.FromSeconds(2));

    private void PublishStatus(
        string text,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        TimeSpan? lifetime = null,
        int priority = 0) =>
        StatusService.Publish(new SasdStatusMessage(text, severity, priority, lifetime));

    private static Panel CreatePagePanel() => new()
    {
        Dock = DockStyle.Fill,
        Padding = new Padding(20),
    };

    private static Label CreateTextLabel(string text) => new()
    {
        AutoSize = true,
        MaximumSize = new Size(820, 0),
        Text = text,
    };

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
