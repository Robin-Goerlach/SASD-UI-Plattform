# Risiko- und Qualitätsregister

## 1. Bewertung

- Eintritt: niedrig, mittel, hoch.
- Auswirkung: niedrig, mittel, hoch, kritisch.
- Status: offen, überwacht, mitigiert, akzeptiert, geschlossen.

## 2. Initiales Risikoregister

| ID | Risiko | Eintritt | Auswirkung | Gegenmaßnahme | Stop-Kriterium |
| --- | --- | --- | --- | --- | --- |
| R-001 | Krypton-Designer instabil | mittel | hoch | R0-Designermatrix, native Fallbacks | reproduzierbarer Datenverlust im Designer |
| R-002 | zu viele veröffentlichte Pakete | mittel | mittel | interne Trennung, wenige Consumerpakete | Basisapp benötigt >8 direkte SASD-Pakete |
| R-003 | Plattform wird Enterprise-Grid-Nachbau | hoch | kritisch | fester Grid-Scope, Build-vs-Buy | Pivot/Formeleditor/Designer wird Kernziel |
| R-004 | UI-Automation flakig | mittel | mittel | AutomationIds, Waits, Quarantänefrist | >5 % Fehlerrate ohne Produktfehler |
| R-005 | Handle-/Memory-Leak | mittel | hoch | Zyklus- und Lifecycle-Tests | fortlaufendes Wachstum bei 100 Zyklen |
| R-006 | Lizenzänderung | niedrig/mittel | hoch | SBOM, Adapter, Updateprüfung | Weitergaberecht unklar |
| R-007 | Fremdtypen leaken in Kern-API | mittel | hoch | Architekturtests | Consumer muss Krypton kennen |
| R-008 | Scope wächst zu schnell | hoch | hoch | Roadmap-Gates, Proposalprozess | R2 blockiert R1 |
| R-009 | State-Korruption | niedrig/mittel | hoch | atomar, Backup, Migrationstests | Start oder Nutzerdaten gefährdet |
| R-010 | Accessibility bleibt nachträglich | mittel | hoch | Galleryzustände und DoD | Kerncontrol ohne Tastatur/A11y |
| R-011 | Windows/.NET-Update bricht Verhalten | mittel | mittel/hoch | Kompatibilitätsmatrix, Wartungsreviews | keine tragfähige Runtimekombination |
| R-012 | Ein-Personen-Wissensrisiko | hoch | mittel/hoch | ausführliche Doku, ADR, Samples | kritische Funktion nur implizit bekannt |

## 3. Qualitätskennzahlen

- Build aus sauberem Checkout erfolgreich;
- null kritische Lizenz-/Securitybefunde;
- null unbegründete Compilerwarnungen;
- Public-API-Dokumentationsquote 100 Prozent für veröffentlichte Typen;
- Gallery-Abdeckung 100 Prozent visueller öffentlicher Komponenten;
- Ressourcenzyklus ohne fortlaufendes Handlewachstum;
- Tastatur- und DPI-Matrix für R1 bestanden;
- Migrationstests für jede State-Schemaversion;
- Consumer-Smoke-Test aus veröffentlichten NuGet-Paketen.

## 4. Qualitätsabweichungen

Akzeptierte Abweichungen benötigen:

- genaue betroffene Version/Komponente;
- Nutzerwirkung;
- Workaround;
- Risikoentscheidung;
- Besitzer und Zielrelease;
- kein stilles Verschieben ohne erneutes Review.
