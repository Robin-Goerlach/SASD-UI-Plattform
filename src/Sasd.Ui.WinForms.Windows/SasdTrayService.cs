namespace Sasd.Ui.WinForms.Windows;

/// <summary>Abstraction for an explicit Windows notification-area icon lifecycle.</summary>
public interface ISasdTrayService : IDisposable
{
    /// <summary>Raised when the user requests the owning application window to be restored.</summary>
    event EventHandler? RestoreRequested;

    /// <summary>Raised when the user selects the explicit Exit command.</summary>
    event EventHandler? ExitRequested;

    /// <summary>Gets whether the tray icon is currently visible.</summary>
    bool Visible { get; }

    /// <summary>Shows or updates the tray icon.</summary>
    void Show(Icon icon, string toolTipText);

    /// <summary>Hides the tray icon without terminating the application.</summary>
    void Hide();
}

/// <summary>
/// Owns one <see cref="NotifyIcon"/> and provides predictable restore/exit events.
/// The service never changes Form visibility or terminates the process by itself;
/// consuming applications remain responsible for their shutdown and minimize policy.
/// </summary>
public sealed class SasdTrayService : ISasdTrayService
{
    private const int NotifyIconTextLimit = 63;
    private readonly NotifyIcon notifyIcon;
    private readonly ContextMenuStrip contextMenu;
    private bool disposed;

    /// <summary>Initialises an invisible tray service.</summary>
    public SasdTrayService()
    {
        contextMenu = new ContextMenuStrip();
        var restoreItem = new ToolStripMenuItem("Restore")
        {
            AccessibleName = "Restore application window",
        };
        var exitItem = new ToolStripMenuItem("Exit")
        {
            AccessibleName = "Exit application",
        };
        restoreItem.Click += OnRestoreRequested;
        exitItem.Click += OnExitRequested;
        contextMenu.Items.Add(restoreItem);
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add(exitItem);

        notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = contextMenu,
            Visible = false,
        };
        notifyIcon.DoubleClick += OnRestoreRequested;
    }

    /// <inheritdoc />
    public event EventHandler? RestoreRequested;

    /// <inheritdoc />
    public event EventHandler? ExitRequested;

    /// <inheritdoc />
    public bool Visible => !disposed && notifyIcon.Visible;

    /// <inheritdoc />
    public void Show(Icon icon, string toolTipText)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentNullException.ThrowIfNull(icon);
        ArgumentException.ThrowIfNullOrWhiteSpace(toolTipText);

        notifyIcon.Icon = icon;
        // WinForms/Windows imposes a small tooltip limit. Truncating is safer for a
        // reusable service than allowing a product name/localisation to throw here.
        notifyIcon.Text = toolTipText.Length <= NotifyIconTextLimit
            ? toolTipText
            : toolTipText[..NotifyIconTextLimit];
        notifyIcon.Visible = true;
    }

    /// <inheritdoc />
    public void Hide()
    {
        if (disposed)
        {
            return;
        }

        notifyIcon.Visible = false;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        // Hide before disposing so a stale icon is less likely to remain in the
        // notification area until Windows refreshes it.
        notifyIcon.Visible = false;
        notifyIcon.DoubleClick -= OnRestoreRequested;
        notifyIcon.Dispose();
        contextMenu.Dispose();
    }

    private void OnRestoreRequested(object? sender, EventArgs e) => RestoreRequested?.Invoke(this, EventArgs.Empty);

    private void OnExitRequested(object? sender, EventArgs e) => ExitRequested?.Invoke(this, EventArgs.Empty);
}
