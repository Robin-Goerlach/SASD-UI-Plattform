# ADR-0005 – No Blanket Wrappers for Primitive Controls

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Thin wrappers such as `SasdButton` or `SasdLabel` would inflate the API, designer surface, and maintenance burden without enough value.

## Decision

Primitive controls remain native or themed. Custom classes are created for composite controls, services, and recurring behavior.

## Alternatives Considered

- wrapping every control in a SASD class
- providing no custom controls

## Positive Consequences

- smaller API
- better designer compatibility
- focus on real added value

## Negative Consequences and Risks

- not every visual property can be enforced centrally

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess when several applications require the same non-trivial extension to a primitive control.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
