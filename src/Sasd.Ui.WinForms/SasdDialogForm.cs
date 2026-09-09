namespace Sasd.Ui.WinForms;

/// <summary>
/// Provides conservative defaults for modal SASD dialogs while keeping normal
/// WinForms composition and designer support available to the application.
/// </summary>
public class SasdDialogForm : SasdForm
{
    /// <summary>Initialises a designer-safe dialog form.</summary>
    public SasdDialogForm()
    {
        // A modal dialog belongs to its owner window and normally should not create
        // another taskbar entry. We deliberately keep the border resizable because
        // business dialogs can contain grids, validation details or translated text.
        ShowInTaskbar = false;
        MinimizeBox = false;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
    }
}
