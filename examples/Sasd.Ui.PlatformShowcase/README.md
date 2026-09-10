# SASD UI Platform — Integrated Showcase

This executable example combines the current SASD WinForms UI Platform modules into one small desktop application.

It is intentionally separate from the Component Gallery and from the focused CRUD/Workbench reference applications:

- **Component Gallery** — component catalogue and acceptance surface;
- **focused samples** — realistic examples for one application shape;
- **Integrated Showcase** — broad manual exercise surface for many platform components in one process.

## Run

From the repository root on Windows:

```powershell
dotnet run --project examples/Sasd.Ui.PlatformShowcase/Sasd.Ui.PlatformShowcase.csproj
```

Or run the repository verification gate first:

```powershell
pwsh ./build/verify.ps1
```

## Demonstration pages

| Page | Platform areas exercised |
| --- | --- |
| Overview | `SasdShellForm`, navigation, command bar, status service, `SasdKpiCard`, `SasdSparkline` |
| Forms & validation | `SasdFieldLayout`, `SasdValidationCoordinator`, `SasdValidationSummary`, ErrorProvider integration |
| Data & grid | `SasdSearchBox`, `SasdFilterBar`, `SasdDataGrid`, `SasdGridColumnChooser`, `SasdDataGridState`, `SasdGridViewDefinition`, in-memory `SasdCsvExporter` preview |
| Controls lab | `SasdSectionPanel`, `SasdBreadcrumb`, `SasdDocumentTabs`, `SasdListView`, `SasdTreeView`, `SasdEmptyState`, `SasdPager` |
| Dialogs & feedback | `SasdDialogService`, application-owned `SasdDialogForm`, `SasdProgressDialog`, `SasdNotificationService`, `SasdNotificationHost`, `SasdBusyOverlay` |
| State & recent items | `SasdStateStore`, versioned JSON state sections, `SasdRecentItemsService` |
| Windows integration | file/folder dialogs, clipboard, safe shell URI handling, `SasdTrayService`, `SasdSystemIconService`, policy-validated drag-and-drop |
| R2 native controls | `SasdPropertyEditor`, `SasdImageViewer`, `SasdKpiCard`, image ownership and zoom behaviour |
| Acceptance lab | live DPI/theme/window snapshot plus an explicit keyboard focus route for manual scale, focus and High-Contrast observations |
| Self test | public-API checks for base controls, dialog defaults, search/filters, grid state, `SasdGridController<T>`, CSV escaping, KPI/accessibility, image ownership, property editor, semantic icons, hidden tray defaults, progress, themes and state round-trip |

The showcase does not try to give every small type a separate page. Related controls are grouped into realistic interaction surfaces, while the **Self test** page covers additional public contracts that are easier to verify programmatically than visually.

The **Acceptance lab** has a different purpose: it assists human checks that depend on the actual Windows display configuration. It intentionally does not claim that a release gate passed merely because it can display the current DPI or because the focus route works on one machine.

## Suggested manual test tour

A useful first pass is:

1. switch between Light, Dark and High Contrast from the command bar and with the shortcuts below;
2. enter invalid and valid form values and inspect the validation summary;
3. search/filter the grid, hide a column, preview CSV, save a view, change the layout and restore it;
4. exercise breadcrumbs, list/tree defaults, paging and document open/close ownership in **Controls lab**;
5. open the information/warning/error/confirmation dialogs, the custom `SasdDialogForm`, notifications, busy overlay and cancellable progress dialog;
6. save/load/remove UI state and add/reload/clear recent items;
7. explicitly show/hide the tray icon, inspect semantic icons, test clipboard/dialog/shell operations and drop accepted/rejected files;
8. change image zoom modes, replace/clear the generated image and toggle the property editor read-only projection;
9. use **Acceptance lab** at the Windows display scales available to you, resize/move the window, then traverse the numbered focus route using Tab and Shift+Tab only;
10. finish with **Self test** and inspect the visible PASS/FAIL list.

For formal DPI/Designer/accessibility acceptance, copy the observations into the project evidence rather than treating the live Acceptance-lab snapshot as permanent proof. In particular, a manual pass at one DPI must not be generalized to 125/150/200 percent or mixed-monitor behavior that was not actually tested.

## Theme shortcuts

- `Ctrl+Alt+L` — Light
- `Ctrl+Alt+D` — Dark
- `Ctrl+Alt+H` — High Contrast
- `Ctrl+T` — Self Test page

## Data and persistence boundaries

The example keeps its business data in memory. `SasdStateStore` is used only for disposable UI/demo state and recent-item references. It is not used as a customer/domain database.

Persisted showcase UI state is stored under the normal LocalAppData hierarchy for:

```text
SASD-GmbH/Sasd.Ui.PlatformShowcase
```

The State page lets users create and remove its demonstration section. Other application data is not touched.

## Security and side effects

The Windows page deliberately keeps side effects explicit:

- file and folder dialogs only return selected paths;
- drag-and-drop validates path metadata and an extension/size policy but does not trust file contents;
- clipboard actions happen only after pressing their buttons;
- the repository URL opens only after pressing **Open repository**;
- the tray icon remains hidden until **Show tray icon** is pressed, and its Exit event does not terminate the showcase;
- the showcase starts no background service and performs no network request by itself.

The **Self test** page is deliberately safer still: it does not open modal dialogs, show a tray icon, launch external programs or access the network. Its StateStore check writes and removes only its dedicated test section.

The **Acceptance lab** also remains local-only. It reads runtime UI/process information such as the current control DPI, window size and Windows High-Contrast flag; it does not inspect other applications or send the observations anywhere.

## Why the code is explicit

The example is meant to be read. It therefore uses direct constructors, fields and event wiring instead of adding a dependency-injection or MVVM framework solely for the demo. A production SASD application can layer its own domain/application architecture around the UI Platform.

The sample intentionally prioritises clarity, correct ownership and predictable error handling over micro-optimisation.
