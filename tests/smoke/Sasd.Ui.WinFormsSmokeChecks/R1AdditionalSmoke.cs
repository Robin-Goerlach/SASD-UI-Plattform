using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.WinFormsSmokeChecks;

/// <summary>
/// Keeps newer R1 checks separate from the original smoke executable so the main
/// scenario remains readable while the foundation grows.
/// </summary>
internal static class R1AdditionalSmoke
{
    public static void ValidateNotificationsAndTray()
    {
        ValidateNotifications();
        ValidateTrayLifecycle();
    }

    private static void ValidateNotifications()
    {
        var service = new SasdNotificationService();
        int added = 0;
        int removed = 0;
        service.NotificationAdded += (_, _) => added++;
        service.NotificationRemoved += (_, _) => removed++;

        Guid persistentId = service.Publish("Persistent warning", SasdNotificationSeverity.Warning);
        Guid transientId = service.Publish(
            "Transient information",
            SasdNotificationSeverity.Information,
            TimeSpan.FromSeconds(5));

        Ensure(service.GetActive().Count == 2 && added == 2,
            "Notification service did not retain published notifications.");
        Ensure(service.Dismiss(transientId) && removed == 1,
            "Notification service did not dismiss an active notification.");
        Ensure(!service.Dismiss(transientId),
            "Notification service dismissed the same notification twice.");
        Ensure(service.GetActive().Single().Id == persistentId,
            "Unexpected notification remained after dismissal.");

        using var host = new SasdNotificationHost();
        host.Bind(service);
        Ensure(ReferenceEquals(host.Service, service), "Notification host did not retain its bound service.");
        host.Unbind();
        Ensure(host.Service is null, "Notification host did not release its service binding.");

        service.Clear();
        Ensure(service.GetActive().Count == 0 && removed == 2,
            "Notification service did not clear remaining notifications.");
    }

    private static void ValidateTrayLifecycle()
    {
        // The smoke test intentionally does not make the icon visible because hosted
        // CI does not represent an interactive user's notification area. Construction,
        // idempotent Hide and deterministic Dispose are still safe to validate here.
        using var tray = new SasdTrayService();
        Ensure(!tray.Visible, "Tray service must start hidden.");
        tray.Hide();
        Ensure(!tray.Visible, "Hiding an already hidden tray icon changed its state.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
