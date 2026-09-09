---
name: Codex development task
about: Bounded implementation or hardening task suitable for Codex
Title: "[Codex] "
---

## Goal

<!-- Describe the observable result. Prefer one coherent outcome over a broad wishlist. -->

## Relevant scope

<!-- Point Codex to the smallest useful area. Add existing patterns/files when known. -->

- Project/component:
- Existing pattern to follow:
- Related roadmap/specification/ADR:

## Out of scope

<!-- State what must not be redesigned or expanded as part of this task. -->

- No unrelated refactoring.
- No new runtime dependency unless separately approved.
- No platform/framework/vendor change unless separately approved.

## Acceptance criteria

- [ ] Requested behavior is implemented and important edge cases are handled.
- [ ] Public API changes have useful XML documentation.
- [ ] Ownership/disposal/threading behavior is explicit where relevant.
- [ ] Relevant behavioral smoke checks are added or updated.
- [ ] No architecture rule or analyzer gate is weakened.
- [ ] `pwsh ./build/verify.ps1` passes on Windows, or the task result clearly states that only `-CompileOnly` was possible.
- [ ] Documentation/roadmap status is updated if the implementation materially changes project status.

## Decision boundary

Codex must stop and explain alternatives **before** implementing a change that requires any of the following:

- a new third-party runtime dependency;
- a target-framework, supported-OS or UI-platform change;
- the final native-vs-Krypton visual-standard decision;
- a major package/module-boundary change;
- a database/ORM responsibility inside the UI platform;
- an avoidable breaking public API;
- telemetry, a network service, daemon or hidden process-wide behavior;
- unusual/commercial/copy-left licensing obligations;
- an R3/out-of-scope enterprise component without an approved requirement.

## Verification notes

<!-- Codex fills this in before closing/handing off the task. -->

- Focused checks run:
- Broad verification run:
- Windows runtime checks executed: yes / no
- Remaining limitations:
