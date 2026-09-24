# Sasd.Ui.Sample.Workbench

Runnable R1 reference consumer for a document-oriented SASD desktop workbench.

## Demonstrated platform components

- `SasdShellForm` as the common application shell;
- `SasdCommandManager` / `SasdCommandBar` with global keyboard shortcuts;
- `SasdNavigationHost` for application pages;
- `SasdTreeView` for application-owned project/document navigation;
- `SasdBreadcrumb` for the current workspace/document location;
- `SasdDocumentTabs` for stable document IDs and visual-control lifetime;
- `SasdStatusService` / `SasdStatusBar` for presentation-neutral feedback.

## Sample workflow

The workspace contains deterministic in-memory project documents plus editable notes.

- **Ctrl+N** creates a new editable note.
- **Ctrl+O** opens the selected tree document.
- **Enter** on a document tree node also opens it.
- **Ctrl+W** closes the active document.
- **Ctrl+Shift+W** closes all documents.

Closing a document disposes its editor control. The sample document model remains application-owned, so reopening an editable note recreates the editor from the retained application state.

## Deliberate boundaries

This reference application does **not** add a database, ORM, rich-editor package or docking library. Those are application/specialist-adapter decisions rather than prerequisites for the base workbench composition.

The sample uses a normal multiline `TextBox` to keep the focus on component contracts, command state, navigation and lifecycle. A later approved editor or docking adapter can replace that presentation without changing the application-owned document model.
