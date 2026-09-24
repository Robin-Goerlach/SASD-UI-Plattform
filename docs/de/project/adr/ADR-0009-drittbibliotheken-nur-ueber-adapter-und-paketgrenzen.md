# ADR-0009 – Drittbibliotheken nur über Adapter- und Paketgrenzen

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Die Plattform soll Lieferanten nutzen können, ohne alle Anwendungen dauerhaft an deren APIs zu koppeln.

## Entscheidung

Fremdtypen bleiben aus Kern-APIs heraus. Herstellerbezogene Funktionen liegen in klar benannten Adapterpaketen.

## Betrachtete Alternativen

- direkte Referenz in allen Anwendungen
- Fork und Zusammenführung der Quelltexte

## Positive Folgen

- Austauschbarkeit
- kleinere Abhängigkeitsfläche
- klarer Lizenzscope

## Negative Folgen und Risiken

- Adapteraufwand
- nicht jede Spezialfunktion vollständig abstrahierbar

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Wenn eine Adapterabstraktion mehr Komplexität als Nutzen erzeugt; dann bleibt sie herstellerspezifisch im Adapter.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
