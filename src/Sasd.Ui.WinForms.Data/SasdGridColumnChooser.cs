using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Provides a reusable column-visibility chooser for a native <see cref="DataGridView"/>.
/// </summary>
/// <remarks>
/// <para>
/// The chooser does not own the grid or its columns. It only subscribes to column lifecycle/state
/// events while bound and removes those subscriptions again during <see cref="Unbind"/> or disposal.
/// </para>
/// <para>
/// Column order, width and persistence remain responsibilities of <see cref="SasdDataGridState"/>.
/// Keeping those concerns separate avoids turning this small R2 helper into an enterprise grid.
/// </para>
/// </remarks>
public sealed class SasdGridColumnChooser : UserControl
{
    private readonly TextBox filterTextBox;
    private readonly CheckedListBox columnList;
    private readonly Button showAllButton;
    private DataGridView? grid;
    private bool updating;

    /// <summary>Initialises a searchable column chooser.</summary>
    public SasdGridColumnChooser()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AccessibleName = "Grid column chooser";
        MinimumSize = new Size(220, 180);

        filterTextBox = new TextBox
        {
            AccessibleName = "Filter grid columns",
            Dock = DockStyle.Fill,
            PlaceholderText = "Filter columns...",
        };
        filterTextBox.TextChanged += OnFilterTextChanged;

        showAllButton = new Button
        {
            AccessibleName = "Show all grid columns",
            AutoSize = true,
            Text = "Show all",
        };
        showAllButton.Click += OnShowAllClick;

        var top = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 2,
            Dock = DockStyle.Top,
            Margin = Padding.Empty,
            RowCount = 1,
        };
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        top.Controls.Add(filterTextBox, 0, 0);
        top.Controls.Add(showAllButton, 1, 0);

        columnList = new CheckedListBox
        {
            AccessibleName = "Available grid columns",
            CheckOnClick = true,
            Dock = DockStyle.Fill,
            IntegralHeight = false,
        };
        columnList.ItemCheck += OnColumnItemCheck;

        Controls.Add(columnList);
        Controls.Add(top);
    }

    /// <summary>Gets the currently bound grid, or <see langword="null"/>.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridView? Grid => grid;

    /// <summary>Gets or sets the text used to filter the list of column names.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FilterText
    {
        get => filterTextBox.Text;
        set => filterTextBox.Text = value ?? string.Empty;
    }

    /// <summary>Binds the chooser to a grid without taking ownership of it.</summary>
    public void Bind(DataGridView targetGrid)
    {
        ArgumentNullException.ThrowIfNull(targetGrid);
        if (ReferenceEquals(grid, targetGrid))
        {
            RefreshColumns();
            return;
        }

        Unbind();
        grid = targetGrid;
        grid.ColumnAdded += OnGridColumnChanged;
        grid.ColumnRemoved += OnGridColumnChanged;
        grid.ColumnStateChanged += OnGridColumnStateChanged;
        grid.ColumnDisplayIndexChanged += OnGridColumnDisplayIndexChanged;
        RefreshColumns();
    }

    /// <summary>Removes event subscriptions from the current grid and clears the chooser.</summary>
    public void Unbind()
    {
        if (grid is not null)
        {
            grid.ColumnAdded -= OnGridColumnChanged;
            grid.ColumnRemoved -= OnGridColumnChanged;
            grid.ColumnStateChanged -= OnGridColumnStateChanged;
            grid.ColumnDisplayIndexChanged -= OnGridColumnDisplayIndexChanged;
            grid = null;
        }

        updating = true;
        try
        {
            columnList.Items.Clear();
        }
        finally
        {
            updating = false;
        }
    }

    /// <summary>Rebuilds the visible chooser entries from the current grid.</summary>
    public void RefreshColumns()
    {
        updating = true;
        try
        {
            columnList.Items.Clear();
            if (grid is null || grid.IsDisposed)
            {
                return;
            }

            IEnumerable<DataGridViewColumn> columns = grid.Columns
                .Cast<DataGridViewColumn>()
                .OrderBy(column => column.DisplayIndex);

            string filter = filterTextBox.Text.Trim();
            if (filter.Length > 0)
            {
                columns = columns.Where(column => MatchesFilter(column, filter));
            }

            foreach (DataGridViewColumn column in columns)
            {
                var item = new ColumnItem(column);
                columnList.Items.Add(item, column.Visible);
            }
        }
        finally
        {
            updating = false;
        }
    }

    /// <summary>Makes every column in the bound grid visible.</summary>
    public void ShowAllColumns()
    {
        if (grid is null || grid.IsDisposed)
        {
            return;
        }

        updating = true;
        try
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.Visible = true;
            }
        }
        finally
        {
            updating = false;
        }

        RefreshColumns();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Unbind();
            filterTextBox.TextChanged -= OnFilterTextChanged;
            showAllButton.Click -= OnShowAllClick;
            columnList.ItemCheck -= OnColumnItemCheck;
        }

        base.Dispose(disposing);
    }

    private void OnFilterTextChanged(object? sender, EventArgs e) => RefreshColumns();

    private void OnShowAllClick(object? sender, EventArgs e) => ShowAllColumns();

    private void OnColumnItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (updating || grid is null || e.Index < 0 || e.Index >= columnList.Items.Count)
        {
            return;
        }

        if (columnList.Items[e.Index] is ColumnItem item)
        {
            item.Column.Visible = e.NewValue == CheckState.Checked;
        }
    }

    private void OnGridColumnChanged(object? sender, DataGridViewColumnEventArgs e)
    {
        if (!updating)
        {
            RefreshColumns();
        }
    }

    private void OnGridColumnStateChanged(object? sender, DataGridViewColumnStateChangedEventArgs e)
    {
        if (!updating && (e.StateChanged & DataGridViewElementStates.Visible) != 0)
        {
            RefreshColumns();
        }
    }

    private void OnGridColumnDisplayIndexChanged(object? sender, DataGridViewColumnEventArgs e)
    {
        if (!updating)
        {
            RefreshColumns();
        }
    }

    private static bool MatchesFilter(DataGridViewColumn column, string filter) =>
        Contains(column.HeaderText, filter) || Contains(column.Name, filter) || Contains(column.DataPropertyName, filter);

    private static bool Contains(string? value, string filter) =>
        !string.IsNullOrEmpty(value) && value.Contains(filter, StringComparison.CurrentCultureIgnoreCase);

    private sealed class ColumnItem
    {
        public ColumnItem(DataGridViewColumn column) => Column = column;

        public DataGridViewColumn Column { get; }

        public override string ToString() => string.IsNullOrWhiteSpace(Column.HeaderText)
            ? Column.Name
            : Column.HeaderText;
    }
}
