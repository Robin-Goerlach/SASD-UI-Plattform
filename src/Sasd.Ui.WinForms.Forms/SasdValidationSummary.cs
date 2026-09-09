using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>Displays a compact summary of validation messages.</summary>
public class SasdValidationSummary : UserControl
{
    private readonly Label headingLabel;
    private readonly ListBox messageList;

    /// <summary>Initialises the validation summary.</summary>
    public SasdValidationSummary()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(12);
        Visible = false;

        headingLabel = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            Font = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold),
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
        set => headingLabel.Text = value;
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
    }

    /// <summary>Clears and hides the summary.</summary>
    public void Clear()
    {
        messageList.Items.Clear();
        Visible = false;
    }
}
