# SASD UI CRUD Reference

This sample is a deliberately small **in-memory** business application that consumes the reusable WinForms platform as an application would. Its purpose is to expose integration gaps before the first real SASD product depends on stable packages.

## Demonstrated platform components

- `SasdForm` as the application window base;
- `SasdCommandBar` and `SasdCommand` for New/Save/Delete actions;
- `SasdSearchBox` and `SasdDataGrid` for list/search interaction;
- `SasdFieldLayout` for the editor layout;
- `SasdValidationCoordinator` and `SasdValidationSummary` for blocking validation;
- `SasdDialogService` for delete confirmation;
- `SasdStatusBar` for transient operation feedback.

## Application-owned concerns

The sample intentionally keeps these concerns outside the UI platform:

- the `CustomerRecord` application model;
- the in-memory list of records;
- search semantics;
- save/delete business decisions;
- future database, repository or API integration.

There is **no database or ORM** in this sample. Replacing the in-memory list with application-specific persistence should not require changes to the reusable UI packages.

## Flows to review

1. Search by name, email or status.
2. Select a row and edit the record.
3. Create a new record.
4. Attempt to save missing/invalid fields and review field + summary validation.
5. Save a valid record and retain/reselect it in the filtered list.
6. Delete a selected record through the confirmation boundary.
7. Verify keyboard focus, DPI scaling, resizing and accessibility names in a normal desktop session.

The implementation favors explicit, readable event wiring over introducing a sample-only MVVM/framework abstraction. Real SASD applications remain free to structure their domain/application layer differently.
