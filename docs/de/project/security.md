# Sicherheitsrichtlinie

## 1. Geltungsbereich

Diese Richtlinie betrifft die SASD UI Platform, ihre NuGet-Pakete, Beispiele, Buildskripte, Dokumentation und Drittanbieteradapter. Fachanwendungen bleiben für Authentifizierung, Autorisierung, Geheimnisse, Datenbankrechte und Netzwerksicherheit selbst verantwortlich.

## 2. Sicherheitsziele

- keine Geheimnisse in UI-State, Logs, Screenshots oder Beispielprojekten;
- sichere Defaults für Datei-, Shell-, Clipboard-, Drag-and-drop- und WebView-Funktionen;
- nachvollziehbare Lieferkette mit Lockfiles, SBOM und Third-Party-Notices;
- minimale Adapterrechte und klare Trust Boundaries;
- fehlerhafte oder manipulierte UI-State-Dateien dürfen keinen unsicheren Startzustand erzwingen;
- Fehleranzeigen trennen benutzerfreundliche Meldung und technische Details.

## 3. Meldung von Schwachstellen

Sicherheitsprobleme werden nicht zunächst als öffentliches Issue veröffentlicht. Bis ein dedizierter Security-Kontakt eingerichtet ist, werden sie intern an die Projektverantwortung der SASD-GmbH gemeldet. Eine Meldung soll enthalten:

- betroffene Version und Paket;
- reproduzierbare Schritte;
- erwartete und tatsächliche Wirkung;
- mögliche Ausnutzung und Schadensfolge;
- vorhandene Gegenmaßnahmen oder Workarounds.

## 4. Reaktionsklassen

| Klasse | Beispiel | Reaktion |
| --- | --- | --- |
| Kritisch | Remote Code Execution, Secret-Leak, beliebige Shell-Ausführung | sofortige Analyse, Veröffentlichung stoppen, Hotfix vorbereiten |
| Hoch | Pfadvalidierung umgehbar, unsichere WebView-Brücke | priorisierter Fix vor nächstem regulären Release |
| Mittel | sensible technische Details im Log, unzureichende Allowlist | Fix im nächsten Wartungsrelease |
| Niedrig | Härtungs- oder Dokumentationslücke ohne direkte Ausnutzung | geplanter Backlogeintrag |

## 5. Sichere Komponentenregeln

### Shell und Prozesse

- keine ungeprüfte Übergabe benutzerkontrollierter Texte an Shell oder Kommandointerpreter;
- bevorzugt direkte Prozessargumente statt zusammengesetzter Befehlszeilen;
- explizite Allowlist für Dateitypen, Protokolle und ausführbare Ziele;
- verständliche Bestätigung vor riskanten externen Aktionen.

### WebView2

- Default-Deny für Navigation und Ressourcen;
- Origin-Allowlist;
- Downloads standardmäßig blockieren oder kontrolliert behandeln;
- keine generische JavaScript-.NET-Brücke;
- lokale Inhalte mit restriktiver Content-Security-Policy soweit möglich;
- Entwicklerwerkzeuge in Produktionskonfiguration deaktivieren, sofern nicht ausdrücklich benötigt.

### Dateien und Drag-and-drop

- Pfade normalisieren und gegen erlaubte Bereiche/Dateitypen prüfen;
- Größen- und Anzahlbegrenzungen vor dem Laden anwenden;
- keine automatische Ausführung oder aktive Inhaltsinterpretation;
- symbolische Links, Netzwerkpfade und unerwartete Dateiendungen bewusst behandeln.

### UI-State

- keine Passwörter, Tokens, vollständigen E-Mail-Inhalte oder andere Geheimnisse speichern;
- SchemaVersion und sichere Migration;
- atomarer Schreibvorgang und Backup;
- ungültige Daten führen zu Reset oder ignoriertem Teilzustand, nicht zu unsicherem Fallback.

## 6. Dependency Security

- zentrale Paketversionen;
- Restore-Lock;
- Vulnerability- und Lizenzscan in CI;
- keine unklar lizenzierten Binärdownloads;
- reproduzierbare Releasebuilds aus sauberem Checkout;
- SBOM pro Release;
- Signierung beziehungsweise Provenance nach technischer Reife bewerten.

## 7. Security-Abnahme für Adapter

Jedes R2-/R3-Adapterpaket benötigt eine eigene Prüfung zu:

- Eingabe- und Dateigrenzen;
- Prozess-/Netzwerkzugriff;
- nativen Komponenten und Lebenszyklus;
- Logging sensibler Inhalte;
- Update- und Exit-Strategie;
- bekannten Schwachstellen und Supportstatus.
