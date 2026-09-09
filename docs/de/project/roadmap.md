# Roadmap – SASD UI Platform

**Stand:** 2026-07-23  
**Version:** 0.1  
**Zeithorizont:** R0 bis R3; keine künstlichen Kalendertermine ohne belastbare Kapazitätsplanung

## 1. Roadmap-Prinzipien

Die Roadmap priorisiert **Nutzbarkeit vor Komponentenanzahl**. Eine Komponente gilt nicht als Fortschritt, nur weil sie implementiert wurde. Fortschritt liegt vor, wenn sie:

- in der Component Gallery vollständig demonstriert wird;
- den Designer-, DPI-, Tastatur-, Accessibility- und Lokalisierungscheck besteht;
- eine dokumentierte öffentliche API besitzt;
- in mindestens einer realen SASD-Anwendung ohne lokale Sonderkopie genutzt wird;
- ein vertretbares Wartungs- und Abhängigkeitsprofil besitzt.

Die Produktlinie wird nicht gleichzeitig in WinForms, WPF, Web und Java entwickelt. Der aktuelle Fokus bleibt WinForms.

## 2. Release-Landkarte

| Stufe | Ziel | Ergebnis | Freigabekriterium |
| --- | --- | --- | --- |
| R0.1 | Architektur- und Repositorygrundlage | Monorepo, Build, Kernverträge, ADR- und Lizenzprozess | sauberer Checkout baut reproduzierbar |
| R0.2 | UI-Technologiepilot | Krypton versus native WinForms; Tokens, Theme, Basisklassen, FormLayout | Designer-, DPI-, Fokus- und High-Contrast-Matrix bestanden |
| R0.3 | Kerninteraktionspilot | Grid, Dialog, Error, Progress, UI-State, UIA-/Screenshot-Pilot | keine Fremdtypen im Kern; Spike in Gallery ausführbar |
| R1.0 | produktives Fundament | R1-Komponenten, Gallery, CRUD-/Workbench-/Utility-Samples, NuGet | erste reale SASD-Anwendung nutzt veröffentlichte Pakete |
| R1.1 | Stabilisierung | API-Bereinigung, Performance, Migration, Dokumentation | drei reale SASD-Projekte nutzen Kernmodule |
| R2.0 | optionale Fachmodule | Charts, Markdown, Code, Diff, Bild, PDF, Barcode, WebView2, Docking | jeder Adapter besitzt Security-, Lizenz- und Lifecycle-Nachweis |
| R3 | bewiesene Zusatzmodule | Wizard, Ribbon und nur konkret benötigte Erweiterungen | eigener Business Case, ADR und Wartungsverantwortung |

## 3. R0.1 – Architektur- und Repositorygrundlage

### Ziele

- Repository `SASD-UI-Platform` anlegen.
- Solution- und Projektstruktur gemäß Architekturdokument erstellen.
- `Directory.Build.props`, `Directory.Packages.props`, `global.json` und Analyzer konfigurieren.
- erste Architekturtests für Abhängigkeitsrichtung und Public-API-Grenzen einrichten.
- ADR-Prozess, Lizenzinventar und Third-Party-Notices vorbereiten.
- Minimalpakete `Sasd.Ui.Core`, `Sasd.Ui.WinForms` und `Sasd.Ui.WinForms.Testing` bauen.

### Liefergegenstände

- reproduzierbarer Debug- und Releasebuild;
- erste interne NuGet-Pakete ohne produktiven Anspruch;
- CI mit Restore, Build, Unit- und Architekturtests;
- Component-Gallery-Skelett;
- dokumentierter lokaler Entwicklerstart.

### Stop-Kriterien

R0.1 wird nicht beendet, wenn:

- Paketabhängigkeiten zyklisch sind;
- Fremdtypen unkontrolliert in öffentlichen Kern-APIs auftauchen;
- der Build lokale Maschinenzustände, GAC oder manuell kopierte DLLs benötigt;
- Lizenzstatus einer Kernabhängigkeit ungeklärt ist.

## 4. R0.2 – UI-Technologiepilot

### Kandidaten

1. native WinForms-Controls;
2. Krypton Standard Toolkit als sichtbare Hauptimplementierung;
3. AntdUI/ReaLTaiizor ausschließlich als getrennte Vergleichsprototypen, nicht als Mischoberfläche.

### Pilotumfang

- Design Tokens für Farbe, Typografie, Abstände, Radien und Statussemantik;
- `SasdThemeService` mit Light, Dark und High Contrast;
- `SasdForm`, `SasdDialogForm`, `SasdUserControl`;
- `SasdSectionPanel` und `SasdFieldLayout`;
- Iconservice und semantische Standardicons;
- Themewechsel zur Laufzeit;
- Designer-Serialisierung und Wiederöffnen im Visual Studio Designer.

### Entscheidungsausgang

Krypton wird Standard, wenn es:

- im Designer reproduzierbar funktioniert;
- DPI- und Fokusverhalten nicht verschlechtert;
- High Contrast und Tastaturbedienung ausreichend unterstützt;
- keine untragbaren Abhängigkeiten oder API-Leaks erzeugt.

Andernfalls wird die native WinForms-Implementierung Standard. Die Design Tokens und Verträge bleiben unabhängig vom Ergebnis erhalten.

## 5. R0.3 – Grid-, Dialog-, State- und Testpilot

### Pilotkomponenten

- `SasdDataGrid` plus `SasdGridController<T>`;
- `SasdSearchBox`, `SasdFilterBar`, `SasdEmptyState`, `SasdBusyOverlay`;
- `SasdDialogService`, `SasdErrorDialog`, `SasdProgressDialog`;
- `SasdStateStore` mit Versionierung, Backup, Migration und Reset;
- UI-Automation über UIA3/FlaUI als Pilot;
- Screenshot-Regression für definierte Gallery-Zustände.

### Nachweise

- Sortierung, Suche, Filter, Auswahl und CSV-Export im Grid-Spike;
- zustandsbehaftete Gridspalten ohne Datenbankkopplung;
- beschädigte State-Datei verhindert den Start nicht;
- Cancel/Progress und Fehlerabschluss funktionieren nachvollziehbar;
- mindestens ein Tastatur-Only-End-to-End-Szenario ist automatisiert.

## 6. R1.0 – produktives Fundament

### Verbindliche Produktbereiche

- Basisklassen und Layout;
- Themes und Icons;
- Commands, Shell, Navigation, Breadcrumbs und Dokumenttabs;
- Grid, Listen, Bäume, Suche, Filter und Pagingverträge;
- Dialoge, Notifications, Fehler und Fortschritt;
- Datei-, Clipboard-, Drag-and-drop-, Tray- und Shellintegration;
- UI-State, Recent Items und sichere Defaults;
- Component Gallery und drei Referenzanwendungen;
- NuGet, Symbole, XML-Dokumentation, SBOM und Third-Party-Notices.

### Pilotmigration

Als erste Übernahme wird eine kleine, klar abgegrenzte Anwendung oder Teilfunktion verwendet. Der Prompt Manager eignet sich für Theme, FormLayout, Dialoge, Search/Filter und Grid-State. Eine vollständige Neugestaltung der Navigation ist kein R1-Zwang.

## 7. R1.1 – Stabilisierung

- API-Review mit Fokus auf Benennung, Nullability, Fehlermodelle und Abbruchbarkeit;
- Performance- und Handle-Leak-Tests;
- Migrationshinweise für alle Breaking Changes;
- echte Verwendung in Prompt Manager, Mail Workbench und einer Utility-Anwendung;
- Reduktion direkter Paketabhängigkeiten für Consumer;
- Dokumentation der bekannten Grenzen statt versteckter Sonderfälle.

## 8. R2.0 – Fachmodule

R2-Module sind getrennte Pakete und werden nicht automatisch installiert.

| Modul | Pilot | Aufnahmebedingung |
| --- | --- | --- |
| Charts | ScottPlot | konkrete Dashboard-/Monitoringanforderung |
| KPI/Sparkline | eigener Wrapper auf Chartbasis | textuelle Accessibility-Alternative vorhanden |
| Markdown/Diff | Markdig/DiffPlex | Notes/Mail-Workbench-Szenario belegt |
| Codeeditor | ScintillaNET | Lebenszyklus und große Dateien getestet |
| Bild/QR/Barcode | spezialisierte Adapter | Lizenz und Exportformate geprüft |
| PDF | PDFsharp/MigraDoc | programmatische Ausgabe genügt; kein Designerbedarf |
| WebView2 | Microsoft WebView2 | Default-Deny, Allowlist und Downloadregeln getestet |
| Docking | Krypton Docking/Workspace | Layoutreset und Ressourcenfreigabe sicher |

## 9. R3 und bewusste Nichtziele

Wizard und Ribbon werden nur entwickelt, wenn mindestens zwei reale Anwendungen denselben Bedarf zeigen oder ein einzelnes strategisches Produkt einen belastbaren Business Case besitzt.

Nicht Teil dieser Roadmap sind Pivot/OLAP, Spreadsheet, Office-Editoren, Report-/Dashboard-Designer, Gantt, komplexes Scheduling, 3D, mobile Controls oder eine plattformübergreifende Neuentwicklung.

## 10. Roadmap-Pflege

Die Roadmap wird bei jedem Gate-Review aktualisiert. Neue Komponenten dürfen nicht direkt in R1/R2 aufgenommen werden. Erforderlich sind:

1. belegtes Anwendungsszenario;
2. Scope- und Wartungsbewertung;
3. Build-vs-Buy-vs-Adapter-Entscheidung;
4. Lizenz- und Sicherheitsprüfung;
5. ADR bei architekturrelevanter Wirkung;
6. Einordnung in Release, Gallery und Testmatrix.
