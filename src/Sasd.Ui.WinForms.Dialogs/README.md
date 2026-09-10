# Sasd.Ui.WinForms.Dialogs

Reusable dialog and non-modal feedback primitives for SASD WinForms applications.

## Implemented R1 foundation

- `ISasdDialogService` / `SasdDialogService` for information, warning, error and confirmation flows;
- `SasdDialogForm` integration for application-owned custom dialogs;
- `SasdErrorDialog` for user-safe error presentation with optional technical details;
- `SasdBusyOverlay` for explicit in-window busy feedback;
- `SasdProgressDialog` for cancellable progress with hardened pre-handle/pre-show worker updates;
- `SasdNotificationService` and `SasdNotificationHost` for transient non-modal feedback;
- notification-host binding/unbinding, explicit dismissal and lifetime handling.

## Threading and lifecycle

WinForms controls remain UI-thread owned. Service publications may originate on worker threads, so visual hosts marshal work to their owner thread once a native handle exists.

`SasdNotificationHost` intentionally models **one current notification**, not an unbounded queue. If worker publications arrive before handle creation, only the latest pending notification is retained and projected when `HandleCreated` runs. An explicit owner-thread notification supersedes an older pending worker value. Disposal detaches service subscriptions and discards pending transient feedback.

`SasdProgressDialog` similarly treats startup/handle creation as an explicit lifecycle boundary. Callers should still dispose application-owned dialogs deterministically and use cancellation tokens for cooperative work cancellation.

## Boundaries

This module does not provide a second application message bus, background worker framework or persistent audit log. Transient notifications are not suitable for security confirmations or failures that the user must acknowledge; those belong on persistent or modal surfaces chosen by the application.

The module introduces no hidden network activity or telemetry. Formal keyboard, DPI, High-Contrast, screen-reader and UI Automation evidence remains part of the project acceptance matrix rather than being inferred from unit/smoke behavior.
