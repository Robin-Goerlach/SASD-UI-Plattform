# Governance und Entscheidungsprozess

## 1. Ziel

Governance verhindert, dass die Plattform durch spontane Komponentenwünsche, wechselnde Lieferanten oder unkontrollierte Abstraktionen wächst.

## 2. Entscheidungsarten

| Entscheidung | Verfahren |
| --- | --- |
| kleine Implementierungsfrage | Code Review |
| neue öffentliche API | API-Review und Changelog |
| neue Komponente | Komponentenproposal |
| neue Drittanbieterabhängigkeit | Dependency Review plus ADR |
| Paketgrenze/Architekturstil | ADR |
| Breaking Change | ADR, Migration und Major-/0.x-Entscheidung |
| Scopeänderung | Lasten-/Pflichtenheft und Roadmap aktualisieren |

## 3. Rollen

- Product Owner priorisiert Nutzen und Scope.
- Architect verantwortet Kohärenz und ADRs.
- Maintainer verantwortet Pakete, Releases und Upstream.
- Quality Owner verantwortet Gates.
- Consumer Representative prüft Nutzbarkeit in realer SASD-Anwendung.

Eine Person kann mehrere Rollen wahrnehmen, muss die Perspektiven im Review dennoch getrennt dokumentieren.

## 4. Decision Record

Eine ADR ist erforderlich, wenn eine Entscheidung:

- schwer rückgängig zu machen ist;
- mehrere Pakete oder Anwendungen betrifft;
- neue Fremdbibliothek einführt;
- Sicherheits-/Lizenzwirkung besitzt;
- öffentliche API oder Persistenzschema prägt;
- eine geplante Produktlinie abgrenzt.

## 5. Aufnahme neuer Komponenten

Der Entscheidungsfluss lautet:

1. Problem und Consumer belegen.
2. vorhandene Plattform-/Frameworkfunktion prüfen.
3. lokale Lösung versus wiederverwendbares Modul abwägen.
4. Build/Buy/Adapter/Fork vergleichen.
5. kleinsten Scope definieren.
6. Tests und Gallery-Zustände festlegen.
7. Release und Wartungsverantwortung zuordnen.
8. bei Freigabe Backlog und Dokumente aktualisieren.

## 6. Stop-the-Line

Entwicklung und Veröffentlichung werden gestoppt bei:

- ungeklärter Lizenzlage;
- kritischer Sicherheitslücke;
- nicht reproduzierbarem Releasebuild;
- Datenverlust-/State-Korruptionsrisiko;
- systematischem Designer- oder Handleproblem im Kern;
- ungeprüften Fremdtypen in Kern-APIs.

## 7. Reviewprotokoll

Gate-Reviews dokumentieren:

- erfüllte Nachweise;
- akzeptierte Abweichungen;
- offene Risiken;
- ADR-Status;
- Entscheidung Go/Conditional Go/No-Go;
- nächste konkrete Arbeitspakete.
