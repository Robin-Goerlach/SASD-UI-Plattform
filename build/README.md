# Build

This directory contains reproducible build, test, pack and release entry points. Build scripts are thin wrappers around documented .NET commands and must not depend on developer-machine state.

## Repository verification

Use the same verification entry point locally and from coding agents:

```powershell
pwsh ./build/verify.ps1
```

On Windows this performs:

1. solution restore;
2. strict Release build with analyzer warnings treated as errors;
3. architecture checks;
4. Core smoke checks;
5. UI-state smoke checks;
6. WinForms foundation smoke checks;
7. Windows integration smoke checks;
8. Shell integration smoke checks;
9. native R2 smoke checks;
10. data/dashboard smoke checks;
11. Krypton adapter regression checks.

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
