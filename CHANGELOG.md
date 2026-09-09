# Changelog

All significant changes to the SASD UI Platform and its project documentation are recorded here. The format is inspired by Keep a Changelog and semantic versioning without creating a dependency on an external standard.

## [Unreleased]

### Added

- first native WinForms R0 component slice with built-in SASD Light, Dark, and High Contrast theme definitions;
- native `SasdThemeService` for explicit recursive theming without a global service locator;
- `SasdSectionPanel` and `SasdFieldLayout` for reusable business-form layouts;
- `SasdValidationCoordinator`, required-field rules, asynchronous rule support, and `SasdValidationSummary`;
- `SasdSearchBox`, `SasdEmptyState`, and `SasdDataGrid` for common data-view scenarios;
- vendor-neutral paging/sorting contracts (`SasdDataQuery`, `SasdDataPage<T>`, `ISasdDataPageSource<T>`);
- persistable `SasdDataGridState` and dependency-free CSV export;
- `SasdCommand`, `SasdCommandRunner`, and WinForms command-surface bindings with re-entry protection;
- `SasdNavigationHost` and prioritized `SasdStatusBar` shell components;
- versioned `SasdStateStore`, atomic replacement, backup recovery, reset, safe window placement, and `SasdFormStateService`;
- `SasdFileDialogService`, `SasdClipboardService`, and constrained `SasdShellService` Windows integration;
- `ISasdDialogService`, detailed error presentation, and `SasdBusyOverlay`;
- expanded Component Gallery covering forms, data, commands, navigation, state, CSV export, feedback, and theme switching;
- dependency-free core, UI-state, and WinForms-foundation smoke checks;
- Windows GitHub Actions restore/build/smoke workflow;
- `global.json` for predictable .NET 8 SDK selection.

### Changed

- the R0 Gallery now acts as an executable specification for multiple reusable modules rather than only a repository scaffold;
- previously reserved `Commands`, `Shell`, `State`, and `Windows` areas now contain initial implementations.

### Planned

- architecture and public API tests beyond the current smoke checks;
- native WinForms versus Krypton visual pilot;
- typed grid controller, richer filter expressions and paging UI;
- progress dialog, notification surface, recent-items service, and stronger migration support;
- designer, DPI, keyboard, High Contrast and UI Automation evidence across the Gallery.

## [0.1.0-docs] – 2026-07-23

### Added

- supplementary documentation landscape for the C# WinForms product line;
- roadmap and project plan from R0 through R3;
- component scope and prioritized backlog;
- development, testing, build, release, and governance guides;
- security, licensing, SBOM, and dependency concept;
- data-storage and UI-state concept with an explicit database boundary;
- migration, support, Gallery, packaging, DPI, and accessibility documents;
- ADR register, ADR template, and initial ADR files;
- GitHub issue and pull request templates.

### Decisions

- no business database inside the UI Platform;
- JSON-based, versioned UI state under LocalAppData;
- WinForms/.NET 8 as the R0/R1 focus;
- later platform lines remain documented separately.
