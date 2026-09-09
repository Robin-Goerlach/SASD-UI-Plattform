# ADR-0015 – Keine Mobile-, macOS- oder Linux-Desktopimplementierung im aktuellen Scope

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Cross-Platform wäre nützlich, würde den aktuellen WinForms-Aufbau jedoch stark verbreitern.

## Entscheidung

Android, iOS, macOS und Linux Desktop erhalten aktuell keine Implementierungsprojekte oder Abhängigkeiten. Ideen bleiben im Zukunftsregister.

## Betrachtete Alternativen

- Avalonia/MAUI parallel
- plattformneutraler Kern von Beginn an

## Positive Folgen

- klarer Fokus
- geringerer Test- und Packagingaufwand

## Negative Folgen und Risiken

- spätere separate Renderingimplementierung nötig

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Bei konkretem Produkt-/Kundenbedarf und verfügbarer Kapazität.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
