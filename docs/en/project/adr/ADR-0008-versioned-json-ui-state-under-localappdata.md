# ADR-0008 – Versioned JSON UI State under LocalAppData

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Window, grid, and layout state require robust local persistence without introducing a database dependency.

## Decision

UI state is stored as versioned UTF-8 JSON under `%LocalAppData%\SASD-GmbH\<Product>`, written atomically, backed up, and migrated. Secrets are prohibited.

## Alternatives Considered

- the Windows Registry
- SQLite in the core
- Application Settings without a schema and recovery concept

## Positive Consequences

- transparent and portable
- easy to test
- no database dependency

## Negative Consequences and Risks

- file corruption must be handled
- concurrent processes require an explicit strategy

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess if multi-process or roaming requirements are demonstrated.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
