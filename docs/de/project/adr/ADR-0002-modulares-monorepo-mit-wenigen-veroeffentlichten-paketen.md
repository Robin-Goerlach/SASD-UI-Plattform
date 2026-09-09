# ADR-0002 – Modulares Monorepo mit wenigen veröffentlichten Paketen

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Komponenten, Samples, Tests und Adapter müssen gemeinsam versionierbar bleiben, ohne Consumer mit einer großen Zahl direkter Pakete zu belasten.

## Entscheidung

Ein Monorepo enthält getrennte interne Projekte. Öffentlich werden wenige kohärente R1-Pakete und opt-in R2-Adapter angeboten.

## Betrachtete Alternativen

- ein großes Assembly
- viele unabhängige Repositories
- jedes Projekt als eigenes Paket veröffentlichen

## Positive Folgen

- konsistente Änderungen und CI
- klare interne Grenzen
- überschaubare Consumeroberfläche

## Negative Folgen und Risiken

- größeres Repository
- Releasepipeline muss Paketbeziehungen korrekt behandeln

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Wenn unabhängige Teams oder Releasezyklen nachweislich entstehen.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
