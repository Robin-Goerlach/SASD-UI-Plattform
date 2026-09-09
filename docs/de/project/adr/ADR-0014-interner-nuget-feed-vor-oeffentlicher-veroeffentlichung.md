# ADR-0014 – Interner NuGet-Feed vor öffentlicher Veröffentlichung

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Die API muss zunächst in realen SASD-Anwendungen stabilisiert werden, bevor öffentlicher Support erwartet wird.

## Entscheidung

R0/R1 werden über internen/private Feed und Releasearchive verteilt. Öffentliche Open-Source-Freigabe erfolgt erst nach R1-Stabilisierung, Lizenz- und Governanceprüfung.

## Betrachtete Alternativen

- sofort GitHub/NuGet öffentlich
- DLL-Kopien
- Quellcode direkt referenzieren

## Positive Folgen

- kontrollierte Lernphase
- saubere Paketnutzung ohne öffentliche Zusagen

## Negative Folgen und Risiken

- weniger externes Feedback
- Feedbetrieb nötig

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach R1.1 und drei realen Consumer-Anwendungen.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
