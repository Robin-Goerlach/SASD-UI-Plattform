# Third-Party Notices

The SASD UI Platform uses third-party packages only behind reviewed package and adapter boundaries. The repository's MIT licence applies to SASD-authored code; third-party components remain governed by their own licences.

## Krypton.Toolkit

- **Package:** `Krypton.Toolkit`
- **Reviewed/pinned version:** `105.26.7.201`
- **Purpose:** R0.2 WinForms visual-component pilot, isolated to `Sasd.Ui.WinForms.Krypton`
- **Licence:** BSD 3-Clause
- **Upstream:** Krypton-Suite / Standard-Toolkit

The project does not copy Krypton source code into the SASD repository. Applications that consume the Krypton adapter receive the dependency through NuGet and must preserve applicable third-party notices when redistributed.

## Dependency review requirements

Before another third-party dependency is added, the project requires:

1. an Architecture Decision Record or an existing ADR that explicitly covers the dependency;
2. identification of the exact package and licence version;
3. review of transitive dependencies and notices;
4. security and maintenance assessment;
5. public-API leakage and exit-strategy review;
6. an update to this file and the release SBOM.

The conceptual screenshot in `artefacts/` was generated specifically for this project. Architecture diagrams are project-authored Graphviz sources and renderings.
