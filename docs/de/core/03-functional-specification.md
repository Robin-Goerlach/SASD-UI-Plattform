# SASD UI Platform - Pflichtenheft für C#-WinForms-Komponenten {.unnumbered}

**Technische Realisierung einer wiederverwendbaren Windows-Desktop-Komponentenbibliothek**

| Merkmal | Festlegung |
| --- | --- |
| Version | 0.1 |
| Stand | 23. Juli 2026 |
| Status | Technischer Entwurf zur Umsetzungsfreigabe |
| Auftragnehmer/Umsetzung | SASD-GmbH |
| Produktlinie | Windows Desktop / C# / WinForms |
| Grundlage | `SASD_UI-Platform_Lastenheft_WinForms-Komponenten_v0.1_2026-07-23` |
| Zielrahmen | R0-Entscheidungspilot, R1-produktives Fundament, R2-Fachmodule, R3-optionale Module |

> Dieses Pflichtenheft beschreibt, **wie** die im Lastenheft definierten 20 Komponentenfamilien und 83 Anforderungen technisch umgesetzt, geprüft, paketiert und eingeführt werden. Es ersetzt nicht die bewussten Abgrenzungen des Lastenhefts.

```{=openxml}
<w:p><w:r><w:br w:type="page"/></w:r></w:p>
```
# Inhaltsübersicht {.unnumbered}

| Kapitel 1-12 | Kapitel 13-24 |
| --- | --- |
| 1. Management-Zusammenfassung | 13. Windows-, Datei- und Systemintegration |
| 2. Bezug zum Lastenheft und Umsetzungsziele | 14. R2-Fachmodule |
| 3. Technische Rahmenbedingungen | 15. Querschnittsanforderungen und Qualitätssicherung |
| 4. Architekturprinzipien | 16. Build, CI/CD, Versionierung und Governance |
| 5. Repository-, Solution- und Paketstruktur | 17. Component Gallery, Vorlagen und Einführung |
| 6. Öffentliche API, Konventionen und Fehlerverträge | 18. Umsetzungsplan und Freigabegates |
| 7. Designsystem, Themes und Icons | 19. Risiken und technische Gegenmaßnahmen |
| 8. Basiskomponenten und Formularsystem | 20. Gesamtabnahme |
| 9. Komponenten- und Servicekatalog | 21. Zukunftsregister und bewusste Nichtziele |
| 10. Application Shell, Commands und Navigation | 22. Rückverfolgbarkeitsmatrix |
| 11. Datenanzeige: Grid, Listen, Bäume und Paging | 23. Komponentenfamilien F01-F20 |
| 12. Dialoge, Feedback, Fortschritt und Fehler | 24. Dokumentenlenkung und Änderungsprotokoll |

```{=openxml}
<w:p><w:r><w:br w:type="page"/></w:r></w:p>
```
# 1. Management-Zusammenfassung

Die SASD UI Platform wird als modularer Monorepo-Verbund für C#-WinForms-Anwendungen realisiert. Der erste produktive Stand konzentriert sich auf eine kleine, robuste und designerfähige Komponentenbasis. Microsoft WinForms bleibt der Plattformkern; Krypton Standard Toolkit wird in R0 als erste sichtbare Implementierung erprobt. Fremdbibliotheken werden nicht zusammenkopiert, sondern hinter klaren SASD-Verträgen oder in ausdrücklich benannten Adapterpaketen verwendet.

Die technische Hauptentscheidung lautet: **Eigene SASD-Komponenten werden nur dort entwickelt, wo wiederverwendbares Verhalten, Qualitätssicherung oder ein stabiler Vertrag entsteht.** Standard-Controls wie Button, Label oder TextBox werden nicht pauschal durch dutzende dünne Wrapper ersetzt. Stattdessen liefern Theme-, FormLayout-, Command-, Validation- und State-Schichten den gemeinsamen Mehrwert. Eigene Controls entstehen insbesondere für Shell, Navigation, Grid-Komfort, Fehlerrückmeldung, Busy-/Empty-Zustände und wiederkehrende Workbench-Strukturen.

| Entscheidung | Festlegung |
| --- | --- |
| Technologiebasis | .NET 8 (`net8.0-windows`) und Windows Forms; Prüfung einer aktuellen LTS in CI, aber kein unnötiges Multi-Targeting in R1. |
| Sichtbares UI-Fundament | Krypton-Pilot als Standard-Theme und Control-Basis; sauberer Rückfall auf native WinForms-Controls bleibt möglich. |
| Repository | Ein Monorepo mit getrennten NuGet-Paketen, Beispielen, Tests, Dokumentation und zentraler Versionsverwaltung. |
| Öffentliche API | SASD-Typen und .NET-BCL-Typen; Fremdtypen nur innerhalb ausdrücklich gekennzeichneter Adapterpakete. |
| Persistenz | Versionierte JSON-Dateien unter `%LocalAppData%\SASD-GmbH\<Produkt>`; atomare Schreibvorgänge, Backup und Migration. |
| Testing | xUnit, UI-Automation über UI Automation/FlaUI-Pilot, Screenshot-Regression, DPI-/Accessibility-Matrix und Ressourcenprüfungen. |
| Veröffentlichung | Zunächst interne/private NuGet-Pakete; öffentliche Open-Source-Freigabe erst nach R1-Stabilisierung und Lizenzprüfung. |

# 2. Bezug zum Lastenheft und Umsetzungsziele

Das zugrunde liegende Lastenheft grenzt die Produktlinie auf Windows, C# und WinForms ein, verlangt Kompatibilität mit bestehenden .NET-8-Projekten und schließt WPF, ASPX/Web, Java, mobile Plattformen sowie große Spezialkomponenten ausdrücklich aus. Diese Vorgaben werden unverändert übernommen.

Das Pflichtenheft setzt die 20 Komponentenfamilien in vier Stufen um:

| Stufe | Technisches Ziel | Verbindliche Ergebnisse |
| --- | --- | --- |
| R0 - Entscheidungspilot | Architektur und Lieferantenwahl validieren | Krypton-/Native-Vergleich, Tokenmodell, BaseForm, Dialog, Grid-Spike, DPI/Designer/Accessibility-Test, Lizenzinventar. |
| R1 - Produktives Fundament | Erste reale SASD-Anwendungen migrierbar machen | F01-F11, F19-F20; NuGet, Gallery, CRUD-, Workbench- und Utility-Vorlage. |
| R2 - Fachmodule | Editor-, Dashboard- und Workbench-Funktionen ergänzen | F12-F16 sowie wesentliche Teile von F18; Adapterpakete statt Kernabhängigkeiten. |
| R3 - Optionale Module | Nur bewiesene Zusatzbedarfe bedienen | Ribbon, Wizard und weitere fortgeschrittene Ansichten; keine automatische Vollsuite-Erweiterung. |

Erfolg bedeutet nicht eine hohe Control-Anzahl. Erfolg bedeutet, dass Prompt Manager, Mail Workbench, Notes und mindestens eine Utility-Anwendung dieselben Pakete einsetzen, ohne lokale Kopien gemeinsamer UI-Logik zu führen.

# 3. Technische Rahmenbedingungen

| Bereich | Festlegung |
| --- | --- |
| Betriebssystem | Windows 10 und Windows 11; 64-Bit ist Referenz. AnyCPU ist zulässig, wenn alle nativen Abhängigkeiten dies unterstützen. |
| Framework | .NET 8 für R0/R1. Eine aktuelle .NET-LTS wird als zusätzlicher CI-Build geprüft. .NET Framework ist kein neues Ziel. |
| IDE | Visual Studio 2022 oder Nachfolger mit WinForms-Designer. Rider/CLI können Builds ausführen, sind aber nicht Design-Time-Referenz. |
| Sprache | C# mit Nullable Reference Types, impliziten Usings nach Projektentscheidung und aktivierten XML-Dokumentationsdateien. |
| UI-Thread | Alle Control-Zugriffe erfolgen auf dem UI-Thread. Asynchrone APIs verwenden `Task`, `CancellationToken` und `IProgress<T>`. |
| Lokalisierung | Ressourcenbasierte deutsche und englische Texte; Kulturformatierung über `CultureInfo`. |
| Konfiguration | Keine geheimen Werte in UI-State. Geheimnisse verbleiben in anwendungsspezifischen Secret-/Credential-Services. |
| Designer | Öffentliche visuelle Controls müssen parameterlose Konstruktoren besitzen und dürfen im Designer keine externen Dienste voraussetzen. |

Nicht unterstützt werden in R1: x86-spezifische Altkomponenten, nicht reproduzierbare GAC-Installationen, globale Hooks, eingebettete Browserruntimes außer WebView2 in R2 sowie Controls, die im Visual-Studio-Designer nur durch manuelle Designer-Code-Änderungen funktionieren.

# 4. Architekturprinzipien

- **Composition over Inheritance:** Vererbung wird auf wenige belastbare Basisklassen begrenzt. Verhalten wird über Services, Controller, Extender und Komposition ergänzt.
- **Contracts first:** Kernanwendungen hängen auf SASD-Verträge, nicht auf Krypton-, ScottPlot-, WebView2- oder andere Fremdtypen.
- **Designer first:** Visuelle Controls bleiben im Designer erzeugbar. Generische Controls werden vermieden; typisierte Logik liegt in Controllern.
- **Fail safe:** Fehler in Persistenz, Theme, Icons, Clipboard oder optionalen Modulen dürfen den Anwendungsstart nicht verhindern.
- **Secure by default:** Datei-, Shell- und WebView-Funktionen verweigern unsichere Aktionen, bis sie explizit erlaubt wurden.
- **One visual system per application:** Eine Anwendung verwendet genau eine sichtbare Haupt-Theme-Implementierung. ReaLTaiizor/AntdUI werden nicht mit Krypton zu einer Oberfläche vermischt.
- **No megafork:** Forks sind letzte Eskalationsstufe und benötigen Wartungs- und Exit-Plan.
- **Pragmatic APIs:** WinForms bleibt sichtbar. Die Plattform versteckt nicht jede Eigenschaft hinter komplexen Abstraktionen.

## 4.1 Abhängigkeitsregel

Die Abhängigkeitsrichtung verläuft von Anwendungen zu SASD-Paketen und von SASD-Adapterpaketen zu Drittbibliotheken. Der Kern kennt keine Adapterpakete.

```text
SASD-Anwendung
  -> Sasd.Ui.WinForms.* (öffentliche SASD-API)
       -> Sasd.Ui.Core
       -> optionale Adapterpakete
            -> Krypton / ScottPlot / ScintillaNET / WebView2 / weitere Drittbibliothek
```

## 4.2 Fremdtypen in öffentlichen APIs

Fremdtypen sind im Kern nicht erlaubt. Eine Ausnahme gilt innerhalb eines ausdrücklich benannten Adapterpakets, wenn die Adapterfunktion ohne den Fremdtyp unbrauchbar wäre. Beispiel: `Sasd.Ui.WinForms.Charts.ScottPlot` darf erweiterte ScottPlot-Konfiguration über einen klar als herstellerspezifisch gekennzeichneten Erweiterungspunkt anbieten; `Sasd.Ui.WinForms.Shell` darf dagegen keinen Krypton-Typ zurückgeben.

# 5. Repository-, Solution- und Paketstruktur

Das Projekt wird als Monorepo `SASD-UI-Platform` geführt. Die Struktur trennt produktiven Code, Adapter, Beispiele, Vorlagen, Tests, Dokumentation und Build-Infrastruktur.

```text
SASD-UI-Platform/
├─ src/
│  ├─ Sasd.Ui.Core/
│  ├─ Sasd.Ui.WinForms/
│  ├─ Sasd.Ui.WinForms.Theming/
│  ├─ Sasd.Ui.WinForms.Commands/
│  ├─ Sasd.Ui.WinForms.Shell/
│  ├─ Sasd.Ui.WinForms.Forms/
│  ├─ Sasd.Ui.WinForms.Data/
│  ├─ Sasd.Ui.WinForms.Dialogs/
│  ├─ Sasd.Ui.WinForms.Windows/
│  ├─ Sasd.Ui.WinForms.State/
│  ├─ Sasd.Ui.WinForms.Krypton/
│  └─ adapters/ ... R2-Module
├─ samples/
│  ├─ Sasd.Ui.ComponentGallery/
│  ├─ Sasd.Ui.Sample.Crud/
│  ├─ Sasd.Ui.Sample.Workbench/
│  ├─ Sasd.Ui.Sample.Utility/
│  └─ Sasd.Ui.Sample.Dashboard/
├─ templates/
├─ tests/
│  ├─ unit/
│  ├─ integration/
│  ├─ ui/
│  ├─ visual/
│  └─ architecture/
├─ docs/
├─ build/
├─ eng/
├─ Directory.Build.props
├─ Directory.Packages.props
├─ global.json
└─ SASD.Ui.Platform.sln
```

| Paket | Abhängigkeiten | Verantwortung | Release |
| --- | --- | --- | --- |
| Sasd.Ui.Core | Keine WinForms-Abhängigkeit | Design Tokens, Ergebnis-/Fehlermodelle, gemeinsame Verträge, Versionierung | R0/R1 |
| Sasd.Ui.WinForms | Sasd.Ui.Core; System.Windows.Forms | Basisklassen, Hilfen, Dispatcher, Empty/Busy/Search/Section | R0/R1 |
| Sasd.Ui.WinForms.Theming | Core, WinForms, Krypton-Adapter intern | Themes, Token-Mapping, Icons, High Contrast | R0/R1 |
| Sasd.Ui.WinForms.Commands | Core, WinForms | Command-Modell, Shortcut-Verwaltung, Bindings | R1 |
| Sasd.Ui.WinForms.Shell | WinForms, Theming, Commands, State | Shell, Navigation, Tabs, Breadcrumb, Status | R1 |
| Sasd.Ui.WinForms.Forms | WinForms, Theming | FormLayout, Binder, Validation Summary | R1 |
| Sasd.Ui.WinForms.Data | WinForms, State | Grid, Listen, Bäume, Suche, Filter, Paging | R1 |
| Sasd.Ui.WinForms.Dialogs | WinForms, Core; Ookii optional intern | Dialog-, Error-, Notification- und Progress-Services | R1 |
| Sasd.Ui.WinForms.Windows | WinForms, Core | Datei, Clipboard, DragDrop, Tray, sichere Shell-Aufrufe | R1 |
| Sasd.Ui.WinForms.State | Core | JSON-StateStore, Migration, MRU, atomare Dateien | R1 |
| Sasd.Ui.WinForms.Krypton | WinForms; Krypton Standard Toolkit | Konkrete visuelle Implementierung und Palette | R0/R1 |
| Sasd.Ui.WinForms.Charts.ScottPlot | WinForms; ScottPlot | Primärer Chart-Adapter | R2 |
| Sasd.Ui.WinForms.Editors | WinForms; Markdig; DiffPlex | Markdown- und Diff-Funktionen | R2 |
| Sasd.Ui.WinForms.Editors.Scintilla | Editors; ScintillaNET | Code-/Konfigurationseditor | R2 |
| Sasd.Ui.WinForms.Media | WinForms; ImageBox/QRCoder/ZXing nach Pilot | Bild, QR und Barcode | R2 |
| Sasd.Ui.Documents.PdfSharp | Core; PDFsharp/MigraDoc | PDF-Erzeugung ohne UI-Abhängigkeit | R2 |
| Sasd.Ui.WinForms.WebView2 | WinForms; WebView2 | Gehärteter WebView-Host | R2 |
| Sasd.Ui.WinForms.Docking.Krypton | Shell; Krypton Docking/Workspace | Docking und Layoutpersistenz | R2 |
| Sasd.Ui.WinForms.Testing | Testprojekte | Testhost, Screenshot-Helfer, UIA-Helfer, Fake-Services | R0/R1 |
| Sasd.Ui.WinForms.Templates | Freigegebene R1/R2-Pakete | dotnet new-/Visual-Studio-Starter und Projektvorlagen | R1/R2 |

## 5.1 Paketierungsregel

- R1 soll nicht mit zwanzig NuGet-Paketen starten. Intern dürfen Projekte bereits getrennt sein; veröffentlicht werden zunächst wenige logisch zusammenhängende Pakete.
- Vorgesehene erste NuGet-Pakete: `Sasd.Ui.Core`, `Sasd.Ui.WinForms`, `Sasd.Ui.WinForms.Krypton`, `Sasd.Ui.WinForms.Data`, `Sasd.Ui.WinForms.Templates`.
- Dialog-, Shell-, State- und Windows-Module können bis zur API-Stabilisierung im Paket `Sasd.Ui.WinForms` gebündelt werden, obwohl sie im Repository getrennte Projekte bleiben.
- R2-Spezialadapter werden immer separat paketiert, damit Anwendungen nur benötigte native oder große Abhängigkeiten beziehen.

# 6. Öffentliche API, Konventionen und Fehlerverträge

## 6.1 Namenskonventionen

| Element | Konvention | Beispiel |
| --- | --- | --- |
| Namespaces | `Sasd.Ui...` | `Sasd.Ui.WinForms.Dialogs` |
| Controls | Präfix `Sasd` | SasdDataGrid, SasdBusyOverlay |
| Services | Interface + Service | IDialogService / SasdDialogService |
| Optionen | Suffix `Options` | DialogOptions, GridOptions |
| Ergebnisse | Suffix `Result` | DialogResult<T>, FileSelectionResult |
| Events | .NET-Ereignismuster | NavigationChanging, NavigationChanged |
| Asynchron | Suffix `Async` | ShowProgressAsync |

## 6.2 Ergebnis- und Fehlermodell

Services werfen nur bei Programmierfehlern oder nicht wiederherstellbaren Zuständen Exceptions. Erwartbare Benutzer- und Umgebungsfehler werden als Ergebnisobjekte zurückgegeben.

```csharp
public sealed record UiOperationResult(
    bool Succeeded,
    string? UserMessage = null,
    string? TechnicalDetails = null,
    string? ErrorCode = null,
    Exception? Exception = null);

public sealed record UiOperationResult<T>(
    bool Succeeded,
    T? Value = default,
    string? UserMessage = null,
    string? TechnicalDetails = null,
    string? ErrorCode = null);
```

## 6.3 Binär- und Quellkompatibilität

- Semantische Versionierung: Patch für kompatible Fehlerkorrekturen, Minor für additive API, Major für Breaking Changes.
- Obsolete APIs bleiben mindestens über eine Minor-Version mit Migrationshinweis erhalten, sofern kein Sicherheitsproblem entgegensteht.
- Public API Baseline wird im Build geprüft; unbeabsichtigte neue öffentliche Typen oder Breaking Changes brechen CI.
- Alle öffentlichen Typen erhalten XML-Dokumentation und mindestens ein ausführbares Beispiel in Gallery oder Samples.

# 7. Designsystem, Themes und Icons

## 7.1 Design-Token-Modell

Design Tokens werden als unveränderliche C#-Records definiert und optional aus versionierten JSON-Dateien geladen. Tokens sind semantisch benannt; Komponenten dürfen keine zufälligen Hexfarben oder projektspezifischen Abstände hart codieren.

```csharp
public sealed record SasdThemeDefinition(
    string Id,
    int SchemaVersion,
    SasdColorTokens Colors,
    SasdTypographyTokens Typography,
    SasdSpacingTokens Spacing,
    SasdSizeTokens Sizes,
    SasdIconTheme Icons);
```

| Tokenbereich | Verbindliche Inhalte |
| --- | --- |
| Farben | Background, Surface, SurfaceVariant, Primary, Secondary, Text, MutedText, Border, Focus, Success, Warning, Error, Info, Selection. |
| Typografie | Standardfont, Monospacefont, Body, Caption, Label, Title, Heading; relative Skalierung statt absoluter Einzelwerte. |
| Abstände | 2, 4, 8, 12, 16, 24, 32 Pixel bei 100 Prozent; DPI-Skalierung über WinForms. |
| Größen | Mindesthöhen für Eingaben und Buttons, Icongrößen, Touch-/Klickziel, Dialogmindestbreiten. |
| Zustände | Normal, Hover, Pressed, Focused, Disabled, ReadOnly, Selected, Error, Warning. |

## 7.2 Theme-Service

`IThemeService` verwaltet aktives Theme, Systembezug und Benachrichtigung. Light und Dark werden zur Laufzeit umgeschaltet. High Contrast wird nicht als frei gestaltetes SASD-Theme simuliert, sondern respektiert Windows-Systemfarben und reduziert dekorative Elemente.

## 7.3 Krypton-Implementierung

R0 implementiert eine Token-zu-Krypton-Palette. Der Pilot gilt als bestanden, wenn Designer, Laufzeitwechsel, DPI, Standardzustände, DataGrid und Navigation ohne unvertretbare Sonderbehandlung funktionieren. Scheitert der Pilot, bleibt das Token- und Service-Modell bestehen und die erste R1-Implementierung verwendet native WinForms-Controls mit zentraler Konfiguration.

## 7.4 Icons

Icons werden über semantische Namen (`Save`, `Delete`, `Search`, `Warning`) angefordert. `IIconService` liefert DPI-gerechte Images und cached sie pro Theme, Größe und Zustand. Das konkrete Iconset wird als eigene, lizenzierte Ressource geführt; Icons werden nicht aus beliebigen Anwendungen kopiert. Für wesentliche Aktionen muss zusätzlich Text oder ein zugänglicher Name vorhanden sein.

# 8. Basiskomponenten und Formularsystem

## 8.1 Entscheidung gegen flächendeckende Primitive-Wrapper

Es werden **keine** eigenen Klassen `SasdButton`, `SasdLabel`, `SasdTextBox` allein zum Umbenennen vorhandener Controls erstellt. Das würde Designer, Dokumentation und Wartung ohne ausreichenden Nutzen vervielfachen. Primitive Controls werden durch Theme-Implementierung, `SasdFieldLayout`, Validierungsadapter, Extensions und Factory-Helfer vereinheitlicht. Eigene Controls entstehen, wenn sie mehrere Controls oder wiederkehrendes Verhalten kapseln.

## 8.2 Form- und View-Basisklassen

| Typ | Verantwortung | Nicht enthalten |
| --- | --- | --- |
| SasdForm | Theme, DPI, State-Key, Icon, Standardfehlergrenze, Help-/About-Hooks, sichere Dispose-Reihenfolge | Fachlogik, Datenzugriff, globaler Service Locator |
| SasdDialogForm | Standardbuttons, Validation, Mindestgröße, Enter/Escape, Dirty-Abfrage | Beliebige Wizard-Logik |
| SasdUserControl | Theme-/Culture-Ereignisse, Designer-Sicherheit, Lebenszyklus | Automatische Dependency Injection im Designer |

## 8.3 FormLayout

`SasdFieldLayout` basiert auf `TableLayoutPanel`, nutzt AutoSize, feste semantische Spalten und DPI-fähige Abstände. Ein Feld besteht logisch aus Label, Control, Hilfetext und Fehlermeldung. Controls können normale WinForms- oder Krypton-Controls sein. Bei schmalen Dialogen darf das Layout auf Label-über-Control umschalten; dieser Wechsel wird durch definierte Breakpoints und nicht durch pixelgenaue manuelle Positionen gesteuert.

## 8.4 Eingabetypen

| Eingabe | Umsetzung | R1-Abnahme |
| --- | --- | --- |
| Text | Ein-/mehrzeilig, MaxLength, ReadOnly, Placeholder über unterstützte Control-API oder Cue Banner | Tastatur, Copy/Paste, Fehlertext und lange deutsche Texte funktionieren. |
| Maskiert | MaskedTextBox oder Krypton-Äquivalent hinter Feldadapter; benutzerdefinierte Masken | Datum, Zeit, PLZ und freie Maske werden validiert. |
| Numerisch | NumericUpDown-Adapter mit `decimal?`-Binding und Kulturformat | Min/Max, Null, Schritt und Dezimalstellen sind getestet. |
| Datum/Zeit | DateTimePicker-Adapter mit optionalem Nullzustand und Range | Tastatur, Picker und Kulturwechsel funktionieren. |
| Auswahl | ComboBox, CheckBox, RadioButton, Toggle; konsistente Enabled/ReadOnly-Semantik | Binding und AccessibleName/State sind korrekt. |
| Datei/Ordner | Button + Textanzeige + `IFileDialogService`; kein direkter Dialogcode im Formular | Filter, Startpfad, Mehrfachwahl und Abbruch sind robust. |
| AutoComplete | R1 einfacher Vorschlagsprovider; MultiSelect/Tags erst R2 nach Pilot | Verzögerte Suche blockiert UI nicht. |

## 8.5 Validierung

Die Validierung unterstützt DataAnnotations, synchrone anwendungsspezifische Regeln und optionale asynchrone Prüfungen. `SasdValidationCoordinator` ordnet Fehler Controls zu, setzt `ErrorProvider`, aktualisiert `SasdValidationSummary` und fokussiert auf Auswahl den betroffenen Eingabebereich. Fehlertexte bleiben beim Nutzer; technische Exceptions werden nicht als Validierungstext angezeigt.

# 9. Komponenten- und Servicekatalog

Die folgende Liste ist der verbindliche Implementierungskatalog. Eintrag bedeutet nicht automatisch eine eigene visuelle Klasse; Services und Controller sind gleichwertige Bestandteile der Komponentenplattform.

| Release | Komponente/Service | Zweck | Funktionsumfang | Paket |
| --- | --- | --- | --- | --- |
| R1 | SasdForm | Basisklasse für Haupt- und Nebenfenster | DPI, Theme, Icon, Fensterzustand, Fehlergrenze, Standard-Shortcuts | Sasd.Ui.WinForms |
| R1 | SasdDialogForm | Basisklasse für modale Dialoge | OK/Abbrechen, Validierung, Mindestgröße, Fokus, Escape/Enter | Sasd.Ui.WinForms |
| R1 | SasdUserControl | Basisklasse für wiederverwendbare Ansichten | Theme-/Lokalisierungswechsel, Designer-Unterstützung, Dispose-Hooks | Sasd.Ui.WinForms |
| R1 | SasdSectionPanel | Thematischer Inhaltsabschnitt | Titel, Beschreibung, Icon, einklappbarer Inhalt, Fehler-/Hinweiszustand | Sasd.Ui.WinForms |
| R1 | SasdFieldLayout | Einheitliches Formularraster | Label, Eingabe, Hilfetext, Pflichtmarkierung, Fehlermeldung, responsive Spalten | Sasd.Ui.WinForms.Forms |
| R1 | SasdValidationSummary | Formularweite Fehlerübersicht | Fehlerliste, Fokusnavigation, Schweregrad, Accessibility | Sasd.Ui.WinForms.Forms |
| R1 | SasdSearchBox | Wiederverwendbare Sucheingabe | Debounce, Löschen, Shortcut, Suchstatus, Ereignisse | Sasd.Ui.WinForms |
| R1 | SasdFilterBar | Kompakte Filterzeile | Filterchips, Zurücksetzen, gespeicherte Filter, Ergebniszahl | Sasd.Ui.WinForms.Data |
| R1 | SasdEmptyState | Leerergebnis-/Erststartansicht | Titel, Erklärung, optionale Aktion, Icon, kein Datenverlust | Sasd.Ui.WinForms |
| R1 | SasdBusyOverlay | Blockierender oder teilblockierender Busy-Zustand | Status, unbestimmter/bestimmter Fortschritt, Abbruch, Fokusfalle | Sasd.Ui.WinForms |
| R1 | SasdShellForm | Standardisierte Anwendungshülle | Navigation, CommandBar, Arbeitsbereich, Status, Fehler-/Updateanzeige | Sasd.Ui.WinForms.Shell |
| R1 | SasdNavigationView | Seiten-/Bereichsnavigation | Hierarchie, Gruppen, Icons, Einklappen, Tastatur, Navigationsergebnis | Sasd.Ui.WinForms.Shell |
| R1 | SasdBreadcrumb | Kontextnavigation | Pfadsegmente, Klicknavigation, Kürzung, Accessibility | Sasd.Ui.WinForms.Shell |
| R1 | SasdDocumentTabs | Dokument- und Arbeitsseiten | Neu, Aktivieren, Schließen, Dirty-State, Wiederherstellung | Sasd.Ui.WinForms.Shell |
| R1 | SasdCommandBar | Befehlsleiste | Commands, Gruppen, Overflow, Shortcuts, Enable/Visible/Checked | Sasd.Ui.WinForms.Commands |
| R1 | SasdStatusService / SasdStatusBar | Anwendungsweite Statusmeldungen | Info/Warnung/Fehler, Fortschritt, Zeitstempel, Priorität | Sasd.Ui.WinForms.Shell |
| R1 | SasdDataGrid | Standardisierte Datentabelle | Binding, Sortierung, Suche, Filter, Auswahl, Bearbeitung, Persistenz, Export | Sasd.Ui.WinForms.Data |
| R1 | SasdGridController<T> | Typisierte Grid-Steuerung ohne generisches Designer-Control | Spaltendefinitionen, Mapping, Auswahl, Commands, Validierung | Sasd.Ui.WinForms.Data |
| R1 | SasdListView | Leichte Listenansicht | Details/Kacheln, Auswahl, Kontextmenü, leere Zustände, VirtualMode-Pilot | Sasd.Ui.WinForms.Data |
| R1 | SasdTreeView | Hierarchische Datenansicht | Lazy Loading, Fehlerknoten, Icons, Auswahl, Kontextaktionen | Sasd.Ui.WinForms.Data |
| R1 | SasdDialogService | Einheitliche modale Interaktion | Info, Warnung, Fehler, Bestätigung, Auswahl, eigene Dialoge | Sasd.Ui.WinForms.Dialogs |
| R1 | SasdErrorDialog | Benutzerfreundliche Fehlerdarstellung | Kurztext, technische Details, Kopieren, Korrelation, Logpfad | Sasd.Ui.WinForms.Dialogs |
| R1 | SasdNotificationService | Nichtmodale Rückmeldungen | Toast/Snackbar, Aktionen, Dauer, Queue, Deaktivierung | Sasd.Ui.WinForms.Dialogs |
| R1 | SasdProgressDialog | Fortschritt für längere Operationen | IProgress, CancellationToken, Details, Fehlerabschluss | Sasd.Ui.WinForms.Dialogs |
| R1 | SasdFileDialogService | Datei- und Ordnerdialoge | Open/Save/Folder, Filter, Mehrfachwahl, letzter Pfad, Validierung | Sasd.Ui.WinForms.Windows |
| R1 | SasdClipboardService | Fehlertolerante Zwischenablage | Text, HTML, Bild, Dateiliste, Wiederholversuche, Fehlerobjekt | Sasd.Ui.WinForms.Windows |
| R1 | SasdDragDropService | Sichere Datei-Drop-Zonen | Erlaubte Typen, Mehrfachdateien, Feedback, Größen-/Pfadprüfung | Sasd.Ui.WinForms.Windows |
| R1 | SasdRecentItemsService | MRU-/Projektverlauf | Pinning, Entfernen, Datenschutz, fehlende Dateien, Begrenzung | Sasd.Ui.WinForms.State |
| R1 | SasdTrayService | Optionaler Tray-Betrieb | Icon, Menü, Doppelklick, Minimize-to-tray, sauberes Beenden | Sasd.Ui.WinForms.Windows |
| R1 | SasdStateStore | Versionierte UI-Zustände | JSON, atomare Speicherung, Migration, Backup, Reset, keine Secrets | Sasd.Ui.WinForms.State |
| R1 | SasdThemeService | Theme-Verwaltung | Light/Dark/High Contrast, Tokens, Laufzeitwechsel, Systembezug | Sasd.Ui.WinForms.Theming |
| R1 | SasdIconService | DPI-fähige Standardsymbole | semantische Namen, Größen, Zustände, Cache, Lizenznachweis | Sasd.Ui.WinForms.Theming |
| R1 | SasdCommandManager | Gemeinsames Command-System | CanExecute, ExecuteAsync, Shortcut, Checked, Fehlerbehandlung | Sasd.Ui.WinForms.Commands |
| R1 | SasdUiDispatcher | Sicherer UI-Thread-Wechsel | Invoke/BeginInvoke/InvokeAsync, Cancellation, Dispose-Prüfung | Sasd.Ui.WinForms |
| R2 | SasdChartView | Technische Diagramme | Linie, Balken, Punkt, Achsen, Legende, Tooltip, Zoom, Export | Sasd.Ui.WinForms.Charts.ScottPlot |
| R2 | SasdKpiCard / SasdSparkline | Dashboard-Kennzahlen | Wert, Trend, Status, zugängliche Textalternative | Sasd.Ui.WinForms.Charts |
| R2 | SasdMarkdownEditor | Markdown-Bearbeitung und Vorschau | Suche, Toolbar, Split-Preview, sichere HTML-Ausgabe | Sasd.Ui.WinForms.Editors |
| R2 | SasdCodeEditor | Code-/Konfigurationseditor | Syntax, Folding, Zeilen, Suche/Ersetzen, Marker, große Dateien | Sasd.Ui.WinForms.Editors.Scintilla |
| R2 | SasdDiffViewer | Textvergleich | Inline/Side-by-side, Navigation, Whitespace-Optionen, Export | Sasd.Ui.WinForms.Editors |
| R2 | SasdImageViewer | Rasterbildanzeige | Zoom, Pan, Fit, Rotate, Hintergrund, Tastatur | Sasd.Ui.WinForms.Media |
| R2 | SasdBarcodeService | QR-/Barcode-Adapter | Erzeugen, Lesen optional, PNG/SVG/Bitmap, Fehlerkorrektur | Sasd.Ui.WinForms.Media |
| R2 | SasdPdfDocumentService | Programmgesteuerte PDF-Ausgabe | Absätze, Tabellen, Bilder, Header/Footer, Metadaten, Seitennummern | Sasd.Ui.Documents.PdfSharp |
| R2 | SasdWebViewHost | Sicherer WebView2-Host | Origin-Allowlist, Navigation, Downloads, Skriptbrücke, lokale Ressourcen | Sasd.Ui.WinForms.WebView2 |
| R2 | SasdDockWorkspace | Docking-/Workspace-Modul | Dokumente, Tools, Layout speichern/laden, beschädigtes Layout zurücksetzen | Sasd.Ui.WinForms.Docking.Krypton |
| R2 | SasdPropertyEditor | Eigenschaftsbearbeitung | Kategorien, Beschreibung, Validierung, Read-only, eigene Editoren | Sasd.Ui.WinForms.Data.Advanced |
| R2 | SasdGridExtensions | Erweiterte Datenfunktionen | Column Chooser, Summaries, Master-Detail, CSV-Export, Paging | Sasd.Ui.WinForms.Data.Advanced |
| R3 | SasdWizard | Geführte mehrstufige Abläufe | Schritte, Validierung, Zurück/Weiter, Abbruch, Wiederaufnahme | Sasd.Ui.WinForms.Wizard |
| R3 | SasdRibbonHost | Optionales Ribbon | Command-Bindung, Tabs, Gruppen, KeyTips, adaptive Zustände | Sasd.Ui.WinForms.Ribbon.Krypton |

# 10. Application Shell, Commands und Navigation

## 10.1 Shell

`SasdShellForm` enthält definierte Regionen: Header/Command-Bereich, Navigation, Arbeitsbereich, optionale rechte/untere Nebenfläche und Statusbereich. Anwendungen konfigurieren die Shell über ein `ShellOptions`-Objekt und registrieren Seiten/Commands; sie greifen nicht direkt auf Krypton-Controls zu.

## 10.2 Command-System

Das Command-System ist WinForms-spezifisch und verwendet nicht `System.Windows.Input.ICommand` als zwingende Abhängigkeit. `IUiCommand` unterstützt synchrones oder asynchrones Ausführen, `CanExecute`, Sichtbarkeit, Checked-State, Icon, Text, Beschreibung und Shortcut. Menü, Kontextmenü, CommandBar und Navigation binden dieselbe Command-Instanz. Fehler werden über den zentralen Error Presenter behandelt.

```csharp
public interface IUiCommand
{
    string Id { get; }
    string Text { get; }
    Keys? Shortcut { get; }
    bool CanExecute(object? parameter = null);
    Task ExecuteAsync(object? parameter = null, CancellationToken cancellationToken = default);
    event EventHandler? StateChanged;
}
```

## 10.3 Navigation

Navigation verwendet stabile Seiten-IDs und ein `INavigationService`. Navigation kann abgebrochen werden, etwa wenn ein Dokument ungespeichert ist. Der Verlauf ist optional. Navigation darf keine Forminstanzen dauerhaft im Speicher halten, wenn Seiten geschlossen wurden.

## 10.4 Dokument-Tabs und Dirty-State

Dokumentseiten implementieren `IDocumentView` mit ID, Titel, Dirty-State, Save-/Close-Command und optionalem Persistenzschlüssel. Schließen fragt nur bei Dirty-State nach. Wiederherstellung speichert keine Dokumentinhalte, sondern ausschließlich sichere Referenzen, die die Anwendung prüfen und erneut öffnen kann.

## 10.5 Docking

Docking ist R2 und wird über `IWorkspaceHost` abstrahiert. Krypton Docking/Workspace ist der Pilot. Layoutdateien sind versioniert und werden bei inkompatiblen oder beschädigten Daten verworfen. Ein nicht ladbares Layout darf nie den Start verhindern.

# 11. Datenanzeige: Grid, Listen, Bäume und Paging

## 11.1 SasdDataGrid

`SasdDataGrid` ist ein nicht generisches, designerfähiges Control. Typisierung und fachliche Spaltendefinition liegen in `SasdGridController<T>`. Diese Trennung vermeidet die bekannten Designerprobleme generischer Controls und erlaubt trotzdem compile-time-nahe Konfiguration.

| Funktion | R1 | R2 |
| --- | --- | --- |
| Binding | BindingSource, DataTable, IList | Asynchrone Page Source und Virtual Mode |
| Spalten | Explizite Definition, Format, Sichtbarkeit, Sortierung | Column Chooser, Summaries, benutzerdefinierte Aggregate |
| Suche | Zentrale Suche über definierte Spalten | Hervorhebung und gespeicherte Suchen |
| Filter | Einfache Operatoren: enthält, gleich, beginnt, Bereich, leer | Komplexere kombinierte Filter ohne visuellen Universal-Builder |
| Auswahl | Einzel/Mehrfach, vollständige Zeile, Auswahlcommands | Master-Detail |
| Bearbeitung | Optional, Validierung, Commit/Cancel, Fehleranzeige | Batch-/Optimistic-Concurrency nur anwendungsspezifisch |
| Persistenz | Breite, Reihenfolge, Sichtbarkeit, Sortierung, Filter | Profile/gespeicherte Ansichten |
| Export | CSV UTF-8, sichtbare oder alle geladenen Zeilen | Weitere Exporte nur per separatem Adapter |

## 11.2 Paging und große Datenmengen

`IDataPageSource<T>` definiert Seitengröße, Sortier- und Filterbeschreibung unabhängig von Datenbank oder ORM. Der Grid-Kern kennt weder Entity Framework noch SQL. Eine Anfrage kann abgebrochen werden; nur das Ergebnis der zuletzt aktiven Anfrage wird angezeigt. Für R1 wird ein Paging-Pilot, für R2 Virtual Mode umgesetzt.

## 11.3 Listen und Bäume

`SasdListView` und `SasdTreeView` bilden leichtere Alternativen zum Grid. Tree-Nodes werden lazy geladen; Zugriffsfehler erscheinen als verständliche Fehlerknoten mit Retry-Command. Ein Folder Tree ist eine Konfiguration des TreeView mit `IFileSystemNodeProvider`, kein vollständiger Explorer-Ersatz.

## 11.4 Performanceziele

| Szenario | Ziel auf Referenzsystem | Messung |
| --- | --- | --- |
| Grid 10.000 geladene Zeilen | Erste Anzeige <= 1,5 s; UI bleibt responsiv | Warmstart, Release-Build, drei Läufe, Median |
| Suche/Filter 10.000 Zeilen | Ergebnis <= 500 ms nach Debounce | In-Memory-Referenzdaten |
| Paging 100.000+ Datensätze | Erste Seite <= 1,5 s zzgl. Datenquellenzeit; Abbruch wirksam | Fake Page Source mit kontrollierter Latenz |
| Tree mit 1.000 sichtbaren Nodes | Auf-/Zuklappen ohne wahrnehmbare Mehrsekundenpause | UI-Automation und Stoppuhr |
| State speichern/laden | <= 100 ms für typische Shell-/Grid-Zustände | Lokales SSD-Dateisystem |

Die Ziele sind Freigabegrenzen für die Referenzimplementation, keine Garantie für langsame Datenquellen. Überschreitungen benötigen Messprotokoll und akzeptierte Abweichung.

# 12. Dialoge, Feedback, Fortschritt und Fehler

## 12.1 DialogService

`IDialogService` bietet typisierte Methoden für Information, Warnung, Fehler, Bestätigung und benutzerdefinierte Dialogmodelle. Anwendungen entscheiden nicht selbst, ob MessageBox, TaskDialog oder SASD-Dialog verwendet wird. Ookii.Dialogs darf intern für native Task-/Dateidialoge eingesetzt werden, ohne dass die Anwendung dessen Typen referenziert.

## 12.2 Error Presenter

Fehler werden in Benutzertext und technische Details getrennt. Der Dialog zeigt eine kurze Handlungsempfehlung, optional aufklappbare Details, Fehlercode/Korrelations-ID, Kopierfunktion und den Pfad zu Logs. Stacktraces erscheinen nie ungefragt als Hauptmeldung. Geheimnisse und vollständige Mail-/Dokumentinhalte werden vor Anzeige/Logging nicht automatisch übernommen.

## 12.3 Notifications

Toasts/Snackbars werden in einer Queue verarbeitet, haben Schweregrad und optionale Aktion und sind vollständig per Tastatur erreichbar. Zeitkritische oder sicherheitsrelevante Fehler dürfen nicht ausschließlich als verschwindender Toast dargestellt werden. Desktop-Benachrichtigungen sind ein optionaler R2-Service und respektieren Anwendungseinstellungen.

## 12.4 Progress und Busy

Operationen über etwa 500 ms sollen Busy-Feedback vorbereiten; ab etwa 2 s ist sichtbarer Fortschritt oder ein unbestimmter Status erforderlich. `SasdProgressDialog` nimmt eine asynchrone Funktion, `IProgress<ProgressInfo>` und `CancellationToken`. Nach Abbruch muss die Anwendung einen konsistenten Zustand wiederherstellen.

# 13. Windows-, Datei- und Systemintegration

## 13.1 Datei- und Ordnerdialoge

`SasdFileDialogService` normalisiert Filter, validiert Startpfade und liefert Ergebnisobjekte. Dateierweiterungen werden nicht allein als Vertrauensbeweis behandelt. Anwendungen validieren Inhalt und Größe vor Verarbeitung. Letzte Pfade werden nur gespeichert, wenn Datenschutzoptionen dies erlauben.

## 13.2 Clipboard

Clipboard-Zugriffe werden wegen temporärer Sperren mit begrenztem Retry und UI-freundlicher Fehlerbehandlung gekapselt. HTML-Clipboard wird nur mit korrektem Format erzeugt. Inhalte werden nicht automatisch ausgeführt oder als vertrauenswürdig interpretiert.

## 13.3 Drag-and-drop

Drop-Zonen definieren erlaubte Formate, maximale Anzahl, optional maximale Dateigröße und Validierungsfunktion. Die visuelle Rückmeldung unterscheidet erlaubt, abgelehnt und prüfend. Netzwerk- und nicht zugreifbare Pfade werden asynchron geprüft.

## 13.4 Tray

Tray-Betrieb wird nur aktiviert, wenn die Anwendung ihn explizit konfiguriert. Schließen, Minimieren und Beenden sind eindeutig. Ein Tray-Prozess darf keine unsichtbare Anwendung hinterlassen, die nur über Task-Manager beendet werden kann.

## 13.5 Sichere Shell-Integration

Öffnen von URLs, Dateien oder Ordnern erfolgt über einen zentralen Service. Unterstützte URI-Schemata werden allowlist-basiert geprüft. Befehlszeilen werden nicht aus unvalidierten Nutzerdaten zusammengesetzt. PowerShell/CMD-Ausführung gehört nicht in den UI-Kern.

## 13.6 WebView2

R2 stellt `SasdWebViewHost` mit gesperrtem Standardprofil bereit. Externe Navigation, Downloads, neue Fenster, Kamera/Mikrofon, Clipboard, Host Objects und DevTools sind standardmäßig deaktiviert oder explizit zu konfigurieren. Lokale Inhalte werden über virtuelle Hostnamen oder sichere Resource-Mappings geladen. Die JavaScript-Brücke akzeptiert versionierte Nachrichtenmodelle und validiert Ursprung sowie Payload.

# 14. R2-Fachmodule

## 14.1 Charts und KPI

ScottPlot ist die primäre Implementierung für technische Diagramme. `IChartView` arbeitet mit SASD-Datenreihen und Darstellungseinstellungen. Linien-, Balken- und Punktdiagramm sind verbindlich; Kreis/Donut, Gauge und Sparkline werden nur ergänzt, wenn sie ohne zweite sichtbare Suite konsistent umsetzbar sind. Jede visuelle Kennzahl erhält eine textuelle Alternative für Accessibility und Export. LiveCharts2 bleibt ein optionaler Adapter für animierte Dashboards, nicht Teil des Basispakets.

## 14.2 Markdown-Editor

Der Editor kombiniert ScintillaNET oder ein geeignetes Text-Control mit Markdig. Vorschau wird in R2 bevorzugt über gehärtetes WebView2 angezeigt. Rohes HTML ist standardmäßig deaktiviert oder sanitisiert. Toolbar-Commands fügen Markdown ein, ersetzen aber keinen vollständigen WYSIWYG-Editor.

## 14.3 Code-/Konfigurationseditor

ScintillaNET wird in einem separaten Adapterpaket gekapselt. Unterstützt werden Syntaxdefinitionen für typische SASD-Dateien, Suche/Ersetzen, Zeilennummern, Folding, Marker, Undo/Redo und große Textdateien. IntelliSense, Language Server und Debugger sind nicht Bestandteil.

## 14.4 Diff Viewer

DiffPlex oder eine vergleichbare permissiv lizenzierte Bibliothek erzeugt ein plattformneutrales Diff-Modell. Die WinForms-Ansicht bietet Inline und Side-by-side, Navigation zwischen Änderungen, Whitespace-Optionen und Kopieren. Merge-/Konfliktauflösung ist zunächst nicht vorgesehen.

## 14.5 Bild, QR und Barcode

Der Image Viewer wird mit Cyotek ImageBox pilotiert; bei Integrationsproblemen entsteht eine kleinere eigene PictureBox-Erweiterung. QR-Erzeugung nutzt QRCoder, allgemeine Barcodes ZXing.Net hinter `IBarcodeService`. Bitmap-Lebenszyklen und GDI-Ressourcen werden strikt geprüft.

## 14.6 PDF

PDFsharp/MigraDoc ist die Standardimplementierung für programmatische PDF-Dokumente. Die API liegt in einem UI-unabhängigen Paket. Unterstützt werden Text, Tabellen, Bilder, Header/Footer, Seitennummern und Metadaten. FastReport OSS wird nur für bandorientierte Reports separat bewertet; QuestPDF bleibt bis zu einer dokumentierten Lizenz- und Business-Entscheidung ausgeschlossen. Ein visueller Report Designer ist nicht Teil des Projekts.

## 14.7 Property Editor und erweiterte Datenansichten

`SasdPropertyEditor` kapselt das Standard-PropertyGrid oder eine geprüfte Alternative. Öffentliche Metadaten verwenden .NET-Attribute und SASD-Editorverträge, keine herstellerspezifischen Attribute. Column Chooser, Summaries und Master-Detail erweitern `SasdDataGrid` in einem optionalen Paket.

# 15. Querschnittsanforderungen und Qualitätssicherung

## 15.1 DPI und Mehrmonitorbetrieb

- Anwendungsmanifest und High-DPI-Modus werden zentral in Templates gesetzt.
- Testmatrix: 100, 125, 150 und 200 Prozent; mindestens ein Wechsel zwischen zwei Monitoren mit unterschiedlicher Skalierung.
- Keine pixelgenauen manuellen Positionen, wenn LayoutPanels geeignet sind.
- Icons werden über `IIconService` in Zielgröße erzeugt; keine Skalierung winziger Rasterbilder auf 200 Prozent.
- Visuelle Baselines werden pro DPI-Stufe getrennt verwaltet.

## 15.2 Tastatur und Accessibility

- Jeder interaktive Control besitzt sinnvollen AccessibleName, Role, Value/State und bei Bedarf Description.
- Fokusindikator darf durch Theme nicht unsichtbar werden.
- Tab-Reihenfolge entspricht der visuellen und fachlichen Reihenfolge.
- Standarddialoge verwenden Enter für primäre und Escape für Abbruchaktion, sofern fachlich zulässig.
- Automatisierte UIA-Prüfung wird durch manuelle Tests mit Accessibility Insights für Windows ergänzt.
- Farben sind nie die einzige Information; Status erhält Text, Icon oder Muster.

## 15.3 Lokalisierung

Gemeinsame Texte liegen in `Sasd.Ui.*.Resources`. Anwendungstexte bleiben in den Anwendungen. Gallery und Samples werden vollständig Deutsch/Englisch ausgeführt. Pseudolokalisierung beziehungsweise künstlich verlängerte Texte prüfen Layoutreserven. Datum, Zahl und Währung verwenden die aktive Kultur; persistierte technische Werte verwenden kulturinvariante Formate.

## 15.4 Logging und Telemetrie

Die Plattform definiert leichte Hooks über `Microsoft.Extensions.Logging.Abstractions` oder einen eigenen minimalen Adapter. Kein Telemetrieanbieter ist Pflicht. Events enthalten Komponente, Operation, Dauer und Ergebnis, aber keine Geheimnisse oder vollständigen Nutzinhalte.

## 15.5 Ressourcen- und Lecktests

| Prüfung | Freigabekriterium |
| --- | --- |
| Fensterzyklus | Nach 100 Öffnen-/Schließen-Zyklen steigt der GDI-/USER-Handle-Bestand nicht fortlaufend; verbleibende Differenz wird dokumentiert und begründet. |
| Event-Abonnements | Geschlossene Views werden vom GC freigegeben; globale Services halten keine versehentlichen Referenzen. |
| Bilder/Icons | Alle dynamisch erzeugten Bitmaps werden disposed oder kontrolliert gecached. |
| WebView2/Editor | R2-Module beenden Prozesse/Handles beim Schließen und besitzen dokumentierten Lebenszyklus. |

## 15.6 Testpyramide

| Ebene | Werkzeug/Ansatz | Abdeckung |
| --- | --- | --- |
| Unit | xUnit | Tokenlogik, Commands, State-Migration, Filter, Ergebnisobjekte, Validatoren |
| Architecture | Reflexions-/Architekturtests | Abhängigkeitsrichtung, Fremdtypen in Public API, Namespace-Regeln |
| Integration | xUnit + temporäre Verzeichnisse/Testhosts | State, File Dialog Abstraktion, Clipboard-Fakes, PDF, Barcode |
| UI Automation | FlaUI/UIA3-Pilot; robuste Selektoren über AutomationId | Navigation, Dialoge, Grid-Grundabläufe, Tastatur |
| Visual Regression | Screenshot-Harness und Pixel-/Toleranzvergleich | Themes, DPI, Zustände, lange Texte |
| Manuell | Checklisten und Accessibility Insights | Designer, High Contrast, Mehrmonitor, Screenreader-nahe Prüfung |

## 15.7 Definition of Done

- Anforderung und Release-Zuordnung dokumentiert.
- Öffentliche API mit XML-Kommentaren und Beispiel.
- Unit-/Integrationstests und bei visueller Komponente Gallery-Seite.
- DPI-, Tastatur-, Accessibility- und Lokalisierungscheck bestanden.
- Keine unfreigegebene Lizenz oder transitive Abhängigkeit.
- Keine neuen Compilerwarnungen; Nullable-Analyse ohne unbegründete Unterdrückung.
- Changelog, bekannte Einschränkungen und Migrationshinweis aktualisiert.

# 16. Build, CI/CD, Versionierung und Governance

## 16.1 Build

`Directory.Build.props` aktiviert Nullable, XML-Dokumentation, deterministische Builds, Analyzers und gemeinsame Metadaten. `Directory.Packages.props` verwaltet Paketversionen zentral. `global.json` fixiert die SDK-Familie mit zulässigem Roll-forward. Release-Builds werden ausschließlich aus sauberem Checkout erzeugt.

## 16.2 CI-Pipeline

| Stufe | Aktionen |
| --- | --- |
| Validate | Format-/Style-Prüfung, Restore-Lock, Lizenz-/Vulnerability-Scan, Architekturtests |
| Build | Debug und Release für net8.0-windows; zusätzlicher LTS-Build soweit unterstützt |
| Test | Unit, Integration, ausgewählte UI-Automation |
| Visual | Gallery-Screenshotmatrix auf dediziertem Windows-Runner |
| Pack | NuGet-Pakete, Symbols, XML-Dokumentation, SBOM, THIRD-PARTY-NOTICES |
| Publish | Versionierter interner Feed und Release-Archiv; erst nach manueller Freigabe |

## 16.3 Versions- und Branchmodell

`main` bleibt releasefähig. Feature-Branches werden per Pull Request integriert. Releases werden getaggt (`v0.1.0`). Bis zur API-Stabilität ist die Produktversion 0.x. Ab 1.0 gelten strengere Kompatibilitätsregeln. Ein separates `develop` ist nicht erforderlich.

## 16.4 Lizenz- und Fork-Governance

Jede neue Abhängigkeit benötigt eine kurze ADR mit Lizenz, Aktivität, Alternativen, API-Leak-Risiko und Exit-Strategie. Forks werden in einer `forks/registry.yml` dokumentiert und monatlich gegen Upstream geprüft. GPL-/unklare Pakete werden nicht in produktive Pakete aufgenommen, bevor eine ausdrückliche rechtliche und geschäftliche Freigabe vorliegt.

# 17. Component Gallery, Vorlagen und Einführung

## 17.1 Component Gallery

Die Gallery ist keine Marketing-Demo, sondern ausführbare Spezifikation. Jede Seite zeigt Normal-, Hover-, Focus-, Disabled-, ReadOnly-, Busy-, Empty-, Error- und High-Contrast-Zustände, soweit anwendbar. Zusätzlich enthält sie den verwendeten Code, Accessibility-Hinweise, DPI-Hinweise, bekannte Einschränkungen und zugehörige Lastenheft-IDs.

## 17.2 CRUD-Vorlage

Demonstriert Navigation, Liste/Grid, Suche, Filter, Formularlayout, Validierung, Dirty-State, Speichern/Löschen, Fehlerdialog, State Persistence und CSV-Export. Datenquelle ist zunächst eine lokale In-Memory- oder SQLite-Demo; die UI hängt nicht auf SQLite.

## 17.3 Workbench-Vorlage

Demonstriert Seiten-/Dokumenttabs, Navigation, Folder Tree, Editorfläche, Properties/Details, Command-System, Status, Recent Files und spätere Docking-Erweiterung. Sie dient als Vorlage für Mail Workbench und Notes.

## 17.4 Utility- und Dashboard-Vorlagen

Die Utility-Vorlage zeigt Tray, Einstellungen, Fortschritt, Logging und sichere Dateioperationen. Die Dashboard-Vorlage folgt in R2 und kombiniert Filter, KPI-Karten, Charts und gespeicherte Ansichten ohne visuellen Designer.

## 17.5 Einführung in bestehende SASD-Projekte

| Pilotanwendung | Erste Übernahme | Nicht im ersten Schritt |
| --- | --- | --- |
| SASD Prompt Manager | Theme, FormLayout, Dialoge, Grid-State, Search/Filter, Error Presenter | Komplette Navigation neu schreiben |
| SASD Mail Workbench | Shell-Verträge, Status/Progress, Recent Items, Datei/Clipboard | Docking und Editor vor R2 erzwingen |
| SASD Notes | Workbench-Shell, Tree, Tabs, State | Vollständige Markdown-/WebView-Vorschau vor R2 |
| TaskHost Local oder kleines Utility | BaseForm, Dialoge, Tray optional, Einstellungen | Große Daten-/Chartmodule |

Migrationen erfolgen komponentenweise. Bestehende Anwendungen müssen nicht gleichzeitig auf die gesamte Plattform umgestellt werden.

# 18. Umsetzungsplan und Freigabegates

| Phase | Inhalt | Exit-Kriterien |
| --- | --- | --- |
| R0.1 Architektur | Repo, Build, Core-Verträge, ADRs, Lizenzprozess | Build reproduzierbar; Abhängigkeitsregeln getestet. |
| R0.2 UI-Pilot | Krypton vs. native WinForms; Theme, BaseForm, FormLayout | Designer-, DPI-, High-Contrast- und Fokusmatrix bestanden oder klare Fallback-Entscheidung. |
| R0.3 Grid/Dialog-Spike | Grid-Suche/Filter/State; Dialog/Error/Progress | Funktionaler Prototyp in Gallery; keine Fremdtypen im Kern. |
| R1.0 Fundament | F01-F11, Gallery, drei Vorlagen, NuGet | Alle MUSS-Anforderungen mit akzeptierten Abweichungen; erste reale App nutzt Pakete. |
| R1.1 Stabilisierung | API-Bereinigung, Performance, Migration, Dokumentation | Zwei weitere reale SASD-Projekte übernehmen Kernmodule. |
| R2.0 Fachmodule | Charts, Editor, Diff, Bild/PDF/Barcode, WebView2, Docking | Jedes Adapterpaket besitzt eigenen Security-/License-/Lifecycle-Test. |
| R3 | Wizard/Ribbon und bewiesene Zusatzmodule | Nur mit dokumentiertem Produktbedarf und Wartungsverantwortung. |

## 18.1 R0-Technologieentscheidungen

| Thema | Entscheidung | Fallback |
| --- | --- | --- |
| Visuelle Basis | Krypton Standard Toolkit wird zuerst implementiert und getestet. | Native WinForms-Implementierung auf denselben Tokens/Services. |
| Systemdialoge | Ookii.Dialogs hinter `IDialogService`/`IFileDialogService` pilotieren. | Windows-Standarddialoge. |
| Grid | DataGridView/Krypton DataGridView plus SASD-Controller. | Native DataGridView ohne erweiterte Fremderweiterung. |
| Persistenz | System.Text.Json, SchemaVersion, atomare Temp+Replace-Strategie. | Reset auf Default; keine alternative DB in R1. |
| UI-Test | FlaUI/UIA3-Pilot plus AutomationId-Konvention. | Manuelle Checkliste und gezielte Integrationstests bis Stabilisierung. |
| PDF | PDFsharp/MigraDoc in R2. | Keine PDF-Funktion in R1; spätere Build-vs-Buy-Entscheidung. |

# 19. Risiken und technische Gegenmaßnahmen

| Risiko | Technische Auswirkung | Gegenmaßnahme | Stop-Kriterium |
| --- | --- | --- | --- |
| Krypton-Designer instabil | Verlorene Designerwerte oder Formfehler | R0-Designer-Matrix, begrenzte Adapterfläche, native Fallback-Implementierung | Nicht reproduzierbare Designerfehler in Kerncontrols |
| Zu viele Pakete | Komplexe Updates und NuGet-Hölle | Interne Projekttrennung, aber wenige veröffentlichte R1-Pakete | Anwendung benötigt >8 direkte SASD-Pakete für Basisshell |
| Abstraktion zu groß | WinForms-Entwicklung wird langsamer | Nur wiederkehrende Services/Composite Controls abstrahieren | Einfache Form benötigt mehr Boilerplate als native Umsetzung |
| Grid wird Enterprise-Nachbau | Mehrjährige Entwicklung | Fester R1/R2-Funktionsumfang, kommerzieller Benchmark/Build-vs-Buy | Pivot, Formeleditor oder universeller Filterdesigner wird verlangt |
| UI-Automation flakig | Unzuverlässige CI | Stabile AutomationIds, Wait-Strategien, kleine Kernflows, Quarantäneprozess | Fehlerrate >5 Prozent ohne Produktfehler |
| Lizenzänderung | Weitergabe gefährdet | Zentrale Paketversionen, SBOM, regelmäßige Prüfung, austauschbare Adapter | Keine klare Weitergabeberechtigung |
| Handle-/Memory-Leaks | Langläufer instabil | Lifecycle-Tests, Dispose-Konvention, Event-Abmeldung, Ressourcenmessung | Fortlaufendes Wachstum in 100-Zyklen-Test |
| Uneinheitliche Optik | Unprofessionelle Anwendungen | Ein Haupttheme, Tokenmapping, Gallery-Baselines | Spezialcontrol nicht an Theme/Fokus anpassbar |

# 20. Gesamtabnahme

R1 wird abgenommen, wenn alle folgenden Punkte erfüllt sind:

- Alle MUSS-Anforderungen des Lastenhefts sind umgesetzt oder besitzen eine ausdrücklich akzeptierte Abweichung.
- Component Gallery, CRUD-, Workbench- und Utility-Vorlage bauen aus sauberem Checkout und verwenden veröffentlichte SASD-NuGet-Pakete.
- Mindestens eine bestehende SASD-Anwendung verwendet die Pakete produktiv oder in einem vollständigen Integrationsbranch.
- DPI 100/125/150/200, Tastatur, High Contrast, Deutsch/Englisch und zentrale Accessibility-Prüfungen sind dokumentiert bestanden.
- Öffentliche APIs enthalten keine unbegründeten Drittanbieter-Typen und sind vollständig XML-dokumentiert.
- NuGet, Symbols, SBOM, THIRD-PARTY-NOTICES, Changelog, Known Issues und Migrationshinweise sind vorhanden.
- State-, Dialog-, Grid-, Theme- und Ressourcenfehler verhindern keinen sicheren Anwendungsstart oder kontrollierten Reset.

# 21. Zukunftsregister und bewusste Nichtziele

Die späteren Produktlinien bleiben im Repository unter `docs/future/` dokumentiert, erhalten aber keine R1-Projekte, Paketabhängigkeiten oder versteckten Vorabimplementierungen.

| Thema | Dokumentierte Idee | Reaktivierung |
| --- | --- | --- |
| SASD WPF Components | Gemeinsame Tokens und fachliche Verträge prüfen; XAML-/MVVM-native Umsetzung, keine Portierung von WinForms-Controlklassen. | Nach stabiler R1. |
| WinUI 3 | Identische kleine Referenzanwendung als Modern-Desktop-Pilot. | Nach WPF oder konkretem Windows-App-SDK-Bedarf. |
| Avalonia | Nur bei realem Bedarf an Linux/macOS/weiteren Desktops; Core und Pro getrennt bewerten. | Aktuell nicht geplant. |
| Modern Web | ASP.NET Core/Blazor, Web Components, Tailwind/daisyUI; eigenes Lasten-/Pflichtenheft. | Nach Desktop-Prioritäten. |
| ASP.NET Web Forms/ASPX | Legacy-Wartung und Migration; AJAX Control Toolkit nur als Funktionshistorie. | Nur konkretes Legacy-Projekt. |
| Java Business UI | OpenXava/XavaPro, Vaadin, OpenUI5 separat. | Nach C# Desktop und Web. |
| Mobile/macOS/Linux Desktop | Keine aktuelle Umsetzung. | Nur nach Kunden-/Produktbedarf. |
| Großkomponenten | Pivot, Spreadsheet, Scheduler, Gantt, Designer, Office/PDF-Editor, 3D. | Build-vs-Buy pro Projekt; keine Eigenentwicklung ohne Business Case. |

# 22. Rückverfolgbarkeitsmatrix Lastenheft - Pflichtenheft

Die Matrix weist für alle 83 Lastenheftanforderungen die technische Umsetzung beziehungsweise bewusste Zurückstellung nach. Der vollständige Anforderungstext verbleibt im Lastenheft; dieses Pflichtenheft verweist auf die Umsetzungsabschnitte.

| Lastenheft-ID | Priorität | Pflichtenheft | Umsetzungsartefakt | Release |
| --- | --- | --- | --- | --- |
| LH-ZIE-001 | MUSS | Kap. 2, 4, 18 | Produktgrenzen, Architekturprinzipien und Releases | R1 |
| LH-ZIE-002 | MUSS | Kap. 2, 4, 18 | Produktgrenzen, Architekturprinzipien und Releases | R1 |
| LH-ZIE-003 | MUSS | Kap. 2, 4, 18 | Produktgrenzen, Architekturprinzipien und Releases | R1 |
| LH-ZIE-004 | MUSS | Kap. 2, 4, 18 | Produktgrenzen, Architekturprinzipien und Releases | R1 |
| LH-ZIE-005 | SOLL | Kap. 2, 4, 18 | Produktgrenzen, Architekturprinzipien und Releases | R2/R3 |
| LH-RHM-001 | MUSS | Kap. 3, 5, 15, 16 | Technologie, Repository, Paketierung und Governance | R1 |
| LH-RHM-002 | MUSS | Kap. 3, 5, 15, 16 | Technologie, Repository, Paketierung und Governance | R1 |
| LH-RHM-003 | MUSS | Kap. 3, 5, 15, 16 | Technologie, Repository, Paketierung und Governance | R1 |
| LH-RHM-004 | MUSS | Kap. 3, 5, 15, 16 | Technologie, Repository, Paketierung und Governance | R1 |
| LH-RHM-005 | MUSS | Kap. 3, 5, 15, 16 | Technologie, Repository, Paketierung und Governance | R1 |
| LH-RHM-006 | SOLL | Kap. 3, 5, 15, 16 | Technologie, Repository, Paketierung und Governance | R2/R3 |
| LH-GRU-001 | MUSS | Kap. 7, 8 | Designsystem und Basiskomponenten | R1 |
| LH-GRU-002 | MUSS | Kap. 7, 8 | Designsystem und Basiskomponenten | R1 |
| LH-GRU-003 | MUSS | Kap. 7, 8 | Designsystem und Basiskomponenten | R1 |
| LH-GRU-004 | MUSS | Kap. 7, 8 | Designsystem und Basiskomponenten | R1 |
| LH-GRU-005 | MUSS | Kap. 7, 8 | Designsystem und Basiskomponenten | R1 |
| LH-GRU-006 | SOLL | Kap. 7, 8 | Designsystem und Basiskomponenten | R2/R3 |
| LH-INP-001 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-002 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-003 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-004 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-005 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-006 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-007 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-008 | MUSS | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R1 |
| LH-INP-009 | SOLL | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R2/R3 |
| LH-INP-010 | KANN | Kap. 8.2, 9 | Formularlayout, Eingaben und Validierung | R2/R3 |
| LH-NAV-001 | MUSS | Kap. 10 | Shell, Commands, Navigation und Workspace | R1 |
| LH-NAV-002 | MUSS | Kap. 10 | Shell, Commands, Navigation und Workspace | R1 |
| LH-NAV-003 | MUSS | Kap. 10 | Shell, Commands, Navigation und Workspace | R1 |
| LH-NAV-004 | MUSS | Kap. 10 | Shell, Commands, Navigation und Workspace | R1 |
| LH-NAV-005 | MUSS | Kap. 10 | Shell, Commands, Navigation und Workspace | R1 |
| LH-NAV-006 | SOLL | Kap. 10 | Shell, Commands, Navigation und Workspace | R2 |
| LH-NAV-007 | KANN | Kap. 10 | Shell, Commands, Navigation und Workspace | R3 |
| LH-DAT-001 | MUSS | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R1 |
| LH-DAT-002 | MUSS | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R1 |
| LH-DAT-003 | MUSS | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R1 |
| LH-DAT-004 | MUSS | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R1/R2 |
| LH-DAT-005 | MUSS | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R1 |
| LH-DAT-006 | MUSS | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R1 |
| LH-DAT-007 | SOLL | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R2/R3 |
| LH-DAT-008 | SOLL | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | R2/R3 |
| LH-DAT-009 | ZURÜCKGESTELLT | Kap. 11 | Grid, Listen, Bäume, Paging und Exporte | Zukunftsregister |
| LH-FED-001 | MUSS | Kap. 12 | Dialoge, Fehler, Notifications und Progress | R1 |
| LH-FED-002 | MUSS | Kap. 12 | Dialoge, Fehler, Notifications und Progress | R1 |
| LH-FED-003 | MUSS | Kap. 12 | Dialoge, Fehler, Notifications und Progress | R1 |
| LH-FED-004 | MUSS | Kap. 12 | Dialoge, Fehler, Notifications und Progress | R1 |
| LH-FED-005 | MUSS | Kap. 12 | Dialoge, Fehler, Notifications und Progress | R1 |
| LH-FED-006 | SOLL | Kap. 12 | Dialoge, Fehler, Notifications und Progress | R2/R3 |
| LH-SYS-001 | MUSS | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R1 |
| LH-SYS-002 | MUSS | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R1 |
| LH-SYS-003 | MUSS | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R1 |
| LH-SYS-004 | MUSS | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R1 |
| LH-SYS-005 | SOLL | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R2/R3 |
| LH-SYS-006 | SOLL | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R2 |
| LH-SYS-007 | KANN | Kap. 13 | Windows-, Datei-, Clipboard-, Tray- und WebView-Integration | R2/R3 |
| LH-DOC-001 | SOLL | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | R2 |
| LH-DOC-002 | SOLL | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | R2 |
| LH-DOC-003 | SOLL | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | R2 |
| LH-DOC-004 | SOLL | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | R2 |
| LH-DOC-005 | SOLL | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | R2 |
| LH-DOC-006 | SOLL | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | R2 |
| LH-DOC-007 | ZURÜCKGESTELLT | Kap. 14.2-14.6 | Editoren, Diff, PDF, Bild und Barcode | Zukunftsregister |
| LH-VIZ-001 | SOLL | Kap. 14.1, 17.4 | Charts, KPI und Dashboard-Vorlage | R2 |
| LH-VIZ-002 | SOLL | Kap. 14.1, 17.4 | Charts, KPI und Dashboard-Vorlage | R2 |
| LH-VIZ-003 | KANN | Kap. 14.1, 17.4 | Charts, KPI und Dashboard-Vorlage | R2 |
| LH-VIZ-004 | SOLL | Kap. 14.1, 17.4 | Charts, KPI und Dashboard-Vorlage | R2 |
| LH-QUL-001 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-002 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-003 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-004 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-005 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-006 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-007 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-008 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-QUL-009 | SOLL | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R2/R3 |
| LH-QUL-010 | MUSS | Kap. 6, 15 | DPI, Accessibility, Security, Tests und API-Qualität | R1 |
| LH-LIE-001 | MUSS | Kap. 16, 17 | Lieferung, Gallery, Vorlagen, NuGet und Dokumentation | R1 |
| LH-LIE-002 | MUSS | Kap. 16, 17 | Lieferung, Gallery, Vorlagen, NuGet und Dokumentation | R1 |
| LH-LIE-003 | MUSS | Kap. 16, 17 | Lieferung, Gallery, Vorlagen, NuGet und Dokumentation | R1 |
| LH-LIE-004 | SOLL | Kap. 16, 17 | Lieferung, Gallery, Vorlagen, NuGet und Dokumentation | R2/R3 |
| LH-LIE-005 | MUSS | Kap. 16, 17 | Lieferung, Gallery, Vorlagen, NuGet und Dokumentation | R1 |
| LH-LIE-006 | MUSS | Kap. 16, 17 | Lieferung, Gallery, Vorlagen, NuGet und Dokumentation | R1 |

# 23. Detailanhang: Komponentenfamilien F01-F20

| ID | Familie | Priorität | Release | Technische Umsetzung | Referenzbasis |
| --- | --- | --- | --- | --- | --- |
| F01 | Basiskomponenten und Formulare | MUSS | R1 | SasdForm, SasdDialogForm, SasdUserControl, primitive Controls über Theme/FormLayout statt Wrapperflut. | Microsoft WinForms; bevorzugt über SASD-Fassade, visuell ggf. Krypton |
| F02 | Design Tokens und Themes | MUSS | R1 | SasdThemeDefinition, IThemeService, Krypton-Palette, IconService, Light/Dark/High Contrast. | SASD-eigene Tokens; Krypton-Paletten als erste Umsetzung |
| F03 | Application Shell | MUSS | R1 | SasdShellForm mit Regionen, Navigation, Arbeitsbereich und Status. | SASD Shell auf WinForms/Krypton |
| F04 | Menüs und Befehle | MUSS | R1 | IUiCommand, CommandManager, Menü-/Toolbar-/Kontextmenü-Bindings und Shortcuts. | WinForms/Krypton plus SASD Command System |
| F05 | Navigation und Layout | MUSS | R1 | NavigationView, Breadcrumb, DocumentTabs, Split-/Section-Layouts, StatusBar. | WinForms/Krypton |
| F06 | Formularlayout und Validierung | MUSS | R1 | FieldLayout, ValidationCoordinator, ValidationSummary, DataAnnotations-/Custom-Regeln. | SASD-eigene Layout-/Validation-Schicht |
| F07 | DataGrid und Datentabellen | MUSS | R1/R2 | SasdDataGrid + GridController<T>, Filter/Search, State, Paging/Virtualisierung, CSV. | DataGridView/Krypton DataGridView; Erweiterungen selektiv |
| F08 | Listen und Bäume | MUSS | R1/R2 | SasdListView, SasdTreeView, Lazy Loading, Folder-Provider. | WinForms/Krypton; ggf. ObjectListView nur nach Pilot |
| F09 | Dialoge und Feedback | MUSS | R1 | DialogService, ErrorDialog, NotificationService, ProgressDialog, BusyOverlay, Tooltips. | SASD Dialog-/Notification-Services; Ookii für Systemdialoge |
| F10 | Datei- und Systemintegration | MUSS | R1 | FileDialog, DragDrop, Clipboard, RecentItems, Tray und sichere Shell-Services. | Windows APIs; Ookii.Dialogs; SASD Services |
| F11 | Zustand und Benutzereinstellungen | MUSS | R1 | SasdStateStore mit JSON, SchemaVersion, Migration, Backup, Reset und MRU. | SASD State Persistence |
| F12 | Diagramme und KPI-Anzeigen | SOLL | R2 | ScottPlot-Adapter, KPI-Karten, textuelle Alternativen; LiveCharts optional. | ScottPlot primär; LiveCharts2 für Dashboard-Fälle |
| F13 | Editoren für technische Inhalte | SOLL | R2 | Markdown/Markdig, Code/ScintillaNET, Diff/DiffPlex in separaten Paketen. | ScintillaNET; Markdig; DiffPlex; ggf. WebView2 für Vorschau |
| F14 | Bild-, Barcode- und PDF-Ausgabe | SOLL | R2 | ImageBox-Pilot, QRCoder/ZXing, PDFsharp/MigraDoc. | Cyotek ImageBox; QRCoder/ZXing; PDFsharp/MigraDoc oder geprüfte Alternative |
| F15 | WebView-Host | SOLL | R2 | Gehärteter WebView2-Host mit Allowlist und versionierter Message Bridge. | Microsoft WebView2 |
| F16 | Docking und Workspace | SOLL | R2 | IWorkspaceHost und Krypton Docking/Workspace in R2. | Krypton Docking/Workspace nach Pilot |
| F17 | Ribbon und Assistenten | KANN | R3 | Wizard-Vertrag und optional Krypton Ribbon in R3. | Krypton Ribbon; SASD Wizard Contract |
| F18 | PropertyGrid und erweiterte Datenansichten | SOLL | R2/R3 | PropertyEditor, Column Chooser, Summaries und Master-Detail als R2/R3-Erweiterung. | Krypton/Standard; Adapter |
| F19 | Komponenten-Galerie und Vorlagen | MUSS | R1/R2 | Component Gallery, CRUD-, Workbench-, Utility-, später Dashboard-/Wizard-Vorlagen. | SASD-eigene Referenzanwendungen |
| F20 | Qualität, Tests und Dokumentation | MUSS | R1+ | Testpyramide, DPI/Accessibility/Localization, API-Doku, CI, SBOM und Releases. | SASD Test-/Dokumentationsstandard |

# 24. Dokumentenlenkung und Änderungsprotokoll

| Version | Datum | Status | Änderung |
| --- | --- | --- | --- |
| 0.1 | 23. Juli 2026 | Entwurf | Erstes Pflichtenheft aus Lastenheft v0.1; Architektur, Pakete, Komponenten, Schnittstellen, Qualität, Releases und vollständige Traceability festgelegt. |


**Ende des Pflichtenhefts**
