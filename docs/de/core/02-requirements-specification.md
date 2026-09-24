# SASD UI Platform – Lastenheft für C#-WinForms-Komponenten

**Anforderungsdefinition für eine wiederverwendbare Windows-Desktop-Komponentenbibliothek**

- **Version:** 0.1
- **Stand:** 23. Juli 2026
- **Status:** Entwurf zur fachlichen Prüfung
- **Auftraggeber/Bedarfsträger:** SASD-GmbH
- **Produktlinie:** Windows Desktop / C# / WinForms
- **Grundlage:** `SASD_UI-Komponenten-Katalog_v2_WinForms-Fokus_2026-07-23.md`

> Dieses Lastenheft beschreibt das **Was und Warum** der gewünschten WinForms-Komponentenplattform. Konkrete Klassenstrukturen, Paketnamen, interne Architektur und Implementierungsdetails werden anschließend im Pflichtenheft festgelegt. Technische Kandidaten werden hier nur genannt, soweit sie eine wichtige Randbedingung oder Bewertungsgrundlage darstellen.

## 1. Management-Zusammenfassung

SASD soll zunächst eine klar begrenzte, wiederverwendbare Komponentenplattform für C#-WinForms-Anwendungen entwickeln. Das Ziel ist keine vollständige Konkurrenz zu DevExpress, Telerik oder Syncfusion, sondern ein zuverlässiger Kern für die tatsächlich wiederkehrenden SASD-Anwendungstypen: Verwaltungsprogramme, technische Workbenches, Editoren, kleine Systemwerkzeuge und Monitoring-Oberflächen.

Die erste Ausbaustufe konzentriert sich auf Basiskomponenten, Themes, Formulare, Navigation, DataGrid, Dialoge, Datei-/Windows-Integration, Zustandsverwaltung, Accessibility, High DPI, Dokumentation und Referenzanwendungen. Spezialisierte Komponenten wie Charts, Markdown-/Codeeditor, WebView2, Bildanzeige und PDF-Ausgabe folgen in einer zweiten Stufe. Pivot/OLAP, Spreadsheet, Office-Editoren, Report-/Dashboard-Designer, komplexes Scheduling, Gantt, 3D und mobile beziehungsweise plattformübergreifende Oberflächen werden bewusst zurückgestellt.

## 2. Ausgangssituation

SASD entwickelt mehrere Windows-Desktopanwendungen mit WinForms. Wiederkehrende Anforderungen wie konsistente Formulare, Dialoge, Navigation, Datentabellen, Themes, Fehlermeldungen, Einstellungen und Dateioperationen werden bisher projektbezogen gelöst. Der UI-Komponenten-Katalog hat eine große Zahl kommerzieller und offener Anbieter erfasst. Für die praktische Umsetzung muss diese Breite nun auf eine wartbare Produktlinie reduziert werden.

Die Komponentenplattform soll vorhandene Open-Source-Bibliotheken kuratieren und kapseln, statt deren Quellcode pauschal zusammenzuführen. Microsoft WinForms bildet den Plattformkern; Krypton ist der bevorzugte erste Pilot für die visuelle Basis. Weitere Bibliotheken werden als spezialisierte Adapter oder Vergleichskandidaten betrachtet.

## 3. Zielgruppen und typische Nutzungsszenarien

| Zielgruppe | Bedarf | Typische Anwendungen |
| --- | --- | --- |
| SASD-Entwickler | Schnelle, konsistente Erstellung und Wartung von Oberflächen | Alle SASD-WinForms-Projekte |
| Administratoren und technische Anwender | Klare Datenansichten, sichere Dateioperationen, Logs, Einstellungen und Status | Mail Workbench, Notes, Secret Manager, Systemtools |
| Fachanwender | Verständliche Formulare, Suche, Filter, Validierung und Export | Prompt Manager, Training Control, Aufgaben-/Verwaltungsprogramme |
| Wartung und Support | Reproduzierbare Fehleranzeige, Diagnoseinformationen, konsistente Versionen | Alle produktiven Anwendungen |

Typische Referenzszenarien sind: CRUD-/Verwaltungsanwendung, dokumentenorientierte Workbench, technischer Editor, kleines Tray-/Systemwerkzeug und Monitoring-/Dashboard-Anwendung.

## 4. Geltungsbereich und bewusste Abgrenzung

### 4.1 Bestandteil dieses Lastenhefts

- Windows-Desktopkomponenten für C# und WinForms
- wiederverwendbare Komponenten, Services, Vorlagen und Qualitätsstandards
- Integration ausgewählter Open-Source-Komponenten über stabile SASD-Verträge
- NuGet-Pakete, Component Gallery, Referenzanwendungen, Tests und Dokumentation
- Kompatibilität mit bestehenden .NET-8-WinForms-Projekten

### 4.2 Nicht Bestandteil

- WPF, WinUI 3, Avalonia oder andere alternative Desktopimplementierungen
- ASP.NET Core/Blazor, ASP.NET Web Forms/ASPX oder Java-Weboberflächen
- Android, iOS, macOS oder Linux-Desktop
- vollständige DevExpress-/Telerik-/Syncfusion-Nachbildung
- Pivot/OLAP, Spreadsheet, Office-/DOCX-Editor, Report-/Dashboard-/Diagram-Designer, PDF-Editor und 3D-Visualisierung
- komplexer Resource Scheduler, vollständiges Projektmanagement-Gantt oder BPMN-Workflow-Designer

## 5. Prioritäten und Releases

| Priorität | Bedeutung |
| --- | --- |
| **MUSS** | Für die erste produktiv nutzbare Version zwingend erforderlich. |
| **SOLL** | Hoher Nutzwert; Aufnahme nach dem Fundament, sofern kein schwerwiegendes Risiko entsteht. |
| **KANN** | Nur bei belegtem Bedarf oder wenn die Umsetzung mit geringem Zusatzaufwand möglich ist. |
| **ZURÜCKGESTELLT** | Bewusst nicht Bestandteil des aktuellen WinForms-Projekts; im Zukunftsregister gesichert. |

| Ausbaustufe | Ziel | Inhalt |
| --- | --- | --- |
| **R0 – Entscheidungspilot** | Technologie- und Lizenzentscheidung | WinForms/Krypton-Pilot, Design Tokens, BaseForm, Dialoge, DataGrid-Spike, DPI/Accessibility-Prüfung. |
| **R1 – Nutzbares Fundament** | Erste produktive Nutzung | F01–F11, F19–F20; CRUD-, Workbench- und Utility-Referenz; NuGet, Gallery, Dokumentation. |
| **R2 – Fachliche Erweiterungen** | Werkzeuge und Dashboards | Charts, technische Editoren, WebView2, Bild/PDF/QR, Docking, erweiterte Datenfunktionen. |
| **R3 – Optionale Module** | Nur bei bewiesenem Bedarf | Ribbon, Wizard, weitere Spezialansichten; Vorbereitung WPF-Lastenheft. |

## 6. Ausgewählte Komponentenfamilien

| ID | Komponentenfamilie | Umfang | Priorität | Release | Voraussichtliche Basis |
| --- | --- | --- | :---: | :---: | --- |
| F01 | **Basiskomponenten und Formulare** | Buttons, Labels, Text-, Zahlen-, Datums- und Auswahlfelder, Gruppen, Panels, Standardzustände | MUSS | R1 | Microsoft WinForms; bevorzugt über SASD-Fassade, visuell ggf. Krypton |
| F02 | **Design Tokens und Themes** | Farben, Typografie, Abstände, Radien, Icons, Light/Dark, High Contrast | MUSS | R1 | SASD-eigene Tokens; Krypton-Paletten als erste Umsetzung |
| F03 | **Application Shell** | Hauptfenster, Navigation, Arbeitsbereich, Status- und Befehlsflächen | MUSS | R1 | SASD Shell auf WinForms/Krypton |
| F04 | **Menüs und Befehle** | Hauptmenü, Kontextmenü, Toolbar/CommandBar, Shortcuts und Aktivierungszustände | MUSS | R1 | WinForms/Krypton plus SASD Command System |
| F05 | **Navigation und Layout** | Sidebar, Navigation View, Tabs, Splitter, Accordion, TreeView, Breadcrumb, StatusBar | MUSS | R1 | WinForms/Krypton |
| F06 | **Formularlayout und Validierung** | Einheitliche Feldanordnung, Pflichtfelder, Inlinefehler und Validierungsübersicht | MUSS | R1 | SASD-eigene Layout-/Validation-Schicht |
| F07 | **DataGrid und Datentabellen** | Anzeige, Bearbeitung, Sortierung, Filterung, Suche, Auswahl, Spaltenzustand und Virtualisierung | MUSS | R1/R2 | DataGridView/Krypton DataGridView; Erweiterungen selektiv |
| F08 | **Listen und Bäume** | ListView/ObjectList, TreeView, Folder Tree sowie große/virtuelle Listen | MUSS | R1/R2 | WinForms/Krypton; ggf. ObjectListView nur nach Pilot |
| F09 | **Dialoge und Feedback** | Nachrichten, modale Dialoge, Toasts, Tooltips, Fortschritt, Busy Overlay und Fehlerdetails | MUSS | R1 | SASD Dialog-/Notification-Services; Ookii für Systemdialoge |
| F10 | **Datei- und Systemintegration** | Datei-/Ordnerwahl, Drag-and-drop, Zwischenablage, Recent Files, Tray und sichere Shell-Integration | MUSS | R1 | Windows APIs; Ookii.Dialogs; SASD Services |
| F11 | **Zustand und Benutzereinstellungen** | Fenster, Splitter, Spalten, Filter, Theme und MRU persistent speichern | MUSS | R1 | SASD State Persistence |
| F12 | **Diagramme und KPI-Anzeigen** | Linien-, Balken-, Punkt-, Kreis-, Gauge-, Sparkline- und einfache Heatmap-Darstellung | SOLL | R2 | ScottPlot primär; LiveCharts2 für Dashboard-Fälle |
| F13 | **Editoren für technische Inhalte** | Markdown-, Code- und Diff-Ansicht mit Suche, Syntax und großen Dateien | SOLL | R2 | ScintillaNET; Markdig; DiffPlex; ggf. WebView2 für Vorschau |
| F14 | **Bild-, Barcode- und PDF-Ausgabe** | Bildanzeige, QR/Barcode, programmgesteuerte PDF-Erzeugung und Export | SOLL | R2 | Cyotek ImageBox; QRCoder/ZXing; PDFsharp/MigraDoc oder geprüfte Alternative |
| F15 | **WebView-Host** | Kontrollierte Einbettung lokaler oder vertrauenswürdiger Webinhalte | SOLL | R2 | Microsoft WebView2 |
| F16 | **Docking und Workspace** | Dokumentfenster, Werkzeugfenster, Layoutspeicherung und Wiederherstellung | SOLL | R2 | Krypton Docking/Workspace nach Pilot |
| F17 | **Ribbon und Assistenten** | Ribbon für komplexe Anwendungen; Stepper/Wizard für geführte Abläufe | KANN | R3 | Krypton Ribbon; SASD Wizard Contract |
| F18 | **PropertyGrid und erweiterte Datenansichten** | Eigenschaftsbearbeitung, Master-Detail, Summen, Column Chooser und Exporte | SOLL | R2/R3 | Krypton/Standard; Adapter |
| F19 | **Komponenten-Galerie und Vorlagen** | Demo aller Zustände sowie Starter für CRUD, Workbench, Dashboard und Utility | MUSS | R1/R2 | SASD-eigene Referenzanwendungen |
| F20 | **Qualität, Tests und Dokumentation** | DPI, Accessibility, Tastatur, Lokalisierung, visuelle Regression, API-Dokumentation und Beispiele | MUSS | R1+ | SASD Test-/Dokumentationsstandard |

## 7. Funktionale und nichtfunktionale Anforderungen

### 7.1 Ziele, Nutzen und Produktabgrenzung

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-ZIE-001 | **MUSS** | Die SASD UI Platform muss eine wiederverwendbare Komponentenbasis für neue und bestehende C#-WinForms-Anwendungen von SASD bereitstellen. Sie soll wiederkehrende UI-Aufgaben vereinheitlichen und die Entwicklung kleiner bis mittlerer Verwaltungs-, Werkzeug-, Editor- und Monitoring-Anwendungen beschleunigen. | Mindestens drei unterschiedliche SASD-Referenzanwendungen können dieselben Pakete und Komponenten ohne projektspezifische Kopien einsetzen. |
| LH-ZIE-002 | **MUSS** | Die erste Produktlinie ist ausschließlich für Windows-Desktopanwendungen auf Basis von WinForms vorgesehen. WPF, Web/ASPX, Java, Android, iOS, macOS und Linux-Desktop sind keine Liefergegenstände dieses Lastenhefts. | Build, Beispiele und Dokumentation enthalten nur die definierte WinForms-Produktlinie; andere Plattformen werden ausschließlich im Zukunftsregister geführt. |
| LH-ZIE-003 | **MUSS** | Die Plattform soll keine vollständige Nachbildung von DevExpress, Telerik oder Syncfusion anstreben. Sie muss einen pragmatischen, gut gepflegten Kern für häufige SASD-Anwendungsfälle schaffen. | Der freigegebene Komponentenplan enthält keine unbewiesenen Großprojekte wie Spreadsheet-, Pivot-/OLAP-, Office- oder Report-Designer. |
| LH-ZIE-004 | **MUSS** | Anwendungen sollen möglichst gegen SASD-eigene Verträge, Services und Basiskomponenten programmiert werden, damit einzelne Fremdbibliotheken austauschbar bleiben. | Öffentliche SASD-APIs geben keine unnötigen herstellerspezifischen Typen zurück; dokumentierte Ausnahmen werden begründet. |
| LH-ZIE-005 | **SOLL** | Die Plattform soll die vorhandenen SASD-Anwendungen Prompt Manager, Mail Workbench, Notes, TaskHost, Desktop Secret Manager, Training Control und weitere Werkzeuge als reale Bedarfsträger berücksichtigen. | Für jede R1-/R2-Komponentenfamilie ist mindestens ein konkreter Anwendungsfall aus einem SASD-Projekt dokumentiert. |

### 7.2 Technischer und organisatorischer Rahmen

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-RHM-001 | **MUSS** | Die Komponenten müssen mindestens mit bestehenden .NET-8-Windows-Projekten einsetzbar sein. Eine parallele Prüfung mit einer aktuellen .NET-LTS-Version ist vorzusehen, ohne die erste Lieferung durch unnötiges Multi-Targeting zu verzögern. | Ein Beispielprojekt unter .NET 8 baut und startet reproduzierbar; die unterstützten Ziel-Frameworks sind in der Paketdokumentation eindeutig genannt. |
| LH-RHM-002 | **MUSS** | Die Entwicklung und Design-Time-Nutzung muss mit Visual Studio und dem WinForms-Designer möglich sein. Designerprobleme dürfen nicht durch manuelle Änderungen an generiertem Designer-Code kompensiert werden. | Alle visuellen Kernkomponenten lassen sich im Designer platzieren, konfigurieren, speichern, schließen und erneut öffnen, ohne Designer-Fehler oder verlorene Einstellungen. |
| LH-RHM-003 | **MUSS** | Die Produktlinie muss als versionierte NuGet-Pakete, Quellrepository, Beispielanwendungen und Dokumentation bereitstellbar sein. | Eine saubere Neuinstallation der Pakete in einer leeren Referenzlösung ist anhand einer dokumentierten Anleitung möglich. |
| LH-RHM-004 | **MUSS** | Permissive Open-Source-Lizenzen sind zu bevorzugen. GPL, unklare Community-Lizenzen, Source-available-Modelle oder kommerzielle Laufzeitbindungen dürfen nur nach expliziter Freigabe verwendet werden. | Für jede direkte und transitive Abhängigkeit existieren Lizenzangabe, Herkunft, Version und Verwendungsentscheidung; THIRD-PARTY-NOTICES und SBOM sind erzeugbar. |
| LH-RHM-005 | **MUSS** | Ein Megafork mehrerer Komponentenbibliotheken ist ausgeschlossen. Die Standardreihenfolge lautet: normale Abhängigkeit, SASD-Adapter, Upstream-Beitrag, temporärer Patch-Fork, dauerhafter Fork nur als letzte Option. | Jeder Fork besitzt einen dokumentierten Grund, Upstream-Abgleich, Wartungsverantwortung und Exit-Plan. |
| LH-RHM-006 | **SOLL** | Kommerzielle Suiten dürfen als Funktionsbenchmark und bei nachgewiesenem Business Case als austauschbare Spezialimplementierung berücksichtigt werden. | Eine kommerzielle Komponente kann entfernt oder ersetzt werden, ohne den gesamten SASD-Kern neu zu entwerfen. |

### 7.3 Grundsystem, Design und Basiskomponenten

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-GRU-001 | **MUSS** | Es muss ein konsistentes SASD-Designsystem mit benannten Design Tokens für Farben, Typografie, Abstände, Größen, Radien, Linien, Fokus und Statusfarben geben. | Alle R1-Komponenten beziehen ihre visuellen Grundwerte aus einer zentralen, dokumentierten Tokenquelle; keine willkürlichen Farb- und Abstandsduplikate in Anwendungen. |
| LH-GRU-002 | **MUSS** | Light Theme, Dark Theme und Windows-High-Contrast müssen unterstützt werden. Der Wechsel zwischen Light und Dark soll zur Laufzeit möglich und persistent sein. | Die Component Gallery zeigt alle Kernkomponenten in den drei Betriebsarten; Texte, Fokus und Status bleiben lesbar. |
| LH-GRU-003 | **MUSS** | Ein einheitliches Iconsystem muss Standardaktionen wie Neu, Öffnen, Speichern, Löschen, Suchen, Filtern, Aktualisieren, Exportieren, Einstellungen, Hilfe und Fehler abdecken. | Icons sind skalierbar oder in passenden DPI-Stufen vorhanden, besitzen semantische Namen und werden nicht direkt aus Anwendungscode über Dateipfade geladen. |
| LH-GRU-004 | **MUSS** | Basiskomponenten müssen mindestens Form, UserControl, Panel, GroupBox, Label, LinkLabel, Button, SplitButton, Separator und Statusanzeige umfassen oder einheitlich kapseln. | Für jede Basiskomponente existieren Normal-, Hover-, Fokus-, Disabled-, Fehler- und gegebenenfalls Busy-Zustände in der Gallery. |
| LH-GRU-005 | **MUSS** | Formulare und Controls müssen konsistente Mindestgrößen, Innenabstände, Tab-Reihenfolgen, Standardbuttons und Abbrechen-Verhalten verwenden. | Eine neue Maske kann ohne projektspezifische Layoutkonventionen erstellt werden und besteht die Tastaturprüfung. |
| LH-GRU-006 | **SOLL** | Krypton Standard Toolkit soll als erste visuelle Implementierungsbasis geprüft werden. AntdUI und ReaLTaiizor bleiben Vergleichs- beziehungsweise Ideenquellen und dürfen nicht unkontrolliert mit einem zweiten sichtbaren Theme-System gemischt werden. | Die Pilotentscheidung dokumentiert Designer-, DPI-, Accessibility-, Wartungs- und Lizenzergebnisse; pro Anwendung ist genau ein sichtbares Haupt-Theme-System festgelegt. |

### 7.4 Formulare und Eingabekomponenten

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-INP-001 | **MUSS** | Textfelder müssen ein- und mehrzeilige Eingabe, Platzhalter beziehungsweise Hilfetext, Read-only, MaxLength, Auswahl, Zwischenablage und eindeutige Fehlerzustände unterstützen. | Alle Zustände sind per Maus und Tastatur bedienbar; Validierungsfehler werden sichtbar und für Assistenztechnologien benannt. |
| LH-INP-002 | **MUSS** | Maskierte Eingaben müssen typische Formate wie Datum, Zeit, Telefon, Postleitzahl und frei definierte Masken abbilden können, ohne ungültige Zeichen stillschweigend zu akzeptieren. | Mindestens fünf dokumentierte Maskenfälle besitzen automatisierte Validierungstests. |
| LH-INP-003 | **MUSS** | Numerische Eingaben müssen Ganzzahl und Dezimalzahl, Min/Max, Schrittweite, Kulturformat, Nullwert und optionale Einheit unterstützen. | Deutsch- und englischsprachige Zahlenformate werden korrekt eingegeben, angezeigt und validiert. |
| LH-INP-004 | **MUSS** | Datums- und Zeiteingaben müssen Tastatureingabe, Picker, Nullwert, Min/Max und kulturspezifische Darstellung unterstützen. | Datum und Zeit können ohne Maus vollständig eingegeben und geändert werden; Grenzwerte werden nachvollziehbar gemeldet. |
| LH-INP-005 | **MUSS** | ComboBox, CheckBox, RadioButton und Toggle müssen einheitliche Bindung, Disabled-/Read-only-Zustände, Beschriftungen und Tastaturbedienung bieten. | Beispielmasken zeigen Datenbindung und Validierung für alle Auswahlkomponenten. |
| LH-INP-006 | **MUSS** | Datei- und Ordnerauswahl muss moderne Windows-Dialoge, Filter, Startverzeichnis, Mehrfachauswahl, sichere Fehlerbehandlung und Abbruch unterstützen. | Die Dialoge funktionieren mit langen Pfaden, nicht vorhandenen Pfaden, Netzpfaden und Benutzerabbruch ohne Ausnahmeverlust. |
| LH-INP-007 | **MUSS** | Ein FormLayout muss Label, Eingabe, Hilfetext, Pflichtkennzeichen und Fehlermeldung konsistent anordnen sowie variable Fensterbreiten und DPI berücksichtigen. | Eine Referenzmaske mit mindestens 20 Feldern bleibt bei 100 bis 200 Prozent DPI ohne Überlappung nutzbar. |
| LH-INP-008 | **MUSS** | Die Validierung muss Feldfehler, formularweite Fehler und eine Validation Summary unterstützen. Fehler dürfen nicht nur farblich vermittelt werden. | Ein ungültiges Formular fokussiert auf Anforderung den ersten Fehler, zeigt eine Zusammenfassung und stellt Fehler per Accessible Description bereit. |
| LH-INP-009 | **SOLL** | AutoComplete und MultiSelect/Tags sollen für Projekte, Kategorien, Empfänger, Schlagwörter und ähnliche Listen bereitstehen. | Eine Referenz mit mindestens 10.000 Vorschlägen bleibt bedienbar und blockiert die UI nicht dauerhaft. |
| LH-INP-010 | **KANN** | Slider, Range Slider und Color Picker können nach konkretem Bedarf ergänzt werden, gehören jedoch nicht zum R1-Lieferumfang. | Eine Aufnahme erfolgt nur mit dokumentiertem Anwendungsfall und ohne zusätzliches Theme-System. |

### 7.5 Application Shell, Navigation und Befehle

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-NAV-001 | **MUSS** | Es muss eine standardisierte Application Shell für SASD-WinForms-Anwendungen geben. Sie umfasst Titel-/Kopfleiste, Hauptnavigation, zentralen Arbeitsbereich, Statusfläche und Dienste für Dialoge, Theme und Einstellungen. | Mindestens die Referenzanwendungen CRUD, Workbench und Utility können dieselbe Shell-Konzeption verwenden. |
| LH-NAV-002 | **MUSS** | Hauptmenü, Kontextmenüs und Toolbar/CommandBar müssen auf einem gemeinsamen Command-System beruhen. Sichtbarkeit, Aktivierung, Text, Icon, Shortcut und Ausführung dürfen nicht mehrfach widersprüchlich definiert werden. | Eine Aktion kann gleichzeitig in Menü, Toolbar und Kontextmenü erscheinen und wechselt überall konsistent den Aktivierungszustand. |
| LH-NAV-003 | **MUSS** | Sidebar beziehungsweise Navigation View muss hierarchische Navigation, Auswahl, Icons, einklappbare Gruppen und optional kompakte Darstellung unterstützen. | Die Navigation ist mit Tastatur vollständig erreichbar; der aktuelle Bereich ist visuell und semantisch eindeutig. |
| LH-NAV-004 | **MUSS** | Tabs beziehungsweise Dokumentseiten müssen Erstellen, Auswählen, Schließen, Schließen-Abfrage bei ungespeicherten Änderungen und Wiederherstellung unterstützen. | Ein Dokument mit Dirty-State kann nicht ohne definierte Benutzerentscheidung verloren gehen. |
| LH-NAV-005 | **MUSS** | Split Pane, Accordion/Expander, TreeView, Breadcrumb und StatusBar müssen als wiederverwendbare Layout- und Navigationsbausteine verfügbar sein. | Eine Workbench-Referenz demonstriert Navigation links, Dokumentbereich, Detailbereich und Statusinformationen ohne projektspezifische Sondercontrols. |
| LH-NAV-006 | **SOLL** | Docking und Workspace sollen für Mail Workbench, Notes und vergleichbare Mehrbereichsanwendungen geprüft werden. Layouts müssen gespeichert, migriert und sicher zurückgesetzt werden können. | Ein gespeichertes Layout wird nach Neustart wiederhergestellt; ungültige oder alte Layoutdaten führen auf ein Standardlayout statt zum Absturz. |
| LH-NAV-007 | **KANN** | Ribbon und geführte Wizards können als optionale Module bereitgestellt werden. Sie dürfen den Kern nicht zu einer Ribbon-Anwendung zwingen. | Anwendungen ohne Ribbon oder Wizard laden keine dafür unnötigen Pakete. |

### 7.6 Datenanzeige, Listen und Tabellen

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-DAT-001 | **MUSS** | Ein SasdDataGrid muss Datenbindung, Sortierung, Spaltenformatierung, Auswahl, optionale Bearbeitung, Zeilenstatus, leere Zustände und Fehlerbehandlung vereinheitlichen. | Eine Referenz zeigt lesende und editierbare Grids mit identischer Grundbedienung und konsistenten Fehlermeldungen. |
| LH-DAT-002 | **MUSS** | Das DataGrid muss Suche und einfache Filter pro Spalte oder über eine zentrale Filterleiste ermöglichen. Filterzustände müssen sichtbar, löschbar und speicherbar sein. | Benutzer können erkennen, dass gefiltert wird, alle Filter mit einer Aktion entfernen und nach Neustart optional wiederherstellen. |
| LH-DAT-003 | **MUSS** | Spaltenreihenfolge, Breite, Sichtbarkeit, Sortierung und gegebenenfalls Filter müssen benutzerbezogen gespeichert und auf Standardwerte zurückgesetzt werden können. | Nach Neustart wird der Zustand wiederhergestellt; nach Schemaänderungen bleiben neue Spalten erreichbar und alte Einträge verursachen keinen Fehler. |
| LH-DAT-004 | **MUSS** | Große Datenmengen müssen über Paging oder Virtual Mode darstellbar sein. Die Oberfläche darf bei Laden, Sortieren oder Filtern nicht unkontrolliert blockieren. | Der gemeinsame Pilot mit 100.000 Datensätzen erfüllt die festgelegten Bedienbarkeits- und Ressourcenprüfungen. |
| LH-DAT-005 | **MUSS** | ListView/ObjectList und TreeView müssen neben dem Grid als leichtere Datenansichten verfügbar sein. Leere, ladende und fehlerhafte Zustände müssen standardisiert sein. | Dieselben Empty-/Loading-/Error-Komponenten werden in List-, Tree- und Grid-Ansichten verwendet. |
| LH-DAT-006 | **MUSS** | Data Pager und Search Panel müssen unabhängig von einer konkreten Datenbanktechnologie nutzbar sein. | Paging- und Suchverträge lassen sich mit In-Memory-Daten, SQLite und einer simulierten entfernten Quelle verwenden. |
| LH-DAT-007 | **SOLL** | Column Chooser, Summaries/Aggregates, Master-Detail und Datenexport nach CSV sollen in R2 folgen. Excel-spezifischer Export ist nur bei konkretem Bedarf aufzunehmen. | Die Funktionen können als optionale Module hinzugefügt werden; ein einfacher Grid-Einsatz bleibt schlank. |
| LH-DAT-008 | **SOLL** | PropertyGrid und virtualisierte Listen sollen nach Pilot für technische Einstellungen, Objektinspektion und große Listen verfügbar sein. | Die Komponente kann eigene Anzeigenamen, Kategorien, Beschreibungen, Validierung und Read-only-Eigenschaften abbilden. |
| LH-DAT-009 | **ZURÜCKGESTELLT** | Pivot/OLAP, vollständiges Spreadsheet und komplexer Filter Builder werden nicht selbst in V1 entwickelt. | Diese Funktionen erscheinen ausschließlich im Zukunfts-/Build-vs-Buy-Register. |

### 7.7 Dialoge, Rückmeldungen und Fehlerdarstellung

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-FED-001 | **MUSS** | Ein DialogService muss Information, Warnung, Fehler, Bestätigung, Ja/Nein/Abbrechen und benutzerdefinierte modale Inhalte einheitlich bereitstellen. | Dialoge verwenden konsistente Titel, Icons, Schaltflächenreihenfolge, Standardaktion, Escape-Verhalten und Besitzerfenster. |
| LH-FED-002 | **MUSS** | Fehlermeldungen müssen eine verständliche Benutzernachricht und optional technische Details mit Kopierfunktion, Korrelationskennung und Logverweis trennen. | Ein Fehlerdialog kann technische Details ein-/ausklappen und kopieren, ohne sensible Daten automatisch anzuzeigen. |
| LH-FED-003 | **MUSS** | Toast/Snackbar und Statusmeldungen müssen nichtmodale Rückmeldung mit Schweregrad, Zeitsteuerung, optionaler Aktion und barrierefreier Ankündigung ermöglichen. | Meldungen überdecken keine kritischen Eingaben dauerhaft und sind per Tastatur erreichbar, wenn sie Aktionen enthalten. |
| LH-FED-004 | **MUSS** | Tooltips und ScreenTips müssen kurze Hilfe, Shortcut und gegebenenfalls weiterführenden Hinweis liefern, dürfen aber keine zwingend benötigten Informationen exklusiv enthalten. | Alle zwingenden Informationen bleiben ohne Maus und ohne Tooltip zugänglich. |
| LH-FED-005 | **MUSS** | Progress Bar/Ring und Busy Overlay müssen unbestimmten und bestimmten Fortschritt, Status, Abbruch und Fehlerzustand unterstützen. | Operationen über einer festgelegten Dauer zeigen Fortschritt oder Busy-Status; abbrechbare Operationen bieten eine funktionierende Abbruchaktion. |
| LH-FED-006 | **SOLL** | Desktop-Benachrichtigungen und Teaching Tips sollen für Hintergrunddienste, neue E-Mails und Einführungshilfen als optionale Services verfügbar sein. | Benachrichtigungen lassen sich deaktivieren und respektieren Anwendungseinstellungen; Teaching Tips sind nicht Voraussetzung für die Bedienung. |

### 7.8 Datei-, Shell- und Windows-Integration

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-SYS-001 | **MUSS** | Recent Files muss zuletzt verwendete Dateien beziehungsweise Projekte mit Pinning, Entfernen, fehlenden Dateien und Datenschutzoptionen verwalten. | Fehlende Dateien werden verständlich behandelt; Nutzer können einzelne Einträge oder die gesamte Liste löschen. |
| LH-SYS-002 | **MUSS** | Drag-and-drop von Dateien muss Drop-Zonen, Dateitypprüfung, Mehrfachdateien, visuelles Feedback und sichere Fehlerbehandlung unterstützen. | Nicht unterstützte Dateien werden abgelehnt, ohne dass die Anwendung abstürzt oder sie ungeprüft verarbeitet. |
| LH-SYS-003 | **MUSS** | Zwischenablagefunktionen für Text, HTML, Bilder und Dateien müssen zentral gekapselt und fehlertolerant sein. | Eine temporär gesperrte Zwischenablage erzeugt eine kontrollierte Rückmeldung statt einer unbehandelten Ausnahme. |
| LH-SYS-004 | **MUSS** | System-Tray-Unterstützung muss Icon, Kontextmenü, Doppelklick, minimierten Hintergrundbetrieb und sauberes Beenden ermöglichen, sofern die Anwendung dies aktiviert. | Eine Anwendung kann Tray-Betrieb optional verwenden; Anwendungen ohne Tray laden keine unnötige Hintergrundlogik. |
| LH-SYS-005 | **SOLL** | Folder Tree soll für Datei-, Backup-, Notiz- und Repository-Werkzeuge hierarchische Navigation mit Lazy Loading und Fehlerzuständen bereitstellen. | Nicht zugreifbare Verzeichnisse blockieren nicht den gesamten Baum; Zugriffsmeldungen sind verständlich. |
| LH-SYS-006 | **SOLL** | WebView2 muss als kontrollierter Host für Markdown-Vorschau, Hilfe, HTML-Inhalte oder ausgewählte Webkomponenten verfügbar sein. | Navigation, Downloads, neue Fenster, lokale Ressourcen, Skriptbrücke und erlaubte Ursprünge sind explizit konfiguriert und dokumentiert. |
| LH-SYS-007 | **KANN** | Globale Hotkeys und ein vollständiger File Explorer werden nur bei nachgewiesenem Produktbedarf entwickelt. | Es existiert kein versteckter globaler Hook oder Shell-Ersatz im R1-Kern. |

### 7.9 Technische Editoren, Dokumente und Medien

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-DOC-001 | **SOLL** | Ein Markdown-Editor soll Textbearbeitung, Suche, grundlegende Formatieraktionen, Vorschau, Links, Tabellen und sicheren Export unterstützen. | Eine mittelgroße Markdown-Datei kann ohne Datenverlust bearbeitet, gespeichert und als Vorschau angezeigt werden. |
| LH-DOC-002 | **SOLL** | Ein Code-/Konfigurationseditor soll Syntaxhervorhebung, Zeilennummern, Suche/Ersetzen, Folding, Undo/Redo und große Textdateien unterstützen. IntelliSense und vollständige IDE-Funktionen sind nicht erforderlich. | Quelltext- und Logdateien im festgelegten Pilotumfang bleiben flüssig navigier- und durchsuchbar. |
| LH-DOC-003 | **SOLL** | Ein Diff Viewer soll Textunterschiede mindestens zeilenweise als Side-by-side oder Inline anzeigen und Unterschiede navigierbar machen. | Zwei Versionen eines Prompts, einer Konfiguration oder eines Quelltextes können verständlich verglichen werden. |
| LH-DOC-004 | **SOLL** | Programmgesteuerte PDF-Erzeugung muss Überschriften, Absätze, Tabellen, Seitenumbrüche, Kopf-/Fußbereiche, Metadaten und Bilder unterstützen. | Ein Beispielreport mit mehreren Seiten wird reproduzierbar erzeugt und visuell geprüft. |
| LH-DOC-005 | **SOLL** | Ein Image Viewer soll Zoom, Pan, Fit, Rotate und Hintergrunddarstellung für gängige Rasterformate bieten. | Große Bilder können ohne fehlerhafte Skalierung betrachtet; Zoom und Fit sind per Maus und Tastatur erreichbar. |
| LH-DOC-006 | **SOLL** | QR- und Barcode-Erzeugung sowie optionales Lesen sollen über eine kleine austauschbare Adapter-API verfügbar sein. | Mindestens QR und ein verbreiteter 1D-Code werden in einem automatisierten Roundtrip-Test erzeugt und gelesen. |
| LH-DOC-007 | **ZURÜCKGESTELLT** | Vollständiger DOCX-Editor, Report Designer, Spreadsheet, PDF-Editor, umfangreicher Rich-Text-Editor und Media Player gehören nicht zum aktuellen Kern. | Für diese Funktionen wird nur eine Build-vs-Buy-Notiz geführt. |

### 7.10 Diagramme, KPI-Anzeigen und Dashboards

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-VIZ-001 | **SOLL** | Ein Chart-Adapter soll mindestens Linien-, Balken- und Punktdiagramme mit Titel, Achsen, Legende, Tooltip, Zoom und Bildexport unterstützen. | Dieselbe SASD-Datenstruktur kann mindestens in einer primären Chart-Implementierung dargestellt und exportiert werden. |
| LH-VIZ-002 | **SOLL** | Kreis-/Donutdiagramm, Gauge und Sparkline sollen als optionale KPI-Ansichten verfügbar sein, sofern sie ohne zweite inkompatible UI-Suite integrierbar sind. | Die Component Gallery dokumentiert sinnvolle und ungeeignete Anwendungsfälle sowie Barrierefreiheitsalternativen. |
| LH-VIZ-003 | **KANN** | Eine einfache Heatmap kann für technische Messwerte aufgenommen werden. Finanzcharts, Treemap/Sunburst, Karten, Network Graph und Diagram Editor werden nur bei konkretem Bedarf betrachtet. | R1 enthält keine spezialisierten Visualisierungsengines außerhalb des definierten Chart-Piloten. |
| LH-VIZ-004 | **SOLL** | Eine Dashboard-Vorlage soll KPI-Karten, Filter, Charts, Statusmeldungen und gespeicherte Ansichten kombinieren, aber keinen universellen visuellen Dashboard-Designer entwickeln. | Die Vorlage ist Quellcode-basiert, dokumentiert und ohne proprietären Designer anpassbar. |

### 7.11 Qualitäts-, Bedienungs- und Sicherheitsanforderungen

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-QUL-001 | **MUSS** | Alle Kernkomponenten müssen Per-Monitor-DPI und Skalierungen von mindestens 100, 125, 150 und 200 Prozent berücksichtigen. Gemischte Mehrmonitor-Szenarien sind zu testen. | Die Referenzanwendungen zeigen keine abgeschnittenen Texte, überlappenden Controls oder unbrauchbar kleinen Icons in der Testmatrix. |
| LH-QUL-002 | **MUSS** | Alle wesentlichen Funktionen müssen per Tastatur erreichbar sein. Fokus muss sichtbar, Tab-Reihenfolge nachvollziehbar und Escape/Enter-Verhalten konsistent sein. | Ein vollständiger Kernablauf der Referenzanwendungen ist ohne Maus ausführbar. |
| LH-QUL-003 | **MUSS** | Accessibility muss über sinnvolle Name/Role/Value-Informationen, Beschreibungen, Zustände und UI-Automation für zentrale Controls unterstützt werden. | Accessibility-Tests und manuelle Prüfung erkennen die Kerncontrols und ihre Zustände; bekannte Grenzen werden dokumentiert. |
| LH-QUL-004 | **MUSS** | Deutsch und Englisch müssen über Ressourcen lokalisierbar sein. Texte dürfen nicht hart in Komponenten eingebaut sein; Formate müssen Kulturinformationen beachten. | Die Gallery kann zwischen Deutsch und Englisch wechseln; lange Übersetzungen verursachen keine unbrauchbaren Layouts. |
| LH-QUL-005 | **MUSS** | Langlaufende oder blockierende Operationen müssen asynchron beziehungsweise in geeigneter Hintergrundverarbeitung erfolgen und Progress, Abbruch sowie Fehlerbehandlung integrieren. | Die UI bleibt während definierter Datei-, Export- und Datenoperationen bedienbar; Abbruch führt in einen konsistenten Zustand. |
| LH-QUL-006 | **MUSS** | Komponenten dürfen keine unbehandelten Ausnahmen nach außen durchreichen, keine Geheimnisse protokollieren und keine unvalidierten Dateien oder Webinhalte automatisch ausführen. | Negativtests für ungültige Pfade, Dateien, Zwischenablage, Datenquellen und WebView-Navigation bestehen ohne Sicherheitsverletzung. |
| LH-QUL-007 | **MUSS** | State Persistence muss Versionswechsel, beschädigte Einstellungen und Zurücksetzen auf Standardwerte robust behandeln. | Beschädigte oder veraltete Zustandsdaten führen zu Wiederherstellung/Reset und nicht zu einem Startabbruch. |
| LH-QUL-008 | **MUSS** | Visuelle Regression, Accessibility-Prüfung, Unit- und Integrationstests sowie Ressourcen-/Handle-Prüfungen müssen Teil der Definition of Done sein. | Eine Komponente kann nur freigegeben werden, wenn die definierte Qualitätscheckliste vollständig dokumentiert ist. |
| LH-QUL-009 | **SOLL** | Telemetry- und Logging-Hooks sollen Ereignisse, Fehler und Leistungsdaten liefern, ohne einen bestimmten Telemetrieanbieter zu erzwingen. | Eine Anwendung kann Logging/Telemetry anbinden oder deaktivieren, ohne Komponenten zu ersetzen. |
| LH-QUL-010 | **MUSS** | Öffentliche APIs müssen verständlich, konsistent, dokumentiert und versionierbar sein. Breaking Changes sind nach semantischer Versionierung und Migrationshinweisen zu behandeln. | Für jede öffentliche Komponente existieren XML-Dokumentation, Beispiel und Changelog-Eintrag. |

### 7.12 Liefergegenstände und Betriebsmodell

| ID | Priorität | Anforderung | Prüfbarkeit / Abnahmekriterium |
| --- | :---: | --- | --- |
| LH-LIE-001 | **MUSS** | Der Lieferumfang muss Quellcode, NuGet-Pakete, Component Gallery, mindestens drei Referenzvorlagen, automatisierte Tests, API-Dokumentation, Lizenznachweise und Änderungsprotokoll umfassen. | Ein Release-Archiv beziehungsweise Release-Eintrag enthält alle genannten Artefakte und lässt sich aus dem Repository reproduzieren. |
| LH-LIE-002 | **MUSS** | Die Component Gallery muss jede freigegebene Komponente mit Zuständen, Varianten, Themes, DPI-Hinweisen, Accessibility-Hinweisen und Codebeispiel zeigen. | Keine öffentliche Komponente ist nur durch Quellcode oder Tests dokumentiert. |
| LH-LIE-003 | **MUSS** | Mindestens folgende Vorlagen sind vorzusehen: CRUD-/Verwaltungsanwendung, Workbench/Editor und kleines System-/Utility-Tool. | Jede Vorlage startet, demonstriert Navigation, Theme, Fehlerbehandlung, Einstellungen und Logging-Hooks und enthält eine kurze Anleitung. |
| LH-LIE-004 | **SOLL** | Eine Dashboard-/Monitoring-Vorlage und eine Wizard-/Setup-Vorlage sollen in R2 beziehungsweise R3 folgen. | Die Vorlagen verwenden nur bereits freigegebene Komponentenfamilien. |
| LH-LIE-005 | **MUSS** | Abhängigkeiten und Paketversionen müssen zentral verwaltet und regelmäßig überprüft werden. Sicherheits- und Lizenzänderungen müssen nachvollziehbar sein. | Ein automatisierter oder dokumentierter Pflegeprozess erzeugt Abhängigkeitsliste, Updateprüfung und SBOM. |
| LH-LIE-006 | **MUSS** | Supportstatus, bekannte Einschränkungen und Migrationshinweise jeder Version müssen dokumentiert sein. | Ein Anwender kann erkennen, welche .NET-Versionen, Windows-Ziele und Komponentenstände unterstützt werden. |

## 8. Referenztechnologien und Lieferantenstrategie

Die folgenden Kandidaten stammen aus dem UI-Komponenten-Katalog und sind **keine abschließende Implementierungsfestlegung**. Die endgültige Auswahl erfolgt im Pflichtenheft und durch Pilotanwendungen.

| Rolle | Primärer Kandidat | Alternative/Ergänzung | Entscheidung im Lastenheft |
| --- | --- | --- | --- |
| Plattform | Microsoft WinForms | - | Verbindlicher Plattformkern |
| Visuelle Basis | Krypton Standard Toolkit | AntdUI; ReaLTaiizor als Ideenquelle | Krypton zuerst pilotieren; sichtbare Theme-Systeme nicht mischen |
| Systemdialoge | Ookii.Dialogs.WinForms | Windows-Standarddialoge | Als Adapter prüfen |
| Grid | DataGridView/Krypton DataGridView | AdvancedDataGridView selektiv | Eigene SASD-Fassade; kein Enterprise-Grid-Nachbau |
| Charts | ScottPlot | LiveCharts2 | R2-Adapter; technische und Dashboard-Anwendungsfälle trennen |
| Codeeditor | ScintillaNET | WebView2/Monaco nur Sonderfall | R2-Adapter |
| Markdown/Diff | Markdig + ScintillaNET; DiffPlex | WebView2-Vorschau | R2 |
| Bildanzeige | Cyotek ImageBox | eigene leichte PictureBox-Erweiterung | R2 |
| Webinhalte | Microsoft WebView2 | CefSharp nur bei begründetem Bedarf | R2, Sicherheitsprofil zwingend |
| PDF-Erzeugung | PDFsharp/MigraDoc | QuestPDF nach Lizenzprüfung | R2; keine WYSIWYG-Designerpflicht |
| QR/Barcode | QRCoder/ZXing.Net | - | Kleiner austauschbarer Adapter |
| Kommerzieller Benchmark | DevExpress, Telerik, Syncfusion | MESCIUS/Infragistics | Funktionsreferenz; Zukauf nur mit Business Case |

## 9. Zukunftsregister – bewusst gesicherte, aber zurückgestellte Themen

| ID | Späteres Projekt/Thema | Gesicherte Idee | Reaktivierungskriterium |
| --- | --- | --- | --- |
| Z-001 | SASD WPF Components | WPF-native Umsetzung gemeinsamer Design Tokens, Commands, Dialog-/State-Services und ausgewählter Komponenten; keine bloße visuelle Kopie von WinForms. | Nach stabiler WinForms-R1 und dokumentierten plattformneutralen Verträgen. |
| Z-002 | WinUI 3 Pilot | Kleine Windows-Referenzanwendung zur Bewertung moderner nativer Windows-Integration und Fluent Design. | Nach WPF-Grundlage oder wenn ein konkretes Projekt moderne Windows-App-SDK-Funktionen verlangt. |
| Z-003 | Avalonia Pilot | WPF-nahe Cross-Platform-Referenz für Windows/Linux/macOS; Core und kommerzielle Pro-Komponenten getrennt bewerten. | Nur bei tatsächlichem Bedarf an Nicht-Windows-Desktop. Aktuell nicht geplant. |
| Z-004 | SASD Modern Web Components | ASP.NET Core/Blazor sowie mögliche Web-Component-, Tailwind- und daisyUI-Adapter. | Eigenes Lastenheft; nicht aus WinForms-Komponenten ableiten. |
| Z-005 | ASP.NET Web Forms Compatibility | Wartung/Migration bestehender ASPX-Anwendungen; AJAX Control Toolkit als historischer Funktions- und Migrationskatalog, nicht als Neuentwicklungsbasis. | Nur bei konkretem Legacy-Projekt. |
| Z-006 | SASD Java Business UI | OpenXava/XavaPro, Vaadin und OpenUI5 als Java-/Enterprise-Webansätze untersuchen. | Separates Java-Projekt nach C#-Desktop und Web-Prioritäten. |
| Z-007 | Mobile und weitere Desktopplattformen | Android, iOS, macOS und Linux-Desktop; mögliche Kandidaten MAUI, Avalonia, Uno oder native Lösungen. | Derzeit bewusst nicht geplant; nur nach Produkt- und Kundenbedarf reaktivieren. |
| Z-008 | Komplexe Geschäftskomponenten | Scheduler, Gantt, Kanban, Workflow, Pivot/OLAP, Spreadsheet, Report-/Dashboard-/Diagram-Designer, vollständiger Dokumenteditor, PDF-Editor und 3D-Visualisierung. | Build-vs-Buy-Entscheidung pro konkretem Projekt; keine Eigenentwicklung ohne belastbaren Business Case. |

## 10. Risiken und Gegenmaßnahmen

| ID | Risiko | Bewertung | Gegenmaßnahme |
| --- | --- | :---: | --- |
| R-001 | Umfang wächst zur Vollsuite | hoch | Strikte Releases, Komponentenfamilien und Ausschlussliste; neue Funktionen nur mit Anwendungsfall. |
| R-002 | Abhängigkeit von einem Theme-/Control-Hersteller | mittel/hoch | SASD-Fassaden, Adapter, keine unnötigen Fremdtypen in öffentlichen APIs. |
| R-003 | Designer- oder DPI-Probleme | hoch | Frühe Referenzanwendung und feste Designer-/DPI-Testmatrix vor breiter Übernahme. |
| R-004 | Uneinheitliche Optik durch mehrere Bibliotheken | hoch | Pro Anwendung genau ein sichtbares Haupt-Theme; Spezialcontrols optisch angleichen. |
| R-005 | Lizenz- oder Weitergaberisiko | hoch | Lizenzinventar, SBOM, THIRD-PARTY-NOTICES, Freigabeprozess und keine unklaren Pakete. |
| R-006 | Wartungsgrab durch Forks | hoch | Fork nur als letzte Option; Upstream-first und dokumentierter Exit-Plan. |
| R-007 | Accessibility wird zu spät betrachtet | hoch | Accessibility als Freigabekriterium ab erster Komponente, nicht als Nachrüstung. |
| R-008 | Komponenten funktionieren nur in Demo, nicht in echten Apps | hoch | Mindestens drei produktnahe Referenzanwendungen und Übernahme in reale SASD-Projekte. |
| R-009 | Übermäßige Abstraktion erschwert WinForms | mittel | Nur wiederkehrende, stabile Konzepte abstrahieren; einfache WinForms-Nutzung zulassen. |
| R-010 | Technologiealterung | mittel | Regelmäßige Abhängigkeits- und Roadmapprüfung; Trennung von Vertrag und Implementierung. |

## 11. Abnahme der Gesamtlieferung

- Alle MUSS-Anforderungen des freizugebenden Releases sind erfüllt oder mit akzeptierter Abweichung dokumentiert.
- Die festgelegte Designer-, DPI-, Tastatur-, Accessibility-, Lokalisierungs- und Ressourcen-Testmatrix ist bestanden.
- Die Component Gallery enthält alle freigegebenen Komponenten und Zustände.
- Mindestens drei Referenzanwendungen bauen reproduzierbar und verwenden die SASD-Pakete ohne Quellcodekopien.
- Öffentliche APIs, Beispiele, Changelog, Lizenzinventar, THIRD-PARTY-NOTICES und SBOM sind vorhanden.
- Bekannte Einschränkungen, Supportstatus, Upgrade- und Rücksetzpfade sind dokumentiert.

## 12. Offene Entscheidungen für das anschließende Pflichtenheft

- Exakte Paket- und Repository-Struktur sowie Namenskonventionen
- Krypton-Pilotentscheidung und Umfang der SASD-Fassaden
- Konkrete Persistenzformate und Migrationsstrategie
- Testwerkzeuge für Desktop-UI, visuelle Regression und Accessibility
- Performancegrenzwerte für Grid, Listen, Editor und Startzeit
- PDF-Bibliothek nach Funktions- und Lizenzprüfung
- Umfang von Docking, Ribbon, Chart- und Editor-Adaptern in R2
- Veröffentlichungsmodell: nur intern, öffentliches Open Source oder Open-Core/Supportangebot

## 13. Quellen- und Entscheidungsbasis

Dieses Lastenheft wurde aus dem internen **SASD UI-Komponenten-Katalog Version 2 mit WinForms-Fokus** (`SASD_UI-Komponenten-Katalog_v2_WinForms-Fokus_2026-07-23.md`) abgeleitet. Der Katalog enthält die breitere Markt- und Funktionsrecherche. Dieses Dokument reduziert diese Breite bewusst auf den für SASD derzeit realistischen WinForms-Umfang.

Wichtige Referenzgruppen aus dem Katalog: Microsoft WinForms, Krypton Standard/Extended Toolkit, AntdUI, ReaLTaiizor, Syncfusion WinForms als Funktionsbenchmark, WebView2, ScottPlot, LiveCharts2, ScintillaNET, Ookii.Dialogs, Cyotek ImageBox, PDFsharp/MigraDoc, FastReport OSS sowie die getrennt vorgemerkten Linien WPF, Avalonia, ASP.NET/ASPX und Java/OpenXava.

## 14. Änderungsprotokoll

| Version | Datum | Änderung |
| --- | --- | --- |
| 0.1 | 23. Juli 2026 | Erster Lastenheftentwurf aus dem UI-Komponenten-Katalog; WinForms-Scope, Komponentenfamilien, Anforderungen, Ausschlüsse und Zukunftsregister festgelegt. |