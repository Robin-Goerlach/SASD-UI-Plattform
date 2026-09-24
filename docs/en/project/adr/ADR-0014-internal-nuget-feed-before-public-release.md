# ADR-0014 – Internal NuGet Feed before Public Release

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

The API must first be stabilized in real SASD applications before public support is expected.

## Decision

R0/R1 are distributed through an internal/private feed and release archives. Public open-source release follows only after R1 stabilization and licensing/governance review.

## Alternatives Considered

- immediate public GitHub/NuGet release
- copying DLLs
- referencing source code directly

## Positive Consequences

- a controlled learning phase
- clean package use without premature public commitments

## Negative Consequences and Risks

- less external feedback
- the feed must be operated

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess after R1.1 and three real consumer applications.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
