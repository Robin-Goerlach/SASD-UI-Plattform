using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.NativeR2SmokeChecks;

/// <summary>
/// Behavioral checks for the dependency-free dashboard and saved-view components.
/// Keeping these checks separate from Program.cs makes the smoke executable easier to read as R2 grows.
/// </summary>
internal static class DashboardSmokeChecks
{
    public static void Run()
    {
        ValidateSavedGridView();
        ValidateSparkline();
        ValidateKpiCard();
    }

    private static void ValidateSavedGridView()
    {
        using var grid = new SasdDataGrid
        {
            AutoGenerateColumns = false,
            FillAvailableWidth = false,
        };
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "Name", Width = 180 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Status", Width = 120 });

        using var search = new SasdSearchBox { SearchText = "critical" };
        using var filters = new SasdFilterBar();
        filters.AddOrUpdateFilter("status", "Status", "Open");

        SasdGridViewDefinition view = SasdGridViewDefinition.Capture("Operations", grid, search, filters);
        Ensure(view.Name == "Operations", "Saved grid view changed its validated name.");
        Ensure(view.SearchText == "critical" && view.Filters.Count == 1,
            "Saved grid view did not capture search/filter state.");

        // Disturb all three pieces of state so Restore must actually do work.
        grid.Columns[0].Width = 60;
        grid.Columns[1].Visible = false;
        search.SearchText = "different";
        filters.ClearFilters();

        int filterNotifications = 0;
        filters.FiltersChanged += (_, _) => filterNotifications++;
        view.Restore(grid, search, filters);

        Ensure(grid.Columns[0].Width == 180 && grid.Columns[1].Visible,
            "Saved grid view did not restore visual column state.");
        Ensure(search.SearchText == "critical", "Saved grid view did not restore search text.");
        Ensure(filters.FilterCount == 1 && filters.ActiveFilters[0].Value == "Open",
            "Saved grid view did not restore active filters.");
        Ensure(filterNotifications == 1,
            "Restoring a saved view raised more than one filter notification for one logical state change.");

        EnsureThrows<ArgumentException>(
            () => _ = new SasdGridViewDefinition(" ", view.GridState),
            "Saved grid view accepted an empty name.");
    }

    private static void ValidateSparkline()
    {
        using var sparkline = new SasdSparkline();
        int changes = 0;
        sparkline.ValuesChanged += (_, _) => changes++;

        double[] source = [10, 12, 11, 15];
        sparkline.SetValues(source);
        source[0] = 999;

        Ensure(sparkline.ValueCount == 4, "Sparkline did not retain all assigned values.");
        Ensure(sparkline.Values[0] == 10, "Sparkline retained the caller-owned array instead of copying values.");
        Ensure(sparkline.AccessibleDescription?.Contains("rising", StringComparison.OrdinalIgnoreCase) == true,
            "Sparkline did not expose a textual trend description for accessibility.");

        sparkline.LineWidth = 3F;
        Ensure(Math.Abs(sparkline.LineWidth - 3F) < float.Epsilon,
            "Sparkline did not retain a valid line width.");

        EnsureThrows<ArgumentException>(
            () => sparkline.SetValues([1, double.NaN]),
            "Sparkline accepted a non-finite value.");
        EnsureThrows<ArgumentOutOfRangeException>(
            () => sparkline.LineWidth = 0F,
            "Sparkline accepted a line width below its documented range.");

        sparkline.ClearValues();
        Ensure(sparkline.ValueCount == 0 && changes == 2,
            "Sparkline clear/change notifications are not predictable.");
    }

    private static void ValidateKpiCard()
    {
        using var card = new SasdKpiCard
        {
            TitleText = "Open incidents",
            ValueText = "12",
            DetailText = "Last 24 hours",
        };

        card.SetTrendValues([8, 9, 11, 12]);
        Ensure(card.TrendValueCount == 4, "KPI card did not forward trend values to its sparkline.");
        Ensure(card.AccessibleName == "Open incidents",
            "KPI card did not expose its title as an accessible name.");
        Ensure(card.AccessibleDescription?.Contains("12", StringComparison.Ordinal) == true,
            "KPI card accessibility description omitted its primary value.");
        Ensure(card.AccessibleDescription?.Contains("rising", StringComparison.OrdinalIgnoreCase) == true,
            "KPI card accessibility description omitted textual trend context.");

        card.ShowSparkline = false;
        Ensure(!card.ShowSparkline, "KPI card did not retain sparkline visibility state.");
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
