# ADR-0005 – Keine flächendeckenden Wrapper für primitive Controls

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Dünne Wrapper wie `SasdButton` oder `SasdLabel` würden API, Designer und Wartung aufblasen, ohne genügend Mehrwert.

## Entscheidung

Primitive Controls bleiben nativ beziehungsweise thematisiert. Eigene Klassen entstehen für Composite Controls, Services und wiederkehrendes Verhalten.

## Betrachtete Alternativen

- jedes Control als SASD-Klasse
- keine eigenen Controls

## Positive Folgen

- kleinere API
- bessere Designerkompatibilität
- Fokus auf echten Mehrwert

## Negative Folgen und Risiken

- nicht jede visuelle Eigenschaft zentral erzwingbar

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Wenn mehrere Anwendungen dieselbe komplexe primitive Erweiterung benötigen.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
