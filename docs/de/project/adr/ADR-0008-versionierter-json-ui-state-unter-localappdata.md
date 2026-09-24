# ADR-0008 – Versionierter JSON-UI-State unter LocalAppData

- **Status:** Accepted
- **Datum:** 2026-07-23
- **Produktlinie:** SASD UI Platform – C# WinForms Components

## Kontext

Fenster-, Grid- und Layoutzustände benötigen lokale, robuste Persistenz, ohne eine Datenbankabhängigkeit einzuführen.

## Entscheidung

UI-State wird als versioniertes UTF-8-JSON unter `%LocalAppData%\SASD-GmbH\<Produkt>` gespeichert, atomar geschrieben, gesichert und migriert. Secrets sind verboten.

## Betrachtete Alternativen

- Registry
- SQLite im Kern
- Application Settings ohne Schema-/Recoverykonzept

## Positive Folgen

- transparent und portabel
- einfach testbar
- keine DB-Abhängigkeit

## Negative Folgen und Risiken

- Dateikorruption muss behandelt werden
- gleichzeitige Prozesse benötigen klare Strategie

## Security, Datenschutz und Lizenz

Die Entscheidung wird im Dependency-, Security- und Lizenzprozess geprüft, soweit Fremdkomponenten, Persistenz, Dateien, Prozesse oder Netzwerkgrenzen betroffen sind. Sie darf keine Geheimnisse in UI-State oder Logs einführen und keine ungeklärten Weitergaberechte erzeugen.

## Validierung

Die Entscheidung wird durch den jeweils zugeordneten R0-/R1-/R2-Piloten, Architekturtests, Gallery-Seiten, Consumer-Samples und gegebenenfalls reale SASD-Anwendungen nachgewiesen.

## Revisionskriterium

Bei nachgewiesenem Mehrprozess-/Roamingbedarf.

## Beziehungen

- Lastenheft: betroffene Muss-/Scope-Anforderungen.
- Pflichtenheft: technische Umsetzung und Releasezuordnung.
- Architekturdokument: ADR-Register und zugehörige Architekturkapitel.
