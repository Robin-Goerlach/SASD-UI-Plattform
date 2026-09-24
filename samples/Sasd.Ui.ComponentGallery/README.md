# SASD UI Component Gallery

The Gallery is the executable specification and primary visual acceptance host for the WinForms UI platform.

## Integration entry point

The application starts with `GalleryShellForm`, an integration-oriented example built on `SasdShellForm`. It demonstrates the common R1 plumbing as one coherent application surface:

- command registration and one shared command runner;
- global keyboard shortcuts and command-bar projection;
- application-owned navigation pages;
- presentation-neutral status publication;
- non-modal notifications through `SasdNotificationService` and `SasdNotificationHost`;
- constrained Windows file drag-and-drop using an explicit `SasdFileDropPolicy`.

The shell also contains a **Native R2** page showing the dependency-free R2 helpers together rather than as isolated constructor examples:

- `SasdGridColumnChooser` bound to a real `SasdDataGrid`;
- `SasdGridViewDefinition` capture/disturb/restore flow with search and filter state;
- `SasdPropertyEditor` with a normal application-owned settings object;
- `SasdImageViewer` demonstrating cloned image ownership;
- `SasdKpiCard` and `SasdSparkline` with textual accessibility context.

Use **Ctrl+Shift+R** to open the Native R2 page. The original `MainForm` remains the detailed catalog for individual controls and can be opened with **Ctrl+D** or the **Detailed gallery** command.

## Acceptance role

Gallery examples should favor readable, general sample code over clever abstraction or micro-optimization. New or materially changed components should demonstrate the states that apply to them, including normal, focus, disabled, read-only, busy, empty, error, High Contrast, DPI and localisation behavior.

The Gallery does not replace automated tests. CI smoke checks cover contracts and lifecycle behavior; the Gallery complements them with visual and interaction review. A component is not considered release-ready only because it appears in this sample.

The conceptual target image is stored at [`../../artefacts/sasd-ui-platform-component-gallery.png`](../../artefacts/sasd-ui-platform-component-gallery.png).
