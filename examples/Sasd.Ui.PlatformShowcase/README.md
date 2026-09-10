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
| Data & grid | `SasdSearchBox`, `SasdFilterBar`, `SasdDataGrid`, `SasdGridColumnChooser`, `SasdDataGridState`, `SasdGridViewDefinition` |
| Controls lab | `SasdSectionPanel`, `SasdBreadcrumb`, `SasdDocumentTabs`, `SasdListView`, `SasdTreeView`, `SasdEmptyState`, `SasdPager` |
| Dialogs & feedback | `SasdDialogService`, `SasdNotificationService`, `SasdNotificationHost`, `SasdBusyOverlay` |
| State & recent items | `SasdStateStore`, versioned JSON state sections, `SasdRecentItemsService` |
| Windows integration | file/folder dialogs, clipboard, safe shell URI handling, policy-validated drag-and-drop |
| R2 native controls | `SasdPropertyEditor`, `SasdImageViewer`, `SasdKpiCard`, image ownership and zoom behaviour |
| Self test | public-API checks for search, filters, grid state, KPI/accessibility, image ownership, property editor, themes and state round-trip |

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
- the showcase starts no background service and performs no network request by itself.

The **Self test** page is deliberately safer still: it does not open modal dialogs, launch external programs or access the network. Its StateStore check writes and removes only its dedicated test section.

## Why the code is explicit

The example is meant to be read. It therefore uses direct constructors, fields and event wiring instead of adding a dependency-injection or MVVM framework solely for the demo. A production SASD application can layer its own domain/application architecture around the UI Platform.

The sample intentionally prioritises clarity, correct ownership and predictable error handling over micro-optimisation.