# ADR-0006 – Composition Root Instead of a Service Locator in Controls

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Controls require services but must not resolve hidden global containers.

## Decision

The application configures services in the composition root. Controls receive dependencies through controllers, properties, factories, or an explicit host; designer paths remain parameterless.

## Alternatives Considered

- a global service locator
- full constructor injection in every control

## Positive Consequences

- testability
- explicit dependencies
- continued designer usability

## Negative Consequences and Risks

- additional composition logic
- separate runtime and designer paths are required

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess if designer/runtime composition creates excessive boilerplate.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
