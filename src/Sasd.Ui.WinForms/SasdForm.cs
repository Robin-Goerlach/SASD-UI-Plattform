using System.ComponentModel;

namespace Sasd.Ui.WinForms;

/// <summary>
/// Provides the deliberately small base class for SASD application windows.
/// Cross-cutting services are composed by the application rather than resolved
/// through a global service locator.
/// </summary>
public class SasdForm : Form
{
    /// <summary>Initialises a designer-safe SASD form.</summary>
    public SasdForm()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        KeyPreview = true;
        StartPosition = FormStartPosition.CenterParent;
    }

    /// <summary>
    /// Gets or sets the stable key used by an application to persist safe UI
    /// state such as bounds and window state.
    /// </summary>
    [Category("SASD")]
    [DefaultValue(null)]
    [Description("Stable application-owned key for versioned UI-state persistence.")]
    public string? StateKey { get; set; }
}
