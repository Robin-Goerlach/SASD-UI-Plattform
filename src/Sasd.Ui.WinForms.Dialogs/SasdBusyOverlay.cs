using System.ComponentModel;

namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Blocks interaction with a region while clearly communicating ongoing work.</summary>
/// <remarks>
/// The overlay owns presentation only. Applications retain ownership of the actual operation,
/// its <see cref="CancellationTokenSource"/> and any rollback/consistency policy. A cancellation
/// request is therefore exposed as an event rather than making the control cancel arbitrary work.
/// </remarks>
public class SasdBusyOverlay : UserControl
{
    private readonly Label messageLabel;
    private readonly ProgressBar progressBar;
    private readonly Button cancelButton;
    private Control? previousFocus;
    private bool cancellationEnabled;
    private int? progressPercentage;

    /// <summary>Initialises the busy overlay.</summary>
    public SasdBusyOverlay()
    {
        Dock = DockStyle.Fill;
        BackColor = SystemColors.Control;
        TabStop = false;
        Visible = false;
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Busy state";

        var content = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Anchor = AnchorStyles.None,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(24),
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        messageLabel = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.None,
            Text = "Working…",
            TextAlign = ContentAlignment.MiddleCenter,
        };

        progressBar = new ProgressBar
        {
            AccessibleName = "Operation progress",
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            MarqueeAnimationSpeed = 30,
            MinimumSize = new Size(240, 20),
            Style = ProgressBarStyle.Marquee,
        };

        cancelButton = new Button
        {
            AccessibleName = "Cancel operation",
            Anchor = AnchorStyles.None,
            AutoSize = true,
            Text = "Cancel",
            Visible = false,
        };
        cancelButton.Click += OnCancelClick;

        content.Controls.Add(messageLabel, 0, 0);
        content.Controls.Add(progressBar, 0, 1);
        content.Controls.Add(cancelButton, 0, 2);

        var host = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1,
        };
        host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        host.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        host.Controls.Add(content, 0, 0);
        Controls.Add(host);
    }

    /// <summary>Raised when the user asks the application to cancel the current operation.</summary>
    public event EventHandler? CancelRequested;

    /// <summary>Gets or sets the message displayed while busy.</summary>
    [Category("SASD")]
    [DefaultValue("Working…")]
    public string Message
    {
        get => messageLabel.Text;
        set
        {
            messageLabel.Text = value ?? string.Empty;
            if (Visible)
            {
                UpdateAccessibilityDescription();
            }
        }
    }

    /// <summary>Gets or sets whether the current busy operation can be cancelled by the user.</summary>
    /// <remarks>
    /// Setting this property controls only presentation and input. The application remains
    /// responsible for responding to <see cref="CancelRequested"/> and cancelling its own work.
    /// </remarks>
    [Category("SASD")]
    [DefaultValue(false)]
    public bool CancellationEnabled
    {
        get => cancellationEnabled;
        set
        {
            cancellationEnabled = value;
            cancelButton.Visible = value;

            if (Visible && !value && cancelButton.ContainsFocus)
            {
                Select();
            }
        }
    }

    /// <summary>
    /// Gets the current determinate percentage, or <see langword="null"/> while progress is indeterminate.
    /// </summary>
    [Browsable(false)]
    public int? ProgressPercentage => progressPercentage;

    /// <summary>Shows the overlay and brings it in front of sibling controls.</summary>
    /// <remarks>
    /// Each new busy operation starts in indeterminate mode. Call <see cref="SetProgress"/> after
    /// <see cref="BeginBusy"/> when the operation can report a meaningful percentage.
    /// </remarks>
    public void BeginBusy(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            Message = message;
        }

        if (!Visible)
        {
            previousFocus = CaptureFocusedControl();
        }

        SetIndeterminateCore();
        Visible = true;
        TabStop = true;
        BringToFront();
        UseWaitCursor = true;
        UpdateAccessibilityDescription();

        // Moving focus into the overlay is part of the blocking contract. It prevents keyboard
        // users from continuing to edit an obscured sibling control while pointer input is
        // already blocked by the overlay's visual surface.
        if (CancellationEnabled)
        {
            cancelButton.Select();
        }
        else
        {
            Select();
        }
    }

    /// <summary>Switches the current busy operation back to indeterminate progress.</summary>
    public void SetIndeterminate()
    {
        SetIndeterminateCore();
        if (Visible)
        {
            UpdateAccessibilityDescription();
        }
    }

    /// <summary>Displays determinate progress from 0 through 100 percent.</summary>
    public void SetProgress(int percentage)
    {
        if (percentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percentage), "Progress must be between 0 and 100 percent.");
        }

        progressPercentage = percentage;
        progressBar.Style = ProgressBarStyle.Blocks;
        progressBar.Value = percentage;
        progressBar.AccessibleDescription = $"{percentage} percent complete.";

        if (Visible)
        {
            UpdateAccessibilityDescription();
        }
    }

    /// <summary>Hides the overlay and restores the normal cursor and prior focus when possible.</summary>
    public void EndBusy()
    {
        UseWaitCursor = false;
        Visible = false;
        TabStop = false;
        AccessibleDescription = null;
        SetIndeterminateCore();

        Control? restoreTarget = previousFocus;
        previousFocus = null;
        if (restoreTarget is not null && !restoreTarget.IsDisposed && restoreTarget.CanSelect)
        {
            // Focus restoration is best-effort. The application may have removed or disabled the
            // old control while work was running, in which case WinForms should choose normally.
            restoreTarget.Select();
        }
    }

    /// <inheritdoc />
    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (Visible &&
            (keyData & Keys.KeyCode) == Keys.Tab &&
            (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
        {
            // The R1 overlay contains at most one interactive action. Keeping Tab/Shift+Tab on
            // that action (or the overlay itself) is sufficient to prevent focus from escaping
            // to obscured sibling controls without process-global keyboard hooks.
            if (CancellationEnabled)
            {
                cancelButton.Select();
            }
            else
            {
                Select();
            }

            return true;
        }

        return base.ProcessDialogKey(keyData);
    }

    /// <inheritdoc />
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (Visible &&
            (keyData & Keys.KeyCode) == Keys.Escape &&
            (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
        {
            // A busy surface must not let Escape fall through to a parent form's CancelButton.
            // When cancellation is enabled the same key becomes an explicit cooperative request.
            if (CancellationEnabled)
            {
                RaiseCancelRequested();
            }

            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            cancelButton.Click -= OnCancelClick;
            previousFocus = null;
        }

        base.Dispose(disposing);
    }

    private void OnCancelClick(object? sender, EventArgs e) => RaiseCancelRequested();

    private void RaiseCancelRequested()
    {
        if (!Visible || !CancellationEnabled)
        {
            return;
        }

        CancelRequested?.Invoke(this, EventArgs.Empty);
    }

    private void SetIndeterminateCore()
    {
        progressPercentage = null;
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.AccessibleDescription = "Progress is indeterminate.";
    }

    private void UpdateAccessibilityDescription()
    {
        string message = string.IsNullOrWhiteSpace(Message)
            ? "Application is busy."
            : Message.Trim();
        string progress = progressPercentage is int percentage
            ? $"{percentage} percent complete."
            : "Progress is indeterminate.";
        string cancellation = CancellationEnabled
            ? "Cancellation is available."
            : string.Empty;

        AccessibleDescription = $"{message} {progress} {cancellation}".Trim();
    }

    private Control? CaptureFocusedControl()
    {
        Form? form = FindForm();
        if (form is null)
        {
            return null;
        }

        Control? active = form.ActiveControl;
        while (active is ContainerControl container && container.ActiveControl is not null)
        {
            active = container.ActiveControl;
        }

        return active;
    }
}
