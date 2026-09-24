# ADR-0013 – UI Automation, visuelle Regression und manuelle A11y-Prüfung

- **Status:** To Validate
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

WinForms-Qualität lässt sich nicht vollständig durch Unit-Tests belegen.

## Entscheidung

Eine Kombination aus UIA/FlaUI, Screenshotregression und manueller Accessibility-/Designerprüfung wird als R0-Pilot aufgebaut.

## Betrachtete Alternativen

- nur manuelle Tests
- nur Screenshottests
- vollständige End-to-End-Automation

## Positive Folgen

- breite Qualitätsabdeckung
- frühe Regressionserkennung

## Negative Folgen und Risiken

- Runner- und Flakinessaufwand

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach R0.3 anhand Stabilität, Laufzeit und Fehlernutzen bewerten.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
