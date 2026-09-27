# Dependencies, Licenses, and SBOM

## 1. Goal

Third-party components are used deliberately but are not absorbed uncontrolled into a mega-fork. Every dependency must be technically, legally, and operationally sustainable.

## 2. Evaluation Fields

| Field | Guiding Question |
| --- | --- |
| Function | Does the package solve a real, recurring need? |
| License | May it be redistributed internally, commercially, and potentially as open source? |
| Activity | Are releases, issues, and maintainers current? |
| Quality | Are tests, documentation, API stability, designer behavior, and DPI support adequate? |
| Security | Are there known vulnerabilities, native components, or update constraints? |
| API leakage | Does it force consumers to use third-party types? |
| Exit | Can it be replaced, isolated, or forked as a last resort? |
| Size | Which transitive packages and runtime costs result? |

## 3. Classification

- **Core dependency:** required for R1 and evaluated most strictly.
- **Adapter dependency:** present only in an optional package.
- **Build/test dependency:** not part of the consumer runtime.
- **Sample dependency:** used exclusively by a sample or the Gallery.
- **Rejected/deferred:** documented with a reason.

## 4. Licensing Rules

- permissive licenses such as MIT, BSD, and Apache are preferred but are not automatically risk-free;
- copyleft or unusual terms require explicit review;
- community/free licenses are not confused with open source;
- vendor trademarks and logos are not presented as SASD products;
- copyright and license notices are preserved;
- transitive dependencies are included in the assessment.

## 5. SBOM and Notices

Every release produces:

- a machine-readable standards-compliant SBOM;
- `THIRD-PARTY-NOTICES` listing package, version, license, and source;
- a vulnerability-scan report covering direct and transitive NuGet dependencies;
- checksums for release artifacts.

### Current automated R1 evidence

The repository already produces two complementary NuGet evidence files during the release dry-run:

- `nuget-dependencies.json` records the resolved direct/transitive package graph and verifies central direct-package versioning plus notice coverage;
- `nuget-vulnerabilities.json` records the timestamped result of `dotnet list package --vulnerable --include-transitive --format json` for every explicit product project.

Restore also enables `NuGetAuditMode=all` so transitive vulnerabilities are surfaced by normal NuGet auditing. The explicit vulnerability gate fails on any reported finding and also fails when the audit source is unavailable; a missing security feed must never be interpreted as a clean scan.

These files are **release evidence, not the final SBOM**. The dependency inventory does not claim SPDX or CycloneDX conformance, and the vulnerability report is inherently time-sensitive because advisory data can change after a build.

## 6. Fork Rule

A fork is permitted only when:

1. upstream does not respond adequately or is no longer maintained;
2. extension, adapter, or pull request is insufficient;
3. the license permits the fork;
4. tests cover the adopted surface;
5. a maintainer and maintenance budget are named;
6. synchronization and exit plans exist.

Forks are recorded with upstream commit, local differences, review date, and security status.

## 7. Update Process

- regular dependency updates in small groups;
- review vendor changelogs and breaking changes;
- run Gallery and visual tests before adopting visual updates;
- prioritize security updates;
- no automatic major update without review.

## 8. Initial Candidates

- Krypton Standard Toolkit: R0 pilot;
- Ookii Dialogs: optional behind services;
- ScottPlot: R2 chart pilot;
- Markdig/DiffPlex: R2 editor functions;
- ScintillaNET: separate R2 adapter;
- WebView2: hardened R2 host;
- PDFsharp/MigraDoc: R2 PDF generation.

Final approval follows the associated ADR and license-inventory review.
