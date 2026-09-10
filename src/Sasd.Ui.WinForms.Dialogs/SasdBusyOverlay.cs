using System.ComponentModel;

namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Blocks interaction with a region while clearly communicating ongoing work.</summary>
public class SasdBusyOverlay : UserControl
{
    private readonly Label messageLabel;
    private readonly ProgressBar progressBar;

    /// <summary>Initialises the busy overlay.</summary>
    public SasdBusyOverlay()
    {
        Dock = DockStyle.Fill;
        BackColor = SystemColors.Control;
        TabStop = false;
        Visible = false;
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Busy state";

        var content = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Anchor = AnchorStyles.None,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(24),
        };

        messageLabel = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.None,
            Text = "Working…",
            TextAlign = ContentAlignment.MiddleCenter,
        };

        progressBar = new ProgressBar
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            MarqueeAnimationSpeed = 30,
            MinimumSize = new Size(240, 20),
            Style = ProgressBarStyle.Marquee,
        };

        content.Controls.Add(messageLabel, 0, 0);
        content.Controls.Add(progressBar, 0, 1);

        var host = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1,
        };
        host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        host.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        host.Controls.Add(content, 0, 0);
        Controls.Add(host);
    }

    /// <summary>Gets or sets the message displayed while busy.</summary>
    [Category("SASD")]
    [DefaultValue("Working…")]
    public string Message
    {
        get => messageLabel.Text;
        set
        {
            messageLabel.Text = value ?? string.Empty;
            if (Visible)
            {
                AccessibleDescription = string.IsNullOrWhiteSpace(messageLabel.Text)
                    ? "Application is busy."
                    : messageLabel.Text.Trim();
            }
        }
    }

    /// <summary>Shows the overlay and brings it in front of sibling controls.</summary>
    public void BeginBusy(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            Message = message;
        }

        Visible = true;
        BringToFront();
        UseWaitCursor = true;
        AccessibleDescription = string.IsNullOrWhiteSpace(Message)
            ? "Application is busy."
            : Message.Trim();
    }

    /// <summary>Hides the overlay and restores the normal cursor.</summary>
    public void EndBusy()
    {
        UseWaitCursor = false;
        Visible = false;
        AccessibleDescription = null;
    }
}
