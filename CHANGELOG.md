# Changelog

All significant changes to the SASD UI Platform and its project documentation are recorded here. The format is inspired by Keep a Changelog and semantic versioning without creating a dependency on an external standard.

## [Unreleased]

### Planned

- R0.1 repository and build foundation.
- Initial ADR validation.
- Krypton/native WinForms pilot.
- Component Gallery skeleton.

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
