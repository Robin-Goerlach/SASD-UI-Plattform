# SASD UI Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4)
![Framework](https://img.shields.io/badge/.NET-8.0-512BD4)
![UI](https://img.shields.io/badge/UI-WinForms-5C2D91)
![Status](https://img.shields.io/badge/status-R0%20repository%20scaffold-orange)

**A modular, designer-friendly C# WinForms UI platform for consistent SASD desktop applications.**

The project provides reusable application shells, forms, commands, data views, dialogs, themes, UI-state persistence, Windows integration, optional specialist adapters, reference applications and enforceable quality standards. It is intentionally focused on **Windows, .NET 8 and WinForms** before any later WPF or web product line.

![Concept design for the SASD UI Platform Component Gallery](artefacts/sasd-ui-platform-component-gallery.png)

> **Design target:** The image above is a concept screenshot for the planned Component Gallery. It communicates the intended structure and visual quality; it is not presented as a screenshot of a completed implementation.

## Project status

The repository is prepared for **R0 — architecture and technology validation**. It contains the complete bilingual documentation baseline, architecture diagrams, source and test scaffolding, GitHub templates and the initial .NET project structure. Production component implementation has not yet been claimed.

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
SASD-UI-Platform/
├─ src/                    Product projects and optional adapters
├─ tests/                  Unit, integration, architecture, UI and visual tests
├─ samples/                Component Gallery and reference applications
├─ templates/              Planned project and application templates
├─ docs/
│  ├─ en/                  English normative and project documentation
│  └─ de/                  German source documentation
├─ artefacts/              Concept screenshot and architecture diagrams
├─ eng/                    Engineering policies and helper assets
├─ build/                  Build and packaging entry points
└─ .github/                Issue and pull-request templates
```

## Documentation

### English

| Document | Purpose |
| --- | --- |
| [Component catalogue](docs/en/core/01-component-catalog.md) | Commercial/open-source market inventory and 123 component classes |
| [Requirements specification](docs/en/core/02-requirements-specification.md) | Product requirements, priorities, exclusions and acceptance criteria |
| [Functional specification](docs/en/core/03-functional-specification.md) | Technical implementation commitments and component catalogue |
| [Architecture](docs/en/core/04-architecture.md) | Module, runtime, package, quality and security architecture |
| [Project documentation index](docs/en/README.md) | Roadmap, development, testing, CI/CD, release, security, governance and ADRs |

### Deutsch

Die vollständigen deutschen Ausgangsdokumente befinden sich unter [`docs/de`](docs/de/README.md). English is the repository and code language; German documentation is retained as the source-language companion edition.

## Source scaffold

The `src` folder establishes the package boundaries defined by the architecture. The first minimal projects are intentionally small:

- `Sasd.Ui.Core` — platform-neutral contracts, tokens and result models;
- `Sasd.Ui.WinForms` — WinForms base classes and UI-thread infrastructure;
- `Sasd.Ui.WinForms.Krypton` — isolated visual adapter placeholder;
- `samples/Sasd.Ui.ComponentGallery` — executable R0 host for future components.

Additional folders already reserve the approved R1 modules without pretending that they are implemented.

## Build prerequisites

- Windows 10 or Windows 11;
- .NET 8 SDK;
- Visual Studio 2022 or later with **.NET desktop development** for designer work.

Planned commands once the R0 projects are populated:

```powershell
dotnet restore SASD.Ui.Platform.sln
dotnet build SASD.Ui.Platform.sln --configuration Release
dotnet test SASD.Ui.Platform.sln --configuration Release
```

The repository package was prepared in an environment without the .NET SDK, so the scaffold has not been represented as a successfully compiled product build. Build validation is an explicit R0.1 task.

## Roadmap

See the [project roadmap](ROADMAP.md). In summary:

- **R0.1:** repository, contracts, architecture tests and reproducible build;
- **R0.2:** native WinForms versus Krypton visual pilot;
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
