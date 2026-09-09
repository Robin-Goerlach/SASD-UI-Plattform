namespace Sasd.Ui.WinForms.Windows;

/// <summary>Event data for an explicit tray open/exit request.</summary>
public sealed class SasdTrayRequestEventArgs : EventArgs
{
    /// <summary>Creates tray request event data.</summary>
    public SasdTrayRequestEventArgs(MouseButtons button = MouseButtons.None) => Button = button;

    /// <summary>Gets the mouse button that caused the request, when applicable.</summary>
    public MouseButtons Button { get; }
}

/// <summary>Application-facing contract for an explicit Windows notification-area icon.</summary>
public interface ISasdTrayService : IDisposable
{
    /// <summary>Occurs when the user asks to open/show the application.</summary>
    event EventHandler<SasdTrayRequestEventArgs>? OpenRequested;

    /// <summary>Occurs when the user chooses the explicit Exit menu item.</summary>
    event EventHandler<SasdTrayRequestEventArgs>? ExitRequested;

    /// <summary>Gets whether the notification-area icon is currently visible.</summary>
    bool IsVisible { get; }

    /// <summary>Gets or sets the short tooltip shown for the notification-area icon.</summary>
    string Text { get; set; }

    /// <summary>Makes the icon visible.</summary>
    void Show();

    /// <summary>Hides the icon without disposing the service.</summary>
    void Hide();

    /// <summary>Replaces the icon. The service clones and owns its copy.</summary>
    void SetIcon(Icon icon);
}

/// <summary>
/// Wraps a WinForms <see cref="NotifyIcon"/> with explicit lifetime and exit semantics.
/// </summary>
/// <remarks>
/// <para>
/// Creating this service does not silently place an icon in the notification area.
/// Applications must call <see cref="Show"/> deliberately. Likewise, an Exit click
/// only raises <see cref="ExitRequested"/>; the service never terminates the process.
/// </para>
/// <para>
/// The service owns the internal <see cref="NotifyIcon"/>, context menu, menu items
/// and a clone of the supplied icon. Callers retain ownership of icon instances passed
/// to the constructor or <see cref="SetIcon"/>.
/// </para>
/// </remarks>
public sealed class SasdTrayService : ISasdTrayService
{
    private readonly NotifyIcon notifyIcon;
    private readonly ContextMenuStrip menu;
    private readonly ToolStripMenuItem openItem;
    private readonly ToolStripMenuItem exitItem;
    private Icon ownedIcon;
    private bool disposed;

    /// <summary>Creates a hidden tray service using the application system icon.</summary>
    public SasdTrayService(string text = "SASD application")
        : this(SystemIcons.Application, text)
    {
    }

    /// <summary>Creates a hidden tray service using a caller-supplied icon.</summary>
    public SasdTrayService(Icon icon, string text = "SASD application")
    {
        ArgumentNullException.ThrowIfNull(icon);
        ownedIcon = (Icon)icon.Clone();

        openItem = new ToolStripMenuItem("Open");
        exitItem = new ToolStripMenuItem("Exit");
        menu = new ContextMenuStrip();
        menu.Items.Add(openItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(exitItem);

        notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = menu,
            Icon = ownedIcon,
            Text = ValidateText(text),
            Visible = false,
        };

        openItem.Click += HandleOpenClick;
        exitItem.Click += HandleExitClick;
        notifyIcon.DoubleClick += HandleDoubleClick;
    }

    /// <inheritdoc />
    public event EventHandler<SasdTrayRequestEventArgs>? OpenRequested;

    /// <inheritdoc />
    public event EventHandler<SasdTrayRequestEventArgs>? ExitRequested;

    /// <inheritdoc />
    public bool IsVisible => !disposed && notifyIcon.Visible;

    /// <inheritdoc />
    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            return notifyIcon.Text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            notifyIcon.Text = ValidateText(value);
        }
    }

    /// <summary>Gets or sets the user-visible text of the default Open menu item.</summary>
    public string OpenText
    {
        get
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            return openItem.Text ?? string.Empty;
        }
        set
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            openItem.Text = ValidateMenuText(value, nameof(value));
        }
    }

    /// <summary>Gets or sets the user-visible text of the default Exit menu item.</summary>
    public string ExitText
    {
        get
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            return exitItem.Text ?? string.Empty;
        }
        set
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            exitItem.Text = ValidateMenuText(value, nameof(value));
        }
    }

    /// <inheritdoc />
    public void Show()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        notifyIcon.Visible = true;
    }

    /// <inheritdoc />
    public void Hide()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        notifyIcon.Visible = false;
    }

    /// <inheritdoc />
    public void SetIcon(Icon icon)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentNullException.ThrowIfNull(icon);

        // Clone first so an invalid/disposed caller icon cannot make us lose the
        // currently working icon before the replacement has been created.
        Icon replacement = (Icon)icon.Clone();
        Icon previous = ownedIcon;
        ownedIcon = replacement;
        notifyIcon.Icon = ownedIcon;
        previous.Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        // Windows may otherwise keep a stale notification-area icon until Explorer
        // refreshes. Hide first, then detach callbacks and dispose all owned objects.
        notifyIcon.Visible = false;
        notifyIcon.DoubleClick -= HandleDoubleClick;
        openItem.Click -= HandleOpenClick;
        exitItem.Click -= HandleExitClick;
        notifyIcon.ContextMenuStrip = null;
        notifyIcon.Dispose();
        menu.Dispose();
        ownedIcon.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    private void HandleOpenClick(object? sender, EventArgs e) =>
        OpenRequested?.Invoke(this, new SasdTrayRequestEventArgs());

    private void HandleExitClick(object? sender, EventArgs e) =>
        ExitRequested?.Invoke(this, new SasdTrayRequestEventArgs());

    private void HandleDoubleClick(object? sender, EventArgs e)
    {
        MouseButtons button = e is MouseEventArgs mouseEvent ? mouseEvent.Button : MouseButtons.None;
        OpenRequested?.Invoke(this, new SasdTrayRequestEventArgs(button));
    }

    private static string ValidateText(string? text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        if (text.Length > 63)
        {
            // NotifyIcon.Text has a small native-shell limit on supported Windows
            // versions. Enforcing the conservative limit avoids a late runtime error.
            throw new ArgumentOutOfRangeException(nameof(text), "Tray tooltip text may contain at most 63 characters.");
        }

        return text;
    }

    private static string ValidateMenuText(string? text, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Tray menu text must not be empty.", parameterName);
        }

        return text;
    }
}
