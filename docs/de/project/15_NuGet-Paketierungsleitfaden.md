# NuGet-Paketierungsleitfaden

## 1. Ziele

- wenige direkte Paketreferenzen für Consumer;
- klare Trennung optionaler schwerer Adapter;
- keine unbeabsichtigten transitiven UI-Suiten;
- identische Versionierung des R1-Kerns;
- gute IntelliSense-Erfahrung durch XML-Dokumentation und Symbole.

## 2. Paketgruppen

### Kern

- `Sasd.Ui.Core`
- `Sasd.Ui.WinForms`
- wenige logisch zusammenhängende R1-Pakete für Theme, Shell, Data, Dialogs, Windows und State.

### Adapter

- Krypton-Implementierung;
- ScottPlot;
- ScintillaNET;
- WebView2;
- Docking;
- PDF/Media.

Adapter sind opt-in und dürfen nicht über ein allgemeines Metapaket in jede Anwendung gelangen.

## 3. Consumer-Paketstrategie

Für typische Anwendungen kann ein kuratiertes R1-Metapaket erwogen werden, wenn es nur leichte Kernpakete bündelt. Ein Metapaket darf keine R2-Adapter enthalten.

## 4. Metadaten

Jedes Paket enthält:

- ID, Titel, Beschreibung;
- Version und Repository-URL;
- Lizenzexpression oder Lizenzdatei;
- Tags;
- README;
- Symbolpaket;
- XML-Dokumentation;
- Release Notes beziehungsweise Changelog-Link;
- deterministische Repository-/Commitmetadaten.

## 5. Abhängigkeitsregeln

- exakte oder kompatible Mindestversionen nach zentraler Strategie;
- keine breiten, unkontrollierten Versionsbereiche;
- keine privaten Assets, die Consumer unerwartet benötigen;
- native Runtimeabhängigkeiten ausdrücklich dokumentieren;
- Build-/Analyzerpakete korrekt als PrivateAssets markieren.

## 6. Paketprüfung

Vor Veröffentlichung:

- Paketinhalt inspizieren;
- leere Consumerlösung erstellen;
- Paket aus geplantem Feed installieren;
- Build, Designer und Minimalbeispiel testen;
- transitive Abhängigkeiten und Lizenznotices prüfen;
- Deinstallation/Updatepfad prüfen.

## 7. Paketanzahl als Architekturindikator

Wenn eine Basisshell mehr als etwa acht direkte SASD-Pakete benötigt, wird die Paketstruktur überprüft. Interne Projekttrennung darf größer sein als die öffentliche Consumeroberfläche.
