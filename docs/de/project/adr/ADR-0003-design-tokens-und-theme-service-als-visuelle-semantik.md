# ADR-0003 – Design Tokens und Theme-Service als visuelle Semantik

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Visuelle Entscheidungen sollen nicht direkt an Krypton oder einzelne Controls gebunden sein.

## Entscheidung

Semantische Design Tokens definieren Farben, Typografie, Abstände und Zustände. `SasdThemeService` mappt sie auf konkrete WinForms-/Kryptonimplementierungen.

## Betrachtete Alternativen

- direkte Farbwerte pro Control
- nur Herstellerpaletten verwenden

## Positive Folgen

- konsistentes Erscheinungsbild
- spätere WPF/Web-Wiederverwendung der Semantik
- testbarer Themewechsel

## Negative Folgen und Risiken

- Mappingaufwand
- nicht jede Herstellerfunktion lässt sich vollständig abstrahieren

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Wenn Tokenmodell Designer oder Performance unvertretbar belastet.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
