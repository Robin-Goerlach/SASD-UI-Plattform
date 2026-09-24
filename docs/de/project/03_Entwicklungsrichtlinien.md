# Entwicklungsrichtlinien

## 1. C#-Grundregeln

- Nullable Reference Types sind aktiviert.
- Öffentliche APIs verwenden eindeutige Namen und XML-Dokumentation.
- `async void` ist nur für echte UI-Eventhandler zulässig.
- Asynchrone Operationen akzeptieren, soweit sinnvoll, `CancellationToken`.
- Exceptions werden nicht als normaler Kontrollfluss verwendet.
- Ergebnisobjekte tragen erwartbare Benutzer-/Operationsfehler; Programmierfehler dürfen sichtbar fehlschlagen.
- Globale statische Zustände werden vermieden.

## 2. WinForms- und Designerregeln

- visuelle öffentliche Controls besitzen parameterlose Konstruktoren;
- Designerpfade führen keine Datei-, Netzwerk-, Datenbank- oder DI-Operationen aus;
- generische Controls werden nicht direkt als Designeroberfläche veröffentlicht;
- Designerwerte müssen nach Schließen und erneutem Öffnen erhalten bleiben;
- UI-Thread-Zugriffe werden über `SasdUiDispatcher` oder kontrollierte Control-Invocation ausgeführt;
- UI-Controls starten keine versteckten Hintergrundthreads ohne dokumentierten Lebenszyklus.

## 3. Vererbung und Komposition

Vererbung wird auf belastbare Basisklassen begrenzt (`SasdForm`, `SasdDialogForm`, `SasdUserControl`). Fachliches Verhalten liegt in Services, Controllern, Bindern und Commands. Neue tiefe Klassenhierarchien benötigen eine ADR.

## 4. Öffentliche APIs

- Kernpakete geben keine Krypton-, ScottPlot-, ScintillaNET- oder WebView2-Typen zurück.
- herstellerspezifische Erweiterungen liegen im Adapterpaket und sind als solche benannt.
- optionale Parameter erhalten sichere Defaults.
- Events besitzen nachvollziehbaren Sender und dokumentierte Lebensdauer.
- IDisposable-/IAsyncDisposable-Verantwortung wird explizit beschrieben.
- neue API wird vor Merge in einem Consumer-Sample verwendet.

## 5. Commands und Benutzeraktionen

- dieselbe Aktion wird über eine Command-Instanz an Menü, Toolbar, ContextMenu und Shortcut gebunden;
- `CanExecute` darf keine teuren Operationen ausführen;
- parallele Ausführung wird ausdrücklich erlaubt oder verhindert;
- Fehler gehen an den zentralen Error Presenter;
- langlaufende Commands unterstützen Busy/Progress und Cancellation.

## 6. Fehlerbehandlung

- Benutzer sehen verständliche, handlungsorientierte Meldungen;
- technische Details sind separat kopierbar;
- Logs enthalten Korrelation, Komponente und Operation, aber keine Geheimnisse;
- optionale UI-Funktionen dürfen den Start nicht verhindern;
- beschädigte Layout-/State-Dateien werden isoliert zurückgesetzt.

## 7. Ressourcenverwaltung

- Bitmaps, Icons, Streams, Timer, Handles und Drittanbietercontrols werden disposed;
- globale Events und Services verwenden explizite Abmeldung oder schwache Kopplung;
- Caches besitzen Grenzen und dokumentierte Besitzverhältnisse;
- geschlossene Views müssen vom GC freigegeben werden können.

## 8. Lokalisierung und Formatierung

- gemeinsame Texte liegen in Plattformressourcen;
- Anwendungstexte bleiben im Consumer;
- technische Persistenzformate sind kulturinvariant;
- sichtbare Zahlen, Datum und Währung verwenden aktive Kultur;
- Layouts werden mit verlängerten Texten geprüft.

## 9. Kommentare

Kommentare erklären Entscheidung, Risiko oder nicht offensichtliche Einschränkung. Sie wiederholen nicht den Code. Öffentliche XML-Kommentare enthalten Zweck, wichtige Parameter, Exceptions beziehungsweise Fehlerergebnisse und ein kurzes Beispiel, wenn die Nutzung nicht selbsterklärend ist.

## 10. Verbotene Muster

- Service Locator in Controls;
- direkte Datenbankzugriffe aus UI-Komponenten;
- unkontrollierte `Application.DoEvents()`-Schleifen;
- Thread.Sleep zur UI-Synchronisation;
- absolute Pfade oder maschinenspezifische Registryannahmen;
- versteckte Telemetrie;
- dünne Wrapper nur zur Umbenennung primitiver Controls;
- Mischung mehrerer sichtbarer Theme-Systeme in einer Anwendung.
