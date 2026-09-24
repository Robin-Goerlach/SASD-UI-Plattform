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

The underlying `build/pack-dry-run.ps1` keeps the list of product packages explicit. It never publishes packages. For each configured product project it creates a normal `.nupkg` plus `.snupkg` in `artifacts/nuget-dry-run/` and inspects the archives directly.

The dry-run verifies that every product package contains:

- its product assembly;
- generated XML API documentation;
- the project-local `README.md` as the NuGet package readme;
- a portable PDB in the symbol package.

Using the project-local module README keeps package guidance close to the public surface it describes instead of maintaining a second copied package document. A product project without that README fails the dry-run rather than silently producing a package with no landing-page documentation.

The dry-run uses version `0.0.0-local` by default and performs no push or feed operation.
