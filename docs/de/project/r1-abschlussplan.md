# R1.0-Abschlussplan

**Status:** Aktiver Abschlussplan  
**Basis:** Repository-`main` ab `6e72c6f` (24.09.2026)  
**Scope:** SASD UI Platform R1 unter Windows / .NET 8 / WinForms

## 1. Zweck

R1 ist an dem Punkt angekommen, an dem zusätzliche Controls die Release-Sicherheit eher verringern als erhöhen würden. Die wiederverwendbare Basis ist breit genug für die erste echte SASD-Nutzung. Die verbleibende Arbeit konzentriert sich deshalb darauf, den vorhandenen Stand nachzuweisen, zu paketieren und zu stabilisieren.

Das Ziel von R1.0 ist nicht, den vollständigen Katalog von DevExpress, Telerik oder Syncfusion nachzubauen. R1.0 ist erreicht, wenn eine reale SASD-WinForms-Anwendung eine kleine, dokumentierte und wartbare Menge von SASD-UI-Paketen verwenden kann und dadurch ein konsistentes Anwendungsfundament erhält, ohne lokale Ersatzimplementierungen pflegen zu müssen.

Bis zum Abschluss von R1.0 werden neue R1-Controls nur aufgenommen, wenn eine reale Consumer-Anwendung eine blockierende Lücke nachweist oder ein bereits freigegebener Vertrag korrigiert werden muss.

## 2. R1-Feature-Freeze

Während der Abschlussphase:

- Fehler im vorhandenen R1-Verhalten beheben;
- Lifecycle, Accessibility, Keyboard, Nullability und Fehlerbehandlung weiter härten;
- Dokumentation, Samples und Release-Evidence verbessern;
- additive kompatible API-Korrekturen zulassen, wenn reale Nutzung sie begründet;
- keine spekulativen Controls, Visual-Systeme oder neuen Runtime-Abhängigkeiten hinzufügen;
- optionale R2/R3-Adapter nicht nur zur Vergrößerung des Komponentenkatalogs vorziehen.

Bereits vorhandene native R2-Helfer dürfen Fehlerkorrekturen und Evidence-Arbeit erhalten, sollen aber nicht vom R1-Abschluss ablenken.

## 3. Abschluss-Arbeitsstränge

### A. Automatisierte Release-Evidence

- kanonisches Windows-Verifikations-Gate grün halten;
- Paketmetadaten, README, XML-Dokumentation und Symbolpaket-Prüfung reproduzierbar halten;
- deterministische SHA-256-Checksums beibehalten/verifizieren und reproduzierbare SBOM-/Third-Party-Evidence ergänzen;
- vor dem Stabilitätsversprechen eine Public-API-Inventur/Baseline etablieren;
- Gallery, Showcase und Referenz-Consumer aus einem sauberen Checkout baubar halten;
- Evidence Matrix aktuell halten.

### B. Manuelle Windows-Abnahme

- Visual-Studio-Designer öffnen/ändern/speichern/erneut öffnen;
- 100 %, 125 %, 150 % und 200 % Display-Skalierung;
- Mixed-DPI-Monitorwechsel, soweit die Hardware dies erlaubt;
- echtes Windows High Contrast;
- repräsentative Abläufe nur per Tastatur;
- Screenreader-/UI-Automation-Prüfung;
- Lokalisierungs-/Long-Label-Prüfung.

Automatisierte Accessible Names und Smoke-Tests ersetzen diese manuellen Gates nicht.

### C. Strategische Entscheidungen

Nicht stillschweigend entscheiden:

1. **öffentliche NuGet-Paketstruktur**;
2. **native WinForms vs. Krypton als sichtbarer Standard**;
3. **UI-Automation-/Screenshot-Regression-Tooling**;
4. neue spezialisierte R2-Adapter bzw. Runtime-Abhängigkeiten.

Der aktuelle Pack-Dry-Run beweist die technische Paketierbarkeit der internen Produktprojekte. Er erklärt diese Projekte nicht automatisch zu eigenständigen öffentlichen Paketen.

### D. Erste reale SASD-Nutzung

Vor R1.0 wird ein begrenztes Feature aus einer realen SASD-Anwendung migriert. Das bevorzugte erste Ziel bleibt **SASD Prompt Manager**.

Geeigneter Scope:

- Theme-/Shell-Integration;
- Formularlayout und Validierung, soweit passend;
- Search-/Filter-UI;
- Commands;
- Dialoge/Notifications;
- UI-State.

Domain-Persistenz, Business-Modelle und anwendungsspezifische Navigation bleiben im Prompt Manager.

## 4. R1.0-Exit-Kriterien

R1.0 darf erst erklärt werden, wenn:

- die vereinbarte öffentliche Paketstruktur dokumentiert und geprüft ist;
- ein sauberer Checkout die vorgesehenen Pakete, Symbole und XML-Dokumentation reproduzierbar erzeugt;
- Public-API-Baseline bzw. Kompatibilitäts-Evidence existiert;
- SBOM, Third-Party Notices, Checksums und bekannte Einschränkungen erzeugt werden;
- Designer-/DPI-/High-Contrast-/Accessibility-Matrix dokumentiert wurde;
- kritische Keyboard-/Business-Flows ausreichende Evidence besitzen;
- ein begrenztes Feature einer realen SASD-Anwendung die paketierte Plattform erfolgreich nutzt;
- Consumer-Feedback geprüft und risikoreiche API-Probleme gelöst oder dokumentiert wurden;
- das vollständige Windows-Verifikations-Gate grün ist;
- Changelog, Migration Notes und Release Notes korrekt sind.

Eine hohe Anzahl von Komponenten ist kein Exit-Kriterium.

## 5. Empfohlene Reihenfolge

1. Roadmap, Evidence Matrix und Codex Queue aktualisieren;
2. dependency-freie Evidence-Lücken und Release-Hygiene schließen;
3. Public-API-Baseline auswählen und umsetzen;
4. Designer-/DPI-/High-Contrast-/Accessibility-Matrix durchführen;
5. Paketstruktur und Native-vs.-Krypton anhand der Evidence entscheiden;
6. internen Release Candidate erzeugen;
7. begrenztes Prompt-Manager-Feature migrieren;
8. APIs anhand dieser Migration korrigieren/vereinfachen;
9. finale Release-Evidence erzeugen und R1.0 intern veröffentlichen.

## 6. Nach R1.0

R1.1 konzentriert sich auf Stabilisierung in weiteren realen SASD-Consumern, Performance-/Handle-Evidence und Migration Support.

R2 wird anschließend der Hauptort für sichtbar reichere professionelle Komponenten und opt-in Adapter wie Charts, Markdown/Diff/Code Editing, PDF/Media, WebView2 und Docking.

R3 bleibt Komponenten mit nachgewiesenem Business Case vorbehalten. Spreadsheet, Pivot/OLAP, Office-Editor, Report-Designer, komplexes Scheduling/Gantt und ähnliche Produktgrößen bleiben außerhalb des Scopes, solange keine spätere Entscheidung diese Grenze ausdrücklich ändert.
