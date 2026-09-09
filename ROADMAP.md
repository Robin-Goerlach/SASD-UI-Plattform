# Roadmap – SASD UI Platform

**As of:** 2026-09-09  
**Version:** 0.2  
**Horizon:** R0 through R3; no artificial calendar dates without reliable capacity planning

## 1. Roadmap Principles

The roadmap prioritizes **usability over component count**. A component does not represent progress merely because it has been implemented. Progress exists when the component:

- is demonstrated completely in the Component Gallery;
- passes designer, DPI, keyboard, accessibility, and localization checks;
- has a documented public API;
- is used by at least one real SASD application without a local special-purpose copy;
- has an acceptable maintenance and dependency profile.

The product line is not developed simultaneously for WinForms, WPF, web, and Java. The current focus remains WinForms.

## 2. Current Implementation Status

The repository is no longer only an R0 scaffold. A reusable native WinForms foundation is implemented and guarded by strict Windows CI, architecture checks and dependency-free smoke executables.

The status below distinguishes **implemented code** from **completed release gates**. A gate is not marked complete merely because most of its classes exist.

| Stage | Current status | Evidence already present | Important work still open |
| --- | --- | --- | --- |
| R0.1 | Largely implemented | .NET 8 solution, central build/package configuration, strict warning-free CI, architecture checks, ADR/licence process, Component Gallery | packaging/API-baseline/release-engineering work can be strengthened further |
| R0.2 | Pilot implemented; decision gate still open | native theme/form baseline, isolated `Krypton.Toolkit` adapter, Krypton smoke check, automated vendor-boundary enforcement | Visual Studio Designer matrix, multi-DPI/focus/high-contrast acceptance and final native-vs-Krypton standard decision |
| R0.3 | Core interaction code largely implemented | grid/controller/paging/search/filter contracts, CSV and grid state, dialogs/error/busy/progress, versioned UI state with backup/migrations, command/shell/state smoke checks | UIA/FlaUI pilot, screenshot regression, keyboard-only end-to-end evidence and wider Gallery coverage |
| R1.0 | Started | reusable forms/validation, commands, navigation/status, data views, Windows services, UI state/recent items and several R1 controls | remaining R1 controls, icons, notifications, drag/drop/tray, reference applications, NuGet publication and first real consumer migration |
| R1.1 | Not started as a release stage | some hardening already happens continuously | formal public-API review, performance/handle-leak matrix and three real SASD consumers |
| R2.0 | Deferred | architecture and candidate list documented | no specialist adapter should be promoted before an R1 consumer need exists |
| R3 | Deferred | scope rules documented | requires demonstrated product need and explicit maintenance ownership |

## 3. Release Map

| Stage | Goal | Result | Release Criterion |
| --- | --- | --- | --- |
| R0.1 | Architecture and repository foundation | Monorepo, build, core contracts, ADR and licensing process | A clean checkout builds reproducibly |
| R0.2 | UI technology pilot | Krypton versus native WinForms; tokens, theme, base classes, form layout | Designer, DPI, focus, and high-contrast matrix passed |
| R0.3 | Core interaction pilot | Grid, dialogs, errors, progress, UI state, UIA/screenshot pilot | No third-party types in the core; spike runs in the Gallery |
| R1.0 | Productive foundation | R1 components, Gallery, CRUD/workbench/utility samples, NuGet | First real SASD application uses published packages |
| R1.1 | Stabilization | API cleanup, performance, migration, documentation | Three real SASD projects use core modules |
| R2.0 | Optional specialist modules | Charts, Markdown, code, diff, images, PDF, barcode, WebView2, docking | Every adapter has security, licensing, and lifecycle evidence |
| R3 | Proven additional modules | Wizard, Ribbon, and only extensions with demonstrated need | Separate business case, ADR, and maintenance ownership |

## 4. R0.1 – Architecture and Repository Foundation

### Goals

- Create the `SASD-UI-Plattform` repository.
- Establish the solution and project structure defined by the architecture document.
- Configure `Directory.Build.props`, `Directory.Packages.props`, `global.json`, and analyzers.
- Add architecture tests for dependency direction and public API boundaries.
- Prepare the ADR process, license inventory, and third-party notices.
- Build minimal `Sasd.Ui.Core` and WinForms foundation packages/projects.

### Implemented

- reproducible Windows restore/build in GitHub Actions;
- warnings treated as errors in CI;
- architecture checks for cycles, platform-neutral `Core`, product-reference boundaries and Krypton isolation;
- central package version management;
- ADR and third-party notice baseline;
- executable Component Gallery and smoke-check projects.

### Remaining Gate Work

- establish repeatable NuGet packaging and symbols as an explicit release path;
- add public-API baseline tooling before API stability is claimed;
- add SBOM/release-engineering helpers under `eng/` when packaging starts.

### Stop Criteria

R0.1 is not complete if:

- package dependencies are cyclic;
- third-party types leak uncontrolled into public core APIs;
- the build depends on local machine state, the GAC, or manually copied DLLs;
- the license status of a core dependency is unresolved.

## 5. R0.2 – UI Technology Pilot

### Candidates

1. native WinForms controls;
2. Krypton Standard Toolkit as the primary visible implementation candidate;
3. AntdUI/ReaLTaiizor only as separate comparison prototypes, never as a mixed visual system.

### Implemented Pilot Scope

- vendor-neutral theme definitions for Light, Dark and High Contrast;
- native `SasdThemeService`;
- `SasdForm`, `SasdDialogForm`, and `SasdUserControl` foundation;
- `SasdSectionPanel` and `SasdFieldLayout`;
- isolated `Sasd.Ui.WinForms.Krypton` project using a centrally pinned Krypton package;
- automated architecture rule preventing Krypton implementation types from leaking into native product projects;
- dedicated Krypton smoke check.

### Still Required Before the Technology Decision

- designer serialization and reopening in Visual Studio;
- 100/125/150/200 percent DPI and mixed-monitor checks;
- keyboard focus traversal and high-contrast verification;
- representative native-versus-Krypton Gallery pages;
- explicit decision record update stating which visual implementation becomes the default.

### Decision Outcome

Krypton becomes the standard if it:

- works reproducibly in the designer;
- does not degrade DPI or focus behavior;
- supports High Contrast and keyboard operation adequately;
- does not introduce unacceptable dependencies or API leakage.

Otherwise, the native WinForms implementation becomes the standard. Design tokens and contracts remain independent of the result.

## 6. R0.3 – Grid, Dialog, State, and Test Pilot

### Implemented or In Progress

- `SasdDataGrid` plus `SasdGridController<T>`;
- `SasdSearchBox`, `SasdFilterBar`, `SasdEmptyState`, and `SasdBusyOverlay`;
- vendor-neutral paging, sorting and filter descriptors;
- CSV export and persisted grid-column state;
- `SasdDialogService`, `SasdErrorDialog`, `SasdProgressDialog`, and `SasdDialogForm`;
- `SasdStateStore` with versioning, backup recovery, incremental migration and reset;
- `SasdRecentItemsService`;
- strict smoke coverage for commands, grid/state, forms, dispatcher, shell safety and Krypton.

### Evidence Still Required for the Gate

- UI Automation through UIA3/FlaUI as a pilot;
- screenshot regression for defined Gallery states;
- at least one keyboard-only end-to-end scenario;
- broader Component Gallery coverage of the implemented components;
- defined manual DPI/accessibility checklist results stored with the release evidence.

## 7. R1.0 – Productive Foundation

### Binding Product Areas

- base classes and layout;
- themes and icons;
- commands, shell, navigation, breadcrumbs, and document tabs;
- grids, lists, trees, search, filtering, and paging contracts;
- dialogs, notifications, errors, and progress;
- file, clipboard, drag-and-drop, tray, and shell integration;
- UI state, recent items, and secure defaults;
- Component Gallery and three reference applications;
- NuGet, symbols, XML documentation, SBOM, and third-party notices.

### Near-Term Implementation Order

Unless a real SASD consumer exposes a more urgent need, prefer the following order:

1. finish the low-risk R1 shell/data-view controls already defined by the specification;
2. add semantic icon service and notification service;
3. add drag-and-drop and tray services with explicit safety boundaries;
4. expand Gallery coverage and keyboard/DPI/accessibility evidence;
5. establish NuGet packaging and API-baseline checks;
6. migrate the first bounded feature from a real SASD application, preferably Prompt Manager;
7. use migration feedback to simplify APIs before R1.0 is declared stable.

### Pilot Migration

The first adoption target is a small, clearly bounded application or feature. Prompt Manager is suitable for themes, form layout, dialogs, search/filtering, commands and UI state. A complete navigation redesign is not mandatory for R1.

## 8. R1.1 – Stabilization

- API review focused on naming, nullability, error models, and cancellation;
- performance and handle-leak tests;
- migration notes for all breaking changes;
- real use in Prompt Manager, Mail Workbench, and one utility application;
- reduction of direct package dependencies for consumers;
- documentation of known boundaries instead of hidden special cases.

## 9. R2.0 – Specialist Modules

R2 modules are separate packages and are not installed automatically.

| Module | Pilot | Admission Condition |
| --- | --- | --- |
| Charts | ScottPlot | Concrete dashboard or monitoring requirement |
| KPI/Sparkline | Custom wrapper based on chart support | Textual accessibility alternative exists |
| Markdown/Diff | Markdig/DiffPlex | Notes or Mail Workbench scenario demonstrated |
| Code editor | ScintillaNET | Lifecycle and large-file behavior tested |
| Images/QR/barcode | Specialist adapters | Licensing and export formats reviewed |
| PDF | PDFsharp/MigraDoc | Programmatic output is sufficient; no designer required |
| WebView2 | Microsoft WebView2 | Default-deny, allowlist, and download rules tested |
| Docking | Krypton Docking/Workspace | Layout reset and resource disposal are reliable |

## 10. R3 and Deliberate Non-Goals

Wizard and Ribbon components are developed only if at least two real applications demonstrate the same need, or if one strategic product has a documented business case.

This roadmap does not include Pivot/OLAP, spreadsheet engines, Office editors, report/dashboard designers, Gantt, complex scheduling, 3D, mobile controls, or a cross-platform rewrite.

## 11. Roadmap Maintenance

The roadmap is updated at every gate review and whenever implementation progress materially changes the next useful development step. New components may not be added directly to R1/R2. Admission requires:

1. a demonstrated application scenario;
2. scope and maintenance evaluation;
3. a build-versus-buy-versus-adapter decision;
4. licensing and security review;
5. an ADR for architecture-relevant impact;
6. allocation to a release, Gallery page, and test matrix.
