# ADR-0004 – Krypton als erster Pilot mit Native-WinForms-Fallback

- **Status:** To Validate
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Krypton bietet eine breite WinForms-Basis, muss aber Designer-, DPI-, Accessibility- und Wartungsanforderungen erfüllen.

## Entscheidung

Krypton wird in R0.2 als erste sichtbare Implementierung pilotiert. Native WinForms bleibt funktionaler Fallback hinter denselben Tokens und Services.

## Betrachtete Alternativen

- Krypton sofort verbindlich festlegen
- nur native WinForms
- AntdUI oder ReaLTaiizor als Standard

## Positive Folgen

- realistischer Suitenkandidat
- kein frühzeitiger Lock-in
- vergleichbare Referenz

## Negative Folgen und Risiken

- doppelte Pilotarbeit
- mögliche Unterschiede zwischen Fallback und Standard

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach Abschluss der R0.2-Matrix annehmen oder verwerfen.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
