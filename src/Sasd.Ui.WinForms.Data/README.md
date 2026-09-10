# Sasd.Ui.WinForms.Data

Reusable data-presentation components and application-neutral query/state helpers for SASD WinForms applications.

## Implemented R1 foundation

- `SasdSearchBox` with DPI-aware layout, explicit search/editor/clear accessibility semantics and a keyboard-reachable clear action that is removed from navigation while empty;
- `SasdFilterBar` with neutral filter descriptors, explicit grouping/action accessibility semantics, a current-filter accessible summary, and deterministic disposal of regenerated chip controls;
- `SasdEmptyState` with optional primary action;
- `SasdDataGrid` with conservative business-application defaults;
- `SasdGridController<T>` and paging/query contracts without database or ORM coupling;
- `SasdPager` with DPI-aware layout, explicit paging-group semantics, descriptive navigation/page-size accessibility text, and a current page/range description derived from loaded state;
- CSV export;
- persisted grid layout state;
- `SasdListView` and `SasdTreeView` defaults.

### Filter-bar accessibility and lifecycle

`SasdFilterBar` exposes the complete active-filter area and the generated chip list as accessible groupings. Its accessible description follows the current filter set so assistive technologies are not forced to infer active state from visual chip styling or the multiplication-sign glyph used by remove buttons. Individual remove actions and the clear-all action have descriptive accessible text independent of their visual captions.

Filter chips are presentation objects owned by the filter bar. Rebuilding the filter set explicitly disposes the generated controls being replaced rather than merely detaching them. This ownership rule matters on long-running data screens where filter state may change repeatedly and prevents abandoned WinForms controls/native resources from accumulating over time.

### Grid-controller lifecycle and failure handling

`SasdGridController<T>` keeps data access application-owned. A newly requested load cancels an older one, but each individual `LoadAsync` invocation retains ownership of its linked `CancellationTokenSource` until the application page source has actually returned. This avoids disposing cancellation registrations while a superseded source call is still unwinding.

If a result set shrinks while the controller is positioned beyond the new final page, the controller performs at most one corrective request for the new final page. If the source changes again during that correction, the controller clamps its page state and exposes an empty page rather than repeatedly retrying or presenting records under the wrong page index. A later explicit refresh can then reload the stabilized source.

Failures from explicit `LoadAsync`, `SearchAsync` and `SortAsync` calls remain normal task failures for the application to await and handle. Loads initiated by the controller's own WinForms pager/header event handlers cannot return a `Task` to application code, so non-cancellation failures are reported through `LoadFailed`. The event carries the technical exception for logging/diagnostics; applications should choose their own user-safe error presentation instead of displaying raw exception details.

Disposing the controller detaches its WinForms event handlers and requests cancellation of an active load. The in-flight `LoadAsync` invocation remains responsible for disposing its own linked cancellation source after the application-owned source call completes.

## Native R2 foundation

- `SasdGridColumnChooser` binds to a native `DataGridView` without taking ownership;
- searchable column list and immediate visibility changes;
- explicit `ShowAllColumns`, `RefreshColumns`, `Bind` and `Unbind` lifecycle;
- width/order/state persistence remains in `SasdDataGridState` rather than being duplicated;
- `SasdGridViewDefinition` combines grid layout, search text and active filters into one serializable saved view;
- `SasdFilterBar.SetFilters(...)` replaces a complete filter state with a single change notification;
- `SasdSparkline` renders compact finite-number trends without introducing a chart-engine dependency;
- `SasdKpiCard` combines accessible textual KPI information with an optional sparkline while leaving good/bad interpretation to the application.

## Boundaries

The Data module deliberately contains no SQL generation, ORM integration or database-specific filtering. Applications translate `SasdDataQuery` and filter descriptors into their own data-access technology.

Saved grid views contain UI state only. They do not persist queries, database connections, ORM expressions or application domain objects. They can be stored by the existing UI-state infrastructure when an application wants named views.

The native R2 dashboard primitives are intentionally small. They provide common operational UI patterns but do not replace a charting library. Rich interactive charts, axes, legends, aggregation and large-series visualization remain the responsibility of the later chart adapter package.
