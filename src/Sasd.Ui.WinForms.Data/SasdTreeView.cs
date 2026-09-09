namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Provides conservative defaults for hierarchical navigation and data views.
/// Node creation, lazy loading and domain semantics remain application-owned.
/// </summary>
public class SasdTreeView : TreeView
{
    /// <summary>Initialises a SASD tree view.</summary>
    public SasdTreeView()
    {
        HideSelection = false;
        FullRowSelect = true;
        ShowNodeToolTips = true;
        DoubleBuffered = true;
    }
}
