namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Severity used for transient, non-modal notifications.</summary>
public enum SasdNotificationSeverity
{
    /// <summary>Normal informational feedback.</summary>
    Information,

    /// <summary>Successful completion feedback.</summary>
    Success,

    /// <summary>Non-fatal condition requiring attention.</summary>
    Warning,

    /// <summary>Failure feedback that is safe to present non-modally.</summary>
    Error,
}

/// <summary>Immutable notification message published by <see cref="SasdNotificationService"/>.</summary>
public sealed record SasdNotification(
    string Message,
    string? Title = null,
    SasdNotificationSeverity Severity = SasdNotificationSeverity.Information,
    TimeSpan? Lifetime = null)
{
    /// <summary>Creates and validates a notification.</summary>
    public static SasdNotification Create(
        string message,
        string? title = null,
        SasdNotificationSeverity severity = SasdNotificationSeverity.Information,
        TimeSpan? lifetime = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        if (message.Length > 4_000)
        {
            throw new ArgumentOutOfRangeException(nameof(message), "Notification messages may contain at most 4000 characters.");
        }

        if (title is { Length: > 300 })
        {
            throw new ArgumentOutOfRangeException(nameof(title), "Notification titles may contain at most 300 characters.");
        }

        if (lifetime is not null && (lifetime <= TimeSpan.Zero || lifetime > TimeSpan.FromHours(24)))
        {
            throw new ArgumentOutOfRangeException(nameof(lifetime), "Notification lifetime must be greater than zero and at most 24 hours.");
        }

        return new SasdNotification(message, title, severity, lifetime);
    }
}

/// <summary>Event data for a published notification.</summary>
public sealed class SasdNotificationEventArgs : EventArgs
{
    /// <summary>Initialises event data.</summary>
    public SasdNotificationEventArgs(SasdNotification notification) =>
        Notification = notification ?? throw new ArgumentNullException(nameof(notification));

    /// <summary>Gets the published notification.</summary>
    public SasdNotification Notification { get; }
}

/// <summary>Application-facing contract for non-modal notification publication.</summary>
public interface ISasdNotificationService
{
    /// <summary>Occurs whenever a notification is published.</summary>
    event EventHandler<SasdNotificationEventArgs>? NotificationPublished;

    /// <summary>Publishes a validated notification.</summary>
    void Publish(SasdNotification notification);
}

/// <summary>
/// Small notification bus that deliberately does not prescribe toast rendering.
/// </summary>
/// <remarks>
/// Events are raised synchronously on the calling thread. A visual subscriber that
/// can receive notifications from worker threads must marshal to its UI thread,
/// for example through <c>SasdUiDispatcher</c>. Keeping this service presentation-
/// neutral lets applications use a status bar, toast host or logging bridge without
/// changing the publication contract.
/// </remarks>
public sealed class SasdNotificationService : ISasdNotificationService
{
    /// <inheritdoc />
    public event EventHandler<SasdNotificationEventArgs>? NotificationPublished;

    /// <summary>Validates and publishes a new notification from primitive values.</summary>
    public void Publish(
        string message,
        string? title = null,
        SasdNotificationSeverity severity = SasdNotificationSeverity.Information,
        TimeSpan? lifetime = null) =>
        Publish(SasdNotification.Create(message, title, severity, lifetime));

    /// <inheritdoc />
    public void Publish(SasdNotification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        // Re-run validation so callers cannot bypass invariants by directly using
        // the record's public constructor.
        SasdNotification validated = SasdNotification.Create(
            notification.Message,
            notification.Title,
            notification.Severity,
            notification.Lifetime);

        NotificationPublished?.Invoke(this, new SasdNotificationEventArgs(validated));
    }
}
