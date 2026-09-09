# Documentation Standard

## 1. Goals

Documentation must make decisions traceable, code usable, and maintenance independent of implicit knowledge.

## 2. Documentation Levels

- product and requirements documents;
- architecture and ADRs;
- developer and process guides;
- API XML documentation;
- Component Gallery;
- samples and tutorials;
- release notes and migration;
- known issues and support diagnostics.

## 3. Markdown Rules

- one H1 per file;
- logically nested headings;
- relative links within the repository;
- tables only for structured comparisons;
- fenced code blocks with language identifiers;
- no raw local file paths as durable references;
- explicit distinction among intended, current, planned, and rejected states.

## 4. Public API

Every public type and member receives:

- purpose;
- parameter and return-value meaning;
- error/exception behavior;
- threading/lifecycle note where relevant;
- minimal example for non-trivial usage.

## 5. Component Reference

For every visual component:

- use cases and non-goals;
- package and namespace;
- important properties and events;
- theme, DPI, and accessibility behavior;
- designer guidance;
- state and persistence guidance;
- complete Gallery page.

## 6. Change Control

- Requirement changes update requirements and functional-specification references.
- Architecture changes update the ADR and architecture document.
- Public API changes update changelog and migration guidance.
- New third parties update licensing documentation and the SBOM.

## 7. Quality Review

Before release:

- check links;
- build examples;
- align terminology and package names;
- verify versions and status;
- replace outdated screenshots;
- state known limitations explicitly.
