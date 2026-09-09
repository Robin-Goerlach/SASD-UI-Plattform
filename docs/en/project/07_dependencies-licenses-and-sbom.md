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

- a machine-readable SBOM;
- `THIRD-PARTY-NOTICES` listing package, version, license, and source;
- a vulnerability-scan report;
- checksums for release artifacts.

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
