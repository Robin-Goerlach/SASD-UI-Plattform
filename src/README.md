# Source Projects

The `src` directory mirrors the approved product-module architecture. Projects are separated to keep ownership, dependency direction and optional technology boundaries visible. Public NuGet packaging may initially combine several internal projects until the public API is stable.

## Implemented platform modules

- `Sasd.Ui.Core` — platform-neutral result types, contracts and design-token records.
- `Sasd.Ui.WinForms` — designer-friendly WinForms base types and UI-thread helper.
- `Sasd.Ui.WinForms.Theming` — native theme mapping and semantic icon service.
- `Sasd.Ui.WinForms.Commands` — reusable commands, bindings, command bar and global shortcut management.
- `Sasd.Ui.WinForms.Shell` — navigation, breadcrumbs, document tabs, status and shell composition.
- `Sasd.Ui.WinForms.Forms` — layout, validation and native property editing.
- `Sasd.Ui.WinForms.Data` — search/filter/grid/list/tree/paging/export/state helpers and native column chooser.
- `Sasd.Ui.WinForms.Dialogs` — standard dialogs, error details, busy/progress and transient notifications.
- `Sasd.Ui.WinForms.Windows` — file/folder dialogs, clipboard, safe shell integration, drag-and-drop and tray services.
- `Sasd.Ui.WinForms.State` — versioned JSON UI state, migration, window state and recent items.
- `Sasd.Ui.WinForms.Media` — native image viewing with explicit ownership and zoom semantics.
- `Sasd.Ui.WinForms.Krypton` — isolated Krypton pilot/adapter; Krypton types remain outside the native contracts.

## R2/R3 adapters

Specialist third-party integrations belong under `adapters` or a clearly isolated adapter project. Each external dependency requires licence/activity/security/lifecycle review and an exit strategy before it becomes part of the supported platform.

The source tree intentionally favors small, comprehensible modules over a single large UI assembly. Applications should reference only the modules they actually use.
