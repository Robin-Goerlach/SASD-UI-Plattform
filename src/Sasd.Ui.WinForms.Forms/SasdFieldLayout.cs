using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>
/// Provides a simple two-column label/editor layout for common business forms.
/// It remains a normal TableLayoutPanel and can therefore be extended with
/// native WinForms controls when an application needs an exception.
/// </summary>
/// <remarks>
/// Editors passed to <see cref="AddField"/> become normal child controls of this
/// layout. Standard WinForms ownership therefore applies: disposing the layout
/// also disposes editors that are still parented to it.
/// </remarks>
public class SasdFieldLayout : TableLayoutPanel
{
    private const int DefaultFieldGap = 8;
    private readonly List<Label> fieldLabels = [];
    private int fieldGap = DefaultFieldGap;

    /// <summary>Initialises an empty field layout.</summary>
    public SasdFieldLayout()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        ColumnCount = 2;
        RowCount = 0;
        Dock = DockStyle.Top;
        GrowStyle = TableLayoutPanelGrowStyle.AddRows;
        Padding = Padding.Empty;
        AccessibleRole = AccessibleRole.Grouping;
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
    }

    /// <summary>Gets or sets the non-negative horizontal gap between labels and editors.</summary>
    /// <remarks>
    /// Changing the gap updates rows previously created by <see cref="AddField"/> as well as
    /// rows added later. Controls manually inserted into the underlying TableLayoutPanel are
    /// deliberately left untouched.
    /// </remarks>
    [Category("SASD")]
    [DefaultValue(DefaultFieldGap)]
    public int FieldGap
    {
        get => fieldGap;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            if (fieldGap == value)
            {
                return;
            }

            fieldGap = value;
            foreach (Label label in fieldLabels)
            {
                // Only labels created by AddField are tracked here. Applications remain free
                // to add exceptional rows without the platform later rewriting their margins.
                label.Margin = new Padding(0, 7, fieldGap, 7);
            }
        }
    }

    /// <summary>Adds one labelled editor row and returns the label owned by the layout.</summary>
    /// <param name="labelText">Human-readable field label without a required marker.</param>
    /// <param name="editor">
    /// Editor control to parent into the second column. Existing accessibility text supplied
    /// by the application is preserved; missing text receives a conservative label-derived default.
    /// </param>
    /// <param name="required">Whether the field should be presented as required.</param>
    public Label AddField(string labelText, Control editor, bool required = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(labelText);
        ArgumentNullException.ThrowIfNull(editor);

        string normalizedLabel = labelText.Trim();
        int row = RowCount;
        RowCount++;
        RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var label = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 7, FieldGap, 7),
            Text = required ? $"{normalizedLabel} *" : normalizedLabel,
            AccessibleName = normalizedLabel,
            AccessibleDescription = required ? "Required field." : null,
        };

        // Asterisk-only required markers are useful visually but are not sufficient for
        // assistive technology. Supply a text default only when the application has not
        // already provided a more specific accessible name/description for the editor.
        if (string.IsNullOrWhiteSpace(editor.AccessibleName))
        {
            editor.AccessibleName = normalizedLabel;
        }

        if (required && string.IsNullOrWhiteSpace(editor.AccessibleDescription))
        {
            editor.AccessibleDescription = "Required field.";
        }

        editor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        editor.Margin = new Padding(0, 4, 0, 4);
        if (editor.MinimumSize.Height == 0)
        {
            editor.MinimumSize = new Size(editor.MinimumSize.Width, 28);
        }

        fieldLabels.Add(label);
        Controls.Add(label, 0, row);
        Controls.Add(editor, 1, row);
        return label;
    }
}
