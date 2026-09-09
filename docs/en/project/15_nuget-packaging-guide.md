# NuGet Packaging Guide

## 1. Goals

- few direct package references for consumers;
- clear separation of optional heavyweight adapters;
- no unintended transitive UI suites;
- aligned versioning of the R1 core;
- strong IntelliSense through XML documentation and symbols.

## 2. Package Groups

### Core

- `Sasd.Ui.Core`
- `Sasd.Ui.WinForms`
- a small number of logically related R1 packages for theme, shell, data, dialogs, Windows integration, and state.

### Adapters

- Krypton implementation;
- ScottPlot;
- ScintillaNET;
- WebView2;
- docking;
- PDF/media.

Adapters are opt-in and must not enter every application through a general metapackage.

## 3. Consumer Package Strategy

A curated R1 metapackage may be considered for typical applications if it bundles only lightweight core packages. It must not include R2 adapters.

## 4. Metadata

Every package contains:

- ID, title, and description;
- version and repository URL;
- license expression or license file;
- tags;
- README;
- symbol package;
- XML documentation;
- release notes or changelog link;
- deterministic repository and commit metadata.

## 5. Dependency Rules

- exact or compatible minimum versions according to the central strategy;
- no broad uncontrolled version ranges;
- no private assets that consumers unexpectedly require;
- native runtime dependencies documented explicitly;
- build/analyzer packages correctly marked as `PrivateAssets`.

## 6. Package Verification

Before publication:

- inspect package contents;
- create an empty consumer solution;
- install from the intended feed;
- test build, designer, and minimal example;
- review transitive dependencies and license notices;
- verify uninstall and update paths.

## 7. Package Count as an Architecture Indicator

If a basic shell requires more than roughly eight direct SASD packages, review the package structure. Internal project separation may be more granular than the public consumer surface.
