# Operations, Support, and Maintenance Concept

## 1. Operating Model

The UI Platform is embedded as a library in SASD applications. It operates no server of its own. Operations therefore mean:

- package distribution;
- compatibility with .NET, Windows, and Visual Studio;
- diagnosis of UI, designer, DPI, and resource problems;
- dependency and security updates;
- support for consuming applications.

## 2. Support Classes

| Class | Meaning |
| --- | --- |
| S1 | Application does not start, data-loss/security risk, critical handle leak or crash |
| S2 | Central component unusable with no acceptable workaround |
| S3 | Limited function or visual/accessibility deviation |
| S4 | Enhancement, documentation, or convenience issue |

## 3. Diagnostic Information

Consumers should be able to provide:

- platform and package versions;
- Windows/runtime version;
- DPI, monitor, and theme configuration;
- affected component and workflow;
- log correlation;
- technical details without secrets;
- screenshot or minimal reproduction;
- state file only after privacy review.

## 4. Logging

The platform uses lightweight logging abstractions and does not create its own global logs without consent. Consumers configure destinations and retention. Logs include component, operation, duration, result, and correlation, but no secrets or complete user content.

## 5. Maintenance

- regular dependency and vulnerability review;
- evaluation of new .NET LTS and Windows versions;
- Visual Studio Designer smoke test after relevant IDE updates;
- annual or event-driven review of the future register;
- removal of deprecated compatibility adapters after migration.

## 6. Known-Issues Process

Known limitations are documented per release, not only in issues. Each entry contains impact, affected version, workaround, and planned correction or deliberate acceptance.

## 7. Lifecycle

- Preview: no production guarantee;
- Current: actively supported;
- Maintenance: defect and security fixes only;
- End of Support: no regular fixes; migration target documented.

Until 1.0, only a small number of parallel lines are supported.
