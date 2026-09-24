# Beitragsleitfaden

## 1. Grundsatz

Beiträge zur SASD UI Platform müssen die Plattform kleiner, verlässlicher oder besser nutzbar machen. Eine hohe Anzahl neuer Controls ist kein Qualitätsmerkmal. Jede Änderung soll mindestens ein reales Anwendungsszenario adressieren.

## 2. Vor Beginn einer Änderung

Für kleine Fehlerkorrekturen genügt ein Issue. Für neue Komponenten, neue Drittanbieterpakete, neue öffentliche APIs oder geänderte Paketgrenzen ist zusätzlich eine kurze Designnotiz beziehungsweise ADR erforderlich.

Ein Komponentenproposal beantwortet:

- Welche SASD-Anwendungen benötigen die Funktion?
- Warum reichen vorhandene WinForms-/SASD-Komponenten nicht?
- Ist die Lösung Control, Composite Control, Service, Controller, Extension oder Adapter?
- Welche Accessibility-, DPI-, Designer- und Lifecycle-Risiken bestehen?
- Welche Drittanbieter- und Lizenzfolgen entstehen?
- Was ist ausdrücklich nicht Bestandteil der ersten Version?

## 3. Branch- und Pull-Request-Modell

- `main` bleibt build- und releasefähig.
- Änderungen erfolgen in kurzen Feature- oder Fix-Branches.
- Pull Requests bleiben thematisch fokussiert.
- Ein PR mischt keine großflächige Formatierung mit funktionalen Änderungen.
- Breaking Changes benötigen ADR, Changelog und Migrationshinweis.

## 4. Definition of Ready

Eine Aufgabe ist bereit zur Umsetzung, wenn:

- Ziel und Nichtziel beschrieben sind;
- Releasezuordnung und betroffene Pakete feststehen;
- Akzeptanzkriterien prüfbar formuliert sind;
- Design- und Fremdbibliotheksrisiken bewertet wurden;
- benötigte Teststufen feststehen.

## 5. Definition of Done

- Code kompiliert ohne neue Warnungen.
- Nullable-Unterdrückungen sind begründet.
- Öffentliche APIs besitzen XML-Dokumentation.
- Unit-/Integrations-/Architekturtests sind ergänzt.
- Visuelle Komponenten besitzen eine Gallery-Seite.
- Tastatur, DPI, Theme, High Contrast, Lokalisierung und Accessibility wurden geprüft.
- Ressourcen und Eventabonnements werden sauber freigegeben.
- Changelog, Migrationshinweise und bekannte Einschränkungen sind aktualisiert.
- Lizenzinventar und SBOM enthalten neue Abhängigkeiten.

## 6. Code-Review-Schwerpunkte

1. **Scope:** Ist die Funktion wirklich plattformweit wiederverwendbar?
2. **API:** Bleibt sie verständlich und frei von unbeabsichtigten Fremdtypen?
3. **Designer:** Kann das Control im Visual Studio Designer erstellt und gespeichert werden?
4. **Lifecycle:** Werden Events, Timer, Bitmaps, Handles und native Ressourcen freigegeben?
5. **Async:** Wird der UI-Thread korrekt behandelt, mit Cancellation und Fehlerfluss?
6. **UX:** Sind Fokus, Tastatur, Fehler-, Busy- und Empty-Zustände konsistent?
7. **Tests:** Belegen Tests die kritischen Eigenschaften statt nur Implementierungsdetails?

## 7. Abhängigkeiten

Neue NuGet-Pakete benötigen vor dem Merge:

- Lizenzidentifikation;
- Aktivitäts- und Wartungsbewertung;
- Prüfung transitiver Abhängigkeiten;
- Security-/Vulnerability-Scan;
- Exit-Strategie;
- Begründung, warum Eigenentwicklung oder bestehende Pakete nicht genügen.

## 8. Dokumentation

Markdown-Dateien verwenden klare Überschriften, kurze Absätze, Tabellen nur bei echtem Vergleichsnutzen und relative Links. Öffentliche Codebeispiele müssen kompilierbar oder ausdrücklich als Pseudocode gekennzeichnet sein.
