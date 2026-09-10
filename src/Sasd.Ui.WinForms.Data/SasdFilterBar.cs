using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Represents one user-visible active filter.</summary>
public sealed record SasdActiveFilter(string Key, string Label, string Value)
{
    /// <summary>Creates a validated filter value for <see cref="SasdFilterBar"/>.</summary>
    public static SasdActiveFilter Create(string key, string label, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentNullException.ThrowIfNull(value);

        if (key.Length > 200)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Filter keys may contain at most 200 characters.");
        }

        return new SasdActiveFilter(key, label, value);
    }

    /// <summary>Gets the compact text shown inside the filter bar.</summary>
    public string DisplayText => string.IsNullOrEmpty(Value) ? Label : $"{Label}: {Value}";
}

/// <summary>
/// Displays the currently active filters as removable chips and provides one
/// predictable place for a clear-all action.
/// </summary>
/// <remarks>
/// <para>
/// This control intentionally does not evaluate predicates. The consuming
/// application remains responsible for translating user choices into its own
/// database, API or in-memory filtering logic.
/// </para>
/// <para>
/// Filter identity is based on <see cref="SasdActiveFilter.Key"/>. Adding a filter
/// with an existing key replaces the old value instead of creating duplicates.
/// </para>
/// </remarks>
[DefaultEvent(nameof(FiltersChanged))]
public class SasdFilterBar : UserControl
{
    private readonly FlowLayoutPanel filterHost;
    private readonly Button clearAllButton;
    private readonly List<SasdActiveFilter> activeFilters = [];

    /// <summary>Initialises the filter bar.</summary>
    public SasdFilterBar()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MinimumSize = new Size(180, 34);
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Active filters";

        filterHost = new FlowLayoutPanel
        {
            AccessibleRole = AccessibleRole.Grouping,
            AccessibleName = "Active filter list",
            AccessibleDescription = "Filters currently applied to the data view.",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            TabStop = false,
            WrapContents = true,
        };

        clearAllButton = new Button
        {
            AccessibleName = "Clear all filters",
            AccessibleDescription = "Removes every active filter from the current view.",
            AutoSize = true,
            FlatStyle = FlatStyle.System,
            Margin = new Padding(6, 0, 0, 0),
            TabIndex = 1,
            Text = "Clear filters",
            Visible = false,
        };
        clearAllButton.Click += (_, _) => ClearFilters();

        Controls.Add(filterHost);
        Controls.Add(clearAllButton);
        clearAllButton.Dock = DockStyle.Right;
        UpdateAccessibleState();
    }

    /// <summary>Occurs after a filter is added, replaced, removed, cleared or replaced as a set.</summary>
    public event EventHandler? FiltersChanged;

    /// <summary>Gets a snapshot of the active filters in display order.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<SasdActiveFilter> ActiveFilters => activeFilters.ToArray();

    /// <summary>Gets the number of active filters.</summary>
    [Browsable(false)]
    public int FilterCount => activeFilters.Count;

    /// <summary>Adds a filter or replaces the existing filter with the same key.</summary>
    public void AddOrUpdateFilter(string key, string label, string value) =>
        AddOrUpdateFilter(SasdActiveFilter.Create(key, label, value));

    /// <summary>Adds a filter or replaces the existing filter with the same key.</summary>
    public void AddOrUpdateFilter(SasdActiveFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        SasdActiveFilter validated = SasdActiveFilter.Create(filter.Key, filter.Label, filter.Value);

        int index = activeFilters.FindIndex(item =>
            string.Equals(item.Key, validated.Key, StringComparison.OrdinalIgnoreCase));

        if (index >= 0)
        {
            activeFilters[index] = validated;
        }
        else
        {
            activeFilters.Add(validated);
        }

        RebuildFilterControls();
        FiltersChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Replaces the complete active-filter set and raises <see cref="FiltersChanged"/> once.
    /// </summary>
    /// <remarks>
    /// Duplicate keys are resolved using the same case-insensitive replacement rule as
    /// <see cref="AddOrUpdateFilter(SasdActiveFilter)"/> while retaining the first key position.
    /// This method is useful when restoring a saved view because listeners can refresh their
    /// data once after the complete filter state has been applied.
    /// </remarks>
    public void SetFilters(IEnumerable<SasdActiveFilter> filters)
    {
        ArgumentNullException.ThrowIfNull(filters);

        var replacement = new List<SasdActiveFilter>();
        foreach (SasdActiveFilter filter in filters)
        {
            ArgumentNullException.ThrowIfNull(filter);
            SasdActiveFilter validated = SasdActiveFilter.Create(filter.Key, filter.Label, filter.Value);
            int existingIndex = replacement.FindIndex(item =>
                string.Equals(item.Key, validated.Key, StringComparison.OrdinalIgnoreCase));

            if (existingIndex >= 0)
            {
                replacement[existingIndex] = validated;
            }
            else
            {
                replacement.Add(validated);
            }
        }

        activeFilters.Clear();
        activeFilters.AddRange(replacement);
        RebuildFilterControls();
        FiltersChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Removes one filter by its stable key.</summary>
    public bool RemoveFilter(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        int index = activeFilters.FindIndex(item =>
            string.Equals(item.Key, key, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            return false;
        }

        activeFilters.RemoveAt(index);
        RebuildFilterControls();
        FiltersChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>Removes all active filters.</summary>
    public void ClearFilters()
    {
        if (activeFilters.Count == 0)
        {
            return;
        }

        activeFilters.Clear();
        RebuildFilterControls();
        FiltersChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RebuildFilterControls()
    {
        // Controls.Clear() would detach controls without disposing them. Explicitly
        // disposing the generated chip controls avoids accumulating native handles
        // when a filter changes repeatedly during a long-running application session.
        Control[] oldControls = filterHost.Controls.Cast<Control>().ToArray();
        filterHost.Controls.Clear();
        foreach (Control control in oldControls)
        {
            control.Dispose();
        }

        foreach (SasdActiveFilter filter in activeFilters)
        {
            filterHost.Controls.Add(CreateFilterChip(filter));
        }

        clearAllButton.Visible = activeFilters.Count > 0;
        UpdateAccessibleState();
    }

    private FlowLayoutPanel CreateFilterChip(SasdActiveFilter filter)
    {
        var chip = new FlowLayoutPanel
        {
            AccessibleRole = AccessibleRole.Grouping,
            AccessibleName = filter.DisplayText,
            AccessibleDescription = $"Active filter {filter.DisplayText}.",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = SystemColors.ControlLight,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0, 0, 6, 4),
            Padding = new Padding(6, 3, 2, 3),
            TabStop = false,
            WrapContents = false,
        };

        chip.Controls.Add(new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 4, 4, 0),
            Text = filter.DisplayText,
        });

        var removeButton = new Button
        {
            AccessibleName = $"Remove filter {filter.DisplayText}",
            AccessibleDescription = $"Removes the active filter {filter.DisplayText}.",
            AutoSize = true,
            FlatStyle = FlatStyle.System,
            Margin = Padding.Empty,
            MinimumSize = new Size(26, 24),
            TabIndex = 0,
            Text = "×",
        };
        removeButton.Click += (_, _) => RemoveFilter(filter.Key);
        chip.Controls.Add(removeButton);

        return chip;
    }

    private void UpdateAccessibleState()
    {
        if (activeFilters.Count == 0)
        {
            AccessibleDescription = "No active filters.";
            return;
        }

        string filters = string.Join("; ", activeFilters.Select(static filter => filter.DisplayText));
        string noun = activeFilters.Count == 1 ? "filter" : "filters";
        AccessibleDescription = $"{activeFilters.Count} active {noun}: {filters}.";
    }
}
