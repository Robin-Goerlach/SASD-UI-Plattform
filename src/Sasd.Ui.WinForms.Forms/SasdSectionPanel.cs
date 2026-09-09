using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>
/// Groups related controls into a visually and semantically consistent section.
/// The initial implementation intentionally builds on the native GroupBox so it
/// remains predictable in the WinForms Designer.
/// </summary>
[DefaultProperty(nameof(SectionTitle))]
public class SasdSectionPanel : GroupBox
{
    /// <summary>Initialises a section with SASD spacing defaults.</summary>
    public SasdSectionPanel()
    {
        Padding = new Padding(12, 20, 12, 12);
        Margin = new Padding(0, 0, 0, 12);
        TabStop = false;
    }

    /// <summary>Gets or sets the title shown for the section.</summary>
    [Category("SASD")]
    [Description("Human-readable heading for the section.")]
    [DefaultValue("")]
    public string SectionTitle
    {
        get => Text;
        set => Text = value ?? string.Empty;
    }
}
