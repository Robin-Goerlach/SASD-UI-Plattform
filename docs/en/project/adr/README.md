# Architecture Decision Records (ADR)

## Purpose

ADRs document individual significant architecture decisions. They do not replace the architecture document; they preserve context, alternatives, consequences, and review criteria.

## Status Values

- Proposed
- Accepted
- Rejected
- Superseded
- Deprecated
- Planned/To Validate

## File Naming

`ADR-0001-short-title.md`

Numbers are never reused. A later decision supersedes an older ADR through a reference but does not delete it.

## Required Content

- status and date;
- context/problem;
- decision;
- alternatives considered;
- positive and negative consequences;
- security and licensing impact;
- validation and review criteria;
- relationships to requirements, functional specification, and architecture.

## Register

| ADR | Title | Status |
| --- | --- | --- |
| 0001 | WinForms and .NET 8 as the R0/R1 platform | Accepted |
| 0002 | Modular monorepo with few public packages | Accepted |
| 0003 | Design tokens and theme service | Accepted |
| 0004 | Krypton as the first pilot with native fallback | To Validate |
| 0005 | No blanket primitive-control wrappers | Accepted |
| 0006 | Composition root instead of service locator | Accepted |
| 0007 | GridController separated from the designer control | Accepted |
| 0008 | Versioned JSON UI state | Accepted |
| 0009 | Third-party libraries behind adapter boundaries | Accepted |
| 0010 | ScottPlot as the primary chart pilot | Planned |
| 0011 | ScintillaNET as a separate code editor adapter | Planned |
| 0012 | WebView2 with default-deny security | Planned |
| 0013 | UIA, visual regression, and manual accessibility review | To Validate |
| 0014 | Internal NuGet feed before public release | Accepted |
| 0015 | No mobile/macOS/Linux desktop implementation in current scope | Accepted |
| 0016 | WPF, ASPX/web, and Java as separate product lines | Accepted |
