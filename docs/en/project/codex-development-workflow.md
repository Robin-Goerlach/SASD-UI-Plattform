# Codex Development Workflow

## 1. Purpose

This document describes how Codex should be used to develop the SASD UI Platform without weakening its architecture, quality gates or decision process.

The repository intentionally uses a short root [`AGENTS.md`](../../../AGENTS.md) as the persistent agent map. The detailed project knowledge remains in the normal repository documentation, especially the architecture, functional specification, roadmap and development/test guidelines.

## 2. Repository-level Codex setup

No credentials, API keys or machine-specific Codex authentication settings belong in this repository.

For repository instructions, Codex uses [`AGENTS.md`](../../../AGENTS.md). Open the repository root as the working directory so the root instructions and project documentation are available to the agent.

The repository does not commit a user-specific `config.toml`. Sandbox mode, login method, model preference, writable directories and MCP credentials are workstation/workspace policy and should be configured outside the repository.

## 3. Recommended task shape

Codex works best when a task resembles a focused GitHub issue. Include the desired outcome, relevant files/components, boundaries and acceptance criteria.

Recommended task template:

```text
Title: <short task title>

Goal:
<what should work after the change>

Relevant scope:
- <component/project/file>
- <existing pattern to follow>

Out of scope:
- <what should not be redesigned>

Acceptance criteria:
- <observable behavior>
- <important edge case>
- public API/XML comments updated where needed
- relevant smoke tests added or updated
- pwsh ./build/verify.ps1 passes on Windows

Decision boundary:
If the implementation requires a new runtime dependency, package-boundary change,
breaking public API, platform change or other strategic decision, stop and explain
alternatives instead of choosing silently.
```

## 4. Tasks suitable for autonomous Codex work

Good tasks are bounded by existing decisions. Examples:

- implement a component already listed in the functional specification or roadmap;
- extend an existing SASD component using the established module pattern;
- add tests for known edge cases;
- fix analyzer, nullable, disposal, threading or state-recovery defects;
- add Component Gallery coverage for an implemented component;
- improve comments/XML documentation without changing behavior;
- perform a small readability refactor inside one module;
- implement a dependency-free R1/R2 helper already approved by project scope.

These tasks can normally be completed without a new architecture decision.

## 5. Tasks that require escalation

Codex must stop and present alternatives before implementing work that would:

- add a new third-party runtime package;
- replace or promote a UI vendor/visual system;
- decide the final native-vs-Krypton standard;
- introduce another target UI platform;
- add direct database/ORM responsibility to the UI platform;
- change major project/package boundaries;
- introduce a breaking public API where compatibility is reasonably possible;
- add telemetry, network services, background daemons or hidden process-wide behavior;
- introduce non-trivial licensing obligations;
- add an out-of-scope R3/enterprise control without an approved requirement.

The escalation should state:

1. the decision required;
2. at least two realistic options when available;
3. effects on architecture, maintenance, security and licensing;
4. a recommended default;
5. what implementation can safely continue before the decision.

## 6. Working sequence for Codex

For a normal coding task:

1. Read the root `AGENTS.md`.
2. Read `ROADMAP.md` and the relevant module `README.md`.
3. Read the applicable architecture/functional-specification sections.
4. Search the repository for the closest existing implementation pattern.
5. Make the smallest coherent implementation that satisfies the requirement.
6. Add decision-focused comments and public XML documentation.
7. Add or update tests before considering the task finished.
8. Run focused checks while iterating.
9. Run the broad repository verification command before final completion when Windows is available.
10. Review the diff for unrelated changes, dependency leakage and accidental public-API changes.
11. Commit the completed work when the task environment permits commits.

## 7. Verification command

The canonical local/agent verification command is:

```powershell
pwsh ./build/verify.ps1
```

On Windows it runs the same automated gate as GitHub Actions:

- restore;
- strict Release build with warnings treated as errors;
- architecture checks;
- Core smoke tests;
- UI-state smoke tests;
- WinForms foundation smoke tests;
- Windows integration smoke tests;
- Shell integration smoke tests;
- native R2 smoke tests;
- data/dashboard smoke tests;
- Krypton regression smoke tests.

For a non-Windows Codex environment:

```powershell
pwsh ./build/verify.ps1 -CompileOnly
```

This is intentionally not equivalent to the Windows gate. The agent must say that WinForms runtime tests were not executed and rely on GitHub Actions for the final Windows result.

## 8. Code expectations

Codex-generated code is held to the same rules as human-written code:

- simple and readable before optimized;
- no speculative abstractions;
- no hidden service locator;
- no database access from UI controls;
- no silent dependency additions;
- no `Thread.Sleep`/`Application.DoEvents()` synchronization;
- public API has useful XML documentation;
- complex or non-obvious WinForms behavior has explanatory comments;
- resource ownership and event unsubscription are explicit;
- security-sensitive paths default to rejection or controlled failure;
- accessibility must not depend on color alone.

## 9. Git/PR behavior

The task environment may control branch creation differently depending on whether Codex runs locally, in the desktop app or in a managed task.

Regardless of environment:

- do not rewrite or force-push published history;
- do not modify unrelated commits;
- keep each commit coherent;
- do not merge unless explicitly authorized by the task;
- do not weaken CI/analyzer rules merely to obtain a green build;
- when a test exposes a real product defect, fix the product rather than changing the test to hide it.

Useful commit prefixes in this repository are `R1:`, `R2:`, `Fix:`, `Test:`, `Docs:` and `CI:`.

## 10. How to use Codex for larger work

For a larger feature, split work into reviewable slices instead of asking for a complete subsystem in one uncontrolled pass. A useful sequence is:

1. contracts/data model;
2. smallest usable implementation;
3. behavioral tests;
4. Gallery/sample integration;
5. documentation/roadmap update;
6. hardening after real use.

This mirrors the repository's existing development pattern and reduces the chance that an agent invents architecture to fill gaps.

## 11. Example Codex prompts

### Implement an approved component

```text
Implement the next dependency-free R2 component already described in the functional
specification. Follow existing Sasd.Ui.WinForms.Data patterns. Keep the API small,
add XML comments and behavior smoke tests, and run build/verify.ps1. Do not add a
third-party dependency. If the specification leaves a strategic choice open, stop
and explain the choice instead of deciding it.
```

### Harden an existing component

```text
Review SasdImageViewer for disposal, invalid input, DPI and accessibility edge cases.
Fix concrete defects without redesigning the public API. Add focused smoke tests and
run the full repository verification. Prefer clarity over micro-optimization.
```

### Extend Gallery coverage

```text
Add a Component Gallery page/scenario for the existing saved-grid-view and KPI
components. Demonstrate normal, empty and relevant error/disabled states. Reuse the
existing shell, commands, status and notification services. Do not introduce another
application framework or theme system.
```

## 12. Maintenance of agent instructions

`AGENTS.md` should remain short enough to act as a map. When a rule needs substantial explanation, put that explanation in the normal project documentation and link to it from `AGENTS.md`.

Update the agent guide when a durable architecture rule, verification command or escalation boundary changes. Do not add transient task details to `AGENTS.md`.
