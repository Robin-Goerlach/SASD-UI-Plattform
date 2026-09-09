# Build and CI/CD Concept

## 1. Goals

- reproducible builds from a clean checkout;
- centralized package and compiler configuration;
- early detection of architecture, licensing, and security problems;
- traceable NuGet and release artifacts;
- Windows-specific UI tests on controlled runners.

## 2. Repository Foundation Files

- `global.json`: SDK family and roll-forward policy;
- `Directory.Build.props`: nullable, XML docs, analyzers, deterministic builds;
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

### Publish

- only from a tag or manually approved release workflow;
- initially to an internal/private feed;
- never publish from a developer working directory;
- immutable versions: an already published package version is never overwritten.

## 4. Runner Separation

- Logic and build jobs may run on standardized Windows runners.
- Designer, DPI, screenshot, and UIA tests require a controlled Windows VM/runner configuration.
- Baseline screenshots are updated only in a defined environment.

## 5. Artifact Retention

- PR artifacts retained temporarily for diagnosis;
- release artifacts retained permanently;
- test reports and screenshots for failed UI/visual tests;
- SBOM, notices, and checksums for every release.

## 6. Local Build

One standard local command should restore, build, and run fast tests. A second, slower command runs integration, UI, and visual tests. Concrete script names are defined in R0.1 and used identically in the README and CI.

## 7. Pipeline Exceptions

A temporary exception requires:

- documented cause;
- owner;
- expiration date;
- issue link;
- no bypass of licensing or critical security gates.
