using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Forms;

namespace Sasd.Ui.CompositeFeedbackSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ValidateEmptyState();
            ValidateValidationSummary();
            ValidateBusyOverlay();
            ValidateNotificationHostLifecycle();

            Console.WriteLine("SASD composite feedback smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateEmptyState()
    {
        using var emptyState = new SasdEmptyState();

        Ensure(emptyState.AccessibleRole == AccessibleRole.Grouping,
            "Empty state does not expose itself as an accessible grouping.");
        Ensure(emptyState.AccessibleName == "Nothing here yet",
            "Empty-state accessible name is not derived from the default title.");

        emptyState.Title = "No reports";
        emptyState.Message = "Create the first report to continue.";
        emptyState.ActionText = "Create report";

        Ensure(emptyState.AccessibleName == "No reports",
            "Changing the empty-state title did not update its accessible name.");
        Ensure(emptyState.AccessibleDescription == "Create the first report to continue.",
            "Changing the empty-state message did not update its accessible description.");

        // The action button remains an implementation detail of the composite control.
        // Walking the normal WinForms child-control tree lets the smoke check exercise the
        // same button a keyboard/mouse user invokes without adding a product-only test hook.
        Button actionButton = FindDescendant<Button>(emptyState)
            ?? throw new InvalidOperationException("Empty-state action button was not created.");
        Ensure(actionButton.AccessibleName == "Create report",
            "Empty-state action text and accessible button name diverged.");

        int invocations = 0;
        emptyState.ActionInvoked += (_, _) => invocations++;
        actionButton.PerformClick();
        Ensure(invocations == 1, "Empty-state action did not raise exactly one invocation event.");

        emptyState.ActionVisible = false;
        Ensure(!emptyState.ActionVisible, "Empty-state action could not be hidden.");
        emptyState.ActionVisible = true;
    }

    private static void ValidateValidationSummary()
    {
        using var summary = new SasdValidationSummary();
        Ensure(summary.AccessibleRole == AccessibleRole.Alert,
            "Validation summary does not expose an alert role.");
        Ensure(!summary.Visible && summary.AccessibleDescription is null,
            "A new validation summary should be hidden and silent.");

        var result = new SasdValidationResult([
            new SasdValidationMessage("name", "Name is required."),
            new SasdValidationMessage("email", "Email address is invalid."),
        ]);

        summary.ShowResult(result);
        Ensure(summary.Visible, "Validation summary remained hidden after receiving messages.");
        Ensure(summary.AccessibleDescription?.Contains("Name is required.", StringComparison.Ordinal) == true,
            "Validation summary accessible text omitted the first visible message.");
        Ensure(summary.AccessibleDescription?.Contains("Email address is invalid.", StringComparison.Ordinal) == true,
            "Validation summary accessible text omitted the second visible message.");

        summary.Heading = "Please correct the form:";
        Ensure(summary.AccessibleDescription?.StartsWith("Please correct the form:", StringComparison.Ordinal) == true,
            "Changing the summary heading did not refresh its accessible text.");

        summary.Clear();
        Ensure(!summary.Visible && summary.AccessibleDescription is null,
            "Clearing validation did not hide and silence the summary.");

        summary.ShowResult(SasdValidationResult.Success);
        Ensure(!summary.Visible && summary.AccessibleDescription is null,
            "A successful validation result unexpectedly exposed an empty summary.");
    }

    private static void ValidateBusyOverlay()
    {
        using var overlay = new SasdBusyOverlay();
        Ensure(!overlay.Visible && !overlay.UseWaitCursor,
            "A new busy overlay should not block the application.");
        Ensure(overlay.AccessibleRole == AccessibleRole.Grouping,
            "Busy overlay does not expose a stable accessible grouping role.");
        Ensure(overlay.AccessibleDescription is null,
            "Hidden busy overlay unexpectedly exposes active busy-state text.");

        overlay.BeginBusy("Loading customers…");
        Ensure(overlay.Visible && overlay.UseWaitCursor,
            "Busy overlay did not enter its blocking state.");
        Ensure(overlay.Message == "Loading customers…",
            "Busy overlay did not retain the supplied user-facing message.");
        Ensure(overlay.AccessibleDescription == "Loading customers…",
            "Busy overlay did not expose its current message through accessible text.");

        overlay.Message = "Still working…";
        Ensure(overlay.AccessibleDescription == "Still working…",
            "Changing a visible busy message did not refresh accessible text.");

        overlay.EndBusy();
        Ensure(!overlay.Visible && !overlay.UseWaitCursor,
            "Busy overlay did not restore its idle state.");
        Ensure(overlay.AccessibleDescription is null,
            "Busy overlay kept stale accessible text after leaving the busy state.");
    }

    private static void ValidateNotificationHostLifecycle()
    {
        var service = new SasdNotificationService();
        var host = new SasdNotificationHost
        {
            // A zero default lifetime deliberately avoids timer/message-pump timing in this
            // smoke check. Timer expiry belongs to UI integration evidence, while the binding
            // and deterministic dismissal contracts can be verified synchronously here.
            DefaultLifetime = TimeSpan.Zero,
        };

        int shown = 0;
        int dismissed = 0;
        host.NotificationShown += (_, _) => shown++;
        host.NotificationDismissed += (_, _) => dismissed++;
        host.Bind(service);

        service.Publish("Saved successfully.", "Save", SasdNotificationSeverity.Success);
        Ensure(host.CurrentNotification is not null && shown == 1,
            "Bound notification host did not receive exactly one service publication.");
        Ensure(host.Visible, "Notification host remained hidden after a publication.");
        Ensure(host.AccessibleDescription?.Contains("Saved successfully.", StringComparison.Ordinal) == true,
            "Notification host accessible text omitted the current message.");

        host.Dismiss();
        Ensure(host.CurrentNotification is null && !host.Visible && dismissed == 1,
            "Notification dismissal did not clear the current visible notification.");
        Ensure(host.AccessibleDescription is null,
            "Dismissed notification remained in the host's accessible description.");

        host.Unbind();
        service.Publish("Detached publication");
        Ensure(shown == 1 && host.CurrentNotification is null,
            "Notification host continued receiving publications after Unbind.");

        host.Bind(service);
        host.Dispose();

        // Dispose must detach the external service subscription. Publishing after disposal
        // therefore exercises a common application shutdown path without touching a dead UI.
        service.Publish("After disposal");
        Ensure(shown == 1, "Disposed notification host remained subscribed to its service.");
        EnsureThrows<ObjectDisposedException>(
            () => host.ShowNotification(new SasdNotification("Rejected after disposal")),
            "Disposed notification host accepted a direct notification.");
    }

    private static TControl? FindDescendant<TControl>(Control parent)
        where TControl : Control
    {
        foreach (Control child in parent.Controls)
        {
            if (child is TControl match)
            {
                return match;
            }

            TControl? descendant = FindDescendant<TControl>(child);
            if (descendant is not null)
            {
                return descendant;
            }
        }

        return null;
    }

    private static void EnsureThrows<TException>(Action action, string message)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
