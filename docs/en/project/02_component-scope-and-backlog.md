# Component Scope and Prioritized Backlog

## 1. Selection Rule

A component is admitted when it can be used by several current or foreseeable SASD WinForms applications. Exotic or extremely expensive components are deliberately not anticipated.

## 2. R1 – Binding Core

### Foundation and Layout

- `SasdForm`, `SasdDialogForm`, `SasdUserControl`;
- `SasdSectionPanel`, `SasdFieldLayout`, `SasdValidationSummary`;
- `SasdSearchBox`, `SasdFilterBar`, `SasdEmptyState`, `SasdBusyOverlay`;
- `SasdUiDispatcher`.

### Shell, Navigation, and Commands

- `SasdShellForm`, `SasdNavigationView`, `SasdBreadcrumb`;
- `SasdDocumentTabs`, `SasdCommandBar`, `SasdCommandManager`;
- `SasdStatusService`/`SasdStatusBar`.

### Data Views

- `SasdDataGrid` and `SasdGridController<T>`;
- `SasdListView`, `SasdTreeView`;
- paging, sorting, and filtering contracts independent of database and ORM;
- CSV export, column state, selection, and basic editing.

### Dialogs and Feedback

- `SasdDialogService`, `SasdErrorDialog`;
- `SasdNotificationService`, `SasdProgressDialog`.

### Windows and File Integration

- `SasdFileDialogService`, `SasdClipboardService`, `SasdDragDropService`;
- `SasdTrayService`, secure external shell and file actions.

### State, Theme, and Icons

- `SasdStateStore`, `SasdRecentItemsService`;
- `SasdThemeService`, `SasdIconService`.

## 3. R2 – Optional Adapters

- technical charts and KPI views;
- Markdown editor and secure preview;
- code/configuration editor;
- diff viewer;
- image viewer;
- QR/barcode service;
- programmatic PDF generation;
- hardened WebView2 host;
- docking/workspace;
- property editor and advanced grid functions.

These components are published only as separate packages. A normal CRUD application must not receive them transitively.

## 4. R3 – Only with Demonstrated Need

- Wizard;
- Ribbon;
- additional advanced views with at least two real consumer scenarios or a strategic single-product need.

## 5. Deliberately Excluded

- Pivot/OLAP;
- spreadsheet engine;
- Word/Office editor;
- visual report or dashboard designer;
- complex scheduler and Gantt;
- specialized 3D/CAD/GIS controls;
- mobile controls;
- cross-platform rendering core;
- universal low-code platform.

## 6. Backlog Prioritization

Every backlog item receives:

- **Reuse value:** number of realistic consumers;
- **Risk reduction:** does it remove local, error-prone special logic?
- **Implementation effort:** including tests, Gallery, and documentation;
- **Maintenance cost:** dependencies, designer behavior, native resources;
- **Dependency value:** does it unblock other core functions?

### Priority Formula

A simple decision aid is:

`Priority = (Reuse value + Risk reduction + Blocker value) - (Effort + Maintenance risk)`

It does not replace professional judgment, but it forces transparent reasoning.

## 7. Component Proposal

Before admission, answer:

1. Which concrete application situation is solved?
2. Which existing component was evaluated?
3. Why is a local helper class insufficient?
4. What is the smallest useful scope?
5. Which states must be visible in the Gallery?
6. Which data, files, or processes cross trust boundaries?
7. What exit strategy exists for third-party libraries?
8. Which release and acceptance criteria apply?
