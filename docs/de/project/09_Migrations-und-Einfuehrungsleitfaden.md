# Migrations- und Einführungsleitfaden

## 1. Grundsatz

Bestehende SASD-Anwendungen werden nach dem Strangler-Prinzip schrittweise migriert. Es gibt keinen Big-Bang-Umbau und keine Pflicht, alle UI-Bereiche gleichzeitig zu ersetzen.

## 2. Vorbereitung

- aktuelle Screenshots und Kernabläufe dokumentieren;
- lokale UI-Helfer, Dialoge, Themecode, Grid-State und Fehleranzeigen inventarisieren;
- technische Schulden und bekannte Bedienungsprobleme markieren;
- Baseline-Tests für kritische Abläufe erstellen;
- Zielpakete und Migrationsumfang festlegen.

## 3. Empfohlene Reihenfolge

1. Themes und Icons;
2. Dialog/Error/Progress;
3. StateStore und Recent Items;
4. Search/Filter/Grid-State;
5. FormLayout und Validation;
6. Commands;
7. Shell/Navigation/Tabs;
8. optionale R2-Module.

Diese Reihenfolge liefert frühen Nutzen bei geringerer struktureller Gefahr.

## 4. Prompt Manager

Geeigneter erster Umfang:

- Theme und Icons;
- `SasdFieldLayout` in klar begrenzten Dialogen;
- zentraler Error Presenter;
- SearchBox/FilterBar;
- Grid-State und CSV-Export;
- StateStore.

Nicht im ersten Schritt: vollständiger Austausch der gesamten Navigation oder gleichzeitige Datenmodelländerung.

## 5. Mail Workbench

- Status/Progress und Fehlerfluss;
- Recent Items;
- Datei/Clipboard/DragDrop;
- Command-Verträge;
- Shellregionen schrittweise.

Docking und Code-/Markdowneditor bleiben R2 und werden nicht vorgezogen, nur weil die Anwendung langfristig davon profitieren könnte.

## 6. Notes

- Tree/List-Verträge;
- Dokumenttabs und Dirty-State;
- StateStore;
- Workbench-Shell.

Markdown-Vorschau/WebView2 erst nach R2-Sicherheits- und Lifecycle-Pilot.

## 7. Utility/TaskHost

Eine kleine Anwendung eignet sich als frühes produktives Testfeld für BaseForm, Dialoge, Einstellungen, Tray und Fortschritt.

## 8. Kompatibilitätsadapter

Temporäre Adapter dürfen alte lokale Interfaces auf neue SASD-Services abbilden. Sie besitzen:

- klaren Namen und Verfallsdatum;
- keine neue Fachlogik;
- Tests für Alt- und Neupfad;
- dokumentiertes Entfernen nach Migration.

## 9. Rollback

- Migration in kleinen PRs;
- alte Implementierung erst entfernen, wenn neue Version produktiv geprüft ist;
- UI-State vor Schemawechsel sichern;
- Feature-Schalter nur dort, wo sie wirklich Rollback ermöglichen und nicht dauerhaft Komplexität erzeugen.

## 10. Abnahmekriterien

Eine Teilmigration ist abgeschlossen, wenn:

- lokale Duplikatlogik entfernt oder eindeutig als Restbestand markiert ist;
- Nutzerabläufe und Tastaturbedienung unverändert oder verbessert sind;
- keine neue direkte Drittanbieterabhängigkeit in der Anwendung entsteht, sofern ein SASD-Paket vorgesehen ist;
- State-/Layoutmigration getestet ist;
- bekannte Abweichungen dokumentiert sind.
