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

## Current ready tasks

### Issue #26 — UI lifecycle endurance checks

Good first Codex task. It is intentionally dependency-free and should exercise existing ownership, disposal and event-lifecycle contracts rather than invent new architecture.

Expected outcome:

- repeatable lifecycle/endurance smoke coverage;
- concrete product fixes only when a real defect is demonstrated;
- no timing-sensitive micro-benchmarks;
- full Windows verification.

### Issue #27 — NuGet packaging dry-run

Release-engineering task using normal SDK packaging capabilities. The task must stop before publication, signing credentials, external feeds or a package-boundary redesign.

Expected outcome:

- repeatable `dotnet pack` dry-run;
- symbols and XML documentation where supported by the existing SDK/toolchain;
- centrally consistent package metadata;
- no package publication.

### Issue #28 — Component coverage matrix

Repository-evidence task that maps implemented public R1/R2 surfaces to Gallery, Showcase, smoke-test and manual acceptance evidence.

Expected outcome:

- one maintainable evidence matrix;
- obvious dependency-free coverage gaps may be filled;
- missing DPI/Designer/accessibility/UIA evidence must remain visibly missing rather than being inferred.

## Suggested order

Use **#26 first**, then **#28**, then **#27**.

The order is deliberate: lifecycle hardening gives immediate product-quality value; the coverage matrix then shows what evidence is truly missing; packaging comes after the current behavior and evidence are better understood.

Do not run these issues concurrently if they touch the same verification files or project metadata. Small, serial pull requests are easier to review and keep the repository gate meaningful.

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
