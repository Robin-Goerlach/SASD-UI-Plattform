using System.Diagnostics.CodeAnalysis;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Sort direction used by vendor-neutral data queries.</summary>
public enum SasdSortDirection
{
    /// <summary>Ascending order.</summary>
    Ascending,

    /// <summary>Descending order.</summary>
    Descending,
}

/// <summary>Describes one stable field sort.</summary>
public sealed record SasdSortDescriptor(string Field, SasdSortDirection Direction)
{
    /// <summary>Creates and validates a sort descriptor.</summary>
    public static SasdSortDescriptor Create(string field, SasdSortDirection direction)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(field);
        return new SasdSortDescriptor(field, direction);
    }
}

/// <summary>Describes one page request without coupling the UI to SQL, an ORM or an API.</summary>
public sealed record SasdDataQuery(
    int Offset,
    int Limit,
    IReadOnlyList<SasdSortDescriptor>? Sort = null,
    string? SearchText = null)
{
    /// <summary>
    /// Validates paging bounds and returns this immutable query so callers can
    /// conveniently validate while constructing a request.
    /// </summary>
    public SasdDataQuery Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegative(Offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Limit);

        if (Limit > 10_000)
        {
            throw new ArgumentOutOfRangeException(nameof(Limit), "Limit must not exceed 10000 rows.");
        }

        return this;
    }
}

/// <summary>One page returned by an application-owned data source.</summary>
public sealed record SasdDataPage<T>(IReadOnlyList<T> Items, int TotalCount)
{
    /// <summary>
    /// Creates a validated data page. The factory remains on the generic type
    /// deliberately so application data-source implementations discover it next
    /// to the page model they return.
    /// </summary>
    [SuppressMessage(
        "Design",
        "CA1000:Do not declare static members on generic types",
        Justification = "The factory is intentionally colocated with the generic page result for discoverability and type safety.")]
    public static SasdDataPage<T> Create(IReadOnlyList<T> items, int totalCount)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);

        // A page cannot contain more rows than the source reports in total.
        if (totalCount < items.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalCount),
                "Total count must be greater than or equal to the number of returned items.");
        }

        return new SasdDataPage<T>(items, totalCount);
    }
}

/// <summary>Implemented by consuming applications to provide paged data.</summary>
public interface ISasdDataPageSource<T>
{
    /// <summary>Loads one page for a validated query.</summary>
    Task<SasdDataPage<T>> LoadAsync(SasdDataQuery query, CancellationToken cancellationToken = default);
}
