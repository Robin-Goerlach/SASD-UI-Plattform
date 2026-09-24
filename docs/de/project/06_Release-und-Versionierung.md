# Release- und Versionierungskonzept

## 1. Versionsmodell

Die Plattform verwendet semantische Versionierung.

- `0.x`: API befindet sich in Aufbau und Stabilisierung.
- `1.0`: R1-Kern ist dokumentiert, produktiv erprobt und kompatibilitätsgeführt.
- Patch: kompatible Fehlerkorrekturen.
- Minor: kompatible neue Funktionen.
- Major: Breaking Changes.

Previews können Suffixe wie `-alpha.N`, `-beta.N` oder `-rc.N` verwenden.

## 2. Paketversionen

Die R1-Pakete verwenden grundsätzlich eine gemeinsame Plattformversion, um Consumer- und Supportkomplexität zu reduzieren. Adapter können nur dann abweichend versioniert werden, wenn sie tatsächlich unabhängig releasen müssen; dies benötigt eine ADR.

## 3. Releaseinhalt

- NuGet-Pakete und Symbole;
- XML-Dokumentation;
- Changelog/Release Notes;
- Migrationshinweise;
- SBOM und Third-Party-Notices;
- bekannte Einschränkungen;
- Prüfsummen;
- Gallery-/Sample-Version passend zum Release.

## 4. Kompatibilitätsregeln

Breaking sind unter anderem:

- Entfernen oder Umbenennen öffentlicher Typen/Member;
- Änderung von Standardverhalten mit hohem Consumer-Risiko;
- neue zwingende Abhängigkeit;
- Änderung persistierter State-Schemata ohne Migration;
- Änderung von AutomationIds ohne Migrations-/Testhinweis.

Nicht jede visuelle Korrektur ist Breaking. Sie wird jedoch im Changelog dokumentiert, wenn Screenshots, Layout oder Nutzerabläufe relevant verändert werden.

## 5. Releaseprozess

1. Scope einfrieren.
2. offene kritische Fehler und Securitybefunde prüfen.
3. vollständige Testmatrix ausführen.
4. Public-API-Diff prüfen.
5. Changelog und Migration aktualisieren.
6. SBOM/Lizenzen prüfen.
7. Release Candidate in realer SASD-Anwendung testen.
8. Tag erzeugen und aus sauberem Checkout packen.
9. intern veröffentlichen.
10. Smoke Test durch Neuinstallation in leerer Consumerlösung.

## 6. Supportlinien

Bis 1.0 wird primär die aktuelle 0.x-Linie unterstützt. Nach 1.0 wird entschieden, ob eine LTS-Linie wirtschaftlich sinnvoll ist. Sicherheitskritische Fehler können auch ältere noch verwendete Versionen betreffen; der Supportumfang wird pro Release dokumentiert.

## 7. Deprecation

Veraltete APIs werden zunächst markiert, dokumentiert und mit Ersatz versehen. Entfernung erfolgt frühestens in einer Major-Version, außer eine akute Sicherheits- oder Lizenzlage erzwingt schnelleres Handeln.
