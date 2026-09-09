# ADR-0006 – Composition Root statt Service Locator in Controls

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Controls benötigen Services, dürfen aber nicht versteckt globale Container auflösen.

## Entscheidung

Die Anwendung konfiguriert Services im Composition Root. Controls erhalten Abhängigkeiten über Controller, Properties, Factory oder klaren Host; Designerpfade bleiben parameterlos.

## Betrachtete Alternativen

- globaler Service Locator
- vollständige Constructor Injection in allen Controls

## Positive Folgen

- testbar
- explizite Abhängigkeiten
- Designer bleibt nutzbar

## Negative Folgen und Risiken

- zusätzliche Kompositionslogik
- zwei Pfade für Runtime und Designer nötig

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Wenn Designer-/Runtime-Komposition zu viel Boilerplate erzeugt.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
