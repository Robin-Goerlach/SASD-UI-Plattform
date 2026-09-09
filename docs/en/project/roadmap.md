# Roadmap – SASD UI Platform

**As of:** 2026-07-23  
**Version:** 0.1  
**Horizon:** R0 through R3; no artificial calendar dates without reliable capacity planning

## 1. Roadmap Principles

The roadmap prioritizes **usability over component count**. A component does not represent progress merely because it has been implemented. Progress exists when the component:

- is demonstrated completely in the Component Gallery;
- passes designer, DPI, keyboard, accessibility, and localization checks;
- has a documented public API;
- is used by at least one real SASD application without a local special-purpose copy;
- has an acceptable maintenance and dependency profile.

The product line is not developed simultaneously for WinForms, WPF, web, and Java. The current focus remains WinForms.

## 2. Release Map

| Stage | Goal | Result | Release Criterion |
| --- | --- | --- | --- |
| R0.1 | Architecture and repository foundation | Monorepo, build, core contracts, ADR and licensing process | A clean checkout builds reproducibly |
| R0.2 | UI technology pilot | Krypton versus native WinForms; tokens, theme, base classes, form layout | Designer, DPI, focus, and high-contrast matrix passed |
| R0.3 | Core interaction pilot | Grid, dialogs, errors, progress, UI state, UIA/screenshot pilot | No third-party types in the core; spike runs in the Gallery |
| R1.0 | Productive foundation | R1 components, Gallery, CRUD/workbench/utility samples, NuGet | First real SASD application uses published packages |
| R1.1 | Stabilization | API cleanup, performance, migration, documentation | Three real SASD projects use core modules |
| R2.0 | Optional specialist modules | Charts, Markdown, code, diff, images, PDF, barcode, WebView2, docking | Every adapter has security, licensing, and lifecycle evidence |
| R3 | Proven additional modules | Wizard, Ribbon, and only extensions with demonstrated need | Separate business case, ADR, and maintenance ownership |

## 3. R0.1 – Architecture and Repository Foundation

### Goals

- Create the `SASD-UI-Platform` repository.
- Establish the solution and project structure defined by the architecture document.
- Configure `Directory.Build.props`, `Directory.Packages.props`, `global.json`, and analyzers.
- Add initial architecture tests for dependency direction and public API boundaries.
- Prepare the ADR process, license inventory, and third-party notices.
- Build minimal `Sasd.Ui.Core`, `Sasd.Ui.WinForms`, and `Sasd.Ui.WinForms.Testing` packages.

### Deliverables

- reproducible Debug and Release builds;
- initial internal NuGet packages without production-readiness claims;
- CI with restore, build, unit tests, and architecture tests;
- Component Gallery skeleton;
- documented local developer setup.

### Stop Criteria

R0.1 is not complete if:

- package dependencies are cyclic;
- third-party types leak uncontrolled into public core APIs;
- the build depends on local machine state, the GAC, or manually copied DLLs;
- the license status of a core dependency is unresolved.

## 4. R0.2 – UI Technology Pilot

### Candidates

1. native WinForms controls;
2. Krypton Standard Toolkit as the primary visible implementation;
3. AntdUI/ReaLTaiizor only as separate comparison prototypes, never as a mixed visual system.

### Pilot Scope

- design tokens for color, typography, spacing, radii, and semantic states;
- `SasdThemeService` with Light, Dark, and High Contrast;
- `SasdForm`, `SasdDialogForm`, and `SasdUserControl`;
- `SasdSectionPanel` and `SasdFieldLayout`;
- icon service and semantic standard icons;
- runtime theme switching;
- designer serialization and reopening in the Visual Studio Designer.

### Decision Outcome

Krypton becomes the standard if it:

- works reproducibly in the designer;
- does not degrade DPI or focus behavior;
- supports High Contrast and keyboard operation adequately;
- does not introduce unacceptable dependencies or API leakage.

Otherwise, the native WinForms implementation becomes the standard. Design tokens and contracts remain independent of the result.

## 5. R0.3 – Grid, Dialog, State, and Test Pilot

### Pilot Components

- `SasdDataGrid` plus `SasdGridController<T>`;
- `SasdSearchBox`, `SasdFilterBar`, `SasdEmptyState`, and `SasdBusyOverlay`;
- `SasdDialogService`, `SasdErrorDialog`, and `SasdProgressDialog`;
- `SasdStateStore` with versioning, backup, migration, and reset;
- UI Automation through UIA3/FlaUI as a pilot;
- screenshot regression for defined Gallery states.

### Evidence

- sorting, search, filtering, selection, and CSV export in the grid spike;
- persisted grid-column state without database coupling;
- a corrupted state file does not prevent application startup;
- cancellation/progress and error completion behave predictably;
- at least one keyboard-only end-to-end scenario is automated.

## 6. R1.0 – Productive Foundation

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

### Pilot Migration

The first adoption target is a small, clearly bounded application or feature. Prompt Manager is suitable for themes, form layout, dialogs, search/filtering, and grid state. A complete navigation redesign is not mandatory for R1.

## 7. R1.1 – Stabilization

- API review focused on naming, nullability, error models, and cancellation;
- performance and handle-leak tests;
- migration notes for all breaking changes;
- real use in Prompt Manager, Mail Workbench, and one utility application;
- reduction of direct package dependencies for consumers;
- documentation of known boundaries instead of hidden special cases.

## 8. R2.0 – Specialist Modules

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

## 9. R3 and Deliberate Non-Goals

Wizard and Ribbon components are developed only if at least two real applications demonstrate the same need, or if one strategic product has a documented business case.

This roadmap does not include Pivot/OLAP, spreadsheet engines, Office editors, report/dashboard designers, Gantt, complex scheduling, 3D, mobile controls, or a cross-platform rewrite.

## 10. Roadmap Maintenance

The roadmap is updated at every gate review. New components may not be added directly to R1/R2. Admission requires:

1. a demonstrated application scenario;
2. scope and maintenance evaluation;
3. a build-versus-buy-versus-adapter decision;
4. licensing and security review;
5. an ADR for architecture-relevant impact;
6. allocation to a release, Gallery page, and test matrix.
