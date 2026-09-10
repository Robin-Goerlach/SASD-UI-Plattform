# Roadmap – SASD UI Platform

**As of:** 2026-09-10  
**Version:** 0.4  
**Horizon:** R0 through R3; no artificial calendar dates without reliable capacity planning

## 1. Roadmap Principles

The roadmap prioritizes **usability and evidence over component count**. A component does not represent release progress merely because a class exists. Progress exists when the component:

- is demonstrated appropriately in the Component Gallery;
- passes relevant designer, DPI, keyboard, accessibility and localization checks;
- has a documented public API and clear ownership/lifecycle rules;
- is exercised by automated behavioral checks where practical;
- is used by a real SASD application before the API is declared stable;
- has an acceptable maintenance, security and dependency profile.

The product line is not developed simultaneously for WinForms, WPF, web and Java. The current implementation focus remains **Windows + .NET 8 + WinForms**.

## 2. Current Implementation Status

The repository now contains a broad native WinForms foundation rather than an R0 scaffold. It is guarded by strict Windows CI, architecture checks, dependency-free smoke executables and a shared `build/verify.ps1` verification entry point.

The status below distinguishes **implemented code** from **completed release gates**. A gate is not marked complete merely because most of its classes exist.

| Stage | Current status | Evidence already present | Important work still open |
| --- | --- | --- | --- |
| R0.1 | Functionally implemented; release-engineering gate still open | .NET 8 solution, central package/build configuration, strict warning-free CI, architecture checks, ADR/licence process, one-command verification | NuGet packaging, public-API baseline, SBOM/release automation |
| R0.2 | Pilot implemented; decision gate still open | native theme/form baseline, isolated `Krypton.Toolkit` adapter, Krypton smoke check, automated vendor-boundary enforcement | Visual Studio Designer matrix, multi-DPI/focus/high-contrast evidence and final native-vs-Krypton standard decision |
| R0.3 | Core interaction implementation substantially complete | grid/controller/paging/search/filter, CSV/grid state, dialogs/error/busy/progress, versioned state, commands/shell, executable Gallery integration, integrated Showcase consumer | UIA/FlaUI pilot, screenshot regression, keyboard-only end-to-end evidence, manual DPI/accessibility evidence |
| R1.0 | Functional component foundation largely implemented | forms/validation, commands/shortcuts, shell/navigation/status, grids/lists/trees, dialogs/notifications, Windows services, state/recent items, drag/drop/tray, Gallery plus CRUD/Workbench/Showcase consumers | NuGet publication, API baseline, first real SASD consumer migration, formal quality matrix |
| R1.1 | Hardening occurs continuously; formal stage not started | analyzer-as-error policy, recovery/disposal/threading fixes and growing smoke coverage | formal API review, performance/handle-leak matrix, migration notes and three real SASD consumers |
| R2.0 | Started with dependency-free native helpers | property editor, column chooser, image viewer, saved grid views, sparkline/KPI, Gallery integration and Showcase exercises | manual DPI/accessibility evidence, real consumer evidence and separately reviewed specialist adapters |
| R3 | Deferred | scope rules documented | requires demonstrated product need and explicit maintenance ownership |

## 3. Release Map

| Stage | Goal | Result | Release Criterion |
| --- | --- | --- | --- |
| R0.1 | Architecture and repository foundation | Monorepo, build, core contracts, ADR/licensing process, common verification entry point | Clean checkout builds reproducibly and structural rules are enforced |
| R0.2 | UI technology pilot | Krypton versus native WinForms; tokens, theme, base classes, form layout | Designer, DPI, focus and High-Contrast matrix passed and decision recorded |
| R0.3 | Core interaction pilot | Grid, dialogs, errors, progress, UI state, integrated Gallery, UIA/screenshot pilot | Core remains vendor-neutral; automated and visual/user-flow evidence exists |
| R1.0 | Productive foundation | R1 components, Gallery, reference samples, NuGet | First real SASD application uses published packages successfully |
| R1.1 | Stabilization | API cleanup, performance, migration, documentation | Three real SASD projects use core modules without local substitute copies |
| R2.0 | Optional specialist modules | dependency-free native helpers plus separately approved adapters | Each adapter has need, security, licensing, lifecycle and exit evidence |
| R3 | Proven additional modules | Wizard, Ribbon and only extensions with demonstrated demand | Separate business case, ADR and maintenance ownership |

## 4. R0.1 – Architecture and Repository Foundation

### Implemented

- reproducible .NET 8 Windows restore/build in GitHub Actions;
- warnings treated as errors in the strict Release build;
- architecture checks for cycles, platform-neutral `Core`, product-reference boundaries and Krypton isolation;
- central package version management;
- ADR and third-party notice baseline;
- executable Component Gallery and focused smoke-check projects;
- repository-wide `AGENTS.md` for coding-agent boundaries;
- shared `build/verify.ps1` used by developers, Codex and GitHub Actions.

### Remaining Gate Work

- establish repeatable NuGet packaging and symbols as an explicit release path;
- add public-API baseline tooling before API stability is claimed;
- add SBOM, checksums and release-engineering helpers under `eng/` when packaging starts;
- make release evidence reproducible rather than relying on manual notes.

### Stop Criteria

R0.1 is not complete if:

- package dependencies are cyclic;
- third-party types leak uncontrolled into neutral/core public APIs;
- the build depends on local machine state, the GAC or manually copied DLLs;
- the license status of a runtime dependency is unresolved;
- local/agent validation and CI silently use different acceptance commands.

## 5. R0.2 – UI Technology Pilot

### Candidates

1. native WinForms controls;
2. Krypton Standard Toolkit as the primary visible implementation candidate;
3. AntdUI/ReaLTaiizor only as separate comparison prototypes if a later need justifies them, never as a mixed visual system.

### Implemented Pilot Scope

- vendor-neutral theme definitions for Light, Dark and High Contrast;
- native `SasdThemeService`;
- `SasdForm`, `SasdDialogForm`, and `SasdUserControl` foundation;
- `SasdSectionPanel` and `SasdFieldLayout`;
- isolated `Sasd.Ui.WinForms.Krypton` project using a centrally pinned Krypton package;
- automated architecture rule preventing Krypton implementation types from leaking into native product projects;
- dedicated Krypton regression smoke check.

### Still Required Before the Technology Decision

- designer serialization and reopening in Visual Studio;
- 100/125/150/200 percent DPI and mixed-monitor checks;
- keyboard focus traversal and High-Contrast verification;
- representative native-versus-Krypton Gallery pages;
- explicit ADR update stating which visible implementation becomes the default.

### Decision Outcome

Krypton becomes the standard only if it:

- works reproducibly in the designer;
- does not degrade DPI or focus behavior;
- supports High Contrast and keyboard operation adequately;
- does not introduce unacceptable dependencies or public-API leakage.

Otherwise, the native WinForms implementation remains the standard. Design tokens and neutral contracts remain independent of the result.

## 6. R0.3 – Grid, Dialog, State, and Test Pilot

### Implemented

- `SasdDataGrid` plus `SasdGridController<T>`;
- `SasdSearchBox`, `SasdFilterBar`, `SasdEmptyState`, `SasdPager` and `SasdBusyOverlay`;
- vendor-neutral paging, sorting and filter descriptors;
- CSV export, persisted grid-column state and saved grid-view definition;
- `SasdDialogService`, `SasdErrorDialog`, `SasdProgressDialog`, `SasdDialogForm` and notification primitives;
- `SasdStateStore` with versioning, backup recovery, incremental migration and reset;
- `SasdRecentItemsService`;
- command manager, global shortcuts, command bar, navigation, status service and shell form;
- strict smoke coverage for commands, grid/state, forms, dispatcher, Windows safety, shell integration, native R2 helpers and Krypton;
- Component Gallery integration shell demonstrating commands, status, notifications and safe drag/drop;
- integrated `Sasd.Ui.PlatformShowcase` consumer with manual exercises and safe public-API self tests across the current modules.

### Evidence Still Required for the Gate

- UI Automation through UIA3/FlaUI as a pilot;
- screenshot regression for defined Gallery states;
- at least one keyboard-only end-to-end scenario;
- broader Component Gallery coverage of remaining implemented controls and important states;
- stored manual DPI/Designer/Accessibility Insights results.

## 7. R1.0 – Productive Foundation

### Implemented Product Areas

The following R1 areas now have usable implementation code:

- base classes and form layout;
- validation coordination and validation summary;
- themes and semantic icons;
- commands, command bar, command manager and global shortcuts;
- shell, navigation, breadcrumbs, document tabs and status;
- grids, typed controller, lists, trees, search, filtering and paging contracts;
- dialogs, notifications, errors, busy and progress;
- file/folder dialogs, clipboard, safe drag-and-drop, tray and constrained shell integration;
- versioned UI state, migrations, recent items and window/grid state;
- executable Component Gallery integration host;
- focused in-memory CRUD and document/workbench reference applications;
- broad integrated Showcase consumer that exercises the platform in one application-like process.

### Remaining R1 Release Work

The next work is no longer mainly “add missing base controls”. It is now:

1. expand Gallery coverage for remaining implemented R1 controls and important states;
2. add keyboard-only and UIA automation for business-critical flows;
3. execute and record Designer, DPI, High-Contrast and accessibility matrices;
4. establish NuGet packaging, symbols and XML-documentation output;
5. introduce public-API baseline/compatibility checks;
6. produce SBOM/third-party release evidence;
7. migrate a bounded feature from a real SASD application, preferably Prompt Manager;
8. simplify APIs based on reference-consumer and real-consumer feedback before declaring R1.0 stable.

### Pilot Migration

The first adoption target should remain small and reversible. Prompt Manager is suitable for themes, form layout, dialogs, search/filtering, commands, notifications and UI state. A complete application navigation redesign is not required for the first migration.

## 8. R1.1 – Stabilization

R1.1 begins after the first real consumer uses packaged R1 APIs.

Primary work:

- formal API review focused on naming, nullability, result/error models and cancellation;
- performance tests against documented reference cases rather than speculative micro-benchmarks;
- handle/resource endurance tests, including repeated open/close cycles;
- migration notes for all breaking changes;
- real use in Prompt Manager, Mail Workbench and one utility application;
- reduction of unnecessary direct package dependencies for consumers;
- documentation of known boundaries instead of hidden special cases.

## 9. R2.0 – Native Helpers and Specialist Modules

R2 work has started, but there are two different categories.

### 9.1 Dependency-free native R2 helpers implemented

These provide common usability without introducing a new runtime vendor decision:

- `SasdPropertyEditor` — searchable property projection and forced read-only presentation;
- `SasdGridColumnChooser` — column visibility/search helper with explicit grid binding lifecycle;
- `SasdGridViewDefinition` — serializable grid layout + search + active-filter UI state;
- `SasdImageViewer` — fit/actual/custom zoom with explicit cloned-image ownership;
- `SasdSparkline` — compact finite-value trend rendering;
- `SasdKpiCard` — accessible textual KPI plus optional sparkline context.

These helpers now have integrated Gallery and Showcase exercises, but still require broader DPI/accessibility evidence and real-consumer feedback before being considered stable.

### 9.2 Specialist adapter candidates

Specialist adapters are separate packages and are not installed automatically.

| Module | Candidate/Pilot | Admission Condition |
| --- | --- | --- |
| Rich charts | ScottPlot first | Concrete dashboard/monitoring need not covered by native KPI/Sparkline |
| Markdown | Markdig + suitable editor/preview | Notes/Mail scenario demonstrated |
| Diff | DiffPlex or equivalent | Real comparison workflow demonstrated |
| Code editor | ScintillaNET | Lifecycle and large-file behavior tested |
| QR/barcode | QRCoder / ZXing.Net behind service | Real import/export requirement and license review |
| PDF | PDFsharp/MigraDoc | Programmatic output is sufficient; no designer required |
| Web content | Microsoft WebView2 | Default-deny navigation/download/permission/message rules tested |
| Docking | Krypton Docking/Workspace or approved alternative | Layout reset, designer behavior and disposal reliable |

A new runtime dependency requires the dependency/licence review and, where architecture-relevant, an ADR before implementation.

## 10. Coding-Agent / Codex Enablement

The repository now supports controlled agent-assisted development and an explicit queue of bounded tasks.

- root `AGENTS.md` defines durable autonomy and escalation boundaries;
- scoped `AGENTS.md` files refine source/test guidance without replacing the architecture documents;
- detailed guidance lives in `docs/en/project/codex-development-workflow.md`;
- `eng/codex/preflight.ps1` checks the local repository/tooling shape without modifying credentials or machine policy;
- `.github/ISSUE_TEMPLATE/codex_task.md` provides a bounded task format;
- `eng/codex/READY_TASKS.md` records the current ready-work queue and completion contract;
- `build/verify.ps1` provides one local/agent/CI verification entry point;
- Codex may autonomously implement already-approved roadmap/specification work and normal hardening fixes;
- Codex must stop before new runtime dependencies, platform changes, major package-boundary changes, breaking public APIs or other strategic choices.

The goal is not unrestricted autonomous change. The goal is to let agents carry routine implementation, testing and documentation work far enough that human attention is reserved for genuine product/architecture decisions.

## 11. Near-Term Priority Order

Unless a real consumer exposes a more urgent defect, prefer this order:

1. use the Codex ready queue for bounded lifecycle, evidence and release-engineering work while keeping agent instructions concise/current;
2. complete a current component-coverage matrix and fill remaining dependency-free Gallery/Showcase gaps;
3. implement a first keyboard-only/UIA3/FlaUI automated user flow after the required dependency/tooling decision;
4. execute manual Designer/DPI/High-Contrast/accessibility matrix and store evidence;
5. implement NuGet pack/symbol/XML-doc release path;
6. add public-API compatibility baseline checks;
7. add SBOM/checksum/third-party release automation;
8. migrate the first bounded real SASD consumer feature;
9. use consumer feedback to harden/simplify R1 and native R2 APIs;
10. only then promote a specialist R2 adapter unless a real project need makes one urgent sooner.

## 12. R3 and Deliberate Non-Goals

Wizard and Ribbon components are developed only if at least two real applications demonstrate the same need, or if one strategic product has a documented business case.

This roadmap does not currently include Pivot/OLAP, spreadsheet engines, Office editors, report/dashboard designers, Gantt, complex scheduling, 3D, mobile controls, or a cross-platform rewrite.

## 13. Roadmap Maintenance

The roadmap is updated at every gate review and whenever implementation progress materially changes the next useful development step.

New components may not be added directly to stable scope merely because they are technically interesting. Admission requires, proportionate to impact:

1. a demonstrated or already-approved application scenario;
2. scope and maintenance evaluation;
3. build-versus-buy-versus-adapter decision;
4. licensing and security review for new dependencies;
5. an ADR for architecture-relevant impact;
6. allocation to a release, Gallery page and test matrix.

For already-approved dependency-free R1/R2 items, implementation may proceed without a new strategic decision as long as existing architecture and public boundaries are preserved.
