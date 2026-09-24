# ADR-0016 – WPF, ASPX/Web, and Java as Separate Future Product Lines

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

These technologies have different UI, tooling, and architecture models.

## Decision

WPF, WinUI/Avalonia, modern web, ASP.NET Web Forms, and Java/OpenXava are planned as separate projects. Semantics, UX rules, and documentation are reused—not control code automatically.

## Alternatives Considered

- a single mega-repository for all platforms
- ignoring platform-specific concerns

## Positive Consequences

- clean technical boundaries
- targeted future evaluation

## Negative Consequences and Risks

- multiple repositories or product lines
- shared features must be synchronized deliberately

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

When a product line is activated, replace or refine this decision through its own requirements and functional specifications.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
