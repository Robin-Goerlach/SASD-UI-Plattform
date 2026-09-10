using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Demonstrates modal dialogs, transient notifications and a busy overlay.</summary>
internal sealed class FeedbackPage : UserControl
{
    private readonly ISasdDialogService dialogService;
    private readonly SasdNotificationService notificationService;
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly SasdNotificationHost notificationHost = new();
    private readonly SasdBusyOverlay busyOverlay = new();

    public FeedbackPage(
        ISasdDialogService dialogService,
        SasdNotificationService notificationService,
        Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        notificationHost.Bind(notificationService);
        notificationHost.Dock = DockStyle.Top;
        notificationHost.DefaultLifetime = TimeSpan.FromSeconds(6);

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "Dialogs and feedback\r\n\r\n" +
                "Use modal dialogs for decisions or important details, transient notifications for non-critical feedback, " +
                "and the busy overlay when a region must temporarily stop accepting input.",
        };

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 12, 0, 12),
            WrapContents = true,
        };
        actions.Controls.Add(CreateButton("Information dialog", () =>
            dialogService.ShowInformation(this, "This is a normal informational dialog.", "Information")));
        actions.Controls.Add(CreateButton("Warning dialog", () =>
            dialogService.ShowWarning(this, "This is an example warning.", "Warning")));
        actions.Controls.Add(CreateButton("Error details", () =>
            dialogService.ShowErrorDetails(
                this,
                "The example operation failed, but the user-facing message remains concise.",
                "Example failure",
                "Technical details belong in the expandable details area.\r\nNo real exception occurred.")));
        actions.Controls.Add(CreateButton("Confirm", ConfirmExample));
        actions.Controls.Add(CreateButton("Info notification", () => PublishNotification(SasdNotificationSeverity.Information)));
        actions.Controls.Add(CreateButton("Success notification", () => PublishNotification(SasdNotificationSeverity.Success)));
        actions.Controls.Add(CreateButton("Warning notification", () => PublishNotification(SasdNotificationSeverity.Warning)));
        actions.Controls.Add(CreateButton("Error notification", () => PublishNotification(SasdNotificationSeverity.Error)));

        var workButton = new Button
        {
            AutoSize = true,
            Text = "Simulate 1.2 s work",
        };
        workButton.Click += OnSimulateWorkClick;
        actions.Controls.Add(workButton);

        var content = new Panel
        {
            Dock = DockStyle.Fill,
        };
        content.Controls.Add(new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "The simulated work uses Task.Delay only to make the busy state visible. " +
                "A real application would await its own cancellable service operation.",
        });
        content.Controls.Add(busyOverlay);

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 4,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(notificationHost, 0, 1);
        layout.Controls.Add(actions, 0, 2);
        layout.Controls.Add(content, 0, 3);
        Controls.Add(layout);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            notificationHost.Unbind();
        }

        base.Dispose(disposing);
    }

    private void ConfirmExample()
    {
        bool confirmed = dialogService.Confirm(
            this,
            "Confirm this harmless showcase action?",
            "Confirmation example");
        publishStatus(
            confirmed ? "Example action confirmed." : "Example action cancelled.",
            confirmed ? SasdStatusSeverity.Success : SasdStatusSeverity.Information,
            TimeSpan.FromSeconds(4));
    }

    private void PublishNotification(SasdNotificationSeverity severity)
    {
        notificationService.Publish(
            $"This is a {severity.ToString().ToLowerInvariant()} notification from the shared notification service.",
            severity.ToString(),
            severity,
            TimeSpan.FromSeconds(6));
    }

    private async void OnSimulateWorkClick(object? sender, EventArgs e)
    {
        if (sender is Control trigger)
        {
            trigger.Enabled = false;
        }

        busyOverlay.BeginBusy("Running example operation…");
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1200));
            publishStatus("Example operation completed.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
            notificationService.Publish(
                "The simulated operation completed.",
                "Done",
                SasdNotificationSeverity.Success,
                TimeSpan.FromSeconds(5));
        }
        finally
        {
            busyOverlay.EndBusy();
            if (sender is Control trigger)
            {
                trigger.Enabled = true;
            }
        }
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
