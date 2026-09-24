# ADR-0011 – ScintillaNET as a Separate Code Editor Adapter

- **Status:** Planned
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Configuration and code views need syntax features, search, and large-file support; this complexity does not belong in the core.

## Decision

ScintillaNET is offered exclusively through a separate editor adapter package.

## Alternatives Considered

- extending RichTextBox
- a web editor hosted through WebView2
- building a custom text editor

## Positive Consequences

- a capable specialist editor
- a lightweight core

## Negative Consequences and Risks

- native resources and lifecycle considerations
- vendor-specific APIs

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Review after R2 lifecycle, licensing, and large-file tests.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
