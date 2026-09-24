# SASD UI Platform – Requirements Specification for C# WinForms Components

**Requirements definition for a reusable Windows desktop component library**

- **Version:** 0.1
- **As of:** July 23, 2026
- **Status:** Draft for subject-matter review
- **Client/Need owner:** SASD-GmbH
- **Product line:** Windows Desktop / C# / WinForms
- **Basis:** `01-component-catalog.md`

> This requirements specification describes the **what and why** of the requested WinForms component platform. Concrete class structures, package names, internal architecture, and implementation details are defined later in the functional specification. Technical candidates are named here only where they represent an important constraint or evaluation basis.

## 1. Executive Summary

SASD will initially develop a clearly bounded, reusable component platform for C# WinForms applications. The objective is not to compete comprehensively with DevExpress, Telerik, or Syncfusion, but to provide a reliable core for the application types that recur in SASD work: administrative applications, technical workbenches, editors, small system utilities, and monitoring interfaces.

The first stage focuses on foundation components, themes, forms, navigation, DataGrid support, dialogs, file and Windows integration, state management, accessibility, High DPI, documentation, and reference applications. Specialist components such as charts, Markdown/code editors, WebView2, image viewing, and PDF output follow in a second stage. Pivot/OLAP, spreadsheets, Office editors, report/dashboard designers, complex scheduling, Gantt, 3D, mobile, and cross-platform interfaces are deliberately deferred.

## 2. Current Situation

SASD develops several Windows desktop applications with WinForms. Recurring needs—consistent forms, dialogs, navigation, data tables, themes, error messages, settings, and file operations—are currently solved separately in each project. The UI component catalog identified a broad range of commercial and open-source providers. For practical implementation, that breadth must now be reduced to a maintainable product line.

The component platform will curate and encapsulate existing open-source libraries instead of merging their source code indiscriminately. Microsoft WinForms is the platform core; Krypton is the preferred first pilot for the visual foundation. Other libraries are considered as specialist adapters or comparison candidates.

## 3. Target Groups and Typical Usage Scenarios

| Target Group | Need | Typical Applications |
| --- | --- | --- |
| SASD developers | Rapid, consistent creation and maintenance of interfaces | All SASD WinForms projects |
| Administrators and technical users | Clear data views, safe file operations, logs, settings, and status | Mail Workbench, Notes, Secret Manager, system tools |
| Business users | Understandable forms, search, filtering, validation, and export | Prompt Manager, Training Control, task and administrative applications |
| Maintenance and support | Reproducible error presentation, diagnostic information, consistent versions | All production applications |

Typical reference scenarios are a CRUD/administrative application, a document-oriented workbench, a technical editor, a small tray/system utility, and a monitoring/dashboard application.

## 4. Scope and Deliberate Boundaries

### 4.1 Included in This Requirements Specification

- Windows desktop components for C# and WinForms
- reusable components, services, templates, and quality standards
- integration of selected open-source components through stable SASD contracts
- NuGet packages, Component Gallery, reference applications, tests, and documentation
- compatibility with existing .NET 8 WinForms projects

### 4.2 Excluded

- WPF, WinUI 3, Avalonia, or other alternative desktop implementations
- ASP.NET Core/Blazor, ASP.NET Web Forms/ASPX, or Java web interfaces
- Android, iOS, macOS, or Linux desktop
- a complete DevExpress/Telerik/Syncfusion reimplementation
- Pivot/OLAP, spreadsheet, Office/DOCX editor, report/dashboard/diagram designer, PDF editor, and 3D visualization
- complex resource scheduling, complete project-management Gantt, or BPMN workflow designer

## 5. Priorities and Releases

| Priority | Meaning |
| --- | --- |
| **MUST** | Mandatory for the first production-usable version. |
| **SHOULD** | High value; add after the foundation unless a serious risk exists. |
| **MAY** | Add only with demonstrated need or very low additional effort. |
| **DEFERRED** | Deliberately outside the current WinForms project; retained in the future register. |

| Stage | Goal | Content |
| --- | --- | --- |
| **R0 – Decision pilot** | Technology and licensing decision | WinForms/Krypton pilot, design tokens, BaseForm, dialogs, DataGrid spike, DPI/accessibility review. |
| **R1 – Usable foundation** | First productive use | F01–F11 and F19–F20; CRUD, workbench, and utility references; NuGet, Gallery, documentation. |
| **R2 – Specialist extensions** | Tools and dashboards | Charts, technical editors, WebView2, image/PDF/QR, docking, advanced data functions. |
| **R3 – Optional modules** | Only with demonstrated need | Ribbon, Wizard, additional specialist views; preparation of a WPF requirements specification. |

## 6. Selected Component Families

| ID | Component Family | Scope | Priority | Release | Expected Foundation |
| --- | --- | --- | :---: | :---: | --- |
| F01 | **Foundation components and forms** | Buttons, labels, text, numeric, date and selection fields, groups, panels, standard states | MUST | R1 | Microsoft WinForms, preferably through a SASD facade; visually Krypton where appropriate |
| F02 | **Design tokens and themes** | Colors, typography, spacing, radii, icons, Light/Dark, High Contrast | MUST | R1 | SASD-owned tokens; Krypton palettes as the first implementation |
| F03 | **Application Shell** | Main window, navigation, workspace, status and command surfaces | MUST | R1 | SASD Shell on WinForms/Krypton |
| F04 | **Menus and commands** | Main menu, context menu, toolbar/CommandBar, shortcuts, enabled states | MUST | R1 | WinForms/Krypton plus SASD Command System |
| F05 | **Navigation and layout** | Sidebar, Navigation View, tabs, splitters, accordion, TreeView, breadcrumb, StatusBar | MUST | R1 | WinForms/Krypton |
| F06 | **Form layout and validation** | Consistent field layout, required fields, inline errors, validation summary | MUST | R1 | SASD layout and validation layer |
| F07 | **DataGrid and data tables** | Display, editing, sorting, filtering, search, selection, column state, virtualization | MUST | R1/R2 | DataGridView/Krypton DataGridView; selective extensions |
| F08 | **Lists and trees** | ListView/ObjectList, TreeView, Folder Tree, large and virtual lists | MUST | R1/R2 | WinForms/Krypton; ObjectListView only after a pilot if needed |
| F09 | **Dialogs and feedback** | Messages, modal dialogs, toasts, tooltips, progress, busy overlay, error details | MUST | R1 | SASD dialog/notification services; Ookii for system dialogs |
| F10 | **File and system integration** | File/folder selection, drag-and-drop, clipboard, recent files, tray, safe shell integration | MUST | R1 | Windows APIs, Ookii.Dialogs, SASD services |
| F11 | **State and user settings** | Persistent windows, splitters, columns, filters, theme, and MRU | MUST | R1 | SASD state persistence |
| F12 | **Charts and KPI displays** | Line, bar, scatter, pie, gauge, sparkline, simple heatmap | SHOULD | R2 | ScottPlot primarily; LiveCharts2 for dashboard scenarios |
| F13 | **Technical content editors** | Markdown, code and diff views with search, syntax, and large-file support | SHOULD | R2 | ScintillaNET, Markdig, DiffPlex, possibly WebView2 for preview |
| F14 | **Image, barcode, and PDF output** | Image viewing, QR/barcode, programmatic PDF creation and export | SHOULD | R2 | Cyotek ImageBox, QRCoder/ZXing, PDFsharp/MigraDoc or reviewed alternative |
| F15 | **WebView host** | Controlled embedding of local or trusted web content | SHOULD | R2 | Microsoft WebView2 |
| F16 | **Docking and workspace** | Document windows, tool windows, layout storage and restoration | SHOULD | R2 | Krypton Docking/Workspace after pilot |
| F17 | **Ribbon and assistants** | Ribbon for complex applications; stepper/Wizard for guided workflows | MAY | R3 | Krypton Ribbon; SASD Wizard contract |
| F18 | **PropertyGrid and advanced data views** | Property editing, master-detail, summaries, column chooser, exports | SHOULD | R2/R3 | Krypton/standard controls; adapters |
| F19 | **Component Gallery and templates** | Demo of all states and starters for CRUD, workbench, dashboard, and utility | MUST | R1/R2 | SASD-owned reference applications |
| F20 | **Quality, tests, and documentation** | DPI, accessibility, keyboard, localization, visual regression, API docs, examples | MUST | R1+ | SASD test and documentation standard |

## 7. Functional and Non-Functional Requirements

### 7.1 Goals, Benefits, and Product Boundaries

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-ZIE-001 | **MUST** | The SASD UI Platform must provide a reusable component foundation for new and existing SASD C# WinForms applications. It must standardize recurring UI tasks and accelerate small to medium administrative, utility, editor, and monitoring applications. | At least three different SASD reference applications can use the same packages and components without project-specific copies. |
| LH-ZIE-002 | **MUST** | The first product line is exclusively for Windows desktop applications based on WinForms. WPF, web/ASPX, Java, Android, iOS, macOS, and Linux desktop are not deliverables of this specification. | Builds, samples, and documentation contain only the defined WinForms product line; other platforms appear only in the future register. |
| LH-ZIE-003 | **MUST** | The platform must not attempt a complete reimplementation of DevExpress, Telerik, or Syncfusion. It must create a pragmatic, well-maintained core for common SASD use cases. | The approved component plan contains no unproven major projects such as spreadsheet, Pivot/OLAP, Office, or report designers. |
| LH-ZIE-004 | **MUST** | Applications should program against SASD-owned contracts, services, and foundation components wherever practical so that individual third-party libraries remain replaceable. | Public SASD APIs return no unnecessary vendor-specific types; documented exceptions include justification. |
| LH-ZIE-005 | **SHOULD** | The platform should treat Prompt Manager, Mail Workbench, Notes, TaskHost, Desktop Secret Manager, Training Control, and other SASD tools as real need owners. | At least one concrete use case from a SASD project is documented for every R1/R2 component family. |

### 7.2 Technical and Organizational Framework

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-RHM-001 | **MUST** | Components must be usable at least in existing .NET 8 Windows projects. A parallel compatibility check with a current .NET LTS release is planned without delaying the first delivery through unnecessary multi-targeting. | A .NET 8 sample builds and starts reproducibly; supported target frameworks are stated clearly in package documentation. |
| LH-RHM-002 | **MUST** | Development and design-time use must work with Visual Studio and the WinForms Designer. Designer defects must not be compensated by manual changes to generated designer code. | Every visual core component can be placed, configured, saved, closed, and reopened in the designer without errors or lost settings. |
| LH-RHM-003 | **MUST** | The product line must be deliverable as versioned NuGet packages, source repository, sample applications, and documentation. | A clean installation into an empty reference solution is possible from a documented guide. |
| LH-RHM-004 | **MUST** | Permissive open-source licenses are preferred. GPL, unclear community licenses, source-available models, or commercial runtime bindings require explicit approval. | Every direct and transitive dependency has a license, origin, version, and usage decision; `THIRD-PARTY-NOTICES` and an SBOM can be generated. |
| LH-RHM-005 | **MUST** | A mega-fork of multiple component libraries is excluded. The standard sequence is normal dependency, SASD adapter, upstream contribution, temporary patch fork, and permanent fork only as a last resort. | Every fork has a documented reason, upstream synchronization plan, maintenance owner, and exit plan. |
| LH-RHM-006 | **SHOULD** | Commercial suites may be used as functional benchmarks and, with a demonstrated business case, as replaceable specialist implementations. | A commercial component can be removed or replaced without redesigning the entire SASD core. |

### 7.3 Foundation System, Design, and Base Components

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-GRU-001 | **MUST** | A consistent SASD design system must define named design tokens for colors, typography, spacing, sizes, radii, lines, focus, and semantic status colors. | Every R1 component obtains its visual foundation from one centrally documented token source; applications contain no arbitrary duplicate colors or spacing values. |
| LH-GRU-002 | **MUST** | Light, Dark, and Windows High Contrast must be supported. Light/Dark switching should be possible at runtime and persisted. | The Component Gallery shows all core components in all three modes; text, focus, and status remain readable. |
| LH-GRU-003 | **MUST** | A consistent icon system must cover standard actions such as New, Open, Save, Delete, Search, Filter, Refresh, Export, Settings, Help, and Error. | Icons are scalable or available at suitable DPI sizes, have semantic names, and are not loaded from application code through raw file paths. |
| LH-GRU-004 | **MUST** | Foundation components must include or consistently integrate at least Form, UserControl, Panel, GroupBox, Label, LinkLabel, Button, SplitButton, Separator, and status display. | Normal, Hover, Focus, Disabled, Error, and where relevant Busy states exist in the Gallery for every foundation component. |
| LH-GRU-005 | **MUST** | Forms and controls must use consistent minimum sizes, padding, tab order, default buttons, and cancel behavior. | A new form can be created without project-specific layout conventions and passes keyboard review. |
| LH-GRU-006 | **SHOULD** | Krypton Standard Toolkit should be evaluated as the first visual implementation foundation. AntdUI and ReaLTaiizor remain comparison or idea sources and must not be mixed uncontrolled with a second visible theme system. | The pilot decision documents designer, DPI, accessibility, maintenance, and licensing results; exactly one visible primary theme system is defined per application. |

### 7.4 Forms and Input Components

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-INP-001 | **MUST** | Text fields must support single- and multi-line input, placeholder or helper text, ReadOnly, MaxLength, selection, clipboard, and unambiguous error states. | Every state is usable by mouse and keyboard; validation errors are visible and named for assistive technologies. |
| LH-INP-002 | **MUST** | Masked input must support common formats such as date, time, telephone, postal code, and custom masks without silently accepting invalid characters. | At least five documented mask scenarios have automated validation tests. |
| LH-INP-003 | **MUST** | Numeric input must support integer and decimal values, minimum/maximum, step, culture format, null, and optional unit. | German and English numeric formats are entered, displayed, and validated correctly. |
| LH-INP-004 | **MUST** | Date and time input must support keyboard input, picker, null, minimum/maximum, and culture-specific display. | Date and time can be entered and changed completely without a mouse; boundary violations are reported clearly. |
| LH-INP-005 | **MUST** | ComboBox, CheckBox, RadioButton, and Toggle must provide consistent binding, Disabled/ReadOnly states, labels, and keyboard operation. | Sample forms demonstrate binding and validation for every selection component. |
| LH-INP-006 | **MUST** | File and folder selection must support modern Windows dialogs, filters, initial directory, multi-selection, safe error handling, and cancellation. | Dialogs handle long paths, missing paths, network paths, and user cancellation without losing exceptions. |
| LH-INP-007 | **MUST** | A form layout must arrange label, input, help text, required marker, and error message consistently while supporting variable window widths and DPI. | A reference form with at least 20 fields remains usable without overlap from 100% through 200% DPI. |
| LH-INP-008 | **MUST** | Validation must support field errors, form-wide errors, and a Validation Summary. Errors must not be conveyed by color alone. | An invalid form can focus the first error, show a summary, and expose errors through accessible descriptions. |
| LH-INP-009 | **SHOULD** | AutoComplete and MultiSelect/Tags should support projects, categories, recipients, tags, and similar lists. | A reference with at least 10,000 suggestions remains usable and does not block the UI indefinitely. |
| LH-INP-010 | **MAY** | Slider, Range Slider, and Color Picker may be added for a concrete need but are not R1 deliverables. | Admission requires a documented use case and no additional theme system. |

### 7.5 Application Shell, Navigation, and Commands

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-NAV-001 | **MUST** | A standardized Application Shell must exist for SASD WinForms applications. It includes title/header area, primary navigation, central workspace, status area, and services for dialogs, theme, and settings. | At least the CRUD, workbench, and utility reference applications can use the same Shell concept. |
| LH-NAV-002 | **MUST** | Main menus, context menus, and toolbar/CommandBar must use one shared command system. Visibility, enabled state, text, icon, shortcut, and execution must not be defined repeatedly and inconsistently. | An action can appear in menu, toolbar, and context menu simultaneously and changes enabled state consistently everywhere. |
| LH-NAV-003 | **MUST** | A sidebar or Navigation View must support hierarchical navigation, selection, icons, collapsible groups, and optional compact presentation. | Navigation is fully keyboard-accessible; the current area is visually and semantically unambiguous. |
| LH-NAV-004 | **MUST** | Tabs or document pages must support create, select, close, close confirmation for unsaved changes, and restoration. | A document with Dirty state cannot be lost without an explicit user decision. |
| LH-NAV-005 | **MUST** | Split Pane, Accordion/Expander, TreeView, Breadcrumb, and StatusBar must be available as reusable layout and navigation building blocks. | A workbench reference demonstrates left navigation, document area, detail area, and status information without project-specific custom controls. |
| LH-NAV-006 | **SHOULD** | Docking and workspace should be evaluated for Mail Workbench, Notes, and comparable multi-region applications. Layouts must be stored, migrated, and reset safely. | A stored layout is restored after restart; invalid or obsolete layout data falls back to a standard layout rather than crashing. |
| LH-NAV-007 | **MAY** | Ribbon and guided Wizards may be provided as optional modules. They must not force the core into a Ribbon application model. | Applications without Ribbon or Wizard do not load unnecessary packages. |

### 7.6 Data Display, Lists, and Tables

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-DAT-001 | **MUST** | `SasdDataGrid` must standardize data binding, sorting, column formatting, selection, optional editing, row status, empty states, and error handling. | A reference shows read-only and editable grids with the same basic interaction and consistent error messages. |
| LH-DAT-002 | **MUST** | The DataGrid must support search and simple filters per column or through a central filter bar. Filter states must be visible, removable, and storable. | Users can recognize active filtering, clear all filters with one action, and optionally restore them after restart. |
| LH-DAT-003 | **MUST** | Column order, width, visibility, sorting, and where applicable filters must be stored per user and resettable to defaults. | State is restored after restart; after schema changes, new columns remain reachable and obsolete entries do not cause errors. |
| LH-DAT-004 | **MUST** | Large data sets must be displayable through paging or Virtual Mode. Loading, sorting, or filtering must not block the interface uncontrolled. | A shared pilot with 100,000 records passes defined usability and resource checks. |
| LH-DAT-005 | **MUST** | ListView/ObjectList and TreeView must be available as lighter data views alongside the grid. Empty, loading, and error states must be standardized. | The same Empty/Loading/Error components are used in list, tree, and grid views. |
| LH-DAT-006 | **MUST** | Data Pager and Search Panel must be usable independently of a specific database technology. | Paging and search contracts work with in-memory data, SQLite, and a simulated remote source. |
| LH-DAT-007 | **SHOULD** | Column Chooser, summaries/aggregates, master-detail, and CSV export should follow in R2. Excel-specific export is added only for a concrete need. | Functions can be added as optional modules; a simple grid remains lightweight. |
| LH-DAT-008 | **SHOULD** | PropertyGrid and virtualized lists should be available after a pilot for technical settings, object inspection, and large lists. | The component supports custom display names, categories, descriptions, validation, and ReadOnly properties. |
| LH-DAT-009 | **DEFERRED** | Pivot/OLAP, a complete spreadsheet, and a complex Filter Builder are not developed in-house for V1. | These functions appear only in the future/build-versus-buy register. |

### 7.7 Dialogs, Feedback, and Error Presentation

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-FED-001 | **MUST** | A DialogService must provide information, warning, error, confirmation, Yes/No/Cancel, and custom modal content consistently. | Dialogs use consistent titles, icons, button order, default action, Escape behavior, and owner window. |
| LH-FED-002 | **MUST** | Error messages must separate an understandable user message from optional technical details with copy function, correlation ID, and log reference. | An error dialog can expand/collapse and copy technical details without automatically exposing sensitive data. |
| LH-FED-003 | **MUST** | Toast/Snackbar and status messages must provide non-modal feedback with severity, timing, optional action, and accessible announcement. | Messages do not permanently cover critical input and are keyboard-accessible when they contain actions. |
| LH-FED-004 | **MUST** | Tooltips and ScreenTips must provide short help, shortcuts, and where appropriate further guidance, but may not contain mandatory information exclusively. | All mandatory information remains available without mouse or tooltip. |
| LH-FED-005 | **MUST** | Progress Bar/Ring and Busy Overlay must support indeterminate and determinate progress, status, cancellation, and error state. | Operations beyond a defined duration show progress or busy status; cancellable operations provide a working cancel action. |
| LH-FED-006 | **SHOULD** | Desktop notifications and Teaching Tips should be available as optional services for background services, new emails, and onboarding help. | Notifications can be disabled and respect application settings; Teaching Tips are not required to operate the application. |

### 7.8 File, Shell, and Windows Integration

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-SYS-001 | **MUST** | Recent Files must manage recently used files or projects with pinning, removal, missing-file handling, and privacy options. | Missing files are handled understandably; users can delete individual entries or the entire list. |
| LH-SYS-002 | **MUST** | File drag-and-drop must support drop zones, file-type validation, multiple files, visual feedback, and safe error handling. | Unsupported files are rejected without crashing the application or processing them unchecked. |
| LH-SYS-003 | **MUST** | Clipboard functions for text, HTML, images, and files must be centrally encapsulated and fault-tolerant. | A temporarily locked clipboard produces controlled feedback instead of an unhandled exception. |
| LH-SYS-004 | **MUST** | System-tray support must provide icon, context menu, double-click, minimized background operation, and clean shutdown where enabled by the application. | Tray mode is optional; applications without tray support load no unnecessary background logic. |
| LH-SYS-005 | **SHOULD** | Folder Tree should provide hierarchical navigation with lazy loading and error states for file, backup, notes, and repository tools. | Inaccessible directories do not block the entire tree; access messages are understandable. |
| LH-SYS-006 | **SHOULD** | WebView2 must be available as a controlled host for Markdown preview, help, HTML content, or selected web components. | Navigation, downloads, new windows, local resources, script bridge, and allowed origins are configured and documented explicitly. |
| LH-SYS-007 | **MAY** | Global hotkeys and a complete File Explorer are developed only when a product need is demonstrated. | No hidden global hook or shell replacement exists in the R1 core. |

### 7.9 Technical Editors, Documents, and Media

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-DOC-001 | **SHOULD** | A Markdown editor should support text editing, search, basic formatting actions, preview, links, tables, and safe export. | A medium-sized Markdown file can be edited, stored, and previewed without data loss. |
| LH-DOC-002 | **SHOULD** | A code/configuration editor should support syntax highlighting, line numbers, search/replace, folding, undo/redo, and large text files. IntelliSense and full IDE functions are not required. | Source and log files within the defined pilot scope remain responsive to navigation and search. |
| LH-DOC-003 | **SHOULD** | A Diff Viewer should show text differences at least line by line as side-by-side or inline and allow navigation among differences. | Two versions of a prompt, configuration, or source file can be compared clearly. |
| LH-DOC-004 | **SHOULD** | Programmatic PDF generation must support headings, paragraphs, tables, page breaks, headers/footers, metadata, and images. | A multi-page sample report is generated reproducibly and reviewed visually. |
| LH-DOC-005 | **SHOULD** | An Image Viewer should provide zoom, pan, fit, rotate, and background display for common raster formats. | Large images can be viewed without incorrect scaling; zoom and fit are available by mouse and keyboard. |
| LH-DOC-006 | **SHOULD** | QR and barcode generation, with optional reading, should be available through a small replaceable adapter API. | At least QR and one common 1D code are generated and read in an automated round-trip test. |
| LH-DOC-007 | **DEFERRED** | A complete DOCX editor, report designer, spreadsheet, PDF editor, extensive rich-text editor, and media player are outside the current core. | Only a build-versus-buy note is maintained for these functions. |

### 7.10 Charts, KPI Displays, and Dashboards

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-VIZ-001 | **SHOULD** | A chart adapter should support at least line, bar, and scatter charts with title, axes, legend, tooltip, zoom, and image export. | The same SASD data structure can be displayed and exported through at least one primary chart implementation. |
| LH-VIZ-002 | **SHOULD** | Pie/donut, gauge, and sparkline should be available as optional KPI views when they can be integrated without a second incompatible UI suite. | The Component Gallery documents suitable and unsuitable uses and accessible alternatives. |
| LH-VIZ-003 | **MAY** | A simple heatmap may be added for technical measurements. Financial charts, treemap/sunburst, maps, network graph, and diagram editor are considered only for concrete demand. | R1 contains no specialist visualization engine beyond the defined chart pilot. |
| LH-VIZ-004 | **SHOULD** | A dashboard template should combine KPI cards, filters, charts, status messages, and saved views without developing a universal visual dashboard designer. | The template is source-based, documented, and adaptable without a proprietary designer. |

### 7.11 Quality, Usability, and Security Requirements

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-QUL-001 | **MUST** | All core components must support Per-Monitor DPI and at least 100%, 125%, 150%, and 200% scaling. Mixed multi-monitor scenarios must be tested. | Reference applications show no clipped text, overlapping controls, or unusably small icons across the test matrix. |
| LH-QUL-002 | **MUST** | All essential functions must be keyboard-accessible. Focus must be visible, tab order understandable, and Escape/Enter behavior consistent. | A complete core workflow in the reference applications can be completed without a mouse. |
| LH-QUL-003 | **MUST** | Accessibility must be supported through meaningful Name/Role/Value information, descriptions, states, and UI Automation for central controls. | Accessibility tests and manual review recognize core controls and states; known limitations are documented. |
| LH-QUL-004 | **MUST** | German and English must be localizable through resources. Text must not be hard-coded into components, and formatting must respect culture information. | The Gallery can switch between German and English; longer translations do not create unusable layouts. |
| LH-QUL-005 | **MUST** | Long-running or blocking operations must execute asynchronously or through suitable background processing and integrate progress, cancellation, and error handling. | The UI remains usable during defined file, export, and data operations; cancellation ends in a consistent state. |
| LH-QUL-006 | **MUST** | Components must not leak unhandled exceptions, log secrets, or automatically execute unvalidated files or web content. | Negative tests for invalid paths, files, clipboard, data sources, and WebView navigation pass without a security violation. |
| LH-QUL-007 | **MUST** | State persistence must handle version changes, corrupted settings, and reset to defaults robustly. | Corrupted or obsolete state causes recovery/reset rather than startup failure. |
| LH-QUL-008 | **MUST** | Visual regression, accessibility review, unit and integration tests, and resource/handle checks must be part of the Definition of Done. | A component can be released only when the defined quality checklist is documented completely. |
| LH-QUL-009 | **SHOULD** | Telemetry and logging hooks should expose events, errors, and performance data without requiring a specific telemetry provider. | An application can connect or disable logging/telemetry without replacing components. |
| LH-QUL-010 | **MUST** | Public APIs must be understandable, consistent, documented, and versionable. Breaking changes follow semantic versioning and migration guidance. | Every public component has XML documentation, an example, and a changelog entry. |

### 7.12 Deliverables and Operating Model

| ID | Priority | Requirement | Verifiability / Acceptance Criterion |
| --- | :---: | --- | --- |
| LH-LIE-001 | **MUST** | Delivery must include source code, NuGet packages, Component Gallery, at least three reference templates, automated tests, API documentation, licensing evidence, and changelog. | A release archive or release entry contains every named artifact and can be reproduced from the repository. |
| LH-LIE-002 | **MUST** | The Component Gallery must show every released component with states, variants, themes, DPI guidance, accessibility guidance, and code example. | No public component is documented only by source code or tests. |
| LH-LIE-003 | **MUST** | At least these templates must be provided: CRUD/administrative application, workbench/editor, and small system/utility tool. | Every template starts, demonstrates navigation, theme, error handling, settings, and logging hooks, and contains a short guide. |
| LH-LIE-004 | **SHOULD** | A dashboard/monitoring template and a Wizard/setup template should follow in R2 and R3 respectively. | Templates use only component families that have already been released. |
| LH-LIE-005 | **MUST** | Dependencies and package versions must be managed centrally and reviewed regularly. Security and licensing changes must be traceable. | An automated or documented maintenance process produces a dependency list, update review, and SBOM. |
| LH-LIE-006 | **MUST** | Support status, known limitations, and migration guidance must be documented for every version. | A user can determine which .NET versions, Windows targets, and component versions are supported. |

## 8. Reference Technologies and Vendor Strategy

The following candidates originate from the UI component catalog and are **not final implementation decisions**. Final selection occurs in the functional specification and through pilot applications.

| Role | Primary Candidate | Alternative/Complement | Decision in This Specification |
| --- | --- | --- | --- |
| Platform | Microsoft WinForms | – | Binding platform core |
| Visual foundation | Krypton Standard Toolkit | AntdUI; ReaLTaiizor as an idea source | Pilot Krypton first; do not mix visible theme systems |
| System dialogs | Ookii.Dialogs.WinForms | Windows standard dialogs | Evaluate as an adapter |
| Grid | DataGridView/Krypton DataGridView | AdvancedDataGridView selectively | SASD facade; no enterprise-grid reimplementation |
| Charts | ScottPlot | LiveCharts2 | R2 adapter; separate technical and dashboard scenarios |
| Code editor | ScintillaNET | WebView2/Monaco only as a special case | R2 adapter |
| Markdown/Diff | Markdig + ScintillaNET; DiffPlex | WebView2 preview | R2 |
| Image viewing | Cyotek ImageBox | Lightweight custom PictureBox extension | R2 |
| Web content | Microsoft WebView2 | CefSharp only with justified need | R2; mandatory security profile |
| PDF generation | PDFsharp/MigraDoc | QuestPDF after license review | R2; no WYSIWYG designer requirement |
| QR/barcode | QRCoder/ZXing.Net | – | Small replaceable adapter |
| Commercial benchmark | DevExpress, Telerik, Syncfusion | MESCIUS/Infragistics | Functional reference; purchase only with business case |

## 9. Future Register – Preserved but Deferred Topics

| ID | Later Project/Topic | Preserved Idea | Reactivation Criterion |
| --- | --- | --- | --- |
| Z-001 | SASD WPF Components | WPF-native implementation of shared design tokens, commands, dialog/state services, and selected components; not merely a visual copy of WinForms. | After stable WinForms R1 and documented platform-neutral contracts. |
| Z-002 | WinUI 3 pilot | Small Windows reference application to evaluate modern native Windows integration and Fluent Design. | After the WPF foundation or when a concrete project requires modern Windows App SDK functions. |
| Z-003 | Avalonia pilot | WPF-like cross-platform reference for Windows/Linux/macOS; assess the open core separately from commercial Pro components. | Only with actual non-Windows desktop demand. Not currently planned. |
| Z-004 | SASD Modern Web Components | ASP.NET Core/Blazor and possible Web Component, Tailwind, and daisyUI adapters. | Separate requirements specification; do not derive directly from WinForms controls. |
| Z-005 | ASP.NET Web Forms Compatibility | Maintenance/migration of existing ASPX applications; AJAX Control Toolkit as a historical functional and migration catalog, not a new-development foundation. | Only for a concrete legacy project. |
| Z-006 | SASD Java Business UI | Evaluate OpenXava/XavaPro, Vaadin, and OpenUI5 as Java/enterprise-web approaches. | Separate Java project after C# desktop and web priorities. |
| Z-007 | Mobile and other desktop platforms | Android, iOS, macOS, and Linux desktop; possible candidates MAUI, Avalonia, Uno, or native solutions. | Deliberately not planned; reactivate only with product and customer demand. |
| Z-008 | Complex business components | Scheduler, Gantt, Kanban, workflow, Pivot/OLAP, spreadsheet, report/dashboard/diagram designer, complete document editor, PDF editor, and 3D visualization. | Build-versus-buy decision per concrete project; no in-house development without a robust business case. |

## 10. Risks and Mitigations

| ID | Risk | Assessment | Mitigation |
| --- | --- | :---: | --- |
| R-001 | Scope grows into a full suite | high | Strict releases, component families, and exclusion list; new functions only with a use case. |
| R-002 | Dependence on one theme/control vendor | medium/high | SASD facades, adapters, no unnecessary vendor types in public APIs. |
| R-003 | Designer or DPI problems | high | Early reference application and fixed designer/DPI matrix before broad adoption. |
| R-004 | Inconsistent appearance from several libraries | high | Exactly one visible primary theme per application; visually align specialist controls. |
| R-005 | Licensing or redistribution risk | high | License inventory, SBOM, `THIRD-PARTY-NOTICES`, approval process, no unclear packages. |
| R-006 | Maintenance trap through forks | high | Fork only as the last option; upstream-first and documented exit plan. |
| R-007 | Accessibility considered too late | high | Accessibility is a release criterion from the first component, not a retrofit. |
| R-008 | Components work only in demos, not real applications | high | At least three production-like reference applications and adoption in real SASD projects. |
| R-009 | Excessive abstraction makes WinForms harder | medium | Abstract only recurring stable concepts; permit straightforward WinForms usage. |
| R-010 | Technology ageing | medium | Regular dependency and roadmap review; separate contracts from implementations. |

## 11. Acceptance of the Overall Delivery

- All MUST requirements for the release are fulfilled or documented through an accepted deviation.
- The defined designer, DPI, keyboard, accessibility, localization, and resource test matrix has passed.
- The Component Gallery contains every released component and state.
- At least three reference applications build reproducibly and use SASD packages without source copies.
- Public APIs, examples, changelog, license inventory, `THIRD-PARTY-NOTICES`, and SBOM exist.
- Known limitations, support status, upgrade paths, and reset paths are documented.

## 12. Open Decisions for the Subsequent Functional Specification

- exact package and repository structure and naming conventions
- Krypton pilot decision and scope of SASD facades
- concrete persistence formats and migration strategy
- test tools for desktop UI, visual regression, and accessibility
- performance thresholds for grid, lists, editor, and startup
- PDF library after functional and licensing review
- scope of docking, Ribbon, chart, and editor adapters in R2
- publication model: internal only, public open source, or open core/support offering

## 13. Source and Decision Basis

This requirements specification was derived from the internal **SASD UI Component Catalog Version 2 with WinForms Focus** (`01-component-catalog.md`). The catalog contains the broader market and functional research. This document deliberately reduces that breadth to the WinForms scope currently realistic for SASD.

Important reference groups from the catalog include Microsoft WinForms, Krypton Standard/Extended Toolkit, AntdUI, ReaLTaiizor, Syncfusion WinForms as a functional benchmark, WebView2, ScottPlot, LiveCharts2, ScintillaNET, Ookii.Dialogs, Cyotek ImageBox, PDFsharp/MigraDoc, FastReport OSS, and the separately deferred WPF, Avalonia, ASP.NET/ASPX, and Java/OpenXava product lines.

## 14. Change Log

| Version | Date | Change |
| --- | --- | --- |
| 0.1 | July 23, 2026 | Initial requirements draft derived from the UI component catalog; established WinForms scope, component families, requirements, exclusions, and future register. |
