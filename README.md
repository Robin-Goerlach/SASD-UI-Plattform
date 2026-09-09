# SASD UI Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4)
![Framework](https://img.shields.io/badge/.NET-8.0-512BD4)
![UI](https://img.shields.io/badge/UI-WinForms-5C2D91)
![Status](https://img.shields.io/badge/status-R0%20foundation%20%2F%20R1%20in%20progress-orange)

**A modular, designer-friendly C# WinForms UI platform for consistent SASD desktop applications.**

The project provides reusable application shells, forms, commands, data views, dialogs, themes, UI-state persistence, Windows integration, optional specialist adapters, reference applications and enforceable quality standards. It is intentionally focused on **Windows, .NET 8 and WinForms** before any later WPF or web product line.

**Project links:** [Roadmap](ROADMAP.md) · [Architecture](docs/en/core/04-architecture.md) · [Requirements](docs/en/core/02-requirements-specification.md) · [Functional specification](docs/en/core/03-functional-specification.md) · [English documentation](docs/en/README.md) · [German documentation](docs/de/README.md)

![Concept design for the SASD UI Platform Component Gallery](artefacts/sasd-ui-platform-component-gallery.png)

> **Design target:** The image above is a concept screenshot for the Component Gallery. It communicates the intended structure and visual quality; it is not presented as a screenshot of a completed implementation.

## Project status

The repository has moved beyond the initial scaffold. The current `main` line contains a reusable native WinForms foundation, an isolated Krypton R0.2 adapter pilot, strict Windows CI, architecture checks and behavioral smoke checks.

Implemented foundations already include themes, forms and validation, commands, navigation/status, DataGrid support, paging/search/filter contracts, dialogs and progress, versioned UI state with backup/migration, recent items, file/clipboard/shell services and the Component Gallery host.

The project is **not yet claiming R1.0 stability**. Important R0/R1 acceptance work remains, especially the Visual Studio Designer/DPI/focus/high-contrast matrix, UI Automation/screenshot regression, broader Gallery coverage, packaging/API-baseline tooling and adoption by a real SASD application. See the [Roadmap](ROADMAP.md) for the current gate status.

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
- Component Gallery and CRUD, workbench and utility reference applications.

### Optional R2/R3 modules

Charts, Markdown/code/diff editors, image viewing, barcode/QR, PDF generation, WebView2, docking, PropertyGrid extensions, wizard and optional ribbon are delivered only through separate modules after individual evaluation.

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
├─ src/                    Product projects and optional adapters
├─ tests/                  Unit, integration, architecture, UI and visual tests
├─ samples/                Component Gallery and reference applications
├─ templates/              Planned project and application templates
├─ docs/
│  ├─ en/                  English normative and project documentation
│  └─ de/                  German companion/source documentation
├─ artefacts/              Concept screenshot and architecture diagrams
├─ eng/                    Engineering policies/configuration (not English docs)
├─ build/                  Build and packaging entry points
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
| [Project documentation index](docs/en/README.md) | Development, testing, CI/CD, release, security, governance and ADRs |

### Deutsch

Die vollständigen deutschen Ausgangs- und Begleitdokumente befinden sich unter [`docs/de`](docs/de/README.md). English is the repository and code language; German documentation is retained as the source-language companion edition.

## Implemented source modules

The `src` folder follows the package boundaries defined by the architecture. Current modules include:

- `Sasd.Ui.Core` — platform-neutral contracts, theme semantics and result models;
- `Sasd.Ui.WinForms` — native WinForms base classes and UI-thread infrastructure;
- `Sasd.Ui.WinForms.Theming` — native theme application;
- `Sasd.Ui.WinForms.Forms` — field layout, sections and validation;
- `Sasd.Ui.WinForms.Data` — grids, paging, search/filter contracts and CSV/state helpers;
- `Sasd.Ui.WinForms.Commands` — reusable command state/execution/binding;
- `Sasd.Ui.WinForms.Shell` — navigation and status foundations;
- `Sasd.Ui.WinForms.Dialogs` — dialogs, error presentation, busy/progress;
- `Sasd.Ui.WinForms.State` — versioned JSON UI state and recent items;
- `Sasd.Ui.WinForms.Windows` — constrained Windows integration services;
- `Sasd.Ui.WinForms.Krypton` — isolated Krypton technology pilot;
- `samples/Sasd.Ui.ComponentGallery` — executable specification and acceptance host.

## Build prerequisites

- Windows 10 or Windows 11;
- .NET 8 SDK;
- Visual Studio 2022 or later with **.NET desktop development** for designer work.

### Build and validation

```powershell
dotnet restore SASD.Ui.Platform.sln
dotnet build SASD.Ui.Platform.sln --configuration Release -p:TreatWarningsAsErrors=true
```

GitHub Actions also runs architecture checks plus Core, UI-state, native WinForms and Krypton smoke checks. Pull requests are not merged until those gates are green.

## Roadmap

The roadmap is intentionally kept at the repository root so it is visible next to the README:

**[`ROADMAP.md`](ROADMAP.md)**

In summary:

- **R0.1:** repository, contracts, architecture checks and reproducible build;
- **R0.2:** native WinForms versus isolated Krypton visual pilot;
- **R0.3:** grid, dialog, state and UI-test pilot;
- **R1:** production foundation and first real SASD migrations;
- **R2:** independently approved specialist adapters;
- **R3:** only additional modules with demonstrated product demand.

## Contributing and security

Please read [CONTRIBUTING.md](CONTRIBUTING.md) before proposing a new component or dependency. Security issues should follow [SECURITY.md](SECURITY.md) and should not initially be disclosed through a public issue.

## Licence

SASD UI Platform is licensed under the [MIT License](LICENSE). Third-party adapters and sample integrations retain their own licences and notices. Inclusion in the component catalogue does not mean that a product's source code or commercial functionality is part of this repository.

---

Copyright © 2026 SASD-GmbH
