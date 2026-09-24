# Sasd.Ui.WinForms.State

Reusable, presentation-only UI-state infrastructure for SASD WinForms applications.

## Implemented R1 foundation

- `SasdStateStore` for versioned JSON sections with atomic replacement, backup recovery and reset;
- `SasdStateMigration` for explicit incremental schema transitions;
- `SasdWindowState` and `SasdFormStateService` for safe form placement/state persistence;
- `SasdRecentItemsService` for a bounded, deduplicated MRU list with optional pinned entries.

## State-store boundary

The module stores **UI state**, not application-domain data. Typical examples are window placement, grid layout, selected theme, view state and safe recent-item references. Credentials, API tokens, complete documents/messages, database connection strings and other secrets do not belong in this store.

The state document is versioned and written through the store's primary/backup replacement flow. Applications own migrations for sections whose shape they define; a failed or obsolete UI-state document must never be treated as the authoritative copy of business data.

## Form/window ownership

`SasdFormStateService` does not own forms. It reads/writes the safe presentation state of caller-owned forms and restores impossible/off-screen coordinates back onto the current Windows desktop. Actual multi-monitor and changed-DPI movement still requires manual acceptance on representative Windows systems.

## Recent items and pinning

`SasdRecentItemsService` persists application-supplied references plus an optional display name and last-opened timestamp. References should be stable, privacy-appropriate identifiers or paths that the application is prepared to persist under LocalAppData; they must not contain secrets merely because the state file is local.

The list remains bounded by `MaximumItems`. Pinned entries are ordered before ordinary MRU entries and are protected from normal trimming. Within each group, newer entries appear first. Re-opening an existing reference refreshes its timestamp/display name but preserves its pin state. If every available slot is pinned, a newly added unpinned entry is not retained rather than silently evicting a favorite.

`SetPinnedAsync(...)` changes only the pin state; it does not pretend that pinning is another open operation and therefore does not change `LastOpenedAtUtc`. `RemoveAsync(...)` can remove pinned or unpinned entries explicitly. `ClearAsync()` intentionally clears the complete recent-items section, including pinned entries.

`IsPinned` is an additive init-only property on `SasdRecentItem`. State written before pinning existed simply deserializes with the normal `false` default, avoiding a migration requirement for this compatible extension.

## Boundaries

- no database/ORM or application-domain persistence;
- no secret/credential storage;
- no roaming/cloud synchronization;
- no background watcher or telemetry;
- application code remains responsible for validating that a recent reference still exists and is still safe to open;
- UI state is recoverable convenience state and must never be the sole authoritative copy of user/business content.
