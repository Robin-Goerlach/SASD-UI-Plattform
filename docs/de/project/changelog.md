# Änderungsprotokoll

Alle wesentlichen Änderungen an der SASD UI Platform und ihrer Projektdokumentation werden in diesem Dokument festgehalten. Das Format orientiert sich an „Keep a Changelog“ und semantischer Versionierung, ohne eine externe Normabhängigkeit zu erzeugen.

## [Unreleased]

### Geplant

- R0.1-Repository und Buildgrundlage.
- erste ADR-Validierung.
- Krypton-/Native-WinForms-Pilot.
- Component-Gallery-Skelett.

## [0.1.0-docs] – 2026-07-23

### Hinzugefügt

- ergänzende Dokumentenlandschaft für die C#-WinForms-Produktlinie;
- Roadmap und Projektplan R0 bis R3;
- Komponenten-Scope und priorisierter Backlog;
- Entwicklungs-, Test-, Build-, Release- und Governanceleitfäden;
- Sicherheits-, Lizenz-, SBOM- und Abhängigkeitskonzept;
- Datenhaltungs- und UI-State-Konzept mit ausdrücklicher Datenbankabgrenzung;
- Migrations-, Support-, Gallery-, Paketierungs-, DPI- und Accessibility-Dokumente;
- ADR-Register, ADR-Vorlage und initiale ADR-Dateien;
- GitHub-Issue- und Pull-Request-Vorlagen.

### Entscheidungen

- keine fachliche Datenbank innerhalb der UI Platform;
- JSON-basierter, versionierter UI-State unter LocalAppData;
- WinForms/.NET 8 als R0/R1-Fokus;
- spätere Plattformlinien bleiben separat dokumentiert.
