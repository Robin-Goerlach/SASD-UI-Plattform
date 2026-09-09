# Codex-Entwicklungsworkflow

## 1. Zweck

Dieses Dokument beschreibt, wie Codex die SASD UI Platform weiterentwickeln soll, ohne Architekturgrenzen, Qualitätsgates oder den Entscheidungsprozess aufzuweichen.

Die kurze [`AGENTS.md`](../../../AGENTS.md) im Repository-Root dient als dauerhafte Agenten-Landkarte. Das eigentliche Projektwissen bleibt in der normalen Repository-Dokumentation, insbesondere in Architektur, Pflichtenheft, Roadmap sowie Entwicklungs- und Testleitfäden.

## 2. Repository-Einrichtung für Codex

Zugangsdaten, API-Schlüssel oder rechnerbezogene Codex-Anmeldekonfiguration gehören nicht in dieses Repository.

Für Repository-Anweisungen verwendet Codex [`AGENTS.md`](../../../AGENTS.md). Das Repository sollte als Arbeitsverzeichnis am Projekt-Root geöffnet werden, damit die Root-Anweisungen und die Projektdokumentation verfügbar sind.

Das Repository enthält bewusst keine benutzerspezifische `config.toml`. Sandbox-Modus, Login-Methode, Modellwahl, beschreibbare Verzeichnisse und MCP-Zugangsdaten sind Arbeitsplatz- bzw. Workspace-Konfiguration und werden außerhalb des Repositorys verwaltet.

## 3. Empfohlene Aufgabenbeschreibung

Codex arbeitet besonders gut mit Aufgaben, die wie ein fokussiertes GitHub-Issue formuliert sind: Ziel, relevante Dateien/Komponenten, Grenzen und Abnahmekriterien sollen klar sein.

Empfohlene Vorlage:

```text
Titel: <kurzer Aufgabentitel>

Ziel:
<was nach der Änderung funktionieren soll>

Relevanter Scope:
- <Komponente/Projekt/Datei>
- <bestehendes Muster, dem gefolgt werden soll>

Nicht im Scope:
- <was nicht neu entworfen werden soll>

Abnahmekriterien:
- <beobachtbares Verhalten>
- <wichtiger Randfall>
- öffentliche API/XML-Kommentare aktualisiert
- relevante Smoke-Tests ergänzt/angepasst
- pwsh ./build/verify.ps1 läuft unter Windows erfolgreich

Entscheidungsgrenze:
Wenn eine neue Runtime-Abhängigkeit, Änderung von Paketgrenzen, ein Breaking Change,
ein Plattformwechsel oder eine andere strategische Entscheidung nötig wird, stoppen
und Alternativen erklären, statt stillschweigend zu entscheiden.
```

## 4. Aufgaben für autonome Codex-Arbeit

Gut geeignet sind Aufgaben, die bereits durch vorhandene Entscheidungen begrenzt sind, zum Beispiel:

- eine im Pflichtenheft oder in der Roadmap bereits vorgesehene Komponente umsetzen;
- eine bestehende SASD-Komponente nach etabliertem Modulmuster erweitern;
- Tests für bekannte Randfälle ergänzen;
- Analyzer-, Nullability-, Dispose-, Threading- oder State-Recovery-Fehler beheben;
- die Component Gallery für vorhandene Komponenten erweitern;
- Kommentare/XML-Dokumentation verbessern, ohne Verhalten zu ändern;
- kleine Lesbarkeits-Refactorings innerhalb eines Moduls;
- bereits freigegebene dependency-freie R1/R2-Helfer implementieren.

## 5. Aufgaben, die eine strategische Entscheidung benötigen

Codex muss stoppen und Alternativen beschreiben, bevor Änderungen umgesetzt werden, die:

- eine neue Runtime-Drittanbieterbibliothek hinzufügen;
- einen UI-Anbieter oder ein sichtbares Designsystem ersetzen oder zum Standard erklären;
- die endgültige Native-vs-Krypton-Entscheidung treffen;
- eine weitere Zielplattform einführen;
- Datenbank-/ORM-Verantwortung in die UI Platform verlagern;
- große Projekt-/Paketgrenzen verändern;
- eine öffentliche API brechen, obwohl eine kompatible Lösung realistisch wäre;
- Telemetrie, Netzwerkdienste, Hintergrund-Daemons oder verstecktes globales Verhalten hinzufügen;
- neue erhebliche Lizenzpflichten erzeugen;
- eine nicht freigegebene R3-/Enterprise-Komponente hinzufügen.

Die Eskalation soll enthalten:

1. notwendige Entscheidung;
2. möglichst mindestens zwei realistische Optionen;
3. Auswirkungen auf Architektur, Wartung, Sicherheit und Lizenzierung;
4. empfohlene Standardoption;
5. was vor der Entscheidung gefahrlos weiterentwickelt werden kann.

## 6. Arbeitsfolge für Codex

Für eine normale Coding-Aufgabe:

1. Root-`AGENTS.md` lesen.
2. `ROADMAP.md` und das relevante Modul-`README.md` lesen.
3. passende Architektur-/Pflichtenheftabschnitte lesen.
4. im Repository nach dem ähnlichsten bestehenden Muster suchen.
5. die kleinste zusammenhängende Implementierung erstellen, die die Anforderung erfüllt.
6. entscheidungsorientierte Kommentare und öffentliche XML-Dokumentation ergänzen.
7. Tests ergänzen oder aktualisieren.
8. während der Entwicklung fokussierte Checks ausführen.
9. vor Abschluss unter Windows die breite Repository-Verifikation ausführen.
10. Diff auf fachfremde Änderungen, Abhängigkeitslecks und unbeabsichtigte API-Änderungen prüfen.
11. abgeschlossene Arbeit committen, sofern die Aufgaben-Umgebung Commits erlaubt.

## 7. Verifikationsbefehl

Der kanonische lokale/Agenten-Befehl lautet:

```powershell
pwsh ./build/verify.ps1
```

Unter Windows führt er dasselbe automatisierte Gate wie GitHub Actions aus: Restore, strengen Release-Build, Architekturprüfungen sowie Core-, State-, WinForms-, Windows-, Shell-, native-R2-, Data/Dashboard- und Krypton-Smoke-Tests.

Für eine nicht-Windows-Codex-Umgebung:

```powershell
pwsh ./build/verify.ps1 -CompileOnly
```

Das ist bewusst nicht gleichwertig zum Windows-Gate. Der Agent muss ausdrücklich nennen, dass die WinForms-Runtime-Tests nicht ausgeführt wurden und GitHub Actions die finale Windows-Prüfung übernimmt.

## 8. Anforderungen an generierten Code

Für Codex-Code gelten dieselben Regeln wie für menschlich geschriebenen Code:

- einfach und lesbar vor optimiert;
- keine spekulativen Abstraktionen;
- kein versteckter Service Locator;
- kein Datenbankzugriff aus UI-Controls;
- keine stillen neuen Abhängigkeiten;
- kein `Thread.Sleep`/`Application.DoEvents()` zur Synchronisation;
- sinnvolle XML-Dokumentation für öffentliche API;
- erklärende Kommentare für komplexes oder überraschendes WinForms-Verhalten;
- klare Ressourcen-Ownership und Event-Abmeldung;
- sicherheitsrelevante Pfade standardmäßig ablehnen oder kontrolliert fehlschlagen lassen;
- Accessibility darf nicht allein von Farbe abhängen.

## 9. Git-/PR-Verhalten

Die Aufgaben-Umgebung kann Branch-Erstellung je nach lokaler CLI, Desktop-App oder verwalteter Codex-Aufgabe unterschiedlich steuern.

Unabhängig davon gilt:

- veröffentlichte Historie nicht überschreiben oder force-pushen;
- keine fremden Commits verändern;
- zusammenhängende Commits erzeugen;
- PRs nur mergen, wenn die Aufgabe das ausdrücklich erlaubt;
- CI-/Analyzer-Regeln nicht abschwächen, nur damit ein Build grün wird;
- wenn ein Test einen echten Produktfehler entdeckt, das Produkt korrigieren und nicht den Test passend machen.

Sinnvolle Commit-Präfixe sind `R1:`, `R2:`, `Fix:`, `Test:`, `Docs:` und `CI:`.

## 10. Größere Arbeiten aufteilen

Größere Features sollten in überprüfbare Schnitte zerlegt werden:

1. Verträge/Datenmodell;
2. kleinste brauchbare Implementierung;
3. Verhaltenstests;
4. Gallery-/Sample-Integration;
5. Dokumentation/Roadmap aktualisieren;
6. Hardening nach realer Nutzung.

Das entspricht dem bisherigen Entwicklungsstil des Repositorys und reduziert die Gefahr, dass ein Agent fehlende Entscheidungen durch selbst erfundene Architektur ersetzt.

## 11. Pflege der Agenten-Anweisungen

`AGENTS.md` soll kurz genug bleiben, um eine Landkarte zu sein. Benötigt eine Regel ausführliche Erläuterungen, gehört diese Erläuterung in die normale Projektdokumentation und wird aus `AGENTS.md` verlinkt.

Die Agenten-Landkarte wird nur aktualisiert, wenn sich dauerhafte Architekturregeln, Verifikationsbefehle oder Eskalationsgrenzen ändern. Temporäre Aufgabendetails gehören nicht hinein.
