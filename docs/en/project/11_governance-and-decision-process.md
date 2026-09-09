# Governance and Decision Process

## 1. Goal

Governance prevents the platform from expanding through spontaneous component requests, changing vendors, or uncontrolled abstractions.

## 2. Decision Types

| Decision | Process |
| --- | --- |
| Small implementation question | Code review |
| New public API | API review and changelog |
| New component | Component proposal |
| New third-party dependency | Dependency review plus ADR |
| Package boundary/architecture style | ADR |
| Breaking change | ADR, migration, and major/0.x decision |
| Scope change | Update requirements, functional specification, and roadmap |

## 3. Roles

- Product Owner prioritizes value and scope.
- Architect owns coherence and ADRs.
- Maintainer owns packages, releases, and upstream relationships.
- Quality Owner owns gates.
- Consumer Representative reviews usability in a real SASD application.

One person may fill several roles but must document the perspectives separately during review.

## 4. Decision Record

An ADR is required when a decision:

- is difficult to reverse;
- affects several packages or applications;
- introduces a new third-party library;
- has security or licensing impact;
- shapes a public API or persistence schema;
- separates a planned product line.

## 5. Admitting New Components

The decision flow is:

1. demonstrate the problem and consumers;
2. evaluate existing platform/framework functionality;
3. compare a local solution with a reusable module;
4. compare build, buy, adapter, and fork options;
5. define the smallest scope;
6. define tests and Gallery states;
7. assign release and maintenance ownership;
8. update backlog and documents after approval.

## 6. Stop the Line

Development and publication stop when there is:

- unresolved licensing status;
- a critical security vulnerability;
- a non-reproducible release build;
- risk of data loss or state corruption;
- a systematic designer or handle problem in the core;
- unreviewed third-party types in core APIs.

## 7. Review Record

Gate reviews document:

- completed evidence;
- accepted deviations;
- open risks;
- ADR status;
- Go/Conditional Go/No-Go decision;
- next concrete work packages.
