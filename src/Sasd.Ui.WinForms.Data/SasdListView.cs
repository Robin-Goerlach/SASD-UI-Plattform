namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// ListView with conservative defaults for business-style row selection.
/// </summary>
/// <remarks>
/// Native WinForms capabilities such as <see cref="ListView.VirtualMode"/>,
/// <see cref="ListView.RetrieveVirtualItem"/>, view modes and context menus remain directly
/// available. The platform deliberately does not wrap those mature contracts with a second
/// data-source abstraction when the native API is sufficient.
/// </remarks>
public class SasdListView : ListView
{
    /// <summary>Initialises the list view.</summary>
    public SasdListView()
    {
        // These defaults avoid several common per-application setup blocks while
        // leaving columns, images, multi-selection, virtual retrieval and activation
        // policy to callers.
        DoubleBuffered = true;
        FullRowSelect = true;
        HideSelection = false;
        MultiSelect = false;
        UseCompatibleStateImageBehavior = false;
        View = View.Details;

        AccessibleRole = AccessibleRole.List;
        AccessibleName = "Items";
        AccessibleDescription = "Application items. Use the application-provided columns, selection and context actions.";
    }
}
