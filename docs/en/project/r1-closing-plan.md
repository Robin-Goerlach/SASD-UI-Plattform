# R1.0 Closing Plan

**Status:** Active closing plan  
**Baseline:** repository `main` at/after `6e72c6f` (2026-09-24)  
**Scope:** SASD UI Platform R1 on Windows / .NET 8 / WinForms

## 1. Purpose

R1 has reached the point where adding more controls would reduce rather than increase release confidence. The reusable foundation is broad enough for the first real SASD adoption. The remaining work therefore focuses on proving, packaging and stabilising what already exists.

The closing goal is not to imitate the complete DevExpress, Telerik or Syncfusion catalog. R1.0 is successful when a real SASD WinForms application can consume a small, documented and maintainable set of SASD UI packages and gain a coherent application foundation without local substitute implementations.

Until R1.0 closes, new R1 controls are accepted only for a demonstrated blocking gap in a real consumer or for a defect in an already approved contract.

## 2. Current baseline

The current product line already includes:

- native WinForms base forms and reusable form layout;
- validation coordination, summary presentation and field navigation;
- themes, semantic icons and an isolated Krypton pilot;
- commands, global shortcuts, shell/navigation, breadcrumbs, document tabs and status;
- DataGrid/controller, search, filters, paging, native ListView virtual-mode evidence and lazy TreeView loading;
- dialogs, error presentation, notifications, busy/progress and cooperative cancellation contracts;
- constrained Windows integration for dialogs, clipboard, drag/drop, tray and shell operations;
- versioned UI state, migration, backup/recovery, form/grid state and recent items;
- dependency-free native R2 helpers that are already in the repository;
- Component Gallery, CRUD, Workbench, Utility and Integrated Showcase consumers;
- strict Windows CI, architecture checks, behavioural smoke suites and a repeatable NuGet pack dry-run.

This is enough functional breadth for R1.0. The remaining risk is evidence, public API/package stability and real-consumer feedback.

## 3. R1 feature freeze

During the closing phase:

- fix defects in existing R1 behaviour;
- strengthen lifecycle, accessibility, keyboard, nullability and error handling;
- improve documentation, samples and release evidence;
- make additive compatibility-safe API corrections when real usage demonstrates a need;
- do not add speculative controls, visual systems or new runtime dependencies;
- do not promote optional R2/R3 adapters merely to increase component count.

R2 native helpers already present may receive defect fixes and evidence work, but they must not distract from the R1 gate.

## 4. Closing workstreams

### A. Repository and automated release evidence

Complete without changing product strategy:

1. keep the canonical Windows verification gate green;
2. keep package metadata, README, XML documentation and symbol-package checks reproducible;
3. add release checksums and repeatable SBOM/third-party evidence using approved tooling or SDK capabilities;
4. establish a public API inventory/baseline before R1 compatibility is claimed;
5. keep the Component Gallery, Showcase and reference consumers buildable from a clean checkout;
6. keep the evidence matrix current after each closing change.

### B. Manual Windows acceptance

Record evidence rather than inferring it from compilation:

1. Visual Studio Designer open/edit/save/reopen for designer-supported controls;
2. 100%, 125%, 150% and 200% display scaling;
3. mixed-DPI monitor movement where the available hardware permits it;
4. real Windows High Contrast;
5. keyboard-only use of representative business flows;
6. screen-reader/UI Automation inspection of representative controls;
7. localisation/long-label review for layouts that are intended to support translated applications.

Automated accessible names and smoke checks remain useful, but they do not replace these manual gates.

### C. Strategic decisions that require explicit approval

Do not silently decide these during routine implementation:

1. **public NuGet topology** — internal projects are currently more granular than the intended consumer package surface;
2. **native WinForms vs Krypton default visual implementation** — decide only after Designer/DPI/focus/High-Contrast evidence;
3. **UI Automation/screenshot-regression tooling** — a new third-party dependency or toolchain needs an explicit decision;
4. any new specialist R2 adapter/runtime dependency.

The current pack dry-run proves technical packability of the internal product projects. It does not declare that all of those projects become separate public packages.

### D. First real SASD adoption

Before R1.0 is declared stable, migrate one bounded feature from a real SASD application.

The preferred first target remains **SASD Prompt Manager** because it can exercise a useful R1 cross-section without forcing a complete application rewrite:

- theme/application shell integration;
- form layout and validation where appropriate;
- search/filter UI;
- commands;
- dialogs/notifications;
- UI state.

The migration must stay reversible. Domain persistence, JSON/business models and application-specific navigation remain owned by Prompt Manager.

Feedback from this migration is allowed to change R1 APIs before 1.0 when the change materially improves maintainability or corrects a poor contract. Breaking changes require explicit review and migration notes.

## 5. R1.0 exit criteria

R1.0 may be declared only when all of the following are true:

- the agreed public package topology is documented and verified;
- a clean checkout produces the intended packages, symbols and XML documentation reproducibly;
- public API baseline/compatibility evidence exists;
- SBOM, third-party notices, checksums and known limitations are produced for the release candidate;
- the Designer/DPI/High-Contrast/accessibility acceptance matrix has been executed and recorded;
- critical keyboard/business flows have sufficient automated and/or recorded manual evidence;
- one bounded real SASD application feature uses the packaged platform successfully;
- consumer feedback has been reviewed and high-risk API problems are resolved or explicitly documented;
- the complete Windows verification gate is green from the release candidate;
- changelog, migration notes and release notes describe the candidate accurately.

A large component count is not an exit criterion.

## 6. Recommended execution order

Unless a real consumer exposes a defect with higher priority:

1. refresh roadmap, evidence matrix and Codex queue to this closing plan;
2. close dependency-free automated evidence gaps and release hygiene;
3. choose and implement the public-API baseline approach;
4. execute the manual Designer/DPI/High-Contrast/accessibility matrix;
5. make the explicit public-package and native-vs-Krypton decisions using the collected evidence;
6. produce an internal release candidate;
7. migrate the bounded Prompt Manager feature against the packaged candidate;
8. fix/simplify R1 APIs based on that migration;
9. produce final release evidence and tag/publish R1.0 internally.

## 7. What comes after R1.0

R1.1 focuses on stabilisation across additional real SASD consumers, performance/handle evidence and migration support.

R2 then becomes the main place for visibly richer professional components and opt-in adapters such as charts, Markdown/diff/code editing, PDF/media, WebView2 and docking. Those modules remain demand-driven and separately reviewed.

R3 stays reserved for components with a demonstrated business case, such as Wizard/Ribbon-class features. Spreadsheet, Pivot/OLAP, Office-editor, report-designer, complex scheduling/Gantt and similar product-scale engines remain out of scope unless a later decision explicitly changes that boundary.
