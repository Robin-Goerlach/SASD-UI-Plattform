namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Provides readable, keyboard-friendly defaults for list-based business views.
/// Applications still own columns, items, images and domain-specific interaction.
/// </summary>
public class SasdListView : ListView
{
    /// <summary>Initialises a SASD list view.</summary>
    public SasdListView()
    {
        View = View.Details;
        FullRowSelect = true;
        HideSelection = false;
        MultiSelect = false;
        UseCompatibleStateImageBehavior = false;
        DoubleBuffered = true;
    }
}
