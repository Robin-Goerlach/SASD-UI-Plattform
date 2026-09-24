# ADR-0012 – WebView2 mit Default-Deny-Sicherheitsprofil

- **Status:** Planned
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

HTML-/Markdownvorschau und eingebettete Webinhalte sind nützlich, schaffen aber eine bedeutende Trust Boundary.

## Entscheidung

`SasdWebViewHost` blockiert Navigation, Downloads und Skriptbrücken standardmäßig. Origins und Funktionen werden explizit erlaubt.

## Betrachtete Alternativen

- ungehärtetes WebView2-Control
- externer Browser
- kein Webinhalt

## Positive Folgen

- kontrollierte Integration
- klare Sicherheitsgrenze

## Negative Folgen und Risiken

- Runtimeabhängigkeit
- komplexer Lifecycle und Securitypflege

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Vor R2-Freigabe durch Security- und Lifecycle-Test.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
