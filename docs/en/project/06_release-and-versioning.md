# Release and Versioning Concept

## 1. Version Model

The platform uses semantic versioning.

- `0.x`: the API is under construction and stabilization.
- `1.0`: the R1 core is documented, proven in production, and governed for compatibility.
- Patch: compatible defect corrections.
- Minor: compatible new functionality.
- Major: breaking changes.

Previews may use suffixes such as `-alpha.N`, `-beta.N`, or `-rc.N`.

## 2. Package Versions

R1 packages generally share one platform version to reduce consumer and support complexity. Adapters may use independent versions only when they genuinely require separate release cycles; this requires an ADR.

## 3. Release Contents

- NuGet packages and symbols;
- XML documentation;
- changelog/release notes;
- migration notes;
- SBOM and third-party notices;
- known limitations;
- checksums;
- a Component Gallery/sample version matching the release.

## 4. Compatibility Rules

Breaking changes include:

- removing or renaming public types or members;
- changing default behavior with high consumer risk;
- introducing a new mandatory dependency;
- changing persisted state schemas without migration;
- changing `AutomationId` values without migration and test notes.

Not every visual correction is breaking. It is nevertheless documented in the changelog when screenshots, layouts, or user workflows change materially.

## 5. Release Process

1. Freeze scope.
2. Review open critical defects and security findings.
3. Run the complete test matrix.
4. Review the public API diff.
5. Update changelog and migration notes.
6. Review SBOM and licenses.
7. Test the release candidate in a real SASD application.
8. Create a tag and package from a clean checkout.
9. Publish internally.
10. Perform a smoke test by installing into an empty consumer solution.

## 6. Support Lines

Until 1.0, support primarily targets the current 0.x line. After 1.0, the project decides whether an LTS line is economically justified. Security-critical defects may affect older versions still in use; the support scope is documented per release.

## 7. Deprecation

Deprecated APIs are first marked, documented, and supplied with a replacement. Removal occurs no earlier than a major version unless an urgent security or licensing issue requires faster action.
