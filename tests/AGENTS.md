# Test Code Agent Guide

These instructions apply to all test and verification code under `tests/` and refine the root `AGENTS.md`.

## Purpose

Tests in this repository are executable evidence for architecture, lifecycle and component behavior. They are not a place to hide product defects or encode assumptions about unstable WinForms internals.

## General rules

- Prefer behavioral assertions over implementation-detail assertions.
- Test public behavior first. Reflection is acceptable inside smoke checks for composite-control inspection when adding a public test hook would pollute the product API.
- Do not weaken production code, analyzer settings or architecture checks simply to make a test pass.
- When a test fails, first decide whether it found a real product defect or an invalid test assumption.
- Keep error messages specific enough that a CI log explains the failed contract without a debugger.
- Tests must clean up temporary files/directories and dispose WinForms/GDI resources they create.
- Do not use network access, external services or machine-specific installed software in ordinary smoke checks.

## Smoke-test style

The smoke executables intentionally avoid a large test-framework dependency during the early platform stages.

- Keep each check small and deterministic.
- Group tests by subsystem rather than creating one project per component.
- Use `[STAThread]` for WinForms smoke executables.
- Avoid displaying real windows, tray icons, dialogs or notifications on the CI desktop unless a future dedicated UI automation test explicitly requires it.
- Do not use sleeps as synchronization. Prefer events, task completion sources or direct synchronous state checks.
- A smoke check should exit non-zero on failure and print the original exception/meaningful assertion message.

## Architecture checks

`tests/architecture/` protects structural boundaries. Changes there deserve extra caution.

- Add rules when a documented architecture boundary can be checked mechanically.
- Do not remove or narrow an architecture rule merely because new product code violates it; fix the product architecture or request a strategic decision.
- Keep the architecture checker dependency-free unless explicitly approved otherwise.

## WinForms lifecycle checks

Pay particular attention to:

- caller-owned versus component-owned resources;
- event unsubscription on `Dispose`/`Unbind`;
- safe behavior before native handle creation;
- duplicate IDs/commands/filters;
- cancellation and re-entry prevention;
- hidden/visible state that could create unwanted desktop side effects;
- invalid paths, unsupported URI schemes and untrusted file metadata;
- accessibility text when visual/color-only information is involved.

## Validation

For a normal product patch, the broad repository gate remains:

```powershell
pwsh ./build/verify.ps1
```

Use focused test projects during development, but do not claim the repository is green until the broad gate or equivalent CI run succeeds.
