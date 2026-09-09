# Engineering (`eng`)

`eng` means **engineering**, not English. English-language project documentation lives under [`docs/en`](../docs/en/README.md); the German companion documentation lives under [`docs/de`](../docs/de/README.md).

This directory is reserved for repository-wide engineering assets that support building, validating, packaging and releasing the SASD UI Platform but are not product source code.

## What belongs here

Typical future contents include:

- public-API baseline and compatibility configuration;
- dependency and licence-policy helpers;
- SBOM generation and verification helpers;
- package/release metadata and versioning support;
- repository validation configuration shared by CI and local builds;
- release-engineering scripts or configuration that do not belong to one product project.

## What does not belong here

- application or component source code — use `src/`;
- tests — use `tests/`;
- user/developer documentation — use `docs/`;
- generated screenshots and diagrams — use `artefacts/`;
- normal build entry points — prefer `build/` when they are commands/scripts developers invoke directly.

## Current status

The directory is intentionally small during R0. Engineering assets should be added only when a real validation, packaging or release requirement exists; placeholder complexity is not a project goal.
