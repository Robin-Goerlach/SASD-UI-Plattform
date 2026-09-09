# Sasd.Ui.WinForms.Data

Reusable data-presentation components and application-neutral query/state helpers for SASD WinForms applications.

## Implemented R1 foundation

- `SasdSearchBox` with clear action;
- `SasdFilterBar` and neutral filter descriptors;
- `SasdEmptyState` with optional primary action;
- `SasdDataGrid` with conservative business-application defaults;
- `SasdGridController<T>` and paging/query contracts without database or ORM coupling;
- `SasdPager`;
- CSV export;
- persisted grid layout state;
- `SasdListView` and `SasdTreeView` defaults.

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
