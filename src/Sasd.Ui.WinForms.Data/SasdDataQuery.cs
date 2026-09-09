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

/// <summary>Operator used by vendor-neutral filter descriptors.</summary>
public enum SasdFilterOperator
{
    /// <summary>Field equals the supplied value.</summary>
    Equals,

    /// <summary>Field does not equal the supplied value.</summary>
    NotEquals,

    /// <summary>Field contains the supplied value.</summary>
    Contains,

    /// <summary>Field starts with the supplied value.</summary>
    StartsWith,

    /// <summary>Field ends with the supplied value.</summary>
    EndsWith,

    /// <summary>Field is greater than the supplied value.</summary>
    GreaterThan,

    /// <summary>Field is greater than or equal to the supplied value.</summary>
    GreaterThanOrEqual,

    /// <summary>Field is less than the supplied value.</summary>
    LessThan,

    /// <summary>Field is less than or equal to the supplied value.</summary>
    LessThanOrEqual,

    /// <summary>Field has no value.</summary>
    IsNull,

    /// <summary>Field has a value.</summary>
    IsNotNull,
}

/// <summary>
/// Describes one application-owned filter without coupling the UI Platform to
/// SQL, an ORM or a concrete REST query language.
/// </summary>
public sealed record SasdFilterDescriptor(string Field, SasdFilterOperator Operator, string? Value = null)
{
    /// <summary>Creates and validates a filter descriptor.</summary>
    public static SasdFilterDescriptor Create(string field, SasdFilterOperator @operator, string? value = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(field);

        bool valueOptional = @operator is SasdFilterOperator.IsNull or SasdFilterOperator.IsNotNull;
        if (!valueOptional && value is null)
        {
            throw new ArgumentNullException(nameof(value), "This filter operator requires a value.");
        }

        return new SasdFilterDescriptor(field, @operator, valueOptional ? null : value);
    }
}

/// <summary>Describes one page request without coupling the UI to SQL, an ORM or an API.</summary>
public sealed record SasdDataQuery(
    int Offset,
    int Limit,
    IReadOnlyList<SasdSortDescriptor>? Sort = null,
    string? SearchText = null,
    IReadOnlyList<SasdFilterDescriptor>? Filters = null)
{
    /// <summary>
    /// Validates paging bounds and conservative query-complexity limits, then
    /// returns this immutable query for convenient fluent construction.
    /// </summary>
    public SasdDataQuery Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegative(Offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Limit);

        if (Limit > 10_000)
        {
            throw new ArgumentOutOfRangeException(nameof(Limit), "Limit must not exceed 10000 rows.");
        }

        if (Sort?.Count > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(Sort), "A query may contain at most 20 sort descriptors.");
        }

        if (Filters?.Count > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(Filters), "A query may contain at most 100 filter descriptors.");
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
