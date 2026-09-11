namespace Sasd.Ui.WinForms.State;

/// <summary>A safe, application-owned reference in a recent-items list.</summary>
public sealed record SasdRecentItem(
    string Reference,
    string? DisplayName,
    DateTimeOffset LastOpenedAtUtc)
{
    /// <summary>
    /// Gets whether this reference is retained ahead of ordinary recent entries.
    /// </summary>
    /// <remarks>
    /// This is an init-only property rather than a new positional constructor parameter so
    /// UI-state documents written before pinning was introduced continue to deserialize with
    /// the natural <see langword="false"/> default.
    /// </remarks>
    public bool IsPinned { get; init; }
}

/// <summary>Maintains a bounded, deduplicated recent-items list in the UI-state store.</summary>
/// <remarks>
/// Pinned references are ordered before unpinned references and are protected from normal MRU
/// trimming. If every available slot is already occupied by pinned entries, adding another
/// unpinned reference succeeds as an operation but the new reference is not retained. This
/// keeps <see cref="MaximumItems"/> a hard bound without silently evicting a pinned item.
/// </remarks>
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

    /// <summary>Loads pinned entries first, then ordinary entries from newest to oldest.</summary>
    public async Task<IReadOnlyList<SasdRecentItem>> LoadAsync(CancellationToken cancellationToken = default)
    {
        List<SasdRecentItem>? items = await store
            .LoadAsync<List<SasdRecentItem>>(StateKey, cancellationToken)
            .ConfigureAwait(false);

        return items is null
            ? Array.Empty<SasdRecentItem>()
            : OrderAndTrim(items);
    }

    /// <summary>Adds or moves a reference to the front of its pinned/unpinned recent-items group.</summary>
    /// <remarks>
    /// Re-opening an existing reference preserves its pin state while refreshing its display
    /// name and last-opened timestamp. Applications therefore do not need to re-pin a favorite
    /// item every time it is opened.
    /// </remarks>
    public async Task AddAsync(
        string reference,
        string? displayName = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var items = (await LoadAsync(cancellationToken).ConfigureAwait(false)).ToList();
        SasdRecentItem? existing = items.FirstOrDefault(item =>
            string.Equals(item.Reference, reference, StringComparison.Ordinal));

        items.RemoveAll(item => string.Equals(item.Reference, reference, StringComparison.Ordinal));
        items.Insert(0, new SasdRecentItem(
            reference,
            NormalizeDisplayName(displayName),
            DateTimeOffset.UtcNow)
        {
            IsPinned = existing?.IsPinned ?? false,
        });

        await SaveOrderedAsync(items, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the pin state for an existing reference without changing its last-opened timestamp.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the reference exists (including when it already had the
    /// requested state); otherwise <see langword="false"/>.
    /// </returns>
    public async Task<bool> SetPinnedAsync(
        string reference,
        bool isPinned,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var items = (await LoadAsync(cancellationToken).ConfigureAwait(false)).ToList();
        int index = items.FindIndex(item =>
            string.Equals(item.Reference, reference, StringComparison.Ordinal));
        if (index < 0)
        {
            return false;
        }

        if (items[index].IsPinned != isPinned)
        {
            items[index] = items[index] with { IsPinned = isPinned };
            await SaveOrderedAsync(items, cancellationToken).ConfigureAwait(false);
        }

        return true;
    }

    /// <summary>Removes a reference if it is present, regardless of its pin state.</summary>
    public async Task<bool> RemoveAsync(string reference, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        var items = (await LoadAsync(cancellationToken).ConfigureAwait(false)).ToList();
        int removed = items.RemoveAll(item => string.Equals(item.Reference, reference, StringComparison.Ordinal));
        if (removed == 0)
        {
            return false;
        }

        await SaveOrderedAsync(items, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>Clears all recent references, including pinned entries, without touching other UI state.</summary>
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        _ = await store.RemoveAsync(StateKey, cancellationToken).ConfigureAwait(false);
    }

    private SasdRecentItem[] OrderAndTrim(IEnumerable<SasdRecentItem> items) =>
        items
            .OrderByDescending(static item => item.IsPinned)
            .ThenByDescending(static item => item.LastOpenedAtUtc)
            .Take(MaximumItems)
            .ToArray();

    private Task SaveOrderedAsync(IEnumerable<SasdRecentItem> items, CancellationToken cancellationToken) =>
        store.SaveAsync(StateKey, OrderAndTrim(items), cancellationToken);

    private static string? NormalizeDisplayName(string? displayName) =>
        string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim();
}
