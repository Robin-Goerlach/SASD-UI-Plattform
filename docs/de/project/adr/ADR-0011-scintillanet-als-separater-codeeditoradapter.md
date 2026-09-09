# ADR-0011 – ScintillaNET als separater Codeeditoradapter

- **Status:** Planned
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Konfigurations- und Codeansichten benötigen Syntax, Suche und große Dateien; diese Komplexität gehört nicht in den Kern.

## Entscheidung

ScintillaNET wird ausschließlich über ein separates Editoradapterpaket angeboten.

## Betrachtete Alternativen

- RichTextBox erweitern
- Webeditor via WebView2
- eigener Texteditor

## Positive Folgen

- leistungsfähiger Spezialeditor
- Kern bleibt leicht

## Negative Folgen und Risiken

- native Ressourcen/Lifecycle
- herstellerspezifische APIs

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach R2-Lifecycle-, Lizenz- und Großdateitest.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
