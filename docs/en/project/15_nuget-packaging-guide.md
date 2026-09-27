# NuGet Packaging Guide

## 1. Goals

- few direct package references for consumers;
- clear separation of optional heavyweight adapters;
- no unintended transitive UI suites;
- aligned versioning of the R1 core;
- strong IntelliSense through XML documentation and symbols.

## 2. Package Groups

The repository dry-runs packaging for granular internal product projects so package metadata, symbols, dependency relationships and XML documentation are verified early. That technical inventory is intentionally larger than the supported direct consumer surface.

### Supported R1 consumer entry packages

| Package | Consumer purpose |
| --- | --- |
| `Sasd.Ui.Core` | Platform-neutral contracts and semantics when an application needs them directly. |
| `Sasd.Ui.WinForms` | Minimal native WinForms foundation. |
| `Sasd.Ui.WinForms.App` | Recommended convenience metapackage for a normal R1 application. |
| `Sasd.Ui.WinForms.Data` | Explicit opt-in for grids, search/filter/paging, lists and trees. |
| `Sasd.Ui.WinForms.Krypton` | Explicit optional visual implementation. |

The supported entry set is a documentation/support boundary, not a claim that only five package IDs exist on the feed. The platform keeps separate assemblies, so implementation packages such as Commands, Forms, Dialogs, Shell, State, Theming and Windows remain real transitive NuGet dependencies.

### R1 application metapackage

`Sasd.Ui.WinForms.App` is dependency-only and contains no runtime assembly. Its direct dependencies are exactly:

- `Sasd.Ui.WinForms.Commands`;
- `Sasd.Ui.WinForms.Dialogs`;
- `Sasd.Ui.WinForms.Forms`;
- `Sasd.Ui.WinForms.Shell`;
- `Sasd.Ui.WinForms.State`;
- `Sasd.Ui.WinForms.Theming`;
- `Sasd.Ui.WinForms.Windows`.

`Sasd.Ui.WinForms.Data`, `Sasd.Ui.WinForms.Krypton`, `Sasd.Ui.WinForms.Media` and all future R2/R3 adapters are intentionally excluded. This boundary is enforced by the package dry-run, not only by documentation.

### Specialist adapters

ScottPlot, ScintillaNET, WebView2, docking, PDF/media and similar specialist capabilities remain separate opt-in packages. They must not enter every application through the R1 App metapackage.

## 3. Consumer Package Strategy

A typical R1 application starts with:

```xml
<ItemGroup>
  <PackageReference Include="Sasd.Ui.WinForms.App" Version="<approved-version>" />
</ItemGroup>
```

Add `Sasd.Ui.WinForms.Data` only when data-heavy controls are needed. Add `Sasd.Ui.WinForms.Krypton` only after the application deliberately chooses that visual implementation.

Applications with unusually small requirements may reference `Sasd.Ui.Core` or `Sasd.Ui.WinForms` directly instead of the App package. Direct references to implementation packages remain possible for advanced cases, but they are not the normal R1 starting point and should have a concrete reason.

ADR-0017 records the package-topology decision and its review criteria.

## 4. Metadata

Every package contains common identity metadata:

- ID, title, and description;
- version and repository URL;
- license expression or license file;
- tags;
- README;
- release notes or changelog link;
- deterministic repository and commit metadata.

Concrete assembly packages additionally contain XML documentation and a symbol package. Dependency-only metapackages deliberately contain neither a runtime assembly nor a symbol package.

## 5. Dependency Rules

- exact or compatible minimum versions according to the central strategy;
- no broad uncontrolled version ranges;
- no private assets that consumers unexpectedly require;
- native runtime dependencies documented explicitly;
- build/analyzer packages correctly marked as `PrivateAssets`.

## 6. Package Verification

Before publication:

- inspect package contents and dependency manifests;
- create a fresh consumer project;
- install from the intended feed;
- restore/build representative public types;
- verify that the App package resolves its exact allowlisted modules and does not pull Data/Krypton/Media;
- test designer and minimal example behavior;
- review transitive dependencies and license notices;
- verify uninstall and update paths.

The current dry-run already performs a local-feed restore/build smoke for `Sasd.Ui.WinForms.App`; the later internal-feed release candidate repeats the same consumer path against the actual feed.

## 7. Package Count as an Architecture Indicator

If a basic shell requires more than roughly eight direct SASD packages, review the package structure. Internal project separation may be more granular than the public consumer surface.
