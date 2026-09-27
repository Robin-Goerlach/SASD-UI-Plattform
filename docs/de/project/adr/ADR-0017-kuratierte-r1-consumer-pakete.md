# ADR-0017 – Kuratierte R1-Consumer-Pakete mit Application-Metapaket

- **Status:** Akzeptiert
- **Datum:** 27.09.2026
- **Produktlinie:** SASD UI Platform – C# WinForms Components
- **Präzisiert:** ADR-0002 – Modulares Monorepo mit wenigen veröffentlichten Paketen

## Kontext

Das Repository enthält bewusst mehr interne Assemblies, als eine typische Anwendung direkt referenzieren sollte. R1 soll diese Code- und Ownership-Grenzen behalten, gleichzeitig aber einen kleinen und verständlichen NuGet-Einstieg anbieten.

Eine große Assembly würde sinnvolle interne Grenzen verwischen. Wenn jede Anwendung Commands, Forms, Dialogs, Shell, State, Theming und Windows einzeln referenzieren müsste, würde die Repository-Struktur unnötig in jede Anwendung getragen.

Ein NuGet-Metapaket erlaubt einen direkten Einstieg, während zur Laufzeit weiterhin die echten modularen Assemblies verwendet werden.

## Entscheidung

R1 definiert fünf **unterstützte direkte Consumer-Einstiegspakete**:

1. `Sasd.Ui.Core`
2. `Sasd.Ui.WinForms`
3. `Sasd.Ui.WinForms.App`
4. `Sasd.Ui.WinForms.Data`
5. `Sasd.Ui.WinForms.Krypton`

`Sasd.Ui.WinForms.App` besitzt keine eigene Runtime-Assembly. Seine direkten Abhängigkeiten sind exakt:

- `Sasd.Ui.WinForms.Commands`
- `Sasd.Ui.WinForms.Dialogs`
- `Sasd.Ui.WinForms.Forms`
- `Sasd.Ui.WinForms.Shell`
- `Sasd.Ui.WinForms.State`
- `Sasd.Ui.WinForms.Theming`
- `Sasd.Ui.WinForms.Windows`

Diese modularen Pakete bleiben reale NuGet-Implementierungsabhängigkeiten, weil ihre Assemblies getrennt bleiben. Sie können daher auf einem internen Feed vorhanden und transitiv sichtbar sein, sind aber **nicht der normale unterstützte direkte Einstieg** für Anwendungsprojekte.

`Sasd.Ui.WinForms.Data`, `Sasd.Ui.WinForms.Krypton`, `Sasd.Ui.WinForms.Media` und spätere R2/R3-Adapter dürfen nicht in die App-Abhängigkeitsliste gelangen, solange keine spätere ADR diese Grenze ausdrücklich ändert.

Templates sind ein R1-Auslieferungsartefakt, aber keine Runtime-Abhängigkeit des App-Metapakets.

## Warum das Metapaket keine Assemblies bündelt

Das App-Paket kopiert bewusst nicht mehrere interne DLLs in ein physisches Paket. Dies würde die Paketerzeugung an Output-Harvesting koppeln, doppelte Assemblies bei zusätzlicher Installation von Data/Krypton begünstigen und API-Ownership verwischen.

Modulare NuGet-Abhängigkeiten erhalten die normale Beziehung zwischen Assembly und Paket und reduzieren trotzdem die Zahl der **direkten** Referenzen einer typischen Anwendung.

## Verifikation

Der Release-Dry-Run muss:

- App als dependency-only NuGet-Paket bauen;
- sicherstellen, dass keine `lib/`- oder `ref/`-Runtime-Assets enthalten sind;
- die exakte direkte Dependency-Allowlist prüfen;
- für das Metapaket kein Symbolpaket erzeugen;
- einen frischen WinForms-Consumer ausschließlich aus dem lokalen Dry-Run-Feed restaurieren;
- repräsentative öffentliche Typen aller App-Module kompilieren;
- nachweisen, dass Data, Krypton und Media nicht transitiv durch App eingezogen werden.

## Positive Folgen

- typische Anwendungen beginnen mit einem einzigen R1-App-Paket;
- interne Assemblies bleiben getrennt testbar und wartbar;
- datenlastige und visuelle/vendorabhängige Funktionen bleiben explizit;
- R2-Adapter gelangen nicht unbemerkt in jede Anwendung.

## Negative Folgen und Risiken

- der Feed enthält weiterhin transitive Implementierungspakete, obwohl die dokumentierte direkte Einstiegsschicht klein ist;
- R1-Paketversionen müssen gemeinsam ausgerichtet bleiben;
- Dokumentation und Support müssen direkte Einstiegspakete von Implementierungspaketen unterscheiden.

## Erwogene Alternativen

**Alle Module gleichrangig veröffentlichen:** für R1 verworfen, weil dies Anwendungen mit Repository-Struktur belastet.

**Eine große R1-Assembly:** verworfen, weil sinnvolle Assembly-Grenzen verloren gehen.

**Alle R1-Assemblies physisch in App bündeln:** zurückgestellt; reduziert die Feed-Anzahl, erhöht aber Paketierungs- und Duplicate-Asset-Komplexität ohne nachgewiesenen R1-Nutzen.

## Review-Kriterium

Neu bewerten, wenn reale SASD-Consumer wiederholt eine andere Standardkombination benötigen, transitive Implementierungspakete administrativ problematisch werden, Assemblies eigene Release-Zyklen erhalten oder eine neue Major-Version vorbereitet wird.
