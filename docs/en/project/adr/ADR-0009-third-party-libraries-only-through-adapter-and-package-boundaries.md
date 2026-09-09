# ADR-0009 – Third-Party Libraries Only through Adapter and Package Boundaries

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

The platform must be able to use vendors without coupling every application permanently to vendor APIs.

## Decision

Third-party types remain outside core public APIs. Vendor-specific functions live in clearly named adapter packages.

## Alternatives Considered

- direct references in every application
- forking and merging source repositories

## Positive Consequences

- replaceability
- a smaller dependency surface
- clear licensing scope

## Negative Consequences and Risks

- adapter effort
- not every specialized function can be abstracted completely

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

If an abstraction creates more complexity than value, keep it explicitly vendor-specific inside its adapter.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
