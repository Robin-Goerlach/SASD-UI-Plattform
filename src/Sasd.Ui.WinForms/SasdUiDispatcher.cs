namespace Sasd.Ui.WinForms;

/// <summary>Provides safe, explicit transitions to a WinForms UI thread.</summary>
public static class SasdUiDispatcher
{
    /// <summary>
    /// Executes <paramref name="action"/> on the owning UI thread of
    /// <paramref name="control"/>.
    /// </summary>
    public static Task InvokeAsync(
        Control control,
        Action action,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(action);
        cancellationToken.ThrowIfCancellationRequested();

        if (control.IsDisposed || control.Disposing)
        {
            return Task.FromException(
                new ObjectDisposedException(control.GetType().FullName));
        }

        if (!control.InvokeRequired)
        {
            action();
            return Task.CompletedTask;
        }

        var completion = new TaskCompletionSource<object?>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        control.BeginInvoke(new Action(() =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                completion.TrySetCanceled(cancellationToken);
                return;
            }

            try
            {
                action();
                completion.TrySetResult(null);
            }
            catch (Exception exception)
            {
                completion.TrySetException(exception);
            }
        }));

        return completion.Task;
    }
}
