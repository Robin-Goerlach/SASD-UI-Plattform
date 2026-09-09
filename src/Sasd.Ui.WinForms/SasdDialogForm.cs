namespace Sasd.Ui.WinForms;

/// <summary>
/// Base window for modal or dialog-like SASD workflows.
/// </summary>
/// <remarks>
/// The class deliberately adds only safe dialog defaults. Applications still own
/// their buttons, validation, business rules and result handling. Keeping this
/// class small also keeps it friendly to the WinForms designer.
/// </remarks>
public class SasdDialogForm : SasdForm
{
    /// <summary>Initialises a designer-safe dialog form.</summary>
    public SasdDialogForm()
    {
        // Dialogs normally should not create a second taskbar entry and should not
        // expose minimise/maximise commands. We intentionally keep the form resizable;
        // fixed-size dialogs are often problematic with DPI scaling or localisation.
        ShowInTaskbar = false;
        MinimizeBox = false;
        MaximizeBox = false;
    }
}
