# Build- und CI/CD-Konzept

## 1. Ziele

- reproduzierbarer Build aus sauberem Checkout;
- zentrale Paket- und Compilerkonfiguration;
- frühe Erkennung von Architektur-, Lizenz- und Sicherheitsproblemen;
- nachvollziehbare NuGet- und Releaseartefakte;
- Windows-spezifische UI-Tests auf kontrollierten Runnern.

## 2. Repository-Grunddateien

- `global.json`: SDK-Familie und Roll-forward;
- `Directory.Build.props`: Nullable, XML-Doku, Analyzer, deterministische Builds;
- `Directory.Packages.props`: zentrale NuGet-Versionen;
- Lockfiles beziehungsweise zentral definierte Restorestrategie;
- `NuGet.config`: freigegebene Quellen und Mapping;
- Buildskripte unter `build/` oder `eng/`.

## 3. Pipeline

### Validate

- Format-/Stylecheck;
- Restore mit Lockprüfung;
- Architekturtests;
- Lizenz- und Vulnerability-Scan;
- Prüfung verbotener Paketquellen;
- Markdown-Link-/Strukturprüfung.

### Build

- Debug und Release;
- `net8.0-windows`;
- Warnungen als Fehler für eigene Projekte, mit bewusst dokumentierten Ausnahmen;
- deterministische Assembly- und Paketmetadaten.

### Test

- Unit und Architekturtests auf jedem PR;
- Integrations- und Componenttests auf jedem PR;
- ausgewählte UIA-Tests auf Windows-Runner;
- vollständige Visual-/DPI-Matrix mindestens vor Release beziehungsweise regelmäßig nachts.

### Pack

- NuGet-Pakete;
- Symbolpakete;
- XML-Dokumentation;
- SBOM;
- `THIRD-PARTY-NOTICES`;
- Release-Notes und Prüfsummen.

### Publish

- nur aus Tag oder manuell freigegebenem Releaseworkflow;
- zunächst interner/private Feed;
- keine Veröffentlichung aus Entwicklerarbeitsverzeichnis;
- unveränderliche Versionen: bereits veröffentlichte Paketversionen werden nicht überschrieben.

## 4. Runnertrennung

- Logik- und Buildjobs können auf standardisierten Windows-Runnern laufen.
- Designer-, DPI-, Screenshot- und UIA-Tests benötigen kontrollierte Windows-VM/Runnerkonfiguration.
- Baseline-Screenshots werden nur auf definierter Umgebung aktualisiert.

## 5. Artefaktaufbewahrung

- PR-Artefakte kurzzeitig für Diagnose;
- Releaseartefakte dauerhaft;
- Testreports und Screenshots für fehlgeschlagene UI-/Visualtests;
- SBOM, Notices und Prüfsummen pro Release.

## 6. Lokaler Build

Ein lokaler Standardbefehl soll Restore, Build und schnelle Tests ausführen. Ein zweiter, langsamer Befehl führt Integrations-, UI- und Visualtests aus. Die konkreten Skriptnamen werden mit R0.1 festgelegt und in README/CI identisch verwendet.

## 7. Pipeline-Ausnahmen

Eine temporäre Ausnahme benötigt:

- dokumentierte Ursache;
- Besitzer;
- Ablaufdatum;
- Issue-Link;
- keine Umgehung von Lizenz- oder kritischen Security-Gates.
