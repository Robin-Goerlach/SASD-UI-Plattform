# Sasd.Ui.WinForms.Shell

Reusable application-shell, navigation, document and status components for SASD WinForms applications.

## Implemented R1 foundation

- `SasdShellForm` composes the command bar, navigation host, status surface and global command manager without a service locator;
- `SasdCommandBar` projects application commands into a standard toolbar surface;
- `SasdNavigationHost` registers application page factories, owns created views, exposes DPI/accessibility-aware navigation surfaces and supports explicit cached/non-cached view lifecycles;
- `SasdBreadcrumb` exposes a stable application-defined path with explicit grouping/link/current-location accessibility semantics while leaving routing to the application;
- `SasdDocumentTabs` owns factory-created document controls, keeps stable document ids, updates visible/accessibility titles together and supports Ctrl+Tab / Ctrl+Shift+Tab document switching;
- `SasdStatusService`, `SasdStatusBinding` and `SasdStatusBar` provide priority-aware status feedback.

## Navigation page ownership and cache policy

Applications own the page factories registered through `SasdNavigationHost.RegisterPage(...)`. Once a factory successfully returns a visual `Control`, ownership of that control transfers to the navigation host. Applications should therefore keep business state outside the view when it must outlive the visual page and should not dispose cached page controls themselves.

With `CachePages == false`, a view is disposed when navigation leaves it. With caching enabled, inactive views remain host-owned and are reused. Runtime changes of `CachePages` are supported as ownership transitions rather than treated as a performance-only hint: disabling caching immediately disposes inactive cached views while preserving the currently displayed child until navigation leaves it; enabling caching adopts the current view into the cache so the next navigation cannot detach it without an owner.

The host also refuses to reattach an unexpectedly disposed cached view. If application code violates the ownership contract and disposes one, the stale cache entry is removed and the registered factory creates a replacement. Host disposal releases inactive cached views explicitly; the current visual child follows normal WinForms parent/child disposal.

The navigation host, navigation list and content pane expose descriptive accessibility metadata, and the host opts into DPI scaling. These automated contracts do not replace the remaining real Designer, mixed-DPI, High-Contrast and UI Automation acceptance work.

## Breadcrumb presentation and ownership

`SasdBreadcrumb` owns the generated `LinkLabel`, separator and current-location controls that represent the current path. Replacing or clearing the path disposes those generated controls rather than merely detaching them, so repeated navigation does not accumulate abandoned WinForms resources.

The control exposes the complete breadcrumb and its internal path host as accessible groupings, ancestor locations as links, decorative separators as separators and the final location as non-interactive static text. Its accessible description follows the number of locations and the current location. Invoking an ancestor raises `ItemInvoked` with the stable application-owned breadcrumb item; the application remains responsible for deciding whether and how navigation occurs.

## Document ownership and keyboard behavior

`SasdDocumentTabs.OpenOrSelect(...)` takes ownership of a newly factory-created visual document control after it is accepted into the host. Closing that document disposes its `TabPage` and therefore the owned content control. Business/domain state that must survive closing belongs outside the visual control.

The host guarantees conventional Ctrl+Tab and Ctrl+Shift+Tab switching only when at least two documents are open. It deliberately does **not** impose a generic Ctrl+W close shortcut: applications need an explicit dirty/save/close-cancellation policy before a keyboard shortcut may safely close potentially unsaved work.

Document titles are exposed both visually and through accessibility metadata. Reopening an existing stable id with a different title updates the same owned document rather than creating duplicate content.

## Boundaries

The Shell module coordinates presentation components only. Applications retain ownership of routing decisions, business state, persistence, unsaved-work policy and the command implementations registered with the shell.

The module contains no database/ORM integration, network/background service, telemetry or vendor-specific shell implementation. Platform keyboard behavior is limited to interactions whose semantics can be guaranteed without application-domain knowledge.
