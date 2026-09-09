# ADR-0012 – WebView2 with a Default-Deny Security Profile

- **Status:** Planned
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

HTML/Markdown preview and embedded web content are useful but introduce a significant trust boundary.

## Decision

`SasdWebViewHost` blocks navigation, downloads, and script bridges by default. Origins and functions must be explicitly allowed.

## Alternatives Considered

- an unhardened WebView2 control
- opening an external browser
- no embedded web content

## Positive Consequences

- controlled integration
- a clear security boundary

## Negative Consequences and Risks

- runtime dependency
- complex lifecycle and security maintenance

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

A security and lifecycle test is required before R2 release.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
