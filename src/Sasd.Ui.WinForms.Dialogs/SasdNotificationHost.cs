using System.ComponentModel;

namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Event data for a notification shown or dismissed by <see cref="SasdNotificationHost"/>.</summary>
public sealed class SasdNotificationHostEventArgs : EventArgs
{
    /// <summary>Initialises notification-host event data.</summary>
    public SasdNotificationHostEventArgs(SasdNotification notification) =>
        Notification = notification ?? throw new ArgumentNullException(nameof(notification));

    /// <summary>Gets the notification involved in the visual transition.</summary>
    public SasdNotification Notification { get; }
}

/// <summary>
/// Simple non-modal WinForms host for the latest application notification.
/// </summary>
/// <remarks>
/// <para>
/// The R1 host intentionally displays one notification at a time. A later release may add
/// queuing, stacking or animations if real applications demonstrate that need. Keeping the
/// first implementation small avoids turning transient feedback into a second dialog framework.
/// </para>
/// <para>
/// This control is not a substitute for modal confirmation, security warnings or detailed
/// error presentation. Important failures must still use an appropriate persistent surface.
/// </para>
/// </remarks>
public sealed class SasdNotificationHost : UserControl
{
    private readonly Label severityLabel;
    private readonly Label titleLabel;
    private readonly Label messageLabel;
    private readonly Button dismissButton;
    private readonly System.Windows.Forms.Timer lifetimeTimer;
    private readonly int ownerThreadId;
    private readonly object dispatchSync = new();
    private ISasdNotificationService? notificationService;
    private SasdNotification? currentNotification;
    private SasdNotification? pendingNotification;
    private bool disposed;

    /// <summary>Creates a hidden notification host.</summary>
    public SasdNotificationHost()
    {
        // WinForms controls are expected to be constructed on their owning UI thread.
        // Remember that thread so owner-thread publications remain usable before a native
        // handle exists and worker publications can be deferred without guessing from
        // InvokeRequired (which is ambiguous before handle creation).
        ownerThreadId = Environment.CurrentManagedThreadId;

        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        AccessibleName = "Application notification";
        Visible = false;

        severityLabel = new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 3, 8, 0),
        };

        titleLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(720, 0),
        };

        messageLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(720, 0),
        };

        dismissButton = new Button
        {
            AutoSize = true,
            Text = "Dismiss",
            AccessibleName = "Dismiss notification",
        };
        dismissButton.Click += OnDismissClick;

        var textLayout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
            Margin = Padding.Empty,
        };
        textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.Controls.Add(titleLabel, 0, 0);
        textLayout.Controls.Add(messageLabel, 0, 1);

        var root = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 3,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            RowCount = 1,
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        root.Controls.Add(severityLabel, 0, 0);
        root.Controls.Add(textLayout, 1, 0);
        root.Controls.Add(dismissButton, 2, 0);
        Controls.Add(root);

        lifetimeTimer = new System.Windows.Forms.Timer();
        lifetimeTimer.Tick += OnLifetimeTimerTick;
    }

    /// <summary>Occurs after a notification becomes the current visible notification.</summary>
    public event EventHandler<SasdNotificationHostEventArgs>? NotificationShown;

    /// <summary>Occurs after the current notification is dismissed.</summary>
    public event EventHandler<SasdNotificationHostEventArgs>? NotificationDismissed;

    /// <summary>Gets the currently displayed notification, or <see langword="null"/>.</summary>
    [Browsable(false)]
    public SasdNotification? CurrentNotification => currentNotification;

    /// <summary>
    /// Gets or sets the lifetime used when a notification does not provide one explicitly.
    /// A non-positive value keeps such notifications visible until they are replaced or dismissed.
    /// </summary>
    [Browsable(false)]
    public TimeSpan DefaultLifetime { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Binds the control to a notification service. Any previous service is detached first.
    /// </summary>
    public void Bind(ISasdNotificationService service)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentNullException.ThrowIfNull(service);

        if (ReferenceEquals(notificationService, service))
        {
            return;
        }

        Unbind();
        notificationService = service;
        notificationService.NotificationPublished += OnNotificationPublished;
    }

    /// <summary>Detaches the current notification service without dismissing the visible message.</summary>
    public void Unbind()
    {
        if (notificationService is null)
        {
            return;
        }

        notificationService.NotificationPublished -= OnNotificationPublished;
        notificationService = null;
    }

    /// <summary>Shows one validated notification immediately on the UI thread.</summary>
    public void ShowNotification(SasdNotification notification)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentNullException.ThrowIfNull(notification);

        SasdNotification validated = SasdNotification.Create(
            notification.Message,
            notification.Title,
            notification.Severity,
            notification.Lifetime);

        // An owner-thread publication before handle creation is already safe to project
        // into managed child-control state. It also supersedes any older worker publication
        // that was waiting for HandleCreated, because the R1 host deliberately models only
        // the latest notification rather than an unbounded notification queue.
        lock (dispatchSync)
        {
            pendingNotification = null;
        }

        currentNotification = validated;
        severityLabel.Text = validated.Severity.ToString();
        titleLabel.Text = validated.Title ?? string.Empty;
        titleLabel.Visible = !string.IsNullOrWhiteSpace(validated.Title);
        messageLabel.Text = validated.Message;
        AccessibleDescription = $"{validated.Severity}: {validated.Message}";
        Visible = true;

        lifetimeTimer.Stop();
        TimeSpan lifetime = validated.Lifetime ?? DefaultLifetime;
        if (lifetime > TimeSpan.Zero)
        {
            lifetimeTimer.Interval = (int)Math.Clamp(lifetime.TotalMilliseconds, 100D, int.MaxValue);
            lifetimeTimer.Start();
        }

        NotificationShown?.Invoke(this, new SasdNotificationHostEventArgs(validated));
    }

    /// <summary>Dismisses the current notification and hides the host.</summary>
    public void Dismiss()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        if (currentNotification is null)
        {
            return;
        }

        SasdNotification dismissed = currentNotification;
        lifetimeTimer.Stop();
        currentNotification = null;
        Visible = false;
        AccessibleDescription = null;
        NotificationDismissed?.Invoke(this, new SasdNotificationHostEventArgs(dismissed));
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        SasdNotification? pending;
        lock (dispatchSync)
        {
            if (disposed)
            {
                return;
            }

            pending = pendingNotification;
            pendingNotification = null;
        }

        // HandleCreated is raised on the control's owner thread. Flushing here avoids
        // BeginInvoke before a handle exists and gives startup worker publications a
        // deterministic hand-off point. Only the latest pending notification is retained,
        // matching the host's documented one-notification-at-a-time model.
        if (pending is not null)
        {
            ShowNotification(pending);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            Unbind();
            lifetimeTimer.Tick -= OnLifetimeTimerTick;
            lifetimeTimer.Dispose();
            dismissButton.Click -= OnDismissClick;

            lock (dispatchSync)
            {
                // A queued startup notification must not retain application data or become
                // visible if a control is disposed before its handle is ever created.
                pendingNotification = null;
                disposed = true;
            }
        }

        base.Dispose(disposing);
    }

    private void OnNotificationPublished(object? sender, SasdNotificationEventArgs e) =>
        Dispatch(e.Notification);

    private void OnDismissClick(object? sender, EventArgs e) => Dismiss();

    private void OnLifetimeTimerTick(object? sender, EventArgs e) => Dismiss();

    private void Dispatch(SasdNotification notification)
    {
        if (disposed || IsDisposed || Disposing)
        {
            return;
        }

        if (!IsHandleCreated)
        {
            // InvokeRequired can return false on a worker thread before handle creation.
            // Direct managed-control access is therefore allowed only on the constructor
            // thread. Worker publications retain the latest value until HandleCreated.
            if (Environment.CurrentManagedThreadId == ownerThreadId)
            {
                ShowNotification(notification);
                return;
            }

            lock (dispatchSync)
            {
                if (disposed || IsDisposed || Disposing)
                {
                    return;
                }

                // Recheck under the lock. The owner thread may have created the handle
                // between the first IsHandleCreated test and acquiring dispatchSync.
                if (!IsHandleCreated)
                {
                    pendingNotification = notification;
                    return;
                }
            }
        }

        if (!InvokeRequired)
        {
            ShowNotification(notification);
            return;
        }

        try
        {
            BeginInvoke(() => ShowNotification(notification));
        }
        catch (InvalidOperationException)
        {
            // The host may close between the lifecycle checks and BeginInvoke.
            // A shutdown race must not turn transient feedback into a process error.
        }
    }
}
