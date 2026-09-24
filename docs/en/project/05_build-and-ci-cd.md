# Build and CI/CD Concept

## 1. Goals

- reproducible builds from a clean checkout;
- centralized package and compiler configuration;
- early detection of architecture, licensing, and security problems;
- traceable NuGet and release artifacts;
- Windows-specific UI tests on controlled runners.

## 2. Repository Foundation Files

- `global.json`: SDK family and roll-forward policy;
- `Directory.Build.props`: nullable, XML docs, analyzers, deterministic builds and common package metadata;
- `Directory.Packages.props`: centralized NuGet versions;
- lock files or a centrally defined restore strategy;
- `NuGet.config`: approved sources and source mapping;
- build scripts under `build/` or `eng/`.

## 3. Pipeline

### Validate

- formatting and style check;
- restore with lock verification;
- architecture tests;
- license and vulnerability scan;
- verification of prohibited package sources;
- Markdown link and structure check.

### Build

- Debug and Release;
- `net8.0-windows`;
- warnings as errors for first-party projects, with explicitly documented exceptions;
- deterministic assembly and package metadata.

### Test

- unit and architecture tests on every PR;
- integration and component tests on every PR;
- selected UIA tests on a Windows runner;
- the complete visual/DPI matrix before releases and, where practical, nightly.

### Pack

- NuGet packages;
- symbol packages;
- XML documentation;
- SBOM;
- `THIRD-PARTY-NOTICES`;
- release notes and checksums.

The current non-publishing packaging check is:

```powershell
pwsh ./build/pack-dry-run.ps1
```

It packs the explicitly listed current `src/` product modules with a neutral local prerelease version (`0.0.0-local` by default), creates `.nupkg` and `.snupkg` files below the ignored `artifacts/` tree, and inspects each archive to verify that the product assembly, XML documentation and portable PDB are present. The command never uploads a package.

A different disposable version or output directory can be supplied explicitly:

```powershell
pwsh ./build/pack-dry-run.ps1 -Version 0.0.0-local.2 -OutputDirectory artifacts/nuget-check
```

The normal local verification command stays unchanged so ordinary inner-loop work does not pay the packaging cost. The Windows CI gate requests the additional evidence explicitly:

```powershell
pwsh ./build/verify.ps1 -IncludePackDryRun
```

A successful dry-run means that the current project/package graph can be packaged consistently. It does **not** mean that the API is stable, that a release version has been selected, that signing is configured, or that publication is approved.

### Publish

- only from a tag or manually approved release workflow;
- initially to an internal/private feed;
- never publish from a developer working directory;
- immutable versions: an already published package version is never overwritten.

The dry-run has no publish step and requires no package-feed credential. Publishing, signing and external feed configuration remain separate release decisions.

## 4. Runner Separation

- Logic and build jobs may run on standardized Windows runners.
- Designer, DPI, screenshot, and UIA tests require a controlled Windows VM/runner configuration.
- Baseline screenshots are updated only in a defined environment.

## 5. Artifact Retention

- PR artifacts retained temporarily for diagnosis;
- local dry-run package output is disposable and ignored by Git;
- release artifacts retained permanently;
- test reports and screenshots for failed UI/visual tests;
- SBOM, notices, and checksums for every release.

## 6. Local Build

The normal comprehensive local gate is:

```powershell
pwsh ./build/verify.ps1
```

Packaging is intentionally opt-in locally. Run `build/pack-dry-run.ps1` separately when investigating package output, or use `build/verify.ps1 -IncludePackDryRun` when the full build/test/package evidence is desired. Visual/DPI/Designer acceptance remains a separate controlled-Windows activity.

## 7. Pipeline Exceptions

A temporary exception requires:

- documented cause;
- owner;
- expiration date;
- issue link;
- no bypass of licensing or critical security gates.
