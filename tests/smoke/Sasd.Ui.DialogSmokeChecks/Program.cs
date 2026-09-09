using System.Reflection;
using Sasd.Ui.WinForms.Dialogs;

namespace Sasd.Ui.DialogSmokeChecks;

internal static class Program
{
    [STAThread]
    private static async Task<int> Main()
    {
        try
        {
            await ValidateQueuedProgressBeforeHandleAsync();
            await ValidateQueuedCompletionBeforeHandleAsync();
            await ValidateReportingAfterDisposeAsync();
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

    private static async Task ValidateQueuedProgressBeforeHandleAsync()
    {
        using var dialog = new SasdProgressDialog("Test", "Waiting...");
        Label messageLabel = ReadPrivateField<Label>(dialog, "messageLabel");
        ProgressBar progressBar = ReadPrivateField<ProgressBar>(dialog, "progressBar");

        await Task.Run(() =>
        {
            dialog.Report(new SasdProgressUpdate("First update", 10));
            dialog.Report(new SasdProgressUpdate("Latest update", 150));
        });

        // A worker must never mutate WinForms controls before their native handle
        // exists. Only the latest state is buffered during this startup window.
        Ensure(messageLabel.Text == "Waiting...", "Worker progress changed controls before handle creation.");

        _ = dialog.Handle;

        Ensure(messageLabel.Text == "Latest update", "Latest queued progress was not applied when the handle was created.");
        Ensure(progressBar.Style == ProgressBarStyle.Continuous, "Queued percentage did not switch the progress bar to determinate mode.");
        Ensure(progressBar.Value == 100, "Queued percentage was not clamped to the progress bar range.");
    }

    private static async Task ValidateQueuedCompletionBeforeHandleAsync()
    {
        using var dialog = new SasdProgressDialog("Test", "Completing...");

        await Task.Run(() => dialog.Complete(DialogResult.Cancel));
        Ensure(dialog.DialogResult == DialogResult.None, "Completion touched the dialog before handle creation.");

        _ = dialog.Handle;
        Ensure(dialog.DialogResult == DialogResult.Cancel, "Queued completion result was not applied on handle creation.");
    }

    private static async Task ValidateReportingAfterDisposeAsync()
    {
        var dialog = new SasdProgressDialog("Test", "Disposed test");
        dialog.Dispose();

        // Late worker callbacks are common when application shutdown races with an
        // operation. They should be harmless rather than producing a cross-thread or
        // disposed-control exception after the window has already gone away.
        await Task.Run(() =>
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
        cancelButton.PerformClick();
        Ensure(dialog.CancellationToken.IsCancellationRequested, "Cancel button did not cancel the exposed token.");
        Ensure(!cancelButton.Enabled, "Cancel button remained enabled after cancellation was requested.");
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
