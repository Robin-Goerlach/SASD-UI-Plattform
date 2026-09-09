using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>
/// Provides a simple two-column label/editor layout for common business forms.
/// It remains a normal TableLayoutPanel and can therefore be extended with
/// native WinForms controls when an application needs an exception.
/// </summary>
public class SasdFieldLayout : TableLayoutPanel
{
    /// <summary>Initialises an empty field layout.</summary>
    public SasdFieldLayout()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        ColumnCount = 2;
        RowCount = 0;
        Dock = DockStyle.Top;
        GrowStyle = TableLayoutPanelGrowStyle.AddRows;
        Padding = new Padding(0);
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
    }

    /// <summary>Gets or sets the horizontal gap between labels and editors.</summary>
    [Category("SASD")]
    [DefaultValue(8)]
    public int FieldGap { get; set; } = 8;

    /// <summary>Adds one labelled editor row and returns the created label.</summary>
    public Label AddField(string labelText, Control editor, bool required = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(labelText);
        ArgumentNullException.ThrowIfNull(editor);

        int row = RowCount;
        RowCount++;
        RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var label = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 7, FieldGap, 7),
            Text = required ? $"{labelText} *" : labelText,
        };

        editor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        editor.Margin = new Padding(0, 4, 0, 4);
        if (editor.MinimumSize.Height == 0)
        {
            editor.MinimumSize = new Size(editor.MinimumSize.Width, 28);
        }

        Controls.Add(label, 0, row);
        Controls.Add(editor, 1, row);
        return label;
    }
}
