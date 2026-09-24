# Projektplan und Meilensteine

## 1. Planungsansatz

Der Plan arbeitet mit **Ergebnissen und Gates**, nicht mit frei erfundenen Fertigstellungsterminen. Erst nach R0.1 kann aus tatsächlicher Durchlaufzeit eine belastbare Zeitplanung abgeleitet werden.

## 2. Arbeitspakete R0.1

| ID | Arbeitspaket | Ergebnis | Abhängigkeit |
| --- | --- | --- | --- |
| WP-R01-01 | Repository und Solution | standardisierte Monorepo-Struktur | keine |
| WP-R01-02 | Buildgrundlage | props, packages, SDK, Analyzer, Lock | WP-R01-01 |
| WP-R01-03 | Kernverträge | Result, Fehler, Tokens, Versionierung | WP-R01-02 |
| WP-R01-04 | Testgrundlage | xUnit, Architekturtests, Testhost | WP-R01-02 |
| WP-R01-05 | Gallery-Skelett | navigierbare Demo mit Basisseiten | WP-R01-03 |
| WP-R01-06 | Governance | ADR, Lizenzregister, Changelog, PR-Check | WP-R01-01 |
| WP-R01-07 | CI | Validate, Build, Test, Pack-Probe | WP-R01-02/04 |

## 3. Arbeitspakete R0.2

- Design-Token-Modell;
- native WinForms-Theme-Referenz;
- Krypton-Adapter und Palette;
- `SasdForm`, `SasdDialogForm`, `SasdUserControl`;
- FormLayout und Validation-Grundlage;
- Iconservice;
- Designer-, DPI-, Fokus- und High-Contrast-Testmatrix;
- ADR-004-Abschlussentscheidung.

## 4. Arbeitspakete R0.3

- SearchBox, FilterBar, EmptyState, BusyOverlay;
- Grid/Controller-Spike;
- Dialog-, Error- und Progress-Services;
- StateStore mit Migration und Recovery;
- UIA-Pilot;
- visuelle Regression;
- erste End-to-End-Gallery-Flows.

## 5. R1-Epics

| Epic | Kerninhalt | Erste Consumer-Anwendung |
| --- | --- | --- |
| EP-R1-BASE | Basisklassen, Layout, Dispatcher | TaskHost/Utility |
| EP-R1-THEME | Themes, Icons, High Contrast | Prompt Manager |
| EP-R1-CMD | Commands und Shortcuts | Prompt Manager |
| EP-R1-SHELL | Shell, Navigation, Tabs, Status | Mail Workbench/Notes |
| EP-R1-DATA | Grid, Liste, Baum, Suche, Filter, Paging | Prompt Manager |
| EP-R1-FEEDBACK | Dialoge, Error, Notification, Progress | alle |
| EP-R1-WINDOWS | Datei, Clipboard, DragDrop, Tray, Shell | Utility/Mail Workbench |
| EP-R1-STATE | UI-State, MRU, Migration | alle |
| EP-R1-DOCS | Gallery, Samples, API-Doku | alle |
| EP-R1-REL | NuGet, SBOM, Releaseprozess | alle |

## 6. Gate-Checklisten

### Gate R0.1

- [ ] sauberer Checkout baut ohne manuelle Vorbereitungen;
- [ ] Architekturtests erkennen verbotene Abhängigkeit;
- [ ] Paketrestore ist gesperrt/reproduzierbar;
- [ ] erste Pakete lassen sich lokal installieren;
- [ ] Lizenzinventar vorhanden;
- [ ] ADR-Register und Vorlage funktionieren.

### Gate R0.2

- [ ] Designer öffnet/speichert alle Pilotcontrols;
- [ ] Light/Dark/High Contrast funktionieren;
- [ ] 100/125/150/200 Prozent DPI geprüft;
- [ ] Tastaturfokus sichtbar;
- [ ] keine kritischen Krypton-API-Leaks im Kern;
- [ ] ADR-004 angenommen oder verworfen.

### Gate R0.3

- [ ] Grid- und Dialogspike ausführbar;
- [ ] ungültiger State startet mit sicheren Defaults;
- [ ] UIA-End-to-End-Fluss stabil;
- [ ] Screenshotbaseline dokumentiert;
- [ ] Ressourcenzyklentest ohne fortlaufendes Wachstum.

### Gate R1.0

- [ ] R1-MUSS-Scope implementiert oder Abweichung akzeptiert;
- [ ] Gallery deckt alle öffentlichen visuellen Komponenten ab;
- [ ] drei Samples bauen aus veröffentlichten Paketen;
- [ ] erste reale SASD-Anwendung nutzt Pakete;
- [ ] NuGet, Symbole, XML-Doku, SBOM und Notices veröffentlicht;
- [ ] Security-, Lizenz- und Migrationsreview bestanden.

## 7. Abhängigkeiten und kritischer Pfad

Der kritische Pfad verläuft über Build/Repository → Core-Verträge → Theme/Designer → Grid/Dialog/State → Gallery/Samples → reale Migration. R2-Module dürfen den kritischen Pfad nicht blockieren.

## 8. Kapazitätsregel

Maximal ein großer UI-Baustein und ein Querschnittsthema gleichzeitig. Beispielsweise Grid plus Testharness, nicht Grid, Shell, Docking und WebView2 parallel. Dadurch bleiben Fehlerursachen und Architekturentscheidungen nachvollziehbar.
