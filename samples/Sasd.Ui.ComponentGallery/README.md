# SASD UI Component Gallery

The Gallery is the executable specification and primary visual acceptance host for the WinForms UI platform.

## R1 entry point

The application starts with `GalleryShellForm`, an integration-oriented example built on `SasdShellForm`. It demonstrates the common R1 plumbing as one coherent application surface:

- command registration and one shared command runner;
- global keyboard shortcuts and command-bar projection;
- application-owned navigation pages;
- presentation-neutral status publication;
- non-modal notifications through `SasdNotificationService` and `SasdNotificationHost`;
- constrained Windows file drag-and-drop using an explicit `SasdFileDropPolicy`.

The original `MainForm` remains the detailed catalog for individual controls and can be opened from the integration shell with **Ctrl+D** or the **Detailed gallery** command.

## Acceptance role

Gallery examples should favor readable, general sample code over clever abstraction or micro-optimization. New or materially changed components should demonstrate the states that apply to them, including normal, focus, disabled, read-only, busy, empty, error, High Contrast, DPI and localisation behavior.

The Gallery does not replace automated tests. CI smoke checks cover contracts and lifecycle behavior; the Gallery complements them with visual and interaction review.

The conceptual target image is stored at [`../../artefacts/sasd-ui-platform-component-gallery.png`](../../artefacts/sasd-ui-platform-component-gallery.png).
