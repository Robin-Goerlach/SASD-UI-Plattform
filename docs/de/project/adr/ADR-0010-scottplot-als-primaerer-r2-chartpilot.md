# ADR-0010 – ScottPlot als primärer R2-Chartpilot

- **Status:** Planned
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Technische Diagramme werden in Dashboards und Monitoringansichten erwartet, sollen aber kein R1-Kerngewicht erzeugen.

## Entscheidung

ScottPlot wird als erster R2-Adapter für technische Linien-, Balken- und Punktdiagramme untersucht.

## Betrachtete Alternativen

- LiveCharts2 als primär
- eigene Chartengine
- kommerzielles Chartpaket

## Positive Folgen

- guter technischer Fokus
- separates opt-in Paket

## Negative Folgen und Risiken

- visuelle Dashboardanforderungen können anderen Adapter benötigen

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach konkretem Dashboard-Szenario und Performance-/Accessibility-Pilot.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
