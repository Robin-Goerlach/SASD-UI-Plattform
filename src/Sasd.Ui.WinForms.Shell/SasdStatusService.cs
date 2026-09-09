namespace Sasd.Ui.WinForms.Shell;

/// <summary>Event data published by <see cref="SasdStatusService"/>.</summary>
public sealed class SasdStatusMessageEventArgs : EventArgs
{
    /// <summary>Initialises status event data.</summary>
    public SasdStatusMessageEventArgs(SasdStatusMessage message) =>
        Message = message ?? throw new ArgumentNullException(nameof(message));

    /// <summary>Gets the validated status message.</summary>
    public SasdStatusMessage Message { get; }
}

/// <summary>Application-facing contract for publishing shell status without owning a status bar.</summary>
public interface ISasdStatusService
{
    /// <summary>Occurs when a status message is published.</summary>
    event EventHandler<SasdStatusMessageEventArgs>? StatusPublished;

    /// <summary>Occurs when the current status should return to its idle state.</summary>
    event EventHandler? ClearRequested;

    /// <summary>Publishes a validated status message.</summary>
    void Publish(SasdStatusMessage message);

    /// <summary>Requests that visual status surfaces clear the current message.</summary>
    void Clear();
}

/// <summary>
/// Small presentation-neutral status publisher for application and infrastructure code.
/// </summary>
/// <remarks>
/// Events are raised synchronously on the calling thread. A WinForms subscriber that
/// accepts publications from worker threads must marshal updates to the UI thread.
/// Keeping publication separate from <see cref="SasdStatusBar"/> allows commands and
/// services to report status without depending on a particular shell layout.
/// </remarks>
public sealed class SasdStatusService : ISasdStatusService
{
    /// <inheritdoc />
    public event EventHandler<SasdStatusMessageEventArgs>? StatusPublished;

    /// <inheritdoc />
    public event EventHandler? ClearRequested;

    /// <summary>Validates and publishes a status message from primitive values.</summary>
    public void Publish(
        string text,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        int priority = 0,
        TimeSpan? lifetime = null) =>
        Publish(new SasdStatusMessage(text, severity, priority, lifetime));

    /// <inheritdoc />
    public void Publish(SasdStatusMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(message.Text);

        if (message.Text.Length > 4_000)
        {
            throw new ArgumentOutOfRangeException(nameof(message), "Status text may contain at most 4000 characters.");
        }

        if (message.Lifetime is { } lifetime && lifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(message), "Status lifetime must be greater than zero when supplied.");
        }

        StatusPublished?.Invoke(this, new SasdStatusMessageEventArgs(message));
    }

    /// <inheritdoc />
    public void Clear() => ClearRequested?.Invoke(this, EventArgs.Empty);
}

/// <summary>
/// Connects an <see cref="ISasdStatusService"/> to a <see cref="SasdStatusBar"/> and
/// owns only the event subscription.
/// </summary>
public sealed class SasdStatusBinding : IDisposable
{
    private readonly ISasdStatusService service;
    private readonly SasdStatusBar statusBar;
    private bool disposed;

    /// <summary>Creates a status binding. Construct it on the WinForms UI thread.</summary>
    public SasdStatusBinding(ISasdStatusService service, SasdStatusBar statusBar)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.statusBar = statusBar ?? throw new ArgumentNullException(nameof(statusBar));
        service.StatusPublished += OnStatusPublished;
        service.ClearRequested += OnClearRequested;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        service.StatusPublished -= OnStatusPublished;
        service.ClearRequested -= OnClearRequested;
        disposed = true;
        GC.SuppressFinalize(this);
    }

    private void OnStatusPublished(object? sender, SasdStatusMessageEventArgs e) =>
        Dispatch(() => statusBar.ShowMessage(e.Message));

    private void OnClearRequested(object? sender, EventArgs e) =>
        Dispatch(statusBar.ClearMessage);

    private void Dispatch(Action action)
    {
        if (disposed || statusBar.IsDisposed || statusBar.Disposing)
        {
            return;
        }

        if (!statusBar.IsHandleCreated)
        {
            // A missing native handle makes InvokeRequired ambiguous. Dropping an
            // early status update is safer than accidentally touching WinForms from
            // a worker thread. Normal shells create the status-bar handle at startup.
            return;
        }

        if (!statusBar.InvokeRequired)
        {
            action();
            return;
        }

        try
        {
            statusBar.BeginInvoke(action);
        }
        catch (InvalidOperationException)
        {
            // The window can close between the lifecycle checks and BeginInvoke.
            // Status is transient, so a shutdown race should not fail the process.
        }
    }
}
