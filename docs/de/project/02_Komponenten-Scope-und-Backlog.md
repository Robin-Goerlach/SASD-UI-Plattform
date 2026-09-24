# Komponenten-Scope und priorisierter Backlog

## 1. Auswahlregel

Eine Komponente wird aufgenommen, wenn sie in mehreren aktuellen oder erwartbaren SASD-WinForms-Anwendungen nutzbar ist. Exotische oder extrem teure Komponenten werden bewusst nicht vorweggenommen.

## 2. R1 – verbindlicher Kern

### Basis und Layout

- `SasdForm`, `SasdDialogForm`, `SasdUserControl`;
- `SasdSectionPanel`, `SasdFieldLayout`, `SasdValidationSummary`;
- `SasdSearchBox`, `SasdFilterBar`, `SasdEmptyState`, `SasdBusyOverlay`;
- `SasdUiDispatcher`.

### Shell, Navigation und Commands

- `SasdShellForm`, `SasdNavigationView`, `SasdBreadcrumb`;
- `SasdDocumentTabs`, `SasdCommandBar`, `SasdCommandManager`;
- `SasdStatusService`/`SasdStatusBar`.

### Datenansichten

- `SasdDataGrid` und `SasdGridController<T>`;
- `SasdListView`, `SasdTreeView`;
- Paging-, Sortier- und Filterverträge unabhängig von Datenbank/ORM;
- CSV-Export, Spaltenzustand, Auswahl und Basiseditierung.

### Dialoge und Rückmeldungen

- `SasdDialogService`, `SasdErrorDialog`;
- `SasdNotificationService`, `SasdProgressDialog`.

### Windows- und Dateiintegration

- `SasdFileDialogService`, `SasdClipboardService`, `SasdDragDropService`;
- `SasdTrayService`, sichere externe Shell-/Dateiaktionen.

### Zustand, Theme und Icons

- `SasdStateStore`, `SasdRecentItemsService`;
- `SasdThemeService`, `SasdIconService`.

## 3. R2 – optionale Adapter

- technische Charts und KPI-Anzeigen;
- Markdowneditor und sichere Vorschau;
- Code-/Konfigurationseditor;
- Diff Viewer;
- Bildanzeige;
- QR-/Barcode-Service;
- programmgesteuerte PDF-Ausgabe;
- gehärteter WebView2-Host;
- Docking/Workspace;
- Property Editor und erweiterte Gridfunktionen.

Diese Komponenten werden nur als getrennte Pakete veröffentlicht. Eine normale CRUD-Anwendung darf sie nicht transitiv erhalten.

## 4. R3 – nur bei bewiesenem Bedarf

- Wizard;
- Ribbon;
- zusätzliche erweiterte Ansichten mit mindestens zwei realen Consumer-Szenarien oder strategischem Einzelbedarf.

## 5. Bewusst ausgeschlossen

- Pivot/OLAP;
- Spreadsheet-Engine;
- Word-/Office-Editor;
- visueller Report- oder Dashboard-Designer;
- komplexer Scheduler und Gantt;
- 3D-/CAD-/GIS-Spezialcontrols;
- mobile Controls;
- plattformübergreifender Renderingkern;
- universelle Low-Code-Plattform.

## 6. Backlog-Priorisierung

Jeder Backlogeintrag erhält:

- **Wiederverwendungswert:** Anzahl realistischer Consumer;
- **Risikoreduktion:** beseitigt er lokale, fehleranfällige Sonderlogik?
- **Implementierungsaufwand:** inklusive Tests, Gallery und Dokumentation;
- **Wartungskosten:** Abhängigkeiten, Designer, native Ressourcen;
- **Abhängigkeit:** blockiert er andere Kernfunktionen?

### Prioritätsformel

Eine einfache Entscheidungshilfe ist:

`Priorität = (Wiederverwendungswert + Risikoreduktion + Blockerwert) - (Aufwand + Wartungsrisiko)`

Sie ersetzt kein fachliches Urteil, zwingt aber zur transparenten Begründung.

## 7. Komponentenproposal

Vor Aufnahme werden folgende Fragen beantwortet:

1. Welche konkrete Anwendungssituation wird gelöst?
2. Welche vorhandene Komponente wurde geprüft?
3. Warum genügt eine lokale Helper-Klasse nicht?
4. Welcher kleinste sinnvolle Funktionsumfang wird geliefert?
5. Welche Zustände müssen in der Gallery sichtbar sein?
6. Welche Daten, Dateien oder Prozesse überschreiten Trust Boundaries?
7. Welche Exit-Strategie besteht bei Fremdbibliotheken?
8. Welches Release und welche Abnahmekriterien gelten?
