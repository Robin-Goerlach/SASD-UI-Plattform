# Dokumentationsstandard

## 1. Ziele

Dokumentation muss Entscheidungen nachvollziehbar, Code nutzbar und Wartung unabhängig von implizitem Wissen machen.

## 2. Dokumentationsebenen

- Produkt-/Anforderungsdokumente;
- Architektur und ADRs;
- Entwickler- und Prozessleitfäden;
- API-XML-Dokumentation;
- Component Gallery;
- Samples und Tutorials;
- Release Notes und Migration;
- Known Issues und Supportdiagnose.

## 3. Markdown-Regeln

- eine H1 pro Datei;
- logisch geschachtelte Überschriften;
- relative Links innerhalb des Repositories;
- Tabellen nur für strukturierte Vergleiche;
- Codeblöcke mit Sprache;
- keine rohen lokalen Dateipfade als dauerhafte Verweise;
- klare Kennzeichnung von Soll, Ist, geplant und verworfen.

## 4. Öffentliche API

Jeder öffentliche Typ und Member erhält:

- Zweck;
- Parameter- und Rückgabebedeutung;
- Fehler-/Exceptionverhalten;
- Threading-/Lifecycle-Hinweis, falls relevant;
- Minimalbeispiel bei nicht trivialer Nutzung.

## 5. Komponentenreferenz

Für jede visuelle Komponente:

- Einsatzfälle und Nichtziele;
- Paket und Namespace;
- wichtige Eigenschaften/Events;
- Theme/DPI/A11y-Verhalten;
- Designerhinweise;
- State-/Persistenzhinweise;
- vollständige Gallery-Seite.

## 6. Änderungslenkung

- Änderungen an Anforderungen aktualisieren Lasten-/Pflichtenheftbezug.
- Architekturänderungen aktualisieren ADR und Architekturdokument.
- Public-API-Änderungen aktualisieren Changelog und Migration.
- neue Drittanbieter aktualisieren Lizenzdokumentation und SBOM.

## 7. Qualitätsprüfung

Vor Release:

- Links prüfen;
- Beispiele bauen;
- Begriffe und Paketnamen abgleichen;
- Versionen/Status prüfen;
- veraltete Screenshots ersetzen;
- bekannte Einschränkungen ausdrücklich nennen.
