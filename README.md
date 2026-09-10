# SASD UI Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4)
![Framework](https://img.shields.io/badge/.NET-8.0-512BD4)
![UI](https://img.shields.io/badge/UI-WinForms-5C2D91)
![Status](https://img.shields.io/badge/status-R1%20foundation%20%2F%20R2%20native%20work-orange)

**A modular, designer-friendly C# WinForms UI platform for consistent SASD desktop applications.**

The project provides reusable application shells, forms, commands, data views, dialogs, themes, UI-state persistence, Windows integration, optional specialist adapters, reference applications and enforceable quality standards. It is intentionally focused on **Windows, .NET 8 and WinForms** before any later WPF or web product line.

**Project links:** [Roadmap](ROADMAP.md) · [Architecture](docs/en/core/04-architecture.md) · [Requirements](docs/en/core/02-requirements-specification.md) · [Functional specification](docs/en/core/03-functional-specification.md) · [Integrated showcase](examples/Sasd.Ui.PlatformShowcase/README.md) · [Codex workflow](docs/en/project/codex-development-workflow.md) · [English documentation](docs/en/README.md) · [German documentation](docs/de/README.md)

![Concept design for the SASD UI Platform Component Gallery](artefacts/sasd-ui-platform-component-gallery.png)

> **Design target:** The image above is a concept screenshot for the Component Gallery. It communicates the intended structure and visual quality; it is not presented as a screenshot of a completed implementation.

## Project status

The repository has moved beyond the initial R0 scaffold. The current `main` line contains a reusable native WinForms foundation, an isolated Krypton R0.2 adapter pilot, an executable R1 integration Gallery, dependency-free native R2 helpers, strict Windows CI, architecture checks and behavioral smoke checks.

Implemented foundations include themes, forms and validation, commands and shortcuts, shell/navigation/status, DataGrid support, paging/search/filter contracts, saved grid views, dialogs and progress, notifications, versioned UI state with backup/migration, recent items, file/clipboard/drag-and-drop/tray/shell services, property editing, column choosing, image viewing, KPI/sparkline primitives and the Component Gallery host.

The project is **not yet claiming R1.0 stability**. Important acceptance work remains, especially the Visual Studio Designer/DPI/focus/high-contrast matrix, UI Automation/screenshot regression, broader Gallery coverage, packaging/API-baseline tooling and adoption by real SASD applications. See the [Roadmap](ROADMAP.md) for the current gate status.

## Product goals

- reduce duplicated UI infrastructure across SASD WinForms applications;
- provide a coherent design system without wrapping every primitive WinForms control;
- keep Visual Studio WinForms Designer support practical;
- isolate third-party libraries behind explicit package and adapter boundaries;
- treat DPI, keyboard operation, accessibility, localisation and resource ownership as release criteria;
- support incremental migration of existing SASD applications;
- remain small enough to maintain with realistic SASD capacity.

## Current scope

### R1 foundation

- base forms, reusable views and semantic form layout;
- design tokens, Light/Dark/High-Contrast handling and icon services;
- application shell, commands, navigation, breadcrumbs, document tabs and status;
- DataGrid, typed grid controller, lists, trees, search, filters, paging and CSV;
- dialogs, errors, notifications, busy and progress handling;
- file/folder dialogs, clipboard, drag-and-drop, tray and safe shell integration;
- versioned JSON UI state, migration, backup, reset and recent items;
- Component Gallery, focused CRUD/Workbench reference applications and the integrated showcase consumer.

### Native R2 foundation

The first R2 work intentionally uses no new third-party runtime dependency:

- `SasdPropertyEditor` with filtered/read-only property projection;
- `SasdGridColumnChooser` and `SasdGridViewDefinition`;
- `SasdImageViewer` with explicit cloned-image ownership and zoom modes;
- `SasdSparkline` and `SasdKpiCard` as small accessible dashboard primitives.

### Optional specialist R2/R3 modules

Rich charts, Markdown/code/diff editors, barcode/QR, PDF generation, WebView2, docking, wizard and optional ribbon are delivered only through separate modules after individual evaluation. The small native Sparkline/KPI controls do not replace the later chart-adapter decision.

### Deliberately deferred

WPF, WinUI 3, Avalonia, modern ASP.NET Core/Blazor, ASP.NET Web Forms/ASPX, Java/OpenXava, Android, iOS, macOS and Linux desktop implementations are recorded in the future register but are not part of the current implementation scope.

## Architecture at a glance

```text
SASD application
  -> stable SASD component and service APIs
       -> Sasd.Ui.Core
       -> Sasd.Ui.WinForms foundations and feature modules
       -> optional named adapters
            -> Krypton / ScottPlot / ScintillaNET / WebView2 / approved dependency
```

The architecture is based on four rules:

1. **Contracts first:** Core APIs use SASD and .NET types, not accidental vendor types.
2. **Composition over inheritance:** Behaviour is composed through services, controllers and small base classes.
3. **Designer first:** Public visual controls remain safe and useful in Visual Studio WinForms Designer.
4. **No megafork:** Normal dependencies and upstream collaboration are preferred over copying projects into one codebase.

## Repository structure

```text
SASD-UI-Plattform/
├─ AGENTS.md               Persistent map/instructions for coding agents such as Codex
├─ src/                    Product projects and optional adapters
├─ tests/                  Unit, integration, architecture, UI and visual tests
├─ samples/                Component Gallery and focused reference applications
├─ examples/               Larger integrated application-like demonstrations
├─ templates/              Planned project and application templates
├─ docs/
│  ├─ en/                  English normative and project documentation
│  └─ de/                  German companion/source documentation
├─ artefacts/              Concept screenshot and architecture diagrams
├─ eng/                    Engineering policies/configuration (not English docs)
├─ build/                  Reproducible build/test/package entry points
└─ .github/                Issue, pull-request and CI configuration
```

`eng/` is short for **engineering**. It is reserved for API-baseline, SBOM, licence/dependency and release-engineering assets. See [`eng/README.md`](eng/README.md).

## Documentation

### English

| Document | Purpose |
| --- | --- |
| [Roadmap](ROADMAP.md) | Current release stages, gate status and near-term implementation order |
| [Component catalogue](docs/en/core/01-component-catalog.md) | Commercial/open-source market inventory and component classes |
| [Requirements specification](docs/en/core/02-requirements-specification.md) | Product requirements, priorities, exclusions and acceptance criteria |
| [Functional specification](docs/en/core/03-functional-specification.md) | Technical implementation commitments and component catalogue |
| [Architecture](docs/en/core/04-architecture.md) | Module, runtime, package, quality and security architecture |
| [Integrated showcase](examples/Sasd.Ui.PlatformShowcase/README.md) | Broad runnable demonstration and manual exercise surface for current components |
| [Codex development workflow](docs/en/project/codex-development-workflow.md) | Agent autonomy, escalation boundaries, task format and verification workflow |
| [Project documentation index](docs/en/README.md) | Development, testing, CI/CD, release, security, governance and ADRs |

### Deutsch

Die vollständigen deutschen Ausgangs- und Begleitdokumente befinden sich unter [`docs/de`](docs/de/README.md). English is the repository and code language; German documentation is retained as the source-language companion edition. Der [Codex-Entwicklungsworkflow](docs/de/project/codex-entwicklungsworkflow.md) ist ebenfalls auf Deutsch verfügbar.

## Implemented source modules

The `src` folder follows the package boundaries defined by the architecture. Current modules include:

- `Sasd.Ui.Core` — platform-neutral contracts, theme semantics and result models;
- `Sasd.Ui.WinForms` — native WinForms base classes and UI-thread infrastructure;
- `Sasd.Ui.WinForms.Theming` — native theme application;
- `Sasd.Ui.WinForms.Forms` — field layout, sections, validation and native property editor;
- `Sasd.Ui.WinForms.Data` — grids, paging, search/filter contracts, CSV/state helpers, saved views and small KPI/sparkline primitives;
- `Sasd.Ui.WinForms.Commands` — reusable command state/execution/binding;
- `Sasd.Ui.WinForms.Shell` — navigation, command-bar integration and status foundations;
- `Sasd.Ui.WinForms.Dialogs` — dialogs, notifications, error presentation, busy/progress;
- `Sasd.Ui.WinForms.State` — versioned JSON UI state and recent items;
- `Sasd.Ui.WinForms.Windows` — constrained Windows integration services;
- `Sasd.Ui.WinForms.Media` — dependency-free image-viewing foundation;
- `Sasd.Ui.WinForms.Krypton` — isolated Krypton technology pilot;
- `samples/Sasd.Ui.ComponentGallery` — executable specification and acceptance host.

## Reference consumers and integrated example

The repository intentionally contains more than one consumer shape so awkward public APIs are found before wider adoption:

- [`samples/Sasd.Ui.Sample.Crud`](samples/Sasd.Ui.Sample.Crud/) — focused CRUD reference application;
- [`samples/Sasd.Ui.Sample.Workbench`](samples/Sasd.Ui.Sample.Workbench/) — focused document/workbench reference application;
- [`examples/Sasd.Ui.PlatformShowcase`](examples/Sasd.Ui.PlatformShowcase/) — broad integrated demonstration with manual component exercises and a safe public-API self-test page.

Run the integrated showcase on Windows with:

```powershell
dotnet run --project examples/Sasd.Ui.PlatformShowcase/Sasd.Ui.PlatformShowcase.csproj
```

The canonical verification script restores and strictly builds all three consumers, so future platform changes cannot silently break the example.

## Build prerequisites

- Windows 10 or Windows 11;
- .NET 8 SDK;
- PowerShell 7 (`pwsh`) for the canonical verification wrapper;
- Visual Studio 2022 or later with **.NET desktop development** for designer work.

### Build and validation

Use the canonical repository verification command:

```powershell
pwsh ./build/verify.ps1
```

This performs restore, strict Release build, architecture checks and all current smoke suites. GitHub Actions invokes the same script, so local/Codex verification and the merge gate use one operational definition.

On a non-Windows environment use `pwsh ./build/verify.ps1 -CompileOnly`; this does **not** replace the Windows runtime checks.

## Coding agents / Codex

The repository contains a curated [`AGENTS.md`](AGENTS.md) for Codex and other instruction-aware coding agents. It is intentionally a concise map rather than a second specification. The detailed source of truth remains under `docs/` and in `ROADMAP.md`.

Read the [Codex development workflow](docs/en/project/codex-development-workflow.md) before assigning broad autonomous work. In particular, Codex may implement already-approved roadmap/specification work, but must stop before introducing new runtime dependencies, platform changes, major package-boundary changes or other strategic choices.

## Roadmap

The roadmap is intentionally kept at the repository root so it is visible next to the README:

**[`ROADMAP.md`](ROADMAP.md)**

In summary:

- **R0.1:** repository, contracts, architecture checks and reproducible build;
- **R0.2:** native WinForms versus isolated Krypton visual pilot;
- **R0.3:** grid, dialog, state and UI-test pilot;
- **R1:** productive foundation, quality evidence, packaging and first real SASD migrations;
- **R2:** dependency-free native helpers plus independently approved specialist adapters;
- **R3:** only additional modules with demonstrated product demand.

## Contributing and security

Please read [CONTRIBUTING.md](CONTRIBUTING.md) before proposing a new component or dependency. Security issues should follow [SECURITY.md](SECURITY.md) and should not initially be disclosed through a public issue.

## Licence

SASD UI Platform is licensed under the [MIT License](LICENSE). Third-party adapters and sample integrations retain their own licences and notices. Inclusion in the component catalogue does not mean that a product's source code or commercial functionality is part of this repository.

---

Copyright © 2026 SASD-GmbH
