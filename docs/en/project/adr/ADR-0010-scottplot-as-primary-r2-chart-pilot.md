# ADR-0010 – ScottPlot as the Primary R2 Chart Pilot

- **Status:** Planned
- **Date:** 2026-07-23
- **Product line:** SASD UI Platform – C# WinForms Components

## Context

Technical charts are expected in dashboards and monitoring views but must not add weight to the R1 core.

## Decision

ScottPlot is evaluated as the first R2 adapter for technical line, bar, and scatter charts.

## Alternatives Considered

- LiveCharts2 as the primary adapter
- an in-house chart engine
- a commercial chart package

## Positive Consequences

- strong technical-chart focus
- a separate opt-in package

## Negative Consequences and Risks

- visual dashboard requirements may require another adapter

## Security, Privacy, and Licensing

The decision is reviewed through the dependency, security, and licensing process wherever third-party components, persistence, files, processes, or network boundaries are involved. It must not introduce secrets into UI state or logs and must not create unresolved redistribution rights.

## Validation

The decision is verified by the associated R0/R1/R2 pilot, architecture tests, Component Gallery pages, consumer samples, and—where applicable—real SASD applications.

## Review Criterion

Review after a concrete dashboard scenario and performance/accessibility pilot.

## Relationships

- Requirements specification: affected mandatory and scope requirements.
- Functional specification: technical implementation and release allocation.
- Architecture document: ADR register and related architecture chapters.
