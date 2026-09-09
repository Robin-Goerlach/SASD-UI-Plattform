# SASD UI Platform — Agent Guide

This file is the short working map for coding agents. It is not the full project specification.
The authoritative project language is **English**. Read the linked repository documents before making non-trivial changes.

## 1. Project purpose

SASD UI Platform is a modular, designer-friendly C# WinForms component platform for SASD applications.
The current implementation target is **.NET 8 / WinForms on Windows**. WPF, WinUI, Avalonia, web, Java, mobile, macOS and Linux desktop are not current implementation targets unless a task explicitly changes that scope.

## 2. Read before changing code

Use these documents as the source of truth, in this order:

1. [`ROADMAP.md`](ROADMAP.md) — current implementation stage and next approved work.
2. [`docs/en/core/04-architecture.md`](docs/en/core/04-architecture.md) — module boundaries and dependency direction.
3. [`docs/en/core/03-functional-specification.md`](docs/en/core/03-functional-specification.md) — component behavior and planned scope.
4. [`docs/en/project/03_development-guidelines.md`](docs/en/project/03_development-guidelines.md) — coding and WinForms rules.
5. [`docs/en/project/04_test-strategy.md`](docs/en/project/04_test-strategy.md) — required quality evidence.
6. [`docs/en/project/07_dependencies-licenses-and-sbom.md`](docs/en/project/07_dependencies-licenses-and-sbom.md) — dependency and license rules.
7. The nearest `README.md` for the module being changed.

If documentation and implementation disagree, do not silently invent a third interpretation. Prefer the newer explicit decision, preserve requirement/ADR intent, and note the discrepancy in the task result.

## 3. Non-negotiable architecture rules

- `Sasd.Ui.Core` remains platform-neutral and must not reference WinForms, Windows-specific APIs or vendor UI packages.
- Product projects under `src/` must not reference samples or tests.
- Third-party implementation types must stay inside explicit adapter packages. Do not leak vendor types into neutral public APIs.
- Krypton remains isolated. Do not make it a dependency of native/core projects.
- UI components perform no direct SQL, ORM, database or application-domain persistence work.
- UI-state persistence stores presentation state only; never store secrets or domain records there.
- Prefer composition over deep inheritance. Stable base classes are `SasdForm`, `SasdDialogForm` and `SasdUserControl`.
- Do not create thin wrappers that only rename primitive WinForms controls.
- Do not mix multiple visible theme systems in one application.
- Do not add hidden telemetry, background threads or process-wide mutable state.

Architecture checks under `tests/architecture/` mechanically enforce part of these rules. Keep them strict.

## 4. Implementation style

- Prefer small, robust and readable implementations over clever abstractions or premature optimization.
- Reuse existing SASD patterns before introducing a new pattern.
- Public types and public members require useful XML documentation.
- Comments explain decisions, lifecycle, ownership, security boundaries or non-obvious WinForms behavior; do not narrate obvious code.
- Nullable Reference Types remain enabled. Fix nullability problems rather than suppressing them casually.
- Treat analyzer warnings as defects unless there is a documented, narrow reason not to.
- `async void` is allowed only for genuine UI event handlers.
- Long-running asynchronous operations accept `CancellationToken` where meaningful.
- Expected operational/user failures should return controlled results; programming defects may fail visibly.
- Preserve designer safety: parameterless constructors, no file/network/database work during construction, and no DI/service lookup from the designer path.
- Explicitly document and implement ownership for `Image`, `Icon`, `Font`, streams, timers, handles, events and other disposable resources.
- Controls must unsubscribe events and release owned resources on disposal.
- Never use `Application.DoEvents()` loops or `Thread.Sleep()` as UI synchronization.

## 5. Security and safety defaults

Security, privacy, legal and dependency concerns are considered before convenience.

- Do not open dropped files, shell targets, URLs or embedded content without the existing validation/service boundaries.
- A file extension is not proof of file content or trust.
- Do not compose unvalidated shell command lines.
- Do not expose raw stack traces as the main user-facing error message.
- Do not persist credentials, tokens, personal secrets or connection strings in UI state.
- Optional UI functionality must not prevent application startup when it fails.

## 6. What an agent may do autonomously

Without a new strategic decision, an agent may:

- implement items already approved by the roadmap, functional specification or existing ADRs;
- fix build, analyzer, test, disposal, accessibility and nullability defects;
- add missing XML comments and decision-focused comments;
- add or strengthen tests for existing behavior;
- extend the Component Gallery to demonstrate already-approved components;
- refactor locally for readability when public behavior and architecture remain stable;
- add dependency-free/native R1/R2 helpers that are already named and scoped by the functional specification.

## 7. Stop and request a strategic decision before

Do not silently make any of these changes:

- adding a new third-party **runtime** dependency or changing to another vendor component;
- changing the final native-vs-Krypton visual-standard decision;
- changing target framework, supported operating system or adding another UI platform;
- introducing a new database/ORM/data-access dependency into the UI platform;
- adding telemetry, an agent service, a background daemon or network service;
- moving responsibilities across major package/module boundaries;
- introducing a breaking public API when a compatible fix is reasonably possible;
- adding R3/out-of-scope components such as Ribbon, spreadsheet, Pivot/OLAP, Gantt, Office editor or designer engines without an approved requirement;
- accepting unusual/copy-left/commercial license obligations without the documented dependency review.

When escalation is required, describe the decision, alternatives, consequences and recommended default. Do not implement the strategic choice first and ask afterward.

## 8. Validation before finishing

On Windows, run the repository verification entry point:

```powershell
pwsh ./build/verify.ps1
```

The script performs restore, strict Release build, architecture checks and the current smoke suites. The GitHub Actions workflow in [`.github/workflows/ci.yml`](.github/workflows/ci.yml) remains the final merge gate.

On a non-Windows agent environment, run the compile/architecture subset supported by `build/verify.ps1 -CompileOnly` and clearly state that WinForms runtime checks were not executed. Never claim tests passed when they were not run.

For a small change, focused tests may be run during development, but run the broad verification command before declaring the task complete when the environment supports it.

## 9. Git and task hygiene

- Work on the branch/ref provided by the task environment. Do not rewrite published history.
- Never force-push, amend someone else's commit, or reset unrelated work.
- Keep changes focused; do not opportunistically reformat unrelated files.
- Commit completed work when the task environment permits commits.
- Use concise, descriptive commit messages such as `R1:`, `R2:`, `Fix:`, `Test:`, `Docs:` or `CI:` when those prefixes improve clarity.
- Do not merge a pull request unless the task explicitly authorizes merging.
- Leave the working tree clean when the environment provides a normal Git worktree.

## 10. Definition of done for agent work

A coding task is not complete merely because code was written. Before finishing, confirm that:

- the requested behavior exists and important edge cases are handled;
- public APIs are documented;
- ownership/disposal and UI-thread behavior are explicit where relevant;
- relevant tests were added or updated;
- strict build/analyzers pass in the available environment;
- no architecture boundary or dependency rule was weakened;
- documentation/roadmap status was updated when the change materially alters project state;
- remaining limitations or unexecuted checks are stated plainly.
