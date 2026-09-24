namespace Sasd.Ui.WinForms;

/// <summary>
/// Base class for reusable, designer-friendly SASD WinForms views.
/// </summary>
public class SasdUserControl : UserControl
{
    /// <summary>Initialises a DPI-aware reusable view.</summary>
    public SasdUserControl()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
    }
}
