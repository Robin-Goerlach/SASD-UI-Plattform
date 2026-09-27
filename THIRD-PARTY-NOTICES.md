# Third-Party Notices

The SASD UI Platform uses third-party packages only behind reviewed package and adapter boundaries. The repository's MIT licence applies to SASD-authored code; third-party components remain governed by their own licences.

## Krypton.Toolkit

- **Package:** `Krypton.Toolkit`
- **Reviewed/pinned version:** `105.26.7.201`
- **Purpose:** R0.2 WinForms visual-component pilot, isolated to `Sasd.Ui.WinForms.Krypton`
- **Licence:** BSD 3-Clause
- **Upstream:** Krypton-Suite / Standard-Toolkit

The project does not copy Krypton source code into the SASD repository. Applications that consume the Krypton adapter receive the dependency through NuGet and must preserve applicable third-party notices when redistributed.

### Transitive .NET packages used by Krypton.Toolkit on net8.0-windows

The current resolved dependency graph for `Krypton.Toolkit 105.26.7.201` also contains the following Microsoft packages. They are not direct SASD dependencies; they are recorded here because release evidence must include transitive packages rather than treating the top-level package as the whole dependency boundary.

| Package | Resolved version | Relationship | Licence | Source |
| --- | --- | --- | --- | --- |
| `System.Resources.Extensions` | `9.0.10` | Transitive dependency of `Krypton.Toolkit` | MIT | dotnet/runtime |
| `System.Formats.Nrbf` | `9.0.10` | Transitive through `System.Resources.Extensions` | MIT | dotnet/runtime |
| `System.Reflection.Metadata` | `9.0.10` | Transitive through `System.Formats.Nrbf` | MIT | dotnet/runtime |
| `System.Collections.Immutable` | `9.0.10` | Transitive through `System.Reflection.Metadata` | MIT | dotnet/runtime |

These entries describe the dependency graph resolved by the current R1 build. A dependency update must regenerate the NuGet dependency evidence and review version/licence/source changes before release.

## Dependency review requirements

Before another third-party dependency is added, the project requires:

1. an Architecture Decision Record or an existing ADR that explicitly covers the dependency;
2. identification of the exact package and licence version;
3. review of transitive dependencies and notices;
4. security and maintenance assessment;
5. public-API leakage and exit-strategy review;
6. an update to this file and the release SBOM.

The conceptual screenshot in `artefacts/` was generated specifically for this project. Architecture diagrams are project-authored Graphviz sources and renderings.
