using System.Reflection;
using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.DataDashboardSmokeChecks;

internal static class Program
{
    [STAThread]
    private static async Task<int> Main()
    {
        try
        {
            ValidateFilterBatchReplacement();
            ValidateSavedGridView();
            ValidatePager();
            await ValidateGridControllerAsync();
            GridControllerResilienceChecks.Run();
            ValidateSparkline();
            ValidateKpiCard();

            Console.WriteLine("SASD data/dashboard smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateFilterBatchReplacement()
    {
        using var filterBar = new SasdFilterBar();
        int changes = 0;
        filterBar.FiltersChanged += (_, _) => changes++;

        filterBar.SetFilters([
            SasdActiveFilter.Create("status", "Status", "Active"),
            SasdActiveFilter.Create("region", "Region", "EU"),
            SasdActiveFilter.Create("STATUS", "Status", "Inactive"),
        ]);

        Ensure(changes == 1, "Batch filter replacement raised more than one change notification.");
        Ensure(filterBar.FilterCount == 2, "Batch filter replacement did not de-duplicate keys case-insensitively.");
        Ensure(filterBar.ActiveFilters[0].Value == "Inactive", "Later duplicate filter value did not replace the earlier value.");
    }

    private static void ValidateSavedGridView()
    {
        using var grid = new SasdDataGrid
        {
            AutoGenerateColumns = false,
            FillAvailableWidth = false,
        };
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "name",
            HeaderText = "Name",
            DisplayIndex = 0,
            Width = 180,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "status",
            HeaderText = "Status",
            DisplayIndex = 1,
            Width = 120,
        });

        using var searchBox = new SasdSearchBox { SearchText = "example" };
        using var filterBar = new SasdFilterBar();
        filterBar.SetFilters([SasdActiveFilter.Create("status", "Status", "Active")]);

        SasdGridViewDefinition view = SasdGridViewDefinition.Capture("Operations", grid, searchBox, filterBar);
        Ensure(view.Name == "Operations", "Saved grid view did not retain its name.");
        Ensure(view.SearchText == "example", "Saved grid view did not capture search text.");
        Ensure(view.Filters.Count == 1, "Saved grid view did not capture active filters.");

        grid.Columns[0].Width = 60;
        grid.Columns[1].Visible = false;
        searchBox.SearchText = "changed";
        filterBar.ClearFilters();

        view.Restore(grid, searchBox, filterBar);
        Ensure(grid.Columns[0].Width == 180, "Saved grid view did not restore column width.");
        Ensure(grid.Columns[1].Visible, "Saved grid view did not restore column visibility.");
        Ensure(searchBox.SearchText == "example", "Saved grid view did not restore search text.");
        Ensure(filterBar.FilterCount == 1 && filterBar.ActiveFilters[0].Value == "Active",
            "Saved grid view did not restore filter state.");

        EnsureThrows<ArgumentException>(
            () => _ = new SasdGridViewDefinition(" ", SasdDataGridState.Capture(grid)),
            "Saved grid view accepted an empty name.");
    }

    private static void ValidatePager()
    {
        using var pager = new SasdPager();

        Ensure(pager.AutoScaleMode == AutoScaleMode.Dpi,
            "Pager is not configured for DPI-aware scaling.");
        Ensure(pager.AccessibleRole == AccessibleRole.Grouping && pager.AccessibleName == "Paging controls",
            "Pager does not expose stable group-level accessibility semantics.");
        Ensure(pager.AccessibleDescription?.Contains("No rows", StringComparison.Ordinal) == true,
            "Empty pager does not expose an accessible empty-result description.");

        Button previousButton = GetPrivateField<Button>(pager, "previousButton");
        Button nextButton = GetPrivateField<Button>(pager, "nextButton");
        Label pageLabel = GetPrivateField<Label>(pager, "pageLabel");
        ComboBox pageSize = GetPrivateField<ComboBox>(pager, "pageSizeComboBox");

        Ensure(previousButton.AccessibleName == "Previous page" && nextButton.AccessibleName == "Next page",
            "Pager navigation buttons do not expose descriptive accessible names.");
        Ensure(pageSize.AccessibleName == "Rows per page",
            "Pager page-size selector does not expose a descriptive accessible name.");

        // The component must clamp stale persisted/application page indices when the
        // total row count shrinks. This is a public state contract and does not need a
        // displayed window to verify.
        pager.SetState(currentPageIndex: 8, currentPageSize: 25, knownTotalCount: 62);
        Ensure(pager.PageIndex == 2, "Pager did not clamp the current page to the final available page.");
        Ensure(pager.PageSize == 25 && pager.TotalCount == 62, "Pager did not retain the supplied page metadata.");
        Ensure(pager.AccessibleDescription?.Contains("Page 3 of 3", StringComparison.Ordinal) == true &&
               pager.AccessibleDescription.Contains("Rows 51 through 62 of 62", StringComparison.Ordinal),
            "Pager accessible description did not follow the clamped loaded state.");
        Ensure(pageLabel.AccessibleDescription == pager.AccessibleDescription,
            "Visible pager range and group-level accessible state diverged.");

        SasdPageRequestedEventArgs? requested = null;
        pager.PageRequested += (_, args) => requested = args;
        pager.SetState(currentPageIndex: 1, currentPageSize: 25, knownTotalCount: 100);

        nextButton.PerformClick();
        Ensure(requested is { PageIndex: 2, PageSize: 25 }, "Pager Next did not request the expected page.");

        // Custom application page sizes remain supported even though the UI initially
        // offers only the normal presets. Changing the selector resets navigation to
        // page zero so a previous offset cannot point beyond the new result set.
        requested = null;
        pager.SetState(currentPageIndex: 1, currentPageSize: 37, knownTotalCount: 120);
        Ensure(pageSize.Items.Contains(37), "Pager did not preserve a custom application page size.");
        pageSize.SelectedItem = 50;
        Ensure(requested is { PageIndex: 0, PageSize: 50 }, "Changing page size did not request the first page with the new size.");
    }

    private static async Task ValidateGridControllerAsync()
    {
        using var grid = new SasdDataGrid();
        using var pager = new SasdPager();
        var source = new RecordingPageSource(
        [
            new ExampleRow(1, "Alpha"),
            new ExampleRow(2, "Beta"),
            new ExampleRow(3, "Gamma"),
            new ExampleRow(4, "Delta"),
            new ExampleRow(5, "Epsilon"),
        ]);
        using var controller = new SasdGridController<ExampleRow>(grid, source, pageSize: 2);
        controller.AttachPager(pager);

        SasdGridPageLoadedEventArgs? loaded = null;
        controller.PageLoaded += (_, args) => loaded = args;

        await controller.LoadAsync();
        Ensure(source.LastQuery is { Offset: 0, Limit: 2 }, "Grid controller did not request its initial page correctly.");
        Ensure(grid.DataSource is BindingSource binding && binding.Count == 2,
            "Grid controller did not bind the returned page to the grid.");
        Ensure(pager.TotalCount == 5 && pager.PageSize == 2 && pager.PageIndex == 0,
            "Grid controller did not synchronize the attached pager after loading.");
        Ensure(loaded is { PageIndex: 0, PageSize: 2, TotalCount: 5 },
            "Grid controller did not publish the expected page-loaded event.");

        await controller.SearchAsync("  beta  ");
        Ensure(controller.SearchText == "beta", "Grid controller did not normalize search text.");
        Ensure(source.LastQuery?.SearchText == "beta" && source.LastQuery.Offset == 0,
            "Grid controller did not reset paging and forward normalized search text.");

        await controller.SortAsync("Name", SasdSortDirection.Descending);
        Ensure(controller.PageIndex == 0, "Grid controller did not reset paging when sort changed.");
        Ensure(source.LastQuery?.Sort is { Count: 1 } sort &&
               sort[0] == SasdSortDescriptor.Create("Name", SasdSortDirection.Descending),
            "Grid controller did not forward the requested sort descriptor.");

        // Exercise the actual pager event path. The in-memory source completes
        // synchronously, which keeps this smoke deterministic without sleeps or a UI
        // message pump while still proving the controller/pager event wiring.
        pager.SetState(currentPageIndex: 0, currentPageSize: 2, knownTotalCount: 5);
        Button nextButton = GetPrivateField<Button>(pager, "nextButton");
        nextButton.PerformClick();
        Ensure(controller.PageIndex == 1 && source.LastQuery?.Offset == 2,
            "Pager request did not drive the controller to the next data offset.");

        controller.Dispose();
        await EnsureThrowsAsync<ObjectDisposedException>(
            () => controller.LoadAsync(),
            "Disposed grid controller accepted a new load request.");
    }

    private static void ValidateSparkline()
    {
        using var sparkline = new SasdSparkline();
        double[] source = [10, 12, 11, 15];
        int changes = 0;
        sparkline.ValuesChanged += (_, _) => changes++;

        sparkline.SetValues(source);
        source[0] = 999;

        Ensure(sparkline.ValueCount == 4, "Sparkline did not retain all values.");
        Ensure(sparkline.Values[0] == 10, "Sparkline retained the caller-owned value array instead of copying it.");
        Ensure(changes == 1, "Sparkline did not raise exactly one value change notification.");
        Ensure(sparkline.AccessibleDescription?.Contains("rising", StringComparison.OrdinalIgnoreCase) == true,
            "Sparkline accessibility text did not describe the trend direction.");

        EnsureThrows<ArgumentException>(
            () => sparkline.SetValues([1, double.NaN]),
            "Sparkline accepted a non-finite value.");
        EnsureThrows<ArgumentOutOfRangeException>(
            () => sparkline.LineWidth = 0,
            "Sparkline accepted a line width below its documented range.");

        sparkline.ClearValues();
        Ensure(sparkline.ValueCount == 0 && changes == 2, "Sparkline did not clear values predictably.");
    }

    private static void ValidateKpiCard()
    {
        using var card = new SasdKpiCard
        {
            TitleText = "Open incidents",
            ValueText = "12",
            DetailText = "2 more than yesterday",
        };

        card.SetTrendValues([8, 9, 10, 10, 12]);
        Ensure(card.TrendValueCount == 5, "KPI card did not forward trend values to its sparkline.");
        Ensure(card.AccessibleName == "Open incidents", "KPI card did not expose its title as accessible name.");
        Ensure(card.AccessibleDescription?.Contains("12", StringComparison.Ordinal) == true,
            "KPI card accessibility text omitted the primary value.");
        Ensure(card.AccessibleDescription?.Contains("2 more than yesterday", StringComparison.Ordinal) == true,
            "KPI card accessibility text omitted the detail text.");

        card.ShowSparkline = false;
        Ensure(!card.ShowSparkline, "KPI card did not hide its trend presentation.");

        card.ClearTrendValues();
        Ensure(card.TrendValueCount == 0, "KPI card did not clear trend values.");
    }

    private static T GetPrivateField<T>(object instance, string fieldName)
        where T : class
    {
        FieldInfo? field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (field?.GetValue(instance) is not T value)
        {
            throw new InvalidOperationException($"Expected private field '{fieldName}' was not available for smoke inspection.");
        }

        return value;
    }

    private static async Task EnsureThrowsAsync<TException>(Func<Task> action, string message)
        where TException : Exception
    {
        try
        {
            await action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void EnsureThrows<TException>(Action action, string message)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed record ExampleRow(int Id, string Name);

    private sealed class RecordingPageSource(IReadOnlyList<ExampleRow> rows) : ISasdDataPageSource<ExampleRow>
    {
        public SasdDataQuery? LastQuery { get; private set; }

        public Task<SasdDataPage<ExampleRow>> LoadAsync(
            SasdDataQuery query,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            LastQuery = query;
            IReadOnlyList<ExampleRow> page = rows.Skip(query.Offset).Take(query.Limit).ToArray();
            return Task.FromResult(SasdDataPage<ExampleRow>.Create(page, rows.Count));
        }
    }
}
