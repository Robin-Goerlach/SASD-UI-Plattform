# Changelog

All significant changes to the SASD UI Platform and its project documentation are recorded here. The format is inspired by Keep a Changelog and semantic versioning without creating a dependency on an external standard.

## [Unreleased]

### Added

- first native WinForms R0 component slice with built-in SASD Light, Dark, and High Contrast theme definitions;
- native `SasdThemeService` for explicit recursive theming without a global service locator;
- `SasdSectionPanel` and `SasdFieldLayout` for reusable business-form layouts;
- `SasdSearchBox`, `SasdEmptyState`, and `SasdDataGrid` for common data-view scenarios;
- `ISasdDialogService` with a native MessageBox implementation;
- an expanded Component Gallery that demonstrates the implemented controls and theme switching;
- dependency-free core smoke checks and a Windows GitHub Actions build workflow;
- `global.json` for predictable .NET 8 SDK selection.

### Planned

- architecture and public API tests beyond the initial smoke checks;
- native WinForms versus Krypton visual pilot;
- typed grid controller, filtering, paging, CSV export, and UI-state persistence;
- richer error, progress, notification, and validation components.

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
