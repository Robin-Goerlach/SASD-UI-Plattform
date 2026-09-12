# Sasd.Ui.WinForms.Data

Reusable data-presentation components and application-neutral query/state helpers for SASD WinForms applications.

## Implemented R1 foundation

- `SasdSearchBox` with DPI-aware/accessibility-aware editor/clear UI, immediate text-state events, a UI-thread debounce, explicit search-request commits and Enter/Escape keyboard behavior;
- `SasdFilterBar` with neutral filter descriptors, explicit grouping/action accessibility semantics, a current-filter accessible summary, and deterministic disposal of regenerated chip controls;
- `SasdEmptyState` with optional primary action;
- `SasdDataGrid` with conservative business-application defaults;
- `SasdGridController<T>` and paging/query contracts without database or ORM coupling;
- `SasdPager` with DPI-aware layout, explicit paging-group semantics, descriptive navigation/page-size accessibility text, and a current page/range description derived from loaded state;
- CSV export;
- persisted grid layout state;
- `SasdListView` with conservative native defaults;
- `SasdTreeView` with conservative native defaults plus application-owned lazy child loading, generic retry presentation and shutdown cancellation.

### Search-box edit and request contract

`SasdSearchBox.SearchTextChanged` remains the immediate edit/state event. Application code that performs filtering, queries or other potentially expensive work should normally subscribe to `SearchRequested` instead. By default, edited text is committed after a 300 ms quiet period. The delay is controlled by `DebounceMilliseconds`; setting it to zero requests on each text change.

The debounce uses a component-owned `System.Windows.Forms.Timer`, so it stays on the WinForms UI message loop and introduces no worker thread or hidden background service. The timer is restarted on each edit and stopped before publishing, making one quiet edit state produce one request. It is explicitly disposed with the control.

`RequestSearch()` commits the current text immediately and cancels its pending debounce. Enter performs the same commit while the native editor owns focus. `ClearSearch()`, the clear button and Escape on a non-empty search clear the text and immediately request the empty state so stale filtered results are not left visible for the debounce interval. An already-empty search does not claim Escape, leaving a containing dialog free to use its normal Cancel/Escape behavior.

`SasdSearchRequestedEventArgs.SearchText` is a snapshot of the exact editor text at publication time. Search interpretation and normalization remain application/controller responsibilities; the UI component does not create SQL, ORM expressions or domain queries.

### Filter-bar accessibility and lifecycle

`SasdFilterBar` exposes the complete active-filter area and the generated chip list as accessible groupings. Its accessible description follows the current filter set so assistive technologies are not forced to infer active state from visual chip styling or the multiplication-sign glyph used by remove buttons. Individual remove actions and the clear-all action have descriptive accessible text independent of their visual captions.

Filter chips are presentation objects owned by the filter bar. Rebuilding the filter set explicitly disposes the generated controls being replaced rather than merely detaching them. This ownership rule matters on long-running data screens where filter state may change repeatedly and prevents abandoned WinForms controls/native resources from accumulating over time.

### Grid-controller lifecycle and failure handling

`SasdGridController<T>` keeps data access application-owned. A newly requested load cancels an older one, but each individual `LoadAsync` invocation retains ownership of its linked `CancellationTokenSource` until the application page source has actually returned. This avoids disposing cancellation registrations while a superseded source call is still unwinding.

If a result set shrinks while the controller is positioned beyond the new final page, the controller performs at most one corrective request for the new final page. If the source changes again during that correction, the controller clamps its page state and exposes an empty page rather than repeatedly retrying or presenting records under the wrong page index. A later explicit refresh can then reload the stabilized source.

Failures from explicit `LoadAsync`, `SearchAsync` and `SortAsync` calls remain normal task failures for the application to await and handle. Loads initiated by the controller's own WinForms pager/header event handlers cannot return a `Task` to application code, so non-cancellation failures are reported through `LoadFailed`. The event carries the technical exception for logging/diagnostics; applications should choose their own user-safe error presentation instead of displaying raw exception details.

Disposing the controller detaches its WinForms event handlers and requests cancellation of an active load. The in-flight `LoadAsync` invocation remains responsible for disposing its own linked cancellation source after the application-owned source call completes.

### Tree lazy-loading and retry contract

`SasdTreeView.RegisterLazyNode(...)` lets an application defer expensive child discovery until a node is first expanded. The application supplies the asynchronous loader and therefore retains responsibility for file-system, API, database or domain access. The UI Platform only owns the presentation/lifecycle boundary around that loader.

A registered node receives a lightweight placeholder so native WinForms exposes an expansion affordance before the actual children are known. The first expansion invokes the loader exactly once. Child nodes returned by the loader must be detached from any other tree location; after successful attachment they follow the normal WinForms tree hierarchy. A successfully loaded node is not reloaded on every collapse/expand cycle.

Loader failures are converted into a generic retry child instead of displaying raw exception text. The technical exception is available through `NodeLoadFailed` for application logging or diagnostics. Selecting the retry child and pressing Enter, or double-clicking it, retries through the same registered loader. Applications should keep user-facing diagnostics separate from technical exception details.

The control owns the cancellation sources it creates for active lazy loads. Disposing the TreeView requests cancellation but deliberately does not dispose an in-flight source until the application-owned loader task has actually unwound; this avoids invalidating cancellation registrations that the loader still uses. Cancellation caused by control disposal is a normal lifecycle boundary and is not surfaced as `NodeLoadFailed`.

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
