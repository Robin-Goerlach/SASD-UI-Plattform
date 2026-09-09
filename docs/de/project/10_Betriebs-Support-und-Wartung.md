# Betriebs-, Support- und Wartungskonzept

## 1. Betriebsmodell

Die UI Platform wird als Bibliothek in SASD-Anwendungen eingebettet. Sie besitzt keinen eigenen Serverbetrieb. Betrieb bedeutet daher:

- Paketversorgung;
- Kompatibilität mit .NET/Windows/Visual Studio;
- Diagnose von UI-, Designer-, DPI- und Ressourcenproblemen;
- Abhängigkeits- und Securityupdates;
- Unterstützung konsumierender Anwendungen.

## 2. Supportklassen

| Klasse | Bedeutung |
| --- | --- |
| S1 | Anwendung startet nicht, Datenverlust-/Securityrisiko, kritischer Handle-/Crashfehler |
| S2 | zentrale Komponente unbrauchbar, kein akzeptabler Workaround |
| S3 | eingeschränkte Funktion oder visuelle/Accessibility-Abweichung |
| S4 | Verbesserung, Dokumentations- oder Komfortthema |

## 3. Diagnoseinformationen

Consumer sollen bei Fehlern bereitstellen können:

- Plattform- und Paketversionen;
- Windows-/Runtimeversion;
- DPI/Monitor-/Themekonfiguration;
- betroffene Komponente und Ablauf;
- Logkorrelation;
- technische Details ohne Geheimnisse;
- Screenshot oder Minimalreproduktion;
- State-Datei nur nach Datenschutzprüfung.

## 4. Logging

Die Plattform nutzt leichte Logging-Abstraktionen und schreibt nicht ungefragt eigene globale Logs. Consumer konfigurieren Ziel und Aufbewahrung. Logs enthalten Komponente, Operation, Dauer, Ergebnis und Korrelation, aber keine Secrets oder vollständige Nutzinhalte.

## 5. Wartung

- regelmäßige Dependency- und Vulnerabilityprüfung;
- Prüfung neuer .NET-LTS- und Windows-Versionen;
- Visual-Studio-Designer-Smoke-Test nach relevanten IDE-Updates;
- jährliche beziehungsweise anlassbezogene Prüfung des Zukunftsregisters;
- Entfernung veralteter Compatibility Adapter nach Migrationsabschluss.

## 6. Known-Issues-Prozess

Bekannte Einschränkungen werden nicht nur in Issues, sondern pro Release dokumentiert. Jeder Eintrag enthält Auswirkung, betroffene Version, Workaround und geplante Behebung beziehungsweise bewusste Akzeptanz.

## 7. Lifecycle

- Preview: nicht produktiv garantiert;
- Current: aktiv unterstützt;
- Maintenance: nur Fehler/Security;
- End of Support: keine regulären Fixes; Migrationsziel dokumentiert.

Bis 1.0 wird nur eine kleine Anzahl paralleler Linien unterstützt.
