namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// ListView with conservative defaults for business-style row selection.
/// </summary>
public class SasdListView : ListView
{
    /// <summary>Initialises the list view.</summary>
    public SasdListView()
    {
        // These defaults avoid several common per-application setup blocks while
        // leaving columns, images, multi-selection and activation policy to callers.
        DoubleBuffered = true;
        FullRowSelect = true;
        HideSelection = false;
        MultiSelect = false;
        UseCompatibleStateImageBehavior = false;
        View = View.Details;
    }
}
