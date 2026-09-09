# ADR-0007 – Separate SasdGridController<T> from the Designer-Compatible Grid

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Generic WinForms controls cause designer problems, while typed grid configuration remains desirable.

## Decision

`SasdDataGrid` is non-generic and designer-compatible. `SasdGridController<T>` encapsulates typed columns, mapping, selection, commands, and validation.

## Alternatives Considered

- a generic grid control
- untyped DataGridView events only
- building a complete enterprise grid

## Positive Consequences

- designer compatibility
- typed configuration
- limited and controllable scope

## Negative Consequences and Risks

- an additional coordination layer
- the lifecycle must be bound cleanly

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Review after the grid spike and the first consumer migration.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
