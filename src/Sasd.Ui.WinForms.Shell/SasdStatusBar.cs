using System.ComponentModel;

namespace Sasd.Ui.WinForms.Shell;

/// <summary>Severity used by shell status messages.</summary>
public enum SasdStatusSeverity
{
    /// <summary>Normal informational status.</summary>
    Information,

    /// <summary>Successful completion.</summary>
    Success,

    /// <summary>Warning that needs attention.</summary>
    Warning,

    /// <summary>Error that should remain visible until replaced deliberately.</summary>
    Error,
}

/// <summary>One status-bar message.</summary>
public sealed record SasdStatusMessage(
    string Text,
    SasdStatusSeverity Severity = SasdStatusSeverity.Information,
    int Priority = 0,
    TimeSpan? Lifetime = null);

/// <summary>Displays prioritized transient or persistent application status messages.</summary>
public class SasdStatusBar : StatusStrip
{
    private readonly ToolStripStatusLabel statusLabel;
    private readonly System.Windows.Forms.Timer clearTimer;
    private int currentPriority = int.MinValue;

    /// <summary>Initialises the status bar.</summary>
    public SasdStatusBar()
    {
        SizingGrip = false;
        statusLabel = new ToolStripStatusLabel
        {
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Text = "Ready",
        };
        Items.Add(statusLabel);

        clearTimer = new System.Windows.Forms.Timer();
        clearTimer.Tick += OnClearTimerTick;
    }

    /// <summary>Gets or sets the idle text displayed after transient messages expire.</summary>
    [Category("SASD")]
    [DefaultValue("Ready")]
    public string IdleText { get; set; } = "Ready";

    /// <summary>
    /// Displays a message unless a currently visible higher-priority message would be overwritten.
    /// </summary>
    public bool ShowMessage(SasdStatusMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(message.Text);

        if (message.Priority < currentPriority)
        {
            return false;
        }

        clearTimer.Stop();
        currentPriority = message.Priority;
        statusLabel.Text = message.Text;
        statusLabel.AccessibleDescription = message.Severity.ToString();

        if (message.Lifetime is { } lifetime && lifetime > TimeSpan.Zero)
        {
            clearTimer.Interval = (int)Math.Clamp(lifetime.TotalMilliseconds, 100, int.MaxValue);
            clearTimer.Start();
        }

        return true;
    }

    /// <summary>Clears the current status and restores the idle text.</summary>
    public void ClearMessage()
    {
        clearTimer.Stop();
        currentPriority = int.MinValue;
        statusLabel.Text = IdleText;
        statusLabel.AccessibleDescription = null;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            clearTimer.Tick -= OnClearTimerTick;
            clearTimer.Dispose();
        }

        base.Dispose(disposing);
    }

    private void OnClearTimerTick(object? sender, EventArgs e) => ClearMessage();
}
