# Architecture Decision Records (ADR)

## Zweck

ADRs dokumentieren einzelne wesentliche Architekturentscheidungen. Sie ersetzen nicht das Architekturdokument, sondern liefern Kontext, Alternativen und Revisionskriterien.

## Statuswerte

- Proposed
- Accepted
- Rejected
- Superseded
- Deprecated
- Planned/To Validate

## Dateinamensschema

`ADR-0001-kurzer-titel.md`

Nummern werden nicht wiederverwendet. Eine spätere Entscheidung ersetzt eine alte ADR durch Verweis, löscht sie aber nicht.

## Pflichtinhalt

- Status und Datum;
- Kontext/Problem;
- Entscheidung;
- betrachtete Alternativen;
- positive und negative Folgen;
- Security-/Lizenzwirkung;
- Validierung beziehungsweise Revisionskriterium;
- Beziehungen zu Lastenheft, Pflichtenheft und Architektur.

## Register

| ADR | Titel | Status |
| --- | --- | --- |
| 0001 | WinForms und .NET 8 als R0/R1-Plattform | Accepted |
| 0002 | Modulares Monorepo und wenige öffentliche Pakete | Accepted |
| 0003 | Design Tokens und Theme-Service | Accepted |
| 0004 | Krypton als erster Pilot mit Native-Fallback | To Validate |
| 0005 | Keine flächendeckenden Primitive-Wrapper | Accepted |
| 0006 | Composition Root statt Service Locator | Accepted |
| 0007 | GridController getrennt vom Designer-Control | Accepted |
| 0008 | Versionierter JSON-UI-State | Accepted |
| 0009 | Drittbibliotheken nur über Adaptergrenzen | Accepted |
| 0010 | ScottPlot als primärer Chartpilot | Planned |
| 0011 | ScintillaNET als separater Codeeditoradapter | Planned |
| 0012 | WebView2 mit Default-Deny | Planned |
| 0013 | UIA, Visual Regression und manuelle A11y-Prüfung | To Validate |
| 0014 | Interner NuGet-Feed vor öffentlicher Veröffentlichung | Accepted |
| 0015 | Keine Mobile-/macOS-/Linux-Desktopimplementierung im aktuellen Scope | Accepted |
| 0016 | WPF, ASPX/Web und Java als getrennte Produktlinien | Accepted |
