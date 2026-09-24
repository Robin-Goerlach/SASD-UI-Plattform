# ADR-0001 – WinForms and .NET 8 as the R0/R1 Platform

- **Status:** Accepted
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Existing SASD desktop applications and current development practices are based on C# and WinForms. Starting several UI technologies in parallel would spread capacity and reduce quality.

## Decision

R0 and R1 use Windows Forms with `net8.0-windows`. Windows 10/11 and the Visual Studio Designer are the reference environment. A current LTS release is verified in CI without unnecessary multi-targeting.

## Alternatives Considered

- WPF immediately as the primary platform
- WinUI 3 as the greenfield foundation
- Avalonia for cross-platform development

## Positive Consequences

- rapid reuse in existing applications
- a familiar designer and operating environment
- clear focus

## Negative Consequences and Risks

- Windows dependency
- later ports require a separate rendering implementation

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Reassess after R1.1 or when .NET 8 support ends.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
