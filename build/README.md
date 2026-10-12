# Build

This directory contains reproducible build, test, pack and release entry points. Build scripts are thin wrappers around documented .NET commands and must not depend on developer-machine state.

## Repository verification

Use the same verification entry point locally and from coding agents:

```powershell
pwsh ./build/verify.ps1
```

On Windows this performs the shared repository gate, including:

1. restore of the product solution and the CRUD, Workbench, Utility and Integrated Showcase consumers;
2. strict Release builds with analyzer warnings treated as errors;
3. architecture checks;
4. Core and UI-state smoke checks;
5. WinForms foundation, forms and composite-feedback smoke checks;
6. Windows integration, shell, keyboard and dialog-threading smoke checks;
7. lifecycle/endurance checks;
8. native R2, data/dashboard and data-interaction smoke checks;
9. an off-screen Integrated Showcase navigation smoke that creates every registered page under a normal WinForms layout lifecycle;
10. Krypton adapter regression checks.

For an environment where restore has already completed:

```powershell
pwsh ./build/verify.ps1 -SkipRestore
```

On non-Windows systems, use:

```powershell
pwsh ./build/verify.ps1 -CompileOnly
```

`-CompileOnly` intentionally does not pretend to validate WinForms runtime behavior. It runs the cross-platform restore/build/architecture/core subset and leaves the Windows runtime suites to a Windows developer machine or GitHub Actions.

The GitHub Actions workflow invokes this same script, so the repository has one operational definition of its automated verification gate.

## NuGet package dry-run

Package evidence is opt-in for ordinary local verification and is always available through the same gate:

```powershell
pwsh ./build/verify.ps1 -IncludePackDryRun
```

The underlying `build/pack-dry-run.ps1` keeps the list of product packages explicit. It never publishes packages. For each configured product project it creates a normal `.nupkg` plus `.snupkg` in `artifacts/nuget-dry-run/`, enables the .NET SDK's built-in Package Validation, and inspects the archives directly.

The dry-run distinguishes concrete assembly packages from dependency-only metapackages. Every package contains its project-local `README.md`. Concrete assembly packages additionally contain their product assembly, generated XML API documentation and a portable PDB in the symbol package. `Sasd.Ui.WinForms.App` deliberately contains no runtime/reference assembly and produces no symbol package; instead the dry-run verifies its exact dependency allowlist.

After all packages are complete, `build/test-r1-metapackage.ps1` creates a fresh WinForms consumer, clears external NuGet sources, restores `Sasd.Ui.WinForms.App` only from the local dry-run feed, verifies the resolved SASD package graph and compiles representative types from every App module. Data, Krypton and Media are asserted absent. `build/write-checksums.ps1` then writes a deterministic UTF-8/LF `SHA256SUMS.txt` for every `.nupkg` and `.snupkg` and immediately recomputes each SHA-256 value from disk. The checksum file contains only distributable package artifacts; unrelated temporary files are deliberately excluded.

Using the project-local module README keeps package guidance close to the public surface it describes instead of maintaining a second copied package document. A product project without that README fails the dry-run rather than silently producing a package with no landing-page documentation.

The dry-run uses version `0.0.0-local` by default and performs no push or feed operation. SDK Package Validation currently checks the internal consistency/applicability of the package being built; it is not yet a cross-version API baseline because no approved R1 baseline package has been selected. ADR-0017 now defines the supported R1 consumer package topology. Checksum generation proves artifact integrity for the dry-run; it does not replace the still-pending SBOM, signing or API-baseline work.

When `-IncludePackDryRun` is used, `build/write-dependency-evidence.ps1` also records the resolved direct and transitive NuGet dependency graph for the same explicit product-project inventory. It verifies that direct dependencies are centrally versioned and that every resolved NuGet package is at least named in `THIRD-PARTY-NOTICES.md`. The generated `artifacts/release-evidence/nuget-dependencies.json` is deterministic release evidence; it is deliberately **not** labelled an SBOM because no SPDX/CycloneDX standard is being claimed.

`build/check-nuget-vulnerabilities.ps1` performs the security companion check. It queries NuGet's current vulnerability data with `--vulnerable --include-transitive --format json`, records the timestamped result as `artifacts/release-evidence/nuget-vulnerabilities.json`, and fails the gate on any reported direct or transitive vulnerability. Audit-source failures such as `NU1900` are also treated as failures so unavailable security data cannot be mistaken for a clean report. `Directory.Build.props` enables `NuGetAuditMode=all` at restore time as an additional transitive audit signal.

The vulnerability report is intentionally time-sensitive: it documents what the configured NuGet audit source knew at that run. It is not an artifact-integrity checksum and it is not a standards-compliant SBOM.
