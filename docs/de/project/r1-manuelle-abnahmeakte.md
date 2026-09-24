# R1-Manuelle-Abnahmeakte

**Zweck:** Release-Evidence für Prüfungen, die nicht aus Build oder headless Smoke-Tests abgeleitet werden können  
**Ausgangsstatus:** jede Prüfung bleibt `NOT RUN`, bis sie tatsächlich ausgeführt und dokumentiert wurde  
**Bezug:** [R1.0-Abschlussplan](r1-abschlussplan.md) · [Accessibility-/DPI-Checkliste](16_Barrierefreiheit-und-DPI-Checkliste.md) · [Integrated Showcase](../../../examples/Sasd.Ui.PlatformShowcase/README.md)

## 1. Evidence-Kopf

| Feld | Erfasster Wert |
| --- | --- |
| Repository Commit-SHA | NOT RUN |
| Tester | NOT RUN |
| Datum/Uhrzeit | NOT RUN |
| Windows Edition/Build | NOT RUN |
| .NET SDK | NOT RUN |
| Visual-Studio-Version | NOT RUN |
| Monitoranordnung | NOT RUN |
| Display-Skalierungen | NOT RUN |
| Windows High Contrast/Contrast Theme | NOT RUN |
| Screenreader/UIA-Werkzeug | NOT RUN |
| Notizen/Evidence-Ablage | NOT RUN |

Erlaubte Ergebniswerte sind `PASS`, `FAIL`, `NOT RUN` und `N/A`. Ein Fehler erhält eine reproduzierbare Notiz oder Issue-Referenz.

## 2. Visual-Studio-Designer

Für jede repräsentative designerfähige Oberfläche: öffnen, eine harmlose reversible Eigenschaft/Layout-Einstellung ändern, speichern, Designer schließen, erneut öffnen und stabile Serialisierung prüfen.

| Oberfläche | Öffnen | Ändern/speichern | Erneut öffnen | Keine unerwarteten generierten Änderungen | Ergebnis/Notizen |
| --- | --- | --- | --- | --- | --- |
| Nativer `SasdForm`-Host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| `SasdDialogForm`-Host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| `SasdUserControl`-Host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| `SasdSectionPanel` + `SasdFieldLayout` | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| Repräsentative Data Controls | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| Repräsentative Shell Controls | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| Krypton-Pilot-Host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |

Die Krypton-Zeile liefert Evidence für die spätere Native-vs.-Krypton-Entscheidung und legt keinen Standard fest.

## 3. DPI-/Layout-Matrix

Integrated Showcase und Acceptance Lab bei jeder verfügbaren Skalierung prüfen.

| Skalierung | Overview | Forms | Data/Grid | Controls | Feedback/Dialoge | Acceptance-Snapshot erfasst | Ergebnis/Notizen |
| ---: | --- | --- | --- | --- | --- | --- | --- |
| 100% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| 125% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| 150% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| 200% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |

Mixed-DPI-Szenarien: Wechsel niedrige→höhere Skalierung, höhere→niedrigere Skalierung und Wiederherstellung des Window State nach Monitor-/Skalierungswechsel. `N/A` nur verwenden, wenn die vorhandene Hardware das Szenario wirklich nicht ermöglicht.

## 4. Echtes Windows High Contrast

Ein echtes Windows Contrast/High-Contrast Theme aktivieren. Das SASD High Contrast Theme allein gilt nicht als Ersatz.

Prüfen: Lesbarkeit, sichtbarer Fokus, Status/Validation nicht nur über Farbe, sichtbare Auswahl in Data/List/Tree, erreichbare Dialog-/Cancel-Aktionen sowie Beobachtungen für Native versus Krypton. Alle Ergebnisse starten mit `NOT RUN`.

## 5. Repräsentative Keyboard-only-Flows

Ohne Maus prüfen:

| Flow | Ergebnis | Notizen |
| --- | --- | --- |
| Acceptance Lab Tab / Shift+Tab | NOT RUN | |
| SearchBox Edit → Enter → Escape/Clear | NOT RUN | |
| Validation Summary → ungültiges Feld | NOT RUN | |
| Breadcrumb Overflow | NOT RUN | |
| Document Tabs auswählen/schließen | NOT RUN | |
| BusyOverlay Cancellation | NOT RUN | |
| Repräsentativer Dialog mit Enter/Escape | NOT RUN | |

## 6. Accessibility / UI Automation

Windows Narrator und/oder ein ausdrücklich freigegebenes Inspection-Tool verwenden und Werkzeug/Version im Evidence-Kopf erfassen.

| Oberfläche | Name/Rolle/Status | Fokus/Reihenfolge | Dynamische-/Fehlerinformation | Ergebnis/Notizen |
| --- | --- | --- | --- | --- |
| Forms und Validation | NOT RUN | NOT RUN | NOT RUN | |
| Search/Filter/Pager | NOT RUN | NOT RUN | NOT RUN | |
| List/Tree/Grid | NOT RUN | NOT RUN | NOT RUN | |
| Breadcrumb/Document Tabs/Shell | NOT RUN | NOT RUN | NOT RUN | |
| Busy/Progress/Notifications | NOT RUN | NOT RUN | NOT RUN | |

Automatisierte Accessible-Property-Checks unterstützen diese Prüfung, ersetzen sie aber nicht.

## 7. Lokalisierung und lange Texte

Deutsch, Englisch, absichtlich verlängerte Labels, repräsentative Datums-/Zahlenwerte und kritisches Clipping/Overflow prüfen. Alle Ergebnisse starten mit `NOT RUN`.

## 8. Abschluss der Session

| Feld | Wert |
| --- | --- |
| Gesamtergebnis | NOT RUN |
| Blockierende Defekte | NOT RUN |
| Nicht-blockierende Beobachtungen | NOT RUN |
| Issue-/PR-Referenzen | NOT RUN |
| Re-Test erforderlich | NOT RUN |

Eine Session schließt kein R1-Gate nur deshalb, weil die Mehrheit der Zeilen besteht. Kritische Keyboard-/Fokusdefekte, nicht behebbares Designer-Verhalten oder wesentliche DPI-/High-Contrast-Fehler bleiben Release-Blocker, bis sie behoben oder ausdrücklich im Entscheidungsprozess akzeptiert wurden.
