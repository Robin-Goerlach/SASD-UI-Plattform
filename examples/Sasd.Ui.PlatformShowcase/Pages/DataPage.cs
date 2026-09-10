using System.ComponentModel;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>
/// Demonstrates search, active filters, DataGrid defaults, CSV export, the column chooser and saved views.
/// </summary>
internal sealed class DataPage : UserControl
{
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly List<DemoCustomer> allCustomers = CreateCustomers();
    private readonly BindingSource bindingSource = new();
    private readonly SasdSearchBox searchBox = new();
    private readonly SasdFilterBar filterBar = new();
    private readonly SasdDataGrid grid = new();
    private readonly SasdGridColumnChooser columnChooser = new();
    private readonly ComboBox statusFilter = new();
    private readonly Label resultLabel = new();
    private readonly TextBox csvPreview = new();
    private SasdGridViewDefinition? savedView;

    public DataPage(Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        ConfigureGrid();
        ConfigureFilterControls();
        columnChooser.Bind(grid);

        csvPreview.AccessibleName = "CSV export preview";
        csvPreview.Dock = DockStyle.Fill;
        csvPreview.Multiline = true;
        csvPreview.ReadOnly = true;
        csvPreview.ScrollBars = ScrollBars.Both;
        csvPreview.WordWrap = false;
        csvPreview.Text = "Press Preview CSV to export the currently visible grid rows and columns to memory.";

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "Data and grid composition\r\n\r\n" +
                "Search and status filters are application logic; the UI Platform provides predictable controls and neutral state. " +
                "Hide columns with the chooser, preview a CSV export, save the current view, change the layout/search/filter, then restore the saved view.",
        };

        var saveViewButton = new Button { AutoSize = true, Text = "Save view" };
        saveViewButton.Click += (_, _) => SaveView();
        var restoreViewButton = new Button { AutoSize = true, Text = "Restore view" };
        restoreViewButton.Click += (_, _) => RestoreView();
        var clearButton = new Button { AutoSize = true, Text = "Clear search/filter" };
        clearButton.Click += (_, _) => ClearSearchAndFilter();
        var csvButton = new Button { AutoSize = true, Text = "Preview CSV" };
        csvButton.Click += OnPreviewCsvClick;

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 8, 0, 8),
        };
        actions.Controls.Add(saveViewButton);
        actions.Controls.Add(restoreViewButton);
        actions.Controls.Add(clearButton);
        actions.Controls.Add(csvButton);
        actions.Controls.Add(new Label { AutoSize = true, Margin = new Padding(14, 7, 4, 0), Text = "Status:" });
        actions.Controls.Add(statusFilter);
        actions.Controls.Add(resultLabel);

        var gridArea = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel2,
            Panel1MinSize = 480,
            Panel2MinSize = 220,
            SplitterDistance = 700,
        };
        gridArea.Panel1.Controls.Add(grid);
        gridArea.Panel2.Padding = new Padding(12, 0, 0, 0);
        gridArea.Panel2.Controls.Add(columnChooser);
        columnChooser.Dock = DockStyle.Fill;

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 6,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(searchBox, 0, 1);
        layout.Controls.Add(filterBar, 0, 2);
        layout.Controls.Add(actions, 0, 3);
        layout.Controls.Add(csvPreview, 0, 4);
        layout.Controls.Add(gridArea, 0, 5);
        Controls.Add(layout);

        ApplyFilter();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            searchBox.SearchTextChanged -= OnSearchChanged;
            filterBar.FiltersChanged -= OnFiltersChanged;
            statusFilter.SelectedIndexChanged -= OnStatusFilterChanged;
            columnChooser.Unbind();
            bindingSource.Dispose();
        }

        base.Dispose(disposing);
    }

    private void ConfigureGrid()
    {
        grid.AutoGenerateColumns = false;
        grid.Dock = DockStyle.Fill;
        grid.FillAvailableWidth = true;
        grid.ReadOnly = true;
        grid.DataSource = bindingSource;
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(DemoCustomer.Name),
            HeaderText = "Name",
            Name = "name",
            FillWeight = 30,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(DemoCustomer.Email),
            HeaderText = "Email",
            Name = "email",
            FillWeight = 40,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(DemoCustomer.Status),
            HeaderText = "Status",
            Name = "status",
            FillWeight = 18,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(DemoCustomer.Score),
            HeaderText = "Score",
            Name = "score",
            FillWeight = 12,
        });
    }

    private void ConfigureFilterControls()
    {
        searchBox.Dock = DockStyle.Top;
        searchBox.PlaceholderText = "Search name, email or status...";
        searchBox.SearchTextChanged += OnSearchChanged;
        filterBar.FiltersChanged += OnFiltersChanged;

        statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        statusFilter.Items.AddRange(["All", "Active", "Paused", "Archived"]);
        statusFilter.SelectedIndex = 0;
        statusFilter.SelectedIndexChanged += OnStatusFilterChanged;
    }

    private void OnSearchChanged(object? sender, EventArgs e) => ApplyFilter();

    private void OnFiltersChanged(object? sender, EventArgs e) => ApplyFilter();

    private void OnStatusFilterChanged(object? sender, EventArgs e)
    {
        string selected = statusFilter.SelectedItem?.ToString() ?? "All";
        if (string.Equals(selected, "All", StringComparison.Ordinal))
        {
            filterBar.RemoveFilter("status");
        }
        else
        {
            filterBar.AddOrUpdateFilter("status", "Status", selected);
        }
    }

    private void ApplyFilter()
    {
        string search = searchBox.SearchText.Trim();
        string? status = filterBar.ActiveFilters
            .FirstOrDefault(filter => string.Equals(filter.Key, "status", StringComparison.OrdinalIgnoreCase))
            ?.Value;

        IEnumerable<DemoCustomer> query = allCustomers;
        if (search.Length > 0)
        {
            query = query.Where(customer =>
                customer.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                customer.Email.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                customer.Status.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(customer => string.Equals(customer.Status, status, StringComparison.OrdinalIgnoreCase));
        }

        var visible = new BindingList<DemoCustomer>(query.ToList());
        bindingSource.DataSource = visible;
        resultLabel.Text = $"{visible.Count} row(s)";
    }

    private void SaveView()
    {
        savedView = SasdGridViewDefinition.Capture("Showcase view", grid, searchBox, filterBar);
        publishStatus("Grid view captured in memory.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    private void RestoreView()
    {
        if (savedView is null)
        {
            publishStatus("Save a grid view first.", SasdStatusSeverity.Warning, TimeSpan.FromSeconds(4));
            return;
        }

        savedView.Restore(grid, searchBox, filterBar);
        ApplyFilter();
        publishStatus("Saved grid view restored.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    private void ClearSearchAndFilter()
    {
        searchBox.SearchText = string.Empty;
        filterBar.ClearFilters();
        statusFilter.SelectedIndex = 0;
        ApplyFilter();
    }

    private async void OnPreviewCsvClick(object? sender, EventArgs e)
    {
        Button? button = sender as Button;
        if (button is not null)
        {
            button.Enabled = false;
        }

        try
        {
            await using var stream = new MemoryStream();
            await SasdCsvExporter.ExportAsync(grid, stream);
            stream.Position = 0;
            using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
            csvPreview.Text = await reader.ReadToEndAsync();
            publishStatus(
                $"CSV preview generated from {grid.Rows.Cast<DataGridViewRow>().Count(row => !row.IsNewRow)} visible row(s).",
                SasdStatusSeverity.Success,
                TimeSpan.FromSeconds(4));
        }
        finally
        {
            if (button is not null && !button.IsDisposed)
            {
                button.Enabled = true;
            }
        }
    }

    private static List<DemoCustomer> CreateCustomers() =>
    [
        new("Ada Lovelace", "ada@example.test", "Active", 96),
        new("Grace Hopper", "grace@example.test", "Active", 93),
        new("Edsger Dijkstra", "edsger@example.test", "Paused", 88),
        new("Margaret Hamilton", "margaret@example.test", "Active", 95),
        new("Barbara Liskov", "barbara@example.test", "Archived", 91),
        new("Donald Knuth", "donald@example.test", "Paused", 90),
        new("Frances Allen", "frances@example.test", "Active", 89),
        new("Niklaus Wirth", "niklaus@example.test", "Archived", 87),
    ];
}
