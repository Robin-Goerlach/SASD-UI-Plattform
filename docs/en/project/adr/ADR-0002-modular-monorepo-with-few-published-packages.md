# ADR-0002 – Modular Monorepo with Few Published Packages

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Components, samples, tests, and adapters must remain versioned together without burdening consumers with a large number of direct package references.

## Decision

A monorepo contains separate internal projects. Only a small set of coherent R1 packages and opt-in R2 adapters are published.

## Alternatives Considered

- one large assembly
- many independent repositories
- publishing every project as a separate package

## Positive Consequences

- consistent changes and CI
- clear internal boundaries
- a manageable consumer-facing surface

## Negative Consequences and Risks

- a larger repository
- the release pipeline must handle package relationships correctly

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess if genuinely independent teams or release cycles emerge.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
