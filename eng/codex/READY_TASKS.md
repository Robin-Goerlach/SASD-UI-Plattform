# Codex Ready Task Queue

**Updated:** 2026-09-24

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

## Current ready work

R1 is now in closing mode. The authoritative execution order is [`docs/en/project/r1-closing-plan.md`](../../docs/en/project/r1-closing-plan.md). Do not add new R1 controls merely because a component could be useful.

### Ready without a new strategic decision

Use the evidence matrix to select small, serial tasks from these areas:

- dependency-free release hygiene and reproducible evidence;
- documentation and evidence-matrix maintenance;
- lifecycle/accessibility/keyboard defect fixes in existing R1 contracts;
- release checksums and third-party evidence using already-approved SDK/repository capabilities;
- preparation work for the manual Designer/DPI/High-Contrast/accessibility matrix;
- bounded reference-consumer fixes that do not redefine package boundaries.

### Stop before these closing decisions

Ask for explicit approval before choosing or implementing:

- the final public NuGet package topology;
- the public-API compatibility tool if it adds a new dependency/toolchain;
- native WinForms versus Krypton as the visible default;
- third-party UI automation or screenshot-regression tooling;
- a new specialist R2 runtime dependency.

### Recently completed closing evidence

- PR #43: GridController resilience, cancellation and failure contracts;
- PRs #44–#50: SearchBox, FilterBar, Breadcrumb, DocumentTabs, navigation and recent-items hardening;
- PR #53: TreeView lazy loading/retry/disposal lifecycle;
- PR #54: Breadcrumb ellipsis navigation;
- PR #55: Integrated Showcase runtime navigation smoke and DataPage layout fix;
- PR #56: BusyOverlay progress/cancellation/focus contract;
- PR #57: NuGet README evidence in the package dry-run;
- PR #58: ValidationSummary field navigation;
- PR #59: executable samples solution;
- PR #60: native ListView virtual-mode pilot.

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
