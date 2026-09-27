# ADR-0017 – Curated R1 Consumer Packages with an Application Metapackage

- **Status:** Accepted
- **Date:** 2026-09-27
- **Product line:** SASD UI Platform – C# WinForms Components
- **Refines:** ADR-0002 – Modular Monorepo with Few Published Packages

## Context

The repository intentionally contains more internal assemblies than a typical application should have to reference directly. R1 needs to preserve those code and ownership boundaries while giving normal SASD applications a small, understandable NuGet entry surface.

Publishing one monolithic assembly would erase useful internal boundaries. Requiring every application to reference Commands, Forms, Dialogs, Shell, State, Theming and Windows individually would expose repository structure as application configuration and make future internal refactoring harder.

NuGet metapackages provide a middle ground: the application can hold one direct reference while the runtime continues to use the real modular assemblies.

## Decision

R1 defines five **supported direct consumer entry packages**:

1. `Sasd.Ui.Core` — platform-neutral contracts and semantics for consumers that need them directly.
2. `Sasd.Ui.WinForms` — minimal native WinForms foundation.
3. `Sasd.Ui.WinForms.App` — dependency-only convenience metapackage for a normal SASD WinForms application.
4. `Sasd.Ui.WinForms.Data` — explicit opt-in for grid/search/filter/paging/list/tree functionality.
5. `Sasd.Ui.WinForms.Krypton` — explicit optional visual adapter.

`Sasd.Ui.WinForms.App` has no runtime assembly. Its direct dependencies are exactly:

- `Sasd.Ui.WinForms.Commands`
- `Sasd.Ui.WinForms.Dialogs`
- `Sasd.Ui.WinForms.Forms`
- `Sasd.Ui.WinForms.Shell`
- `Sasd.Ui.WinForms.State`
- `Sasd.Ui.WinForms.Theming`
- `Sasd.Ui.WinForms.Windows`

Those modular packages remain real NuGet implementation dependencies because their assemblies stay separate. They may therefore be present on an internal feed and visible transitively, but they are **not the normal supported direct starting point** for application project files.

`Sasd.Ui.WinForms.Data`, `Sasd.Ui.WinForms.Krypton`, `Sasd.Ui.WinForms.Media` and future R2/R3 adapters are prohibited from the App metapackage dependency set unless a later ADR explicitly changes that boundary.

Templates are an R1 delivery asset but are not a runtime dependency of the App metapackage.

## Why the metapackage does not bundle assemblies

The App package deliberately does not copy multiple internal DLLs into one physical package. Doing so would couple package construction to output-file harvesting, risk duplicate assemblies when consumers also install Data/Krypton packages, and blur which assembly owns a public API.

Keeping the modular packages as NuGet dependencies preserves normal assembly/package relationships while reducing the number of **direct** references an application normally manages.

## Verification

The release dry-run must:

- build the App project as a dependency-only NuGet package;
- verify that it contains no `lib/` or `ref/` runtime assets;
- verify its exact direct dependency allowlist;
- produce no symbol package for the dependency-only metapackage;
- restore a fresh WinForms consumer using only the local dry-run package source;
- compile representative public types from every module behind the App package;
- prove that Data, Krypton and Media are not pulled transitively by App.

## Positive Consequences

- a normal application can start with one R1 application package;
- internal assemblies remain independently testable and maintainable;
- data-heavy and visual/vendor dependencies stay explicit;
- later R2 adapters cannot silently enter every application;
- package installation is testable from the same artifacts intended for release.

## Negative Consequences and Risks

- the package feed still contains transitive implementation packages even though the documented direct entry surface is small;
- package versions across the R1 product line must remain aligned;
- maintainers must distinguish supported direct entry packages from implementation packages in documentation and support.

These costs are preferred to either a monolithic assembly or forcing repository-level module knowledge into every consumer project.

## Alternatives Considered

### Publish every module as an equal first-class entry package

Rejected for R1 because it burdens normal applications with repository-level package composition and weakens ADR-0002's consumer-surface goal.

### One large R1 assembly/package

Rejected because it removes useful internal assembly boundaries and increases coupling.

### Physically bundle all R1 assemblies inside one App package

Deferred. It reduces feed package count but complicates ownership, duplicate-asset handling and package composition without a demonstrated R1 need.

## Review Criterion

Reassess when:

- real SASD consumers repeatedly need a different default package combination;
- package-feed administration makes transitive implementation packages materially costly;
- assemblies gain independent release cycles;
- a future major version is prepared.

A review must preserve the rule that specialist R2/R3 dependencies are opt-in unless there is explicit evidence to change it.
