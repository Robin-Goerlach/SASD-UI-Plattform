# SASD UI Platform — Functional Specification for C# WinForms Components

**Technical implementation of a reusable Windows desktop component library**

| Attribute | Definition |
| --- | --- |
| Version | 0.1 |
| Date | 23 July 2026 |
| Status | Technical draft for implementation approval |
| Implementing organisation | SASD-GmbH |
| Product line | Windows Desktop / C# / WinForms |
| Based on | `02-requirements-specification.md` |
| Delivery frame | R0 decision pilot, R1 production foundation, R2 feature modules, R3 optional modules |

> This functional specification describes **how** the 20 component families and 83 requirements defined in the requirements specification will be implemented, verified, packaged and introduced. It does not override the deliberate scope boundaries of the requirements specification.

## Contents

1. Executive summary
2. Relationship to the requirements specification and implementation objectives
3. Technical constraints
4. Architecture principles
5. Repository, solution and package structure
6. Public API, conventions and error contracts
7. Design system, themes and icons
8. Base components and form system
9. Component and service catalogue
10. Application shell, commands and navigation
11. Data presentation: grid, lists, trees and paging
12. Dialogs, feedback, progress and errors
13. Windows, file and system integration
14. R2 feature modules
15. Cross-cutting requirements and quality assurance
16. Build, CI/CD, versioning and governance
17. Component Gallery, templates and adoption
18. Implementation plan and release gates
19. Risks and technical countermeasures
20. Overall acceptance
21. Future register and deliberate non-goals
22. Traceability matrix
23. Component families F01–F20
24. Document control and change log

# 1. Executive summary

The SASD UI Platform will be implemented as a modular monorepo for C# WinForms applications. The first production-ready release concentrates on a small, robust and designer-friendly component foundation. Microsoft WinForms remains the platform core. Krypton Standard Toolkit is evaluated in R0 as the first visual implementation. Third-party libraries are not copied into a large combined fork; they are consumed behind clearly defined SASD contracts or in explicitly named adapter packages.

The central technical decision is: **SASD components are created only where reusable behaviour, quality assurance or a stable contract provides concrete value.** Standard controls such as `Button`, `Label` and `TextBox` are not replaced by dozens of thin wrappers. Shared value is instead provided through theme, form-layout, command, validation and state layers. Dedicated controls are created especially for application shells, navigation, grid productivity, error feedback, busy and empty states, and recurring workbench layouts.

| Decision area | Definition |
| --- | --- |
| Technology baseline | .NET 8 (`net8.0-windows`) and Windows Forms. A current LTS is checked in CI, but R1 avoids unnecessary multi-targeting. |
| Visual foundation | Krypton pilot as the preferred theme and control foundation; a clean fallback to native WinForms controls remains possible. |
| Repository | One monorepo with separated NuGet packages, samples, tests, documentation and central version management. |
| Public API | SASD types and .NET BCL types. Third-party types appear only in explicitly identified adapter packages. |
| Persistence | Versioned JSON under `%LocalAppData%\SASD-GmbH\<Product>` with atomic writes, backup and migration. |
| Testing | xUnit, a UI Automation/FlaUI pilot, screenshot regression, DPI/accessibility matrices and resource checks. |
| Distribution | Private/internal NuGet packages first. Public open-source release follows only after R1 stabilisation and licence review. |

# 2. Relationship to the requirements specification and implementation objectives

The requirements specification limits the product line to Windows, C# and WinForms, requires compatibility with existing .NET 8 projects, and deliberately excludes WPF, ASPX/Web, Java, mobile platforms and large specialist components from the current scope. Those decisions are adopted unchanged.

Implementation is divided into four stages:

| Stage | Technical objective | Mandatory outcome |
| --- | --- | --- |
| R0 — decision pilot | Validate architecture and supplier choices | Krypton/native comparison, token model, base form, dialog, grid spike, DPI/designer/accessibility tests and licence inventory. |
| R1 — production foundation | Make real SASD applications migratable | F01–F11 and F19–F20; NuGet packages, Gallery, CRUD, workbench and utility templates. |
| R2 — feature modules | Add editor, dashboard and workbench capabilities | F12–F16 and major parts of F18; adapters instead of core dependencies. |
| R3 — optional modules | Address only proven additional needs | Ribbon, wizard and other advanced views; no automatic expansion into a full suite. |

Success is not measured by the number of controls. Success means that Prompt Manager, Mail Workbench, Notes and at least one utility application use the same packages without local copies of shared UI logic.

# 3. Technical constraints

| Area | Definition |
| --- | --- |
| Operating system | Windows 10 and Windows 11. 64-bit is the reference target. AnyCPU is allowed only if all native dependencies support it. |
| Framework | .NET 8 for R0/R1. A current .NET LTS is additionally built in CI. .NET Framework is not a new target. |
| IDE | Visual Studio 2022 or successor with WinForms Designer. Rider and CLI builds are supported, but are not the design-time reference. |
| Language | C# with nullable reference types, centrally chosen implicit usings and XML documentation files enabled. |
| UI thread | Every control access occurs on the UI thread. Asynchronous APIs use `Task`, `CancellationToken` and `IProgress<T>`. |
| Localisation | Resource-based German and English texts; culture-sensitive formatting through `CultureInfo`. |
| Configuration | UI state never contains secrets. Secrets stay in application-specific credential or secret services. |
| Designer | Public visual controls provide parameterless constructors and do not require external services at design time. |

R1 does not support x86-only legacy controls, non-reproducible GAC installations, global hooks, embedded browser runtimes other than the R2 WebView2 module, or controls that work in the Visual Studio Designer only after manual edits to generated designer code.

# 4. Architecture principles

- **Composition over inheritance:** Inheritance is limited to a small set of durable base classes. Behaviour is added through services, controllers, extenders and composition.
- **Contracts first:** Applications depend on SASD contracts, not on Krypton, ScottPlot, WebView2 or other vendor types.
- **Designer first:** Visual controls remain designer-instantiable. Generic visual controls are avoided; typed behaviour belongs in controllers.
- **Fail safe:** Persistence, theme, icon, clipboard or optional-module failures must not prevent application startup.
- **Secure by default:** File, shell and WebView operations deny unsafe actions until explicitly permitted.
- **One visual system per application:** An application uses exactly one primary visual theme implementation. ReaLTaiizor or AntdUI are not mixed with Krypton in one visible surface.
- **No megafork:** Forking is the final escalation level and requires both a maintenance plan and an exit plan.
- **Pragmatic APIs:** WinForms remains visible. The platform does not hide every property behind artificial abstractions.

## 4.1 Dependency rule

```text
SASD application
  -> Sasd.Ui.WinForms.* (public SASD API)
       -> Sasd.Ui.Core
       -> optional adapter packages
            -> Krypton / ScottPlot / ScintillaNET / WebView2 / other third-party library
```

The core never references adapter packages.

## 4.2 Third-party types in public APIs

Third-party types are prohibited in core public APIs. An exception is allowed inside an explicitly named adapter package when the adapter would otherwise be unusable. For example, `Sasd.Ui.WinForms.Charts.ScottPlot` may expose an intentionally vendor-specific advanced configuration hook, while `Sasd.Ui.WinForms.Shell` must not return a Krypton type.

# 5. Repository, solution and package structure

The project is maintained as the `SASD-UI-Platform` monorepo. Product code, adapters, samples, templates, tests, documentation and build infrastructure are separated.

```text
SASD-UI-Platform/
├─ src/
│  ├─ Sasd.Ui.Core/
│  ├─ Sasd.Ui.WinForms/
│  ├─ Sasd.Ui.WinForms.Theming/
│  ├─ Sasd.Ui.WinForms.Commands/
│  ├─ Sasd.Ui.WinForms.Shell/
│  ├─ Sasd.Ui.WinForms.Forms/
│  ├─ Sasd.Ui.WinForms.Data/
│  ├─ Sasd.Ui.WinForms.Dialogs/
│  ├─ Sasd.Ui.WinForms.Windows/
│  ├─ Sasd.Ui.WinForms.State/
│  ├─ Sasd.Ui.WinForms.Krypton/
│  └─ adapters/ ... R2 modules
├─ samples/
│  ├─ Sasd.Ui.ComponentGallery/
│  ├─ Sasd.Ui.Sample.Crud/
│  ├─ Sasd.Ui.Sample.Workbench/
│  ├─ Sasd.Ui.Sample.Utility/
│  └─ Sasd.Ui.Sample.Dashboard/
├─ templates/
├─ tests/
│  ├─ unit/
│  ├─ integration/
│  ├─ ui/
│  ├─ visual/
│  └─ architecture/
├─ docs/
├─ build/
├─ eng/
├─ Directory.Build.props
├─ Directory.Packages.props
├─ global.json
└─ SASD.Ui.Platform.sln
```

| Package/project | Dependencies | Responsibility | Release |
| --- | --- | --- | --- |
| `Sasd.Ui.Core` | No WinForms dependency | Design tokens, result/error models, shared contracts and versioning | R0/R1 |
| `Sasd.Ui.WinForms` | Core, System.Windows.Forms | Base classes, helpers, dispatcher, empty/busy/search/section components | R0/R1 |
| `Sasd.Ui.WinForms.Theming` | Core, WinForms, internal Krypton adapter | Themes, token mapping, icons and High Contrast | R0/R1 |
| `Sasd.Ui.WinForms.Commands` | Core, WinForms | Command model, shortcut management and bindings | R1 |
| `Sasd.Ui.WinForms.Shell` | WinForms, Theming, Commands, State | Shell, navigation, tabs, breadcrumb and status | R1 |
| `Sasd.Ui.WinForms.Forms` | WinForms, Theming | Field layout, binding and validation summary | R1 |
| `Sasd.Ui.WinForms.Data` | WinForms, State | Grid, lists, trees, search, filters and paging | R1 |
| `Sasd.Ui.WinForms.Dialogs` | WinForms, Core; Ookii optional internally | Dialog, error, notification and progress services | R1 |
| `Sasd.Ui.WinForms.Windows` | WinForms, Core | Files, clipboard, drag-and-drop, tray and safe shell calls | R1 |
| `Sasd.Ui.WinForms.State` | Core | JSON state store, migration, MRU and atomic files | R1 |
| `Sasd.Ui.WinForms.Krypton` | WinForms, Krypton | Concrete visual implementation and palette | R0/R1 |
| `Sasd.Ui.WinForms.Charts.ScottPlot` | WinForms, ScottPlot | Primary chart adapter | R2 |
| `Sasd.Ui.WinForms.Editors` | WinForms, Markdig, DiffPlex | Markdown and diff features | R2 |
| `Sasd.Ui.WinForms.Editors.Scintilla` | Editors, ScintillaNET | Code and configuration editor | R2 |
| `Sasd.Ui.WinForms.Media` | WinForms; selected image/QR/barcode dependencies | Images, QR codes and barcodes | R2 |
| `Sasd.Ui.Documents.PdfSharp` | Core, PDFsharp/MigraDoc | UI-independent PDF generation | R2 |
| `Sasd.Ui.WinForms.WebView2` | WinForms, WebView2 | Hardened embedded web host | R2 |
| `Sasd.Ui.WinForms.Docking.Krypton` | Shell, Krypton Docking/Workspace | Docking and layout persistence | R2 |
| `Sasd.Ui.WinForms.Testing` | Test projects | Test host, screenshots, UIA helpers and fakes | R0/R1 |
| `Sasd.Ui.WinForms.Templates` | Approved R1/R2 packages | `dotnet new`/Visual Studio starters and project templates | R1/R2 |

## 5.1 Packaging rules

- R1 must not begin with twenty public NuGet packages. Projects may already be separated internally, while early public packages remain few and cohesive.
- Initial packages: `Sasd.Ui.Core`, `Sasd.Ui.WinForms`, `Sasd.Ui.WinForms.Krypton`, `Sasd.Ui.WinForms.Data` and `Sasd.Ui.WinForms.Templates`.
- Dialog, shell, state and Windows projects may be bundled into `Sasd.Ui.WinForms` until their APIs stabilise.
- Every R2 specialist adapter is packaged separately so applications acquire only the heavy or native dependencies they use.

# 6. Public API, conventions and error contracts

## 6.1 Naming conventions

| Element | Convention | Example |
| --- | --- | --- |
| Namespaces | `Sasd.Ui...` | `Sasd.Ui.WinForms.Dialogs` |
| Controls | Prefix `Sasd` | `SasdDataGrid`, `SasdBusyOverlay` |
| Services | Interface plus Service | `IDialogService`, `SasdDialogService` |
| Options | `Options` suffix | `DialogOptions`, `GridOptions` |
| Results | `Result` suffix | `DialogResult<T>`, `FileSelectionResult` |
| Events | Standard .NET event pattern | `NavigationChanging`, `NavigationChanged` |
| Asynchronous members | `Async` suffix | `ShowProgressAsync` |

## 6.2 Result and error model

Services throw only for programming errors or unrecoverable internal states. Expected user and environment failures are returned as result objects.

```csharp
public sealed record UiOperationResult(
    bool Succeeded,
    string? UserMessage = null,
    string? TechnicalDetails = null,
    string? ErrorCode = null,
    Exception? Exception = null);

public sealed record UiOperationResult<T>(
    bool Succeeded,
    T? Value = default,
    string? UserMessage = null,
    string? TechnicalDetails = null,
    string? ErrorCode = null);
```

## 6.3 Binary and source compatibility

- Semantic versioning applies: patch for compatible fixes, minor for additive APIs, major for breaking changes.
- Obsolete APIs remain for at least one minor version with a migration note unless a security issue prevents this.
- A public API baseline is checked during build; accidental new public types and breaking changes fail CI.
- Every public type has XML documentation and at least one executable example in the Gallery or samples.

# 7. Design system, themes and icons

## 7.1 Design-token model

Design tokens are immutable C# records and can optionally be loaded from versioned JSON. Tokens use semantic names; components may not hard-code arbitrary hexadecimal colours or project-specific spacing values.

```csharp
public sealed record SasdThemeDefinition(
    string Id,
    int SchemaVersion,
    SasdColorTokens Colors,
    SasdTypographyTokens Typography,
    SasdSpacingTokens Spacing,
    SasdSizeTokens Sizes,
    SasdIconTheme Icons);
```

| Token group | Mandatory content |
| --- | --- |
| Colours | Background, Surface, SurfaceVariant, Primary, Secondary, Text, MutedText, Border, Focus, Success, Warning, Error, Info and Selection. |
| Typography | Default and monospaced fonts; Body, Caption, Label, Title and Heading; relative scaling rather than isolated absolute values. |
| Spacing | 2, 4, 8, 12, 16, 24 and 32 pixels at 100%; DPI scaling through WinForms. |
| Sizes | Minimum input/button heights, icon sizes, click targets and minimum dialog widths. |
| States | Normal, Hover, Pressed, Focused, Disabled, ReadOnly, Selected, Error and Warning. |

## 7.2 Theme service

`IThemeService` controls the active theme, system association and change notifications. Light and Dark can be changed at runtime. High Contrast is not imitated by a decorative SASD theme; Windows system colours are respected and decorative elements are reduced.

## 7.3 Krypton implementation

R0 implements token-to-Krypton palette mapping. The pilot passes when designer use, runtime switching, DPI, standard states, DataGrid and navigation work without unacceptable special handling. If it fails, the token/service model remains and R1 uses centrally configured native WinForms controls.

## 7.4 Icons

Icons are requested by semantic names such as `Save`, `Delete`, `Search` and `Warning`. `IIconService` returns DPI-correct images and caches them by theme, size and state. The icon set is maintained as a separately licensed asset. Icons are never copied from arbitrary applications. Important actions also expose text or an accessible name.

# 8. Base components and form system

## 8.1 No blanket primitive wrappers

Classes such as `SasdButton`, `SasdLabel` and `SasdTextBox` are not created merely to rename existing controls. Primitive controls are standardised through the visual implementation, `SasdFieldLayout`, validation adapters, extension methods and factories. Dedicated controls are justified when they compose multiple controls or reusable behaviour.

## 8.2 Form and view base classes

| Type | Responsibility | Explicitly excluded |
| --- | --- | --- |
| `SasdForm` | Theme, DPI, state key, icon, standard error boundary, help/about hooks and safe disposal order | Business logic, data access and global service locator |
| `SasdDialogForm` | Standard buttons, validation, minimum size, Enter/Escape and dirty-state confirmation | Arbitrary wizard logic |
| `SasdUserControl` | Theme/culture events, designer safety and lifecycle | Automatic dependency injection in the designer |

## 8.3 Form layout

`SasdFieldLayout` is based on `TableLayoutPanel`, uses `AutoSize`, semantic columns and DPI-aware spacing. A logical field contains label, input, help text and error text. Native or Krypton controls can be hosted. Narrow dialogs may switch to label-above-control through defined breakpoints rather than hand-positioned pixels.

## 8.4 Input types

| Input | Implementation | R1 acceptance |
| --- | --- | --- |
| Text | Single/multiline, `MaxLength`, read-only and placeholder/cue banner | Keyboard, copy/paste, errors and long German text work. |
| Masked | `MaskedTextBox` or Krypton equivalent behind a field adapter | Date, time, postal code and custom masks are validated. |
| Numeric | `NumericUpDown` adapter with `decimal?` binding and culture formatting | Min/max, null, step and decimal places are tested. |
| Date/time | Date-time adapter with optional null and range | Keyboard, picker and culture changes work. |
| Choice | ComboBox, CheckBox, RadioButton and Toggle with consistent enabled/read-only semantics | Binding and accessible name/state are correct. |
| File/folder | Button plus text display and `IFileDialogService`; no dialog code in the form | Filters, initial directory, multiple selection and cancellation are robust. |
| Auto-complete | Simple delayed provider in R1; multi-select/tags only after an R2 pilot | Delayed search never blocks the UI. |

## 8.5 Validation

Validation supports DataAnnotations, synchronous application rules and optional asynchronous checks. `SasdValidationCoordinator` associates errors with controls, updates `ErrorProvider` and `SasdValidationSummary`, and focuses the affected input when selected. User-facing validation messages never contain raw technical exceptions.

# 9. Component and service catalogue

An entry does not necessarily mean a dedicated visual class. Services and controllers are equal first-class parts of the platform.

| Release | Component/service | Purpose | Scope | Package |
| --- | --- | --- | --- | --- |
| R1 | `SasdForm` | Base for main and secondary windows | DPI, theme, icon, state, error boundary and standard shortcuts | `Sasd.Ui.WinForms` |
| R1 | `SasdDialogForm` | Base for modal dialogs | OK/Cancel, validation, minimum size, focus, Escape/Enter | `Sasd.Ui.WinForms` |
| R1 | `SasdUserControl` | Base for reusable views | Theme/localisation changes, designer support and disposal hooks | `Sasd.Ui.WinForms` |
| R1 | `SasdSectionPanel` | Semantic content section | Title, description, icon, collapsible body and status | `Sasd.Ui.WinForms` |
| R1 | `SasdFieldLayout` | Consistent form grid | Label, input, help, required marker, error and responsive columns | `Sasd.Ui.WinForms.Forms` |
| R1 | `SasdValidationSummary` | Form-level error overview | Error list, focus navigation, severity and accessibility | `Sasd.Ui.WinForms.Forms` |
| R1 | `SasdSearchBox` | Reusable search input | Debounce, clear, shortcut, search state and events | `Sasd.Ui.WinForms` |
| R1 | `SasdFilterBar` | Compact filter area | Filter chips, reset, saved filters and result count | `Sasd.Ui.WinForms.Data` |
| R1 | `SasdEmptyState` | No-result/first-use surface | Title, explanation, optional action and icon | `Sasd.Ui.WinForms` |
| R1 | `SasdBusyOverlay` | Blocking or partial busy state | Status, determinate/indeterminate progress, cancel and focus trap | `Sasd.Ui.WinForms` |
| R1 | `SasdShellForm` | Standard application shell | Navigation, command bar, workspace, status, error/update indication | `Sasd.Ui.WinForms.Shell` |
| R1 | `SasdNavigationView` | Page/area navigation | Hierarchy, groups, icons, collapse, keyboard and navigation result | `Sasd.Ui.WinForms.Shell` |
| R1 | `SasdBreadcrumb` | Context navigation | Segments, click navigation, ellipsis and accessibility | `Sasd.Ui.WinForms.Shell` |
| R1 | `SasdDocumentTabs` | Documents and work pages | Create, activate, close, dirty state and restoration | `Sasd.Ui.WinForms.Shell` |
| R1 | `SasdCommandBar` | Command surface | Commands, groups, overflow, shortcuts and state | `Sasd.Ui.WinForms.Commands` |
| R1 | `SasdStatusService` / `SasdStatusBar` | Application-wide status | Info/warning/error, progress, timestamp and priority | `Sasd.Ui.WinForms.Shell` |
| R1 | `SasdDataGrid` | Standard data table | Binding, sorting, search, filter, selection, editing, state and export | `Sasd.Ui.WinForms.Data` |
| R1 | `SasdGridController<T>` | Typed grid control logic | Column definitions, mapping, selection, commands and validation | `Sasd.Ui.WinForms.Data` |
| R1 | `SasdListView` | Lightweight list | Details/tiles, selection, context menu, empty state and virtual-mode pilot | `Sasd.Ui.WinForms.Data` |
| R1 | `SasdTreeView` | Hierarchical data | Lazy loading, error nodes, icons, selection and context actions | `Sasd.Ui.WinForms.Data` |
| R1 | `SasdDialogService` | Consistent modal interaction | Info, warning, error, confirmation, choice and custom dialogs | `Sasd.Ui.WinForms.Dialogs` |
| R1 | `SasdErrorDialog` | User-friendly error display | Summary, technical details, copy, correlation and log path | `Sasd.Ui.WinForms.Dialogs` |
| R1 | `SasdNotificationService` | Non-modal feedback | Toast/snackbar, actions, duration, queue and disable option | `Sasd.Ui.WinForms.Dialogs` |
| R1 | `SasdProgressDialog` | Long-running operation progress | `IProgress`, cancellation, details and error completion | `Sasd.Ui.WinForms.Dialogs` |
| R1 | `SasdFileDialogService` | File and folder dialogs | Open/save/folder, filters, multiple selection, last path and validation | `Sasd.Ui.WinForms.Windows` |
| R1 | `SasdClipboardService` | Fault-tolerant clipboard | Text, HTML, image, file list, bounded retries and error result | `Sasd.Ui.WinForms.Windows` |
| R1 | `SasdDragDropService` | Safe file drop areas | Allowed types, multiple files, visual feedback and validation | `Sasd.Ui.WinForms.Windows` |
| R1 | `SasdTrayService` | Notification-area integration | Menu, show/hide, explicit exit and state | `Sasd.Ui.WinForms.Windows` |
| R1 | `SasdShellIntegrationService` | Safe OS integration | Open approved URLs/files/folders with scheme validation | `Sasd.Ui.WinForms.Windows` |
| R1 | `SasdStateStore` | Versioned user-interface state | JSON, schema version, migration, atomic write, backup and reset | `Sasd.Ui.WinForms.State` |
| R1 | `SasdRecentItemsService` | Recent-item list | MRU, pin, validation, privacy and cleanup | `Sasd.Ui.WinForms.State` |
| R1 | `SasdUiDispatcher` | Safe UI-thread transition | Invoke/BeginInvoke/InvokeAsync, cancellation and disposal checks | `Sasd.Ui.WinForms` |
| R2 | `SasdChartView` | Technical charts | Line, bar, scatter, axes, legend, tooltip, zoom and export | `Sasd.Ui.WinForms.Charts.ScottPlot` |
| R2 | `SasdKpiCard` / `SasdSparkline` | Dashboard metrics | Value, trend, status and accessible text alternative | `Sasd.Ui.WinForms.Charts` |
| R2 | `SasdMarkdownEditor` | Markdown edit and preview | Search, toolbar, split preview and safe HTML | `Sasd.Ui.WinForms.Editors` |
| R2 | `SasdCodeEditor` | Code/configuration editing | Syntax, folding, lines, search/replace, markers and large files | `Sasd.Ui.WinForms.Editors.Scintilla` |
| R2 | `SasdDiffViewer` | Text comparison | Inline/side-by-side, navigation, whitespace options and export | `Sasd.Ui.WinForms.Editors` |
| R2 | `SasdImageViewer` | Raster image display | Zoom, pan, fit, rotate, background and keyboard | `Sasd.Ui.WinForms.Media` |
| R2 | `SasdBarcodeService` | QR/barcode adapter | Generate, optional scan, PNG/SVG/Bitmap and error correction | `Sasd.Ui.WinForms.Media` |
| R2 | `SasdPdfDocumentService` | Programmatic PDF output | Paragraphs, tables, images, header/footer, metadata and page numbers | `Sasd.Ui.Documents.PdfSharp` |
| R2 | `SasdWebViewHost` | Secure WebView2 host | Origin allow-list, navigation, downloads, script bridge and local resources | `Sasd.Ui.WinForms.WebView2` |
| R2 | `SasdDockWorkspace` | Docking/workspace module | Documents, tools, save/load layout and corrupted-layout reset | `Sasd.Ui.WinForms.Docking.Krypton` |
| R2 | `SasdPropertyEditor` | Property editing | Categories, descriptions, validation, read-only and custom editors | `Sasd.Ui.WinForms.Data.Advanced` |
| R2 | `SasdGridExtensions` | Advanced data functions | Column chooser, summaries, master-detail, CSV export and paging | `Sasd.Ui.WinForms.Data.Advanced` |
| R3 | `SasdWizard` | Guided multi-step workflows | Steps, validation, Back/Next, cancel and resume | `Sasd.Ui.WinForms.Wizard` |
| R3 | `SasdRibbonHost` | Optional ribbon | Command binding, tabs, groups, KeyTips and adaptive state | `Sasd.Ui.WinForms.Ribbon.Krypton` |

# 10. Application shell, commands and navigation

## 10.1 Shell

`SasdShellForm` exposes defined regions: header/command area, navigation, workspace, optional right/bottom auxiliary area and status. Applications configure the shell through `ShellOptions` and register pages/commands; they do not access Krypton controls directly.

## 10.2 Command system

The command system is WinForms-specific and does not require `System.Windows.Input.ICommand`. `IUiCommand` supports synchronous or asynchronous execution, `CanExecute`, visibility, checked state, icon, text, description and shortcut. Menus, context menus, command bars and navigation bind the same command instance. Failures go through the central error presenter.

```csharp
public interface IUiCommand
{
    string Id { get; }
    string Text { get; }
    Keys? Shortcut { get; }
    bool CanExecute(object? parameter = null);
    Task ExecuteAsync(object? parameter = null,
        CancellationToken cancellationToken = default);
    event EventHandler? StateChanged;
}
```

## 10.3 Navigation

Navigation uses stable page IDs and `INavigationService`. It can be cancelled, for example when an unsaved document blocks leaving. History is optional. Closed page instances must not remain referenced indefinitely.

## 10.4 Document tabs and dirty state

Document pages implement `IDocumentView` with ID, title, dirty state, save/close commands and an optional persistence key. Confirmation occurs only for dirty documents. Restoration stores safe references, not the document content itself; the application validates and reopens them.

## 10.5 Docking

Docking belongs to R2 and is abstracted by `IWorkspaceHost`. Krypton Docking/Workspace is the pilot. Layout files are versioned and discarded when incompatible or corrupted. A broken layout must never prevent startup.

# 11. Data presentation: grid, lists, trees and paging

## 11.1 `SasdDataGrid`

`SasdDataGrid` is a non-generic designer-friendly control. Typed column and domain configuration lives in `SasdGridController<T>`, avoiding designer problems with generic controls while retaining strongly typed configuration.

| Capability | R1 | R2 |
| --- | --- | --- |
| Binding | BindingSource, DataTable and IList | Asynchronous page source and virtual mode |
| Columns | Explicit definition, format, visibility and sorting | Column chooser, summaries and custom aggregates |
| Search | Central search across selected columns | Highlighting and saved searches |
| Filters | Contains, equals, begins with, range and empty | More complex combined filters without a universal visual query builder |
| Selection | Single/multiple, full row and selection commands | Master-detail |
| Editing | Optional edit, validation, commit/cancel and errors | Batch/optimistic concurrency remains application-specific |
| Persistence | Width, order, visibility, sort and filter | Profiles and saved views |
| Export | UTF-8 CSV for visible or all loaded rows | Further formats only through separate adapters |

## 11.2 Paging and large datasets

`IDataPageSource<T>` defines page size, sorting and filters without a database or ORM dependency. The grid core knows neither Entity Framework nor SQL. Requests are cancellable and only the latest active request may update the UI. R1 includes a paging pilot; R2 adds virtual mode.

## 11.3 Lists and trees

`SasdListView` and `SasdTreeView` are lighter alternatives to the grid. Tree nodes load lazily. Access failures are represented by understandable error nodes with a retry command. A folder tree is a TreeView configuration with `IFileSystemNodeProvider`, not a complete Explorer replacement.

## 11.4 Performance targets

| Scenario | Target on reference system | Measurement |
| --- | --- | --- |
| Grid with 10,000 loaded rows | First display ≤1.5 s; UI remains responsive | Warm run, Release build, three runs, median |
| Search/filter over 10,000 rows | Result ≤500 ms after debounce | In-memory reference data |
| Paging 100,000+ records | First page ≤1.5 s plus source latency; cancellation effective | Fake page source with controlled latency |
| Tree with 1,000 visible nodes | Expand/collapse without multi-second pauses | UI automation and stopwatch |
| Save/load state | ≤100 ms for typical shell/grid state | Local SSD |

These are release limits for the reference implementation, not guarantees for slow sources. Deviations require a measurement report and explicit acceptance.

# 12. Dialogs, feedback, progress and errors

## 12.1 Dialog service

`IDialogService` exposes typed methods for information, warning, error, confirmation and custom models. Applications do not choose between MessageBox, TaskDialog and SASD forms themselves. Ookii.Dialogs may be used internally without leaking its types.

## 12.2 Error presenter

User-facing text and technical details are separated. The dialog shows a concise recommendation, optional expandable details, error/correlation ID, copy action and log location. Stack traces never appear as the primary message. Secrets and complete mail or document contents are not copied automatically into display or logs.

## 12.3 Notifications

Toast/snackbar notifications are queued, expose severity and optional actions, and remain keyboard accessible. Time-critical or security-relevant failures are never represented only by a disappearing toast. Native desktop notifications are an optional R2 service and respect application settings.

## 12.4 Progress and busy state

Operations lasting roughly more than 500 ms should prepare busy feedback; from roughly 2 seconds visible progress or an indeterminate status is required. `SasdProgressDialog` receives an asynchronous function, `IProgress<ProgressInfo>` and `CancellationToken`. Cancellation must leave the application in a consistent state.

# 13. Windows, file and system integration

## 13.1 File and folder dialogs

`SasdFileDialogService` normalises filters, validates start paths and returns result objects. File extensions are not trusted as proof of content. Applications validate content and size before processing. Last-used paths are stored only when privacy settings permit it.

## 13.2 Clipboard

Clipboard access uses bounded retries because temporary locking is expected. HTML clipboard content is generated with the correct format. Clipboard content is never executed or trusted automatically.

## 13.3 Drag-and-drop

Drop zones define allowed formats, maximum item count, optional size limits and validation. Visual feedback distinguishes allowed, rejected and validating states. Network and inaccessible paths are checked asynchronously.

## 13.4 Tray

Tray operation is enabled only when explicitly configured. Close, minimise and exit behaviours are unambiguous. The service must never leave an invisible process that can only be terminated through Task Manager.

## 13.5 Safe shell integration

A central service opens URLs, files and folders. URI schemes are validated through an allow-list. Command lines are never composed from unvalidated user data. PowerShell or CMD execution is outside the UI core.

## 13.6 WebView2

R2 provides `SasdWebViewHost` with a locked-down default profile. External navigation, downloads, new windows, camera/microphone, clipboard, host objects and DevTools are disabled by default or require explicit policy. Local content is delivered through virtual hosts or safe resource mappings. The JavaScript bridge accepts versioned messages and validates origin and payload.

# 14. R2 feature modules

## 14.1 Charts and KPI

ScottPlot is the primary technical-chart implementation. `IChartView` receives SASD data series and presentation settings. Line, bar and scatter plots are mandatory. Pie/donut, gauges and sparklines are added only if they can remain visually consistent without a second visible suite. Every visual metric has a textual alternative for accessibility and export. LiveCharts2 remains an optional animated-dashboard adapter.

## 14.2 Markdown editor

The editor combines ScintillaNET or a suitable text control with Markdig. R2 preview preferably uses hardened WebView2. Raw HTML is disabled or sanitised by default. Toolbar commands insert Markdown; they do not attempt a full WYSIWYG editor.

## 14.3 Code/configuration editor

ScintillaNET is isolated in its own adapter package. Supported capabilities include syntax definitions for common SASD files, search/replace, line numbers, folding, markers, undo/redo and large text files. IntelliSense, language servers and debugger integration are excluded.

## 14.4 Diff viewer

DiffPlex or another permissively licensed library produces a vendor-neutral diff model. The WinForms view supports inline and side-by-side modes, change navigation, whitespace options and copying. Merge/conflict resolution is not part of the initial scope.

## 14.5 Image, QR and barcode

Cyotek ImageBox is piloted for image viewing; if integration fails, a smaller PictureBox extension is created. QRCoder generates QR codes and ZXing.Net handles general barcodes behind `IBarcodeService`. Bitmap lifecycles and GDI resources are tested strictly.

## 14.6 PDF

PDFsharp/MigraDoc is the default implementation for programmatic PDF documents. The API resides in a UI-independent package. Text, tables, images, header/footer, page numbers and metadata are supported. FastReport OSS is evaluated separately only for band-oriented reports. QuestPDF remains excluded until a documented licence and business decision. A visual report designer is not part of the project.

## 14.7 Property editor and advanced data views

`SasdPropertyEditor` wraps the standard PropertyGrid or an approved alternative. Public metadata uses .NET attributes and SASD editor contracts, not vendor-specific attributes. Column chooser, summaries and master-detail extend `SasdDataGrid` in an optional package.

# 15. Cross-cutting requirements and quality assurance

## 15.1 DPI and multi-monitor

- Templates centrally configure the application manifest and High-DPI mode.
- Test matrix: 100%, 125%, 150% and 200%, including movement between monitors with different scaling.
- Layout panels are preferred over manual pixel positions.
- Icons are produced at the target size by `IIconService`; tiny raster images are not enlarged to 200%.
- Visual baselines are maintained per DPI level.

## 15.2 Keyboard and accessibility

- Every interactive control exposes meaningful accessible name, role, value/state and, where required, description.
- Themes may not hide the focus indicator.
- Tab order follows visual and business order.
- Standard dialogs use Enter for the primary action and Escape for cancel when appropriate.
- Automated UIA checks are supplemented by manual Accessibility Insights for Windows reviews.
- Colour is never the only carrier of information; status also uses text, icon or pattern.

## 15.3 Localisation

Shared texts reside in `Sasd.Ui.*.Resources`; application texts remain in applications. Gallery and samples are complete in English and German. Pseudo-localisation and artificially long labels test layout reserves. Dates, numbers and currency use the active culture; persisted technical values use invariant formats.

## 15.4 Logging and telemetry

The platform provides lightweight hooks through `Microsoft.Extensions.Logging.Abstractions` or a minimal equivalent. No telemetry provider is mandatory. Events may include component, operation, duration and outcome, but not secrets or complete user content.

## 15.5 Resource and leak tests

| Check | Release criterion |
| --- | --- |
| Window cycle | After 100 open/close cycles, GDI/USER handles do not grow continuously; any residual delta is documented and justified. |
| Event subscriptions | Closed views become collectable; global services do not retain them accidentally. |
| Images/icons | Dynamically created bitmaps are disposed or deliberately cached. |
| WebView2/editor | R2 modules close processes/handles and document lifecycle. |

## 15.6 Test pyramid

| Layer | Tool/approach | Coverage |
| --- | --- | --- |
| Unit | xUnit | Tokens, commands, state migration, filters, result objects and validators |
| Architecture | Reflection/architecture tests | Dependency direction, third-party public types and namespace rules |
| Integration | xUnit with temporary directories/test hosts | State, file-dialog abstraction, clipboard fakes, PDF and barcode |
| UI automation | FlaUI/UIA3 pilot with stable AutomationIds | Navigation, dialogs, core grid flows and keyboard |
| Visual regression | Screenshot harness and tolerance comparison | Themes, DPI, states and long text |
| Manual | Checklists and Accessibility Insights | Designer, High Contrast, multi-monitor and screen-reader-oriented review |

## 15.7 Definition of Done

- Requirement and release assignment are documented.
- Public API has XML comments and an example.
- Unit/integration tests exist, plus a Gallery page for visual components.
- DPI, keyboard, accessibility and localisation checks pass.
- No unapproved licence or transitive dependency is introduced.
- No new compiler warnings; nullable analysis has no unjustified suppression.
- Changelog, known limitations and migration notes are updated.

# 16. Build, CI/CD, versioning and governance

## 16.1 Build

`Directory.Build.props` enables nullable analysis, XML documentation, deterministic builds, analysers and shared metadata. `Directory.Packages.props` centrally controls versions. `global.json` fixes the SDK family with an approved roll-forward policy. Release builds originate from a clean checkout.

## 16.2 CI pipeline

| Stage | Actions |
| --- | --- |
| Validate | Formatting/style, locked restore, licence/vulnerability scan and architecture tests |
| Build | Debug and Release for `net8.0-windows`; additional LTS build when supported |
| Test | Unit, integration and selected UI automation |
| Visual | Gallery screenshot matrix on a dedicated Windows runner |
| Pack | NuGet, symbols, XML docs, SBOM and `THIRD-PARTY-NOTICES` |
| Publish | Versioned internal feed and release archive after manual approval |

## 16.3 Version and branch model

`main` remains releasable. Feature branches are merged through pull requests. Releases are tagged (`v0.1.0`). Product versions remain 0.x until APIs stabilise; 1.0 introduces stricter compatibility. A permanent `develop` branch is not required.

## 16.4 Licence and fork governance

Every new dependency needs a short ADR covering licence, activity, alternatives, public-API leakage and exit strategy. Forks are registered in `forks/registry.yml` and compared with upstream monthly. GPL or unclear packages do not enter production packages without explicit legal and business approval.

# 17. Component Gallery, templates and adoption

## 17.1 Component Gallery

The Gallery is an executable specification, not merely a marketing demo. Each page demonstrates applicable Normal, Hover, Focus, Disabled, ReadOnly, Busy, Empty, Error and High-Contrast states. It also includes the example code, accessibility/DPI notes, known limitations and related requirement IDs.

## 17.2 CRUD template

Demonstrates navigation, list/grid, search, filter, form layout, validation, dirty state, save/delete, errors, state persistence and CSV export. The first data source is in-memory or SQLite for demonstration, while the UI remains independent of SQLite.

## 17.3 Workbench template

Demonstrates page/document tabs, navigation, folder tree, editor area, properties/details, commands, status, recent files and later docking. It is the starting point for Mail Workbench and Notes.

## 17.4 Utility and dashboard templates

The utility template shows tray integration, settings, progress, logging and safe file operations. The R2 dashboard combines filters, KPI cards, charts and saved views without a visual dashboard designer.

## 17.5 Adoption in existing SASD projects

| Pilot application | First adoption | Deferred from first step |
| --- | --- | --- |
| SASD Prompt Manager | Theme, form layout, dialogs, grid state, search/filter and error handling | Ribbon, docking and dashboard |
| SASD Mail Workbench | Shell, navigation, document tabs, status, file dialogs and progress | Full docking, HTML host and advanced editor |
| SASD Notes | Workbench shell, tree, recent files, Markdown/code editor pilot | Visual graph and full plugin UI |
| SASD utility application | Base form, settings, progress, tray and safe shell integration | Data-heavy feature modules |

Migration follows a strangler approach: shared services and one self-contained surface are migrated first. Existing applications remain buildable at every step.

# 18. Implementation plan and release gates

| Gate | Mandatory evidence |
| --- | --- |
| R0.1 Architecture | Dependency tests, public contracts, package proposal and approved ADRs |
| R0.2 Visual pilot | Native/Krypton comparison, light/dark/high contrast, designer and DPI evidence |
| R0.3 Interaction pilot | Shell, dialog and grid spike; keyboard and UIA smoke tests |
| R1 Alpha | F01–F11 implementation complete enough for Gallery and one sample |
| R1 Beta | Prompt Manager pilot migrated; API baseline and migration notes available |
| R1 Release | All MUST requirements accepted, packages signed/published internally and SBOM complete |
| R2 Release | Each adapter independently accepted; no feature forces another adapter into an application |
| R3 Release | Only after documented application demand and separate approval |

## 18.1 R0 technology decisions

R0 must produce explicit decisions for Krypton, native fallback, Ookii dialogs, grid strategy, UI automation framework, screenshot tooling, icon set and licence handling. A comparison table records functionality, designer behaviour, DPI, accessibility, activity, licence, package size and exit cost.

# 19. Risks and technical countermeasures

| Risk | Countermeasure |
| --- | --- |
| Wrapper explosion | Require a reusable-behaviour justification for every public visual type. |
| Krypton limitations or stagnation | Stable token/service contracts, native fallback and isolated Krypton package. |
| Designer instability | Parameterless constructors, design-mode guards, Gallery and Visual Studio smoke tests. |
| DataGrid scope growth | Fixed R1 boundary, typed controller, optional advanced package and no spreadsheet goal. |
| Third-party licence drift | Central inventory, lock files, notices, SBOM and release review. |
| Accessibility regressions | UIA checks, manual review, visible focus and High-Contrast test matrix. |
| UI-thread deadlocks | Async guidelines, dispatcher, cancellation and analyzers/review checklist. |
| Resource leaks | Window-cycle, handle and GC tests; strict bitmap/event ownership. |
| Platform dilution | Future register; separate projects for WPF, Web/ASPX and Java. |
| Excessive package fragmentation | Separate repository projects but few public R1 packages. |

# 20. Overall acceptance

R1 is accepted only when:

1. all MUST requirements from the requirements specification are implemented or have an explicitly accepted deviation;
2. Component Gallery and CRUD, workbench and utility samples run from a clean clone;
3. Prompt Manager and at least one further real SASD application consume packaged components;
4. public APIs have XML documentation, examples and compatibility baselines;
5. DPI, keyboard, localisation, accessibility and resource test matrices pass;
6. licence inventory, notices, SBOM and dependency scan are complete;
7. setup, migration, support and known-limitations documentation is available in English and German;
8. no optional R2/R3 dependency is required by the R1 core.

# 21. Future register and deliberate non-goals

The following are documented but excluded from current implementation:

- WPF component line after WinForms R1.
- WinUI 3 and Avalonia comparison pilots after WPF foundations.
- Modern ASP.NET Core/Blazor Web components as a separate product line.
- ASP.NET Web Forms/ASPX compatibility project, using AJAX Control Toolkit only as historical inspiration.
- Java business UI/OpenXava project.
- Android, iOS, macOS and Linux desktop implementations.
- Pivot/OLAP engine, spreadsheet, Office editor, visual report/dashboard designer, complex scheduler/Gantt, 3D rendering, GIS suite, BPMN designer and complete file explorer.

# 22. Traceability matrix: requirements to implementation

Priority terms are rendered in English while IDs remain unchanged: **MUST**, **SHOULD**, **MAY**, **DEFERRED**.

| Requirement range | Priority | Implemented in | Technical area | Release |
| --- | --- | --- | --- | --- |
| LH-ZIE-001…006 | MUST | Chapters 1–5, 15–18 | Product objectives, architecture, delivery and adoption | R0/R1 |
| LH-GRU-001…005 | MUST | Chapters 7–8 | Design system and base components | R1 |
| LH-GRU-006 | SHOULD | Chapters 7–8 | Extended base-component behaviour | R2/R3 |
| LH-INP-001…008 | MUST | Chapters 8–9 | Form layout, inputs and validation | R1 |
| LH-INP-009 | SHOULD | Chapters 8–9 | Advanced input behaviour | R2/R3 |
| LH-INP-010 | MAY | Chapters 8–9 | Optional specialised input | R2/R3 |
| LH-NAV-001…005 | MUST | Chapter 10 | Shell, commands, navigation and workspace | R1 |
| LH-NAV-006 | SHOULD | Chapter 10 | Docking/workspace | R2 |
| LH-NAV-007 | MAY | Chapter 10 | Ribbon | R3 |
| LH-DAT-001…003 | MUST | Chapter 11 | Grid, lists, trees, paging and export | R1 |
| LH-DAT-004 | MUST | Chapter 11 | Large-data strategy | R1/R2 |
| LH-DAT-005…006 | MUST | Chapter 11 | State and export | R1 |
| LH-DAT-007…008 | SHOULD | Chapter 11 | Advanced data functions | R2/R3 |
| LH-DAT-009 | DEFERRED | Chapter 21 | Spreadsheet/pivot class | Future register |
| LH-FED-001…005 | MUST | Chapter 12 | Dialogs, errors, notifications and progress | R1 |
| LH-FED-006 | SHOULD | Chapter 12 | Native desktop notification | R2/R3 |
| LH-SYS-001…004 | MUST | Chapter 13 | Windows, file, clipboard, tray and shell integration | R1 |
| LH-SYS-005 | SHOULD | Chapter 13 | Extended Windows integration | R2/R3 |
| LH-SYS-006 | SHOULD | Chapter 13 | WebView2 | R2 |
| LH-SYS-007 | MAY | Chapter 13 | Optional system integration | R2/R3 |
| LH-DOC-001…006 | SHOULD | Chapter 14 | Editors, diff, PDF, image and barcode | R2 |
| LH-DOC-007 | DEFERRED | Chapter 21 | Full office/report designer | Future register |
| LH-VIZ-001…002 | SHOULD | Chapters 14 and 17 | Charts and KPI | R2 |
| LH-VIZ-003 | MAY | Chapters 14 and 17 | Optional visualisation | R2 |
| LH-VIZ-004 | SHOULD | Chapters 14 and 17 | Dashboard sample | R2 |
| LH-QUL-001…008 | MUST | Chapters 6 and 15 | DPI, accessibility, security, testing and API quality | R1 |
| LH-QUL-009 | SHOULD | Chapters 6 and 15 | Extended quality measures | R2/R3 |
| LH-QUL-010 | MUST | Chapters 6 and 15 | Documentation and maintainability | R1 |
| LH-LIE-001…003 | MUST | Chapters 16–17 | Delivery, Gallery, templates and NuGet | R1 |
| LH-LIE-004 | SHOULD | Chapters 16–17 | Extended delivery assets | R2/R3 |
| LH-LIE-005…006 | MUST | Chapters 16–17 | Documentation and release evidence | R1 |

The full one-row-per-ID matrix remains authoritative in the German source and in `02-requirements-specification.md`; this grouped view preserves all 83 requirement ranges without changing their allocation.

# 23. Detailed appendix: component families F01–F20

| ID | Family | Priority | Release | Technical implementation | Reference basis |
| --- | --- | --- | --- | --- | --- |
| F01 | Base components and forms | MUST | R1 | `SasdForm`, `SasdDialogForm`, `SasdUserControl`; primitives standardised through theme/layout rather than wrapper proliferation | Microsoft WinForms; SASD façade; Krypton visual implementation where approved |
| F02 | Design tokens and themes | MUST | R1 | `SasdThemeDefinition`, `IThemeService`, Krypton palette, icon service and Light/Dark/High Contrast | SASD tokens; Krypton palettes first |
| F03 | Application shell | MUST | R1 | `SasdShellForm` with regions, navigation, workspace and status | SASD shell on WinForms/Krypton |
| F04 | Menus and commands | MUST | R1 | `IUiCommand`, command manager, menu/toolbar/context bindings and shortcuts | WinForms/Krypton plus SASD command system |
| F05 | Navigation and layout | MUST | R1 | Navigation view, breadcrumb, document tabs, split/section layouts and status bar | WinForms/Krypton |
| F06 | Form layout and validation | MUST | R1 | Field layout, validation coordinator/summary, DataAnnotations and custom rules | SASD layout/validation layer |
| F07 | DataGrid and data tables | MUST | R1/R2 | `SasdDataGrid`, `SasdGridController<T>`, search/filter/state, paging/virtualisation and CSV | DataGridView/Krypton DataGridView with selective extensions |
| F08 | Lists and trees | MUST | R1/R2 | `SasdListView`, `SasdTreeView`, lazy loading and folder provider | WinForms/Krypton; ObjectListView only after pilot |
| F09 | Dialogs and feedback | MUST | R1 | Dialog, error, notification and progress services, busy overlay and tooltips | SASD services; Ookii for system dialogs |
| F10 | File and system integration | MUST | R1 | Dialogs, drag/drop, clipboard, recent items, tray and safe shell services | Windows APIs, Ookii and SASD services |
| F11 | State and user settings | MUST | R1 | JSON state store, schema version, migration, backup, reset and MRU | SASD state persistence |
| F12 | Charts and KPI | SHOULD | R2 | ScottPlot adapter, KPI cards and textual alternatives; optional LiveCharts | ScottPlot primary, LiveCharts2 for dashboard cases |
| F13 | Technical-content editors | SHOULD | R2 | Markdown/Markdig, code/ScintillaNET and diff/DiffPlex in separate packages | ScintillaNET, Markdig, DiffPlex and optional WebView2 preview |
| F14 | Image, barcode and PDF output | SHOULD | R2 | ImageBox pilot, QRCoder/ZXing and PDFsharp/MigraDoc | Selected permissive libraries |
| F15 | WebView host | SHOULD | R2 | Hardened WebView2 host with allow-list and versioned message bridge | Microsoft WebView2 |
| F16 | Docking and workspace | SHOULD | R2 | `IWorkspaceHost` with Krypton Docking/Workspace | Krypton after pilot |
| F17 | Ribbon and wizards | MAY | R3 | Wizard contract and optional Krypton Ribbon | Krypton Ribbon and SASD wizard contract |
| F18 | PropertyGrid and advanced data views | SHOULD | R2/R3 | Property editor, column chooser, summaries and master-detail | Native/Krypton plus adapters |
| F19 | Component Gallery and templates | MUST | R1/R2 | Gallery, CRUD, workbench, utility and later dashboard/wizard templates | SASD reference applications |
| F20 | Quality, tests and documentation | MUST | R1+ | Test pyramid, DPI/accessibility/localisation, API docs, CI, SBOM and releases | SASD quality standard |

# 24. Document control and change log

| Version | Date | Status | Change |
| --- | --- | --- | --- |
| 0.1 | 23 July 2026 | Draft | Initial functional specification derived from requirements v0.1; architecture, packages, components, interfaces, quality, releases and traceability defined. |

**End of functional specification**
