# SASD UI Utility Reference

This sample is a small Windows desktop utility that demonstrates how application code should consume the platform's Windows integration services instead of scattering static framework calls throughout forms.

## Demonstrated services

- `SasdFileDialogService` for existing-file and folder selection;
- `SasdClipboardService` for recoverable text clipboard operations;
- `SasdShellService` for constrained opening of existing paths and allowed URI schemes;
- `SasdStatusBar` for transient user feedback;
- `SasdForm` as the normal DPI-aware application window base.

## Safety boundaries

The sample intentionally does **not**:

- execute arbitrary command lines;
- interpret a selected file as trusted content;
- hide shell/clipboard errors behind unhandled UI-thread exceptions;
- make file dialogs responsible for business validation;
- introduce a database, background service or new runtime dependency.

The text box is only a convenient staging surface for the example. Choosing a path does not open it automatically; opening is a separate explicit action through `SasdShellService`.

## Manual review flow

1. Choose a file and verify that only the selected path is returned.
2. Choose a folder.
3. Copy/paste text and observe recoverable status feedback.
4. Try **Open path** with an empty or missing path and verify that the application remains running.
5. Open the fixed HTTPS example and confirm that no arbitrary URI scheme is composed by the sample.
6. Review resizing, keyboard focus, DPI scaling and accessible names.
