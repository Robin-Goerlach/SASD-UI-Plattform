using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Displays a consistent empty state with a title, explanatory text and an
/// optional action button.
/// </summary>
[DefaultEvent(nameof(ActionInvoked))]
public class SasdEmptyState : UserControl
{
    private readonly Label titleLabel;
    private readonly Label messageLabel;
    private readonly Button actionButton;

    /// <summary>Initialises the empty-state control.</summary>
    public SasdEmptyState()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(240, 150);
        Padding = new Padding(24);

        var layout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Dock = DockStyle.Top,
            RowCount = 3,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        Font prototype = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
        titleLabel = new Label
        {
            AutoSize = true,
            Font = new Font(prototype, FontStyle.Bold),
            Text = "Nothing here yet",
        };
        messageLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(520, 0),
            Margin = new Padding(0, 8, 0, 16),
            Text = "There are no items to display.",
        };
        actionButton = new Button
        {
            AutoSize = true,
            MinimumSize = new Size(110, 30),
            Text = "Create item",
        };
        actionButton.Click += (_, _) => ActionInvoked?.Invoke(this, EventArgs.Empty);

        layout.Controls.Add(titleLabel, 0, 0);
        layout.Controls.Add(messageLabel, 0, 1);
        layout.Controls.Add(actionButton, 0, 2);
        Controls.Add(layout);
    }

    /// <summary>Occurs when the optional action is invoked.</summary>
    public event EventHandler? ActionInvoked;

    /// <summary>Gets or sets the empty-state title.</summary>
    [Category("SASD")]
    [DefaultValue("Nothing here yet")]
    public string Title
    {
        get => titleLabel.Text;
        set => titleLabel.Text = value ?? string.Empty;
    }

    /// <summary>Gets or sets the explanatory message.</summary>
    [Category("SASD")]
    [DefaultValue("There are no items to display.")]
    public string Message
    {
        get => messageLabel.Text;
        set => messageLabel.Text = value ?? string.Empty;
    }

    /// <summary>Gets or sets the action-button text.</summary>
    [Category("SASD")]
    [DefaultValue("Create item")]
    public string ActionText
    {
        get => actionButton.Text;
        set => actionButton.Text = value ?? string.Empty;
    }

    /// <summary>Gets or sets whether the action button is visible.</summary>
    [Category("SASD")]
    [DefaultValue(true)]
    public bool ActionVisible
    {
        get => actionButton.Visible;
        set => actionButton.Visible = value;
    }
}
