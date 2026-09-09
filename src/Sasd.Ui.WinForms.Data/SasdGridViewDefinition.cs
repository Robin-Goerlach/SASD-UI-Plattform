namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Captures a named, persistable view of one data-grid surface.
/// </summary>
/// <remarks>
/// <para>
/// A view combines visual grid layout with optional search text and active filters. It deliberately
/// contains no data-source, SQL, ORM or application-specific predicate information. The consuming
/// application remains responsible for translating restored search/filter state into a query.
/// </para>
/// <para>
/// Instances are ordinary serializable data and can therefore be stored with the existing
/// `SasdStateStore` without introducing a second persistence mechanism in the Data module.
/// </para>
/// </remarks>
public sealed record SasdGridViewDefinition
{
    /// <summary>Initialises a validated saved-grid view.</summary>
    public SasdGridViewDefinition(
        string name,
        SasdDataGridState gridState,
        string searchText = "",
        IReadOnlyList<SasdActiveFilter>? filters = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(gridState);

        string normalizedName = name.Trim();
        if (normalizedName.Length > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(name), "Saved view names may contain at most 120 characters.");
        }

        Name = normalizedName;
        GridState = gridState;
        SearchText = searchText ?? string.Empty;
        Filters = ValidateAndCopyFilters(filters ?? Array.Empty<SasdActiveFilter>());
    }

    /// <summary>Gets the user-facing view name.</summary>
    public string Name { get; init; }

    /// <summary>Gets the captured column visibility, order and width state.</summary>
    public SasdDataGridState GridState { get; init; }

    /// <summary>Gets the optional search text captured with the view.</summary>
    public string SearchText { get; init; }

    /// <summary>Gets a validated snapshot of active filters captured with the view.</summary>
    public IReadOnlyList<SasdActiveFilter> Filters { get; init; }

    /// <summary>
    /// Captures a view from a grid and, when supplied, its search/filter controls.
    /// </summary>
    public static SasdGridViewDefinition Capture(
        string name,
        DataGridView grid,
        SasdSearchBox? searchBox = null,
        SasdFilterBar? filterBar = null)
    {
        ArgumentNullException.ThrowIfNull(grid);

        return new SasdGridViewDefinition(
            name,
            SasdDataGridState.Capture(grid),
            searchBox?.SearchText ?? string.Empty,
            filterBar?.ActiveFilters ?? Array.Empty<SasdActiveFilter>());
    }

    /// <summary>
    /// Restores the view into the supplied controls.
    /// </summary>
    /// <remarks>
    /// Restoring a view changes only UI state. It does not fetch or filter data. Search/filter
    /// controls raise their normal change notifications so the consuming application can respond
    /// through the same code path it uses for direct user interaction.
    /// </remarks>
    public void Restore(
        DataGridView grid,
        SasdSearchBox? searchBox = null,
        SasdFilterBar? filterBar = null)
    {
        ArgumentNullException.ThrowIfNull(grid);

        GridState.Restore(grid);
        if (searchBox is not null)
        {
            searchBox.SearchText = SearchText;
        }

        if (filterBar is not null)
        {
            filterBar.SetFilters(Filters);
        }
    }

    private static IReadOnlyList<SasdActiveFilter> ValidateAndCopyFilters(IEnumerable<SasdActiveFilter> filters)
    {
        var result = new List<SasdActiveFilter>();
        foreach (SasdActiveFilter filter in filters)
        {
            ArgumentNullException.ThrowIfNull(filter);
            SasdActiveFilter validated = SasdActiveFilter.Create(filter.Key, filter.Label, filter.Value);
            int existingIndex = result.FindIndex(item =>
                string.Equals(item.Key, validated.Key, StringComparison.OrdinalIgnoreCase));

            if (existingIndex >= 0)
            {
                result[existingIndex] = validated;
            }
            else
            {
                result.Add(validated);
            }
        }

        return result.ToArray();
    }
}
