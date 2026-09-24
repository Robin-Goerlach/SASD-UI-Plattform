# ADR-0003 – Design Tokens and Theme Service as Visual Semantics

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Visual decisions must not be tied directly to Krypton or individual controls.

## Decision

Semantic design tokens define colors, typography, spacing, and states. `SasdThemeService` maps them to concrete WinForms and Krypton implementations.

## Alternatives Considered

- direct color values in each control
- using vendor palettes only

## Positive Consequences

- consistent appearance
- semantic reuse in later WPF and web product lines
- testable runtime theme switching

## Negative Consequences and Risks

- mapping effort
- not every vendor feature can be abstracted completely

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess if the token model imposes unacceptable designer or performance costs.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
