using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>Event data for a validation-summary message activation.</summary>
public sealed class SasdValidationMessageInvokedEventArgs : EventArgs
{
    /// <summary>Initialises validation-message invocation data.</summary>
    public SasdValidationMessageInvokedEventArgs(SasdValidationMessage message, bool focusMoved)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        FocusMoved = focusMoved;
    }

    /// <summary>Gets the validation message activated by the user.</summary>
    public SasdValidationMessage Message { get; }

    /// <summary>Gets whether the bound coordinator moved focus to the registered field.</summary>
    public bool FocusMoved { get; }
}

/// <summary>Displays a compact summary of validation messages with optional field navigation.</summary>
/// <remarks>
/// The summary can bind to a <see cref="SasdValidationCoordinator"/> for automatic result display
/// and focus navigation. The coordinator remains application-owned; the summary owns only its
/// event subscription and never disposes the coordinator.
/// </remarks>
public class SasdValidationSummary : UserControl
{
    private readonly Label headingLabel;
    private readonly ListBox messageList;
    private readonly Font headingFont;
    private SasdValidationCoordinator? coordinator;

    /// <summary>Initialises the validation summary.</summary>
    public SasdValidationSummary()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(12);
        Visible = false;
        AccessibleRole = AccessibleRole.Alert;
        AccessibleName = "Validation summary";

        // The summary creates this emphasis font itself. Treating it as an owned
        // resource keeps repeated form open/close cycles from depending on implicit
        // child-control behavior for disposal of a GDI object we allocated.
        headingFont = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold);
        headingLabel = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            Font = headingFont,
            Text = "Please review the following fields:",
        };

        messageList = new ListBox
        {
            AccessibleName = "Validation messages",
            AccessibleDescription = "Press Enter or double-click a validation message to navigate to its field when navigation is available.",
            DisplayMember = nameof(SasdValidationMessage.Message),
            Dock = DockStyle.Top,
            IntegralHeight = true,
            Height = 96,
            Margin = new Padding(0, 8, 0, 0),
        };
        messageList.DoubleClick += OnMessageListDoubleClick;
        messageList.KeyDown += OnMessageListKeyDown;

        Controls.Add(messageList);
        Controls.Add(headingLabel);
    }

    /// <summary>Raised when the user activates a validation message.</summary>
    /// <remarks>
    /// When a coordinator is bound, focus navigation is attempted before this event is raised.
    /// Applications may also handle this event without binding a coordinator to implement a
    /// custom navigation policy using the stable <see cref="SasdValidationMessage.FieldKey"/>.
    /// </remarks>
    public event EventHandler<SasdValidationMessageInvokedEventArgs>? MessageInvoked;

    /// <summary>Gets or sets the summary heading.</summary>
    [Category("SASD")]
    [DefaultValue("Please review the following fields:")]
    public string Heading
    {
        get => headingLabel.Text;
        set
        {
            headingLabel.Text = value ?? string.Empty;
            UpdateAccessibilityDescription();
        }
    }

    /// <summary>
    /// Binds the summary to a coordinator for automatic completed-result display and field focus navigation.
    /// </summary>
    /// <remarks>
    /// Any previous coordinator is detached first. Ownership does not transfer; callers remain
    /// responsible for disposing the coordinator after the summary has been disposed or unbound.
    /// </remarks>
    public void Bind(SasdValidationCoordinator validationCoordinator)
    {
        ArgumentNullException.ThrowIfNull(validationCoordinator);
        if (ReferenceEquals(coordinator, validationCoordinator))
        {
            return;
        }

        Unbind();
        coordinator = validationCoordinator;
        coordinator.ValidationCompleted += OnValidationCompleted;
    }

    /// <summary>Detaches the bound coordinator without clearing the currently visible result.</summary>
    public void Unbind()
    {
        if (coordinator is null)
        {
            return;
        }

        coordinator.ValidationCompleted -= OnValidationCompleted;
        coordinator = null;
    }

    /// <summary>Displays the supplied validation result.</summary>
    public void ShowResult(SasdValidationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        messageList.BeginUpdate();
        try
        {
            messageList.Items.Clear();
            foreach (SasdValidationMessage message in result.Messages)
            {
                // Retain the full message object instead of only its display text. The stable
                // FieldKey and severity are needed for keyboard/mouse activation and custom
                // application navigation without maintaining a second parallel lookup table.
                messageList.Items.Add(message);
            }

            messageList.SelectedIndex = messageList.Items.Count > 0 ? 0 : -1;
        }
        finally
        {
            messageList.EndUpdate();
        }

        Visible = result.Messages.Count > 0;
        UpdateAccessibilityDescription();
    }

    /// <summary>Clears and hides the summary.</summary>
    public void Clear()
    {
        messageList.Items.Clear();
        Visible = false;
        AccessibleDescription = null;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Unbind();
            messageList.DoubleClick -= OnMessageListDoubleClick;
            messageList.KeyDown -= OnMessageListKeyDown;

            // headingFont is private to this composite control and can therefore be
            // released deterministically without affecting application-owned fonts.
            headingFont.Dispose();
        }

        base.Dispose(disposing);
    }

    private void OnValidationCompleted(object? sender, SasdValidationResult result) => ShowResult(result);

    private void OnMessageListDoubleClick(object? sender, EventArgs e) => InvokeSelectedMessage();

    private void OnMessageListKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
        {
            return;
        }

        InvokeSelectedMessage();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void InvokeSelectedMessage()
    {
        if (messageList.SelectedItem is not SasdValidationMessage message)
        {
            return;
        }

        bool focusMoved = coordinator?.TryFocusField(message.FieldKey) ?? false;
        MessageInvoked?.Invoke(this, new SasdValidationMessageInvokedEventArgs(message, focusMoved));
    }

    private void UpdateAccessibilityDescription()
    {
        if (messageList.Items.Count == 0)
        {
            AccessibleDescription = null;
            return;
        }

        // Keep accessible text derived from the same full message objects shown in the list.
        // Include severity because color or ErrorProvider glyphs must never be the only channel
        // that distinguishes informational guidance, warnings and blocking errors.
        var messages = new string[messageList.Items.Count];
        for (int index = 0; index < messageList.Items.Count; index++)
        {
            messages[index] = messageList.Items[index] is SasdValidationMessage message
                ? $"{message.Severity}: {message.Message}"
                : string.Empty;
        }

        AccessibleDescription = $"{headingLabel.Text} {string.Join(" ", messages)}".Trim();
    }
}
