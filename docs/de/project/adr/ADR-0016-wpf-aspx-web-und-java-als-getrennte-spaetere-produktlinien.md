# ADR-0016 – WPF, ASPX/Web und Java als getrennte spätere Produktlinien

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Die Technologien besitzen unterschiedliche UI-, Tooling- und Architekturmodelle.

## Entscheidung

WPF, WinUI/Avalonia, modernes Web, ASP.NET Web Forms und Java/OpenXava werden als getrennte Projekte geplant. Wiederverwendet werden Semantik, UX-Regeln und Dokumentation, nicht automatisch Controlcode.

## Betrachtete Alternativen

- ein gemeinsames Mega-Repository mit allen Plattformen
- plattformspezifische Themen ignorieren

## Positive Folgen

- saubere technische Grenzen
- spätere gezielte Evaluation

## Negative Folgen und Risiken

- mehrere Repositories/Produktlinien
- gemeinsame Features müssen bewusst synchronisiert werden

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Bei Aktivierung einer Produktlinie durch eigenes Lasten-/Pflichtenheft ersetzen oder präzisieren.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
