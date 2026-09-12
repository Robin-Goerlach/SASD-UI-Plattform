# Sasd.Ui.WinForms.Dialogs

Reusable dialog and non-modal feedback primitives for SASD WinForms applications.

## Implemented R1 foundation

- `ISasdDialogService` / `SasdDialogService` for information, warning, error and confirmation flows;
- `SasdDialogForm` integration for application-owned custom dialogs;
- `SasdErrorDialog` for user-safe error presentation with optional technical details;
- `SasdBusyOverlay` for explicit in-window busy feedback, indeterminate/determinate progress, cooperative cancellation requests and bounded keyboard focus while work is active;
- `SasdProgressDialog` for cancellable progress with hardened pre-handle/pre-show worker updates;
- `SasdNotificationService` and `SasdNotificationHost` for transient non-modal feedback;
- notification-host binding/unbinding, explicit dismissal and lifetime handling.

## Busy-overlay ownership and input policy

`SasdBusyOverlay` owns only the presentation of a busy operation. Applications continue to own the actual `Task`, `CancellationTokenSource`, transaction/rollback behavior and any domain consistency policy. Setting `CancellationEnabled` exposes a cancel action and `CancelRequested` event; the overlay never cancels arbitrary application work by itself.

Each `BeginBusy(...)` starts in indeterminate mode. Applications that can report meaningful completion use `SetProgress(0..100)` and can return to marquee mode with `SetIndeterminate()`. The progress surface exposes accessible state independently from the busy message.

A visible busy overlay moves keyboard focus onto itself or its cancel action and keeps Tab/Shift+Tab inside the blocking surface. Escape is consumed so it cannot accidentally activate a parent form's cancel/close behavior; when cancellation is enabled, Escape raises the same cooperative `CancelRequested` event as the button. `EndBusy()` hides the surface and best-effort restores the previously focused control when it still exists and can be selected.

## Threading and lifecycle

WinForms controls remain UI-thread owned. Service publications may originate on worker threads, so visual hosts marshal work to their owner thread once a native handle exists.

`SasdNotificationHost` intentionally models **one current notification**, not an unbounded queue. If worker publications arrive before handle creation, only the latest pending notification is retained and projected when `HandleCreated` runs. An explicit owner-thread notification supersedes an older pending worker value. Disposal detaches service subscriptions and discards pending transient feedback.

`SasdProgressDialog` similarly treats startup/handle creation as an explicit lifecycle boundary. Callers should still dispose application-owned dialogs deterministically and use cancellation tokens for cooperative work cancellation.

## Boundaries

This module does not provide a second application message bus, background worker framework or persistent audit log. Transient notifications are not suitable for security confirmations or failures that the user must acknowledge; those belong on persistent or modal surfaces chosen by the application.

The module introduces no hidden network activity or telemetry. Formal keyboard, DPI, High-Contrast, screen-reader and UI Automation evidence remains part of the project acceptance matrix rather than being inferred from unit/smoke behavior.
