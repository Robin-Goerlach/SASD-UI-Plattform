namespace Sasd.Ui.WinForms;

/// <summary>Provides safe, explicit transitions to a WinForms UI thread.</summary>
public static class SasdUiDispatcher
{
    /// <summary>
    /// Executes <paramref name="action"/> on the owning UI thread of
    /// <paramref name="control"/>.
    /// </summary>
    /// <remarks>
    /// A WinForms control without a native handle cannot reliably answer
    /// <see cref="Control.InvokeRequired"/> from a worker thread. Callers therefore
    /// must use this dispatcher only after the control handle has been created.
    /// This explicit failure is safer than accidentally executing UI code on the
    /// wrong thread.
    /// </remarks>
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

        if (!control.IsHandleCreated)
        {
            return Task.FromException(new InvalidOperationException(
                "The control handle must be created before work can be dispatched to its UI thread."));
        }

        if (!control.InvokeRequired)
        {
            action();
            return Task.CompletedTask;
        }

        var completion = new TaskCompletionSource<object?>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        try
        {
            control.BeginInvoke(new Action(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    completion.TrySetCanceled(cancellationToken);
                    return;
                }

                if (control.IsDisposed || control.Disposing)
                {
                    completion.TrySetException(new ObjectDisposedException(control.GetType().FullName));
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
        }
        catch (InvalidOperationException exception)
        {
            // The handle can disappear between the checks above and BeginInvoke
            // when a form is closing. Surface that race as a normal failed task.
            completion.TrySetException(exception);
        }

        return completion.Task;
    }
}
