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
    /// <summary>Returns a validated query.</summary>
    public SasdDataQuery Validate()
    {
        if (Offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Offset));
        }

        if (Limit <= 0 || Limit > 10_000)
        {
            throw new ArgumentOutOfRangeException(nameof(Limit), "Limit must be between 1 and 10000.");
        }

        return this;
    }
}

/// <summary>One page returned by an application-owned data source.</summary>
public sealed record SasdDataPage<T>(IReadOnlyList<T> Items, int TotalCount)
{
    /// <summary>Creates a validated data page.</summary>
    public static SasdDataPage<T> Create(IReadOnlyList<T> items, int totalCount)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (totalCount < 0 || totalCount < items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(totalCount));
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
