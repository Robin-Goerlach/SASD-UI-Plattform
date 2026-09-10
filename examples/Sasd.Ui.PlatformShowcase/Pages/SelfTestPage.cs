using Sasd.Ui.Core;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Media;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.State;
using Sasd.Ui.WinForms.Theming;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>
/// Provides a small in-process test surface that exercises public component APIs without relying on private fields.
/// </summary>
internal sealed class SelfTestPage : UserControl
{
    private const string TestStateKey = "showcase:self-test";

    private readonly SasdStateStore stateStore;
    private readonly SasdThemeService themeService;
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly ListBox resultsList = new();
    private readonly Button runButton = new();

    public SelfTestPage(
        SasdStateStore stateStore,
        SasdThemeService themeService,
        Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        this.themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "Showcase self test\r\n\r\n" +
                "These checks exercise only public APIs and avoid modal dialogs or external processes. " +
                "They complement repository smoke tests by making important component behaviour observable in the example application.",
        };

        runButton.AutoSize = true;
        runButton.Text = "Run safe self tests";
        runButton.Click += OnRunClick;

        resultsList.Dock = DockStyle.Fill;
        resultsList.HorizontalScrollbar = true;

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(runButton, 0, 1);
        layout.Controls.Add(resultsList, 0, 2);
        Controls.Add(layout);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            runButton.Click -= OnRunClick;
        }

        base.Dispose(disposing);
    }

    private async void OnRunClick(object? sender, EventArgs e)
    {
        runButton.Enabled = false;
        resultsList.Items.Clear();
        int passed = 0;
        int failed = 0;

        try
        {
            await RunCheckAsync("SearchBox stores text", () =>
            {
                using var search = new SasdSearchBox { SearchText = "alpha" };
                Ensure(search.SearchText == "alpha", "Search text was not retained.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("FilterBar replaces duplicate keys", () =>
            {
                using var filters = new SasdFilterBar();
                filters.AddOrUpdateFilter("status", "Status", "Active");
                filters.AddOrUpdateFilter("status", "Status", "Paused");
                Ensure(filters.FilterCount == 1, "Duplicate filter key created a second active filter.");
                Ensure(filters.ActiveFilters[0].Value == "Paused", "Replacement filter value was not retained.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("Grid state restores column visibility", () =>
            {
                using var grid = new SasdDataGrid { AutoGenerateColumns = false };
                grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "first", HeaderText = "First" });
                grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "second", HeaderText = "Second" });
                SasdDataGridState state = SasdDataGridState.Capture(grid);
                grid.Columns[1].Visible = false;
                state.Restore(grid);
                Ensure(grid.Columns[1].Visible, "Captured grid state did not restore column visibility.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("KPI card accepts trend values", () =>
            {
                using var card = new SasdKpiCard { TitleText = "Self test", ValueText = "42" };
                card.SetTrendValues([1, 2, 3, 4]);
                Ensure(card.TrendValueCount == 4, "KPI trend values were not retained.");
                Ensure(!string.IsNullOrWhiteSpace(card.AccessibleDescription), "KPI accessibility description is empty.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("ImageViewer clones caller image", () =>
            {
                using var viewer = new SasdImageViewer();
                using var source = new Bitmap(16, 12);
                viewer.SetImage(source);
                Image copy = viewer.Image ?? throw new InvalidOperationException("Image viewer contains no assigned image.");
                Ensure(!ReferenceEquals(source, copy), "Image viewer retained the caller-owned image instance.");
                Ensure(copy.Width == 16 && copy.Height == 12, "Image dimensions changed unexpectedly.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("PropertyEditor retains application object", () =>
            {
                using var editor = new SasdPropertyEditor();
                var settings = new DemoSettings();
                editor.SelectedObject = settings;
                editor.ForceReadOnly = true;
                Ensure(ReferenceEquals(settings, editor.SelectedObject), "Property editor replaced the application-owned object.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("Theme service has a valid current theme", () =>
            {
                // Store the property value once. A property can theoretically return a different
                // result between calls, so C# nullable flow analysis correctly does not carry the
                // first null-check across a second independent property access.
                SasdThemeDefinition currentTheme = themeService.Current
                    ?? throw new InvalidOperationException("Theme service has no current definition.");
                Ensure(!string.IsNullOrWhiteSpace(currentTheme.Id), "Current theme has no stable identifier.");
                return Task.CompletedTask;
            }, () => passed++, () => failed++);

            await RunCheckAsync("StateStore round trip", async () =>
            {
                var expected = new DemoUiState("self-test", DateTimeOffset.UtcNow);
                await stateStore.SaveAsync(TestStateKey, expected);
                DemoUiState? actual = await stateStore.LoadAsync<DemoUiState>(TestStateKey);
                Ensure(actual?.Notes == expected.Notes, "Saved state did not round-trip through the store.");
                _ = await stateStore.RemoveAsync(TestStateKey);
            }, () => passed++, () => failed++);
        }
        finally
        {
            runButton.Enabled = true;
        }

        string summary = $"Self test complete: {passed} passed, {failed} failed.";
        resultsList.Items.Add(new string('-', 50));
        resultsList.Items.Add(summary);
        publishStatus(
            summary,
            failed == 0 ? SasdStatusSeverity.Success : SasdStatusSeverity.Error,
            TimeSpan.FromSeconds(6));
    }

    private async Task RunCheckAsync(
        string name,
        Func<Task> check,
        Action onPassed,
        Action onFailed)
    {
        try
        {
            await check();
            resultsList.Items.Add($"PASS  {name}");
            onPassed();
        }
        catch (Exception exception)
        {
            resultsList.Items.Add($"FAIL  {name} — {exception.Message}");
            onFailed();
        }
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
