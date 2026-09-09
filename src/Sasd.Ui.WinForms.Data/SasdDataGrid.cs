using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Provides conservative business-application defaults on top of the native
/// DataGridView while keeping the full underlying WinForms API available.
/// </summary>
public class SasdDataGrid : DataGridView
{
    /// <summary>Initialises a data grid with SASD defaults.</summary>
    public SasdDataGrid()
    {
        AutoGenerateColumns = true;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        BackgroundColor = SystemColors.Window;
        BorderStyle = BorderStyle.FixedSingle;
        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        RowHeadersVisible = false;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        MultiSelect = false;
        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        AllowUserToOrderColumns = true;
        ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
        DoubleBuffered = true;
    }

    /// <summary>
    /// Gets or sets whether columns fill the available width. Applications may
    /// disable this for wide operational grids that require horizontal scrolling.
    /// </summary>
    [Category("SASD")]
    [DefaultValue(true)]
    public bool FillAvailableWidth
    {
        get => AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill;
        set => AutoSizeColumnsMode = value
            ? DataGridViewAutoSizeColumnsMode.Fill
            : DataGridViewAutoSizeColumnsMode.None;
    }
}
