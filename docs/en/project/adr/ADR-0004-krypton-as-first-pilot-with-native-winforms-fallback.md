# ADR-0004 – Krypton as the First Pilot with a Native WinForms Fallback

- **Status:** To Validate
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Krypton provides a broad WinForms foundation but must meet designer, DPI, accessibility, and maintenance requirements.

## Decision

Krypton is piloted in R0.2 as the first visible implementation. Native WinForms remains the functional fallback behind the same tokens and services.

## Alternatives Considered

- making Krypton mandatory immediately
- native WinForms only
- AntdUI or ReaLTaiizor as the standard

## Positive Consequences

- a realistic suite candidate
- no premature lock-in
- a comparable reference implementation

## Negative Consequences and Risks

- duplicated pilot effort
- possible differences between fallback and standard implementation

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Accept or reject this ADR after completion of the R0.2 evaluation matrix.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
