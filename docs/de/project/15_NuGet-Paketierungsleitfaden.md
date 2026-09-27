# NuGet-Paketierungsleitfaden

## 1. Ziele

- wenige direkte Paketreferenzen für Consumer;
- klare Trennung optionaler schwerer Adapter;
- keine unbeabsichtigten transitiven UI-Suiten;
- identische Versionierung des R1-Kerns;
- gute IntelliSense-Erfahrung durch XML-Dokumentation und Symbole.

## 2. Paketgruppen

Das Repository führt den Pack-Dry-Run bewusst für granulare interne Produktprojekte aus. So werden Metadaten, Symbole, Dependency-Beziehungen und XML-Dokumentation früh geprüft. Dieses technische Inventar ist absichtlich größer als die unterstützte direkte Consumer-Oberfläche.

### Unterstützte direkte R1-Consumer-Einstiegspakete

| Paket | Zweck für Consumer |
| --- | --- |
| `Sasd.Ui.Core` | Plattformneutrale Verträge und Semantik bei direktem Bedarf. |
| `Sasd.Ui.WinForms` | Minimales natives WinForms-Fundament. |
| `Sasd.Ui.WinForms.App` | Empfohlenes Komfort-Metapaket für typische R1-Anwendungen. |
| `Sasd.Ui.WinForms.Data` | Explizites Opt-in für Grid, Search/Filter/Paging, Listen und Bäume. |
| `Sasd.Ui.WinForms.Krypton` | Explizite optionale visuelle Implementierung. |

Die fünf Einstiegspakete sind eine Dokumentations- und Supportgrenze. Da die Plattform getrennte Assemblies beibehält, bleiben Implementierungspakete wie Commands, Forms, Dialogs, Shell, State, Theming und Windows reale transitive NuGet-Abhängigkeiten und können auf dem Feed sichtbar sein.

### R1-Application-Metapaket

`Sasd.Ui.WinForms.App` ist dependency-only und besitzt keine eigene Runtime-Assembly. Seine direkten Abhängigkeiten sind exakt:

- `Sasd.Ui.WinForms.Commands`;
- `Sasd.Ui.WinForms.Dialogs`;
- `Sasd.Ui.WinForms.Forms`;
- `Sasd.Ui.WinForms.Shell`;
- `Sasd.Ui.WinForms.State`;
- `Sasd.Ui.WinForms.Theming`;
- `Sasd.Ui.WinForms.Windows`.

`Sasd.Ui.WinForms.Data`, `Sasd.Ui.WinForms.Krypton`, `Sasd.Ui.WinForms.Media` und spätere R2/R3-Adapter sind bewusst ausgeschlossen. Der Pack-Dry-Run erzwingt diese Grenze.

### Spezialadapter

ScottPlot, ScintillaNET, WebView2, Docking, PDF/Media und ähnliche Funktionen bleiben getrennte Opt-in-Pakete und gelangen nicht über das allgemeine R1-App-Metapaket in jede Anwendung.

## 3. Consumer-Paketstrategie

Eine typische R1-Anwendung startet mit:

```xml
<ItemGroup>
  <PackageReference Include="Sasd.Ui.WinForms.App" Version="<freigegebene-version>" />
</ItemGroup>
```

`Sasd.Ui.WinForms.Data` wird nur bei datenlastigen Oberflächen ergänzt. `Sasd.Ui.WinForms.Krypton` wird nur ergänzt, wenn die Anwendung diese visuelle Implementierung bewusst auswählt.

Sehr kleine Anwendungen dürfen direkt mit `Sasd.Ui.Core` oder `Sasd.Ui.WinForms` arbeiten. Direkte Referenzen auf Implementierungspakete bleiben für begründete Spezialfälle möglich, sind aber nicht der normale R1-Einstieg.

ADR-0017 dokumentiert die Paketentscheidung und ihre Review-Kriterien.

## 4. Metadaten

Jedes Paket enthält gemeinsame Identitätsmetadaten:

- ID, Titel, Beschreibung;
- Version und Repository-URL;
- Lizenzexpression oder Lizenzdatei;
- Tags;
- README;
- Release Notes beziehungsweise Changelog-Link;
- deterministische Repository-/Commitmetadaten.

Konkrete Assembly-Pakete enthalten zusätzlich XML-Dokumentation und ein Symbolpaket. Dependency-only-Metapakete enthalten bewusst weder Runtime-Assembly noch Symbolpaket.

## 5. Abhängigkeitsregeln

- exakte oder kompatible Mindestversionen nach zentraler Strategie;
- keine breiten, unkontrollierten Versionsbereiche;
- keine privaten Assets, die Consumer unerwartet benötigen;
- native Runtimeabhängigkeiten ausdrücklich dokumentieren;
- Build-/Analyzerpakete korrekt als PrivateAssets markieren.

## 6. Paketprüfung

Vor Veröffentlichung:

- Paketinhalt und Dependency-Manifest inspizieren;
- frisches Consumer-Projekt erzeugen;
- Paket aus geplantem Feed installieren;
- repräsentative öffentliche Typen restaurieren und bauen;
- prüfen, dass App exakt die erlaubten Module und nicht Data/Krypton/Media einzieht;
- Designer und Minimalbeispiel testen;
- transitive Abhängigkeiten und Lizenznotices prüfen;
- Deinstallation/Updatepfad prüfen.

Der aktuelle Dry-Run führt bereits einen lokalen Feed-Restore/Build-Smoke für `Sasd.Ui.WinForms.App` aus; der spätere interne Release Candidate wiederholt diesen Weg gegen den echten Feed.

## 7. Paketanzahl als Architekturindikator

Wenn eine Basisshell mehr als etwa acht direkte SASD-Pakete benötigt, wird die Paketstruktur überprüft. Interne Projekttrennung darf größer sein als die öffentliche Consumeroberfläche.
