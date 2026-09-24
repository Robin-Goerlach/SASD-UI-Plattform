# SASD UI Platform – Supplementary Project Documentation

**Product line:** C# WinForms Components  
**Documentation version:** 0.1  
**Date:** 2026-07-23  
**Status:** Working baseline for R0/R1

## Purpose of This Package

This package complements the existing foundation documents:

1. **Requirements specification** – describes needs, goals, priorities, and scope boundaries.
2. **Functional specification** – describes the planned technical implementation.
3. **Architecture document** – describes structure, dependencies, runtime flows, and architecture decisions.

The Markdown files in this package answer the practical questions that remain for implementation: In which order will work proceed? How are contributions reviewed? How are testing, releases, dependencies, UI state, migrations, and support organized? Which decisions require an ADR? Which topics remain deliberately outside the current scope?

## Guiding Decision

The SASD UI Platform is **not a complete reimplementation of DevExpress** and not a collection repository for arbitrary controls. It is a curated, modular, designer-friendly WinForms component platform for real SASD applications.

Custom components are developed only when at least one of the following benefits is created:

- recurring behavior is encapsulated reliably;
- a stable SASD API protects applications from changes in third-party libraries;
- usability, accessibility, DPI behavior, or error handling is standardized;
- multiple SASD applications can demonstrably use the same solution;
- testability, documentation, and maintainability improve compared with local one-off logic.

## Document Overview

| Document | Purpose |
| --- | --- |
| [roadmap.md](roadmap.md) | Product phases, milestones, release gates, and prioritization |
| [changelog.md](changelog.md) | Traceable changes to documentation and later to the product |
| [contributing.md](contributing.md) | Development and contribution process |
| [security.md](security.md) | Security model, vulnerability reporting, and secure defaults |
| [00_document-landscape.md](00_document-landscape.md) | Roles of all documents and maintenance ownership |
| [01_project-plan-and-milestones.md](01_project-plan-and-milestones.md) | Operational planning from R0 through R3 |
| [02_component-scope-and-backlog.md](02_component-scope-and-backlog.md) | Binding component scope and prioritized backlog |
| [03_development-guidelines.md](03_development-guidelines.md) | C#, WinForms, designer, and API rules |
| [04_test-strategy.md](04_test-strategy.md) | Test pyramid, matrices, and release criteria |
| [05_build-and-ci-cd.md](05_build-and-ci-cd.md) | Reproducible build and pipeline gates |
| [06_release-and-versioning.md](06_release-and-versioning.md) | SemVer, package releases, and compatibility |
| [07_dependencies-licenses-and-sbom.md](07_dependencies-licenses-and-sbom.md) | Vendor, license, and fork governance |
| [08_data-storage-and-ui-state.md](08_data-storage-and-ui-state.md) | Persistence boundaries, JSON state, and migration |
| [09_migration-and-adoption-guide.md](09_migration-and-adoption-guide.md) | Incremental adoption in existing SASD applications |
| [10_operations-support-and-maintenance.md](10_operations-support-and-maintenance.md) | Maintenance model, support classes, and lifecycle |
| [11_governance-and-decision-process.md](11_governance-and-decision-process.md) | Decision paths, reviews, and responsibilities |
| [12_future-register.md](12_future-register.md) | WPF, WinUI, Avalonia, web/ASPX, Java, and later product lines |
| [13_risk-and-quality-register.md](13_risk-and-quality-register.md) | Living risk and quality register |
| [14_component-gallery-concept.md](14_component-gallery-concept.md) | Executable specification and visual reference |
| [15_nuget-packaging-guide.md](15_nuget-packaging-guide.md) | Package boundaries, metadata, and consumer experience |
| [16_accessibility-and-dpi-checklist.md](16_accessibility-and-dpi-checklist.md) | Practical UI component review checklist |
| [17_documentation-standard.md](17_documentation-standard.md) | Requirements for Markdown, XML docs, examples, and change control |
| [adr/README.md](adr/README.md) | ADR process and register |

## Recommended Use

1. The requirements specification, functional specification, and architecture document remain the normative baseline.
2. `roadmap.md` and the project plan control implementation order.
3. Component scope, development guidelines, and test strategy govern pull requests.
4. Every significant technology or dependency decision receives an ADR.
5. Releases are approved only when the defined gates are demonstrably satisfied.

## Current Scope

- **In scope:** Windows 10/11, C#, .NET 8, WinForms, Visual Studio Designer, internal NuGet packages, the Krypton pilot, and native WinForms fallbacks.
- **Later:** WPF, WinUI 3, Avalonia, ASP.NET Core/Blazor, ASP.NET Web Forms/ASPX, and Java/OpenXava.
- **Not currently planned:** Android, iOS, macOS, and Linux desktop implementations.

## Documentation Status

These files are planning and governance documents. They describe the intended target state. Technical statements become proven only when their associated R0/R1 gates are supported by code, tests, Component Gallery pages, and release artifacts.
