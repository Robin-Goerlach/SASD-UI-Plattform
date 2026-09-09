# Datenhaltung, Persistenz und UI-State

## 1. Architekturentscheidung

Die SASD UI Platform besitzt **keine fachliche Datenbank** und kein ORM. Sie kennt weder Tabellen, Entitäten noch Geschäftsobjekte der Consumer-Anwendungen. Ein klassisches Datenbankdokument wäre deshalb für die Plattform irreführend.

Die Plattform stellt ausschließlich bereit:

- versionierte UI-Zustände;
- Fenster- und Layoutzustände;
- Grid-/Listen-/Baumansichtszustände;
- Recent Items/MRU;
- sichere Persistenzverträge und Migrationsmechanismen;
- datenquellenneutrale Paging-/Filter-/Sortiermodelle.

## 2. Verantwortungsgrenze

| Plattform | Consumer-Anwendung |
| --- | --- |
| Fenstergröße/-position | fachliche Datensätze |
| Themeauswahl | Datenbankverbindung |
| Spaltenbreite/-reihenfolge | ORM/SQL/REST |
| gespeicherte Filterdefinition | Berechtigungen |
| zuletzt geöffnete sichere Referenzen | Dokument- oder Mailinhalte |
| Navigation/UI-Layout | Transaktionen und Konsistenz |

## 3. Speicherort

Standard:

```text
%LocalAppData%\SASD-GmbH\<Produkt>\
├─ ui-state.json
├─ ui-state.backup.json
├─ layouts\
└─ logs\
```

Die genaue Anwendungskomponente entscheidet über Produktnamen und zusätzliche Unterordner. Roaming wird nicht als Standard angenommen, weil Monitor-, Fenster- und lokale Pfadreferenzen maschinenspezifisch sein können.

## 4. Dateiformat

- UTF-8 JSON;
- oberste `SchemaVersion`;
- stabile, semantische Schlüssel;
- unbekannte Felder werden tolerant behandelt;
- technische Zeitangaben in UTC/ISO-Format;
- kulturinvariante numerische Persistenz.

Beispiel:

```json
{
  "schemaVersion": 2,
  "theme": "Dark",
  "mainWindow": {
    "x": 120,
    "y": 80,
    "width": 1400,
    "height": 900,
    "state": "Normal"
  },
  "views": {
    "prompt-list": {
      "sort": [{ "field": "UpdatedAt", "direction": "Descending" }],
      "columns": [
        { "id": "Title", "width": 320, "visible": true }
      ]
    }
  }
}
```

## 5. Atomare Speicherung

1. neuen Zustand in Temp-Datei schreiben;
2. Flush/Close;
3. vorhandene Datei als Backup sichern;
4. Temp atomar ersetzen, soweit Dateisystem/Plattform dies unterstützt;
5. bei Fehler bisherige gültige Datei erhalten;
6. Fehler loggen, Anwendung aber mit aktuellem In-Memory-Zustand weiterführen.

## 6. Migration

- Migrationen sind schrittweise (`v1 -> v2 -> v3`), nicht beliebige Sprünge mit monolithischem Code;
- jede Migration ist unit-getestet;
- bei irreparabler Datei wird Backup versucht;
- bleibt Migration erfolglos, wird der betroffene Teilzustand zurückgesetzt;
- Migrationsfehler dürfen keine fachlichen Daten löschen.

## 7. Fensterwiederherstellung

Gespeicherte Koordinaten werden gegen aktuelle Monitore und Working Areas validiert. Ein Fenster, das vollständig außerhalb sichtbarer Bereiche liegt, wird auf einem verfügbaren Monitor mit sicherer Mindestgröße geöffnet.

## 8. Grid-, Filter- und Pagingzustand

Persistiert werden nur UI-Definitionen, keine vollständigen Ergebnisdaten. Filter und Sortierung verwenden stabile Feld-IDs, die die Consumer-Anwendung auf ihre Datenquelle abbildet.

`IDataPageSource<T>` beziehungsweise entsprechende Verträge können implementiert werden durch:

- In-Memory-LINQ;
- SQLite;
- Entity Framework;
- REST/API;
- andere Dienste.

Der UI-Kern kennt die konkrete Umsetzung nicht.

## 9. Recent Items

MRU-Einträge enthalten nur notwendige Referenzen. Sensible Titel/Pfade können anonymisiert, deaktiviert oder vom Consumer kontrolliert werden. Fehlende Dateien werden angezeigt oder entfernt, aber nicht automatisch neu erzeugt.

## 10. Geheimnisse und Datenschutz

Nicht im UI-State speichern:

- Passwörter;
- API-Tokens;
- Connection Strings mit Secrets;
- vollständige E-Mail- oder Dokumentinhalte;
- private Schlüssel;
- unverschlüsselte Zugangsdaten.

Consumer verwenden dafür Credential Manager, Secret Manager oder produktspezifische sichere Speicherung.

## 11. SQLite in Samples

Eine CRUD-Referenz darf SQLite verwenden, um Paging und Datenbindung realistisch zu demonstrieren. SQLite ist dabei **keine Plattformabhängigkeit**. Das Sample implementiert die Datenquellenverträge außerhalb der UI-Pakete.
