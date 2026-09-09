# Source Code Agent Guide

These instructions apply to all product code under `src/` and refine the repository root `AGENTS.md`.

## Before editing

- Read the nearest module `README.md`.
- Inspect sibling components before inventing a new pattern.
- For architecture-relevant work, read `docs/en/core/04-architecture.md` and the applicable functional-specification section.
- Check whether the requested component is already listed in `ROADMAP.md` or `docs/en/core/03-functional-specification.md`.

## Public API rules

- Public types and members require useful XML documentation.
- Prefer SASD/.NET types in public neutral APIs. Vendor-specific types belong only in explicit adapter projects.
- Keep nullability explicit and analyzer-clean.
- Prefer additive, compatible APIs. Do not casually rename or remove public members.
- Do not expose test-only hooks merely to make a test convenient; test observable behavior or use reflection inside smoke tests when appropriate.

## WinForms component rules

- Preserve parameterless construction and designer safety.
- Constructors must not perform file, network, database, shell or long-running work.
- Use DPI-aware layout and system colors unless a theming abstraction deliberately overrides them.
- Keyboard and accessibility behavior are part of the component contract, not optional polish.
- Do not use color as the only carrier of status/meaning.
- Avoid hidden global state and service locators.
- Use composition before deep visual inheritance.

## Ownership and lifecycle

For every event subscription or disposable/native resource, decide who owns it.

- If a component creates/clones an `Image`, `Icon`, `Font`, `Timer`, stream or other disposable resource, it normally owns and disposes that resource.
- If an application passes an object/control/service to a component, the application normally remains the owner unless the API explicitly documents transfer of ownership.
- Unsubscribe from events when a binding/component is disposed or unbound.
- Avoid stale WinForms handles, leaked GDI resources and hidden process-lifetime subscriptions.
- Do not dispose caller-owned resources merely because the component referenced them.

## Threading and failure behavior

- WinForms controls are UI-thread-affine. Marshal deliberately; do not rely on ambiguous `InvokeRequired` behavior before handle creation.
- Keep long-running work asynchronous and cancellable where meaningful.
- Expected operational failures should be converted to controlled results/messages at the appropriate service boundary.
- Optional UI conveniences should fail safely rather than preventing application startup.

## Module boundaries

- `Sasd.Ui.Core`: no Windows/WinForms/vendor dependency.
- `Sasd.Ui.WinForms`: base WinForms contracts/helpers only.
- `Sasd.Ui.WinForms.Krypton`: Krypton implementation only; vendor types must not leak out into neutral modules.
- `Sasd.Ui.WinForms.State`: UI state only, never business/domain persistence or secrets.
- `Sasd.Ui.WinForms.Data`: presentation/query contracts only, no SQL/ORM/database implementation.
- `Sasd.Ui.WinForms.Windows`: validated Windows integration boundaries; never execute unvalidated shell input.
- `Sasd.Ui.WinForms.Media`: image/media presentation and ownership, not implicit file loading or content trust.

## New dependencies

Do not add a new runtime package without an explicit dependency/ADR decision. If a task seems to require one, stop and report:

1. why the dependency is needed;
2. viable native/existing alternatives;
3. license and maintenance implications;
4. proposed package boundary and exit strategy.

## Verification

After source changes, add or update the nearest behavioral smoke checks and run the repository verification command from the root when the environment supports it:

```powershell
pwsh ./build/verify.ps1
```
