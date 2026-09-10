using Sasd.Ui.WinForms;

namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>One progress update for a long-running operation.</summary>
public sealed record SasdProgressUpdate(string? Message = null, int? Percent = null);

/// <summary>Provides a small cancellable progress dialog for application-owned async operations.</summary>
public sealed class SasdProgressDialog : SasdForm, IProgress<SasdProgressUpdate>
{
    private readonly Label messageLabel;
    private readonly ProgressBar progressBar;
    private readonly Button cancelButton;
    private readonly CancellationTokenSource cancellation = new();
    private readonly object pendingLock = new();
    private SasdProgressUpdate? pendingUpdate;
    private DialogResult? pendingCompletion;
    private volatile bool completed;
    private volatile bool disposed;

    /// <summary>Initialises the progress dialog.</summary>
    public SasdProgressDialog(string title, string initialMessage, bool allowCancellation = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(initialMessage);

        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimizeBox = false;
        MaximizeBox = false;
        ControlBox = false;
        MinimumSize = new Size(460, 190);
        ClientSize = new Size(520, 190);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(20),
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        messageLabel = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            MaximumSize = new Size(470, 0),
            Text = initialMessage,
        };

        progressBar = new ProgressBar
        {
            Dock = DockStyle.Top,
            Height = 22,
            Margin = new Padding(0, 16, 0, 16),
            Style = ProgressBarStyle.Marquee,
            MarqueeAnimationSpeed = 30,
        };

        cancelButton = new Button
        {
            AutoSize = true,
            Anchor = AnchorStyles.Right,
            Enabled = allowCancellation,
            Text = "Cancel",
        };
        cancelButton.Click += (_, _) => RequestCancellation();

        layout.Controls.Add(messageLabel, 0, 0);
        layout.Controls.Add(progressBar, 0, 1);
        layout.Controls.Add(cancelButton, 0, 2);
        Controls.Add(layout);

        // InvokeRequired is not a reliable cross-thread test before a native handle
        // exists. Progress may safely be applied as soon as the UI thread creates the
        // handle, but closing the form from HandleCreated itself is unsafe because the
        // WinForms show lifecycle has not completed yet. Queued completion is therefore
        // drained later from Shown, when Close() has normal form semantics.
        HandleCreated += OnHandleCreated;
        Shown += OnShown;
    }

    /// <summary>Gets the token cancelled when the user requests cancellation.</summary>
    public CancellationToken CancellationToken => cancellation.Token;

    /// <summary>Reports progress. May be called from a worker thread.</summary>
    public void Report(SasdProgressUpdate value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (disposed)
        {
            return;
        }

        if (!IsHandleCreated)
        {
            QueueProgress(value);
            return;
        }

        DispatchToUi(() => ApplyProgress(value));
    }

    /// <summary>Marks the operation complete and closes the dialog.</summary>
    public void Complete(DialogResult result = DialogResult.OK)
    {
        if (disposed)
        {
            return;
        }

        // A handle can exist before the form is actually visible. Calling Close while
        // the native handle is still being created/showed can interfere with WinForms'
        // internal lifecycle. Buffer completion until Shown in that startup window.
        if (!IsHandleCreated || !Visible)
        {
            QueueCompletion(result);
            return;
        }

        DispatchToUi(() => ApplyCompletion(result));
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            HandleCreated -= OnHandleCreated;
            Shown -= OnShown;

            lock (pendingLock)
            {
                disposed = true;
                pendingUpdate = null;
                pendingCompletion = null;
            }

            cancellation.Dispose();
        }

        base.Dispose(disposing);
    }

    private void QueueProgress(SasdProgressUpdate value)
    {
        lock (pendingLock)
        {
            if (disposed || completed || pendingCompletion is not null)
            {
                return;
            }

            // Progress updates are state, not an audit trail. Keeping only the most
            // recent startup value avoids an unbounded queue if a fast worker starts
            // before the dialog becomes visible.
            pendingUpdate = value;
        }
    }

    private void QueueCompletion(DialogResult result)
    {
        lock (pendingLock)
        {
            if (disposed || completed || pendingCompletion is not null)
            {
                return;
            }

            pendingCompletion = result;
        }
    }

    private void OnHandleCreated(object? sender, EventArgs e)
    {
        SasdProgressUpdate? update;

        lock (pendingLock)
        {
            if (disposed)
            {
                return;
            }

            update = pendingUpdate;
            pendingUpdate = null;
        }

        if (update is not null)
        {
            ApplyProgress(update);
        }
    }

    private void OnShown(object? sender, EventArgs e)
    {
        DialogResult? completion;

        lock (pendingLock)
        {
            if (disposed || completed)
            {
                return;
            }

            completion = pendingCompletion;
            pendingCompletion = null;
        }

        if (completion is { } result)
        {
            // Shown runs on the UI thread after WinForms has completed the initial
            // handle/show sequence. A fast operation that already completed may now
            // close the dialog without racing native handle creation.
            ApplyCompletion(result);
        }
    }

    private void DispatchToUi(Action action)
    {
        if (disposed || IsDisposed || Disposing)
        {
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
            // Close/Dispose can destroy the handle between the lifecycle check and
            // BeginInvoke. ObjectDisposedException derives from InvalidOperationException,
            // so this one catch deliberately covers both shutdown races.
        }
    }

    private void ApplyProgress(SasdProgressUpdate value)
    {
        if (disposed || completed)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(value.Message))
        {
            messageLabel.Text = value.Message;
        }

        if (value.Percent is { } percent)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = Math.Clamp(percent, progressBar.Minimum, progressBar.Maximum);
        }
        else
        {
            progressBar.Style = ProgressBarStyle.Marquee;
        }
    }

    private void ApplyCompletion(DialogResult result)
    {
        lock (pendingLock)
        {
            if (disposed || completed)
            {
                return;
            }

            completed = true;
            pendingUpdate = null;
            pendingCompletion = null;
        }

        DialogResult = result;
        Close();
    }

    private void RequestCancellation()
    {
        if (disposed || completed || cancellation.IsCancellationRequested)
        {
            return;
        }

        cancellation.Cancel();
        cancelButton.Enabled = false;
        messageLabel.Text = "Cancelling…";
    }
}
