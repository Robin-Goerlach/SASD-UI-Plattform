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
- width/order/state persistence remains in `SasdDataGridState` rather than being duplicated.

## Boundaries

The Data module deliberately contains no SQL generation, ORM integration or database-specific filtering. Applications translate `SasdDataQuery` and filter descriptors into their own data-access technology. The R2 column chooser is a focused usability helper, not an attempt to build an enterprise grid suite.
