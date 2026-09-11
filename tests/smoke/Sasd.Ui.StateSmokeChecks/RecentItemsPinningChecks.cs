using Sasd.Ui.WinForms.State;

namespace Sasd.Ui.StateSmokeChecks;

/// <summary>
/// Focused behavioral checks for the pinned-MRU contract. The timestamps are seeded
/// deterministically so ordering assertions never depend on wall-clock resolution.
/// </summary>
internal static class RecentItemsPinningChecks
{
    public static async Task RunAsync(string root)
    {
        string recentRoot = Path.Combine(root, "recent-items-pinning");
        await using var store = new SasdStateStore(new SasdStateStoreOptions(
            "SASD-GmbH",
            "RecentItemsPinningSmokeChecks",
            recentRoot));
        var service = new SasdRecentItemsService(store, maximumItems: 2);

        DateTimeOffset alphaOpened = new(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);
        DateTimeOffset betaOpened = new(2026, 1, 2, 8, 0, 0, TimeSpan.Zero);

        // Write the pre-pinning record shape intentionally. Deserializing these entries through
        // SasdRecentItem proves that the additive IsPinned property keeps older UI-state
        // documents readable with the expected false default.
        await store.SaveAsync("recent-items", new List<LegacyRecentItem>
        {
            new("alpha", "Alpha", alphaOpened),
            new("beta", "Beta", betaOpened),
        });

        IReadOnlyList<SasdRecentItem> loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2 && loaded.All(static item => !item.IsPinned),
            "Legacy recent-item state did not deserialize with an unpinned default.");
        Ensure(loaded[0].Reference == "beta" && loaded[1].Reference == "alpha",
            "Legacy recent-item ordering changed before pinning was applied.");

        Ensure(await service.SetPinnedAsync("alpha", true),
            "Existing recent item could not be pinned.");
        loaded = await service.LoadAsync();
        Ensure(loaded[0].Reference == "alpha" && loaded[0].IsPinned,
            "Pinned recent item was not ordered ahead of newer unpinned entries.");
        Ensure(loaded[0].LastOpenedAtUtc == alphaOpened,
            "Pinning changed the item's last-opened timestamp.");

        // With one pinned slot and one ordinary slot, a new recent item must evict the oldest
        // unpinned item rather than the pinned favorite.
        await service.AddAsync("gamma", "Gamma");
        loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2 &&
               loaded[0].Reference == "alpha" && loaded[0].IsPinned &&
               loaded[1].Reference == "gamma" && !loaded[1].IsPinned,
            "MRU trimming evicted a pinned item instead of the oldest unpinned item.");

        await service.AddAsync("alpha", "  Alpha renamed  ");
        loaded = await service.LoadAsync();
        SasdRecentItem reopenedAlpha = loaded.Single(item => item.Reference == "alpha");
        Ensure(reopenedAlpha.IsPinned,
            "Re-opening an existing recent item lost its pin state.");
        Ensure(reopenedAlpha.DisplayName == "Alpha renamed",
            "Re-opening a pinned recent item did not normalize/update its display name.");
        Ensure(reopenedAlpha.LastOpenedAtUtc >= alphaOpened,
            "Re-opening a pinned recent item did not refresh its last-opened timestamp.");

        Ensure(await service.SetPinnedAsync("gamma", true),
            "Second existing recent item could not be pinned.");
        loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2 && loaded.All(static item => item.IsPinned),
            "Pinning the second item did not retain both favorites within the configured bound.");

        // MaximumItems remains a hard bound. When every slot is pinned, a new ordinary recent
        // reference must be dropped instead of silently evicting one of those pinned entries.
        await service.AddAsync("delta", "Delta");
        loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2 && loaded.All(static item => item.IsPinned),
            "Adding an unpinned item displaced a fully pinned recent-items set.");
        Ensure(loaded.All(static item => item.Reference != "delta"),
            "Unpinned item was retained even though every bounded slot was protected by pinning.");

        Ensure(await service.SetPinnedAsync("alpha", false),
            "Existing recent item could not be unpinned.");
        await service.AddAsync("delta", "Delta");
        loaded = await service.LoadAsync();
        Ensure(loaded.Any(static item => item.Reference == "gamma" && item.IsPinned) &&
               loaded.Any(static item => item.Reference == "delta" && !item.IsPinned) &&
               loaded.All(static item => item.Reference != "alpha"),
            "Unpinning did not make the ordinary item eligible for normal MRU trimming.");

        Ensure(!await service.SetPinnedAsync("missing", true),
            "Pinning reported success for a reference that is not present.");

        Ensure(await service.RemoveAsync("gamma"),
            "Pinned recent item could not be removed explicitly.");
        loaded = await service.LoadAsync();
        Ensure(loaded.Count == 1 && loaded[0].Reference == "delta",
            "Removing a pinned item changed the wrong recent reference.");

        Ensure(await service.SetPinnedAsync("delta", true),
            "Remaining recent item could not be pinned before clear verification.");
        await service.ClearAsync();
        Ensure((await service.LoadAsync()).Count == 0,
            "ClearAsync did not remove pinned recent items together with ordinary items.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed record LegacyRecentItem(
        string Reference,
        string? DisplayName,
        DateTimeOffset LastOpenedAtUtc);
}
