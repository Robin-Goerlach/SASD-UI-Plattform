# Sasd.Ui.WinForms.Windows

Reusable Windows-integration services for SASD WinForms applications.

## Implemented R1 foundation

- native open/save/folder dialogs through `ISasdFileDialogService`;
- clipboard access with bounded retry handling through `ISasdClipboardService`;
- constrained shell integration through `ISasdShellService`;
- file drag-and-drop validation through `ISasdDragDropService`;
- explicit notification-area lifetime and Open/Exit requests through `ISasdTrayService`.

## Security and ownership rules

The module deliberately keeps Windows integration behind small service boundaries so application code does not scatter `Clipboard`, `Process.Start`, `NotifyIcon` or drag/drop parsing across forms.

- File extensions are hints, not trust boundaries. Applications must validate imported file contents.
- Drag/drop validation never opens or executes dropped items.
- Shell URI schemes remain allowlisted rather than accepting arbitrary command strings.
- Tray creation is hidden by default; applications must call `Show()` explicitly.
- Choosing the tray Exit item raises an event. The service never terminates the process itself.
- Services document whether returned/native resources are caller-owned or service-owned.

These services are infrastructure helpers only; they do not define application business policy.
