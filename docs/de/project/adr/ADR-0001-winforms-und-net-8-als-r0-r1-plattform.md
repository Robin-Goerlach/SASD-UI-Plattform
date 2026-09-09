# ADR-0001 – WinForms und .NET 8 als R0/R1-Plattform

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Die bestehenden SASD-Desktopanwendungen und aktuelle Entwicklungspraxis basieren auf C# und WinForms. Ein paralleler Start mehrerer UI-Technologien würde Kapazität und Qualität verteilen.

## Entscheidung

R0 und R1 verwenden Windows Forms mit `net8.0-windows`. Windows 10/11 und Visual Studio Designer sind Referenz. Eine aktuelle LTS wird in CI geprüft, ohne unnötiges Multi-Targeting.

## Betrachtete Alternativen

- WPF sofort als Hauptplattform
- WinUI 3 als Greenfield-Basis
- Avalonia für Cross-Platform

## Positive Folgen

- schnelle Wiederverwendung in bestehenden Anwendungen
- bekannter Designer und Betriebsrahmen
- klarer Fokus

## Negative Folgen und Risiken

- Windowsbindung
- spätere Portierung erfordert eigene Renderingimplementierung

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Nach R1.1 oder bei Ende des .NET-8-Supports erneut bewerten.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
