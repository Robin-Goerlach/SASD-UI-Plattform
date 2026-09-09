# Documentation Landscape and Document Control

## 1. Goal

The documentation landscape prevents contradictory standalone files and unnecessary duplicate maintenance. Every document has a clearly defined role.

## 2. Normative Hierarchy

1. **Requirements specification:** What is needed and why?
2. **Functional specification:** How will the need be implemented technically?
3. **Architecture document:** Which structural decisions and boundaries apply?
4. **ADRs:** Why was a specific architecture decision made?
5. **Roadmap/project plan:** In which order will delivery occur?
6. **Development, testing, build, and release guides:** How is daily work performed and verified?
7. **Component Gallery and samples:** Executable evidence of actual behavior.

When documents conflict, the higher-ranked normative document takes precedence. A detected conflict is not interpreted silently; it is resolved through a documented change.

## 3. Document Types

| Type | Content | Trigger for Change |
| --- | --- | --- |
| Requirements document | Goals, scope, must/should/may priorities | Changed product need |
| Functional/architecture document | Technical solution and boundaries | Architecture or scope decision |
| ADR | Individual irreversible or expensive decision | New technology, package boundary, security model |
| Roadmap | Delivery order and gates | Gate review, capacity, or priority change |
| Guideline | Day-to-day development rules | Repeated quality deviation |
| Runbook/support | Diagnosis and maintenance | New operational case or incident |
| Reference | Component API and examples | New or changed public function |

## 4. Maintenance Responsibilities

In a current one-person or small-team setting, one person may fill several roles; the roles remain conceptually separate:

- **Product Owner:** scope and priority;
- **Architect:** package boundaries, ADRs, and technical coherence;
- **Maintainer:** releases, dependencies, and support;
- **Quality Owner:** tests, accessibility, DPI, and release gates;
- **Documentation Owner:** clarity, links, and revision status.

## 5. Metadata Standard

Every major document contains:

- title;
- product line;
- version/revision;
- status;
- scope;
- change history or a reference to `CHANGELOG.md`;
- references to superior and subordinate documents.

## 6. Storage

```text
/
├─ README.md
├─ ROADMAP.md
├─ CHANGELOG.md
├─ CONTRIBUTING.md
├─ SECURITY.md
├─ docs/
│  ├─ 00_document-landscape.md
│  ├─ ... subject and process documents
│  └─ adr/
└─ .github/
```

## 7. Review Cadence

- Gate review: roadmap, risks, ADR status, and open decisions.
- Release review: changelog, security, SBOM, migration, and known limitations.
- Quarterly or after major updates: future register and dependencies.
- Immediately: security-relevant or licensing-critical changes.
