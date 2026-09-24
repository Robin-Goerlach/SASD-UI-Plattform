# Project Plan and Milestones

## 1. Planning Approach

The plan is based on **outcomes and gates**, not invented completion dates. Only after R0.1 can actual throughput support reliable calendar planning.

## 2. R0.1 Work Packages

| ID | Work Package | Result | Dependency |
| --- | --- | --- | --- |
| WP-R01-01 | Repository and solution | Standardized monorepo structure | none |
| WP-R01-02 | Build foundation | Props, packages, SDK, analyzers, lock strategy | WP-R01-01 |
| WP-R01-03 | Core contracts | Result, errors, tokens, versioning | WP-R01-02 |
| WP-R01-04 | Test foundation | xUnit, architecture tests, test host | WP-R01-02 |
| WP-R01-05 | Gallery skeleton | Navigable demo with foundation pages | WP-R01-03 |
| WP-R01-06 | Governance | ADRs, license register, changelog, PR checks | WP-R01-01 |
| WP-R01-07 | CI | Validate, build, test, pack probe | WP-R01-02/04 |

## 3. R0.2 Work Packages

- design-token model;
- native WinForms theme reference;
- Krypton adapter and palette;
- `SasdForm`, `SasdDialogForm`, `SasdUserControl`;
- form layout and validation foundation;
- icon service;
- designer, DPI, focus, and High Contrast test matrix;
- ADR-0004 final decision.

## 4. R0.3 Work Packages

- SearchBox, FilterBar, EmptyState, BusyOverlay;
- grid/controller spike;
- dialog, error, and progress services;
- StateStore with migration and recovery;
- UIA pilot;
- visual regression;
- first end-to-end Gallery flows.

## 5. R1 Epics

| Epic | Core Content | First Consumer Application |
| --- | --- | --- |
| EP-R1-BASE | Base classes, layout, dispatcher | TaskHost/utility |
| EP-R1-THEME | Themes, icons, High Contrast | Prompt Manager |
| EP-R1-CMD | Commands and shortcuts | Prompt Manager |
| EP-R1-SHELL | Shell, navigation, tabs, status | Mail Workbench/Notes |
| EP-R1-DATA | Grid, list, tree, search, filter, paging | Prompt Manager |
| EP-R1-FEEDBACK | Dialogs, errors, notifications, progress | all |
| EP-R1-WINDOWS | Files, clipboard, drag-and-drop, tray, shell | utility/Mail Workbench |
| EP-R1-STATE | UI state, MRU, migration | all |
| EP-R1-DOCS | Gallery, samples, API documentation | all |
| EP-R1-REL | NuGet, SBOM, release process | all |

## 6. Gate Checklists

### Gate R0.1

- [ ] A clean checkout builds without manual preparation.
- [ ] Architecture tests detect a prohibited dependency.
- [ ] Package restore is locked and reproducible.
- [ ] Initial packages can be installed locally.
- [ ] A license inventory exists.
- [ ] The ADR register and template work.

### Gate R0.2

- [ ] The designer opens and saves every pilot control.
- [ ] Light, Dark, and High Contrast work.
- [ ] 100%, 125%, 150%, and 200% DPI are reviewed.
- [ ] Keyboard focus is visible.
- [ ] No critical Krypton API leakage exists in the core.
- [ ] ADR-0004 is accepted or rejected.

### Gate R0.3

- [ ] Grid and dialog spikes are executable.
- [ ] Invalid state starts with secure defaults.
- [ ] The UIA end-to-end flow is stable.
- [ ] The screenshot baseline is documented.
- [ ] Resource-cycle testing shows no continuing growth.

### Gate R1.0

- [ ] R1 mandatory scope is implemented or deviations are accepted.
- [ ] The Gallery covers every public visual component.
- [ ] Three samples build from published packages.
- [ ] The first real SASD application uses the packages.
- [ ] NuGet packages, symbols, XML docs, SBOM, and notices are published.
- [ ] Security, licensing, and migration reviews are passed.

## 7. Dependencies and Critical Path

The critical path runs through build/repository → core contracts → theme/designer → grid/dialog/state → Gallery/samples → real migration. R2 modules must not block the critical path.

## 8. Capacity Rule

At most one major UI building block and one cross-cutting concern are developed at the same time—for example, grid plus test harness, not grid, shell, docking, and WebView2 in parallel. This keeps root causes and architecture decisions traceable.
