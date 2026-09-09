namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// TreeView with stable selection and rendering defaults for SASD navigation and data trees.
/// </summary>
public class SasdTreeView : TreeView
{
    /// <summary>Initialises the tree view.</summary>
    public SasdTreeView()
    {
        DoubleBuffered = true;
        HideSelection = false;
        ShowNodeToolTips = true;
    }
}
