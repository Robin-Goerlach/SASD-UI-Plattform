namespace Sasd.Ui.WinForms.State;

/// <summary>A safe, application-owned reference in a recent-items list.</summary>
public sealed record SasdRecentItem(
    string Reference,
    string? DisplayName,
    DateTimeOffset LastOpenedAtUtc);

/// <summary>Maintains a bounded, deduplicated recent-items list in the UI-state store.</summary>
public sealed class SasdRecentItemsService
{
    private const string StateKey = "recent-items";
    private readonly SasdStateStore store;

    /// <summary>Initialises the service.</summary>
    public SasdRecentItemsService(SasdStateStore store, int maximumItems = 12)
    {
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        if (maximumItems <= 0 || maximumItems > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumItems), "Maximum items must be between 1 and 100.");
        }

        MaximumItems = maximumItems;
    }

    /// <summary>Gets the maximum number of persisted entries.</summary>
    public int MaximumItems { get; }

    /// <summary>Loads the current list ordered from newest to oldest.</summary>
    public async Task<IReadOnlyList<SasdRecentItem>> LoadAsync(CancellationToken cancellationToken = default)
    {
        var items = await store.LoadAsync<List<SasdRecentItem>>(StateKey, cancellationToken).ConfigureAwait(false);
        return items is null
            ? Array.Empty<SasdRecentItem>()
            : items.OrderByDescending(item => item.LastOpenedAtUtc).Take(MaximumItems).ToArray();
    }

    /// <summary>Adds or moves a reference to the front of the recent-items list.</summary>
    public async Task AddAsync(
        string reference,
        string? displayName = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var items = (await LoadAsync(cancellationToken).ConfigureAwait(false)).ToList();
        items.RemoveAll(item => string.Equals(item.Reference, reference, StringComparison.Ordinal));
        items.Insert(0, new SasdRecentItem(reference, NormalizeDisplayName(displayName), DateTimeOffset.UtcNow));

        if (items.Count > MaximumItems)
        {
            items.RemoveRange(MaximumItems, items.Count - MaximumItems);
        }

        await store.SaveAsync(StateKey, items, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Removes a reference if it is present.</summary>
    public async Task<bool> RemoveAsync(string reference, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var items = (await LoadAsync(cancellationToken).ConfigureAwait(false)).ToList();
        int removed = items.RemoveAll(item => string.Equals(item.Reference, reference, StringComparison.Ordinal));
        if (removed == 0)
        {
            return false;
        }

        await store.SaveAsync(StateKey, items, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>Clears recent references without touching other UI state.</summary>
    public Task ClearAsync(CancellationToken cancellationToken = default) =>
        store.RemoveAsync(StateKey, cancellationToken);

    private static string? NormalizeDisplayName(string? displayName) =>
        string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim();
}
