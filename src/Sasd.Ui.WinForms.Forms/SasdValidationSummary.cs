using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>Displays a compact summary of validation messages.</summary>
public class SasdValidationSummary : UserControl
{
    private readonly Label headingLabel;
    private readonly ListBox messageList;
    private readonly Font headingFont;

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
            Dock = DockStyle.Top,
            IntegralHeight = true,
            Height = 96,
            Margin = new Padding(0, 8, 0, 0),
        };

        Controls.Add(messageList);
        Controls.Add(headingLabel);
    }

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

    /// <summary>Displays the supplied validation result.</summary>
    public void ShowResult(SasdValidationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        messageList.BeginUpdate();
        try
        {
            messageList.Items.Clear();
            foreach (var message in result.Messages)
            {
                messageList.Items.Add(message.Message);
            }
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
            // headingFont is private to this composite control and can therefore be
            // released deterministically without affecting application-owned fonts.
            headingFont.Dispose();
        }

        base.Dispose(disposing);
    }

    private void UpdateAccessibilityDescription()
    {
        if (messageList.Items.Count == 0)
        {
            AccessibleDescription = null;
            return;
        }

        // Keep the accessible text derived from the same messages that are visible.
        // This is intentionally a plain summary, not a claim of live-region/UIA support;
        // formal screen-reader behavior still belongs to manual/UIA acceptance work.
        var messages = new string[messageList.Items.Count];
        for (int index = 0; index < messageList.Items.Count; index++)
        {
            messages[index] = Convert.ToString(messageList.Items[index]) ?? string.Empty;
        }

        AccessibleDescription = $"{headingLabel.Text} {string.Join(" ", messages)}".Trim();
    }
}
