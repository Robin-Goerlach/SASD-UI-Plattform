using System.Reflection;
using Sasd.Ui.WinForms.Dialogs;

namespace Sasd.Ui.DialogSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ValidateQueuedProgressBeforeHandle();
            ValidateQueuedCompletionBeforeShow();
            ValidateReportingAfterDispose();
            ValidateCancellation();

            Console.WriteLine("SASD dialog smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateQueuedProgressBeforeHandle()
    {
        using var dialog = new SasdProgressDialog("Test", "Waiting...");
        Label messageLabel = ReadPrivateField<Label>(dialog, "messageLabel");
        ProgressBar progressBar = ReadPrivateField<ProgressBar>(dialog, "progressBar");

        RunWorker(() =>
        {
            dialog.Report(new SasdProgressUpdate("First update", 10));
            dialog.Report(new SasdProgressUpdate("Latest update", 150));
        });

        // Keep handle creation on this executable's STA thread. An async console Main has no
        // WinForms synchronization context, so an await could otherwise resume on a pool thread
        // and make the smoke test itself violate the lifecycle rule it is intended to verify.
        Ensure(messageLabel.Text == "Waiting...", "Worker progress changed controls before handle creation.");
        _ = dialog.Handle;

        Ensure(messageLabel.Text == "Latest update", "Latest queued progress was not applied when the handle was created.");
        Ensure(progressBar.Style == ProgressBarStyle.Continuous, "Queued percentage did not switch the progress bar to determinate mode.");
        Ensure(progressBar.Value == 100, "Queued percentage was not clamped to the progress bar range.");
    }

    private static void ValidateQueuedCompletionBeforeShow()
    {
        using var dialog = new SasdProgressDialog("Test", "Completing...");

        RunWorker(() => dialog.Complete(DialogResult.Cancel));
        Ensure(dialog.DialogResult == DialogResult.None, "Completion touched the dialog before its show lifecycle began.");

        // A fast worker can complete before ShowDialog is entered. The dialog should still
        // participate in a normal WinForms show sequence and then close from Shown, rather
        // than attempting Close from HandleCreated while native handle creation is in flight.
        DialogResult result = dialog.ShowDialog();
        Ensure(result == DialogResult.Cancel, "Queued completion did not close the shown dialog with the requested result.");
        Ensure(dialog.DialogResult == DialogResult.Cancel, "Queued completion result was not retained by the dialog.");
    }

    private static void ValidateReportingAfterDispose()
    {
        var dialog = new SasdProgressDialog("Test", "Disposed test");
        dialog.Dispose();

        // Late worker callbacks are common when application shutdown races with an
        // operation. They should be harmless rather than producing a cross-thread or
        // disposed-control exception after the window has already gone away.
        RunWorker(() =>
        {
            dialog.Report(new SasdProgressUpdate("Late", 50));
            dialog.Complete();
        });
    }

    private static void ValidateCancellation()
    {
        using var dialog = new SasdProgressDialog("Test", "Cancelable");
        Button cancelButton = ReadPrivateField<Button>(dialog, "cancelButton");

        Ensure(!dialog.CancellationToken.IsCancellationRequested, "Progress dialog starts in a cancelled state.");

        dialog.Shown += (_, _) =>
        {
            // PerformClick models the actual control interaction only after the button is
            // visible/selectable. Calling PerformClick on an unshown control is not a faithful
            // WinForms user-interaction test and may legitimately do nothing.
            cancelButton.PerformClick();
            dialog.Complete(DialogResult.Cancel);
        };

        DialogResult result = dialog.ShowDialog();
        Ensure(result == DialogResult.Cancel, "Cancellation smoke dialog did not close with the expected result.");
        Ensure(dialog.CancellationToken.IsCancellationRequested, "Cancel button did not cancel the exposed token.");
        Ensure(!cancelButton.Enabled, "Cancel button remained enabled after cancellation was requested.");
    }

    private static void RunWorker(Action action)
    {
        // Block the STA test thread while the worker performs only the public cross-thread call.
        // No message pumping is needed because pre-handle/pre-show calls are expected to buffer state.
        Task.Run(action).GetAwaiter().GetResult();
    }

    private static T ReadPrivateField<T>(object instance, string fieldName)
        where T : class
    {
        FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Private field '{fieldName}' was not found.");

        return field.GetValue(instance) as T
            ?? throw new InvalidOperationException($"Private field '{fieldName}' did not contain {typeof(T).Name}.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
