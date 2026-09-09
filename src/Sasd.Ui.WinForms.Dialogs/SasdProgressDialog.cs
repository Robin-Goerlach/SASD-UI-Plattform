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
    private bool completed;

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
    }

    /// <summary>Gets the token cancelled when the user requests cancellation.</summary>
    public CancellationToken CancellationToken => cancellation.Token;

    /// <summary>Reports progress. May be called from a worker thread.</summary>
    public void Report(SasdProgressUpdate value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (IsDisposed || completed)
        {
            return;
        }

        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => Report(value)));
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

    /// <summary>Marks the operation complete and closes the dialog.</summary>
    public void Complete(DialogResult result = DialogResult.OK)
    {
        if (IsDisposed || completed)
        {
            return;
        }

        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => Complete(result)));
            return;
        }

        completed = true;
        DialogResult = result;
        Close();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            cancellation.Dispose();
        }

        base.Dispose(disposing);
    }

    private void RequestCancellation()
    {
        if (completed || cancellation.IsCancellationRequested)
        {
            return;
        }

        cancellation.Cancel();
        cancelButton.Enabled = false;
        messageLabel.Text = "Cancelling…";
    }
}
