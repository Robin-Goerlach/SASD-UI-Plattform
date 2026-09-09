# Teststrategie

## 1. Ziel

Die Teststrategie belegt nicht nur funktionale Korrektheit, sondern auch Designerfähigkeit, DPI-Verhalten, Accessibility, Ressourcenfreigabe und Consumer-Nutzbarkeit.

## 2. Testpyramide

| Ebene | Zweck | Beispiele |
| --- | --- | --- |
| Unit | reine Logik | Tokens, Commands, Filter, Migration, Validatoren |
| Architektur | Strukturregeln | Abhängigkeitsgraph, Public-API-Fremdtypen, Namespaces |
| Integration | Dateisystem/Services | StateStore, PDF, Barcode, Clipboard-Fakes |
| Component | Control in Testhost | Zustände, Events, Themewechsel, Dispose |
| UI Automation | Benutzerabläufe | Navigation, Dialog, Grid, Tastatur |
| Visual Regression | visuelle Abweichung | Theme, DPI, lange Texte, High Contrast |
| Manuell | schwer automatisierbare Qualität | Designer, Mehrmonitor, Accessibility Insights |

## 3. Verbindliche Testmatrix

### Betriebssystem und Runtime

- Windows 11 als Hauptreferenz;
- Windows 10 soweit Supportziel und Runner verfügbar;
- .NET 8 Releasebuild;
- zusätzliche aktuelle LTS-Kompatibilitätsprüfung ohne unnötiges Multi-Targeting.

### DPI

- 100, 125, 150 und 200 Prozent;
- Wechsel zwischen Monitoren mit unterschiedlicher Skalierung;
- Fensterwiederherstellung nach DPI-/Monitoränderung;
- keine abgeschnittenen Beschriftungen oder unsichtbaren Buttons.

### Themes

- Light;
- Dark;
- High Contrast beziehungsweise systemnahe kontrastreiche Darstellung;
- Laufzeitwechsel ohne Neustart, soweit unterstützt.

### Kultur und Texte

- Deutsch;
- Englisch;
- Pseudolokalisierung/verlängerte Texte;
- Datum, Zahl, Währung und Sortierung.

## 4. Komponentenabnahme

Jede visuelle Komponente zeigt und testet, soweit anwendbar:

- Normal;
- Hover;
- Fokus;
- Disabled;
- ReadOnly;
- Busy;
- Empty;
- Error/Warning;
- lange Texte;
- High Contrast;
- Tastaturbedienung.

## 5. Grid-Testfälle

- Spaltensortierung und Mehrfachsortierung nach Scope;
- Suche und Filterkombination;
- Auswahlwechsel und Selection Persistence;
- Editierung und Validierungsfehler;
- CSV-Export mit Kultur-/Escapingprüfung;
- gespeicherte Spaltenreihenfolge und -breite;
- Paging mit Cancellation und Race-Condition-Schutz;
- leere, kleine und größere Datenmengen;
- Datenquelle In-Memory sowie simulierte entfernte Quelle.

## 6. StateStore-Testfälle

- erstmaliger Start ohne Datei;
- atomarer erfolgreicher Schreibvorgang;
- Prozessabbruch zwischen Temp und Replace;
- beschädigtes JSON;
- unbekannte Felder;
- ältere SchemaVersion;
- fehlgeschlagene Migration;
- Backup und Reset;
- keine Secrets in bekannten State-Typen.

## 7. Ressourcen- und Langzeittests

- 100 Öffnen-/Schließen-Zyklen relevanter Fenster;
- GDI-/USER-Handles vor/nach Zyklus;
- GC-Freigabe geschlossener Views;
- Timer-/Event-/Task-Abbruch;
- WebView2-/Editorprozesse nach R2;
- Bild- und Iconcache mit Grenzwerten.

## 8. UI-Automation-Regeln

- stabile `AutomationId` statt sichtbarer Texte als primärer Selektor;
- explizite Wait-Bedingungen statt Sleep;
- kleine, geschäftskritische Kernflows;
- reproduzierbarer Testzustand;
- flakige Tests werden nicht still ignoriert, sondern mit Ursache und Quarantänefrist dokumentiert.

## 9. Freigaberegel

Ein Release wird nicht freigegeben, wenn ein kritischer Test rot ist, eine neue visuelle Komponente keine Gallery-Abdeckung besitzt oder bekannte Handle-/Designerprobleme ohne akzeptierte Ausnahme bestehen.
