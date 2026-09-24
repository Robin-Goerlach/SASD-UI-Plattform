# Dokumentenlandschaft und Dokumentenlenkung

## 1. Ziel

Die Dokumentenlandschaft verhindert widersprüchliche Einzeldateien und unnötige Doppelpflege. Jedes Dokument besitzt eine klar definierte Rolle.

## 2. Normative Hierarchie

1. **Lastenheft:** Was wird benötigt und warum?
2. **Pflichtenheft:** Wie soll der Bedarf technisch umgesetzt werden?
3. **Architekturdokument:** Welche strukturellen Entscheidungen und Grenzen gelten?
4. **ADRs:** Warum wurde eine konkrete Architekturentscheidung getroffen?
5. **Roadmap/Projektplan:** In welcher Reihenfolge wird geliefert?
6. **Entwicklungs-, Test-, Build- und Releaseleitfäden:** Wie wird täglich gearbeitet und geprüft?
7. **Component Gallery und Beispiele:** Ausführbarer Nachweis des tatsächlichen Verhaltens.

Bei Widersprüchen gilt das höher eingeordnete normative Dokument. Ein erkannter Widerspruch wird nicht stillschweigend interpretiert, sondern durch eine dokumentierte Änderung bereinigt.

## 3. Dokumenttypen

| Typ | Inhalt | Änderungsanlass |
| --- | --- | --- |
| Anforderungsdokument | Ziele, Scope, Muss/Soll/Kann | geänderter Produktbedarf |
| Pflichten-/Architekturdokument | technische Lösung und Grenzen | Architektur- oder Scopeentscheidung |
| ADR | einzelne irreversible oder teure Entscheidung | neue Technologie, Paketgrenze, Sicherheitsmodell |
| Roadmap | Reihenfolge und Gates | Gate-Review, Kapazitäts- oder Prioritätsänderung |
| Richtlinie | tägliche Entwicklungsregeln | wiederkehrende Qualitätsabweichung |
| Runbook/Support | Diagnose und Wartung | neuer Betriebsfall oder Störung |
| Referenz | Komponenten-API, Beispiele | neue/geänderte öffentliche Funktion |

## 4. Pflegeverantwortung

In der aktuellen Ein-Personen-/Kleinteamsituation kann eine Person mehrere Rollen ausüben; die Rollen bleiben dennoch getrennt gedacht:

- **Product Owner:** Scope und Priorität;
- **Architect:** Paketgrenzen, ADRs und technische Kohärenz;
- **Maintainer:** Releases, Abhängigkeiten und Support;
- **Quality Owner:** Tests, Accessibility, DPI und Freigabegates;
- **Documentation Owner:** Verständlichkeit, Links und Änderungsstand.

## 5. Metadatenstandard

Jedes größere Dokument enthält:

- Titel;
- Produktlinie;
- Version/Stand;
- Status;
- Geltungsbereich;
- Änderungsprotokoll oder Verweis auf `CHANGELOG.md`;
- Verweise auf über- und untergeordnete Dokumente.

## 6. Ablage

```text
/
├─ README.md
├─ ROADMAP.md
├─ CHANGELOG.md
├─ CONTRIBUTING.md
├─ SECURITY.md
├─ docs/
│  ├─ 00_Dokumentenlandschaft.md
│  ├─ ... Fach- und Prozessdokumente
│  └─ adr/
└─ .github/
```

## 7. Reviewrhythmus

- Gate-Review: Roadmap, Risiken, ADR-Status und offene Entscheidungen.
- Release-Review: Changelog, Security, SBOM, Migration, bekannte Einschränkungen.
- Quartalsweise oder bei bedeutenden Updates: Zukunftsregister und Abhängigkeiten.
- Sofort: Sicherheitsrelevante oder lizenzkritische Änderungen.
