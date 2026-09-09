using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.DataDashboardSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ValidateFilterBatchReplacement();
            ValidateSavedGridView();
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
}
