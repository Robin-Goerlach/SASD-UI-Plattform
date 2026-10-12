# Abhängigkeiten, Lizenzen und SBOM

## 1. Ziel

Drittanbieterkomponenten werden bewusst genutzt, aber nicht unkontrolliert in einen Megafork übernommen. Jede Abhängigkeit muss technisch, rechtlich und betrieblich tragbar sein.

## 2. Bewertungsfelder

| Feld | Leitfrage |
| --- | --- |
| Funktion | löst das Paket einen echten, wiederkehrenden Bedarf? |
| Lizenz | darf es intern, kommerziell und gegebenenfalls Open Source weitergegeben werden? |
| Aktivität | gibt es aktuelle Releases, Issues und Maintainer? |
| Qualität | Tests, Dokumentation, API-Stabilität, Designer/DPI? |
| Security | bekannte Schwachstellen, native Bestandteile, Updatefähigkeit? |
| API-Leak | zwingt es Consumer auf Fremdtypen? |
| Exit | kann es ersetzt, isoliert oder notfalls geforkt werden? |
| Größe | welche transitiven Pakete und Runtimekosten entstehen? |

## 3. Klassifikation

- **Kernabhängigkeit:** für R1 erforderlich, besonders streng bewertet.
- **Adapterabhängigkeit:** nur in optionalem Paket.
- **Build-/Testabhängigkeit:** nicht Teil der Consumer-Runtime.
- **Beispielabhängigkeit:** ausschließlich Sample/Gallery.
- **abgelehnt/zurückgestellt:** dokumentiert mit Grund.

## 4. Lizenzregeln

- permissive Lizenzen wie MIT/BSD/Apache sind bevorzugt, aber nicht automatisch risikofrei;
- Copyleft oder unklare Sonderbedingungen benötigen ausdrückliche Prüfung;
- Community-/Gratislizenzen werden nicht mit Open Source verwechselt;
- Markennamen und Logos werden nicht als eigene SASD-Produkte ausgegeben;
- Copyright- und Lizenztexte bleiben erhalten;
- transitive Abhängigkeiten werden mitbewertet.

## 5. SBOM und Notices

Jedes Release erzeugt:

- eine maschinenlesbare standardkonforme SBOM;
- `THIRD-PARTY-NOTICES` mit Paket, Version, Lizenz und Quelle;
- einen Vulnerability-Scanbericht für direkte und transitive NuGet-Abhängigkeiten;
- Prüfsummen der Releaseartefakte.

### Aktuelle automatisierte R1-Evidence

Der Release-Dry-Run erzeugt bereits zwei ergänzende NuGet-Evidence-Dateien:

- `nuget-dependencies.json` erfasst den aufgelösten direkten/transitiven Paketgraphen und prüft zentrale Versionierung direkter Pakete sowie Notice-Abdeckung;
- `nuget-vulnerabilities.json` erfasst den zeitgestempelten Befund von `dotnet list package --vulnerable --include-transitive --format json` für jedes explizite Produktprojekt.

Zusätzlich aktiviert der Restore `NuGetAuditMode=all`, damit auch normale NuGet-Audits transitive Schwachstellen berücksichtigen. Das explizite Vulnerability-Gate schlägt bei jedem bekannten Fund fehl und ebenfalls dann, wenn die Audit-Quelle nicht verfügbar ist. Fehlende Sicherheitsdaten dürfen nicht als sauberer Scan interpretiert werden.

Diese Dateien sind **Release-Evidence, aber noch keine endgültige SBOM**. Das Dependency-Inventar beansprucht keine SPDX-/CycloneDX-Konformität; der Vulnerability-Bericht ist zudem zeitabhängig, weil sich Advisory-Daten nach einem Build ändern können.

## 6. Fork-Regel

Ein Fork ist nur zulässig, wenn:

1. Upstream nicht ausreichend reagiert oder nicht mehr gepflegt wird;
2. Erweiterung/Adapter/Pull Request nicht genügt;
3. Lizenz dies erlaubt;
4. Tests die übernommene Fläche abdecken;
5. Maintainer und Wartungsbudget benannt sind;
6. Synchronisations- und Exit-Plan existieren.

Forks werden in einem Register mit Upstream-Commit, lokaler Differenz, Reviewdatum und Sicherheitsstatus geführt.

## 7. Aktualisierungsprozess

- regelmäßige Dependency-Updates in kleinen Gruppen;
- Changelog und Breaking Changes der Lieferanten prüfen;
- Gallery/Visualtests vor Übernahme visueller Updates;
- Securityupdates priorisieren;
- keine automatische Major-Aktualisierung ohne Review.

## 8. Erste Kandidaten

- Krypton Standard Toolkit: R0-Pilot;
- Ookii Dialogs: optional hinter Services;
- ScottPlot: R2-Chartpilot;
- Markdig/DiffPlex: R2-Editorfunktionen;
- ScintillaNET: separater R2-Adapter;
- WebView2: gehärteter R2-Host;
- PDFsharp/MigraDoc: R2-PDF-Erzeugung.

Die endgültige Freigabe erfolgt erst nach jeweiliger ADR und Lizenzinventarprüfung.
