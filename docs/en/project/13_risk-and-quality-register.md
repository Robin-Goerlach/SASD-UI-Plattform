# Risk and Quality Register

## 1. Assessment

- Probability: low, medium, high.
- Impact: low, medium, high, critical.
- Status: open, monitored, mitigated, accepted, closed.

## 2. Initial Risk Register

| ID | Risk | Probability | Impact | Mitigation | Stop Criterion |
| --- | --- | --- | --- | --- | --- |
| R-001 | Krypton designer is unstable | medium | high | R0 designer matrix, native fallbacks | Reproducible data loss in the designer |
| R-002 | Too many published packages | medium | medium | Internal separation, few consumer packages | Base application needs more than eight direct SASD packages |
| R-003 | Platform turns into an enterprise-grid reimplementation | high | critical | Fixed grid scope, build-versus-buy review | Pivot, formula editor, or designer becomes a core goal |
| R-004 | UI Automation is flaky | medium | medium | Automation IDs, explicit waits, quarantine deadline | More than 5% failures without product defects |
| R-005 | Handle or memory leak | medium | high | Cycle and lifecycle tests | Continuing growth over 100 cycles |
| R-006 | License changes | low/medium | high | SBOM, adapters, update review | Redistribution rights unclear |
| R-007 | Third-party types leak into the core API | medium | high | Architecture tests | Consumers must know Krypton |
| R-008 | Scope grows too quickly | high | high | Roadmap gates, proposal process | R2 blocks R1 |
| R-009 | State corruption | low/medium | high | Atomic writes, backup, migration tests | Startup or user data is endangered |
| R-010 | Accessibility is postponed | medium | high | Gallery states and Definition of Done | Core control lacks keyboard or accessibility support |
| R-011 | Windows/.NET update breaks behavior | medium | medium/high | Compatibility matrix, maintenance reviews | No sustainable runtime combination remains |
| R-012 | Single-person knowledge risk | high | medium/high | Detailed docs, ADRs, samples | Critical function is known only implicitly |

## 3. Quality Metrics

- successful build from a clean checkout;
- zero critical licensing or security findings;
- zero unjustified compiler warnings;
- 100% public API documentation for published types;
- 100% Gallery coverage for public visual components;
- resource cycle without continuing handle growth;
- passed keyboard and DPI matrix for R1;
- migration tests for every state schema version;
- consumer smoke test using published NuGet packages.

## 4. Quality Deviations

An accepted deviation requires:

- exact affected version and component;
- user impact;
- workaround;
- risk decision;
- owner and target release;
- no silent postponement without renewed review.
