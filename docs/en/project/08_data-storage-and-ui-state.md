# Data Storage, Persistence, and UI State

## 1. Architecture Decision

The SASD UI Platform has **no business database** and no ORM. It knows neither tables, entities, nor business objects belonging to consumer applications. A conventional database design document would therefore be misleading for the platform.

The platform provides only:

- versioned UI state;
- window and layout state;
- grid/list/tree view state;
- recent items/MRU;
- secure persistence contracts and migration mechanisms;
- data-source-neutral paging, filtering, and sorting models.

## 2. Responsibility Boundary

| Platform | Consumer Application |
| --- | --- |
| Window size and position | Business records |
| Theme selection | Database connection |
| Column width and order | ORM/SQL/REST |
| Stored filter definition | Authorization |
| Recently opened safe references | Document or email contents |
| Navigation and UI layout | Transactions and consistency |

## 3. Storage Location

Default:

```text
%LocalAppData%\SASD-GmbH\<Product>\
├─ ui-state.json
├─ ui-state.backup.json
├─ layouts\
└─ logs\
```

The consuming application selects the product name and additional folders. Roaming is not assumed by default because monitor, window, and local-path references can be machine-specific.

## 4. File Format

- UTF-8 JSON;
- top-level `SchemaVersion`;
- stable semantic keys;
- tolerant handling of unknown fields;
- technical timestamps in UTC/ISO format;
- culture-invariant numeric persistence.

Example:

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

## 5. Atomic Persistence

1. Write the new state to a temporary file.
2. Flush and close it.
3. Preserve the existing file as a backup.
4. Replace it atomically where the file system and platform support this.
5. Retain the previous valid file if replacement fails.
6. Log the error and continue with the current in-memory state.

## 6. Migration

- migrations are incremental (`v1 -> v2 -> v3`), not arbitrary jumps through monolithic code;
- every migration is unit-tested;
- if a file cannot be repaired, attempt the backup;
- if migration still fails, reset only the affected partial state;
- migration errors must never delete business data.

## 7. Window Restoration

Stored coordinates are validated against current monitors and working areas. A window fully outside visible areas is opened on an available monitor with a safe minimum size.

## 8. Grid, Filter, and Paging State

Only UI definitions are persisted, never complete result data. Filters and sorting use stable field IDs that the consumer maps to its data source.

`IDataPageSource<T>` or equivalent contracts may be implemented by:

- in-memory LINQ;
- SQLite;
- Entity Framework;
- REST/API;
- other services.

The UI core does not know the concrete implementation.

## 9. Recent Items

MRU entries contain only necessary references. Sensitive titles or paths can be anonymized, disabled, or controlled by the consumer. Missing files may be displayed or removed but are never recreated automatically.

## 10. Secrets and Privacy

Do not store in UI state:

- passwords;
- API tokens;
- connection strings containing secrets;
- complete email or document contents;
- private keys;
- unencrypted credentials.

Consumers use Credential Manager, Secret Manager, or product-specific secure storage.

## 11. SQLite in Samples

A CRUD reference application may use SQLite to demonstrate paging and data binding realistically. SQLite is **not a platform dependency**. The sample implements data-source contracts outside the UI packages.
