# ADR-0007 – SasdGridController<T> getrennt vom designerfähigen Grid

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Generische WinForms-Controls verursachen Designerprobleme, typisierte Gridkonfiguration bleibt dennoch wünschenswert.

## Entscheidung

`SasdDataGrid` ist nicht generisch und designerfähig. `SasdGridController<T>` kapselt typisierte Spalten, Mapping, Auswahl, Commands und Validierung.

## Betrachtete Alternativen

- generisches Grid-Control
- nur untypisierte DataGridView-Events
- vollständiger Enterprise-Grid-Eigenbau

## Positive Folgen

- Designerfähigkeit
- typisierte Konfiguration
- begrenzter Scope

## Negative Folgen und Risiken

- zusätzliche Koordinationsschicht
- Lifecycle muss sauber gebunden werden

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach Grid-Spike und erster Consumer-Migration prüfen.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
