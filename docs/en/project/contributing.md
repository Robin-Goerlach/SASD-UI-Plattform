# Contribution Guide

## 1. Principle

Contributions to the SASD UI Platform must make the platform smaller, more reliable, or easier to use. A high number of new controls is not a quality metric. Every change should address at least one real application scenario.

## 2. Before Starting a Change

A small defect correction requires an issue. New components, new third-party packages, new public APIs, or changed package boundaries also require a short design note or ADR.

A component proposal answers:

- Which SASD applications need the function?
- Why are existing WinForms or SASD components insufficient?
- Is the solution a control, composite control, service, controller, extension, or adapter?
- Which accessibility, DPI, designer, and lifecycle risks exist?
- Which third-party and licensing consequences arise?
- What is explicitly excluded from the first version?

## 3. Branch and Pull Request Model

- `main` remains buildable and releasable.
- Changes are developed in short feature or fix branches.
- Pull requests remain focused on one subject.
- A PR does not mix broad formatting changes with functional changes.
- Breaking changes require an ADR, changelog entry, and migration note.

## 4. Definition of Ready

A task is ready for implementation when:

- the goal and non-goals are described;
- the release allocation and affected packages are known;
- acceptance criteria are testable;
- design and third-party-library risks have been assessed;
- required test levels are defined.

## 5. Definition of Done

- Code compiles without new warnings.
- Nullable suppressions are justified.
- Public APIs have XML documentation.
- Unit, integration, and architecture tests are added.
- Visual components have a Component Gallery page.
- Keyboard operation, DPI, theme, High Contrast, localization, and accessibility have been reviewed.
- Resources and event subscriptions are released correctly.
- Changelog, migration notes, and known limitations are updated.
- The license inventory and SBOM include new dependencies.

## 6. Code Review Focus

1. **Scope:** Is the function genuinely reusable across the platform?
2. **API:** Is it understandable and free of unintended third-party types?
3. **Designer:** Can the control be created and saved in the Visual Studio Designer?
4. **Lifecycle:** Are events, timers, bitmaps, handles, and native resources released?
5. **Async:** Is the UI thread handled correctly, including cancellation and error flow?
6. **UX:** Are focus, keyboard, error, busy, and empty states consistent?
7. **Tests:** Do tests prove critical properties rather than implementation details only?

## 7. Dependencies

New NuGet packages require the following before merge:

- license identification;
- activity and maintenance evaluation;
- review of transitive dependencies;
- security/vulnerability scan;
- exit strategy;
- explanation of why in-house development or existing packages are insufficient.

## 8. Documentation

Markdown files use clear headings, short paragraphs, tables only where comparison benefits from them, and relative links. Public code examples must compile or be explicitly identified as pseudocode.
