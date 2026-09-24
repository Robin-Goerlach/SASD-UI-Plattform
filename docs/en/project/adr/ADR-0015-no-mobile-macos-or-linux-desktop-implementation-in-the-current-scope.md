# ADR-0015 – No Mobile, macOS, or Linux Desktop Implementation in the Current Scope

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Cross-platform support would be useful but would broaden the current WinForms effort substantially.

## Decision

Android, iOS, macOS, and Linux desktop receive no implementation projects or dependencies at present. Ideas remain recorded in the future register.

## Alternatives Considered

- parallel Avalonia/MAUI development
- a platform-neutral core from the beginning

## Positive Consequences

- clear focus
- lower testing and packaging effort

## Negative Consequences and Risks

- a separate rendering implementation will be required later

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess when there is concrete product or customer demand and sufficient capacity.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
