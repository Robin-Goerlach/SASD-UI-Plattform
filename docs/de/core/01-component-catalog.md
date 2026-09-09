# SASD UI-Komponenten-Katalog – Version 2, WinForms-Fokus

**Desktop-, Web-/AJAX-Komponenten, kommerzielle Referenzprodukte, Open-Source-Alternativen, Vorlagen, Startgewichtung und SASD-Entwicklungsstrategie**

- **Stand der Recherche:** 23. Juli 2026
- **Ziel:** Entscheidungsgrundlage für zwei SASD-Komponentenlinien; operative Priorität ist zunächst C# WinForms
- **Erfasste Anbieter/Projekte:** 161
- **Erfasste Komponentenklassen:** 123
- **Erfasste externe Vorlagenquellen:** 23
- **Vorgeschlagene SASD-Vorlagen:** 15

> **Wichtiger Hinweis:** „Open Source“, „Open Core“, „Source-available“, „Community Edition“ und „kostenlos nutzbar“ sind nicht dasselbe. Der Katalog beschreibt den recherchierten Stand und ersetzt keine juristische Lizenzprüfung. Vor Übernahme, Fork, Bündelung oder kommerziellem Vertrieb müssen die konkrete Version, transitive Abhängigkeiten, Markenrechte und Weitergabebedingungen erneut geprüft werden.

## Inhaltsverzeichnis

- [SASD-Entscheidung 2026: WinForms zuerst](#sasd-entscheidung-2026-winforms-zuerst)
- [Startgewichtung und Priorisierung](#startgewichtung-und-priorisierung)
- [Prüfung der nachgereichten Quellen](#prüfung-der-nachgereichten-quellen)
- [Vertiefung der wichtigsten WinForms-Lieferanten](#vertiefung-der-wichtigsten-winforms-lieferanten)
- [Management-Zusammenfassung](#management-zusammenfassung)
- [Abgrenzung: Zwei Produktlinien statt eines gemeinsamen Renderers](#abgrenzung-zwei-produktlinien-statt-eines-gemeinsamen-renderers)
- [Bewertungs- und Lizenzlegende](#bewertungs--und-lizenzlegende)
- [Kommerzielle und historische Referenzsuiten](#kommerzielle-und-historische-referenzsuiten)
- [Open-Source- und Community-Komponenten für Desktop](#open-source--und-community-komponenten-für-desktop)
- [Open-Source-, Open-Core- und Community-Komponenten für Web/AJAX](#open-source--open-core--und-community-komponenten-für-webajax)
- [Startgewichtung aller 123 Komponentenklassen für WinForms](#startgewichtung-aller-123-komponentenklassen-für-winforms)
- [Komponenten-Funktionskatalog](#komponenten-funktionskatalog)
- [Vorlagen und Referenzanwendungen](#vorlagen-und-referenzanwendungen)
- [Vorgeschlagener SASD-Vorlagenkatalog](#vorgeschlagener-sasd-vorlagenkatalog)
- [Zielarchitektur der SASD UI Platform](#zielarchitektur-der-sasd-ui-platform)
- [Entwicklungsregeln für Desktop-Komponenten](#entwicklungsregeln-für-desktop-komponenten)
- [Entwicklungsregeln für Web-/AJAX-Komponenten](#entwicklungsregeln-für-web-ajax-komponenten)
- [Qualitätssicherung](#qualitätssicherung)
- [Fork-, Upstream- und Lieferantenstrategie](#fork--upstream--und-lieferantenstrategie)
- [Roadmap](#roadmap)
- [Empfohlene erste Technologieauswahl](#empfohlene-erste-technologieauswahl)
- [Recherchegrenzen und Pflegeprozess](#recherchegrenzen-und-pflegeprozess)
- [Quellenverzeichnis](#quellenverzeichnis)


## SASD-Entscheidung 2026: WinForms zuerst

### Empfohlene Reihenfolge

1. **WinForms jetzt:** Die vorhandenen SASD-Anwendungen, Erfahrungen und Entwicklungsabläufe nutzen bereits WinForms. Der Visual-Studio-Designer, die direkte Ereignisprogrammierung und die stabile Windows-Integration ermöglichen den schnellsten Aufbau einer wiederverwendbaren Komponentenbasis.
2. **WPF danach:** WPF ergänzt XAML, Styles, ControlTemplates, Binding, MVVM und ein stärkeres Designsystem. Die zweite Produktlinie sollte nicht WinForms imitieren, sondern die gemeinsamen SASD-Verträge und Design Tokens nativ in WPF umsetzen.
3. **Anschließend zwei begrenzte Desktop-Piloten:**
   - **WinUI 3**, wenn native moderne Windows-Integration, Fluent Design und die Microsoft-Windows-Roadmap dominieren.
   - **Avalonia**, wenn Linux/macOS und eine WPF-nahe Cross-Platform-Architektur wichtiger sind.
   Die Entscheidung erfolgt anhand derselben kleinen Referenzanwendung, nicht anhand von Marketingseiten.
4. **ASP.NET Web Forms/ASPX separat und später:** „ASPX“ bezeichnet hauptsächlich die Dateiendung; die Plattform ist ASP.NET Web Forms. Sie bleibt relevant für Wartung und Migration bestehender Systeme, ist aber keine sinnvolle Basis für eine neue SASD-Webkomponentenlinie. Das archivierte AJAX Control Toolkit dient als Funktions- und Migrationskatalog.
5. **Modernes Web als eigenes C#-Projekt:** Später ASP.NET Core/Blazor und – je nach Ziel – JavaScript-/Web-Component-Adapter. Dieser Bereich darf nicht durch Anforderungen alter Web-Forms-Seiten eingeschränkt werden.
6. **Java/OpenXava als separates Projekt:** OpenXava ist ein modellgetriebenes Java/JPA-Anwendungsframework und keine C#- oder ASPX-Komponentenbibliothek. Seine Ideen für CRUD, Metadaten, Listen, Filter, Export und automatisch generierte Verwaltungsoberflächen sind wertvoll, gehören aber in einen eigenen Java-Katalog.

### Warum WinForms trotz modernerer Plattformen richtig ist

WinForms ist für SASD kein Endpunkt, sondern die **erste Produktions- und Lernplattform**. Entscheidend ist nicht, welche Technologie theoretisch am modernsten ist, sondern wo SASD kurzfristig:

- Komponenten wirklich in mehreren eigenen Anwendungen einsetzen kann,
- API- und Designregeln praktisch validiert,
- Visual-Studio-Designer und bestehende .NET-Kenntnisse nutzt,
- High-DPI-, Accessibility-, Tastatur- und Persistenzprobleme real löst,
- belastbare Tests, Beispiele und Dokumentation aufbaut,
- und erst danach gemeinsame Konzepte nach WPF, WinUI, Avalonia oder Web überträgt.

### Projektgrenzen

| Projektlinie | Aktueller Status | Zweck |
| --- | --- | --- |
| **SASD WinForms Components** | **Jetzt starten** | Produktiver Kern, Designsystem, Standardcontrols, Shells, Dialoge, Grid-Komfort, spezialisierte Adapter. |
| **SASD WPF Components** | Nächste Stufe | XAML/MVVM-native Umsetzung gemeinsamer Verträge und Designs. |
| **SASD Modern Desktop Evaluation** | Nach WPF-Grundlage | Vergleich WinUI 3 und Avalonia mit identischer Referenzanwendung. |
| **SASD ASP.NET Web Forms Compatibility** | Später, separat | Wartung, Migration und Kompatibilität vorhandener ASPX/Web-Forms-Anwendungen. |
| **SASD Modern Web Components** | Später, separat | ASP.NET Core/Blazor, Web Components und gegebenenfalls Tailwind/daisyUI. |
| **SASD Java Business UI** | Später, separat | OpenXava, Vaadin, OpenUI5 und andere Java-/Enterprise-Webansätze. |

## Startgewichtung und Priorisierung

Die Werte sind **Startwerte für eine technische Vorauswahl**, keine endgültigen Qualitätsurteile. Sie werden durch Pilotanwendungen, Tests und Lizenzprüfung ersetzt.

### Bewertungsmodell

| Kriterium | Gewicht | Leitfrage |
| --- | ---: | --- |
| Strategische Passung | 25 | Unterstützt das Projekt die aktuelle SASD-WinForms-Linie und wiederkehrende Anwendungsfälle? |
| Lizenz und Weitergabe | 15 | Darf SASD das Paket sicher verwenden, bündeln, anpassen und gegebenenfalls kommerziell ausliefern? |
| Aktivität und Zukunftsfähigkeit | 15 | Werden Releases, Fehlerbehebung, moderne .NET-Versionen und Sicherheitsfragen gepflegt? |
| Funktionsbreite und Qualität | 15 | Deckt das Projekt relevante Komponenten mit ausreichender Tiefe ab? |
| Designer und Entwicklererfahrung | 10 | Funktionieren Visual-Studio-Designer, IntelliSense, NuGet, Beispiele und Debugging zuverlässig? |
| Dokumentation und Beispiele | 10 | Können Funktionen ohne Quellcodearchäologie verstanden und eingesetzt werden? |
| DPI, Accessibility und Tests | 5 | Sind Skalierung, Tastatur, UI Automation, Mehrmonitorbetrieb und Testbarkeit berücksichtigt? |
| Integrations- und Wartungsrisiko | 5 | Wie klein ist das Risiko eigener Patches, API-Brüche und inkompatibler Themes? |

### Gewichtungsklassen

| Klasse | Punkte | Bedeutung |
| --- | ---: | --- |
| **S** | 85–100 | Kernkandidat; sofort in einer Referenzanwendung prüfen. |
| **A** | 70–84 | Sehr relevant; als Adapter, Spezialkomponente oder kommerzielle Option bewerten. |
| **B** | 55–69 | Selektiv verwenden; nicht automatisch Teil des SASD-Kerns. |
| **C** | 40–54 | Experiment, Ideenquelle oder spätere Beobachtung. |
| **D** | 0–39 | Legacy, Lizenzrisiko, unpassender Scope oder derzeit nicht übernehmen. |

### Startgewichtung der wichtigsten Technologien und Lieferanten

| Kandidat | Klasse | Startwert | Vorgesehene Rolle | Wichtigste Bedingung |
| --- | :---: | ---: | --- | --- |
| Microsoft WinForms | **S** | 95 | Plattformkern | Gemeinsame SASD-Controls nicht unnötig von Fremdsuiten abhängig machen. |
| Krypton Standard Toolkit | **S** | 89 | Bevorzugte visuelle Basis und Shell | Designer, DPI, Ressourcenfreigabe und Theme-Persistenz im Pilot testen. |
| Microsoft WebView2 | **S** | 86 | Sichere Brücke zu Webinhalten und modernen Editoren | Runtime-/Updatekonzept und Browser-Sicherheitsgrenzen dokumentieren. |
| ScottPlot | **A** | 83 | Technische Charts, Monitoring, Zeitreihen | Datenmengen, Interaktion und Export mit SASD-Testdaten prüfen. |
| Microsoft WPF | **A** | 84 | Zweite Desktop-Plattform | Eigene XAML-native Architektur statt bloßer WinForms-Portierung. |
| ScintillaNET | **A** | 80 | Quelltext-, Log- und Konfigurationseditor | Wrapper-API, Suchmodell und große Dateien testen. |
| Ookii.Dialogs.WinForms | **A** | 79 | Moderne Systemdialoge | Windows-Versionen und Fallbackverhalten testen. |
| LiveCharts2 | **A** | 77 | Animierte Dashboard-Charts | Rendering- und Speicherverhalten prüfen. |
| Cyotek ImageBox | **A** | 76 | Bildanzeige, Zoom, Pan und Auswahl | Nur als Spezialkomponente, nicht als allgemeines UI-Toolkit. |
| Syncfusion WinForms | **A** | 76 | Kommerzielle/Community-Option und Funktionsbenchmark | Aktuelle Community-Lizenzbedingungen und Verteilung jedes Pakets prüfen. |
| AntdUI | **A** | 73 | Moderner alternativer WinForms-Pilot | Lizenzdatei, englische Dokumentation, Designer und Langzeitpflege verifizieren. |
| Avalonia UI | **A** | 72 | Späterer Cross-Platform-Pilot | Freien Kern klar von Pro-Controls und kostenpflichtigem Tooling trennen. |
| Windows App SDK / WinUI 3 | **A** | 72 | Späterer moderner Windows-Pilot | Reifegrad, Packaging, Designer und Portierungsaufwand praktisch vergleichen. |
| ReaLTaiizor | **B** | 68 | Einzelcontrols, Themes und Prototypen | Keine Mischung mehrerer Stilfamilien innerhalb eines SASD-Produkts. |
| AdvancedDataGridView | **B** | 67 | Filter-/Sortiererweiterung des Standard-DataGridView | DPI, VirtualMode, Binding und große Datenmengen testen. |
| FastReport Open Source | **B** | 65 | Erste Report-Engine | Designer-/Exportgrenzen gegenüber kommerzieller Edition exakt dokumentieren. |
| AcrylicUI | **B** | 64 | Windows-11-artiger UI-/Docking-Pilot | Projektgröße, Reife, Designer und Accessibility intensiv prüfen. |
| DockPanel Suite | **B** | 62 | Alternative Docking-Engine | Nur einsetzen, falls Krypton Docking die Anforderungen nicht erfüllt. |
| KGySoft.WinForms | **B** | 61 | Spezialisierte Controls und Komponenten | Nichtstandardisierte Lizenz und Paketumfang juristisch/technisch prüfen. |
| CefSharp | **B** | 60 | Chromium-Fallback | Nur wenn WebView2 technisch nicht genügt; höherer Betriebs- und Updateaufwand. |
| Material3.WinForms | **C** | 48 | Beobachtung/Experiment | Sehr junges Preview-Projekt; keine Kernabhängigkeit. |
| AJAX Control Toolkit | **D** | 30 | Legacy-Funktions- und Migrationsreferenz | Archiviert; nicht für Neuentwicklung. |
| SunnyUI | **D** | 34 | Ideen- und Vergleichsquelle | GPL-/kommerzielle Nutzungsbedingungen vor jeder Verwendung klären. |
| OpenXava | **D*** | 28 | *Nur im C#-WinForms-Kontext niedrig; später Java-Projekt* | Nicht als C#-Control-Suite einordnen. |

### Priorität der zu entwickelnden SASD-WinForms-Komponenten

| Priorität | Komponentenfamilien | Strategie |
| --- | --- | --- |
| **P0 – Fundament** | Application Shell, BaseForm, Theme/Design Tokens, Icons, Standardbuttons, Eingaben, FormLayout, Validierung, Fehlerdarstellung, Dialoge, Toast/InfoBar, Busy/Progress, Statusleiste, Navigation, Such-/Filterleiste, Einstellungen, Fenster-/Spaltenpersistenz, Lokalisierung, Logging-Hooks | Selbst definieren; auf WinForms/Krypton aufbauen; durchgängige Verträge und Tests. |
| **P1 – Hoher Wiederverwendungswert** | DataGridView-Wrapper, List-/Tree-Ansichten, Master-Detail, Docking/Workspace, Ribbon/Commandbar, Property Editor, Chart-Adapter, Codeeditor, Image Viewer, WebView2-Host, CSV-/PDF-Export, Druckvorschau | Vorhandene OSS-Komponenten kapseln; kein Quellcode-Megafork. |
| **P2 – Spätere Geschäftskomponenten** | Scheduler, Kalender, Timeline, Maps, Report Viewer, Markdown-/HTML-Editor, PDF Viewer, Kanban, Gantt Light, erweiterte Grid-Gruppierung | Funktionsbedarf anhand konkreter SASD-Anwendungen belegen; Build-vs-Buy prüfen. |
| **P3 – Nicht selbst in V1 bauen** | Pivot/OLAP, vollständiger Ressourcenplaner, Spreadsheet-Engine, DOCX-Rich-Text-Editor, Dashboard Designer, Report Designer, Diagram Designer, PDF-Editor, 3D-/GPU-Hochleistungscharts | Kommerzielles Produkt oder klar abgegrenzter Adapter. |

### Mindest-Pilot für jeden Kernkandidaten

Jeder S- oder A-Kandidat muss dieselbe Referenzanwendung bestehen:

- 100.000 tabellarische Datensätze mit Sortierung, Filterung und virtueller Anzeige,
- Light/Dark und Wechsel zur Laufzeit,
- 100 %, 125 %, 150 %, 200 % und gemischte Mehrmonitor-DPI,
- vollständige Tastaturbedienung und sichtbarer Fokus,
- Screenreader-/UI-Automation-Baum für zentrale Controls,
- deutsche und englische Lokalisierung mit langen Texten,
- Designer öffnen, speichern, neu laden und Quellcode-Merge,
- Speicher-/Handleprüfung nach wiederholtem Öffnen und Schließen,
- Paketupdate inklusive visueller Regression,
- Beispiel, API-Dokumentation, Lizenznachweise und `THIRD-PARTY-NOTICES.md`.

## Prüfung der nachgereichten Quellen

Diese Tabelle verhindert Doppelungen: Bereits vorhandene Anbieter werden **vertieft**, nicht als neuer Eintrag gezählt.

| Quelle | Bereits enthalten? | Ergebnis der Prüfung | Einordnung im Katalog |
| --- | :---: | --- | --- |
| ajaxtoolkit.net | Ja, als AJAX Control Toolkit | Historische Website mit Komponentenübersicht; Inhalte und Verweise sind teilweise deutlich älter als der heutige Projektstatus. | Als Legacy-Dokumentationsquelle ergänzen; GitHub-Archiv bleibt maßgeblich. |
| Taiizor/ReaLTaiizor | Ja, D10 | Aktuelles WinForms-Paket mit sehr vielen Stilfamilien und Controls. | D10 vertiefen; selektiver Kandidat, nicht SASD-Basisdesign. |
| Krypton Standard Toolkit | Ja, D08 | Aktive Suite aus Toolkit, Ribbon, Navigator, Workspace und Docking für moderne .NET-Versionen. | D08/D09 vertiefen; primärer WinForms-Pilot. |
| Syncfusion WinForms UI Controls | Ja, kommerzielle Referenzsuite | Sehr breite WinForms-Suite mit über hundert Controls und Dokument-SDKs. | Vollständige Funktionsgruppen als Benchmark aufnehmen; keine OSS-/Fork-Basis. |
| Avalonia UI | Ja, D05 | MIT-lizenzierter Frameworkkern; bestimmte neue Pro-Controls und professionelle Werkzeuge sind getrennt lizenziert. | Späterer Cross-Platform-Pilot; Lizenzgrenzen pro Paket dokumentieren. |
| OpenXava / XavaPro | Nein | Modellgetriebenes Java/JPA-Webanwendungsframework, keine C#-/WinForms-Komponentensuite. | Neuer, ausdrücklich zurückgestellter Java-Abschnitt; nicht in WinForms-Auswahl einmischen. |
| SourceForge DevExpress Alternatives | Indirekt | Markt-/Verzeichnisliste mit heterogenen Treffern; enthält auch Produkte, die keine direkten UI-Suite-Alternativen sind. | Discovery-Quelle mit niedriger Beweiskraft; Kandidaten nur nach Primärquellen aufnehmen. |
| Slashdot DevExpress Alternatives | Indirekt | Ähnliche Marketplace-/Verzeichnislogik und Überschneidungen mit SourceForge. | Nicht doppelt zählen; nur als zusätzlicher Suchindex behandeln. |
| Jon Hilton: Blazor component libraries | Teilweise | Nützliche historische Blazor-Sammlung, vom Autor selbst als unvollständig bezeichnet; Preis- und Statusangaben altern. | Als Discovery-/Vorlagenquelle behalten, jeden Kandidaten offiziell verifizieren. |
| EDUCBA DevExpress alternatives | Teilweise | Schwerpunkt liegt stark auf Reporting-Produkten; keine belastbare vollständige DevExpress-Gegenüberstellung. | Reporting-Kandidaten als Suchhinweis, nicht als gleichwertige Vollsuiten klassifizieren. |
| awesome-dotnet-winforms | Neu als Suchquelle | Kuratierte Liste freier/queloffener WinForms-Projekte mit ausdrücklichem Lizenzprüfhinweis. | Als Discovery-Quelle aufnehmen; daraus ausgewählte aktuelle Kandidaten D40–D49 verifizieren. |

### Quellenhierarchie für künftige Pflege

| Stufe | Quellentyp | Verwendung |
| --- | --- | --- |
| **A** | Offizielle Dokumentation, Produktseite, Repository, Lizenzdatei, NuGet/npm-Metadaten | Maßgeblich für Funktionen, Version, Lizenz und Supportstatus. |
| **B** | Offizielle Blogs, Release Notes, Roadmaps, Beispielgalerien | Ergänzend für Reifegrad, Entwicklungstempo und konkrete Möglichkeiten. |
| **C** | Kuratierte Awesome-Listen, Fachartikel, seriöse Vergleiche | Discovery und Erfahrungshinweise; alle Aussagen verifizieren. |
| **D** | Marketplace-„Alternatives“-Seiten, SEO-Listen, alte Blogübersichten | Nur Kandidaten finden; niemals alleinige Grundlage einer SASD-Entscheidung. |

## Vertiefung der wichtigsten WinForms-Lieferanten

### Krypton Standard Toolkit – vorgesehene SASD-Basis

Krypton ist nicht nur ein Theme für Standardcontrols, sondern eine aus mehreren zusammenpassenden Paketen bestehende WinForms-Suite:

- **Toolkit:** Forms, Buttons, Labels, TextBox/MaskedTextBox/RichTextBox, ComboBox, ListBox, CheckedListBox, TreeView, DataGridView, PropertyGrid, Date-/Time-Eingaben, NumericUpDown, TrackBar, ProgressBar, Panels, GroupBox, Header, Separator, SplitContainer, ContextMenu, ToolStrip/StatusStrip-Integration, Dialoge und Paletten.
- **Ribbon:** Tabs, Groups, Buttons, CheckButtons, Galleries, ComboBoxen, Textfelder, Backstage/Application Menu, Quick Access Toolbar, KeyTips und kontextbezogene Tabs.
- **Navigator:** Tabs, Pages, Buttons, Header-/Bar-Modi, unterschiedliche Navigationserscheinungen und programmatische Seitenauswahl.
- **Workspace:** Dokument-/Arbeitsbereiche mit Zellen, Sequenzen, Seiten und layoutfähiger Struktur.
- **Docking:** Dockbare Fenster, Dokumente, Auto-Hide, Floating Windows, Workspace-Integration und Layoutverwaltung.
- **Themes/Paletten:** Office-, Visual-Studio- und weitere Paletten, eigene Farben/Renderer und zentraler PaletteManager.

**SASD-Rolle:** Krypton soll zunächst **Implementierungsdetail hinter SASD-eigenen Controls und Shells** bleiben. Anwendungen sollen möglichst `SasdButton`, `SasdDataGrid`, `SasdDialogService` und `SasdWorkspace` verwenden, nicht überall direkt Krypton-Typen. So bleibt ein späterer Wechsel oder eine WPF-Umsetzung möglich.

**Pilotrisiken:** Designer-Serialisierung, Themewechsel zur Laufzeit, DPI/Mehrmonitor, Accessibility/UI Automation, Ressourcen/Handles, Docking-Persistenz, NuGet-Paketaufteilung und Upgradeverhalten.

### Krypton Extended Toolkit – kontrollierte Ergänzung

Das Extended Toolkit enthält zahlreiche spezialisierte Pakete, darunter AdvancedDataGridView, zusätzliche Buttons, Calendar, Circular ProgressBar, ComboBoxen, Datenvisualisierung, InputBoxen, MessageBoxen, Navigation, Notifications, Outlook Grid, Panels, spezialisierte Dialoge, Theme Switcher, Toasts, Toggle Switches und Toolbox-Komponenten.

**SASD-Regel:** Nicht das Gesamtpaket ungeprüft übernehmen. Jede Erweiterung erhält:

1. einen konkreten SASD-Anwendungsfall,
2. einen eigenen Lizenz-/Abhängigkeitsnachweis,
3. einen Designer-/DPI-/Accessibility-Test,
4. einen Adapter oder eine SASD-Fassade,
5. eine dokumentierte Entfernungsmöglichkeit.

### ReaLTaiizor – breite visuelle Sammlung

ReaLTaiizor bündelt zahlreiche Stilfamilien und neu gezeichnete Controls. Je nach Paketstand gehören unter anderem Air-, Crown-, Dungeon-, Dream-, Forever-, Fox-, Hope-, Lost-, Material-, Metro-, Moon-, Night-, Poison-, Ribbon-, Royal-, Space-, Thunder- und weitere Designs dazu. Typische Komponenten sind Forms, Buttons, Textfelder, ComboBoxen, Tabs, Panels, GroupBoxen, CheckBoxen, RadioButtons, ProgressBars, Slider, Switches, Listen und Labels.

**Stärke:** Sehr schnelle visuelle Prototypen und eine ungewöhnlich große Auswahl an Erscheinungsbildern.

**Risiko:** Die Stilfamilien bilden kein einheitliches Enterprise-Designsystem. Maße, Zustände, API-Stil, DPI-Verhalten und Accessibility können sich unterscheiden. SASD sollte daher nur ausgewählte Controls einer festgelegten Stilfamilie kapseln und keine Anwendung als Mischung vieler Themes bauen.

### Syncfusion WinForms – vollständiger Funktionsbenchmark

Syncfusion ist eine kommerzielle Suite mit Community-Lizenzprogramm und keine frei forkbare Open-Source-Basis. Für SASD ist sie dennoch wichtig, weil sie zeigt, welche Funktionsbreite Anwender von einer etablierten WinForms-Suite erwarten können.

#### Daten, Grid und Listen

- DataGrid/SfDataGrid
- klassisches Grid Control
- Pivot Grid
- ListView
- ComboBox
- Editable ListBox
- Multicolumn ComboBox
- Multicolumn ListBox
- Multicolumn TreeView
- TreeView

#### Diagramme und Datenvisualisierung

- Chart
- Pivot Chart
- Sparkline
- Bullet Graph
- Digital Gauge
- Linear Gauge
- Radial Gauge
- Smith Chart
- TreeMap
- Map
- Barcode
- Themes/Skins und Theme Studio

#### Eingaben und Editoren

- TextBox
- MaskedTextBox
- Numeric TextBox
- NumericUpDown
- AutoComplete
- CheckBox
- Radio Button
- Color Picker und Color Picker DropDown
- Folder Browser
- Range Slider
- Radial Slider
- TrackBar
- Rating
- Watermark Text Provider
- Spell Checker

#### Navigation und Anwendungs-Shell

- Ribbon
- Menu und ContextMenuStrip
- ToolBar
- TabControl
- Breadcrumb
- Navigation Pane
- Navigation Drawer
- Tree Navigator
- Radial Menu
- Wizard
- Tab Splitter Container
- Excel-like Tabbar Splitter
- Scroll Frame

#### Layout und Fensterverwaltung

- Docking Manager
- Border Layout
- Card Layout
- Carousel
- Flow Layout
- Gradient Panel
- Grid Bag Layout
- Grid Layout
- Tile Layout
- SplitContainer
- Popup
- Gradient Label

#### Formulare, Dialoge und Benachrichtigungen

- Form/Metro Form/Office 2007 Form
- Tabbed Form
- MessageBox
- Splash Screen
- Progress Bar
- Status Bar/StatusStrip
- Tooltip
- Hub Tile

#### Kalender und Planung

- Calendar
- DateTimePicker
- Scheduler als eigenständiges SDK beziehungsweise Produktpaket

#### Editoren, Viewer und Dokumente

- Syntax Editor
- HTML Viewer
- PDF Viewer
- Spreadsheet Editor
- PDF-, Word-, Excel- und PowerPoint-Bibliotheken
- Smart Data Extraction
- Markdown-Verarbeitung je nach aktuellem Produktpaket

#### Sonstige Entwicklungsbausteine

- Calculation Engine
- Calculator
- Clock
- Grouping
- QTP Add-on
- eigenständige SDKs für Grid, Chart, Scheduler und Diagram
- AI AssistView als aktueller/teilweise vorläufiger Conversational-UI-Baustein

**SASD-Rolle:** Referenz für Vollständigkeit und optionaler Beschleuniger, falls die jeweils aktuelle Community- oder kommerzielle Lizenz passt. Kein Fork-Ziel. Die Community-Bedingungen müssen zum Zeitpunkt jeder Nutzung erneut geprüft werden. Am 23. Juli 2026 nennt Syncfusion als Schwellenwerte weniger als 1 Mio. USD Jahresbruttoumsatz, höchstens fünf Entwickler, höchstens zehn Beschäftigte und insgesamt nicht mehr als 3 Mio. USD externes Kapital. Für Open-Source-Projekte verlangt Syncfusion eine vorherige Registrierung; maßgeblich bleibt stets die aktuelle Lizenzvereinbarung.

### Avalonia – späterer Cross-Platform-Kandidat

Avalonia bleibt strategisch interessant, aber nicht die nächste operative Baustelle. Der Frameworkkern und viele Standardcontrols sind Open Source; professionelle Tooling-Angebote und bestimmte neue fortgeschrittene Controls werden separat angeboten. Insbesondere darf ein alter MIT-lizenzierter TreeDataGrid-Stand nicht mit aktuellen Pro-Produkten gleichen Namens verwechselt werden.

**SASD-Prüfpunkte:** Linux-Desktopqualität, Packaging, native Dialoge, Accessibility, DataGrid/TreeGrid-Lösung, Designer/Previewer-Lizenz, Drittanbieterökosystem, Ressourcenverbrauch und Portierungsaufwand aus WPF.

### AJAX Control Toolkit – historische ASPX-Funktionsquelle

Das AJAX Control Toolkit erweiterte klassische ASP.NET-Web-Forms-Controls über Extender und Composite Controls. Beispiele sind Accordion, AutoComplete, Calendar, CascadingDropDown, CollapsiblePanel, ColorPicker, ConfirmButton, DragPanel, FilteredTextBox, HoverMenu, MaskedEdit, ModalPopup, PasswordStrength, Slider, Tabs, TextBoxWatermark und Animationen.

Das Repository ist archiviert. Die Website `ajaxtoolkit.net` bleibt als historische Übersicht interessant, ist aber nicht ausreichend aktuell, um technischen Status oder eine Neuentwicklungsentscheidung zu begründen.

**SASD-Nutzung später:**

- Inventar bestehender ASPX-Seiten,
- Abbildung alter Extender auf modernes JavaScript/Blazor,
- Migrationsmuster für Partial Postbacks, UpdatePanel und serverseitigen ViewState,
- keine neue Kernkomponente auf Basis des Toolkits.

### OpenXava – für den späteren Java-Katalog

OpenXava erzeugt aus Java-/JPA-Modellen vollständige Verwaltungsoberflächen. Zu den dokumentierten Funktionen gehören editierbare Listen, Paging, Filter, Spaltenanpassung, PDF-/Excel-Ausgabe, Karten- und Diagrammansichten, Detailmasken, Tabs, Frames, Dialoge, Referenz-/Sammlungseditoren, responsive Layouts, Karten, Diskussionen, Bildergalerien, Datei-Upload und Dashboards. XavaPro ergänzt kommerzielle Funktionen und Support.

**Wichtige Abgrenzung:** OpenXava ist eher mit modellgetriebenen Business-Application-Frameworks wie DevExpress XAF zu vergleichen als mit einem einzelnen Button-/Grid-Toolkit. Seine Metadaten-, CRUD- und Generatorideen können später den SASD-Java- und eventuell einen C#-Low-Code-Katalog inspirieren; technisch wird es nicht in die WinForms-Komponentenlinie integriert.


## Management-Zusammenfassung

Der Markt besteht aus fünf deutlich verschiedenen Gruppen:

1. **Vollsuiten** wie DevExpress, Telerik, Syncfusion, MESCIUS und Infragistics. Sie liefern nicht nur Controls, sondern auch Designer, Themes, Export, Dokumentverarbeitung, Demos, Upgrade-Werkzeuge, Support und abgestimmte Releasezyklen.
2. **Spezialsuiten** für DataGrid, Charts, Reporting, Rich Text, Scheduling oder Diagramme. Beispiele sind Xceed, SciChart, LightningChart, TX Text Control, Bryntum und DHTMLX.
3. **Open-Source-Frameworks und Themes** wie WinForms, WPF, Avalonia, Krypton, WPF UI, MudBlazor, daisyUI oder Ant Design. Sie decken den allgemeinen UI-Bedarf ab, aber selten alle Enterprise-Funktionen.
4. **Open-Core-Produkte** wie AG Grid oder MUI X. Ein leistungsfähiger freier Kern wird durch kommerzielle Enterprise-Funktionen ergänzt.
5. **Headless- und Primitive-Bibliotheken** wie TanStack Table, Radix UI, Headless UI, Ark UI oder Tiptap. Sie liefern Verhalten und Zustandslogik, überlassen aber Markup und Gestaltung dem Produktteam.

Für SASD ist deshalb nicht das Zusammenkopieren fremder Repositories sinnvoll, sondern eine **kuratierte Plattform aus eigenen Komponentenverträgen, Design Tokens, Adaptern, Vorlagen und getesteten Paketkombinationen**. Ein Fork ist die letzte Eskalationsstufe, nicht der normale Integrationsweg.

### Strategische Empfehlung

- **Desktop V1:** WinForms auf .NET 8–10, Krypton als visuelle Basis, ScottPlot/LiveCharts2 für Charts, Mapsui für Karten, ScintillaNET für Code, FastReport OSS oder QuestPDF/PDFsharp für ausgewählte Berichte. WPF/Avalonia werden über Pilotprojekte vorbereitet.
- **Web V1:** Blazor mit MudBlazor oder Radzen als Suite; alternativ Tailwind+daisyUI für stärker kontrollierte Oberflächen. AG Grid Community, TanStack Table oder Tabulator werden als Grid-Adapter verglichen. Apache ECharts dient als bevorzugte umfangreiche Chartengine.
- **Gemeinsamer Kern:** Design Tokens, Icons, Lokalisierung, Accessibility-Regeln, Befehls- und Ereignisnamen, Datenmodelle, Validierungsregeln, Telemetrie und Testfälle. Nicht gemeinsam sind Rendering, Fokusmodell, Lebenszyklus und Plattformintegration.
- **Nicht selbst bauen in V1:** Enterprise-Pivot, vollständiger Reportdesigner, Spreadsheet-Engine, DOCX-Editor, Ressourcen-Scheduler, hochperformante 3D-Charts und vollwertiger Diagrammdesigner.

## Abgrenzung: Zwei Produktlinien statt eines gemeinsamen Renderers

SASD sollte zwei klar getrennte Oberflächenlinien entwickeln:

### 1. SASD Desktop Components

Desktop-Komponenten laufen in einem langlebigen Prozess, besitzen native Fenster, direkten Dateisystem- und Gerätezugriff, detaillierte Tastaturmodelle und häufig umfangreiche Designerintegration. Sie müssen High DPI, mehrere Monitore, Thread-Affinität, Drag-and-drop zwischen Anwendungen, System-Clipboard, Drucker, Tray und Offlinebetrieb beherrschen.

### 2. SASD Web/AJAX Components

Webkomponenten laufen in einem Browser-Sicherheitsmodell. Sie müssen Responsive Design, asynchrone Serverkommunikation, URL-/Routing-Zustand, Browser-History, ARIA, Touch, langsame Netzwerke, Server-Side Rendering/Hydration oder Blazor-Server-Latenz berücksichtigen. „AJAX“ wird im Katalog weit gefasst: klassische ASP.NET-Web-Forms-Extender, Fetch/XHR, SPA-Komponenten, Blazor und Web Components.

### Was tatsächlich gemeinsam sein kann

| Gemeinsamer Baustein | Beispiele |
| --- | --- |
| Design Tokens | Farbrollen, Typografie, Abstände, Radien, Schatten, Motion, Z-Index |
| Semantische Zustände | Default, Hover, Pressed, Focused, Selected, Disabled, Readonly, Invalid, Busy |
| Domänenmodelle | Tabellenspalte, Filterausdruck, Sortierung, Pagination, Validierungsfehler, Command |
| Formatierung | Datum, Zeit, Zahlen, Währung, Bytes, Dauer, Severity |
| Richtlinien | Accessibility, Lokalisierung, Logging, Telemetrie, Datenschutz, Fehlertexte |
| Testfälle | Komponentenverhalten, Tastatursequenzen, Datenzustände, leere/fehlerhafte/lade Zustände |
| Dokumentation | Begriffe, Beispiele, Migrationsregeln und Entscheidungstabellen |

### Was nicht künstlich vereinheitlicht werden sollte

- DOM und ARIA im Web gegenüber UI Automation und nativen Accessibility APIs auf Desktop.
- Browser-Routing gegenüber Fenster-/Dokumentenverwaltung.
- CSS-Layout gegenüber WinForms-Ankern oder XAML-Layoutsystemen.
- Web-Hydration und Netzwerkzustände gegenüber Desktop-Threading und Message Loop.
- Touch-/Browser-Gesten gegenüber systemweiten Shortcuts und Drag-and-drop zwischen Prozessen.

## Bewertungs- und Lizenzlegende

| Kennzeichnung | Bedeutung für SASD |
| --- | --- |
| Permissives Open Source | MIT, BSD oder Apache-2.0: grundsätzlich gut integrierbar; Hinweise und Abhängigkeiten bleiben zu prüfen. |
| Copyleft | Änderungs- und Weitergabepflichten können das Vertriebsmodell beeinflussen; isoliert und juristisch prüfen. |
| Open Core | Freier Kern plus kommerzielle Erweiterungen. Nur dokumentierte freie APIs und Funktionen als Basis einplanen. |
| Source-available | Quelltext ist sichtbar, aber Nutzung, Änderung oder Vertrieb sind nicht automatisch frei. |
| Community License | Kostenfreie Nutzung nur unter Bedingungen, etwa Umsatz-, Team- oder Organisationsgrenzen. |
| Kommerziell | Integration über reguläre Lizenz; kein Fork oder Quellcodeübernahme ohne ausdrückliche Rechte. |
| Legacy/archiviert | Nur als Ideen-, Migrations- oder Kompatibilitätsquelle; nicht für neue Kernarchitektur. |

## Kommerzielle und historische Referenzsuiten

### 1. DevExpress Universal

- **Einordnung:** Kommerzielle Vollsuite
- **Plattformen:** WinForms, WPF, Blazor, ASP.NET Core/MVC, JavaScript/TypeScript (DevExtreme), Reporting, Office/PDF, XAF/XPO
- **Lizenz-/Statushinweis:** Kommerziell; DevExtreme-Quellcode öffentlich einsehbar, Nutzungsrechte dennoch produktbezogen prüfen
- **Offizielle Quelle:** [https://www.devexpress.com/subscriptions/universal.xml](https://www.devexpress.com/subscriptions/universal.xml)
- **Wesentliche Komponenten:** Data Grid, TreeList, Pivot Grid, Card View, Charts, Gauges, Maps, Scheduler, Gantt, Diagram, Dashboard, Reporting, Spreadsheet, Rich Text Editor, PDF Viewer, Ribbon, Docking, Navigation, Form Layout, Data Editors, File Manager, Upload, HTML/Markdown Editor, Chat/AI-Oberflächen, Dokument-APIs, ORM und Anwendungsframework.
- **Bewertung für SASD:** Referenz für Funktionsbreite, Integrationsgrad, Designer, Dokumentation, Demos und Support. Kein realistisches Eins-zu-eins-Nachbauziel für SASD; sinnvoll als Benchmark für Komponentenverträge und Qualitätsmerkmale.

### 2. Progress Telerik UI / Kendo UI

- **Einordnung:** Kommerzielle Vollsuite
- **Plattformen:** WinForms, WPF, Blazor, ASP.NET AJAX/Web Forms, ASP.NET Core/MVC, Angular, React, Vue, jQuery
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.telerik.com/devcraft](https://www.telerik.com/devcraft)
- **Wesentliche Komponenten:** Grid/GridView, TreeList, PivotGrid, Charts, Gauges, Maps, Scheduler, Calendar, Gantt, TaskBoard, Diagram, Spreadsheet, Rich Text/Document Editor, PDF Viewer, Docking, Ribbon, PropertyGrid, Formulare, Navigation, Upload/File Manager, Conversational UI, Barcodes und Themes.
- **Bewertung für SASD:** Zweite zentrale Referenz neben DevExpress. Besonders wertvoll für die Trennung zwischen Desktop-Produkten, klassischem ASP.NET AJAX und nativen Framework-Komponenten unter Kendo UI.

### 3. Syncfusion Essential Studio

- **Einordnung:** Kommerzielle Vollsuite mit Community-Lizenzprogramm
- **Plattformen:** WinForms, WPF, WinUI, .NET MAUI, Blazor, ASP.NET Core/MVC, Angular, React, Vue, Flutter, JavaScript
- **Lizenz-/Statushinweis:** Kommerziell; Community-Lizenz nur bei jeweils geltenden Voraussetzungen
- **Offizielle Quelle:** [https://www.syncfusion.com/products/essential-studio](https://www.syncfusion.com/products/essential-studio)
- **Wesentliche Komponenten:** DataGrid, TreeGrid, Pivot, Charts, Scheduler, Gantt, Diagram, Kanban, Maps, Gauges, PDF/Office-Viewer und -Bibliotheken, Spreadsheet, Rich Text Editor, Dashboard- und Formularbausteine, Dateiupload, Navigation und Eingabekomponenten.
- **Bewertung für SASD:** Sehr breite Plattformabdeckung und wichtiger Vergleich für Cross-Platform-Strategien. Lizenzbedingungen und die Grenze zwischen freien Toolkits und kommerzieller Suite müssen pro Paket geprüft werden.

### 4. MESCIUS ComponentOne

- **Einordnung:** Kommerzielle .NET-Vollsuite
- **Plattformen:** WinForms, WPF, WinUI, .NET MAUI, Blazor, ASP.NET Core, Web API
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://developer.mescius.com/componentone](https://developer.mescius.com/componentone)
- **Wesentliche Komponenten:** FlexGrid, FlexChart, FlexPivot, FlexReport, FlexViewer, FlexSheet/Spreadsheet, FlexDiagram, Scheduler, Gauges, Maps, Input, Ribbon, Docking, Themes, PDF/Excel/Word-Dokumentdienste und Datenvirtualisierung.
- **Bewertung für SASD:** Starker Kandidat als Referenz für modulare .NET-Pakete, Grid/Chart/Pivot sowie gemeinsame Desktop- und Web-Produktlinien.

### 5. MESCIUS Wijmo

- **Einordnung:** Kommerzielle JavaScript-Suite
- **Plattformen:** JavaScript/TypeScript, Angular, React, Vue, Web Components
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://developer.mescius.com/wijmo](https://developer.mescius.com/wijmo)
- **Wesentliche Komponenten:** FlexGrid, MultiRow, TransposedGrid, Pivot, OLAP, Charts, Gauges, Kalender, Eingaben, Navigation, Viewer, Barcode, Karten und Spreadsheet-nahe Funktionen.
- **Bewertung für SASD:** Leichtgewichtige Web-Referenz mit TypeScript und wenig Abhängigkeiten. Besonders relevant für ein SASD-Webpaket mit adapterfähigen, frameworkübergreifenden Komponenten.

### 6. MESCIUS SpreadJS / Spread.NET / ActiveReports

- **Einordnung:** Kommerzielle Spezialprodukte
- **Plattformen:** JavaScript, WinForms, WPF, Blazor/ASP.NET-Viewer
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://developer.mescius.com/](https://developer.mescius.com/)
- **Wesentliche Komponenten:** Excel-artige Tabellenkalkulation, Formeln, Pivot, Import/Export, Dokument- und Berichtserstellung, Reportdesigner, Viewer, Druck und PDF/Excel-Ausgabe.
- **Bewertung für SASD:** Benchmark für die besonders aufwendigen Produktklassen Spreadsheet und Reporting. Diese Bereiche sollten in SASD zunächst über Adapter eingebunden und nicht neu implementiert werden.

### 7. Infragistics Ultimate / Ignite UI

- **Einordnung:** Kommerzielle Vollsuite
- **Plattformen:** WinForms, WPF, Blazor, Angular, React, Web Components, ASP.NET
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.infragistics.com/products/ultimate](https://www.infragistics.com/products/ultimate)
- **Wesentliche Komponenten:** Data Grid, Tree/Grid-Hierarchien, Spreadsheet, Charts, Gauges, Maps, Scheduler/Kalender, Docking, Ribbon, PropertyGrid, Editoren, Navigation, App Builder, Designsystem und eingebettete Analytik.
- **Bewertung für SASD:** Wichtige Referenz für Design-to-Code, App-Builder und datenintensive Oberflächen. Teilweise sind einzelne Web-Komponenten offen verfügbar; Funktions- und Lizenzgrenzen genau prüfen.

### 8. Actipro Software Controls

- **Einordnung:** Kommerzielle Desktop-Spezialsuite
- **Plattformen:** WPF, WinForms, WinUI
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.actiprosoftware.com/products/controls](https://www.actiprosoftware.com/products/controls)
- **Wesentliche Komponenten:** SyntaxEditor, Docking/MDI, Ribbon/Bars, Grids, PropertyGrid, Editors, Shell/File-Browser, Navigation, Charts, Micro Charts, Gauges, Wizard, Views, Themes und Barcode.
- **Bewertung für SASD:** Vorbild für eine fokussierte, hochwertige Desktop-Suite. Besonders interessant sind SyntaxEditor, Docking, PropertyGrid und Shell-Komponenten.

### 9. Xceed

- **Einordnung:** Kommerzielle Desktop- und Datenkomponenten
- **Plattformen:** WPF, .NET-Bibliotheken
- **Lizenz-/Statushinweis:** Kommerziell; Extended WPF Toolkit Community Edition separat offen
- **Offizielle Quelle:** [https://xceed.com/](https://xceed.com/)
- **Wesentliche Komponenten:** Hochentwickeltes WPF DataGrid, Toolkit-Controls, Themes sowie Daten-, Zip-, FTP- und Dokumentbibliotheken.
- **Bewertung für SASD:** Referenz für ein spezialisiertes Enterprise-DataGrid. Zeigt, dass ein Grid allein ein dauerhaftes Produkt mit Virtualisierung, Export, Master-Detail und Support sein kann.

### 10. Nevron Open Vision / Vision for .NET

- **Einordnung:** Kommerzielle Cross-Platform-Suite
- **Plattformen:** Blazor WebAssembly, WinForms, WPF, macOS, ASP.NET/MVC
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.nevron.com/dotnet-controls](https://www.nevron.com/dotnet-controls)
- **Wesentliche Komponenten:** Chart, Diagram, Grid, Rich Text Editor, Gauge, Scheduler, Ribbon, Docking, Layouts, Buttons, Eingaben, Maps und weitere Widgets aus gemeinsamer Codebasis.
- **Bewertung für SASD:** Besonders relevante Referenz für die Idee einer gemeinsamen Komponentenlogik über Desktop und Web. Trotzdem sollte SASD Rendering und Plattformintegration getrennt halten.

### 11. SciChart

- **Einordnung:** Kommerzielle Hochleistungsvisualisierung
- **Plattformen:** WPF, Avalonia/XPF, JavaScript, mobile Plattformen
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.scichart.com/](https://www.scichart.com/)
- **Wesentliche Komponenten:** 2D/3D-Charts, Echtzeit- und Streaming-Daten, wissenschaftliche Diagramme, Heatmaps, Spektrogramme, Finanzcharts, Annotationen und GPU-beschleunigtes Rendering.
- **Bewertung für SASD:** Benchmark für medizinische, technische und finanzielle Echtzeitvisualisierung. Kein sinnvoller Eigenbau für eine allgemeine SASD-Suite.

### 12. LightningChart

- **Einordnung:** Kommerzielle Hochleistungsvisualisierung
- **Plattformen:** .NET WinForms/WPF/UWP, JavaScript, Python
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://lightningchart.com/](https://lightningchart.com/)
- **Wesentliche Komponenten:** GPU-beschleunigte 2D/3D-Charts, Signalwerkzeuge, Heatmaps, Spektrogramme, Geokarten, Dashboards und große Streaming-Datenmengen.
- **Bewertung für SASD:** Weitere Leistungsreferenz für Spezialdiagramme; als optionaler Adapter denkbar, nicht als SASD-Kern.

### 13. Steema TeeChart

- **Einordnung:** Kommerzielle Chart-Suite
- **Plattformen:** .NET, JavaScript/TypeScript, Delphi VCL/FireMonkey, Java und weitere
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.steema.com/](https://www.steema.com/)
- **Wesentliche Komponenten:** Breites Sortiment an 2D/3D-Diagrammtypen, Interaktion, Zoom, Scrolling, Annotationen, Editor, Export und plattformabhängige Integrationen.
- **Bewertung für SASD:** Relevante historische und aktuelle Referenz für eine chartzentrierte Mehrplattformstrategie; Delphi bleibt für SASD vorerst außerhalb des Umfangs.

### 14. TX Text Control

- **Einordnung:** Kommerzielle Dokumenteditor-Suite
- **Plattformen:** WinForms, WPF, ASP.NET Core, Blazor/Browser-Integration
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.textcontrol.com/](https://www.textcontrol.com/)
- **Wesentliche Komponenten:** Word-Prozessor, Dokumenteditor, Serienbriefe, DOCX/RTF/PDF-Verarbeitung, Formulare, Kommentare, Track Changes, Signaturen, Barcode und Dokumentautomatisierung.
- **Bewertung für SASD:** Benchmark für professionelle Textverarbeitung. Für SASD zunächst nur Adapter- oder Integrationskandidat.

### 15. Sencha Ext JS

- **Einordnung:** Kommerzielles Webframework mit Vollsuite
- **Plattformen:** JavaScript/TypeScript, Classic und Modern Toolkit
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.sencha.com/products/extjs/](https://www.sencha.com/products/extjs/)
- **Wesentliche Komponenten:** Grid, PivotGrid, Tree, Charts, D3-Adapter, Formulare, Layouts, Panels, Fenster, Menüs, Toolbars, Calendar und Exporter in einem integrierten Framework.
- **Bewertung für SASD:** Web-Referenz für eine stark integrierte LOB-Plattform. Gegenmodell zu kleinen austauschbaren Bibliotheken; hoher Lock-in, aber konsistente APIs.

### 16. DHTMLX Suite und PM-Komponenten

- **Einordnung:** Kommerzielle Websuite mit einzelnen Community-Editionen
- **Plattformen:** JavaScript/TypeScript, React, Angular, Vue, Svelte-Integration
- **Lizenz-/Statushinweis:** Gemischt: kommerziell; DHTMLX Gantt Community unter MIT mit reduziertem Umfang
- **Offizielle Quelle:** [https://dhtmlx.com/](https://dhtmlx.com/)
- **Wesentliche Komponenten:** Grid, TreeGrid, Gantt, Scheduler, Kanban, To-do, Booking, Diagram, Spreadsheet, Rich Text Editor, Vault/File-Upload, Formulare, Layouts und Toolbar.
- **Bewertung für SASD:** Sehr relevante Quelle für Projektplanung, Scheduling und Workflow-Komponenten. Community- und Pro-Funktionen müssen getrennt katalogisiert werden.

### 17. Bryntum

- **Einordnung:** Kommerzielle Web-Spezialsuite
- **Plattformen:** JavaScript/TypeScript, React, Angular, Vue
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://bryntum.com/](https://bryntum.com/)
- **Wesentliche Komponenten:** Grid, Scheduler, Scheduler Pro, Gantt, Calendar und TaskBoard/Kanban mit gemeinsamer Daten- und Scheduling-Engine.
- **Bewertung für SASD:** Referenz für tief integrierte Projekt- und Ressourcenplanung. Für SASD eher externer Spezialadapter als Eigenentwicklung.

### 18. AG Grid Enterprise / AG Charts Enterprise

- **Einordnung:** Open-Core-Datagrid und Charts
- **Plattformen:** JavaScript, React, Angular, Vue
- **Lizenz-/Statushinweis:** Community MIT; Enterprise kommerziell
- **Offizielle Quelle:** [https://www.ag-grid.com/](https://www.ag-grid.com/)
- **Wesentliche Komponenten:** Community: Sortierung, Filter, Paging, Editing, Theming. Enterprise: serverseitige Datenmodelle, Gruppierung, Pivot, Excel-Export, integrierte Charts, Master-Detail, Tree Data und erweiterte Werkzeuge.
- **Bewertung für SASD:** Einer der stärksten Kandidaten für ein SASD-Web-Grid. Die freie Community-Version ist direkt nutzbar; Enterprise-Funktionen dürfen nicht nachgebildet oder übernommen werden.

### 19. Handsontable

- **Einordnung:** Kommerzielle Spreadsheet-Komponente mit nichtkommerziellen Optionen
- **Plattformen:** JavaScript, React, Angular, Vue
- **Lizenz-/Statushinweis:** Source-available/kommerziell; genaue Nutzungslizenz prüfen
- **Offizielle Quelle:** [https://handsontable.com/](https://handsontable.com/)
- **Wesentliche Komponenten:** Excel-artiges Grid, Zelltypen, Formeln, Copy/Paste, Fill Handle, Freeze, Merge, Validierung, Kommentare und große Tabellen.
- **Bewertung für SASD:** Benchmark für spreadsheetartige Bedienung. Nicht automatisch als Open Source behandeln.

### 20. Highcharts

- **Einordnung:** Kommerzielle Web-Visualisierung mit Quellzugang
- **Plattformen:** JavaScript/TypeScript und Framework-Wrapper
- **Lizenz-/Statushinweis:** Für kommerzielle Nutzung kostenpflichtig
- **Offizielle Quelle:** [https://www.highcharts.com/](https://www.highcharts.com/)
- **Wesentliche Komponenten:** Charts, Stock/Finanzcharts, Maps, Gantt, Dashboards, Export und Accessibility.
- **Bewertung für SASD:** Sehr ausgereifte Chart-Referenz; Lizenzmodell klar von permissivem Open Source unterscheiden.

### 21. Wisej.NET

- **Einordnung:** Kommerzielles serverseitiges Web-UI-System
- **Plattformen:** ASP.NET/ASP.NET Core, C#/VB.NET, Browser, Hybrid
- **Lizenz-/Statushinweis:** Community- und kommerzielle Editionen
- **Offizielle Quelle:** [https://wisej.com/](https://wisej.com/)
- **Wesentliche Komponenten:** Desktopartige Webfenster, Forms, Pages, Desktop/Taskbar, Grid, Charts, Layouts, Eingaben, Responsive Profiles, Theme Builder, Visual-Studio-WYSIWYG-Designer und JavaScript-Erweiterungen.
- **Bewertung für SASD:** Besonders interessant als Brücke von WinForms-Denken zu Weboberflächen. Architektur und Laufzeitmodell unterscheiden sich jedoch stark von Blazor oder clientseitigen Web Components.

### 22. AJAX Control Toolkit

- **Einordnung:** Historische Open-Source-Web-Forms-Suite
- **Plattformen:** ASP.NET Web Forms
- **Lizenz-/Statushinweis:** BSD-3-Clause; Repository seit Oktober 2024 archiviert
- **Offizielle Quelle:** [https://github.com/DevExpress/AjaxControlToolkit](https://github.com/DevExpress/AjaxControlToolkit)
- **Wesentliche Komponenten:** Accordion, AutoComplete, Calendar, CascadingDropDown, CollapsiblePanel, ColorPicker, ConfirmButton, DragPanel, FilteredTextBox, HoverMenu, MaskedEdit, ModalPopup, PasswordStrength, Slider, Tabs, Watermark und Animation-Extender.
- **Bewertung für SASD:** Wichtige historische Quelle für AJAX-Extender und progressive Verbesserung klassischer Server-Controls. Nur als Ideen- und Migrationsreferenz verwenden, nicht als Basis neuer SASD-Webprojekte.

### 23. MindFusion UI Controls

- **Einordnung:** Kommerzielle Desktop- und Web-Spezialsuite
- **Plattformen:** WinForms, WPF, JavaScript, Java, mobile Plattformen
- **Lizenz-/Statushinweis:** Kommerziell; einzelne Beispiele oder Hilfsprojekte separat lizenziert
- **Offizielle Quelle:** [https://mindfusion.dev/](https://mindfusion.dev/)
- **Wesentliche Komponenten:** Diagramme und Flowcharts, Scheduler/Kalender, Charts, Gauges, Maps, Virtual Keyboard, Spreadsheet, Reporting sowie klassische JavaScript-Widgets wie TreeView, Tabs, Menüs, Dialoge und Date/Time Picker.
- **Bewertung für SASD:** Relevanter mittelgroßer Spezialanbieter mit parallelen Desktop- und Webprodukten. Besonders wertvoll als Vergleich für Diagramm-, Scheduler- und Visualisierungsadapter.

### 24. Flexmonster Pivot Table & Charts

- **Einordnung:** Kommerzielle Web-Spezialkomponente
- **Plattformen:** JavaScript/TypeScript, React, Angular, Vue, Blazor-Integration
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://www.flexmonster.com/](https://www.flexmonster.com/)
- **Wesentliche Komponenten:** Pivot Table, Pivot Charts, Slice-/Dice-Operationen, Drill-down, OLAP- und tabellarische Datenquellen, JSON/CSV, serverseitige Datenanbindung, Export und Dashboard-Integration.
- **Bewertung für SASD:** Starke Referenz für eingebettete Pivot- und BI-Funktionen. Ein eigener SASD-Pivotkern wäre unverhältnismäßig aufwendig; Integration über Adapter bevorzugen.

### 25. DayPilot Pro

- **Einordnung:** Kommerzielle Planungs- und Kalenderkomponenten
- **Plattformen:** JavaScript, Angular, React, Vue, ASP.NET, ASP.NET MVC
- **Lizenz-/Statushinweis:** Kommerziell; Lite-/Beispielausgaben je nach Produkt separat prüfen
- **Offizielle Quelle:** [https://javascript.daypilot.org/](https://javascript.daypilot.org/)
- **Wesentliche Komponenten:** Event Calendar, Resource Scheduler, Month View und Gantt mit Drag-and-drop, Zeitachsen, Ressourcen, Buchung, Schichtplanung, Time Tracking und Theme Designer.
- **Bewertung für SASD:** Fokussierter Benchmark für Scheduler- und Ressourcenplanung; nützlich für eine spätere SASD-Planungsproduktlinie.

### 26. GoJS

- **Einordnung:** Kommerzielle Diagramm- und Graphkomponente
- **Plattformen:** JavaScript/TypeScript, React/Angular/Vue-Integration
- **Lizenz-/Statushinweis:** Kommerziell für produktiven Einsatz; Evaluierungslizenz verfügbar
- **Offizielle Quelle:** [https://gojs.net/](https://gojs.net/)
- **Wesentliche Komponenten:** Interaktive Diagramme, Flowcharts, Organigramme, BPMN-nahe Modelle, Mindmaps, Graphlayouts, Datenbindung, Drag-and-drop, Undo/Redo, Palette und Overview.
- **Bewertung für SASD:** Wichtige Referenz für ausgereifte Diagrammeditoren. Für SASD eher optionaler Adapter als Eigenentwicklung.

### 27. DevComponents DotNetBar

- **Einordnung:** Kommerzielle WinForms-Controlsuite
- **Plattformen:** WinForms
- **Lizenz-/Statushinweis:** Kommerziell
- **Offizielle Quelle:** [https://marketplace.visualstudio.com/items?itemName=DevCo.DotNetBarforWindowsForms](https://marketplace.visualstudio.com/items?itemName=DevCo.DotNetBarforWindowsForms)
- **Wesentliche Komponenten:** Ribbon, Bars und Toolbars, Docking, Navigation, Metro-/Office-Styles, PropertyGrid, Editors, SuperGrid, Tree, Wizard, Schedule/Calendar und weitere WinForms-Controls.
- **Bewertung für SASD:** Historisch wichtige WinForms-Suite und Vergleichsquelle für Ribbon, Docking und stark gestaltete Desktopoberflächen. Langfristigen Produktstatus vor Beschaffung prüfen.

## Open-Source- und Community-Komponenten für Desktop

Die folgende Liste enthält Frameworks, allgemeine Control-Sammlungen und Spezialkomponenten. Ein einzelnes Projekt deckt bewusst nicht alle Kategorien ab.

### D01. Microsoft WinForms

- **Plattformen:** Windows/.NET
- **Lizenz:** MIT
- **Schwerpunkt:** Reife Windows-Desktopbasis mit Designer, Datenbindung und großem Ökosystem.
- **Komponenten/Funktionen:** Standardcontrols, DataGridView, TreeView, ListView, Menüs, ToolStrip, Dialoge, MDI, Druck und benutzerdefinierte Controls.
- **SASD-Einschätzung:** Für bestehende SASD-Projekte kurzfristig Hauptplattform. Fehlende moderne Enterprise-Funktionen über SASD-Wrapper ergänzen.
- **Quelle:** [https://github.com/dotnet/winforms](https://github.com/dotnet/winforms)

### D02. Microsoft WPF

- **Plattformen:** Windows/.NET/XAML
- **Lizenz:** MIT
- **Schwerpunkt:** XAML, Styling, Templates, Binding, MVVM und vektorbasierte Oberfläche.
- **Komponenten/Funktionen:** DataGrid, TreeView, ListView, FlowDocument, RichTextBox, Animation, 2D-Grafik, Commands, Ressourcen und ControlTemplates.
- **SASD-Einschätzung:** Geeignet für langfristige Windows-Desktopprodukte mit stärkerem Designsystem und MVVM.
- **Quelle:** [https://github.com/dotnet/wpf](https://github.com/dotnet/wpf)

### D03. Windows App SDK / WinUI 3

- **Plattformen:** Windows
- **Lizenz:** Open-Source-Komponenten plus Windows-Laufzeit-/SDK-Abhängigkeiten; konkrete Pakete einzeln prüfen
- **Schwerpunkt:** Microsofts modernes natives Windows-UI-Framework und Fluent Design.
- **Komponenten/Funktionen:** NavigationView, CommandBar, NumberBox, InfoBar, TeachingTip, modernisierte Standardcontrols, Windowing, App Lifecycle, Notifications und Windows-Integration.
- **SASD-Einschätzung:** Nach der WPF-Grundlage in einer identischen Referenzanwendung gegen Avalonia testen. Microsoft empfiehlt WinUI 3 für neue native Windows-Apps; für SASD überwiegt aktuell dennoch der Produktivitäts- und Bestandsvorteil von WinForms.
- **Quelle:** [https://github.com/microsoft/WindowsAppSDK](https://github.com/microsoft/WindowsAppSDK)

### D04. .NET MAUI Community Toolkit

- **Plattformen:** Android, iOS, macOS, Windows
- **Lizenz:** MIT
- **Schwerpunkt:** Erweiterungen, Behaviors, Converter und zusätzliche UI/UX-Controls.
- **Komponenten/Funktionen:** Popup, MediaElement, DrawingView, CameraView je nach Paket, Animationen, Touch/StatusBar-Helfer und Converter.
- **SASD-Einschätzung:** Nur relevant, wenn SASD später echte mobile Anwendungen entwickelt.
- **Quelle:** [https://github.com/CommunityToolkit/Maui](https://github.com/CommunityToolkit/Maui)

### D05. Avalonia UI

- **Plattformen:** Windows, Linux, macOS, mobile Ziele und WebAssembly
- **Lizenz:** Frameworkkern MIT; professionelle Werkzeuge und bestimmte fortgeschrittene Controls separat lizenziert
- **Schwerpunkt:** Cross-Platform-XAML-Framework mit WPF-ähnlichem Modell.
- **Komponenten/Funktionen:** 70+ freie Standardcontrols nach Herstellerangabe, Styles, Binding, Rendering, Themes, plattformübergreifende Fenster und Drittanbieterökosystem; Pro-Angebote ergänzen unter anderem fortgeschrittene Daten- und Chartkomponenten.
- **SASD-Einschätzung:** Wichtigster Cross-Platform-Pilot nach WinForms und WPF. Lizenz und Produktzuordnung jedes DataGrid-/TreeDataGrid-/Tooling-Pakets ausdrücklich festhalten.
- **Quelle:** [https://github.com/AvaloniaUI/Avalonia](https://github.com/AvaloniaUI/Avalonia)

### D06. Uno Platform

- **Plattformen:** Windows, WebAssembly, Linux, macOS, mobile Ziele
- **Lizenz:** Apache-2.0 für Kernprojekte; kommerzielle Zusatzangebote
- **Schwerpunkt:** WinUI-kompatible Cross-Platform-Anwendungen.
- **Komponenten/Funktionen:** WinUI-Controlmodell, Material/Fluent Themes, Toolkit, Navigation, Responsive Layouts und WebAssembly-Ausgabe.
- **SASD-Einschätzung:** Alternative zu Avalonia; vor Festlegung über Prototyp und Linux-Desktopqualität vergleichen.
- **Quelle:** [https://github.com/unoplatform/uno](https://github.com/unoplatform/uno)

### D07. Eto.Forms

- **Plattformen:** Windows, Linux, macOS
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** Abstraktion über native Desktop-Toolkits.
- **Komponenten/Funktionen:** Standardcontrols, Layouts, Menüs, Dialoge, Drawing und Plattformhandler.
- **SASD-Einschätzung:** Interessant für native Optik mit kleinerem Komponentenökosystem; eher Nischenoption.
- **Quelle:** [https://github.com/picoe/Eto](https://github.com/picoe/Eto)

### D08. Krypton Standard Toolkit

- **Plattformen:** WinForms/.NET
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** Umfangreiche moderne WinForms-Suite.
- **Komponenten/Funktionen:** Toolkit, Ribbon, Navigator, Workspace, Docking, Themes/Paletten, DataGridView-Integration, Toolbars, Dialoge und Standardcontrols.
- **SASD-Einschätzung:** Beste kurzfristige Basis für eine einheitliche SASD-WinForms-Oberfläche.
- **Quelle:** [https://github.com/Krypton-Suite/Standard-Toolkit](https://github.com/Krypton-Suite/Standard-Toolkit)

### D09. Krypton Extended Toolkit

- **Plattformen:** WinForms/.NET
- **Lizenz:** MIT
- **Schwerpunkt:** Ergänzende Controls und Hilfsfunktionen rund um Krypton.
- **Komponenten/Funktionen:** Zusätzliche Dialoge, Editors, Toolbars, Farb- und UI-Helfer.
- **SASD-Einschätzung:** Nur gezielt übernehmen; API- und Qualitätsprüfung pro Paket.
- **Quelle:** [https://github.com/Krypton-Suite/Extended-Toolkit](https://github.com/Krypton-Suite/Extended-Toolkit)

### D10. ReaLTaiizor

- **Plattformen:** WinForms
- **Lizenz:** MIT
- **Schwerpunkt:** Große Sammlung visuell unterschiedlicher Themes und Controls.
- **Komponenten/Funktionen:** Material-, Metro-, Hope-, Poison- und weitere Styles, Buttons, Eingaben, Tabs, Progress, Panels und Form-Themes.
- **SASD-Einschätzung:** Für Prototypen und Einzelcontrols; als alleinige SASD-Designbasis weniger konsistent als Krypton.
- **Quelle:** [https://github.com/Taiizor/ReaLTaiizor](https://github.com/Taiizor/ReaLTaiizor)

### D11. MaterialSkin.2

- **Plattformen:** WinForms
- **Lizenz:** MIT
- **Schwerpunkt:** Material-Design-Theming für WinForms.
- **Komponenten/Funktionen:** Forms, Buttons, Textfelder, Tabs, Drawer, Checkboxen, RadioButtons und ThemeManager.
- **SASD-Einschätzung:** Option für materialorientierte Tools, aber keine Enterprise-Vollsuite.
- **Quelle:** [https://github.com/leocb/MaterialSkin](https://github.com/leocb/MaterialSkin)

### D12. DockPanel Suite

- **Plattformen:** WinForms
- **Lizenz:** MIT
- **Schwerpunkt:** Visual-Studio-artiges Docking und Dokumentfenster.
- **Komponenten/Funktionen:** DockContent, Tool Windows, Document Tabs, Auto-Hide, Floating Windows und Layoutpersistenz.
- **SASD-Einschätzung:** Adapter für IDE-/Workbench-Oberflächen; nicht parallel mit einem zweiten Docking-System im selben Produktkern verwenden.
- **Quelle:** [https://github.com/dockpanelsuite/dockpanelsuite](https://github.com/dockpanelsuite/dockpanelsuite)

### D13. WPF UI

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Fluent-/Windows-11-artige WPF-Oberflächen.
- **Komponenten/Funktionen:** Navigation, FluentWindow, Dialoge, Snackbar, NumberBox, Themes, Mica/Backdrop-Helfer und Icons.
- **SASD-Einschätzung:** Gute moderne WPF-Shell-Basis.
- **Quelle:** [https://github.com/lepoco/wpfui](https://github.com/lepoco/wpfui)

### D14. MahApps.Metro

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Bewährtes modernes WPF-Theming und erweiterte Controls.
- **Komponenten/Funktionen:** MetroWindow, Flyouts, Dialoge, HamburgerMenu, NumericUpDown, Toggles, Tile und Styles.
- **SASD-Einschätzung:** Reif und dokumentiert; sinnvoll, wenn Fluent-Optik nicht zwingend ist.
- **Quelle:** [https://github.com/MahApps/MahApps.Metro](https://github.com/MahApps/MahApps.Metro)

### D15. MaterialDesignInXamlToolkit

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Material Design für WPF.
- **Komponenten/Funktionen:** Themes, DialogHost, DrawerHost, Cards, Snackbar, Chips, Pickers, Buttons und DataGrid-Styling.
- **SASD-Einschätzung:** Starker Stilbaukasten, aber mit SASD-Designsystem abstimmen.
- **Quelle:** [https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

### D16. HandyControl

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Umfangreiche WPF-Controlbibliothek mit vielen neu gestalteten Standard- und Spezialcontrols.
- **Komponenten/Funktionen:** Über 80 Custom Controls, Date/Time, Carousel, Steps, Pagination, Growl, Dialoge, Tabs, Panels und Styles.
- **SASD-Einschätzung:** Breiter OSS-Kandidat; Wartung, Dokumentationssprache und API-Stabilität vor Kernnutzung testen.
- **Quelle:** [https://github.com/HandyOrg/HandyControl](https://github.com/HandyOrg/HandyControl)

### D17. Fluent.Ribbon

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Office-artiges Ribbon für WPF.
- **Komponenten/Funktionen:** Ribbon, Tabs, Groups, Backstage, Quick Access Toolbar, Gallery und ScreenTips.
- **SASD-Einschätzung:** Spezialadapter für Office-artige Anwendungen.
- **Quelle:** [https://github.com/fluentribbon/Fluent.Ribbon](https://github.com/fluentribbon/Fluent.Ribbon)

### D18. AvalonDock

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** IDE-artiges Dokument- und ToolWindow-Docking.
- **Komponenten/Funktionen:** DockingManager, LayoutDocuments, Anchorables, Floating Windows, Layoutserialisierung, MVVM-Integration.
- **SASD-Einschätzung:** Bevorzugter OSS-Dockingkandidat für WPF.
- **Quelle:** [https://github.com/Dirkster99/AvalonDock](https://github.com/Dirkster99/AvalonDock)

### D19. Extended WPF Toolkit Community Edition

- **Plattformen:** WPF
- **Lizenz:** Microsoft Public License/Community-Edition; Paketdetails prüfen
- **Schwerpunkt:** Ergänzung fehlender WPF-Controls.
- **Komponenten/Funktionen:** PropertyGrid, CheckComboBox, ColorPicker, DateTimePicker, NumericUpDown, Wizard, BusyIndicator und weitere Editors.
- **SASD-Einschätzung:** Nützlich für einzelne Lücken; nicht mit dem kommerziellen Xceed DataGrid verwechseln.
- **Quelle:** [https://github.com/xceedsoftware/wpftoolkit](https://github.com/xceedsoftware/wpftoolkit)

### D20. PropertyTools

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Technische Daten- und Eigenschaftseditoren.
- **Komponenten/Funktionen:** PropertyGrid, DataGrid, TreeList/MultiSelectTree, ColorPicker, Datei-/Verzeichniswahl, Spinner und Formulare.
- **SASD-Einschätzung:** Sehr passend für SASD-Admin-, Konfigurations- und Entwicklerwerkzeuge.
- **Quelle:** [https://github.com/PropertyTools/PropertyTools](https://github.com/PropertyTools/PropertyTools)

### D21. FluentAvalonia

- **Plattformen:** Avalonia
- **Lizenz:** MIT
- **Schwerpunkt:** Fluent-Design-Controls und Themes für Avalonia.
- **Komponenten/Funktionen:** NavigationView, CommandBar, ContentDialog, NumberBox, InfoBar, TeachingTip und Fluent Theme.
- **SASD-Einschätzung:** Naheliegende Shell-Basis für Avalonia-Produkte.
- **Quelle:** [https://github.com/amwx/FluentAvalonia](https://github.com/amwx/FluentAvalonia)

### D22. SukiUI

- **Plattformen:** Avalonia
- **Lizenz:** MIT
- **Schwerpunkt:** Moderne Avalonia-UI-Bibliothek mit eigenem Design.
- **Komponenten/Funktionen:** Windows, Cards, Dialoge, Notifications, SideMenu, Tabs, Toggles und Themes.
- **SASD-Einschätzung:** Attraktiv für Dashboard- und Desktopwerkzeuge; Reifegrad im Pilotprojekt prüfen.
- **Quelle:** [https://github.com/kikipoulet/SukiUI](https://github.com/kikipoulet/SukiUI)

### D23. Semi.Avalonia

- **Plattformen:** Avalonia
- **Lizenz:** MIT
- **Schwerpunkt:** Komplettes Theme für native Avalonia-Controls nach Semi Design.
- **Komponenten/Funktionen:** Styles und Themes für Standardcontrols, Light/Dark und Design Tokens.
- **SASD-Einschätzung:** Designoption; zusätzliche Controls kommen über Ursa.
- **Quelle:** [https://github.com/irihitech/Semi.Avalonia](https://github.com/irihitech/Semi.Avalonia)

### D24. Ursa.Avalonia

- **Plattformen:** Avalonia
- **Lizenz:** MIT
- **Schwerpunkt:** Enterprise-orientierte Zusatzcontrols für Avalonia.
- **Komponenten/Funktionen:** Navigation, Dialoge, Formulare und weitere Geschäftsanwendungscontrols in Ergänzung zu Semi.Avalonia.
- **SASD-Einschätzung:** Wichtiger Kandidat für eine breitere Avalonia-Komponentenbasis.
- **Quelle:** [https://github.com/irihitech/Ursa.Avalonia](https://github.com/irihitech/Ursa.Avalonia)

### D25. ScottPlot

- **Plattformen:** WinForms, WPF, Avalonia, Blazor und weitere .NET-Ziele
- **Lizenz:** MIT
- **Schwerpunkt:** Schnelle technische und wissenschaftliche Diagramme.
- **Komponenten/Funktionen:** Line, Signal, Scatter, Bar, Heatmap, Finance, Histogram, Annotationen, Interaktion und Bildexport.
- **SASD-Einschätzung:** Bevorzugte SASD-Basis für Monitoring, Zeitreihen und technische Daten.
- **Quelle:** [https://github.com/ScottPlot/ScottPlot](https://github.com/ScottPlot/ScottPlot)

### D26. LiveCharts2

- **Plattformen:** WinForms, WPF, Avalonia, MAUI, Blazor, WinUI
- **Lizenz:** MIT
- **Schwerpunkt:** Animierte, moderne Cross-Platform-Charts.
- **Komponenten/Funktionen:** Cartesian, Pie/Donut, Polar, Gauges, GeoMaps, Tooltips und Animationen.
- **SASD-Einschätzung:** Bevorzugt für visuell ansprechende Dashboards; ScottPlot bleibt für große technische Datenmengen.
- **Quelle:** [https://github.com/beto-rodriguez/LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2)

### D27. OxyPlot

- **Plattformen:** WPF, WinForms und weitere .NET-Ziele
- **Lizenz:** MIT
- **Schwerpunkt:** Nüchterne wissenschaftliche Plotbibliothek.
- **Komponenten/Funktionen:** Linien, Flächen, Balken, Scatter, Heatmaps, Achsen, Annotationen und Export.
- **SASD-Einschätzung:** Stabile Alternative für analytische Darstellungen mit geringem Animationserfordernis.
- **Quelle:** [https://github.com/oxyplot/oxyplot](https://github.com/oxyplot/oxyplot)

### D28. Mapsui

- **Plattformen:** WPF, WinForms, Avalonia, MAUI, Uno, Blazor, WinUI
- **Lizenz:** MIT
- **Schwerpunkt:** Cross-Platform-Kartenkomponente für .NET.
- **Komponenten/Funktionen:** Raster-/Vektorkarten, Layer, Marker, Features, Projektionen, Zoom/Pan und Datenprovider.
- **SASD-Einschätzung:** Bevorzugter OSS-Kartenadapter für .NET.
- **Quelle:** [https://github.com/Mapsui/Mapsui](https://github.com/Mapsui/Mapsui)

### D29. Helix Toolkit

- **Plattformen:** WPF, WinUI, SharpDX-basierte Varianten
- **Lizenz:** MIT
- **Schwerpunkt:** 3D-Visualisierung für .NET.
- **Komponenten/Funktionen:** 3D-Viewport, Modelle, Kamera, Licht, Import, Manipulatoren und technische Szenen.
- **SASD-Einschätzung:** Nur als optionaler 3D-Adapter.
- **Quelle:** [https://github.com/helix-toolkit/helix-toolkit](https://github.com/helix-toolkit/helix-toolkit)

### D30. FastReport Open Source

- **Plattformen:** .NET, ASP.NET Core, Blazor-Viewer je nach Integration
- **Lizenz:** MIT für Open-Source-Kern; Designer-/Pro-Funktionen getrennt
- **Schwerpunkt:** Bandorientierte Berichtserstellung.
- **Komponenten/Funktionen:** Report Engine, Bands, Tabellen, Matrix, Charts, Barcodes, Datenquellen, Preview und Exporte je nach Paket.
- **SASD-Einschätzung:** Mögliche erste Reportingbasis, aber Designer- und Lizenzgrenzen dokumentieren.
- **Quelle:** [https://github.com/FastReports/FastReport](https://github.com/FastReports/FastReport)

### D31. QuestPDF

- **Plattformen:** .NET server- und desktopseitig
- **Lizenz:** Community-/kommerzielle Lizenz nach Organisationsgröße; nicht pauschal MIT
- **Schwerpunkt:** Codebasierte PDF-Dokumenterzeugung.
- **Komponenten/Funktionen:** Fluent Layout API, Tabellen, Text, Bilder, Header/Footer, Pagination, Vorschauwerkzeuge und PDF-Ausgabe.
- **SASD-Einschätzung:** Sehr geeignet für programmatische Berichte; Lizenz vor kommerzieller Verteilung prüfen.
- **Quelle:** [https://www.questpdf.com/](https://www.questpdf.com/)

### D32. PDFsharp / MigraDoc

- **Plattformen:** .NET
- **Lizenz:** MIT
- **Schwerpunkt:** PDF-Erzeugung und dokumentorientiertes Layout.
- **Komponenten/Funktionen:** PDF-Zeichnen, Dokumentmodell, Absätze, Tabellen, Bilder, Seitenlayout und Schriften.
- **SASD-Einschätzung:** Leichte OSS-Basis für PDF-Ausgabe ohne visuellen Designer.
- **Quelle:** [https://github.com/empira/PDFsharp](https://github.com/empira/PDFsharp)

### D33. AvalonEdit

- **Plattformen:** WPF
- **Lizenz:** MIT
- **Schwerpunkt:** Text- und Quellcodeeditor aus SharpDevelop.
- **Komponenten/Funktionen:** Syntax-Highlighting, Folding, Suche, Zeilennummern, Completion-Integration und große Dokumente.
- **SASD-Einschätzung:** Bevorzugter WPF-Codeeditor.
- **Quelle:** [https://github.com/icsharpcode/AvalonEdit](https://github.com/icsharpcode/AvalonEdit)

### D34. ScintillaNET

- **Plattformen:** WinForms
- **Lizenz:** MIT
- **Schwerpunkt:** WinForms-Wrapper für den Scintilla-Codeeditor.
- **Komponenten/Funktionen:** Syntaxfarben, Folding, Marker, Autocomplete, CallTips, Multi-Selection und Suche/Ersetzen.
- **SASD-Einschätzung:** Bevorzugter WinForms-Codeeditor.
- **Quelle:** [https://github.com/desjarlais/Scintilla.NET](https://github.com/desjarlais/Scintilla.NET)

### D35. ReoGrid

- **Plattformen:** WinForms, WPF in Varianten
- **Lizenz:** MIT für Community-Projektstände; Paket und Edition prüfen
- **Schwerpunkt:** Spreadsheet-Komponente für .NET.
- **Komponenten/Funktionen:** Zellen, Formeln, Formatierung, Freeze, Outline, Charts, Import/Export und Skriptfunktionen.
- **SASD-Einschätzung:** Interessanter Prototypkandidat; Aktivität, Formelfunktionalität und Dateikompatibilität gründlich testen.
- **Quelle:** [https://github.com/unvell/ReoGrid](https://github.com/unvell/ReoGrid)

### D36. SourceGrid

- **Plattformen:** WinForms
- **Lizenz:** MIT in verbreiteten Forks; konkreten Fork prüfen
- **Schwerpunkt:** Anpassbares zellenorientiertes Grid.
- **Komponenten/Funktionen:** Zelltypen, Editoren, virtuelle Daten, Selection, Formatting, Frozen Rows/Columns und Drag-and-drop.
- **SASD-Einschätzung:** Nur als Spezialgrid oder Ideenquelle; moderne .NET-Unterstützung vor Einsatz prüfen.
- **Quelle:** [https://github.com/siemens/sourcegrid](https://github.com/siemens/sourcegrid)

### D37. ObjectListView

- **Plattformen:** WinForms
- **Lizenz:** GPL/commerzielle Varianten je nach Distribution
- **Schwerpunkt:** Leistungsfähige objektgebundene ListView-Erweiterung.
- **Komponenten/Funktionen:** Grouping, Filtering, TreeListView, DataListView, Cell Editing, Checkboxes und Rendering.
- **SASD-Einschätzung:** Lizenz und Wartungsstand machen eine Aufnahme in den SASD-Kern unattraktiv; nur Vergleichsquelle.
- **Quelle:** [https://github.com/ojwoodford/ObjectListView](https://github.com/ojwoodford/ObjectListView)

### D38. CefSharp

- **Plattformen:** WinForms, WPF
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** Chromium-Einbettung in .NET-Desktopanwendungen.
- **Komponenten/Funktionen:** BrowserControl, JavaScript-Bindings, Request Handling, DevTools, Downloads und benutzerdefinierte Schemes.
- **SASD-Einschätzung:** Brücke für komplexe Webkomponenten im Desktop; hoher Ressourcen- und Sicherheitspflegebedarf.
- **Quelle:** [https://github.com/cefsharp/CefSharp](https://github.com/cefsharp/CefSharp)

### D39. Terminal.Gui

- **Plattformen:** Terminal/Console auf .NET
- **Lizenz:** MIT
- **Schwerpunkt:** Textbasierte Desktop-/Serveroberflächen.
- **Komponenten/Funktionen:** Windows, Dialoge, Menüs, Tabellen, TreeView, Eingaben, Layout und Maus/Tastatur.
- **SASD-Einschätzung:** Sinnvoll für SASD-Adminwerkzeuge auf Servern ohne grafische Oberfläche.
- **Quelle:** [https://github.com/gui-cs/Terminal.Gui](https://github.com/gui-cs/Terminal.Gui)


### D40. AntdUI

- **Plattformen:** WinForms auf .NET Framework 4.0/4.8 sowie .NET 8/10 laut Projektangabe
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Übertragung der Ant-Design-Sprache auf WinForms mit reinem GDI-Rendering, SVG, DPI-Anpassung und AOT-Unterstützung.
- **Layout:** Divider, StackPanel, FlowPanel, GridPanel und Splitter.
- **Navigation:** Breadcrumb, Dropdown, Menu, PageHeader, TabHeader, Pagination und Steps.
- **Dateneingabe:** Checkbox, ColorPicker, DatePicker, DatePickerRange, Input, InputNumber, Radio, Rate, Select, SelectNumber, Slider, SliderRange, Switch, TimePicker, Transfer und UploadDragger.
- **Datenanzeige:** Avatar, Badge, Calendar, Panel, Carousel, Collapse, Preview, ImagePreview, Popover, Segmented, Table, Tabs, Tag, Timeline, Tooltip, Tour, Tree sowie verschiedene Label-/LED-/Hyperlink-Controls und Chart.
- **Feedback:** Alert, Drawer, Message, Modal, Notification, Progress, Spin und Watermark.
- **Weitere Controls:** Message-/Chat-Listen, Battery, Signal, Shield, ContextMenuStrip, Image3D, Docking, Ribbon und OutlookBar laut aktueller Projektübersicht.
- **Plattformmerkmale:** hochwertige Kantenglättung, unterbrechbare Animationen, Emoji, Schatten, randlose Fenster mit nativen Funktionen, Light/Dark-Themes, Internationalisierung und interaktive Sicherheitszonen.
- **SASD-Einschätzung:** Sehr interessanter moderner Pilot neben Krypton. Vor einer Kernentscheidung Designer-Serialisierung, Accessibility/UI Automation, englische Dokumentation, Langzeitpflege und die Qualität komplexer Controls wie Table/Docking praktisch testen.
- **Quelle:** [https://github.com/AntdUI/AntdUI](https://github.com/AntdUI/AntdUI)

### D41. AcrylicUI

- **Plattformen:** .NET Core/.NET WinForms, Windows 11
- **Lizenz:** MIT
- **Schwerpunkt:** Modernes Control- und Docking-Framework mit Windows-11-Optik.
- **Komponenten/Funktionen:** Acrylic Panels, Dark/Modern Controls, Docking, randlose Fenster, Snap Layout, abgerundete Ecken, Schatten sowie High-DPI-Skalierung.
- **SASD-Einschätzung:** Attraktiver technischer Pilot für moderne Workbench-Oberflächen. Wegen kleinerem Ökosystem nicht ohne Designer-, Accessibility-, Stabilitäts- und Wartungstest als Basis verwenden.
- **Quelle:** [https://github.com/colhountech/AcrylicUI](https://github.com/colhountech/AcrylicUI)

### D42. KGySoft.WinForms

- **Plattformen:** WinForms von älteren .NET-Framework-Zielen bis zu modernen .NET-Windows-Zielen; Paket 5.0.1 wurde im Juli 2026 veröffentlicht
- **Lizenz:** Projektspezifische KGy-SOFT-Lizenz; nicht ohne Prüfung wie MIT/BSD behandeln
- **Schwerpunkt:** Fortgeschrittene Controls und Komponenten rund um `System.Windows.Forms`, insbesondere DPI, Lokalisierung, Dialoge und Grafik.
- **Komponenten/Funktionen:** `BaseForm` mit Command Bindings, MDI-Unterstützung und dynamischer Lokalisierung; Task Dialogs mit Kompatibilitätsmodus; verbesserte Varianten klassischer Controls; Image Viewer mit Panning/Zoom; High-DPI-/Mehrmonitor-Helfer; Visual-Style-Helfer; Screenshot-Funktionen und Integration mit den KGySoft-Drawing-Bibliotheken.
- **SASD-Einschätzung:** Technisch besonders interessant, weil es reale WinForms-Schwächen adressiert. Aufgrund kleinerer Verbreitung und eigener Lizenz zunächst als Spezialadapter testen, nicht blind als allgemeines Fundament übernehmen.
- **Quelle:** [https://github.com/koszeggy/KGySoft.WinForms](https://github.com/koszeggy/KGySoft.WinForms)

### D43. AdvancedDataGridView

- **Plattformen:** WinForms, .NET Framework sowie moderne .NET-Versionen
- **Lizenz:** MIT
- **Schwerpunkt:** Erweiterung des Standard-DataGridView um Excel-artige Filter- und Sortierfunktionen.
- **Komponenten/Funktionen:** Filtermenüs in Spaltenköpfen, Mehrfachsortierung, Filterstrings, BindingSource-Integration und Erweiterung vorhandener DataGridView-Modelle.
- **SASD-Einschätzung:** Guter Kandidat für einen `SasdDataGrid`-Prototyp, sofern High DPI, VirtualMode, große Datenmengen, benutzerdefinierte Zellen und Accessibility bestehen.
- **Quelle:** [https://github.com/davidegironi/advanceddatagridview](https://github.com/davidegironi/advanceddatagridview)

### D44. Ookii.Dialogs.WinForms

- **Plattformen:** WinForms, .NET Framework und moderne .NET-Versionen
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** Moderne Windows-Systemdialoge für WinForms.
- **Komponenten/Funktionen:** Task Dialog, Credential Dialog, Progress Dialog, Input Dialog sowie moderne Open-/Save-/Folder-Dialoge.
- **SASD-Einschätzung:** Sehr guter Spezialbaustein hinter einem `ISasdDialogService`; reduziert eigene Win32-Interop und verbessert die native Windows-Integration.
- **Quelle:** [https://github.com/ookii-dialogs/ookii-dialogs-winforms](https://github.com/ookii-dialogs/ookii-dialogs-winforms)

### D45. Cyotek.Windows.Forms.ImageBox

- **Plattformen:** WinForms
- **Lizenz:** MIT
- **Schwerpunkt:** Bildanzeige und interaktive Bildnavigation.
- **Komponenten/Funktionen:** Zoom, Scrollen, Panning, Bereichsauswahl, Pixel-/Rasterdarstellung, Overlays, virtuelle Darstellungsmodi und anpassbares Rendering.
- **SASD-Einschätzung:** Reifer Spezialkandidat für Screenshot-, Dokument-, Scan-, Medien- und Forschungswerkzeuge.
- **Quelle:** [https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox](https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox)

### D46. Cyotek Windows Forms Controls

- **Plattformen:** WinForms
- **Lizenz:** Je Projekt prüfen; zahlreiche Cyotek-Projekte verwenden MIT
- **Schwerpunkt:** Kleine spezialisierte Controls statt einer Vollsuite.
- **Komponenten/Funktionen:** ColorPicker-Controlsuite, TabList mit Design-Time-Unterstützung und weitere wiederverwendbare WinForms-Helfer aus den Cyotek-Open-Source-Projekten.
- **SASD-Einschätzung:** Einzelne Controls können Lücken schließen; Aktivität und Paketversion unterscheiden sich deutlich, daher nicht als geschlossene Suite behandeln.
- **Quelle:** [https://www.cyotek.com/open-source](https://www.cyotek.com/open-source)

### D47. Material3.WinForms

- **Plattformen:** WinForms, .NET Framework 4.7.2+ und modernes .NET je nach aktuellem Stand
- **Lizenz:** MIT laut Projektangabe; konkrete Preview-Version prüfen
- **Schwerpunkt:** Material Design 3 für WinForms.
- **Komponenten/Funktionen:** Dynamische Farben, Light/Dark, Typografieskala, Elevation, State Layers, Motion sowie Designerunterstützung im Aufbau.
- **SASD-Einschätzung:** Interessante Ideenquelle und Experiment; aufgrund des jungen Preview-Stands derzeit keine Kernabhängigkeit.
- **Quelle:** [https://github.com/robinrodricks/Material3.WinForms](https://github.com/robinrodricks/Material3.WinForms)

### D48. SunnyUI

- **Plattformen:** WinForms, .NET Framework 4+ sowie .NET 8/10 laut Projektangabe
- **Lizenz:** GPL-3.0 und zusätzliche Hinweise zur kommerziellen Nutzung; verbindlich juristisch prüfen
- **Schwerpunkt:** Umfangreiche Control-, Utility- und Mehrseiten-Frameworksammlung.
- **Komponenten/Funktionen:** 70+ Controls, Themes, Frames/Pages, Navigation, Eingaben, Tabellen-/Listenbausteine, Dialoge, Utilities und Beispielanwendungen.
- **SASD-Einschätzung:** Funktions- und Designquelle, aber wegen Lizenz-/Kommerzialisierungsfragen nicht als SASD-Kern einplanen.
- **Quelle:** [https://github.com/yhuse/SunnyUI](https://github.com/yhuse/SunnyUI)

### D49. Microsoft Edge WebView2

- **Plattformen:** WinForms, WPF, WinUI/Win32 und weitere Windows-Hosts
- **Lizenz/Runtime:** Microsoft SDK plus Edge-WebView2-Runtime; Distributions- und Updatevorgaben beachten
- **Schwerpunkt:** Einbettung moderner HTML/CSS/JavaScript-Oberflächen auf Basis der Microsoft-Edge-Runtime.
- **Komponenten/Funktionen:** WebView-Control, Navigation, JavaScript-Ausführung, Host Objects/Web Messages, DevTools, Download-/Request-/Permission-Ereignisse, virtuelle Hostnamen, Profile und Runtime-Verteilung.
- **SASD-Einschätzung:** Bevorzugte Desktop-Webbrücke vor CefSharp. Geeignet für komplexe HTML-/Markdown-Editoren, Hilfesysteme, Dashboards und wiederverwendete Webkomponenten; Sicherheits- und Updatekonzept ist Pflicht.
- **Quelle:** [https://learn.microsoft.com/microsoft-edge/webview2/](https://learn.microsoft.com/microsoft-edge/webview2/)

### D50. Awesome .NET WinForms Libraries

- **Plattformen:** Discovery-Katalog für WinForms
- **Lizenz:** Keine gemeinsame Lizenz; jedes verlinkte Projekt separat prüfen
- **Schwerpunkt:** Kuratierte Sammlung frei verfügbarer WinForms-Bibliotheken.
- **Komponenten/Funktionen:** Verweise auf UI-Frameworks, Dialoge, Grid-/List-/Tree-Erweiterungen, Bildcontrols, Drag-and-drop, Testing, Hosting und Hilfsbibliotheken.
- **SASD-Einschätzung:** Wertvolle Recherchequelle, aber kein Lieferant und keine Abhängigkeit. Nur gepflegte Kandidaten mit Primärquellen in den Hauptkatalog übernehmen.
- **Quelle:** [https://github.com/tbolon/awesome-dotnet-winforms](https://github.com/tbolon/awesome-dotnet-winforms)


## Open-Source-, Open-Core- und Community-Komponenten für Web/AJAX

Die Webliste trennt bewusst vollständige Framework-Komponenten, Designsysteme, Headless Primitives und Spezialbibliotheken. Eine CSS-Bibliothek ersetzt kein DataGrid; ein Headless Primitive ersetzt kein fertiges Theme; ein Grid ersetzt keine Anwendungs-Shell.

### W01. MudBlazor

- **Plattformen:** Blazor
- **Lizenz:** MIT
- **Schwerpunkt:** Material-Design-Komponentensuite mit überwiegend C#-Implementierung.
- **Komponenten/Funktionen:** DataGrid/Table, Formulare, Dialoge, Navigation, Charts, Pickers, TreeView, Tabs, Snackbar, Upload und Layout.
- **SASD-Einschätzung:** Stärkster allgemeiner Kandidat für eine SASD-Blazor-Basis.
- **Quelle:** [https://github.com/MudBlazor/MudBlazor](https://github.com/MudBlazor/MudBlazor)

### W02. Radzen Blazor Components

- **Plattformen:** Blazor
- **Lizenz:** MIT
- **Schwerpunkt:** Sehr breite native Blazor-Bibliothek.
- **Komponenten/Funktionen:** 145+ Komponenten einschließlich DataGrid, Scheduler, Charts, Formulare, Tree, Kanban-nahe Bausteine, Themes und Accessibility.
- **SASD-Einschätzung:** Breitester freier Blazor-Funktionskandidat; API, Styling und Supportmodell gegen MudBlazor vergleichen.
- **Quelle:** [https://github.com/radzenhq/radzen-blazor](https://github.com/radzenhq/radzen-blazor)

### W03. Microsoft Fluent UI Blazor

- **Plattformen:** Blazor
- **Lizenz:** MIT
- **Schwerpunkt:** Fluent-Design-Web-Components für Blazor.
- **Komponenten/Funktionen:** Buttons, Inputs, DataGrid, Dialog, Menu, Tabs, TreeView, Toast, Layout und Design Tokens.
- **SASD-Einschätzung:** Gute Wahl für Microsoft-nahe, zugängliche Geschäftsanwendungen; Produktstatus regelmäßig prüfen.
- **Quelle:** [https://github.com/microsoft/fluentui-blazor](https://github.com/microsoft/fluentui-blazor)

### W04. Ant Design Blazor

- **Plattformen:** Blazor
- **Lizenz:** MIT
- **Schwerpunkt:** Enterprise-Komponenten auf Basis des Ant-Design-Systems.
- **Komponenten/Funktionen:** Table, Tree, Form, Select, DatePicker, Upload, Modal, Drawer, Menu, Steps, Tabs, Descriptions, Charts-Integration und Pro-Layout.
- **SASD-Einschätzung:** Stark für klassische Enterprise-Oberflächen und umfangreiche Formulare.
- **Quelle:** [https://github.com/ant-design-blazor/ant-design-blazor](https://github.com/ant-design-blazor/ant-design-blazor)

### W05. HAVIT Blazor Bootstrap

- **Plattformen:** Blazor/Bootstrap 5
- **Lizenz:** MIT
- **Schwerpunkt:** Freie Bootstrap-Komponenten plus Projektvorlagen.
- **Komponenten/Funktionen:** Grid, Date/Time, Autosuggest, Tags, Formulare, Modal, Offcanvas, Navigation und Utility-Komponenten.
- **SASD-Einschätzung:** Pragmatische, gut integrierbare Basis für SASD-Business-Webapps.
- **Quelle:** [https://github.com/havit/Havit.Blazor](https://github.com/havit/Havit.Blazor)

### W06. BootstrapBlazor

- **Plattformen:** Blazor/Bootstrap
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Enterprise-orientierte Blazor-Komponentenbibliothek.
- **Komponenten/Funktionen:** Table, Tree, Form, Editors, Upload, Charts, Layout, Dialoge, Notifications und Admin-Komponenten.
- **SASD-Einschätzung:** Umfangreicher Kandidat; Dokumentation, Community und Internationalisierung im Pilot prüfen.
- **Quelle:** [https://github.com/dotnetcore/BootstrapBlazor](https://github.com/dotnetcore/BootstrapBlazor)

### W07. Blazor Bootstrap

- **Plattformen:** Blazor/Bootstrap 5
- **Lizenz:** MIT
- **Schwerpunkt:** Leichtgewichtige responsive Bootstrap-Komponenten.
- **Komponenten/Funktionen:** Grid, Charts, Modal, Offcanvas, Accordion, Toast, AutoComplete, Date/Time und Navigation.
- **SASD-Einschätzung:** Geeignet, wenn eine schlanke Bootstrap-Basis genügt.
- **Quelle:** [https://github.com/vikramlearning/blazorbootstrap](https://github.com/vikramlearning/blazorbootstrap)

### W08. Blazorise

- **Plattformen:** Blazor mit Bootstrap, Tailwind, Bulma, AntDesign, Material
- **Lizenz:** Dual-/Community-/kommerzielles Modell; nicht pauschal als vollständig MIT behandeln
- **Schwerpunkt:** Abstraktionsschicht über mehrere CSS-Provider.
- **Komponenten/Funktionen:** 100+ Form-, Daten-, Navigations-, Chart-, Scheduling- und Medienkomponenten.
- **SASD-Einschätzung:** Architektonisch interessant, aber Lizenzmodell und Provider-Abstraktion erhöhen Komplexität.
- **Quelle:** [https://github.com/Megabit/Blazorise](https://github.com/Megabit/Blazorise)

### W09. Bootstrap

- **Plattformen:** HTML/CSS/JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Weit verbreitetes responsives CSS- und JS-Framework.
- **Komponenten/Funktionen:** Grid/Layout, Buttons, Formulare, Navbars, Cards, Modal, Dropdown, Collapse, Carousel, Toast und Utilities.
- **SASD-Einschätzung:** Solide Webbasis, aber ohne Enterprise-DataGrid, Scheduler oder Diagramm-Engine.
- **Quelle:** [https://github.com/twbs/bootstrap](https://github.com/twbs/bootstrap)

### W10. Tailwind CSS

- **Plattformen:** CSS Build Tool
- **Lizenz:** MIT
- **Schwerpunkt:** Utility-first Styling statt fertiger Laufzeitkomponenten.
- **Komponenten/Funktionen:** Layout-, Spacing-, Typografie-, Farb-, Responsive- und State-Utilities.
- **SASD-Einschätzung:** Sehr geeignet als Designsystem-Unterbau; Komponentenverhalten muss separat implementiert werden.
- **Quelle:** [https://github.com/tailwindlabs/tailwindcss](https://github.com/tailwindlabs/tailwindcss)

### W11. daisyUI

- **Plattformen:** Tailwind CSS
- **Lizenz:** MIT
- **Schwerpunkt:** Semantische Komponentenklassen und Themes für Tailwind.
- **Komponenten/Funktionen:** Buttons, Formulare, Cards, Navbar, Drawer, Modal, Tabs, Steps, Chat, Timeline, Table, Dropdown und Themes.
- **SASD-Einschätzung:** Bevorzugte leichte Web-Designbasis für SASD-JavaScript- oder Blazor-Projekte mit Tailwind.
- **Quelle:** [https://github.com/saadeghi/daisyui](https://github.com/saadeghi/daisyui)

### W12. Flowbite

- **Plattformen:** Tailwind CSS, JS und Framework-Wrapper
- **Lizenz:** MIT für Kern; Pro-Produkte separat
- **Schwerpunkt:** Interaktive Tailwind-Komponenten.
- **Komponenten/Funktionen:** Modal, Dropdown, Datepicker, Navbar, Sidebar, Tabs, Carousel, Forms und Dashboard-Bausteine.
- **SASD-Einschätzung:** Alternative zu daisyUI, wenn mehr fertiges JavaScript-Verhalten gewünscht ist.
- **Quelle:** [https://github.com/themesberg/flowbite](https://github.com/themesberg/flowbite)

### W13. MUI Core

- **Plattformen:** React
- **Lizenz:** MIT
- **Schwerpunkt:** Material-Design-Komponenten für React.
- **Komponenten/Funktionen:** Formulare, Navigation, Dialoge, Layout, Feedback, Data Display und Theming.
- **SASD-Einschätzung:** Reifer React-Baukasten; MUI X als Open-Core separat bewerten.
- **Quelle:** [https://github.com/mui/material-ui](https://github.com/mui/material-ui)

### W14. MUI X

- **Plattformen:** React
- **Lizenz:** Community MIT plus Pro/Premium kommerziell
- **Schwerpunkt:** Erweiterte Daten- und Planungscomponents.
- **Komponenten/Funktionen:** Data Grid, Date/Time Pickers, Charts, Tree View und Scheduler; fortgeschrittene Funktionen teilweise kostenpflichtig.
- **SASD-Einschätzung:** Gutes Beispiel für Open-Core-Produktgrenzen.
- **Quelle:** [https://mui.com/x/](https://mui.com/x/)

### W15. Ant Design

- **Plattformen:** React; Designsystem mit Portierungen
- **Lizenz:** MIT
- **Schwerpunkt:** Enterprise-Designsystem und umfangreiche React-Komponenten.
- **Komponenten/Funktionen:** Table, Tree, Form, Select, DatePicker, Upload, Modal, Drawer, Menu, Steps, Tabs, Descriptions und Feedback.
- **SASD-Einschätzung:** Starke Referenz für konsistente Enterprise-Patterns.
- **Quelle:** [https://github.com/ant-design/ant-design](https://github.com/ant-design/ant-design)

### W16. Chakra UI

- **Plattformen:** React
- **Lizenz:** MIT
- **Schwerpunkt:** Zugängliche, composable React-Komponenten.
- **Komponenten/Funktionen:** Formulare, Layout, Overlays, Navigation, Feedback und Theme-System.
- **SASD-Einschätzung:** Gute Accessibility- und Composability-Referenz, aber weniger Datenkomponenten.
- **Quelle:** [https://github.com/chakra-ui/chakra-ui](https://github.com/chakra-ui/chakra-ui)

### W17. Mantine

- **Plattformen:** React
- **Lizenz:** MIT
- **Schwerpunkt:** Breite React-Komponenten- und Hooks-Suite.
- **Komponenten/Funktionen:** Formulare, Data Display, Navigation, Overlays, Dates, Notifications, Rich Text Integration und Hooks.
- **SASD-Einschätzung:** Sehr produktive React-Basis; Enterprise-Grid separat ergänzen.
- **Quelle:** [https://github.com/mantinedev/mantine](https://github.com/mantinedev/mantine)

### W18. shadcn/ui

- **Plattformen:** React und Portierungen
- **Lizenz:** MIT
- **Schwerpunkt:** Kopierbarer Komponentenquellcode statt klassischer Paketabhängigkeit.
- **Komponenten/Funktionen:** Formulare, Dialoge, Dropdown, Navigation, Data Table Patterns, Charts, Sidebar, Calendar und Command Palette.
- **SASD-Einschätzung:** Architektonisch interessant für SASD: kontrollierter Quellcodebesitz ohne Mega-Fork, aber Updates müssen bewusst übernommen werden.
- **Quelle:** [https://github.com/shadcn-ui/ui](https://github.com/shadcn-ui/ui)

### W19. Radix UI

- **Plattformen:** React
- **Lizenz:** MIT
- **Schwerpunkt:** Headless, zugängliche UI-Primitives.
- **Komponenten/Funktionen:** Dialog, Dropdown, Menu, Tabs, Tooltip, Popover, Select, Accordion, Toast und Focus Management.
- **SASD-Einschätzung:** Sehr gute Basis für eigene Designs und Accessibility.
- **Quelle:** [https://github.com/radix-ui/primitives](https://github.com/radix-ui/primitives)

### W20. Headless UI

- **Plattformen:** React, Vue
- **Lizenz:** MIT
- **Schwerpunkt:** Unstyled zugängliche Verhaltenskomponenten.
- **Komponenten/Funktionen:** Dialog, Disclosure, Listbox, Menu, Popover, Tabs, Combobox, Switch und Transition.
- **SASD-Einschätzung:** Ideal mit Tailwind, wenn SASD die visuelle Kontrolle behalten will.
- **Quelle:** [https://github.com/tailwindlabs/headlessui](https://github.com/tailwindlabs/headlessui)

### W21. Ark UI

- **Plattformen:** React, Vue, Solid, Svelte
- **Lizenz:** MIT
- **Schwerpunkt:** Headless, frameworkübergreifende State-Machines und Primitives.
- **Komponenten/Funktionen:** Accordion, Combobox, DatePicker, Dialog, Menu, Select, Slider, Tabs, Toast, TreeView und weitere.
- **SASD-Einschätzung:** Interessant für einen gemeinsamen verhaltensorientierten Webkern.
- **Quelle:** [https://github.com/chakra-ui/ark](https://github.com/chakra-ui/ark)

### W22. React Aria Components

- **Plattformen:** React
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Accessibility und internationales Verhalten.
- **Komponenten/Funktionen:** Buttons, Collections, Calendar, DatePicker, ComboBox, Menu, Table, Tree, Overlays und Focus Management.
- **SASD-Einschätzung:** Referenz für barrierefreie Komponentenverträge und Tastaturmodelle.
- **Quelle:** [https://github.com/adobe/react-spectrum](https://github.com/adobe/react-spectrum)

### W23. Carbon Design System

- **Plattformen:** React, Web Components und Designressourcen
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** IBM-Designsystem für komplexe Unternehmensprodukte.
- **Komponenten/Funktionen:** Formulare, DataTable, Navigation, Modal, Notifications, Charts-Ökosystem und Design Tokens.
- **SASD-Einschätzung:** Gute Governance- und Designsystemreferenz.
- **Quelle:** [https://github.com/carbon-design-system/carbon](https://github.com/carbon-design-system/carbon)

### W24. PatternFly

- **Plattformen:** React und Designsystem
- **Lizenz:** MIT
- **Schwerpunkt:** Enterprise- und Infrastruktur-UIs, ursprünglich aus Red Hat.
- **Komponenten/Funktionen:** DataTable, Topology, Wizard, Navigation, Cards, Forms, Charts und Dashboard-Patterns.
- **SASD-Einschätzung:** Besonders passend als Referenz für SASD-Systemadministrationsoberflächen.
- **Quelle:** [https://github.com/patternfly/patternfly-react](https://github.com/patternfly/patternfly-react)

### W25. Fluent UI React

- **Plattformen:** React und Web Components
- **Lizenz:** MIT
- **Schwerpunkt:** Microsoft Fluent Design für Webprodukte.
- **Komponenten/Funktionen:** Formulare, DataGrid/Table, Navigation, Dialoge, Popover, Tree, Toolbar und Design Tokens.
- **SASD-Einschätzung:** Referenz für Windows-nahe Weboberflächen und Accessibility.
- **Quelle:** [https://github.com/microsoft/fluentui](https://github.com/microsoft/fluentui)

### W26. PrimeNG / PrimeReact / PrimeVue

- **Plattformen:** Angular, React, Vue
- **Lizenz:** Bestehende MIT-Versionen; angekündigte künftige Hauptversionen mit geändertem Modell – jeweils konkret prüfen
- **Schwerpunkt:** Breite Komponentenfamilien für mehrere Frameworks.
- **Komponenten/Funktionen:** DataTable, TreeTable, Charts, Calendar, Gantt-nahe Timeline, Forms, Upload, Editor, Menu, Dialog, PickList und Themes.
- **SASD-Einschätzung:** Funktionsstark, aber wegen Lizenzstrategie nicht als dauerhaft unveränderliche OSS-Grundlage voraussetzen.
- **Quelle:** [https://www.primefaces.org/](https://www.primefaces.org/)

### W27. AG Grid Community

- **Plattformen:** JavaScript, React, Angular, Vue
- **Lizenz:** MIT
- **Schwerpunkt:** Professionelles freies Datagrid mit optionaler Enterprise-Erweiterung.
- **Komponenten/Funktionen:** Sortierung, Filterung, Pagination, Editing, Custom Cells, Theming und Framework-Wrapper.
- **SASD-Einschätzung:** Bevorzugter vollständiger Web-Grid-Kandidat, sofern Community-Funktionen reichen.
- **Quelle:** [https://github.com/ag-grid/ag-grid](https://github.com/ag-grid/ag-grid)

### W28. TanStack Table

- **Plattformen:** TypeScript/JavaScript, React, Vue, Solid, Svelte, Qwik u.a.
- **Lizenz:** MIT
- **Schwerpunkt:** Headless Tabellen- und Datagrid-Engine.
- **Komponenten/Funktionen:** Rows/Columns, Sorting, Filtering, Grouping, Pagination, Selection, Expansion und kontrollierter State.
- **SASD-Einschätzung:** Bevorzugt, wenn SASD Markup, Styling und Interaktion selbst definieren will.
- **Quelle:** [https://github.com/TanStack/table](https://github.com/TanStack/table)

### W29. Tabulator

- **Plattformen:** Vanilla JS und Framework-Integration
- **Lizenz:** MIT
- **Schwerpunkt:** Feature-reiches eigenständiges Web-DataGrid.
- **Komponenten/Funktionen:** Ajax, Virtual DOM, Editing, Grouping, Filter, Tree, Pagination, Print, Download und Formatters.
- **SASD-Einschätzung:** Starker frameworkneutraler Kandidat für Admin- und Datenwerkzeuge.
- **Quelle:** [https://github.com/olifolkerd/tabulator](https://github.com/olifolkerd/tabulator)

### W30. Grid.js

- **Plattformen:** Vanilla JS, React, Angular, Vue
- **Lizenz:** MIT
- **Schwerpunkt:** Leichtgewichtiges Table-Plugin.
- **Komponenten/Funktionen:** Sortierung, Suche, Pagination, Serverdaten, Plugins und Custom Cells.
- **SASD-Einschätzung:** Geeignet für einfache Tabellen, nicht für ein Enterprise-Grid-Ziel.
- **Quelle:** [https://github.com/grid-js/gridjs](https://github.com/grid-js/gridjs)

### W31. DataTables

- **Plattformen:** HTML/JavaScript, React/Vue-Integration
- **Lizenz:** MIT; Editor-Erweiterung kommerziell
- **Schwerpunkt:** Progressive Erweiterung vorhandener HTML-Tabellen.
- **Komponenten/Funktionen:** Paging, Search, Multi-column Sort, Responsive, Buttons, Export und viele Extensions.
- **SASD-Einschätzung:** Gut für klassische Weboberflächen; jQuery-Historie und Extension-Lizenzen beachten.
- **Quelle:** [https://datatables.net/](https://datatables.net/)

### W32. Chart.js

- **Plattformen:** JavaScript und Wrapper
- **Lizenz:** MIT
- **Schwerpunkt:** Einfach zugängliche Canvas-Charts.
- **Komponenten/Funktionen:** Line, Bar, Pie, Doughnut, Radar, Polar, Bubble, Scatter, Mixed Charts, Plugins und Animation.
- **SASD-Einschätzung:** Gute Standarddiagrammbasis für kleinere Dashboards.
- **Quelle:** [https://github.com/chartjs/Chart.js](https://github.com/chartjs/Chart.js)

### W33. Apache ECharts

- **Plattformen:** JavaScript und Wrapper
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Breite, leistungsfähige Canvas/SVG-Datenvisualisierung.
- **Komponenten/Funktionen:** Über 20 Diagrammtypen, große Daten, progressive Darstellung, Geo, Graph, Sankey, Heatmap, Treemap, Gauge und DataZoom.
- **SASD-Einschätzung:** Bevorzugte umfassende OSS-Web-Chartengine.
- **Quelle:** [https://github.com/apache/echarts](https://github.com/apache/echarts)

### W34. Plotly.js

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Wissenschaftliche, statistische und 3D-Diagramme.
- **Komponenten/Funktionen:** Über 40 Typen, 3D, Statistical, Financial, Maps, WebGL/SVG und Interaktion.
- **SASD-Einschätzung:** Spezialadapter für wissenschaftliche und analytische Weboberflächen.
- **Quelle:** [https://github.com/plotly/plotly.js](https://github.com/plotly/plotly.js)

### W35. D3.js

- **Plattformen:** JavaScript
- **Lizenz:** ISC
- **Schwerpunkt:** Low-Level-Bausteine für maßgeschneiderte Visualisierungen.
- **Komponenten/Funktionen:** Selections, Scales, Shapes, Axes, Hierarchy, Geo, Force, Zoom, Drag und Data Transformations.
- **SASD-Einschätzung:** Keine fertige Business-Chart-Suite; nutzen, wenn Standardbibliotheken nicht ausreichen.
- **Quelle:** [https://github.com/d3/d3](https://github.com/d3/d3)

### W36. Vega / Vega-Lite

- **Plattformen:** JavaScript und JSON-Spezifikation
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** Deklarative Grammatik für Visualisierungen.
- **Komponenten/Funktionen:** Marks, Encoding, Transformations, Scales, Interaktionen, Facets und Komposition.
- **SASD-Einschätzung:** Interessant für konfigurierbare Dashboards und gespeicherte Visualisierungsspezifikationen.
- **Quelle:** [https://github.com/vega/vega-lite](https://github.com/vega/vega-lite)

### W37. FullCalendar Standard

- **Plattformen:** JavaScript, React, Angular, Vue
- **Lizenz:** MIT für Standardkern; Premium Scheduler separat
- **Schwerpunkt:** Ereigniskalender und Drag-and-drop-Scheduling.
- **Komponenten/Funktionen:** Month/Week/Day/List Views, Events, Drag, Resize, Recurrence-Integration und Plugins.
- **SASD-Einschätzung:** Bevorzugter OSS-Kalenderkern; Ressourcen-/Timeline-Scheduler ist Premium.
- **Quelle:** [https://github.com/fullcalendar/fullcalendar](https://github.com/fullcalendar/fullcalendar)

### W38. DHTMLX Gantt Community

- **Plattformen:** JavaScript und Framework-Integration
- **Lizenz:** MIT
- **Schwerpunkt:** Freie Basis für Projektzeitpläne.
- **Komponenten/Funktionen:** Task Grid, Timeline, Projekte, Milestones, Abhängigkeiten, Drag-and-drop, Zoom, Export und Lokalisierungen.
- **SASD-Einschätzung:** Erster Gantt-Prototypkandidat; fortgeschrittene Pro-Funktionen nicht voraussetzen.
- **Quelle:** [https://github.com/DHTMLX/gantt](https://github.com/DHTMLX/gantt)

### W39. Frappe Gantt

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Leichtgewichtiges interaktives Gantt.
- **Komponenten/Funktionen:** Tasks, Dependencies, Progress, View Modes, Drag und Custom Popup.
- **SASD-Einschätzung:** Für einfache Gantt-Darstellung, nicht für Ressourcenplanung.
- **Quelle:** [https://github.com/frappe/gantt](https://github.com/frappe/gantt)

### W40. Tiptap Core

- **Plattformen:** JavaScript, React, Vue
- **Lizenz:** MIT für Kern; Cloud/Kollaboration/AI teilweise kommerziell
- **Schwerpunkt:** Headless Rich-Text-Editor auf ProseMirror.
- **Komponenten/Funktionen:** 100+ Extensions, Schema, Commands, Tables, Mentions, Collaboration-Anbindung und frei gestaltbare UI.
- **SASD-Einschätzung:** Bevorzugter moderner Web-Editor, wenn SASD die Oberfläche selbst bauen will.
- **Quelle:** [https://github.com/ueberdosis/tiptap](https://github.com/ueberdosis/tiptap)

### W41. ProseMirror

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Modularer Kern für strukturierte Rich-Text-Editoren.
- **Komponenten/Funktionen:** Document Model, State, Transactions, Commands, History, Input Rules, Tables und Collaboration-Grundlagen.
- **SASD-Einschätzung:** Technische Basis, aber hoher Eigenentwicklungsaufwand für eine fertige Editor-UX.
- **Quelle:** [https://github.com/ProseMirror/prosemirror](https://github.com/ProseMirror/prosemirror)

### W42. Quill

- **Plattformen:** JavaScript
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** Fertiger modularer WYSIWYG-Webeditor.
- **Komponenten/Funktionen:** Toolbar, Formatierung, Embeds, Delta-Datenmodell, Events, Themes und Custom Formats.
- **SASD-Einschätzung:** Einfacher als Tiptap, aber weniger flexibel für sehr strukturierte Editoren.
- **Quelle:** [https://github.com/slab/quill](https://github.com/slab/quill)

### W43. Lexical

- **Plattformen:** JavaScript/React
- **Lizenz:** MIT
- **Schwerpunkt:** Erweiterbares Editorframework von Meta.
- **Komponenten/Funktionen:** Editor State, Nodes, Commands, History, Rich Text, Lists, Tables, Collaboration-Plugins und Accessibility.
- **SASD-Einschätzung:** Starker React-Kandidat für komplexe Editoren.
- **Quelle:** [https://github.com/facebook/lexical](https://github.com/facebook/lexical)

### W44. Editor.js

- **Plattformen:** JavaScript
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Blockbasierter Editor mit strukturiertem JSON-Output.
- **Komponenten/Funktionen:** Paragraph, Header, List, Table, Image, Quote und Plugin-API.
- **SASD-Einschätzung:** Geeignet für Wissensdatenbanken und modulare Inhalte.
- **Quelle:** [https://github.com/codex-team/editor.js](https://github.com/codex-team/editor.js)

### W45. CodeMirror 6

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Modularer Codeeditor für den Browser.
- **Komponenten/Funktionen:** Syntax, Linting, Autocomplete, Search, Folding, Multiple Languages, State und Extensions.
- **SASD-Einschätzung:** Bevorzugter leichter Web-Codeeditor.
- **Quelle:** [https://github.com/codemirror/dev](https://github.com/codemirror/dev)

### W46. Monaco Editor

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Browsereditor aus Visual Studio Code.
- **Komponenten/Funktionen:** IntelliSense, Syntax, Diff, Minimap, Multi-Cursor, Folding, Diagnostics und Language Services.
- **SASD-Einschätzung:** Bevorzugt für IDE-nahe SASD-Webwerkzeuge; relativ großer Footprint.
- **Quelle:** [https://github.com/microsoft/monaco-editor](https://github.com/microsoft/monaco-editor)

### W47. Mermaid

- **Plattformen:** JavaScript/Markdown-Integration
- **Lizenz:** MIT
- **Schwerpunkt:** Diagramme aus Textdefinitionen.
- **Komponenten/Funktionen:** Flowchart, Sequence, Class, State, ER, Gantt, Journey, Git Graph, Mindmap und Timeline.
- **SASD-Einschätzung:** Sehr wertvoll für Dokumentation und generierte technische Diagramme.
- **Quelle:** [https://github.com/mermaid-js/mermaid](https://github.com/mermaid-js/mermaid)

### W48. React Flow / XYFlow

- **Plattformen:** React, Svelte
- **Lizenz:** MIT für Kern; Pro-Dienste separat
- **Schwerpunkt:** Node-basierte interaktive Diagramme und Editoren.
- **Komponenten/Funktionen:** Nodes, Edges, Handles, Zoom/Pan, Selection, MiniMap, Controls und Custom Nodes.
- **SASD-Einschätzung:** Gute Basis für Workflow-, Netzwerk- und Pipeline-Designer.
- **Quelle:** [https://github.com/xyflow/xyflow](https://github.com/xyflow/xyflow)

### W49. Cytoscape.js

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Graphvisualisierung und -analyse.
- **Komponenten/Funktionen:** Graph Layouts, Styling, Events, Selection, Algorithms, Compound Nodes und Extensions.
- **SASD-Einschätzung:** Bevorzugt für Netzwerke, Abhängigkeiten und Topologien.
- **Quelle:** [https://github.com/cytoscape/cytoscape.js](https://github.com/cytoscape/cytoscape.js)

### W50. bpmn-js

- **Plattformen:** JavaScript
- **Lizenz:** bpmn.io License/MIT-nahe Komponenten; Bedingungen prüfen
- **Schwerpunkt:** BPMN-2.0-Viewer und -Modeler.
- **Komponenten/Funktionen:** BPMN Rendering, Editing, Palette, Properties-Integration, Import/Export und Extensions.
- **SASD-Einschätzung:** Spezialadapter für Prozessmodellierung.
- **Quelle:** [https://github.com/bpmn-io/bpmn-js](https://github.com/bpmn-io/bpmn-js)

### W51. Leaflet

- **Plattformen:** JavaScript
- **Lizenz:** BSD-2-Clause
- **Schwerpunkt:** Leichtgewichtige interaktive 2D-Webkarten.
- **Komponenten/Funktionen:** Tile Layers, Marker, Popups, GeoJSON, Events, Zoom/Pan und großes Plugin-Ökosystem.
- **SASD-Einschätzung:** Bevorzugte einfache Webkartenbasis.
- **Quelle:** [https://github.com/Leaflet/Leaflet](https://github.com/Leaflet/Leaflet)

### W52. OpenLayers

- **Plattformen:** JavaScript
- **Lizenz:** BSD-2-Clause
- **Schwerpunkt:** Umfangreiche Web-GIS-Bibliothek.
- **Komponenten/Funktionen:** Raster/Vector, Projektionen, OGC-Dienste, Editing, Styling, Interactions und komplexe Layer.
- **SASD-Einschätzung:** Für anspruchsvollere GIS-Anwendungen als Leaflet.
- **Quelle:** [https://github.com/openlayers/openlayers](https://github.com/openlayers/openlayers)

### W53. MapLibre GL JS

- **Plattformen:** JavaScript/WebGL
- **Lizenz:** BSD-3-Clause
- **Schwerpunkt:** GPU-beschleunigte Vektorkarten.
- **Komponenten/Funktionen:** Vector Tiles, Styles, 3D Terrain/Buildings, Interaktion, Sources, Layers und Controls.
- **SASD-Einschätzung:** Bevorzugt für moderne Vektorkarten und große Geodaten.
- **Quelle:** [https://github.com/maplibre/maplibre-gl-js](https://github.com/maplibre/maplibre-gl-js)

### W54. Uppy

- **Plattformen:** JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Modularer Dateiupload.
- **Komponenten/Funktionen:** Dashboard, DragDrop, Webcam, Chunking/Tus, Cloud-Quellen, StatusBar und Plugins.
- **SASD-Einschätzung:** Bevorzugte Uploadbasis für komplexe Webworkflows.
- **Quelle:** [https://github.com/transloadit/uppy](https://github.com/transloadit/uppy)

### W55. FilePond

- **Plattformen:** JavaScript und Wrapper
- **Lizenz:** MIT für Core; einzelne Plugins/Lizenzen prüfen
- **Schwerpunkt:** Benutzerfreundlicher Datei- und Bildupload.
- **Komponenten/Funktionen:** DragDrop, Preview, Validation, Chunking, Image Transform und Plugins.
- **SASD-Einschätzung:** Gute Alternative für kompakte Uploadkomponenten.
- **Quelle:** [https://github.com/pqina/filepond](https://github.com/pqina/filepond)

### W56. SortableJS

- **Plattformen:** JavaScript und Wrapper
- **Lizenz:** MIT
- **Schwerpunkt:** Drag-and-drop-Sortierung von Listen und Grids.
- **Komponenten/Funktionen:** Reorder, Cross-list Drag, Handles, Groups, Cloning und Events.
- **SASD-Einschätzung:** Nützlicher Verhaltensbaustein für Kanban und konfigurierbare Listen.
- **Quelle:** [https://github.com/SortableJS/Sortable](https://github.com/SortableJS/Sortable)

### W57. SweetAlert2

- **Plattformen:** JavaScript und Wrapper
- **Lizenz:** MIT
- **Schwerpunkt:** Moderne modale Alerts und Bestätigungen.
- **Komponenten/Funktionen:** Alert, Confirm, Prompt, Toast, Async Validation und Custom HTML.
- **SASD-Einschätzung:** Einfacher Adapterkandidat, falls die Basissuite keine guten Dialoge liefert.
- **Quelle:** [https://github.com/sweetalert2/sweetalert2](https://github.com/sweetalert2/sweetalert2)

### W58. JSON Forms

- **Plattformen:** React, Angular, Vue
- **Lizenz:** MIT
- **Schwerpunkt:** Formulare aus JSON Schema und UI Schema.
- **Komponenten/Funktionen:** Generierte Editors, Layouts, Validation, Rules, Custom Renderers und Material/Vuetify-Renderer.
- **SASD-Einschätzung:** Sehr interessant für metadatengesteuerte SASD-Adminoberflächen.
- **Quelle:** [https://github.com/eclipsesource/jsonforms](https://github.com/eclipsesource/jsonforms)

### W59. OpenUI5

- **Plattformen:** JavaScript, TypeScript-Unterstützung und Enterprise-Web
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Offenes Enterprise-UI-Framework aus dem SAP-Umfeld mit MVC, Datenbindung und umfangreichen Controls.
- **Komponenten/Funktionen:** Tables, Lists, Trees, Forms, Value Helps, Date/Time, Shell, Navigation, Object Pages, Charts-Integration, OData-Bindung, Themes, i18n und Accessibility.
- **SASD-Einschätzung:** Wichtige Referenz für metadaten- und datenbindungsorientierte Geschäftsanwendungen; schwergewichtiger als ein schlanker SASD-Komponentenbaukasten.
- **Quelle:** [https://openui5.org/](https://openui5.org/)

### W60. Vaadin Flow Components

- **Plattformen:** Java und Web Components
- **Lizenz:** Open-Source-Kern überwiegend Apache-2.0; Pro-Komponenten und Services kommerziell
- **Schwerpunkt:** Serverseitig programmierbare Java-Weboberflächen mit Web-Component-Basis.
- **Komponenten/Funktionen:** Grid, Formulare, Date/Time, Upload, Dialog, Tabs, App Layout, Menu Bar, Notifications und offene Grundcontrols; Grid Pro, Charts, Dashboard, Map und weitere Komponenten teilweise kommerziell.
- **SASD-Einschätzung:** Unverzichtbarer Vergleich für Java-Unternehmensanwendungen und Open-Core-Grenzen; für SASD nur relevant, falls Java-Web als dritte Produktlinie aufgenommen wird.
- **Quelle:** [https://vaadin.com/components](https://vaadin.com/components)

### W61. Webix UI

- **Plattformen:** JavaScript/TypeScript und Framework-Integration
- **Lizenz:** Open-Source-Standard-/Community-Teile plus kommerzielle PRO-Edition
- **Schwerpunkt:** Umfangreiche JavaScript-Widgetsuite für datenintensive Geschäftsanwendungen.
- **Komponenten/Funktionen:** DataTable, TreeTable, Forms, Layouts, Windows, Menus, Charts, File Manager, Kanban, Scheduler, Gantt, Pivot, Spreadsheet und Report-/Dashboard-Bausteine; komplexe Widgets häufig PRO.
- **SASD-Einschätzung:** Relevante Open-Core-Alternative zu Ext JS und Kendo; Funktionsgrenzen und Lizenz pro Widget dokumentieren.
- **Quelle:** [https://webix.com/](https://webix.com/)

### W62. Ionic Framework

- **Plattformen:** Web Components, Angular, React, Vue; mobile Apps und PWA
- **Lizenz:** MIT für das Framework; Appflow und Unternehmensdienste kommerziell
- **Schwerpunkt:** Cross-Platform-UI-Toolkit für mobile und responsive Webanwendungen.
- **Komponenten/Funktionen:** Navigation, Tabs, Menus, Lists, Cards, Forms, Modals, Popovers, Toasts, Date/Time, Gestures, responsive Layouts und native Geräteintegration über Capacitor.
- **SASD-Einschätzung:** Wichtige Referenz für Touch- und Mobile-first-Komponenten; nicht als Desktop-LOB-Control-Suite missverstehen.
- **Quelle:** [https://ionicframework.com/](https://ionicframework.com/)

### W63. Angular Material

- **Plattformen:** Angular
- **Lizenz:** MIT-artige Open-Source-Lizenz
- **Schwerpunkt:** Offizielle Material-Design-Komponentenbibliothek des Angular-Ökosystems.
- **Komponenten/Funktionen:** Table, Tree, Form Field, Inputs, Select, Datepicker, Autocomplete, Dialog, Menu, Sidenav, Tabs, Stepper, Snackbar, Tooltip, Drag-and-drop/CDK und Accessibility-Bausteine.
- **SASD-Einschätzung:** Stabile Angular-Basis mit gutem CDK; komplexe Enterprise-Grids, Pivot, Gantt und Reporting müssen ergänzt werden.
- **Quelle:** [https://material.angular.dev/](https://material.angular.dev/)

### W64. Vuetify

- **Plattformen:** Vue
- **Lizenz:** MIT/Open Source; kommerzielle Templates und Support separat
- **Schwerpunkt:** Umfangreiches Material-Design-Komponentenframework für Vue.
- **Komponenten/Funktionen:** Data Table, Forms, Navigation, Dialogs, Cards, Layout/Grid, Treeview, Date Input, Stepper, Tabs, Alerts, Progress und Theme-System.
- **SASD-Einschätzung:** Eine der wichtigsten Vue-Komponentenbasen und guter Vergleich für konsistente Themes und responsive Geschäftsanwendungen.
- **Quelle:** [https://vuetifyjs.com/](https://vuetifyjs.com/)

### W65. Quasar Framework

- **Plattformen:** Vue, SPA, SSR, PWA, Capacitor/Cordova, Electron und Browser Extensions
- **Lizenz:** MIT
- **Schwerpunkt:** Integriertes Cross-Platform-Framework mit CLI und umfangreicher UI-Bibliothek.
- **Komponenten/Funktionen:** QTable, Tree, Forms, Select, Date/Time, WYSIWYG Editor, Uploader, Dialog, Drawer, Tabs, Stepper, Carousel, Virtual Scroll, Ajax Loading Bar, Plugins und Utilities.
- **SASD-Einschätzung:** Besonders interessante Vorlage für eine gemeinsame Web-/Mobile-/Electron-Produktlinie; stärkeres Framework-Commitment als bei einzelnen Komponentenpaketen.
- **Quelle:** [https://quasar.dev/](https://quasar.dev/)

### W66. Element Plus

- **Plattformen:** Vue 3
- **Lizenz:** MIT
- **Schwerpunkt:** Enterprise-orientierte Vue-3-Komponentenbibliothek.
- **Komponenten/Funktionen:** Table, Tree, TreeSelect, Form, Cascader, Date/Time Picker, Upload, Dialog, Drawer, Menu, Tabs, Steps, Transfer, Calendar und Descriptions.
- **SASD-Einschätzung:** Pragmatischer Vue-Kandidat für daten- und formularreiche Anwendungen.
- **Quelle:** [https://element-plus.org/](https://element-plus.org/)

### W67. UIkit

- **Plattformen:** HTML/CSS/JavaScript
- **Lizenz:** MIT
- **Schwerpunkt:** Leichtgewichtiges modulares Frontend-Framework.
- **Komponenten/Funktionen:** Grid, Cards, Navbar, Offcanvas, Modal, Dropdown, Tabs, Slideshow, Formulare, Upload, Sortable und Utility-Klassen.
- **SASD-Einschätzung:** Schlanke Bootstrap-Alternative; keine Enterprise-Datenkomponenten.
- **Quelle:** [https://getuikit.com/](https://getuikit.com/)

### W68. Bulma

- **Plattformen:** CSS
- **Lizenz:** MIT
- **Schwerpunkt:** CSS-only Framework auf Flexbox-Basis.
- **Komponenten/Funktionen:** Layout, Columns, Forms, Buttons, Navbar, Panels, Cards, Messages, Modal-Markup, Tabs und Helpers.
- **SASD-Einschätzung:** Geeignet als reine Stylingbasis; sämtliches interaktives Verhalten muss ergänzt werden.
- **Quelle:** [https://bulma.io/](https://bulma.io/)

### W69. Shoelace / Web Awesome

- **Plattformen:** Standard Web Components
- **Lizenz:** Open-Source-Kern; Marken-/Produktentwicklung und aktuelle Lizenz je Release prüfen
- **Schwerpunkt:** Frameworkunabhängige, zugängliche Custom Elements.
- **Komponenten/Funktionen:** Buttons, Inputs, Select, Dialog, Drawer, Dropdown, Tree, Tabs, Tooltip, Color Picker, Rating, Carousel und Design Tokens.
- **SASD-Einschätzung:** Wichtige Referenz für eine frameworkneutrale SASD-Web-API auf Basis echter Web Components.
- **Quelle:** [https://www.webawesome.com/](https://www.webawesome.com/)

### W70. React-admin

- **Plattformen:** React, REST/GraphQL und austauschbare UI Kits
- **Lizenz:** MIT für Open-Source-Edition; Enterprise-Module kommerziell
- **Schwerpunkt:** Komplettes Framework für datengetriebene B2B-, CRUD- und Admin-Anwendungen.
- **Komponenten/Funktionen:** Resources, Data Providers, Authentication, Access Control, DataGrids, Filter, Forms, Relationships, Undo, Routing, i18n, Themes, Preferences, Upload und Benachrichtigungen.
- **SASD-Einschätzung:** Sehr wichtige Vorlage für SASD-Webanwendungen: nicht nur Controls, sondern wiederverwendbare Anwendungslogik und komplette CRUD-Muster.
- **Quelle:** [https://marmelab.com/react-admin/](https://marmelab.com/react-admin/)

### W71. Refine

- **Plattformen:** React, headless und mehrere UI-/Backend-Integrationen
- **Lizenz:** MIT/Open Source für Core; Cloud-/Enterprise-Dienste separat
- **Schwerpunkt:** Meta-Framework für interne Werkzeuge, Admin Panels, Dashboards und B2B-Anwendungen.
- **Komponenten/Funktionen:** Data/Auth/Access Provider, Routing, CRUD Hooks, Forms, Tables, Notifications, Live/Realtime, Audit- und UI-Integrationen für Ant Design, MUI, Chakra und Headless.
- **SASD-Einschätzung:** Passende Architekturvorlage für Provider- und Adapterkonzepte der SASD Web Platform.
- **Quelle:** [https://refine.dev/](https://refine.dev/)

### W72. FINOS Perspective

- **Plattformen:** Web Components, JavaScript und Python-Integration
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Interaktive Analyse großer und streamender Datensätze.
- **Komponenten/Funktionen:** Virtualisierte Datentabellen, Pivoting, Aggregation, Sortierung, Filterung, Charts, Streaming Updates und konfigurierbare Viewer.
- **SASD-Einschätzung:** Spezialkandidat für hochinteraktive Datenarbeitsplätze und Finanz-/Monitoring-Dashboards.
- **Quelle:** [https://perspective.finos.org/](https://perspective.finos.org/)

### W73. RevoGrid

- **Plattformen:** Web Components mit React/Vue/Angular/Svelte-Integration
- **Lizenz:** MIT für Community-Kern; Pro-Angebote separat
- **Schwerpunkt:** Performantes virtuelles Data Grid und Spreadsheet-nahe Bearbeitung.
- **Komponenten/Funktionen:** Virtualisierung, Editing, Sorting, Filtering, Grouping, Pinning, Custom Cells, Range Selection und große Datenmengen.
- **SASD-Einschätzung:** Relevanter offener Grid-Kandidat, wenn frameworkneutrale Web Components bevorzugt werden.
- **Quelle:** [https://revolist.github.io/revogrid/](https://revolist.github.io/revogrid/)

### W74. SlickGrid / Slickgrid-Universal

- **Plattformen:** JavaScript/TypeScript und Framework-Wrapper
- **Lizenz:** MIT
- **Schwerpunkt:** Bewährtes virtuelles Grid für sehr große Datenbestände.
- **Komponenten/Funktionen:** Virtual Rendering, Editing, Sorting, Filtering über Erweiterungen, Grouping, Aggregates, Frozen Columns, Plugins und DataView.
- **SASD-Einschätzung:** Technisch leistungsfähig und historisch etabliert; API-Modernität und passender aktiver Fork müssen bewusst ausgewählt werden.
- **Quelle:** [https://github.com/6pac/SlickGrid](https://github.com/6pac/SlickGrid)

### W75. CKEditor 5

- **Plattformen:** JavaScript/TypeScript und Framework-Integrationen
- **Lizenz:** Dual: GPL 2+ oder kommerziell; zusätzliche Premiumfunktionen kommerziell
- **Schwerpunkt:** Modulares Rich-Text-Editor-Framework mit eigenem Datenmodell.
- **Komponenten/Funktionen:** Classic/Inline/Balloon Editors, Tabellen, Medien, Markdown-/HTML-Verarbeitung, Kommentare, Track Changes, Collaboration und umfangreiches Pluginmodell.
- **SASD-Einschätzung:** Funktionsstarke Referenz, aber Copyleft- und Premium-Lizenzgrenzen sind für SASD besonders sorgfältig zu prüfen.
- **Quelle:** [https://ckeditor.com/ckeditor-5/](https://ckeditor.com/ckeditor-5/)

### W76. TinyMCE

- **Plattformen:** JavaScript/TypeScript und Framework-Integrationen
- **Lizenz:** Open-Source-Ausgabe und kommerzielle Cloud-/Premiumangebote; konkrete Version und Lizenz prüfen
- **Schwerpunkt:** Etablierter eingebetteter WYSIWYG-HTML-Editor.
- **Komponenten/Funktionen:** Formatierung, Tabellen, Medien, Code/HTML, Templates, Spellchecking-, Collaboration- und Exportfunktionen je nach Edition und Plugins.
- **SASD-Einschätzung:** Wichtige Alternative zu CKEditor und Tiptap; Lizenz- und Pluginmatrix vor Produktaufnahme festhalten.
- **Quelle:** [https://www.tiny.cloud/](https://www.tiny.cloud/)

### W77. PDF.js

- **Plattformen:** JavaScript/Web
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** PDF-Rendering im Browser.
- **Komponenten/Funktionen:** PDF Parsing und Canvas/SVG-Rendering, Text Layer, Suche, Seitenansicht, Zoom, Thumbnails und Viewer-Anwendung.
- **SASD-Einschätzung:** Bevorzugte freie Grundlage für reine PDF-Anzeige; Bearbeitung, Signatur und komplexe Annotationen sind separate Produktklassen.
- **Quelle:** [https://mozilla.github.io/pdf.js/](https://mozilla.github.io/pdf.js/)

### W78. Golden Layout

- **Plattformen:** JavaScript/TypeScript
- **Lizenz:** MIT
- **Schwerpunkt:** Docking- und Multi-Panel-Layoutmanager für Webanwendungen.
- **Komponenten/Funktionen:** Dockbare Panels, Tabs, Splitter, Popouts, Layout-Persistenz und frameworkfähige Component Hosts.
- **SASD-Einschätzung:** Nützliche Webvorlage für IDE-, Dashboard- und Workspace-Oberflächen.
- **Quelle:** [https://golden-layout.com/](https://golden-layout.com/)

### W79. GridStack.js

- **Plattformen:** JavaScript/TypeScript und Framework-Wrapper
- **Lizenz:** MIT
- **Schwerpunkt:** Drag-and-drop Dashboard- und Widget-Layouts.
- **Komponenten/Funktionen:** Responsive Grid, Move/Resize, Nested Grids, Serialization, Dynamic Widgets, Touch und Layout-Persistenz.
- **SASD-Einschätzung:** Bevorzugter Kandidat für konfigurierbare SASD-Dashboards.
- **Quelle:** [https://gridstackjs.com/](https://gridstackjs.com/)

### W80. Fabric.js

- **Plattformen:** JavaScript/TypeScript Canvas
- **Lizenz:** MIT
- **Schwerpunkt:** Objektmodell und Interaktion auf HTML Canvas.
- **Komponenten/Funktionen:** Shapes, Text, Images, Selection, Transform, Grouping, Serialization, SVG Import/Export, Filters und Events.
- **SASD-Einschätzung:** Grundlage für Zeichenflächen und einfache Designer; kein fertiger Diagrammeditor.
- **Quelle:** [https://fabricjs.com/](https://fabricjs.com/)

### W81. Konva

- **Plattformen:** JavaScript/TypeScript Canvas und Framework-Wrapper
- **Lizenz:** MIT
- **Schwerpunkt:** Performante interaktive 2D-Canvas-Szenen.
- **Komponenten/Funktionen:** Shapes, Layers, Events, Drag-and-drop, Transform, Animation, Hit Detection, Caching und Export.
- **SASD-Einschätzung:** Alternative zu Fabric für interaktive Visualisierungen und individuelle Designer.
- **Quelle:** [https://konvajs.org/](https://konvajs.org/)

### W82. maxGraph

- **Plattformen:** JavaScript/TypeScript
- **Lizenz:** Apache-2.0
- **Schwerpunkt:** Weiterentwicklung der offenen mxGraph-Diagrammbibliothek.
- **Komponenten/Funktionen:** Graphmodelle, Nodes/Edges, Editing, Selection, Layouts, Styles, Serialization, Undo/Redo und Interaktion.
- **SASD-Einschätzung:** Interessanter offener Kandidat für Flowchart- und Netzwerkdiagramme; Reifegrad und API-Stabilität im Prototyp prüfen.
- **Quelle:** [https://github.com/maxGraph/maxGraph](https://github.com/maxGraph/maxGraph)

### W83. TOAST UI Komponenten

- **Plattformen:** JavaScript/TypeScript und Framework-Wrapper
- **Lizenz:** MIT für viele Einzelprojekte; jedes Repository prüfen
- **Schwerpunkt:** Sammlung eigenständiger Daten-, Editor-, Kalender- und Medienkomponenten.
- **Komponenten/Funktionen:** Grid, Calendar, Markdown Editor, Image Editor, Charts und Pagination mit separaten Paketen.
- **SASD-Einschätzung:** Breite Ergänzungsquelle; kein vollständig vereinheitlichtes Enterprise-Suite-Modell.
- **Quelle:** [https://ui.toast.com/](https://ui.toast.com/)

### W84. jsreport

- **Plattformen:** Node.js, Web Designer und API
- **Lizenz:** Open-Core: freie Community-/Core-Funktionen plus kommerzielle Editionen
- **Schwerpunkt:** Serverseitige Berichtserstellung aus HTML, Templates und Daten.
- **Komponenten/Funktionen:** Report Designer, Templates, Engines, PDF/Excel-Ausgabe, Scheduling, Versioning, Data Sources, API und Erweiterungen je nach Edition.
- **SASD-Einschätzung:** Relevante Web-/Serveralternative zu klassischen .NET-Reportengines; Lizenz und Betriebsmodell separat bewerten.
- **Quelle:** [https://jsreport.net/](https://jsreport.net/)

## Startgewichtung aller 123 Komponentenklassen für WinForms

Die ausführlichen Funktionsbeschreibungen folgen unmittelbar im Komponenten-Funktionskatalog. Diese Matrix ergänzt für **jede** Klasse eine erste Umsetzungspriorität aus Sicht des aktuellen WinForms-Projekts.

| ID | Bereich | Komponente | Priorität | Startwert | Frühester Horizont | Begründung |
| --- | --- | --- | :---: | ---: | --- | --- |
| DATA-001 | Datenanzeige und Tabellen | DataGrid / DataTable | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| DATA-002 | Datenanzeige und Tabellen | TreeGrid / TreeList | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| DATA-003 | Datenanzeige und Tabellen | PivotGrid / OLAP | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| DATA-004 | Datenanzeige und Tabellen | PropertyGrid | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| DATA-005 | Datenanzeige und Tabellen | ListView / ObjectList | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| DATA-006 | Datenanzeige und Tabellen | Card View | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: mittel. |
| DATA-007 | Datenanzeige und Tabellen | Virtualized List | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| DATA-008 | Datenanzeige und Tabellen | Master-Detail | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| DATA-009 | Datenanzeige und Tabellen | Infinite Scroll | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: mittel. |
| DATA-010 | Datenanzeige und Tabellen | Data Pager | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| DATA-011 | Datenanzeige und Tabellen | Column Chooser | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| DATA-012 | Datenanzeige und Tabellen | Filter Builder | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| DATA-013 | Datenanzeige und Tabellen | Search Panel | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| DATA-014 | Datenanzeige und Tabellen | Summaries / Aggregates | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| DATA-015 | Datenanzeige und Tabellen | Data Export | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| FORM-016 | Formulare und Eingabe | TextBox / TextArea | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FORM-017 | Formulare und Eingabe | Masked Input | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FORM-018 | Formulare und Eingabe | Numeric Editor | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FORM-019 | Formulare und Eingabe | Date Picker | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FORM-020 | Formulare und Eingabe | Time Picker | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FORM-021 | Formulare und Eingabe | Date Range Picker | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: mittel. |
| FORM-022 | Formulare und Eingabe | Calendar | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| FORM-023 | Formulare und Eingabe | ComboBox / Select | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FORM-024 | Formulare und Eingabe | AutoComplete | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| FORM-025 | Formulare und Eingabe | MultiSelect / Tags | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| FORM-026 | Formulare und Eingabe | CheckBox / Radio / Toggle | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FORM-027 | Formulare und Eingabe | Slider / Range Slider | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| FORM-028 | Formulare und Eingabe | Color Picker | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| FORM-029 | Formulare und Eingabe | File / Folder Picker | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FORM-030 | Formulare und Eingabe | Upload | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: hoch. |
| FORM-031 | Formulare und Eingabe | Rating | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: niedrig. |
| FORM-032 | Formulare und Eingabe | Signature Pad | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: mittel. |
| FORM-033 | Formulare und Eingabe | Form Layout | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FORM-034 | Formulare und Eingabe | Validation Summary | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FORM-035 | Formulare und Eingabe | Schema-driven Form | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| NAV-036 | Navigation und Layout | Application Shell | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| NAV-037 | Navigation und Layout | Menu / Context Menu | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| NAV-038 | Navigation und Layout | Toolbar / CommandBar | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| NAV-039 | Navigation und Layout | Ribbon | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| NAV-040 | Navigation und Layout | Sidebar / Navigation Drawer | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| NAV-041 | Navigation und Layout | Navigation View | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| NAV-042 | Navigation und Layout | Tabs / Documents | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| NAV-043 | Navigation und Layout | Docking / MDI | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: sehr hoch. |
| NAV-044 | Navigation und Layout | Split Pane | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| NAV-045 | Navigation und Layout | Accordion / Expander | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| NAV-046 | Navigation und Layout | TreeView | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| NAV-047 | Navigation und Layout | Breadcrumb | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| NAV-048 | Navigation und Layout | Stepper / Wizard | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| NAV-049 | Navigation und Layout | Tile / Dashboard Layout | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| NAV-050 | Navigation und Layout | Responsive Layout | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: hoch. |
| NAV-051 | Navigation und Layout | Status Bar | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FEED-052 | Dialoge, Feedback und Hilfen | Message Dialog | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FEED-053 | Dialoge, Feedback und Hilfen | Content Dialog / Modal | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FEED-054 | Dialoge, Feedback und Hilfen | Drawer / Flyout | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: mittel. |
| FEED-055 | Dialoge, Feedback und Hilfen | Toast / Snackbar | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FEED-056 | Dialoge, Feedback und Hilfen | Desktop Notification | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| FEED-057 | Dialoge, Feedback und Hilfen | Tooltip / ScreenTip | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FEED-058 | Dialoge, Feedback und Hilfen | Popover | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: mittel. |
| FEED-059 | Dialoge, Feedback und Hilfen | Progress Bar / Ring | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| FEED-060 | Dialoge, Feedback und Hilfen | Busy Overlay | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FEED-061 | Dialoge, Feedback und Hilfen | Skeleton | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: niedrig. |
| FEED-062 | Dialoge, Feedback und Hilfen | Error Details Dialog | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| FEED-063 | Dialoge, Feedback und Hilfen | Help / Teaching Tip | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| VIZ-064 | Diagramme, Dashboards und Geodaten | Cartesian Chart | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| VIZ-065 | Diagramme, Dashboards und Geodaten | Pie / Donut | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| VIZ-066 | Diagramme, Dashboards und Geodaten | Financial Chart | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| VIZ-067 | Diagramme, Dashboards und Geodaten | Heatmap | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| VIZ-068 | Diagramme, Dashboards und Geodaten | Treemap / Sunburst | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| VIZ-069 | Diagramme, Dashboards und Geodaten | Sankey | **WEB** | 10 | Separates Webprojekt | Im aktuellen WinForms-Backlog nicht priorisieren; später browser- beziehungsweise Blazor-nativ behandeln. Aufwand laut Funktionskatalog: hoch. |
| VIZ-070 | Diagramme, Dashboards und Geodaten | Gauge | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| VIZ-071 | Diagramme, Dashboards und Geodaten | Sparkline / Micro Chart | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| VIZ-072 | Diagramme, Dashboards und Geodaten | Dashboard | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: sehr hoch. |
| VIZ-073 | Diagramme, Dashboards und Geodaten | Map 2D | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| VIZ-074 | Diagramme, Dashboards und Geodaten | Map 3D / Globe | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| VIZ-075 | Diagramme, Dashboards und Geodaten | Network Graph | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| VIZ-076 | Diagramme, Dashboards und Geodaten | Diagram Editor | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| VIZ-077 | Diagramme, Dashboards und Geodaten | Text-to-Diagram | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| PLAN-078 | Planung, Zeit und Workflow | Scheduler | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: sehr hoch. |
| PLAN-079 | Planung, Zeit und Workflow | Resource Scheduler | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| PLAN-080 | Planung, Zeit und Workflow | Gantt | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: sehr hoch. |
| PLAN-081 | Planung, Zeit und Workflow | Kanban / TaskBoard | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| PLAN-082 | Planung, Zeit und Workflow | Timeline | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: mittel. |
| PLAN-083 | Planung, Zeit und Workflow | Calendar Heatmap | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: mittel. |
| PLAN-084 | Planung, Zeit und Workflow | Recurrence Editor | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| PLAN-085 | Planung, Zeit und Workflow | Workflow Designer | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: sehr hoch. |
| DOC-086 | Dokumente, Editoren und Medien | Rich Text Editor | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: sehr hoch. |
| DOC-087 | Dokumente, Editoren und Medien | Document Editor | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| DOC-088 | Dokumente, Editoren und Medien | Markdown Editor | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| DOC-089 | Dokumente, Editoren und Medien | Code Editor | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| DOC-090 | Dokumente, Editoren und Medien | Diff Viewer | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| DOC-091 | Dokumente, Editoren und Medien | PDF Viewer | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| DOC-092 | Dokumente, Editoren und Medien | PDF Generator | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| DOC-093 | Dokumente, Editoren und Medien | Report Engine | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: sehr hoch. |
| DOC-094 | Dokumente, Editoren und Medien | Report Designer | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| DOC-095 | Dokumente, Editoren und Medien | Spreadsheet | **P3** | 25 | Zukauf/Adapter | Zu komplex für V1; keine Eigenentwicklung ohne belastbaren Business Case. Aufwand laut Funktionskatalog: sehr hoch. |
| DOC-096 | Dokumente, Editoren und Medien | Image Viewer | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| DOC-097 | Dokumente, Editoren und Medien | Image Editor | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| DOC-098 | Dokumente, Editoren und Medien | Media Player | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| DOC-099 | Dokumente, Editoren und Medien | Barcode / QR | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| SYS-100 | Dateien, Shell und Systemintegration | File Explorer | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: sehr hoch. |
| SYS-101 | Dateien, Shell und Systemintegration | Folder Tree | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| SYS-102 | Dateien, Shell und Systemintegration | Recent Files | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: niedrig. |
| SYS-103 | Dateien, Shell und Systemintegration | Drag and Drop Files | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| SYS-104 | Dateien, Shell und Systemintegration | Clipboard | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| SYS-105 | Dateien, Shell und Systemintegration | System Tray | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| SYS-106 | Dateien, Shell und Systemintegration | Global Hotkeys | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| SYS-107 | Dateien, Shell und Systemintegration | Browser/WebView | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| QUAL-108 | Themes, Qualität und Plattformdienste | Design Tokens | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-109 | Themes, Qualität und Plattformdienste | Light/Dark Theme | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| QUAL-110 | Themes, Qualität und Plattformdienste | High Contrast | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-111 | Themes, Qualität und Plattformdienste | Accessibility | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: sehr hoch. |
| QUAL-112 | Themes, Qualität und Plattformdienste | Localization | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-113 | Themes, Qualität und Plattformdienste | RTL Layout | **P2** | 50 | Bedarfsgesteuert | Erst bei einem konkreten Produktbedarf prototypisieren; Nutzen gegen Wartungsaufwand prüfen. Aufwand laut Funktionskatalog: hoch. |
| QUAL-114 | Themes, Qualität und Plattformdienste | High DPI / Scaling | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-115 | Themes, Qualität und Plattformdienste | Keyboard Navigation | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-116 | Themes, Qualität und Plattformdienste | Command System | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-117 | Themes, Qualität und Plattformdienste | Undo/Redo | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: hoch. |
| QUAL-118 | Themes, Qualität und Plattformdienste | State Persistence | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-119 | Themes, Qualität und Plattformdienste | Telemetry Hooks | **P1** | 72 | Nach Fundament | Hoher Wiederverwendungswert; bevorzugt über einen austauschbaren Adapter oder eine SASD-Fassade. Aufwand laut Funktionskatalog: mittel. |
| QUAL-120 | Themes, Qualität und Plattformdienste | Visual Regression Tests | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-121 | Themes, Qualität und Plattformdienste | Accessibility Tests | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |
| QUAL-122 | Themes, Qualität und Plattformdienste | Component Gallery | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: mittel. |
| QUAL-123 | Themes, Qualität und Plattformdienste | API Documentation | **P0** | 90 | Jetzt | Grundfunktion für mehrere aktuelle SASD-WinForms-Anwendungen; eigener Vertrag und automatisierte Tests. Aufwand laut Funktionskatalog: hoch. |

### Interpretation

- **P0:** Bestandteil des ersten SASD-WinForms-Fundaments oder zwingende Qualitätsinfrastruktur.
- **P1:** Zweite Ausbaustufe; typischerweise Spezialadapter, produktive Workbench-Funktion oder erweiterter Datenbaustein.
- **P2:** Nur durch einen nachgewiesenen Anwendungsfall starten; kein Vorratsbau.
- **P3:** Voraussichtlich zukaufen, integrieren oder ausdrücklich aus V1 ausschließen.
- **WEB:** Funktion ist sinnvoll, wird aber im separaten modernen Webprojekt statt als WinForms-Kern behandelt.

Die Priorität bewertet **nicht die allgemeine Wichtigkeit** einer Funktion. Ein Spreadsheet oder Ressourcen-Scheduler kann für ein bestimmtes Produkt zentral sein; er bleibt dennoch P3, weil SASD ihn nicht als allgemeine V1-Komponente selbst entwickeln sollte.


## Komponenten-Funktionskatalog

Der Funktionskatalog ist die eigentliche Produktlandkarte. Er beschreibt nicht bloß Control-Namen, sondern die erwarteten Fähigkeiten, geeignete Referenzbibliotheken und die strategische SASD-Behandlung.

### Datenanzeige und Tabellen

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| DataGrid / DataTable | Tabellarische Anzeige und Bearbeitung mit Spalten, Zeilen, Sortierung, Filterung, Auswahl, Editing, Validierung und Virtualisierung. | DataGridView/Krypton, WPF DataGrid, Telerik/DevExpress, PropertyTools | AG Grid, TanStack Table, Tabulator, Radzen, MudBlazor, Kendo | Kernadapter | hoch |
| TreeGrid / TreeList | Hierarchische Daten in Tabellenform mit Ein-/Ausklappen, Lazy Loading und Baumspalte. | DevExpress TreeList, Telerik GridView-Hierarchie, PropertyTools | AG Grid Tree Data, Prime TreeTable, DHTMLX TreeGrid, Ant Table Tree | später | hoch |
| PivotGrid / OLAP | Mehrdimensionale Aggregation, Dimensionsachsen, Drill-down, Measures, Summen und Feldliste. | DevExpress/Telerik/Syncfusion/MESCIUS | Kendo/DevExtreme/Syncfusion/Wijmo/AG Grid Enterprise | zukaufen/Adapter | sehr hoch |
| PropertyGrid | Automatische Bearbeitung von Objekteigenschaften über Metadaten, Kategorien und Editoren. | PropertyTools, Extended WPF Toolkit, Actipro/Telerik | JSON Forms, Schema Form, eigene Inspector-Komponente | Kern Desktop | mittel |
| ListView / ObjectList | Listen mit Icons, Spalten, Gruppierung, Details/Tile-Ansichten und Virtualisierung. | WinForms ListView, WPF ListView, ObjectListView | virtuelle Listen, DataList, frameworkeigene List Components | Kern | mittel |
| Card View | Datensätze als Karten mit Templates, Gruppierung, Auswahl und responsivem Layout. | DevExpress CardView, ItemsControl/Templates | CSS Grid + Cards, DevExtreme/Kendo ListView | Kern Web | mittel |
| Virtualized List | Nur sichtbare Elemente rendern, um große Datenmengen performant darzustellen. | WPF VirtualizingPanel, DataGrid Virtual Mode | TanStack Virtual, Framework Virtualize, AG Grid | Kern | hoch |
| Master-Detail | Hauptdatensatz und abhängige Details in eingebetteten Bereichen oder Split Views. | DataGrid Master-Detail, SplitContainer, Docking | Grid Detail Templates, Drawer/Panel, nested routes | Vorlage | mittel |
| Infinite Scroll | Nachladen bei Scrollposition, Cursor- oder Seiten-basiert. | Custom Virtualization/Data Provider | IntersectionObserver, Virtualize, Grid DataSource | Adapter | mittel |
| Data Pager | Seitennavigation, Seitengröße, Gesamtanzahl und Serverabfragen. | BindingNavigator/eigenes Control | Pagination Components, Grid Pager | Kern | niedrig |
| Column Chooser | Spalten ein-/ausblenden, Reihenfolge ändern und Layout speichern. | Grid-spezifisch/eigene Dialoge | AG/Kendo/DevExtreme oder eigener Drawer | Kern Grid | mittel |
| Filter Builder | Visueller Ausdruckseditor für verschachtelte AND/OR-Regeln. | DevExpress/Telerik oder Eigenbau | DevExtreme FilterBuilder, Kendo, QueryBuilder-Projekte | später | hoch |
| Search Panel | Globale Suche über sichtbare Felder mit Highlighting und Debounce. | SASD SearchBox + BindingSource | Toolbar + Grid API/TanStack Filter | Kern | niedrig |
| Summaries / Aggregates | Summen, Mittelwerte, Min/Max, Count und benutzerdefinierte Aggregate in Footer/Gruppen. | Grid-Engine oder berechnete Footer | Grid/TanStack Aggregation | Kern Grid | mittel |
| Data Export | Export nach CSV, XLSX, PDF, JSON und Zwischenablage mit Format-/Spaltenkontrolle. | CsvHelper, ClosedXML, Reporting/PDF | SheetJS, Grid Exporter, serverseitige Exporte | Kern CSV; Rest Adapter | hoch |

### Formulare und Eingabe

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| TextBox / TextArea | Ein- oder mehrzeilige Texteingabe, Validierung, Masken, Placeholder und Undo. | WinForms/WPF/Krypton | HTML Input/Textarea, Framework Components | Kern | niedrig |
| Masked Input | Formatgesteuerte Eingabe für Telefon, Datum, IDs oder Codes. | MaskedTextBox, Telerik/DevExpress Editors | Inputmask, Cleave, Suite-Komponenten | Kern | mittel |
| Numeric Editor | Zahlen mit Format, Min/Max, Spin Buttons, Währung, Prozent und Nullwerten. | NumericUpDown, WPF UI, PropertyTools | Number Input, MudNumericField, Kendo NumericTextBox | Kern | niedrig |
| Date Picker | Datumsauswahl, Min/Max, deaktivierte Tage, Kultur und Parsing. | DateTimePicker, WPF Kalender | native input, Flatpickr, Suite-Komponenten | Kern | mittel |
| Time Picker | Zeit, Minutenraster, Zeitzone und 12/24-Stunden-Format. | DateTimePicker/Spezialcontrols | TimePicker Components | Kern | mittel |
| Date Range Picker | Start/Ende mit Konsistenzprüfung und Presets. | Spezialcontrol oder zwei Picker | MUI/Kendo/Ant/Mud/Radzen | Kern Web | mittel |
| Calendar | Monats-/Jahresansicht zur Datumsauswahl oder Ereignisdarstellung. | MonthCalendar/Telerik | FullCalendar, Suite Calendar | Adapter | mittel |
| ComboBox / Select | Auswahlliste mit Textsuche, Binding und Custom Items. | ComboBox/Krypton/WPF | Select/Combobox, Radix/React Aria | Kern | niedrig |
| AutoComplete | Asynchrone Vorschläge, Highlighting, Debounce und Tastaturnavigation. | AutoCompleteMode/eigenes Popup | Combobox/Autosuggest Components | Kern | mittel |
| MultiSelect / Tags | Mehrfachauswahl als Chips/Tags mit Suche und Entfernen. | CheckedComboBox, TokenEdit kommerziell | Ant Select, MUI Autocomplete, MudSelect | Kern | mittel |
| CheckBox / Radio / Toggle | Boolesche oder exklusive Auswahl, tri-state und zugängliche Beschriftung. | Standardcontrols/Krypton | native/headless Komponenten | Kern | niedrig |
| Slider / Range Slider | Wertebereich per Track, Ticks, Tooltips und zwei Handles. | TrackBar/WPF Slider | Radix Slider, noUiSlider, Suite | Kern | mittel |
| Color Picker | Farbwahl per Palette, RGB/HSL/Hex, Alpha und zuletzt verwendete Farben. | PropertyTools/HandyControl | Pickr, suiteeigene Picker | Adapter | mittel |
| File / Folder Picker | Datei-, Ordner- und Mehrfachauswahl mit Filtern und Verlauf. | OpenFileDialog/FolderBrowserDialog | File Input, File System Access API eingeschränkt | Kern je Plattform | mittel |
| Upload | Dateien auswählen, Drag/drop, Fortschritt, Chunking, Retry und Validierung. | Desktop Dateiimport/Background Tasks | Uppy, FilePond, Telerik/DevExpress Upload | Kern Web Adapter | hoch |
| Rating | Bewertung per Sternen/Symbolen mit halben Werten und Readonly-Modus. | Custom/HandyControl | MUI/Ant/Mud/Radzen | optional | niedrig |
| Signature Pad | Handschriftliche Unterschrift mit Touch/Stift, Undo und Export. | InkCanvas/WPF oder Spezialcontrol | signature_pad / Canvas | optional | mittel |
| Form Layout | Responsive Anordnung von Labels, Editoren, Gruppen und Hilfetexten. | TableLayoutPanel, WPF Grid, DevExpress LayoutControl | CSS Grid/Flex, Form Layout Components | Kern | mittel |
| Validation Summary | Feldfehler sammeln, Fokussteuerung, Inline- und globale Meldungen. | ErrorProvider, IDataErrorInfo/INotifyDataErrorInfo | HTML validation, Blazor EditForm, schema validators | Kern | mittel |
| Schema-driven Form | Formular aus Metadaten/JSON Schema generieren und dynamisch validieren. | PropertyGrid/eigene Metadatenengine | JSON Forms, RJSF, Form.io | später | hoch |

### Navigation und Layout

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| Application Shell | Gemeinsamer Rahmen mit Navigation, Command Area, Content, Status und Benutzerkontext. | Krypton/WPF UI/Avalonia Shell | Admin layout, AppShell, sidebar/topbar | Kernvorlage | hoch |
| Menu / Context Menu | Hierarchische Befehle, Icons, Shortcuts, Checked States und dynamische Einträge. | MenuStrip/Krypton/WPF Menu | Menu primitives, ContextMenu Components | Kern | mittel |
| Toolbar / CommandBar | Häufige Befehle, Groups, Overflow, Toggle und Custom Items. | ToolStrip/Krypton/Fluent Ribbon | Toolbar, CommandBar, responsive overflow | Kern | mittel |
| Ribbon | Office-artige Tabs, Gruppen, Galleries, Backstage und Quick Access. | Krypton Ribbon, Fluent.Ribbon | Fluent UI Ribbon fehlt oft; Web meist Custom | optional | hoch |
| Sidebar / Navigation Drawer | Vertikale Navigation mit Gruppen, Collapse, Icons und responsive Overlay. | NavigationView, WPF UI, Custom Panel | daisyUI Drawer, MUI Drawer, Admin templates | Kern | mittel |
| Navigation View | Hierarchische Appnavigation mit Breadcrumb und ausgewählter Seite. | WPF UI/FluentAvalonia | Fluent/MUI/Ant Navigation | Kern | mittel |
| Tabs / Documents | Mehrere Inhalte, Close/Reorder, Dirty-State und persistente Auswahl. | TabControl, Krypton Navigator, Docking | Tabs/Router, sortable tabs | Kern | mittel |
| Docking / MDI | Andockbare Werkzeuge, Dokumente, Auto-Hide, Floating Windows und Layoutpersistenz. | DockPanel Suite, AvalonDock, Krypton Docking | GoldenLayout, FlexLayout, desktopartige Webshell | Desktop Kern; Web optional | sehr hoch |
| Split Pane | Veränderbare Bereiche horizontal/vertikal, Min/Max und Persistenz. | SplitContainer/GridSplitter | Split.js, CSS resize, suite splitters | Kern | mittel |
| Accordion / Expander | Platzsparende Gruppen, Single-/Multi-Expand und Animation. | Expander/Accordion Controls | native details, Radix Accordion, Bootstrap | Kern | niedrig |
| TreeView | Hierarchische Navigation, Checkboxen, Lazy Loading, Drag/drop und Kontextmenü. | TreeView, Krypton, WPF | Ant/MUI/Mud/Radzen, jsTree/Fancytree | Kern | hoch |
| Breadcrumb | Pfadnavigation, Overflow und klickbare Ebenen. | Eigenes Control/ToolStrip | Bootstrap/MUI/Ant/Fluent | Kern | niedrig |
| Stepper / Wizard | Mehrstufiger Ablauf, Validierung, Back/Next, Fortschritt und Resume. | Wizard, TabControl/SASD Wizard | Steps/Stepper Components | Kernvorlage | mittel |
| Tile / Dashboard Layout | Verschiebbare/resizable Widgets, Raster und gespeicherte Layouts. | TableLayout/Grid + Drag oder kommerziell | GridStack, React Grid Layout, dashboards | später | hoch |
| Responsive Layout | Anpassung an Fenstergröße, Breakpoints und alternative Navigation. | WPF VisualState/Triggers, WinForms eigene Regeln | CSS Grid/Flex/Container Queries | Kern Web | hoch |
| Status Bar | Status, Fortschritt, Verbindung, Benutzerhinweise und Tastaturzustände. | StatusStrip/Krypton | Footer/Status area, live regions | Kern | niedrig |

### Dialoge, Feedback und Hilfen

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| Message Dialog | Info, Warnung, Fehler, Frage mit klaren Aktionen. | MessageBox/SASD Dialog | Modal/Alert/SweetAlert2 | Kern | niedrig |
| Content Dialog / Modal | Beliebiger Inhalt, Fokusfalle, Validierung und asynchrone Aktionen. | Custom Form/ContentDialog | Radix Dialog, Bootstrap Modal, Suite | Kern | mittel |
| Drawer / Flyout | Temporärer seitlicher Inhalt für Details, Filter oder Einstellungen. | Flyout Panels, MahApps | MUI/Ant Drawer, offcanvas | Kern Web | mittel |
| Toast / Snackbar | Nicht blockierende Rückmeldung, Queue, Aktionen und Auto-Dismiss. | DesktopAlert/SASD Notification | Snackbar/Toast libraries | Kern | mittel |
| Desktop Notification | Betriebssystembenachrichtigung mit Aktionen und Deep Link. | Windows App Notifications | Web Notifications/PWA | Adapter | hoch |
| Tooltip / ScreenTip | Kontextinformation, Shortcut, Rich Content und Verzögerung. | ToolTip, Ribbon ScreenTip | native title, Popper-based Tooltip | Kern | niedrig |
| Popover | Interaktiver schwebender Inhalt mit Fokus- und Positionierungslogik. | Popup/Flyout | Radix Popover, Floating UI | Kern Web | mittel |
| Progress Bar / Ring | Determinate/indeterminate Fortschritt und Statuswechsel. | ProgressBar/Krypton | HTML progress, component suites | Kern | niedrig |
| Busy Overlay | Blockiert Teilbereich während asynchroner Operation und verhindert Doppelaktionen. | BusyIndicator/Overlay Panel | Backdrop + Spinner/Skeleton | Kern | mittel |
| Skeleton | Platzhalterlayout während Datenladen zur Vermeidung von Layoutsprung. | Custom WPF/WinForms | MUI/Ant/Mud Skeleton | Kern Web | niedrig |
| Error Details Dialog | Benutzerfreundliche Meldung plus kopierbare technische Details, Correlation-ID und Logpfad. | SASD Standarddialog | SASD Modal/Drawer | Kern | mittel |
| Help / Teaching Tip | Kontextuelle Einführung, Feature Discovery und Tour-Schritte. | TeachingTip/Custom Overlay | Shepherd.js, Intro.js, Fluent TeachingTip | optional | mittel |

### Diagramme, Dashboards und Geodaten

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| Cartesian Chart | Linie, Fläche, Balken, Scatter, Achsen, Zoom, Tooltips und Legenden. | ScottPlot, LiveCharts2, OxyPlot | ECharts, Chart.js, Plotly, Kendo/DevExtreme | Kernadapter | hoch |
| Pie / Donut | Anteile mit Labels, Drilldown und Selection. | LiveCharts2/OxyPlot | Chart.js/ECharts | Adapter | mittel |
| Financial Chart | Candlestick, OHLC, Volume, Indicators und große Zeitreihen. | ScottPlot Finance, kommerzielle Engines | Plotly/ECharts/Highcharts Stock | Spezialadapter | hoch |
| Heatmap | Matrix oder räumliche Intensitäten mit Farbskala und Tooltip. | ScottPlot/OxyPlot/SciChart | ECharts/Plotly | Adapter | hoch |
| Treemap / Sunburst | Hierarchische Größenverhältnisse und Drilldown. | LiveCharts/Nevron/kommerziell | ECharts/Plotly/D3 | Adapter | hoch |
| Sankey | Flüsse zwischen Kategorien mit gewichteten Verbindungen. | kommerziell oder Custom | ECharts/Plotly/D3 | Web Adapter | hoch |
| Gauge | Radial/linear, KPI-Bereiche, Schwellen und Animation. | LiveCharts2/Nevron/Commercial | ECharts/Mud/Radzen | Adapter | mittel |
| Sparkline / Micro Chart | Kompakte Verlaufsgrafik in Gridzelle oder KPI-Karte. | Actipro/ScottPlot Mini | Inline SVG/Chart.js/ECharts | Kern Dashboard | mittel |
| Dashboard | Kombination aus KPIs, Filtern, Charts, Grids und gespeicherten Layouts. | SASD Template/kommerziell | Admin template + GridStack + Charts | Vorlage | sehr hoch |
| Map 2D | Kacheln, Vektoren, Marker, Layer, GeoJSON und Interaktion. | Mapsui | Leaflet/OpenLayers/MapLibre | Adapter | hoch |
| Map 3D / Globe | Terrain, 3D Tiles, Kamera und große Geodaten. | Spezialengine | CesiumJS/MapLibre/Deck.gl | nicht Kern | sehr hoch |
| Network Graph | Knoten/Kanten, Layoutalgorithmen, Selection und Details. | GraphX/kommerziell/Custom | Cytoscape.js, React Flow, D3 | Spezialadapter | hoch |
| Diagram Editor | Shapes, Connectoren, Ports, Snap, Undo, Zoom, Persistenz und Export. | MindFusion/Nevron/kommerziell | React Flow, maxGraph, JointJS, bpmn-js | später | sehr hoch |
| Text-to-Diagram | Diagramm aus DSL/Markdown für reproduzierbare Dokumentation. | WebView/Exportintegration | Mermaid | Kern Dokumentation | mittel |

### Planung, Zeit und Workflow

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| Scheduler | Terminansichten, Ressourcen, Drag/Resize, Recurrence, Zeitzonen und Konflikte. | Telerik/DevExpress/Syncfusion | FullCalendar, DHTMLX, Bryntum, Radzen | Adapter/Zukauf | sehr hoch |
| Resource Scheduler | Ressourcenzeilen/-spalten, Kapazität, Gruppierung und Timeline. | kommerziell | FullCalendar Premium, Bryntum, DHTMLX Pro | zukaufen | sehr hoch |
| Gantt | Task Tree, Timeline, Dependencies, Milestones, Baselines, Critical Path und Editing. | Telerik/DevExpress/Syncfusion | DHTMLX Community/Pro, Bryntum, Frappe Gantt | Adapter | sehr hoch |
| Kanban / TaskBoard | Spalten, Karten, Swimlanes, WIP-Limits, Drag/drop und Filter. | Custom/Dashboard Panels | SortableJS + Cards, DHTMLX/Bryntum, Radzen Patterns | Vorlage | hoch |
| Timeline | Chronologische Ereignisse, Gruppen, Zoom und Details. | Custom ItemsControl/Chart | vis-timeline, Suite Timeline | Adapter | mittel |
| Calendar Heatmap | Aktivität oder Auslastung über Tage/Wochen. | Custom/Chart | D3/ECharts/GitHub-style components | optional | mittel |
| Recurrence Editor | RRULE-/Serienterminbearbeitung mit Ausnahmen. | Scheduler-spezifisch | FullCalendar/RRule/Suite | Adapter | hoch |
| Workflow Designer | Schritte, Entscheidungen, Verbindungen, Validierung und Versionierung. | Diagram + Domain Engine | React Flow/bpmn-js + Backend | später | sehr hoch |

### Dokumente, Editoren und Medien

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| Rich Text Editor | Formatierung, Listen, Tabellen, Bilder, Links, Undo, Paste Cleanup und Serialisierung. | RichTextBox, TX Text, Nevron | Tiptap, Quill, Lexical, CKEditor/TinyMCE | Adapter | sehr hoch |
| Document Editor | Seitenlayout, Header/Footer, DOCX, Serienbrief, Track Changes, Kommentare und Druck. | TX Text/DevExpress/Telerik | Syncfusion/DevExpress/TX Text Web | zukaufen | sehr hoch |
| Markdown Editor | Text/Preview, Syntax, Toolbar, Links, Tabellen und Export. | AvalonEdit/Scintilla + Markdig | CodeMirror/Monaco + markdown renderer | Kern | mittel |
| Code Editor | Syntax, Folding, Completion, Diagnostics, Multi-cursor und Diff. | ScintillaNET/AvalonEdit | Monaco/CodeMirror | Adapter | hoch |
| Diff Viewer | Zeilen-/Wortvergleich, Inline/Side-by-side, Navigation und Patch. | DiffPlex + Custom View | Monaco Diff, jsdiff + UI | Kern DevTools | hoch |
| PDF Viewer | Rendern, Suche, Zoom, Navigation, Annotationen, Formulare und Druck. | WebView/PDFium/Commercial | PDF.js, commercial viewers | Adapter | hoch |
| PDF Generator | Programmgesteuerte Dokumente, Tabellen, Seitenumbrüche, Bilder und Metadaten. | QuestPDF/PDFsharp | serverseitig dieselben .NET-Tools oder pdf-lib/jsPDF | Kern Adapter | hoch |
| Report Engine | Datenquellen, Bands, Gruppierung, Parameter, Subreports, Viewer und Export. | FastReport OSS/Commercial suites | FastReport/JSReport/Server Rendering | Adapter | sehr hoch |
| Report Designer | Visueller WYSIWYG-Designer, Data Dictionary, Expressions und Preview. | kommerziell/FastReport Designer Grenzen | commercial/open-core | zukaufen | sehr hoch |
| Spreadsheet | Zellen, Formeln, Formatierung, Sheets, Charts, Import/Export und Collaboration. | ReoGrid/Commercial | Handsontable/SpreadJS/Syncfusion | Adapter/Zukauf | sehr hoch |
| Image Viewer | Zoom, Pan, Rotate, Fit, Metadata und Annotationen. | PictureBox/Custom/WPF Image | Canvas/WebGL viewers | Kern | mittel |
| Image Editor | Crop, Resize, Filters, Drawing, Undo und Export. | ImageSharp + UI/Commercial | Toast UI Image Editor/Fabric.js | optional | hoch |
| Media Player | Audio/Video, Playlist, Captions, Speed, Fullscreen und Streaming. | LibVLCSharp/MediaElement | HTML5 video/audio, video.js | Adapter | hoch |
| Barcode / QR | Erzeugen und Lesen verschiedener 1D/2D-Codes. | ZXing.Net/QRCoder | ZXing JS/qrcode libraries | Kernadapter | mittel |

### Dateien, Shell und Systemintegration

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| File Explorer | Verzeichnisbaum, Dateiliste, Vorschau, Kontextmenü, Operationen und Rechte. | Shell controls/Actipro/Custom | File Manager suites/elFinder + Backend | später | sehr hoch |
| Folder Tree | Hierarchische Dateisystemnavigation mit Lazy Loading und Icons. | TreeView + System.IO | Tree component + API | Kern für Tools | hoch |
| Recent Files | MRU-Liste, Pinning, fehlende Dateien und Datenschutz. | SASD Service + Menu | local/server state + list | Kern | niedrig |
| Drag and Drop Files | Import, Export, Reorder und visuelle Drop-Zonen. | WinForms/WPF DnD | HTML5 DnD/Uppy | Kern | mittel |
| Clipboard | Text, HTML, Bilder, Dateien und sichere Fehlerbehandlung. | System Clipboard | Clipboard API mit Permissions | Adapter | mittel |
| System Tray | Hintergrundbetrieb, Kontextmenü und Benachrichtigungen. | NotifyIcon | PWA/Web Notifications nur eingeschränkt | Desktop Adapter | mittel |
| Global Hotkeys | Systemweite Shortcuts und Konfliktbehandlung. | Win32/Library | Browser nur fokussierte Shortcuts | Desktop optional | hoch |
| Browser/WebView | Webinhalte oder Webkomponenten im Desktop hosten. | WebView2/CefSharp | nicht anwendbar | Adapter | hoch |

### Themes, Qualität und Plattformdienste

| Komponente | Ausführliche Funktion | Desktop-Kandidaten | Web-/AJAX-Kandidaten | SASD-Entscheidung | Aufwand |
| --- | --- | --- | --- | --- | --- |
| Design Tokens | Plattformneutrale Namen für Farben, Typografie, Abstände, Radien, Schatten und Motion. | JSON -> C#/XAML/Krypton Palette | JSON -> CSS variables/Tailwind config | Kern | hoch |
| Light/Dark Theme | Systemmodus, manuelle Auswahl, Kontrast und persistente Präferenz. | Theme Manager/Palettes | prefers-color-scheme + tokens | Kern | mittel |
| High Contrast | Erhöhte Kontraste, Systemfarben und sichtbarer Fokus. | Windows High Contrast | forced-colors/media queries | Kern | hoch |
| Accessibility | Name/Role/Value, Tastatur, Fokus, Screenreader, Live Regions und Kontrast. | UI Automation/AccessibleObject | ARIA/WCAG/semantic HTML | Kernqualität | sehr hoch |
| Localization | Übersetzbare Texte, Formate, RTL, Pluralformen und Laufzeitwechsel. | .resx/CultureInfo | i18n library/Intl/Blazor localization | Kern | hoch |
| RTL Layout | Rechts-nach-links, gespiegelte Navigation und bidirektionaler Text. | RightToLeft/FlowDirection | dir=rtl + logical CSS properties | später | hoch |
| High DPI / Scaling | Per-monitor DPI, Pixelraster, Icons und Layoutanpassung. | WinForms/WPF DPI APIs | Browser Zoom/DevicePixelRatio | Kern Desktop | hoch |
| Keyboard Navigation | Tab-Reihenfolge, Shortcuts, roving tabindex und Escape-Verhalten. | TabIndex/Commands | ARIA patterns/headless libs | Kernqualität | hoch |
| Command System | Befehle, CanExecute, Shortcuts, Icons, Telemetrie und Undo-Kopplung. | ICommand/SASD Command | Actions/hooks/command registry | Kern | hoch |
| Undo/Redo | Transaktionshistorie, Gruppierung, Dirty-State und Speichergrenzen. | Command/Memento | Editor/store history | Kern für Editoren | hoch |
| State Persistence | Fenster, Splitter, Gridspalten, Filter und Nutzerpräferenzen speichern. | JSON Settings/SQLite | localStorage/server profile | Kern | hoch |
| Telemetry Hooks | Nutzungs-, Fehler- und Performanceereignisse ohne UI-Abhängigkeit. | OpenTelemetry/EventSource | OpenTelemetry Web/RUM | Adapter | mittel |
| Visual Regression Tests | Screenshots gegen Referenz, Toleranzen und Plattformmatrix. | Appium/WinAppDriver/Playwright via desktop bridge | Playwright/Cypress | Kern Qualität | hoch |
| Accessibility Tests | Automatisierte Regeln plus manuelle Screenreader-/Tastaturtests. | Accessibility Insights/UIA tests | axe-core/Playwright | Kern Qualität | hoch |
| Component Gallery | Interaktive Demonstration aller Zustände, Varianten und Themes. | Demo-App | Storybook/Blazor Demo | Kernprojekt | mittel |
| API Documentation | Beispiele, Parameter, Events, Recipes, Migration und Versionierung. | DocFX | Storybook/Typedoc/DocFX | Kernprojekt | hoch |

## Vorlagen und Referenzanwendungen

Vorlagen sind mehr als optische Screenshots. Eine brauchbare Vorlage sollte Navigation, Datenfluss, Loading/Error/Empty States, Accessibility, Theme, Lokalisierung, Tests, Beispielservice und Build-/Updateprozess enthalten.

| Vorlage/Quelle | Nutzen | Ziel | Link |
| --- | --- | --- | --- |
| DevExpress UI Template Gallery | Kommerzielle Referenzvorlagen für Formulare, Navigation, Grid- und Dashboard-Szenarien. | Desktop/Web | https://www.devexpress.com/products/net/controls/winforms/ui-templates/ |
| Telerik Demo Applications | Hotel-, Dashboard-, CRM- und Controls-Demos mit Quellcode je Produkt. | Desktop/Web | https://www.telerik.com/support/demos |
| Kendo UI Building Blocks und Figma Kits | Vorgefertigte Seitenabschnitte, Themes und Designressourcen. | Web | https://www.telerik.com/kendo-ui |
| MudBlazor Templates | Blazor-Projektvorlagen und Layout-Grundlagen. | Web/Blazor | https://github.com/MudBlazor/Templates |
| Ant Design Pro / Pro Layout | Enterprise-Admin-Shell, Navigation und Standardseiten. | Web/React/Blazor-Portierungen | https://pro.ant.design/ |
| MUI Templates | Dashboard-, Sign-in-, Checkout-, Blog- und Marketingvorlagen. | Web/React | https://mui.com/store/ |
| HAVIT Blazor Templates | Einfache und Enterprise-Projektvorlagen mit Bootstrap und optionalem gRPC-Stack. | Web/Blazor | https://havit.blazor.eu/getting-started |
| AdminLTE | Freies Bootstrap-Admin-Dashboard mit wiederverwendbaren Seiten und Widgets. | Web | https://github.com/ColorlibHQ/AdminLTE |
| Tabler | Freies hochwertiges Bootstrap-Dashboard-UI-Kit. | Web | https://github.com/tabler/tabler |
| CoreUI Free Admin | Freie Adminvorlagen für Bootstrap, React, Angular und Vue. | Web | https://github.com/coreui/coreui-free-bootstrap-admin-template |
| Gentelella | Freies modernes Admin-Dashboard für interne Werkzeuge. | Web | https://github.com/ColorlibHQ/gentelella |
| TailAdmin | Tailwind-basierte freie und kommerzielle Dashboardvarianten. | Web | https://github.com/TailAdmin/free-tailwind-dashboard-template |
| Avalonia Templates | dotnet-new- und IDE-Vorlagen für Avalonia-Anwendungen. | Desktop Cross-Platform | https://github.com/AvaloniaUI/avalonia-dotnet-templates |
| Krypton Toolkit Examples | Beispiele für Ribbon, Docking, Navigator, Paletten und Controls. | Desktop WinForms | https://github.com/Krypton-Suite/Standard-Toolkit |
| Telerik WinForms Demo Application | Komponentengalerie und reale Beispielszenarien. | Desktop WinForms | https://www.telerik.com/products/winforms.aspx |

## Vorgeschlagener SASD-Vorlagenkatalog

| SASD-Vorlage | Enthaltene Oberflächenmuster | Primäre Verwendung |
| --- | --- | --- |
| SASD Desktop Standard Shell | Navigation links, CommandBar oben, Content-Bereich, Statusleiste, Theme, Logging, Settings und About. | WinForms zuerst; später WPF/Avalonia |
| SASD Workbench / IDE Shell | Dockbare Toolfenster, Dokument-Tabs, Command-Palette, Output/Log, Properties und persistente Layouts. | Prompt Manager, Notes, Mail Workbench, Entwicklerwerkzeuge |
| SASD Master-Detail CRUD | Such-/Filterleiste, Grid, Detailformular, Validierung, Audit-Historie und Export. | Datenbank- und Verwaltungsanwendungen |
| SASD Data Explorer | Verbindungs-/Quellenauswahl, Tree, Grid, Query/Filter, Chart, Export und Details. | DBA-, Monitoring- und Analysewerkzeuge |
| SASD Monitoring Dashboard | KPI-Karten, Zeitreihen, Alarme, Filter, Zeitbereich, Auto-Refresh und Drilldown. | Desktop und Web |
| SASD Mail-/Message-Client | Ordnerbaum, Nachrichtenliste, Preview, Suche, Filter, Composer und Benachrichtigungsregeln. | SASD Mail Workbench |
| SASD Settings Center | Kategorienavigation, Settings-Form, Suche, Defaults, Import/Export und sichere Secret-Verweise. | Alle Produkte |
| SASD Wizard / Installer | Schritte, Voraussetzungen, Validierung, Dry Run, Fortschritt, Rollback und Abschlussbericht. | Serverinstallationen und Konfiguration |
| SASD Report Center | Reportliste, Parameter, Vorschau, Zeitplanung, Export und Historie. | Desktop/Web |
| SASD Project Planner | TreeGrid, Gantt, Kanban, Ressourcen, Abhängigkeiten und Statusberichte. | Spätere Produktlinie |
| SASD Web Admin Portal | Responsive Sidebar, Header, Breadcrumb, CRUD, Rollen/Rechte, Audit und Notifications. | Blazor oder TypeScript |
| SASD Web Data Workspace | Serverseitiges Grid, Filter Builder, gespeicherte Views, Charts und Exportjobs. | Datenintensive Webanwendungen |
| SASD Knowledge Base | Baumnavigation, Markdown/Rich Text, Suche, Tags, Backlinks, Versionen und Mermaid. | Notes und Dokumentation |
| SASD Security Console | Assets, Findings, Severity, Timeline, Evidence, Remediation, Reports und RBAC. | Security-Produkte |
| SASD Component Gallery | Alle Controls, Zustände, Themes, Accessibility, Events, Performance und Codebeispiele. | Pflichtprojekt der Plattform |

### Mindestinhalt jeder SASD-Vorlage

- lauffähiges Beispiel mit realistischen Beispieldaten;
- Light/Dark und mindestens ein High-Contrast-Prüfszenario;
- Loading-, Empty-, Partial-, Offline- und Error-State;
- Tastatur- und Fokusbeschreibung;
- strukturierte Logging- und Fehlerbehandlung;
- Konfigurations- und Secret-Abgrenzung;
- Unit-, Komponenten- und mindestens ein End-to-End-Test;
- Screenshot und kurze Architekturentscheidung;
- Update-/Migrationshinweise für verwendete Fremdpakete;
- `THIRD-PARTY-NOTICES.md` und maschinenlesbare SBOM.

## Zielarchitektur der SASD UI Platform

```text
SASD.UI
├── Sasd.Ui.Contracts
│   ├── Commands, Validation, Selection, Paging, Sorting, Filtering
│   └── plattformneutrale Komponentenmodelle
├── Sasd.Ui.DesignTokens
│   ├── tokens.json
│   ├── Generator für C#/XAML/Krypton-Paletten
│   └── Generator für CSS Variables/Tailwind
├── Sasd.Ui.Icons
├── Sasd.Ui.Localization
├── Sasd.Ui.Accessibility
├── Sasd.Ui.Desktop
│   ├── WinForms
│   ├── Wpf
│   ├── Avalonia
│   └── Adapters (Krypton, Charts, Maps, Editors, Reporting)
├── Sasd.Ui.Web
│   ├── Blazor
│   ├── WebComponents oder TypeScript
│   └── Adapters (Grid, Charts, Calendar, Editor, Upload)
├── Sasd.Ui.Templates
├── Sasd.Ui.ComponentGallery.Desktop
├── Sasd.Ui.ComponentGallery.Web
└── Sasd.Ui.Tests
    ├── Contracts
    ├── Accessibility
    ├── VisualRegression
    └── Integration
```

### Adapterprinzip

Ein SASD-Adapter kapselt nur dort, wo ein stabiler eigener Vertrag echten Nutzen bringt. Ein universelles `ISasdControl` wäre zu abstrakt. Sinnvoll sind fachlich schmale Verträge wie `ISasdChart`, `IDataGridStateStore`, `IFilePicker`, `INotificationService` oder `IMarkdownEditor`. Plattformtypische Fähigkeiten dürfen über optionale Interfaces oder konkrete Erweiterungen erreichbar bleiben.

### Beispiel eines plattformneutralen Grid-Zustands

```csharp
public sealed record SasdGridState(
    IReadOnlyList<SasdSortRule> SortRules,
    SasdFilterExpression? Filter,
    IReadOnlyList<SasdColumnState> Columns,
    int PageSize,
    string? ContinuationToken);
```

Desktop und Web können diesen Zustand speichern und austauschen, ohne dass eine WinForms-Spalte oder ein DOM-Element in den gemeinsamen Vertrag gelangt.

## Entwicklungsregeln für Desktop-Komponenten

1. **Designerfähigkeit bewusst entscheiden.** Nicht jedes Control benötigt Visual-Studio-Designerunterstützung. Für häufig platzierte Controls erhöht sie aber die Produktivität erheblich.
2. **UI-Thread nicht blockieren.** Lange Datei-, Netzwerk- und Datenoperationen laufen asynchron; UI-Updates werden kontrolliert marshalled.
3. **High DPI und mehrere Monitore testen.** Dialoge dürfen beim Monitorwechsel nicht falsch skalieren oder außerhalb des sichtbaren Bereichs erscheinen.
4. **Tastatur zuerst.** Tab-Reihenfolge, Access Keys, Shortcuts, Fokusrahmen und Screenreader-Namen gehören zur Definition of Done.
5. **Layoutzustände versionieren.** Gespeicherte Gridspalten oder Dockinglayouts müssen bei Versionswechsel migriert oder sicher verworfen werden können.
6. **Ressourcen deterministisch freigeben.** Bilder, Handles, Timer, Browser, Dateiwatcher und Events dürfen keine Leaks erzeugen.
7. **Rendering nicht unnötig selbst schreiben.** Owner Drawing nur mit messbarem Nutzen und visuellen Regressionstests einsetzen.
8. **Offline- und Fehlerbetrieb vorsehen.** Desktopsoftware muss ohne Netzwerk kontrolliert starten und aussagekräftige Diagnosen liefern.
9. **Komponenten klein halten.** Application Shell, Grid Utilities, Dialoge und Services in getrennten Paketen veröffentlichen.
10. **Kommerzielle Adapter isolieren.** Anwendungen ohne Lizenz müssen weiterhin gegen freie Adapter gebaut werden können.

## Entwicklungsregeln für Web-/AJAX-Komponenten

1. **Semantisches HTML vor ARIA-Reparatur.** Native Elemente sind die Ausgangsbasis; ARIA ergänzt nur fehlende Semantik.
2. **Responsiveness ist Komponentenverhalten.** Nicht nur Seiten, auch Grids, Dialoge und Navigationsbereiche benötigen definierte kleine Ansichten.
3. **Netzwerkzustände modellieren.** Loading, Retry, Timeout, Partial Data, Offline und Stale Data sind sichtbare Zustände.
4. **Serverseitige Datenoperationen standardisieren.** Sortierung, Filter, Paging und Continuation Tokens erhalten ein gemeinsames Request-/Response-Modell.
5. **Hydration und Blazor-Server-Latenz testen.** Doppelte Events, kurzzeitig nicht interaktive Controls und hohe Roundtrip-Zahlen vermeiden.
6. **Bundlegröße überwachen.** Spezialkomponenten lazy laden; keine zweite vollständige UI-Suite für ein einzelnes Control einbinden.
7. **Frameworkunabhängigkeit gezielt einsetzen.** Web Components oder Headless Engines nur dort nutzen, wo mehrere Frontend-Stacks realistisch sind.
8. **Sicherheit standardisieren.** XSS-sichere Ausgabe, Content Security Policy, Upload-Prüfung, CSRF/Antiforgery und sichere URL-Behandlung gehören zu jeder Komponente.
9. **Browser- und Eingabematrix definieren.** Chromium, Firefox und Safari sowie Tastatur, Maus und Touch entsprechend Zielgruppe testen.
10. **URL und History respektieren.** Navigation, Tabs und Filter sollen bei sinnvoller Produktlogik bookmarkfähig sein.

## Qualitätssicherung

| Qualitätsbereich | Pflichtprüfungen |
| --- | --- |
| API-Qualität | Konsistente Namen, Nullable-Annotationen, Events, async APIs, Exceptions, Versionierung und Beispiele. |
| Accessibility | WCAG-orientierte Webtests, UI-Automation auf Desktop, Tastaturabläufe, Fokus, Screenreader und Kontrast. |
| Visual Regression | Alle Themes, DPI/Zoom, States, Validierungsfehler, lange Texte, Lokalisierung und RTL-Szenarien. |
| Performance | Startzeit, Speicher, Scrollen, 10k/100k Datensätze, Streaming, Resize, Themewechsel und Netzwerk-Latenz. |
| Robustheit | Leere/null Daten, kaputte Dateien, Serverfehler, Abbruch, Retry, Parallelaktionen und Wiederherstellung. |
| Security | Dependency Scan, SBOM, Signierung, sichere Defaults, XSS/Injection, Upload, URL- und Dateipfadbehandlung. |
| Kompatibilität | Unterstützte .NET-/Browser-/OS-Versionen, Upgradepfad und automatisierte Matrix. |
| Dokumentation | Getting Started, vollständige API, Recipes, bekannte Grenzen, Migrationsleitfaden und Beispielapp. |

### Definition of Done für eine SASD-Komponente

- stabiler öffentlicher API-Vertrag;
- Light/Dark/High-Contrast geprüft;
- Tastatur und Accessibility dokumentiert;
- lokalisierbare Texte ohne Hardcoding;
- Loading/Empty/Error/Disabled/Readonly abgedeckt;
- Unit- und Komponenten-/UI-Tests vorhanden;
- Beispiel in der Component Gallery;
- Lizenz- und Herkunftsnachweis vollständig;
- Performancebudget eingehalten;
- Changelog und Migrationshinweis vorhanden.

## Fork-, Upstream- und Lieferantenstrategie

### Eskalationsreihenfolge

```text
unveränderte Abhängigkeit
        ↓
Konfiguration / Styling / Composition
        ↓
SASD-Adapter oder Erweiterungsmethode
        ↓
Upstream-Issue und Pull Request
        ↓
temporärer Patch-Fork mit Rebase-Plan
        ↓
dauerhafter SASD-Fork nur mit Wartungsbudget
```

### Kriterien für einen dauerhaften Fork

- permissive und eindeutig geprüfte Lizenz;
- hohe strategische Bedeutung für mindestens mehrere SASD-Produkte;
- Upstream ist unmaintained oder lehnt notwendige, allgemeingültige Änderungen ab;
- automatisierte Tests decken Kernfunktionen, Accessibility und Rendering ab;
- benannter Maintainer und Releaseprozess;
- dokumentierte Abweichungen vom Upstream;
- Security- und Updateverantwortung ist finanziell tragbar;
- Markenname und Paketidentität verletzen keine Rechte des Ursprungsprojekts.

### Lieferantenbewertung

Jeder externe Baustein erhält eine Scorecard aus Aktivität, Bus-Faktor, Lizenzklarheit, Dokumentation, Testabdeckung, Releasehäufigkeit, Security Policy, .NET-/Browserunterstützung, Accessibility, API-Stabilität, Paketgröße, Performance und Migrationskosten. Der höchste Funktionsumfang allein entscheidet nicht.


## Zurückgestellte separate Projekte

### SASD ASP.NET Web Forms Compatibility

Dieser Katalog führt Web Forms weiterhin, aber nicht als moderne Webplattform. Ein späteres Projekt sollte umfassen:

- Inventar von `.aspx`, Master Pages, User Controls, Server Controls, ViewState, PostBack und UpdatePanel,
- Funktionsmatrix des AJAX Control Toolkit und möglicher Drittanbieter wie Telerik UI for ASP.NET AJAX,
- Sicherheits-, Browser- und Wartungsrisiken alter Anwendungen,
- Migrationspfade zu ASP.NET Core, Blazor oder getrenntem JavaScript-Frontend,
- Adapter nur dort, wo bestehende Kundenanwendungen dies rechtfertigen.

### SASD Java Business UI

OpenXava wird zusammen mit Vaadin, OpenUI5, PrimeFaces, Jakarta Faces und JavaFX erneut untersucht. Der spätere Katalog muss unterscheiden zwischen:

- modellgetriebenen CRUD-/Business-Application-Frameworks,
- serverseitigen Java-Webframeworks,
- clientseitigen Web-Component-Systemen,
- Java-Desktopframeworks,
- freien Community- und kommerziellen Pro-Editionen.

Diese Trennung verhindert, dass Java-Frameworkentscheidungen die C#-WinForms-Architektur vorzeitig verkomplizieren.


## Roadmap

> **Verbindliche Reihenfolge:** WinForms → WPF → WinUI-3/Avalonia-Vergleich → modernes Web; Web Forms und Java separat.

| Phase | Inhalt | Ergebnis |
| --- | --- | --- |
| 0 – Inventar und Governance | Lizenzregister, Design Tokens, Namenskonventionen, Scorecard und Component Gallery Skeleton. | Kein produktiver Fork; belastbare Entscheidungsbasis. |
| 1 – Desktop Foundation | Krypton-basierte Shell, Dialoge, Notifications, Settings, Grid Utilities, State Persistence und ScottPlot-Adapter. | Wiederverwendbares internes NuGet-Paket für bestehende SASD-WinForms-Projekte. |
| 2 – Web Foundation | Blazor-Pilot MudBlazor vs. Radzen, Tailwind/daisyUI-Prototyp, Grid- und Chartvergleich. | Festgelegter Webstack und responsive Admin-Shell. |
| 3 – Gemeinsame Standards | Token-Generatoren, Localization, Accessibility, Commands, Validation und Telemetry Contracts. | Konsistente Desktop-/Web-Produktfamilie ohne falschen Einheitsrenderer. |
| 4 – Vorlagen | CRUD, Workbench, Monitoring, Settings, Report Center und Web Admin Portal. | Schneller Projektstart mit getesteten Mustern. |
| 5 – Spezialadapter | Mapsui/Leaflet, Codeeditor, Reporting, Upload, Calendar/Gantt und Diagramme nach Bedarf. | Erweiterbares Ökosystem ohne Mega-Suite. |
| 6 – Öffentliche Plattform | SemVer, NuGet/npm, Website, Demos, Changelog, SBOM und Contribution Guide. | Open-Source-Veröffentlichung mit professioneller Governance. |
| 7 – Kommerzielle Angebote | LTS, Support, Migration, kundenspezifische Themes, Audits und Schulungen. | Nachhaltiges Geschäftsmodell statt Verkauf bloßer Forks. |

## Empfohlene erste Technologieauswahl

| Bedarf | Primäre Empfehlung | Alternative | Nicht in V1 selbst bauen |
| --- | --- | --- | --- |
| WinForms-Basis | Krypton Standard Toolkit | ReaLTaiizor punktuell | vollständige eigene Theme-/Ribbon-Suite |
| WPF-Basis | WPF UI oder MahApps + PropertyTools | MaterialDesignInXaml/HandyControl | eigene vollständige Controlsuite |
| Cross-Platform Desktop | Avalonia + FluentAvalonia/Semi+Ursa | Uno Platform | eigener Renderer |
| Desktop Docking | Krypton Docking oder DockPanel Suite | AvalonDock für WPF | neue Dockingengine |
| Desktop Charts | ScottPlot | LiveCharts2/OxyPlot | GPU-Hochleistungsengine |
| Desktop Karten | Mapsui | Webkarte in WebView | eigene GIS-Engine |
| Blazor Suite | MudBlazor oder Radzen nach Pilot | Ant Design Blazor/HAVIT | komplette eigene Suite vor Produktbedarf |
| Web Designsystem | Tailwind + daisyUI oder Headless UI | Bootstrap/Flowbite | zweites paralleles Designsystem |
| Web DataGrid | AG Grid Community oder Tabulator | TanStack Table für volle UI-Kontrolle | Pivot/OLAP-Grid |
| Web Charts | Apache ECharts | Chart.js/Plotly.js | eigene Chartengine |
| Kalender | FullCalendar Standard | Suite-Komponente | Ressourcen-Scheduler |
| Gantt | DHTMLX Gantt Community für Pilot | Frappe Gantt | vollständige PM-Engine |
| Rich Text Web | Tiptap oder Quill | Lexical | DOCX-kompatibler Word-Prozessor |
| Codeeditor | ScintillaNET/AvalonEdit und Monaco/CodeMirror | — | eigene Syntaxengine |
| Reporting | QuestPDF/PDFsharp oder FastReport OSS | kommerzielle Suite | visueller Reportdesigner |

## Recherchegrenzen und Pflegeprozess

Dieser Katalog ist breit angelegt, aber der Markt verändert sich fortlaufend. Neue Releases, Lizenzwechsel, Archivierungen und Übernahmen können Bewertungen kurzfristig ändern. Besonders Open-Core-Produkte, Community-Lizenzen und Projekte mit kommerziellen Plugins müssen vor jeder produktiven Entscheidung erneut geprüft werden.

### Empfohlener Aktualisierungszyklus

- **monatlich:** Security Advisories und kritische Abhängigkeiten;
- **quartalsweise:** Releases, .NET-/Browser-Support und Lizenzänderungen;
- **halbjährlich:** Scorecard und Technologieentscheidungen;
- **vor jedem Major Upgrade:** Migrationsleitfäden, Breaking Changes und Paketlizenzen;
- **vor Veröffentlichung/Vertrieb:** vollständige juristische und technische Third-Party-Prüfung.

### Bewusst nicht als „vollständig“ behauptet

Kein statisches Dokument kann jede kleine Nischenkomponente, jeden aufgegebenen Fork und jedes neue kommerzielle Produkt enthalten. Aufgenommen wurden die für .NET-Desktop, Blazor, klassische AJAX/Web-Forms-Modernisierung und moderne Webanwendungen relevantesten Suiten sowie repräsentative Spezialbibliotheken. Weitere Kandidaten sollten über die gleiche Scorecard ergänzt werden, statt unkritisch in den Kern zu gelangen.

## Quellenverzeichnis

Die Recherche bevorzugt offizielle Produktseiten, Dokumentationen und Primär-Repositories. Abruf- und Bewertungsstand ist der 23. Juli 2026.

- **DevExpress Universal:** [https://www.devexpress.com/subscriptions/universal.xml](https://www.devexpress.com/subscriptions/universal.xml)
- **DevExpress WinForms:** [https://www.devexpress.com/products/net/controls/winforms/](https://www.devexpress.com/products/net/controls/winforms/)
- **DevExpress WPF:** [https://www.devexpress.com/products/net/controls/wpf/](https://www.devexpress.com/products/net/controls/wpf/)
- **DevExpress Blazor:** [https://www.devexpress.com/blazor/](https://www.devexpress.com/blazor/)
- **DevExtreme GitHub:** [https://github.com/DevExpress/DevExtreme](https://github.com/DevExpress/DevExtreme)
- **Telerik WinForms:** [https://www.telerik.com/products/winforms.aspx](https://www.telerik.com/products/winforms.aspx)
- **Telerik WPF:** [https://www.telerik.com/products/wpf/overview.aspx](https://www.telerik.com/products/wpf/overview.aspx)
- **Telerik ASP.NET AJAX:** [https://www.telerik.com/products/aspnet-ajax.aspx](https://www.telerik.com/products/aspnet-ajax.aspx)
- **Kendo UI:** [https://www.telerik.com/kendo-ui](https://www.telerik.com/kendo-ui)
- **Syncfusion Essential Studio:** [https://www.syncfusion.com/products/essential-studio](https://www.syncfusion.com/products/essential-studio)
- **ComponentOne:** [https://developer.mescius.com/componentone](https://developer.mescius.com/componentone)
- **Wijmo:** [https://developer.mescius.com/wijmo](https://developer.mescius.com/wijmo)
- **Infragistics Ultimate:** [https://www.infragistics.com/products/ultimate](https://www.infragistics.com/products/ultimate)
- **Actipro Controls:** [https://www.actiprosoftware.com/products/controls](https://www.actiprosoftware.com/products/controls)
- **Xceed:** [https://xceed.com/](https://xceed.com/)
- **Nevron Open Vision:** [https://www.nevron.com/dotnet-controls](https://www.nevron.com/dotnet-controls)
- **SciChart:** [https://www.scichart.com/](https://www.scichart.com/)
- **LightningChart:** [https://lightningchart.com/](https://lightningchart.com/)
- **Steema:** [https://www.steema.com/](https://www.steema.com/)
- **Wisej.NET:** [https://wisej.com/](https://wisej.com/)
- **AJAX Control Toolkit:** [https://github.com/DevExpress/AjaxControlToolkit](https://github.com/DevExpress/AjaxControlToolkit)
- **Sencha Ext JS:** [https://www.sencha.com/products/extjs/](https://www.sencha.com/products/extjs/)
- **DHTMLX:** [https://dhtmlx.com/](https://dhtmlx.com/)
- **Bryntum:** [https://bryntum.com/](https://bryntum.com/)
- **AG Grid:** [https://www.ag-grid.com/](https://www.ag-grid.com/)
- **Krypton Toolkit:** [https://github.com/Krypton-Suite/Standard-Toolkit](https://github.com/Krypton-Suite/Standard-Toolkit)
- **Avalonia UI:** [https://github.com/AvaloniaUI/Avalonia](https://github.com/AvaloniaUI/Avalonia)
- **WPF UI:** [https://github.com/lepoco/wpfui](https://github.com/lepoco/wpfui)
- **MahApps.Metro:** [https://github.com/MahApps/MahApps.Metro](https://github.com/MahApps/MahApps.Metro)
- **MaterialDesignInXaml:** [https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- **HandyControl:** [https://github.com/HandyOrg/HandyControl](https://github.com/HandyOrg/HandyControl)
- **AvalonDock:** [https://github.com/Dirkster99/AvalonDock](https://github.com/Dirkster99/AvalonDock)
- **Ursa.Avalonia:** [https://github.com/irihitech/Ursa.Avalonia](https://github.com/irihitech/Ursa.Avalonia)
- **ScottPlot:** [https://github.com/ScottPlot/ScottPlot](https://github.com/ScottPlot/ScottPlot)
- **LiveCharts2:** [https://github.com/beto-rodriguez/LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2)
- **Mapsui:** [https://github.com/Mapsui/Mapsui](https://github.com/Mapsui/Mapsui)
- **FastReport Open Source:** [https://github.com/FastReports/FastReport](https://github.com/FastReports/FastReport)
- **MudBlazor:** [https://github.com/MudBlazor/MudBlazor](https://github.com/MudBlazor/MudBlazor)
- **Radzen Blazor:** [https://github.com/radzenhq/radzen-blazor](https://github.com/radzenhq/radzen-blazor)
- **Ant Design Blazor:** [https://github.com/ant-design-blazor/ant-design-blazor](https://github.com/ant-design-blazor/ant-design-blazor)
- **HAVIT Blazor:** [https://github.com/havit/Havit.Blazor](https://github.com/havit/Havit.Blazor)
- **BootstrapBlazor:** [https://github.com/dotnetcore/BootstrapBlazor](https://github.com/dotnetcore/BootstrapBlazor)
- **daisyUI:** [https://github.com/saadeghi/daisyui](https://github.com/saadeghi/daisyui)
- **MUI:** [https://github.com/mui/material-ui](https://github.com/mui/material-ui)
- **Ant Design:** [https://github.com/ant-design/ant-design](https://github.com/ant-design/ant-design)
- **Chakra UI:** [https://github.com/chakra-ui/chakra-ui](https://github.com/chakra-ui/chakra-ui)
- **Mantine:** [https://github.com/mantinedev/mantine](https://github.com/mantinedev/mantine)
- **shadcn/ui:** [https://github.com/shadcn-ui/ui](https://github.com/shadcn-ui/ui)
- **Radix UI:** [https://github.com/radix-ui/primitives](https://github.com/radix-ui/primitives)
- **Carbon:** [https://github.com/carbon-design-system/carbon](https://github.com/carbon-design-system/carbon)
- **PatternFly:** [https://github.com/patternfly/patternfly-react](https://github.com/patternfly/patternfly-react)
- **TanStack Table:** [https://github.com/TanStack/table](https://github.com/TanStack/table)
- **Tabulator:** [https://github.com/olifolkerd/tabulator](https://github.com/olifolkerd/tabulator)
- **Grid.js:** [https://github.com/grid-js/gridjs](https://github.com/grid-js/gridjs)
- **Chart.js:** [https://github.com/chartjs/Chart.js](https://github.com/chartjs/Chart.js)
- **Apache ECharts:** [https://github.com/apache/echarts](https://github.com/apache/echarts)
- **Plotly.js:** [https://github.com/plotly/plotly.js](https://github.com/plotly/plotly.js)
- **FullCalendar:** [https://github.com/fullcalendar/fullcalendar](https://github.com/fullcalendar/fullcalendar)
- **Tiptap:** [https://github.com/ueberdosis/tiptap](https://github.com/ueberdosis/tiptap)
- **Quill:** [https://github.com/slab/quill](https://github.com/slab/quill)
- **Lexical:** [https://github.com/facebook/lexical](https://github.com/facebook/lexical)
- **Monaco Editor:** [https://github.com/microsoft/monaco-editor](https://github.com/microsoft/monaco-editor)
- **Mermaid:** [https://github.com/mermaid-js/mermaid](https://github.com/mermaid-js/mermaid)
- **React Flow:** [https://github.com/xyflow/xyflow](https://github.com/xyflow/xyflow)
- **Leaflet:** [https://github.com/Leaflet/Leaflet](https://github.com/Leaflet/Leaflet)
- **OpenLayers:** [https://github.com/openlayers/openlayers](https://github.com/openlayers/openlayers)
- **MapLibre GL JS:** [https://github.com/maplibre/maplibre-gl-js](https://github.com/maplibre/maplibre-gl-js)
- **AdminLTE:** [https://github.com/ColorlibHQ/AdminLTE](https://github.com/ColorlibHQ/AdminLTE)
- **Tabler:** [https://github.com/tabler/tabler](https://github.com/tabler/tabler)
- **CoreUI Free:** [https://github.com/coreui/coreui-free-bootstrap-admin-template](https://github.com/coreui/coreui-free-bootstrap-admin-template)
- **MindFusion:** [https://mindfusion.dev/](https://mindfusion.dev/)
- **Flexmonster:** [https://www.flexmonster.com/](https://www.flexmonster.com/)
- **DayPilot:** [https://javascript.daypilot.org/](https://javascript.daypilot.org/)
- **GoJS:** [https://gojs.net/](https://gojs.net/)
- **DotNetBar:** [https://marketplace.visualstudio.com/items?itemName=DevCo.DotNetBarforWindowsForms](https://marketplace.visualstudio.com/items?itemName=DevCo.DotNetBarforWindowsForms)
- **OpenUI5:** [https://openui5.org/](https://openui5.org/)
- **Vaadin Components:** [https://vaadin.com/components](https://vaadin.com/components)
- **Webix:** [https://webix.com/](https://webix.com/)
- **Ionic Framework:** [https://ionicframework.com/](https://ionicframework.com/)
- **Angular Material:** [https://material.angular.dev/](https://material.angular.dev/)
- **Vuetify:** [https://vuetifyjs.com/](https://vuetifyjs.com/)
- **Quasar Framework:** [https://quasar.dev/](https://quasar.dev/)
- **Element Plus:** [https://element-plus.org/](https://element-plus.org/)
- **UIkit:** [https://getuikit.com/](https://getuikit.com/)
- **Bulma:** [https://bulma.io/](https://bulma.io/)
- **Web Awesome:** [https://www.webawesome.com/](https://www.webawesome.com/)
- **React-admin:** [https://marmelab.com/react-admin/](https://marmelab.com/react-admin/)
- **Refine:** [https://refine.dev/](https://refine.dev/)
- **FINOS Perspective:** [https://perspective.finos.org/](https://perspective.finos.org/)
- **RevoGrid:** [https://revolist.github.io/revogrid/](https://revolist.github.io/revogrid/)
- **SlickGrid:** [https://github.com/6pac/SlickGrid](https://github.com/6pac/SlickGrid)
- **CKEditor 5:** [https://ckeditor.com/ckeditor-5/](https://ckeditor.com/ckeditor-5/)
- **TinyMCE:** [https://www.tiny.cloud/](https://www.tiny.cloud/)
- **PDF.js:** [https://mozilla.github.io/pdf.js/](https://mozilla.github.io/pdf.js/)
- **Golden Layout:** [https://golden-layout.com/](https://golden-layout.com/)
- **GridStack.js:** [https://gridstackjs.com/](https://gridstackjs.com/)
- **Fabric.js:** [https://fabricjs.com/](https://fabricjs.com/)
- **Konva:** [https://konvajs.org/](https://konvajs.org/)
- **maxGraph:** [https://github.com/maxGraph/maxGraph](https://github.com/maxGraph/maxGraph)
- **TOAST UI:** [https://ui.toast.com/](https://ui.toast.com/)
- **jsreport:** [https://jsreport.net/](https://jsreport.net/)
- **AJAX Control Toolkit Legacy Site:** [https://www.ajaxtoolkit.net/](https://www.ajaxtoolkit.net/)
- **AntdUI:** [https://github.com/AntdUI/AntdUI](https://github.com/AntdUI/AntdUI)
- **AcrylicUI:** [https://github.com/colhountech/AcrylicUI](https://github.com/colhountech/AcrylicUI)
- **KGySoft.WinForms:** [https://github.com/koszeggy/KGySoft.WinForms](https://github.com/koszeggy/KGySoft.WinForms)
- **AdvancedDataGridView:** [https://github.com/davidegironi/advanceddatagridview](https://github.com/davidegironi/advanceddatagridview)
- **Ookii.Dialogs.WinForms:** [https://github.com/ookii-dialogs/ookii-dialogs-winforms](https://github.com/ookii-dialogs/ookii-dialogs-winforms)
- **Cyotek Open Source / ImageBox:** [https://www.cyotek.com/open-source](https://www.cyotek.com/open-source), [https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox](https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox)
- **Microsoft WebView2:** [https://learn.microsoft.com/microsoft-edge/webview2/](https://learn.microsoft.com/microsoft-edge/webview2/)
- **OpenXava Features:** [https://www.openxava.org/features/](https://www.openxava.org/features/)
- **Awesome .NET WinForms Libraries:** [https://github.com/tbolon/awesome-dotnet-winforms](https://github.com/tbolon/awesome-dotnet-winforms)
- **SourceForge/Slashdot DevExpress Alternatives:** [https://sourceforge.net/software/product/DevExpress/alternatives](https://sourceforge.net/software/product/DevExpress/alternatives), [https://slashdot.org/software/p/DevExpress/alternatives](https://slashdot.org/software/p/DevExpress/alternatives)
- **Jon Hilton Blazor Component Libraries:** [https://jonhilton.net/blazor-component-libraries/](https://jonhilton.net/blazor-component-libraries/)
- **EDUCBA DevExpress Alternatives:** [https://www.educba.com/devexpress-alternative/](https://www.educba.com/devexpress-alternative/)

## Änderungsprotokoll Version 2

- Strategische Reihenfolge auf WinForms → WPF → WinUI 3/Avalonia festgelegt.
- ASP.NET Web Forms und Java/OpenXava als getrennte spätere Projekte abgegrenzt.
- Nachgereichte Quellen dedupliziert und nach Quellenqualität bewertet.
- Startgewichtung, Prioritätsmodell und einheitlicher Pilot-Test eingeführt.
- Alle 123 Komponentenklassen mit einer WinForms-Startpriorität versehen.
- Krypton, ReaLTaiizor, Syncfusion, Avalonia, AJAX Control Toolkit und OpenXava vertieft.
- AntdUI, AcrylicUI, KGySoft.WinForms, AdvancedDataGridView, Ookii.Dialogs, Cyotek Controls, Material3.WinForms, SunnyUI, WebView2 und die Awesome-WinForms-Recherchequelle ergänzt.
- Keine der neu aufgenommenen Bewertungen ersetzt eine technische oder juristische Einzelprüfung.

---

**Dokumentende – SASD UI-Komponenten-Katalog, Version 2, Stand 23.07.2026**
