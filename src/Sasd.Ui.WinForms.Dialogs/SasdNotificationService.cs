namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Semantic severity used by non-modal application notifications.</summary>
public enum SasdNotificationSeverity
{
    /// <summary>Neutral information that does not require immediate user action.</summary>
    Information,

    /// <summary>Positive feedback confirming that an operation completed successfully.</summary>
    Success,

    /// <summary>A recoverable condition that deserves user attention.</summary>
    Warning,

    /// <summary>An operation failure or other important problem visible to the user.</summary>
    Error,
}

/// <summary>Represents one application notification with a stable runtime identity.</summary>
public sealed record SasdNotification(
    Guid Id,
    string Message,
    SasdNotificationSeverity Severity,
    DateTimeOffset CreatedAtUtc,
    TimeSpan? Lifetime = null);

/// <summary>Event data for notification collection changes.</summary>
public sealed class SasdNotificationEventArgs : EventArgs
{
    /// <summary>Initialises notification event data.</summary>
    public SasdNotificationEventArgs(SasdNotification notification) => Notification = notification;

    /// <summary>Gets the affected notification.</summary>
    public SasdNotification Notification { get; }
}

/// <summary>
/// Stores transient in-application notifications independently of a particular
/// visual host. Applications may publish from background work; UI hosts marshal
/// collection changes to their owning WinForms thread.
/// </summary>
public sealed class SasdNotificationService
{
    private readonly object sync = new();
    private readonly Dictionary<Guid, SasdNotification> active = [];

    /// <summary>Raised after a notification has been added.</summary>
    public event EventHandler<SasdNotificationEventArgs>? NotificationAdded;

    /// <summary>Raised after a notification has been removed.</summary>
    public event EventHandler<SasdNotificationEventArgs>? NotificationRemoved;

    /// <summary>Publishes a new notification and returns its ID.</summary>
    public Guid Publish(
        string message,
        SasdNotificationSeverity severity = SasdNotificationSeverity.Information,
        TimeSpan? lifetime = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        if (lifetime is { } configuredLifetime && configuredLifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(lifetime), "Notification lifetime must be positive when configured.");
        }

        var notification = new SasdNotification(
            Guid.NewGuid(),
            message,
            severity,
            DateTimeOffset.UtcNow,
            lifetime);

        lock (sync)
        {
            active.Add(notification.Id, notification);
        }

        NotificationAdded?.Invoke(this, new SasdNotificationEventArgs(notification));
        return notification.Id;
    }

    /// <summary>Dismisses one notification. Returns false when it is already absent.</summary>
    public bool Dismiss(Guid notificationId)
    {
        SasdNotification? removed;
        lock (sync)
        {
            if (!active.Remove(notificationId, out removed))
            {
                return false;
            }
        }

        NotificationRemoved?.Invoke(this, new SasdNotificationEventArgs(removed));
        return true;
    }

    /// <summary>Returns a stable snapshot ordered by creation time.</summary>
    public IReadOnlyList<SasdNotification> GetActive()
    {
        lock (sync)
        {
            return active.Values
                .OrderBy(notification => notification.CreatedAtUtc)
                .ToArray();
        }
    }

    /// <summary>Dismisses all currently active notifications.</summary>
    public void Clear()
    {
        SasdNotification[] removed;
        lock (sync)
        {
            removed = active.Values.ToArray();
            active.Clear();
        }

        foreach (SasdNotification notification in removed)
        {
            NotificationRemoved?.Invoke(this, new SasdNotificationEventArgs(notification));
        }
    }
}

/// <summary>
/// Displays notifications published by a <see cref="SasdNotificationService"/> as
/// dismissible in-application cards. It does not use system balloon notifications,
/// so critical feedback remains inside the owning application window.
/// </summary>
public sealed class SasdNotificationHost : UserControl
{
    private readonly FlowLayoutPanel stack;
    private readonly Dictionary<Guid, Control> cards = [];
    private readonly Dictionary<Guid, System.Windows.Forms.Timer> timers = [];
    private SasdNotificationService? service;

    /// <summary>Initialises an unbound notification host.</summary>
    public SasdNotificationHost()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;

        stack = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(4),
        };
        Controls.Add(stack);
    }

    /// <summary>Gets the currently bound service, if any.</summary>
    public SasdNotificationService? Service => service;

    /// <summary>Binds the host to a notification service and renders its current snapshot.</summary>
    public void Bind(SasdNotificationService notificationService)
    {
        ArgumentNullException.ThrowIfNull(notificationService);
        if (ReferenceEquals(service, notificationService))
        {
            return;
        }

        UnbindCurrentService();
        service = notificationService;
        service.NotificationAdded += OnNotificationAdded;
        service.NotificationRemoved += OnNotificationRemoved;
        RebuildFromService();
    }

    /// <summary>Stops displaying service notifications without clearing service state.</summary>
    public void Unbind()
    {
        UnbindCurrentService();
        service = null;
        ClearVisuals();
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        // If notifications arrived from a worker thread before the WinForms handle
        // existed, their events could not be marshalled. Rebuild from authoritative
        // service state whenever a handle becomes available.
        RebuildFromService();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            UnbindCurrentService();
            ClearVisuals();
        }

        base.Dispose(disposing);
    }

    private void OnNotificationAdded(object? sender, SasdNotificationEventArgs e)
    {
        // An event can already be queued while the host is being rebound. Ignore it
        // when it belongs to the previous service instead of rendering stale state.
        if (!ReferenceEquals(sender, service))
        {
            return;
        }

        PostToUi(() => AddCard(e.Notification));
    }

    private void OnNotificationRemoved(object? sender, SasdNotificationEventArgs e)
    {
        if (!ReferenceEquals(sender, service))
        {
            return;
        }

        PostToUi(() => RemoveCard(e.Notification.Id));
    }

    private void RebuildFromService()
    {
        if (service is null || IsDisposed)
        {
            return;
        }

        if (InvokeRequired)
        {
            PostToUi(RebuildFromService);
            return;
        }

        ClearVisuals();
        foreach (SasdNotification notification in service.GetActive())
        {
            AddCard(notification);
        }
    }

    private void AddCard(SasdNotification notification)
    {
        if (cards.ContainsKey(notification.Id) || service is null)
        {
            return;
        }

        var messageLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(520, 0),
            Text = notification.Message,
            AccessibleName = $"{notification.Severity} notification: {notification.Message}",
            Margin = new Padding(0, 4, 8, 4),
        };
        var dismissButton = new Button
        {
            AutoSize = true,
            Text = "Dismiss",
            AccessibleName = $"Dismiss {notification.Severity} notification",
            Margin = new Padding(8, 0, 0, 0),
        };
        dismissButton.Click += (_, _) => service?.Dismiss(notification.Id);

        var card = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            Padding = new Padding(8),
            Margin = new Padding(0, 0, 0, 6),
            BorderStyle = BorderStyle.FixedSingle,
        };
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        card.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        card.Controls.Add(messageLabel, 0, 0);
        card.Controls.Add(dismissButton, 1, 0);
        stack.Controls.Add(card);
        cards.Add(notification.Id, card);

        if (notification.Lifetime is { } lifetime)
        {
            int interval = (int)Math.Clamp(lifetime.TotalMilliseconds, 1, int.MaxValue);
            var timer = new System.Windows.Forms.Timer { Interval = interval };
            timer.Tick += (_, _) => service?.Dismiss(notification.Id);
            timers.Add(notification.Id, timer);
            timer.Start();
        }
    }

    private void RemoveCard(Guid notificationId)
    {
        if (timers.Remove(notificationId, out System.Windows.Forms.Timer? timer))
        {
            timer.Stop();
            timer.Dispose();
        }

        if (cards.Remove(notificationId, out Control? card))
        {
            stack.Controls.Remove(card);
            card.Dispose();
        }
    }

    private void ClearVisuals()
    {
        foreach (System.Windows.Forms.Timer timer in timers.Values)
        {
            timer.Stop();
            timer.Dispose();
        }

        timers.Clear();
        foreach (Control card in cards.Values)
        {
            stack.Controls.Remove(card);
            card.Dispose();
        }

        cards.Clear();
    }

    private void UnbindCurrentService()
    {
        if (service is null)
        {
            return;
        }

        service.NotificationAdded -= OnNotificationAdded;
        service.NotificationRemoved -= OnNotificationRemoved;
    }

    private void PostToUi(Action action)
    {
        if (IsDisposed || Disposing)
        {
            return;
        }

        if (!IsHandleCreated)
        {
            // Service state remains authoritative; OnHandleCreated rebuilds later.
            return;
        }

        if (!InvokeRequired)
        {
            action();
            return;
        }

        try
        {
            BeginInvoke(action);
        }
        catch (InvalidOperationException)
        {
            // The handle can disappear while a form is closing. Notifications are
            // transient UI state and must never block shutdown.
        }
    }
}
