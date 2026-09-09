# ADR-0013 – UI Automation, Visual Regression, and Manual Accessibility Review

- **Status:** To Validate
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

WinForms quality cannot be proven completely through unit tests.

## Decision

A combination of UI Automation/FlaUI, screenshot regression, and manual accessibility/designer review is piloted in R0.

## Alternatives Considered

- manual tests only
- screenshot tests only
- complete end-to-end automation

## Positive Consequences

- broad quality coverage
- early regression detection

## Negative Consequences and Risks

- runner maintenance and flaky-test effort

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Evaluate after R0.3 based on stability, execution time, and defect-detection value.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
