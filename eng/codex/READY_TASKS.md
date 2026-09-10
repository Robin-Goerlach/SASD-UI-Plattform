# Codex Ready Task Queue

**Updated:** 2026-09-10

This file is the small operational handoff for starting Codex work on the SASD UI Platform. It complements the durable rules in the root `AGENTS.md` and the detailed workflow in `docs/en/project/codex-development-workflow.md`.

The queue intentionally contains only bounded work that fits existing project decisions. Strategic choices remain human decisions.

## Before starting any task

From the repository root, run:

```powershell
pwsh ./eng/codex/preflight.ps1
```

Then ask Codex to read, in order:

1. `AGENTS.md`;
2. the GitHub issue selected below;
3. `ROADMAP.md`;
4. the nearest scoped `AGENTS.md` and module `README.md` files;
5. the architecture, functional specification and test guidance linked by `AGENTS.md`.

A useful first instruction is:

```text
Work on GitHub issue #<number> as a bounded SASD UI Platform task.
Read AGENTS.md and the referenced project documentation before changing code.
Preserve public compatibility and package boundaries. Do not add a runtime
third-party dependency or make another strategic decision unless the issue
explicitly authorizes it. Prefer clear, well-commented code over clever
abstractions or micro-optimization. Add or strengthen behavioral checks and
run the broad verification gate before declaring the work complete.
```

## Current ready task

### Issue #27 — NuGet packaging dry-run

Current release-engineering task using normal SDK packaging capabilities. It validates the existing package graph without choosing a public release version and must stop before publication, signing credentials, external feeds or a package-boundary redesign.

Expected outcome:

- repeatable `dotnet pack` dry-run for the explicitly listed current product modules;
- `.nupkg` and `.snupkg` output in a disposable ignored directory;
- XML documentation and portable PDB presence verified from package contents;
- centrally consistent authors/company/repository/license metadata;
- no package publication.

## Recently completed

### Issue #28 — Component coverage matrix

Completed by PR #33. `docs/en/project/11_component-evidence-matrix.md` now separates implemented R1/R2 surfaces from direct Gallery, Showcase, smoke-test and still-pending manual acceptance evidence. Missing Designer, DPI, real Windows High Contrast, keyboard-only and UIA evidence stays explicit instead of being inferred from compilation.

### Issue #26 — UI lifecycle endurance checks

Completed by PR #31. The canonical Windows gate repeats representative create/use/dispose cycles for image ownership, document-tab ownership/event lifecycle and hidden tray-service disposal without introducing timing thresholds, handle-count assumptions or a new dependency.

## After #27

Use the evidence matrix rather than component count to choose the next small hardening task. The highest-value dependency-free gaps currently include direct behavioral coverage for `SasdGridController<T>`, `SasdPager`, `SasdRecentItemsService` and `SasdFormStateService`, followed by the manual keyboard/DPI/Designer/High-Contrast evidence that requires a controlled Windows environment.

Do not run overlapping tasks concurrently when they touch the same verification files or project metadata. Small, serial pull requests are easier to review and keep the repository gate meaningful.

## Completion contract for Codex

Before handing a task back, Codex should report:

- files changed and why;
- public API changes, or an explicit statement that there were none;
- ownership/disposal/threading decisions introduced or changed;
- focused checks executed;
- result of `pwsh ./build/verify.ps1` on Windows, or a clear statement that only `-CompileOnly` was possible;
- any CI result available to the task environment;
- remaining limitations, manual checks or strategic decisions;
- the commit and pull-request reference when the environment created them.

A task is not complete merely because code was generated. A red, cancelled or skipped required gate remains unfinished work.

## Safety boundary

Never put API tokens, GitHub credentials, Codex login state, MCP credentials, workstation-specific `config.toml`, SSH keys or local secret paths in this directory. Repository instructions describe behavior; credentials and sandbox policy remain outside the repository.
