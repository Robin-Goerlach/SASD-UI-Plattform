# Sasd.Ui.Sample.Utility

Runnable R1 reference consumer for SASD UI Platform Windows utilities, progress feedback, tray lifecycle and local UI-state services.

## Demonstrated platform behavior

- native file and folder selection through `SasdFileDialogService`;
- fault-tolerant clipboard text read/write through `SasdClipboardService`;
- allow-listed URI and existing-path opening through `SasdShellService`;
- cancellable worker progress through `SasdProgressDialog`, including progress reported before the dialog handle exists;
- explicit notification-area visibility and application-owned Open/Exit policy through `SasdTrayService`;
- versioned local UI-state storage and form placement through `SasdStateStore` / `SasdFormStateService`;
- runtime diagnostics and status feedback without a background service or telemetry.

## Ownership and privacy boundaries

The sample keeps business/content data outside the UI-state store. Selected paths and arbitrary clipboard text are deliberately not persisted because they may reveal sensitive user data. Only small presentation settings, such as window placement and whether the sample tray icon was enabled, are stored.

The application owns process-exit policy. `SasdTrayService` raises requests but never terminates the process itself. Likewise, shell and clipboard operations return recoverable result objects so a temporary Windows integration failure does not become an unhandled UI-thread exception.

The sample owns every service instance it creates and disposes tray/state resources explicitly with the form. It contains no database/ORM access, network client, hidden daemon, telemetry or application-specific persistence framework.

## Verification

The repository-wide `build/verify.ps1` gate restores and builds this project explicitly with analyzer warnings treated as errors. It is intentionally kept outside the hand-maintained product solution, like the other reference consumers, so the gate also proves that the sample does not rely on solution-only state or copied binaries.
