using Sasd.Ui.WinForms;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>
/// Small application-owned dialog used to demonstrate <see cref="SasdDialogForm"/>.
/// </summary>
/// <remarks>
/// The UI Platform supplies safe dialog-window defaults, while the consuming application
/// still owns the actual fields, validation rules, buttons and result interpretation.
/// </remarks>
internal sealed class ShowcaseInputDialog : SasdDialogForm
{
    private readonly TextBox valueTextBox = new();

    /// <summary>Creates the demonstration dialog.</summary>
    public ShowcaseInputDialog()
    {
        Text = "SasdDialogForm example";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(440, 230);
        ClientSize = new Size(500, 250);

        valueTextBox.AccessibleName = "Example value";
        valueTextBox.Dock = DockStyle.Top;
        valueTextBox.Text = "Editable application-owned value";

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(430, 0),
            Text =
                "This window derives from SasdDialogForm. The base class supplies safe dialog defaults; " +
                "this example owns the content and result handling.",
        };

        var okButton = new Button
        {
            AutoSize = true,
            DialogResult = DialogResult.OK,
            Text = "OK",
        };
        var cancelButton = new Button
        {
            AutoSize = true,
            DialogResult = DialogResult.Cancel,
            Text = "Cancel",
        };

        AcceptButton = okButton;
        CancelButton = cancelButton;

        var buttons = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 12, 0, 0),
            WrapContents = false,
        };
        buttons.Controls.Add(okButton);
        buttons.Controls.Add(cancelButton);

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            RowCount = 4,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(new Label { AutoSize = true, Padding = new Padding(0, 14, 0, 4), Text = "Value" }, 0, 1);
        layout.Controls.Add(valueTextBox, 0, 2);
        layout.Controls.Add(buttons, 0, 3);
        Controls.Add(layout);
    }

    /// <summary>Gets the trimmed value entered by the user.</summary>
    public string Value => valueTextBox.Text.Trim();
}
