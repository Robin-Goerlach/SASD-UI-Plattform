# SASD UI Component Catalogue — Version 2, WinForms Focus

**Market inventory, capability map and prioritisation basis for the SASD UI Platform**

| Attribute | Value |
| --- | --- |
| Version | 2.0 English edition |
| Date | 23 July 2026 |
| Primary current scope | C# / .NET 8 / Windows Forms |
| Future registers | WPF, WinUI 3, Avalonia, modern Web/Blazor, ASP.NET Web Forms and Java |
| Purpose | Preserve the full supplier/component landscape while deriving a realistic WinForms implementation backlog |

> The catalogue is an inventory and decision aid, not a promise to implement every listed control. Licences, maintenance activity and product boundaries must be verified again before adoption or distribution.

## SASD decision: WinForms first

The recommended sequence is: **WinForms foundation → WPF product line → WinUI 3/Avalonia pilots → modern ASP.NET Core/Blazor product line → separate ASP.NET Web Forms compatibility project → separate Java/OpenXava project**. Android, iOS, macOS and Linux desktop implementations are recorded but not planned.

### Why this sequence is appropriate

- Current SASD applications already use WinForms and .NET 8, so components can be proven in real products immediately.
- A stable WinForms product creates design tokens, command/navigation semantics, test methods and governance that can inform later platforms.
- WPF and web frameworks need native platform architecture rather than a lowest-common-denominator renderer.
- ASP.NET Web Forms and AJAX Control Toolkit belong to a legacy/migration line, while Java/OpenXava belongs to a separate ecosystem.

## Prioritisation model

| Class | Meaning |
| --- | --- |
| P0 | Required for the WinForms foundation and current SASD applications. |
| P1 | High-value extension after the foundation is stable. |
| P2 | Prototype only for a concrete product need. |
| P3 | Buy, integrate or defer; own implementation is disproportionate. |
| WEB | Keep for a separate browser/Blazor project. |
| FUTURE | WPF, WinUI, Avalonia, Java, mobile or other later product line. |

## Licence legend

| Term | SASD interpretation |
| --- | --- |
| Permissive open source | MIT, BSD or Apache-2.0; generally suitable, but notices and transitive dependencies still require review. |
| Copyleft | Modification/distribution duties may affect the business model; isolate and review legally. |
| Open core | Free core plus commercial extensions; plan only documented free capabilities as the open baseline. |
| Source available | Source can be read, but use, modification and redistribution are not automatically free. |
| Community licence | Free only under conditions such as revenue, organisation or team limits. |
| Commercial | Integrate under a valid licence; no source reuse or fork without explicit rights. |
| Legacy/archived | Feature, migration or compatibility reference only; not a new architectural foundation. |

## Commercial and historical reference suites

### 1. DevExpress Universal

- **Classification:** Commercial full suite
- **Platforms:** WinForms, WPF, Blazor, ASP.NET Core/MVC, DevExtreme JS/TS, reporting, Office/PDF, XAF/XPO
- **Key capabilities:** Data Grid, TreeList, Pivot, charts, scheduler, Gantt, diagram, dashboard, reports, spreadsheet, rich text, PDF, ribbon, docking, navigation and document APIs.
- **SASD assessment:** Primary benchmark for breadth, designer integration, documentation and support; not a realistic one-to-one implementation target.
- **Official source:** [https://www.devexpress.com/subscriptions/universal.xml](https://www.devexpress.com/subscriptions/universal.xml)

### 2. Progress Telerik UI / Kendo UI

- **Classification:** Commercial full suite
- **Platforms:** WinForms, WPF, Blazor, ASP.NET AJAX/Web Forms, ASP.NET Core/MVC, Angular, React, Vue and jQuery
- **Key capabilities:** Grid, TreeList, Pivot, charts, maps, scheduler, Gantt, TaskBoard, diagram, spreadsheet, document editor, PDF, docking, ribbon, PropertyGrid, upload and themes.
- **SASD assessment:** Second primary benchmark; especially useful for separating desktop, classic ASP.NET AJAX and modern framework-native products.
- **Official source:** [https://www.telerik.com/devcraft](https://www.telerik.com/devcraft)

### 3. Syncfusion Essential Studio

- **Classification:** Commercial suite with conditional Community licence
- **Platforms:** WinForms, WPF, WinUI, MAUI, Blazor, ASP.NET, Angular, React, Vue, Flutter and JavaScript
- **Key capabilities:** DataGrid, TreeGrid, Pivot, charts, scheduler, Gantt, diagram, Kanban, maps, gauges, document libraries, spreadsheet, rich text and forms.
- **SASD assessment:** High-value functional benchmark and possible licensed dependency; eligibility and product boundaries require regular review.
- **Official source:** [https://www.syncfusion.com/products/essential-studio](https://www.syncfusion.com/products/essential-studio)

### 4. MESCIUS ComponentOne

- **Classification:** Commercial .NET suite
- **Platforms:** WinForms, WPF, WinUI, MAUI, Blazor, ASP.NET Core and Web API
- **Key capabilities:** FlexGrid, FlexChart, FlexPivot, FlexReport, viewer, spreadsheet, diagram, scheduler, maps, inputs, ribbon, docking, document services and virtualisation.
- **SASD assessment:** Strong reference for modular .NET packaging and data-heavy desktop controls.
- **Official source:** [https://developer.mescius.com/componentone](https://developer.mescius.com/componentone)

### 5. MESCIUS Wijmo

- **Classification:** Commercial JavaScript suite
- **Platforms:** JavaScript/TypeScript, Angular, React, Vue and Web Components
- **Key capabilities:** FlexGrid variants, Pivot/OLAP, charts, gauges, calendars, inputs, navigation, viewers, barcode and maps.
- **SASD assessment:** Useful later for a lightweight framework-integrated SASD Web product line.
- **Official source:** [https://developer.mescius.com/wijmo](https://developer.mescius.com/wijmo)

### 6. MESCIUS SpreadJS / Spread.NET / ActiveReports

- **Classification:** Commercial specialist products
- **Platforms:** JavaScript, WinForms, WPF and web viewers
- **Key capabilities:** Excel-like spreadsheet, formulas, pivot, import/export, report engine, designer, viewer, print and PDF/Excel output.
- **SASD assessment:** Benchmark for spreadsheet and reporting classes that should be integrated rather than rebuilt.
- **Official source:** [https://developer.mescius.com/](https://developer.mescius.com/)

### 7. Infragistics Ultimate / Ignite UI

- **Classification:** Commercial full suite
- **Platforms:** WinForms, WPF, Blazor, Angular, React, Web Components and ASP.NET
- **Key capabilities:** Grid, hierarchical data, spreadsheet, charts, gauges, maps, scheduler, docking, ribbon, PropertyGrid, editors, app builder and analytics.
- **SASD assessment:** Reference for design-to-code and data-intensive applications; licence boundaries vary by product.
- **Official source:** [https://www.infragistics.com/products/ultimate](https://www.infragistics.com/products/ultimate)

### 8. Actipro Software Controls

- **Classification:** Commercial desktop specialist suite
- **Platforms:** WPF, WinForms and WinUI
- **Key capabilities:** Syntax editor, docking/MDI, ribbon/bars, grids, PropertyGrid, editors, shell browser, navigation, charts, gauges, wizard and themes.
- **SASD assessment:** Excellent quality benchmark for focused desktop products, especially editor, docking, PropertyGrid and shell.
- **Official source:** [https://www.actiprosoftware.com/products/controls](https://www.actiprosoftware.com/products/controls)

### 9. Xceed

- **Classification:** Commercial desktop and data components
- **Platforms:** WPF and .NET libraries
- **Key capabilities:** Enterprise WPF DataGrid, toolkit controls, themes, compression, FTP and document libraries.
- **SASD assessment:** Demonstrates that an enterprise grid is a product of its own; use as benchmark rather than build target.
- **Official source:** [https://xceed.com/](https://xceed.com/)

### 10. Nevron Open Vision / Vision for .NET

- **Classification:** Commercial cross-platform suite
- **Platforms:** Blazor WebAssembly, WinForms, WPF, macOS and ASP.NET/MVC
- **Key capabilities:** Chart, diagram, grid, rich text, gauge, scheduler, ribbon, docking, layouts, inputs and maps from a common codebase.
- **SASD assessment:** Relevant conceptual reference for shared logic, while SASD should keep platform rendering separate.
- **Official source:** [https://www.nevron.com/dotnet-controls](https://www.nevron.com/dotnet-controls)

### 11. SciChart

- **Classification:** Commercial high-performance visualisation
- **Platforms:** WPF, Avalonia/XPF, JavaScript and mobile
- **Key capabilities:** 2D/3D real-time charts, heatmaps, spectrograms, finance charts, annotations and GPU rendering.
- **SASD assessment:** Specialist benchmark only; not a general SASD core implementation.
- **Official source:** [https://www.scichart.com/](https://www.scichart.com/)

### 12. LightningChart

- **Classification:** Commercial high-performance visualisation
- **Platforms:** .NET WinForms/WPF/UWP, JavaScript and Python
- **Key capabilities:** GPU 2D/3D charts, signal tools, heatmaps, maps, dashboards and high-volume streaming.
- **SASD assessment:** Optional future adapter candidate for proven specialist requirements.
- **Official source:** [https://lightningchart.com/](https://lightningchart.com/)

### 13. Steema TeeChart

- **Classification:** Commercial chart suite
- **Platforms:** .NET, JavaScript/TypeScript, Delphi, Java and others
- **Key capabilities:** Large chart type set, gauges, maps, financial and scientific visualisation, export and interaction.
- **SASD assessment:** Cross-language chart benchmark; adapter only if a concrete requirement justifies it.
- **Official source:** [https://www.steema.com/product/net](https://www.steema.com/product/net)

### 14. TX Text Control

- **Classification:** Commercial document-editing suite
- **Platforms:** WinForms, WPF, ASP.NET and related .NET platforms
- **Key capabilities:** Word-processing editor, DOCX, PDF, mail merge, headers/footers, track changes, comments, forms and printing.
- **SASD assessment:** Reference for a document editor class that SASD should buy or integrate rather than implement.
- **Official source:** [https://www.textcontrol.com/](https://www.textcontrol.com/)

### 15. Sencha Ext JS

- **Classification:** Commercial JavaScript enterprise UI
- **Platforms:** JavaScript with classic and modern toolkits
- **Key capabilities:** Grid, tree, pivot, charts, forms, layout, scheduler-related ecosystem, data package, themes and tooling.
- **SASD assessment:** Historical and current benchmark for integrated browser business applications.
- **Official source:** [https://www.sencha.com/products/extjs/](https://www.sencha.com/products/extjs/)

### 16. DHTMLX Suite and project-management components

- **Classification:** Commercial/open-core web components
- **Platforms:** JavaScript/TypeScript and framework integrations
- **Key capabilities:** Grid, tree, form, calendar, scheduler, Gantt, Kanban, diagram, spreadsheet, rich text and file manager depending on product.
- **SASD assessment:** Useful later for Gantt/scheduler comparisons; licence and Community/Pro boundaries must be explicit.
- **Official source:** [https://dhtmlx.com/](https://dhtmlx.com/)

### 17. Bryntum

- **Classification:** Commercial project-planning suite
- **Platforms:** JavaScript, React, Angular and Vue
- **Key capabilities:** Scheduler, Scheduler Pro, Gantt, Calendar, Grid and TaskBoard with advanced resource planning.
- **SASD assessment:** Premium benchmark for complex scheduling; prefer licensed adapter over own implementation.
- **Official source:** [https://bryntum.com/](https://bryntum.com/)

### 18. AG Grid Enterprise / AG Charts Enterprise

- **Classification:** Open-core grid and charts
- **Platforms:** JavaScript, React, Angular and Vue
- **Key capabilities:** Community grid basics; Enterprise server-side models, grouping, pivot, Excel export, integrated charts, master-detail and tree data.
- **SASD assessment:** Strong future Web-grid candidate; never reimplement proprietary Enterprise features.
- **Official source:** [https://www.ag-grid.com/](https://www.ag-grid.com/)

### 19. Handsontable

- **Classification:** Commercial/source-available spreadsheet component
- **Platforms:** JavaScript, React, Angular and Vue
- **Key capabilities:** Excel-like grid, cell types, formulas, copy/paste, fill, freeze, merge, validation and comments.
- **SASD assessment:** Spreadsheet benchmark; do not treat as permissive open source.
- **Official source:** [https://handsontable.com/](https://handsontable.com/)

### 20. Highcharts

- **Classification:** Commercial web visualisation with source access
- **Platforms:** JavaScript/TypeScript and framework wrappers
- **Key capabilities:** Charts, stock, maps, Gantt, dashboards, export and accessibility.
- **SASD assessment:** Mature visualisation benchmark with licensing clearly separated from open source.
- **Official source:** [https://www.highcharts.com/](https://www.highcharts.com/)

### 21. Wisej.NET

- **Classification:** Commercial server-side web UI
- **Platforms:** ASP.NET/ASP.NET Core with C# or VB.NET
- **Key capabilities:** Desktop-like windows, forms, pages, desktop/taskbar, grid, charts, layouts, inputs, responsive profiles and Visual Studio designer.
- **SASD assessment:** Interesting bridge from WinForms thinking to web, but runtime architecture differs from Blazor/client components.
- **Official source:** [https://wisej.com/](https://wisej.com/)

### 22. AJAX Control Toolkit

- **Classification:** Archived BSD-3-Clause Web Forms suite
- **Platforms:** ASP.NET Web Forms
- **Key capabilities:** Accordion, AutoComplete, calendar, cascading dropdown, collapsible panel, colour picker, modal popup, masked input, tabs, watermark and animation extenders.
- **SASD assessment:** Historical feature and migration reference only; repository was archived in October 2024.
- **Official source:** [https://github.com/DevExpress/AjaxControlToolkit](https://github.com/DevExpress/AjaxControlToolkit)

### 23. MindFusion UI Controls

- **Classification:** Commercial specialist suite
- **Platforms:** WinForms, WPF, JavaScript, Java and mobile
- **Key capabilities:** Diagram, scheduler, charts, gauges, maps, virtual keyboard, spreadsheet, reporting and classic widgets.
- **SASD assessment:** Useful mid-sized reference for diagram, planning and visualisation adapters.
- **Official source:** [https://mindfusion.dev/](https://mindfusion.dev/)

### 24. Flexmonster Pivot Table & Charts

- **Classification:** Commercial web BI component
- **Platforms:** JavaScript/TypeScript, React, Angular, Vue and Blazor integration
- **Key capabilities:** Pivot table/charts, drill-down, OLAP/tabular sources, JSON/CSV, server connector, export and dashboards.
- **SASD assessment:** Strong pivot benchmark; an own SASD pivot engine would be disproportionate.
- **Official source:** [https://www.flexmonster.com/](https://www.flexmonster.com/)

### 25. DayPilot Pro

- **Classification:** Commercial scheduling components
- **Platforms:** JavaScript, Angular, React, Vue, ASP.NET and MVC
- **Key capabilities:** Event calendar, resource scheduler, month view and Gantt with drag/drop, timelines, resources and booking.
- **SASD assessment:** Focused benchmark for a future planning product line.
- **Official source:** [https://javascript.daypilot.org/](https://javascript.daypilot.org/)

### 26. GoJS

- **Classification:** Commercial diagramming component
- **Platforms:** JavaScript/TypeScript with React/Angular/Vue integration
- **Key capabilities:** Flowcharts, organisation charts, mind maps, graph layouts, data binding, drag/drop, undo, palette and overview.
- **SASD assessment:** Mature diagram-editor reference; optional adapter rather than own implementation.
- **Official source:** [https://gojs.net/](https://gojs.net/)

### 27. DevComponents DotNetBar

- **Classification:** Commercial WinForms control suite
- **Platforms:** WinForms
- **Key capabilities:** Ribbon, bars, docking, navigation, Office/Metro styles, PropertyGrid, SuperGrid, tree, wizard and calendar.
- **SASD assessment:** Historically important WinForms reference; verify current maintenance before any purchase.
- **Official source:** [https://marketplace.visualstudio.com/items?itemName=DevCo.DotNetBarforWindowsForms](https://marketplace.visualstudio.com/items?itemName=DevCo.DotNetBarforWindowsForms)

## Open-source and community desktop components

These entries include frameworks, broad control collections and specialist components. No single project is expected to cover the whole SASD product scope.

### D01. Microsoft WinForms

- **Platforms:** Windows/.NET
- **Licence/status:** MIT
- **Capabilities:** Mature designer-based desktop foundation: standard controls, DataGridView, TreeView, ListView, menus, ToolStrip, dialogs, MDI, printing and custom controls.
- **SASD assessment:** Primary SASD platform for current applications; add common behaviour rather than replace the framework.
- **Source:** [https://github.com/dotnet/winforms](https://github.com/dotnet/winforms)

### D02. Microsoft WPF

- **Platforms:** Windows/.NET/XAML
- **Licence/status:** MIT
- **Capabilities:** XAML, styling, templates, binding, commands, DataGrid, documents, animation and vector rendering.
- **SASD assessment:** Planned second SASD desktop product line after WinForms R1.
- **Source:** [https://github.com/dotnet/wpf](https://github.com/dotnet/wpf)

### D03. Windows App SDK / WinUI 3

- **Platforms:** Windows
- **Licence/status:** Mixed Microsoft open-source/runtime terms
- **Capabilities:** Modern native Windows UI, Fluent controls, NavigationView, CommandBar, NumberBox, InfoBar, lifecycle, windowing and notifications.
- **SASD assessment:** Evaluate later against Avalonia through identical pilot applications.
- **Source:** [https://github.com/microsoft/WindowsAppSDK](https://github.com/microsoft/WindowsAppSDK)

### D04. .NET MAUI Community Toolkit

- **Platforms:** Android, iOS, macOS and Windows
- **Licence/status:** MIT
- **Capabilities:** Popup, MediaElement, DrawingView, behaviours, converters, animations and platform helpers.
- **SASD assessment:** Out of current scope because SASD does not currently plan mobile products.
- **Source:** [https://github.com/CommunityToolkit/Maui](https://github.com/CommunityToolkit/Maui)

### D05. Avalonia UI

- **Platforms:** Windows, Linux, macOS, mobile and WebAssembly
- **Licence/status:** MIT core; commercial Pro additions
- **Capabilities:** Cross-platform XAML framework with controls, styles, binding, rendering and third-party ecosystem.
- **SASD assessment:** Future cross-platform pilot only; distinguish core from paid advanced controls.
- **Source:** [https://github.com/AvaloniaUI/Avalonia](https://github.com/AvaloniaUI/Avalonia)

### D06. Uno Platform

- **Platforms:** Windows, WebAssembly, Android, iOS, macOS and Linux targets
- **Licence/status:** Apache-2.0 core plus commercial services
- **Capabilities:** WinUI-style cross-platform UI, tooling and extensions.
- **SASD assessment:** Future comparison candidate, not part of current WinForms work.
- **Source:** [https://github.com/unoplatform/uno](https://github.com/unoplatform/uno)

### D07. Eto.Forms

- **Platforms:** Windows, macOS and Linux via native backends
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Cross-platform .NET abstraction for forms, controls, menus, drawing and dialogs.
- **SASD assessment:** Interesting lightweight concept but not aligned with current Windows-first visual quality target.
- **Source:** [https://github.com/picoe/Eto](https://github.com/picoe/Eto)

### D08. Krypton Standard Toolkit

- **Platforms:** WinForms
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Modernised controls, palettes, ribbon, navigator, workspace, docking-related components, DataGridView integration and themes.
- **SASD assessment:** Preferred R0 visual foundation and first upstream community to collaborate with.
- **Source:** [https://github.com/Krypton-Suite/Standard-Toolkit](https://github.com/Krypton-Suite/Standard-Toolkit)

### D09. Krypton Extended Toolkit

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Additional Krypton controls, dialogs, utilities and experimental extensions.
- **SASD assessment:** Use selectively after code, licence, maturity and overlap review.
- **Source:** [https://github.com/Krypton-Suite/Extended-Toolkit](https://github.com/Krypton-Suite/Extended-Toolkit)

### D10. ReaLTaiizor

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Large collection of modern themes and controls inspired by Material, Metro and other styles.
- **SASD assessment:** Useful visual reference and isolated pilot; not mixed with Krypton in one application.
- **Source:** [https://github.com/Taiizor/ReaLTaiizor](https://github.com/Taiizor/ReaLTaiizor)

### D11. MaterialSkin.2

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Material-styled forms and common controls.
- **SASD assessment:** Possible reference for theming, but narrower and less suitable as the complete SASD foundation.
- **Source:** [https://github.com/IgnaceMaes/MaterialSkin](https://github.com/IgnaceMaes/MaterialSkin)

### D12. DockPanel Suite

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Visual Studio-style docking, document panes, tool windows and layout persistence.
- **SASD assessment:** Alternative docking adapter only; maintenance status requires review.
- **Source:** [https://github.com/dockpanelsuite/dockpanelsuite](https://github.com/dockpanelsuite/dockpanelsuite)

### D13. WPF UI

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** Fluent/Windows 11-inspired controls, navigation, themes, dialogs and window effects.
- **SASD assessment:** Candidate for a future SASD WPF visual foundation.
- **Source:** [https://github.com/lepoco/wpfui](https://github.com/lepoco/wpfui)

### D14. MahApps.Metro

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** Modern windows, dialogs, flyouts, controls and themes.
- **SASD assessment:** Mature WPF reference for later product-line evaluation.
- **Source:** [https://github.com/MahApps/MahApps.Metro](https://github.com/MahApps/MahApps.Metro)

### D15. MaterialDesignInXamlToolkit

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** Material Design themes, cards, drawers, dialogs, snackbar, inputs and pickers.
- **SASD assessment:** WPF reference; do not combine indiscriminately with another visible theme suite.
- **Source:** [https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

### D16. HandyControl

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** Broad WPF control collection, themes, navigation, inputs, notifications and data views.
- **SASD assessment:** Future WPF comparison candidate; verify English documentation and maintenance.
- **Source:** [https://github.com/HandyOrg/HandyControl](https://github.com/HandyOrg/HandyControl)

### D17. Fluent.Ribbon

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** Office-style Ribbon with tabs, groups, contextual tabs and KeyTips.
- **SASD assessment:** Optional WPF ribbon reference.
- **Source:** [https://github.com/fluentribbon/Fluent.Ribbon](https://github.com/fluentribbon/Fluent.Ribbon)

### D18. AvalonDock

- **Platforms:** WPF
- **Licence/status:** Ms-PL/MIT depending maintained fork
- **Capabilities:** Docking manager, documents, anchorable tool panes and persisted layouts.
- **SASD assessment:** Future WPF workbench candidate after fork/licence selection.
- **Source:** [https://github.com/Dirkster99/AvalonDock](https://github.com/Dirkster99/AvalonDock)

### D19. Extended WPF Toolkit Community Edition

- **Platforms:** WPF
- **Licence/status:** MS-PL
- **Capabilities:** PropertyGrid, DateTime controls, wizard, colour picker and numerous utility controls.
- **SASD assessment:** Useful WPF specialist collection; commercial Plus features remain separate.
- **Source:** [https://github.com/xceedsoftware/wpftoolkit](https://github.com/xceedsoftware/wpftoolkit)

### D20. PropertyTools

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** PropertyGrid, spreadsheet-like grid, tree, colour, file and numeric editors.
- **SASD assessment:** Good future technical-application reference.
- **Source:** [https://github.com/PropertyTools/PropertyTools](https://github.com/PropertyTools/PropertyTools)

### D21. FluentAvalonia

- **Platforms:** Avalonia
- **Licence/status:** MIT
- **Capabilities:** Fluent-inspired controls and themes for Avalonia.
- **SASD assessment:** Potential future Avalonia visual layer.
- **Source:** [https://github.com/amwx/FluentAvalonia](https://github.com/amwx/FluentAvalonia)

### D22. SukiUI

- **Platforms:** Avalonia
- **Licence/status:** MIT
- **Capabilities:** Modern themed Avalonia controls, dialogs, notifications and navigation.
- **SASD assessment:** Future Avalonia comparison candidate.
- **Source:** [https://github.com/kikipoulet/SukiUI](https://github.com/kikipoulet/SukiUI)

### D23. Semi.Avalonia

- **Platforms:** Avalonia
- **Licence/status:** MIT
- **Capabilities:** Enterprise-oriented design system and controls inspired by Semi Design.
- **SASD assessment:** Future Avalonia component reference.
- **Source:** [https://github.com/irihitech/Semi.Avalonia](https://github.com/irihitech/Semi.Avalonia)

### D24. Ursa.Avalonia

- **Platforms:** Avalonia
- **Licence/status:** MIT
- **Capabilities:** Additional Avalonia controls and application patterns.
- **SASD assessment:** Future specialist supplement only.
- **Source:** [https://github.com/irihitech/Ursa.Avalonia](https://github.com/irihitech/Ursa.Avalonia)

### D25. ScottPlot

- **Platforms:** WinForms, WPF, Avalonia, Blazor and others
- **Licence/status:** MIT
- **Capabilities:** High-performance interactive technical plotting, large data, annotations and export.
- **SASD assessment:** Primary R2 WinForms chart adapter.
- **Source:** [https://github.com/ScottPlot/ScottPlot](https://github.com/ScottPlot/ScottPlot)

### D26. LiveCharts2

- **Platforms:** WinForms, WPF, Avalonia, Blazor, MAUI and WinUI
- **Licence/status:** MIT
- **Capabilities:** Animated charts, gauges, maps and cross-framework API.
- **SASD assessment:** Optional R2 dashboard adapter, not required by the core.
- **Source:** [https://github.com/beto-rodriguez/LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2)

### D27. OxyPlot

- **Platforms:** WPF, WinForms/Avalonia integrations and others
- **Licence/status:** MIT
- **Capabilities:** Scientific/static plots with axes, legends, annotations and export.
- **SASD assessment:** Alternative chart adapter where simple analytical plots are sufficient.
- **Source:** [https://github.com/oxyplot/oxyplot](https://github.com/oxyplot/oxyplot)

### D28. Mapsui

- **Platforms:** WPF, WinUI, Avalonia, MAUI and others
- **Licence/status:** MIT
- **Capabilities:** Interactive maps, layers, tiles, features, projections and markers.
- **SASD assessment:** Deferred until a real geospatial use case exists.
- **Source:** [https://github.com/Mapsui/Mapsui](https://github.com/Mapsui/Mapsui)

### D29. Helix Toolkit

- **Platforms:** WPF, WinUI, SharpDX-related targets
- **Licence/status:** MIT
- **Capabilities:** 3D visualisation, models, cameras, lights and interaction.
- **SASD assessment:** Explicitly exotic for current SASD scope; future specialist only.
- **Source:** [https://github.com/helix-toolkit/helix-toolkit](https://github.com/helix-toolkit/helix-toolkit)

### D30. FastReport Open Source

- **Platforms:** WinForms/.NET reporting
- **Licence/status:** MIT core with commercial additions
- **Capabilities:** Band-based report engine, templates, data sources, preview/export depending edition.
- **SASD assessment:** Possible R2 report-engine evaluation; designer/licence limitations documented.
- **Source:** [https://github.com/FastReports/FastReport](https://github.com/FastReports/FastReport)

### D31. QuestPDF

- **Platforms:** Cross-platform .NET document generation
- **Licence/status:** Community/commercial licence conditions
- **Capabilities:** Fluent programmatic PDF layout, tables, images and pagination.
- **SASD assessment:** Excluded until licence/business eligibility is explicitly approved.
- **Source:** [https://github.com/QuestPDF/QuestPDF](https://github.com/QuestPDF/QuestPDF)

### D32. PDFsharp / MigraDoc

- **Platforms:** Cross-platform .NET with platform-specific packages
- **Licence/status:** MIT
- **Capabilities:** Programmatic PDF, drawing and document layout.
- **SASD assessment:** Preferred R2 PDF generation pilot after current package compatibility review.
- **Source:** [https://github.com/empira/PDFsharp](https://github.com/empira/PDFsharp)

### D33. AvalonEdit

- **Platforms:** WPF
- **Licence/status:** MIT
- **Capabilities:** Syntax highlighting, folding, search and extensible text editing.
- **SASD assessment:** Future WPF editor candidate.
- **Source:** [https://github.com/icsharpcode/AvalonEdit](https://github.com/icsharpcode/AvalonEdit)

### D34. ScintillaNET

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Native-backed code editor with syntax, folding, markers, search and large-file support.
- **SASD assessment:** Primary R2 code/configuration editor adapter.
- **Source:** [https://github.com/desjarlais/Scintilla.NET](https://github.com/desjarlais/Scintilla.NET)

### D35. ReoGrid

- **Platforms:** WinForms and WPF variants
- **Licence/status:** MIT
- **Capabilities:** Spreadsheet-like grid, formulas, worksheets, import/export and scripting features.
- **SASD assessment:** Deferred specialist adapter; not part of R1 DataGrid.
- **Source:** [https://github.com/unvell/ReoGrid](https://github.com/unvell/ReoGrid)

### D36. SourceGrid

- **Platforms:** WinForms/.NET Framework-oriented
- **Licence/status:** MIT
- **Capabilities:** Cell-based grid, custom editors, virtualisation, frozen rows/columns and drag/drop.
- **SASD assessment:** Feature reference; current target compatibility and maintenance make it unsuitable as R1 base.
- **Source:** [https://github.com/siemens/sourcegrid](https://github.com/siemens/sourcegrid)

### D37. ObjectListView

- **Platforms:** WinForms
- **Licence/status:** GPL/commercial dual history; verify fork
- **Capabilities:** Enhanced ListView with columns, grouping, virtual mode, filtering and tree-list variants.
- **SASD assessment:** Pilot only after exact licence and maintained fork are approved.
- **Source:** [https://github.com/zzzprojects/ObjectListView](https://github.com/zzzprojects/ObjectListView)

### D38. CefSharp

- **Platforms:** WinForms and WPF
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Chromium embedding, JavaScript bridge, custom schemes and browser control.
- **SASD assessment:** WebView2 is preferred; CefSharp only for requirements WebView2 cannot meet.
- **Source:** [https://github.com/cefsharp/CefSharp](https://github.com/cefsharp/CefSharp)

### D39. Terminal.Gui

- **Platforms:** Console/terminal .NET
- **Licence/status:** MIT
- **Capabilities:** Cross-platform terminal windows, menus, dialogs, forms, lists and text controls.
- **SASD assessment:** Separate console UI interest; not a desktop component dependency.
- **Source:** [https://github.com/gui-cs/Terminal.Gui](https://github.com/gui-cs/Terminal.Gui)

### D40. AntdUI

- **Platforms:** WinForms
- **Licence/status:** Apache-2.0
- **Capabilities:** Broad modern control set including layout, navigation, inputs, table, feedback, chat, docking and ribbon-style components.
- **SASD assessment:** Serious isolated R0 comparison candidate beside Krypton; verify designer, DPI, accessibility and long-term API stability.
- **Source:** [https://github.com/AntdUI/AntdUI](https://github.com/AntdUI/AntdUI)

### D41. AcrylicUI

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Modern acrylic/Fluent-inspired windows and controls.
- **SASD assessment:** Visual reference or small optional theme; not primary foundation without stronger maturity evidence.
- **Source:** [https://github.com/leocb/AcrylicUI](https://github.com/leocb/AcrylicUI)

### D42. KGySoft.WinForms

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** High-quality utilities and controls, advanced drawing, colour handling, animations and compatibility helpers.
- **SASD assessment:** Valuable specialist dependency for selected behaviour after package-level review.
- **Source:** [https://github.com/koszeggy/KGySoft.Drawing](https://github.com/koszeggy/KGySoft.Drawing)

### D43. AdvancedDataGridView

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** DataGridView filter headers, date/time and text filters, and binding helpers.
- **SASD assessment:** Useful R1/R2 feature reference or narrow dependency for grid filtering.
- **Source:** [https://github.com/davidegironi/advanceddatagridview](https://github.com/davidegironi/advanceddatagridview)

### D44. Ookii.Dialogs.WinForms

- **Platforms:** WinForms
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Task Dialog, Vista-style file/folder dialogs, credential and progress dialogs.
- **SASD assessment:** Preferred internal candidate for Windows-native dialogs behind SASD contracts.
- **Source:** [https://github.com/ookii-dialogs/ookii-dialogs-winforms](https://github.com/ookii-dialogs/ookii-dialogs-winforms)

### D45. Cyotek.Windows.Forms.ImageBox

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Image zoom, pan, selection, pixel grid and viewport handling.
- **SASD assessment:** Primary R2 image-viewer pilot.
- **Source:** [https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox](https://github.com/cyotek/Cyotek.Windows.Forms.ImageBox)

### D46. Cyotek Windows Forms Controls

- **Platforms:** WinForms
- **Licence/status:** MIT
- **Capabilities:** Colour editors, screen colour picker, image and utility controls.
- **SASD assessment:** Selective specialist source; avoid importing the whole collection without need.
- **Source:** [https://github.com/cyotek/Cyotek.Windows.Forms](https://github.com/cyotek/Cyotek.Windows.Forms)

### D47. Material3.WinForms

- **Platforms:** WinForms
- **Licence/status:** Permissive project licence; verify exact release
- **Capabilities:** Material 3 styled WinForms controls and themes.
- **SASD assessment:** Visual pilot/reference only until licence, maintenance and designer behaviour are confirmed.
- **Source:** [https://github.com/robinrodricks/Material3.WinForms](https://github.com/robinrodricks/Material3.WinForms)

### D48. SunnyUI

- **Platforms:** WinForms
- **Licence/status:** LGPL/commercial terms; verify
- **Capabilities:** Large modern themed control collection with navigation, inputs, charts and utilities.
- **SASD assessment:** Licence and origin/maintenance risk require caution; not a default SASD dependency.
- **Source:** [https://github.com/yhuse/SunnyUI](https://github.com/yhuse/SunnyUI)

### D49. Microsoft Edge WebView2

- **Platforms:** WinForms, WPF and native Windows
- **Licence/status:** Microsoft licence/runtime terms
- **Capabilities:** Embedded Edge web content, navigation, messaging, virtual hosts and browser APIs.
- **SASD assessment:** Primary R2 embedded-web host behind a default-deny SASD security wrapper.
- **Source:** [https://github.com/MicrosoftEdge/WebView2Samples](https://github.com/MicrosoftEdge/WebView2Samples)

### D50. Awesome .NET WinForms Libraries

- **Platforms:** WinForms ecosystem
- **Licence/status:** Curated list; mixed licences
- **Capabilities:** Discovery list covering controls, themes, charts, editors, dialogs and utilities.
- **SASD assessment:** Research source only; every linked project requires independent validation.
- **Source:** [https://github.com/Tim-Sirry/awesome-dotnet-winforms](https://github.com/Tim-Sirry/awesome-dotnet-winforms)

## Open-source, open-core and community Web/AJAX components

These projects remain in the future Web register. They are not dependencies of the current WinForms product.

### W01. MudBlazor

- **Platforms:** Blazor
- **Licence/status:** MIT
- **Capabilities:** Material components, forms, tables/DataGrid, dialogs, navigation, theming and virtualisation.
- **SASD assessment:** Leading open-source Blazor suite candidate for future SASD Web.
- **Source:** [https://github.com/MudBlazor/MudBlazor](https://github.com/MudBlazor/MudBlazor)

### W02. Radzen Blazor Components

- **Platforms:** Blazor
- **Licence/status:** MIT components; commercial tooling/services separate
- **Capabilities:** Large component set including DataGrid, forms, charts, scheduler, tree and dialogs.
- **SASD assessment:** Strong future Blazor reference with clear separation from paid tooling.
- **Source:** [https://github.com/radzenhq/radzen-blazor](https://github.com/radzenhq/radzen-blazor)

### W03. Microsoft Fluent UI Blazor

- **Platforms:** Blazor
- **Licence/status:** MIT
- **Capabilities:** Fluent Web Components wrappers, forms, navigation, DataGrid and design tokens.
- **SASD assessment:** Natural Microsoft-style option for a future Blazor product line.
- **Source:** [https://github.com/microsoft/fluentui-blazor](https://github.com/microsoft/fluentui-blazor)

### W04. Ant Design Blazor

- **Platforms:** Blazor
- **Licence/status:** MIT
- **Capabilities:** Enterprise component suite based on Ant Design.
- **SASD assessment:** Useful for admin/business UI comparison.
- **Source:** [https://github.com/ant-design-blazor/ant-design-blazor](https://github.com/ant-design-blazor/ant-design-blazor)

### W05. HAVIT Blazor Bootstrap

- **Platforms:** Blazor
- **Licence/status:** MIT
- **Capabilities:** Bootstrap-native forms, grids, charts, dialogs, navigation and utilities.
- **SASD assessment:** Pragmatic future candidate for business applications.
- **Source:** [https://github.com/havit/Havit.Blazor](https://github.com/havit/Havit.Blazor)

### W06. BootstrapBlazor

- **Platforms:** Blazor
- **Licence/status:** Apache-2.0
- **Capabilities:** Broad Bootstrap-based component suite, forms, tables, charts and utilities.
- **SASD assessment:** Feature-rich reference; maintenance and localisation fit should be reviewed.
- **Source:** [https://github.com/dotnetcore/BootstrapBlazor](https://github.com/dotnetcore/BootstrapBlazor)

### W07. Blazor Bootstrap

- **Platforms:** Blazor
- **Licence/status:** MIT
- **Capabilities:** Bootstrap 5 components, grids, charts, modals, toasts and navigation.
- **SASD assessment:** Lightweight future Blazor option.
- **Source:** [https://github.com/vikramlearning/blazorbootstrap](https://github.com/vikramlearning/blazorbootstrap)

### W08. Blazorise

- **Platforms:** Blazor
- **Licence/status:** Apache-2.0 core with commercial products
- **Capabilities:** Provider abstraction over Bootstrap, Bulma, Material and others; DataGrid, charts and forms.
- **SASD assessment:** Interesting abstraction reference, but SASD should avoid lowest-common-denominator APIs.
- **Source:** [https://github.com/Megabit/Blazorise](https://github.com/Megabit/Blazorise)

### W09. Bootstrap

- **Platforms:** Web
- **Licence/status:** MIT
- **Capabilities:** Responsive CSS/JS foundation with grid, forms, navigation, modal, dropdown and utilities.
- **SASD assessment:** Baseline web technology and template source.
- **Source:** [https://getbootstrap.com/](https://getbootstrap.com/)

### W10. Tailwind CSS

- **Platforms:** Web
- **Licence/status:** MIT
- **Capabilities:** Utility-first CSS and design-token-friendly configuration.
- **SASD assessment:** Strong future SASD Web styling foundation.
- **Source:** [https://tailwindcss.com/](https://tailwindcss.com/)

### W11. daisyUI

- **Platforms:** Tailwind CSS
- **Licence/status:** MIT
- **Capabilities:** Semantic component classes, themes, forms, navigation, dialogs, cards and tables.
- **SASD assessment:** High-value visual foundation for a later SASD Web UI kit, but not a behavioural component engine.
- **Source:** [https://daisyui.com/](https://daisyui.com/)

### W12. Flowbite

- **Platforms:** Tailwind CSS, JS and framework integrations
- **Licence/status:** MIT core plus commercial Pro
- **Capabilities:** Interactive Tailwind components, forms, navigation, modal, date picker and templates.
- **SASD assessment:** Useful future component/template source; Pro boundaries must remain explicit.
- **Source:** [https://flowbite.com/](https://flowbite.com/)

### W13. MUI Core

- **Platforms:** React
- **Licence/status:** MIT
- **Capabilities:** Material UI components, themes and responsive layout.
- **SASD assessment:** Major React benchmark.
- **Source:** [https://mui.com/material-ui/](https://mui.com/material-ui/)

### W14. MUI X

- **Platforms:** React
- **Licence/status:** Community plus commercial Pro/Premium
- **Capabilities:** Advanced Data Grid, date/time pickers, charts and tree views.
- **SASD assessment:** Open-core feature-boundary benchmark.
- **Source:** [https://mui.com/x/](https://mui.com/x/)

### W15. Ant Design

- **Platforms:** React and design ecosystem
- **Licence/status:** MIT
- **Capabilities:** Enterprise forms, tables, navigation, feedback, data display and layout.
- **SASD assessment:** Key admin/business design reference.
- **Source:** [https://ant.design/](https://ant.design/)

### W16. Chakra UI

- **Platforms:** React
- **Licence/status:** MIT
- **Capabilities:** Accessible composable primitives and theming.
- **SASD assessment:** Reference for accessibility-oriented component APIs.
- **Source:** [https://chakra-ui.com/](https://chakra-ui.com/)

### W17. Mantine

- **Platforms:** React
- **Licence/status:** MIT
- **Capabilities:** Large hooks/component suite, forms, dates, rich text and extensions.
- **SASD assessment:** Broad modern React benchmark.
- **Source:** [https://mantine.dev/](https://mantine.dev/)

### W18. shadcn/ui

- **Platforms:** React/Tailwind/Radix
- **Licence/status:** MIT components copied into application
- **Capabilities:** Curated accessible components with local source ownership.
- **SASD assessment:** Important source-ownership model, but implies consumer maintenance.
- **Source:** [https://ui.shadcn.com/](https://ui.shadcn.com/)

### W19. Radix UI

- **Platforms:** React
- **Licence/status:** MIT
- **Capabilities:** Accessible unstyled primitives for dialogs, menus, popovers, tabs and forms.
- **SASD assessment:** Strong behavioural foundation reference.
- **Source:** [https://www.radix-ui.com/](https://www.radix-ui.com/)

### W20. Headless UI

- **Platforms:** React and Vue
- **Licence/status:** MIT
- **Capabilities:** Unstyled accessible menu, listbox, dialog, combobox, tabs and transitions.
- **SASD assessment:** Reference for separating behaviour from styling.
- **Source:** [https://headlessui.com/](https://headlessui.com/)

### W21. Ark UI

- **Platforms:** React, Vue, Solid and Svelte
- **Licence/status:** MIT
- **Capabilities:** Headless accessible state-machine-based components.
- **SASD assessment:** Cross-framework behaviour reference.
- **Source:** [https://ark-ui.com/](https://ark-ui.com/)

### W22. React Aria Components

- **Platforms:** React
- **Licence/status:** Apache-2.0
- **Capabilities:** Accessible interaction and component primitives based on Adobe React Aria.
- **SASD assessment:** High-quality accessibility reference.
- **Source:** [https://react-spectrum.adobe.com/react-aria/](https://react-spectrum.adobe.com/react-aria/)

### W23. Carbon Design System

- **Platforms:** React, Web Components and design assets
- **Licence/status:** Apache-2.0
- **Capabilities:** IBM enterprise design system, forms, data display, navigation and accessibility guidance.
- **SASD assessment:** Design-system governance reference.
- **Source:** [https://carbondesignsystem.com/](https://carbondesignsystem.com/)

### W24. PatternFly

- **Platforms:** React and web
- **Licence/status:** MIT
- **Capabilities:** Enterprise administration components and layouts.
- **SASD assessment:** Strong template/reference source for infrastructure tools.
- **Source:** [https://www.patternfly.org/](https://www.patternfly.org/)

### W25. Fluent UI React

- **Platforms:** React and Web Components ecosystem
- **Licence/status:** MIT
- **Capabilities:** Microsoft Fluent controls, tokens and accessibility.
- **SASD assessment:** Potential visual alignment with Windows products.
- **Source:** [https://react.fluentui.dev/](https://react.fluentui.dev/)

### W26. PrimeNG / PrimeReact / PrimeVue

- **Platforms:** Angular, React and Vue
- **Licence/status:** MIT core with commercial templates/support
- **Capabilities:** Broad suites: grids, forms, charts, tree, scheduler-like components and themes.
- **SASD assessment:** Major open-source multi-framework benchmark.
- **Source:** [https://www.primefaces.org/](https://www.primefaces.org/)

### W27. AG Grid Community

- **Platforms:** JS, React, Angular and Vue
- **Licence/status:** MIT
- **Capabilities:** Grid sorting, filtering, paging, editing and themes.
- **SASD assessment:** Top future Web-grid candidate; Enterprise boundaries remain separate.
- **Source:** [https://www.ag-grid.com/](https://www.ag-grid.com/)

### W28. TanStack Table

- **Platforms:** React, Vue, Solid and others
- **Licence/status:** MIT
- **Capabilities:** Headless table engine for sorting, filtering, grouping and pagination.
- **SASD assessment:** Excellent behaviour-first reference.
- **Source:** [https://tanstack.com/table](https://tanstack.com/table)

### W29. Tabulator

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Interactive data tables, editing, filters, grouping, virtual DOM and export.
- **SASD assessment:** Strong independent Web-grid option.
- **Source:** [https://tabulator.info/](https://tabulator.info/)

### W30. Grid.js

- **Platforms:** JavaScript and wrappers
- **Licence/status:** MIT
- **Capabilities:** Lightweight searchable, sortable and pageable data grid.
- **SASD assessment:** Useful simple-grid reference.
- **Source:** [https://gridjs.io/](https://gridjs.io/)

### W31. DataTables

- **Platforms:** jQuery/web
- **Licence/status:** MIT
- **Capabilities:** Long-established table enhancement with sorting, search, paging and extensions.
- **SASD assessment:** Legacy/current reference; not ideal for a new typed component architecture.
- **Source:** [https://datatables.net/](https://datatables.net/)

### W32. Chart.js

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Popular canvas charts with plugins and framework wrappers.
- **SASD assessment:** Default lightweight future chart candidate.
- **Source:** [https://www.chartjs.org/](https://www.chartjs.org/)

### W33. Apache ECharts

- **Platforms:** JavaScript
- **Licence/status:** Apache-2.0
- **Capabilities:** Rich interactive charts, maps, large data and dashboards.
- **SASD assessment:** Powerful future visualisation adapter candidate.
- **Source:** [https://echarts.apache.org/](https://echarts.apache.org/)

### W34. Plotly.js

- **Platforms:** JavaScript
- **Licence/status:** MIT core
- **Capabilities:** Scientific, statistical, 3D and interactive plots.
- **SASD assessment:** Specialist web chart reference.
- **Source:** [https://plotly.com/javascript/](https://plotly.com/javascript/)

### W35. D3.js

- **Platforms:** JavaScript
- **Licence/status:** ISC
- **Capabilities:** Low-level data-driven visualisation and DOM/SVG manipulation.
- **SASD assessment:** Foundation for custom visuals, not a simple component suite.
- **Source:** [https://d3js.org/](https://d3js.org/)

### W36. Vega / Vega-Lite

- **Platforms:** JavaScript/specification
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Declarative grammar for interactive charts and data transforms.
- **SASD assessment:** Useful model-driven visualisation reference.
- **Source:** [https://vega.github.io/vega-lite/](https://vega.github.io/vega-lite/)

### W37. FullCalendar Standard

- **Platforms:** JavaScript and frameworks
- **Licence/status:** MIT core; premium plugins commercial
- **Capabilities:** Calendar views, events, drag/drop; resources/timeline in premium products.
- **SASD assessment:** Primary future open-core calendar benchmark.
- **Source:** [https://fullcalendar.io/](https://fullcalendar.io/)

### W38. DHTMLX Gantt Community

- **Platforms:** JavaScript
- **Licence/status:** GPL/commercial depending edition
- **Capabilities:** Gantt task tree, timeline, links and editing.
- **SASD assessment:** Licence-sensitive future Gantt reference.
- **Source:** [https://dhtmlx.com/docs/products/dhtmlxGantt/](https://dhtmlx.com/docs/products/dhtmlxGantt/)

### W39. Frappe Gantt

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Lightweight Gantt with tasks, dependencies and interaction.
- **SASD assessment:** Simple future planning prototype candidate.
- **Source:** [https://github.com/frappe/gantt](https://github.com/frappe/gantt)

### W40. Tiptap Core

- **Platforms:** JavaScript, Vue and React
- **Licence/status:** MIT core with commercial extensions/cloud
- **Capabilities:** Headless ProseMirror-based rich-text editor.
- **SASD assessment:** Preferred future rich-text architecture reference.
- **Source:** [https://tiptap.dev/](https://tiptap.dev/)

### W41. ProseMirror

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Toolkit for schema-driven rich-text editors, transactions and plugins.
- **SASD assessment:** Low-level editor foundation.
- **Source:** [https://prosemirror.net/](https://prosemirror.net/)

### W42. Quill

- **Platforms:** JavaScript
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Rich-text editor with Delta model and modules.
- **SASD assessment:** Simpler editor reference.
- **Source:** [https://quilljs.com/](https://quilljs.com/)

### W43. Lexical

- **Platforms:** JavaScript/React
- **Licence/status:** MIT
- **Capabilities:** Extensible editor framework with immutable editor state.
- **SASD assessment:** Modern editor architecture reference.
- **Source:** [https://lexical.dev/](https://lexical.dev/)

### W44. Editor.js

- **Platforms:** JavaScript
- **Licence/status:** Apache-2.0
- **Capabilities:** Block-style structured content editor.
- **SASD assessment:** Useful for structured note/content scenarios.
- **Source:** [https://editorjs.io/](https://editorjs.io/)

### W45. CodeMirror 6

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Modular code editor with language packages, search and extensions.
- **SASD assessment:** Primary future lightweight code-editor candidate.
- **Source:** [https://codemirror.net/](https://codemirror.net/)

### W46. Monaco Editor

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** VS Code editor core with syntax, completion, diff and language features.
- **SASD assessment:** Powerful but heavy future code-editor adapter.
- **Source:** [https://microsoft.github.io/monaco-editor/](https://microsoft.github.io/monaco-editor/)

### W47. Mermaid

- **Platforms:** JavaScript/Markdown
- **Licence/status:** MIT
- **Capabilities:** Text-defined flowcharts, sequence, class, ER, Gantt and other diagrams.
- **SASD assessment:** Excellent documentation and preview component candidate.
- **Source:** [https://mermaid.js.org/](https://mermaid.js.org/)

### W48. React Flow / XYFlow

- **Platforms:** React and Svelte
- **Licence/status:** MIT
- **Capabilities:** Node-based editors, handles, edges, viewport and interaction.
- **SASD assessment:** Strong future workflow/diagram foundation.
- **Source:** [https://xyflow.com/](https://xyflow.com/)

### W49. Cytoscape.js

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Graph visualisation, layouts, analysis and interaction.
- **SASD assessment:** Graph/network specialist candidate.
- **Source:** [https://js.cytoscape.org/](https://js.cytoscape.org/)

### W50. bpmn-js

- **Platforms:** JavaScript
- **Licence/status:** bpmn.io licence (permissive with notices)
- **Capabilities:** BPMN viewer/modeler, palette, properties integrations and import/export.
- **SASD assessment:** Specialised workflow adapter reference.
- **Source:** [https://bpmn.io/toolkit/bpmn-js/](https://bpmn.io/toolkit/bpmn-js/)

### W51. Leaflet

- **Platforms:** JavaScript
- **Licence/status:** BSD-2-Clause
- **Capabilities:** Lightweight interactive maps, markers, layers and plugins.
- **SASD assessment:** Primary simple map candidate.
- **Source:** [https://leafletjs.com/](https://leafletjs.com/)

### W52. OpenLayers

- **Platforms:** JavaScript
- **Licence/status:** BSD-2-Clause
- **Capabilities:** Advanced GIS maps, projections, vector/raster layers and interactions.
- **SASD assessment:** Heavy geospatial specialist reference.
- **Source:** [https://openlayers.org/](https://openlayers.org/)

### W53. MapLibre GL JS

- **Platforms:** JavaScript/WebGL
- **Licence/status:** BSD-3-Clause
- **Capabilities:** Vector-tile maps, styles, 3D terrain and interaction.
- **SASD assessment:** Modern map engine candidate if a real requirement emerges.
- **Source:** [https://maplibre.org/maplibre-gl-js/docs/](https://maplibre.org/maplibre-gl-js/docs/)

### W54. Uppy

- **Platforms:** JavaScript and frameworks
- **Licence/status:** MIT
- **Capabilities:** Modular file upload, drag/drop, resumable upload and providers.
- **SASD assessment:** Strong future upload reference.
- **Source:** [https://uppy.io/](https://uppy.io/)

### W55. FilePond

- **Platforms:** JavaScript and frameworks
- **Licence/status:** MIT core with commercial plugins
- **Capabilities:** File upload UI, previews, validation and asynchronous processing.
- **SASD assessment:** Useful open-core upload option.
- **Source:** [https://pqina.nl/filepond/](https://pqina.nl/filepond/)

### W56. SortableJS

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Drag-and-drop sorting and list reordering.
- **SASD assessment:** Small reusable behaviour candidate.
- **Source:** [https://sortablejs.github.io/Sortable/](https://sortablejs.github.io/Sortable/)

### W57. SweetAlert2

- **Platforms:** JavaScript
- **Licence/status:** MIT
- **Capabilities:** Accessible styled alerts, confirmations, prompts and toasts.
- **SASD assessment:** Feedback interaction reference.
- **Source:** [https://sweetalert2.github.io/](https://sweetalert2.github.io/)

### W58. JSON Forms

- **Platforms:** React, Angular and Vue
- **Licence/status:** MIT
- **Capabilities:** Schema-driven forms from JSON Schema and UI Schema.
- **SASD assessment:** Reference for a later schema-form module.
- **Source:** [https://jsonforms.io/](https://jsonforms.io/)

### W59. OpenUI5

- **Platforms:** JavaScript
- **Licence/status:** Apache-2.0
- **Capabilities:** SAP enterprise UI framework with controls, data binding, routing and themes.
- **SASD assessment:** Major enterprise web reference.
- **Source:** [https://openui5.org/](https://openui5.org/)

### W60. Vaadin Flow Components

- **Platforms:** Java/server-driven web
- **Licence/status:** Apache-2.0 core with commercial components
- **Capabilities:** Web Components exposed to Java, grids, forms, navigation and application framework.
- **SASD assessment:** Important later Java enterprise comparison.
- **Source:** [https://vaadin.com/components](https://vaadin.com/components)

### W61. Webix UI

- **Platforms:** JavaScript
- **Licence/status:** GPL/commercial
- **Capabilities:** Broad widgets, DataTable, tree, scheduler, forms and layouts.
- **SASD assessment:** Licence-sensitive suite benchmark.
- **Source:** [https://webix.com/](https://webix.com/)

### W62. Ionic Framework

- **Platforms:** Web, Angular, React and Vue
- **Licence/status:** MIT
- **Capabilities:** Mobile-oriented components, navigation and native bridge ecosystem.
- **SASD assessment:** Out of current scope; retain for future mobile discussions.
- **Source:** [https://ionicframework.com/](https://ionicframework.com/)

### W63. Angular Material

- **Platforms:** Angular
- **Licence/status:** MIT
- **Capabilities:** Material components, CDK accessibility, overlays, tables and forms.
- **SASD assessment:** Primary Angular benchmark.
- **Source:** [https://material.angular.io/](https://material.angular.io/)

### W64. Vuetify

- **Platforms:** Vue
- **Licence/status:** MIT
- **Capabilities:** Material design component framework with grids, forms and data display.
- **SASD assessment:** Primary Vue benchmark.
- **Source:** [https://vuetifyjs.com/](https://vuetifyjs.com/)

### W65. Quasar Framework

- **Platforms:** Vue, web, Electron and mobile wrappers
- **Licence/status:** MIT
- **Capabilities:** Large UI framework, CLI, layouts and cross-target packaging.
- **SASD assessment:** Interesting single-codebase reference, not current scope.
- **Source:** [https://quasar.dev/](https://quasar.dev/)

### W66. Element Plus

- **Platforms:** Vue
- **Licence/status:** MIT
- **Capabilities:** Enterprise forms, tables, navigation, feedback and data components.
- **SASD assessment:** Strong Vue admin reference.
- **Source:** [https://element-plus.org/](https://element-plus.org/)

### W67. UIkit

- **Platforms:** Web
- **Licence/status:** MIT
- **Capabilities:** Lightweight CSS/JS components and responsive layout.
- **SASD assessment:** Template/style reference.
- **Source:** [https://getuikit.com/](https://getuikit.com/)

### W68. Bulma

- **Platforms:** Web CSS
- **Licence/status:** MIT
- **Capabilities:** CSS-only responsive components and utilities.
- **SASD assessment:** Simple styling reference.
- **Source:** [https://bulma.io/](https://bulma.io/)

### W69. Shoelace / Web Awesome

- **Platforms:** Web Components
- **Licence/status:** MIT core with evolving product model
- **Capabilities:** Framework-neutral accessible custom elements and design tokens.
- **SASD assessment:** Important Web Components candidate; verify current branding/licensing.
- **Source:** [https://shoelace.style/](https://shoelace.style/)

### W70. React-admin

- **Platforms:** React
- **Licence/status:** MIT core with commercial Enterprise
- **Capabilities:** Data-driven admin applications, CRUD resources, auth, routing and data providers.
- **SASD assessment:** Excellent application-template reference.
- **Source:** [https://marmelab.com/react-admin/](https://marmelab.com/react-admin/)

### W71. Refine

- **Platforms:** React and supported UI kits
- **Licence/status:** MIT core with commercial services
- **Capabilities:** Headless CRUD/admin framework, data/auth providers, routing and forms.
- **SASD assessment:** Strong architecture/template reference.
- **Source:** [https://refine.dev/](https://refine.dev/)

### W72. FINOS Perspective

- **Platforms:** Web and Python integrations
- **Licence/status:** Apache-2.0
- **Capabilities:** High-performance streaming analytics tables, pivots and charts.
- **SASD assessment:** Specialist data-analysis reference.
- **Source:** [https://perspective.finos.org/](https://perspective.finos.org/)

### W73. RevoGrid

- **Platforms:** Web Components and framework wrappers
- **Licence/status:** MIT
- **Capabilities:** High-performance virtualised data grid.
- **SASD assessment:** Future open-source grid candidate.
- **Source:** [https://revolist.github.io/revogrid/](https://revolist.github.io/revogrid/)

### W74. SlickGrid / Slickgrid-Universal

- **Platforms:** JavaScript and framework integrations
- **Licence/status:** MIT
- **Capabilities:** Virtualised grid, editing, grouping, filters and plugins.
- **SASD assessment:** Mature grid reference; choose maintained distribution carefully.
- **Source:** [https://github.com/ghiscoding/slickgrid-universal](https://github.com/ghiscoding/slickgrid-universal)

### W75. CKEditor 5

- **Platforms:** Web frameworks
- **Licence/status:** GPL/commercial dual model
- **Capabilities:** Rich document editing, collaboration and plugins.
- **SASD assessment:** Powerful but licence-sensitive editor benchmark.
- **Source:** [https://ckeditor.com/ckeditor-5/](https://ckeditor.com/ckeditor-5/)

### W76. TinyMCE

- **Platforms:** Web frameworks
- **Licence/status:** GPL/commercial
- **Capabilities:** Mature rich-text editor with plugins, file handling and collaboration options.
- **SASD assessment:** Licence-sensitive editor benchmark.
- **Source:** [https://www.tiny.cloud/](https://www.tiny.cloud/)

### W77. PDF.js

- **Platforms:** Web
- **Licence/status:** Apache-2.0
- **Capabilities:** PDF rendering, text/search layers and viewer components.
- **SASD assessment:** Primary future browser PDF viewer foundation.
- **Source:** [https://mozilla.github.io/pdf.js/](https://mozilla.github.io/pdf.js/)

### W78. Golden Layout

- **Platforms:** Web
- **Licence/status:** MIT
- **Capabilities:** Dockable panels and multi-window application layouts.
- **SASD assessment:** Workbench-layout reference.
- **Source:** [https://golden-layout.com/](https://golden-layout.com/)

### W79. GridStack.js

- **Platforms:** Web
- **Licence/status:** MIT
- **Capabilities:** Drag/resizable dashboard grid and widget layouts.
- **SASD assessment:** Future dashboard-layout candidate.
- **Source:** [https://gridstackjs.com/](https://gridstackjs.com/)

### W80. Fabric.js

- **Platforms:** Canvas/web
- **Licence/status:** MIT
- **Capabilities:** Object model, drawing, transforms, selection and serialisation.
- **SASD assessment:** Image/canvas editor foundation.
- **Source:** [http://fabricjs.com/](http://fabricjs.com/)

### W81. Konva

- **Platforms:** Canvas/web
- **Licence/status:** MIT
- **Capabilities:** 2D canvas scene graph, interaction, shapes and transforms.
- **SASD assessment:** Alternative graphical editor foundation.
- **Source:** [https://konvajs.org/](https://konvajs.org/)

### W82. maxGraph

- **Platforms:** JavaScript
- **Licence/status:** Apache-2.0
- **Capabilities:** Diagram/graph editor successor ecosystem related to mxGraph.
- **SASD assessment:** Possible open diagram foundation after maturity review.
- **Source:** [https://github.com/maxGraph/maxGraph](https://github.com/maxGraph/maxGraph)

### W83. TOAST UI Components

- **Platforms:** JavaScript
- **Licence/status:** MIT or product-specific open licences
- **Capabilities:** Grid, calendar, editor, image editor and charts as separate projects.
- **SASD assessment:** Valuable specialist collection; review each project independently.
- **Source:** [https://ui.toast.com/](https://ui.toast.com/)

### W84. jsreport

- **Platforms:** Node.js, web designer and API
- **Licence/status:** Open-core
- **Capabilities:** Server-side reporting from HTML/templates/data with PDF/Excel, scheduling and extensions.
- **SASD assessment:** Relevant future server-report alternative; operating and licence model need separate assessment.
- **Source:** [https://jsreport.net/](https://jsreport.net/)

## Start weighting of all 123 component classes for WinForms

This matrix preserves every component class from the German source and assigns its initial WinForms priority. Detailed implementation commitments are narrowed further by the requirements and functional specifications.

| ID | Area | Component | Priority | Score | Earliest horizon | Rationale |
| --- | --- | --- | :---: | ---: | --- | --- |
| DATA-001 | Data display and tables | DataGrid / DataTable | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| DATA-002 | Data display and tables | TreeGrid / TreeList | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| DATA-003 | Data display and tables | PivotGrid / OLAP | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| DATA-004 | Data display and tables | PropertyGrid | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| DATA-005 | Data display and tables | ListView / ObjectList | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| DATA-006 | Data display and tables | Card View | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: medium. |
| DATA-007 | Data display and tables | Virtualized List | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| DATA-008 | Data display and tables | Master-Detail | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| DATA-009 | Data display and tables | Infinite Scroll | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: medium. |
| DATA-010 | Data display and tables | Data Pager | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| DATA-011 | Data display and tables | Column Chooser | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| DATA-012 | Data display and tables | Filter Builder | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| DATA-013 | Data display and tables | Search Panel | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| DATA-014 | Data display and tables | Summaries / Aggregates | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| DATA-015 | Data display and tables | Data Export | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| FORM-016 | Forms and input | TextBox / TextArea | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FORM-017 | Forms and input | Masked Input | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FORM-018 | Forms and input | Numeric Editor | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FORM-019 | Forms and input | Date Picker | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FORM-020 | Forms and input | Time Picker | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FORM-021 | Forms and input | Date Range Picker | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: medium. |
| FORM-022 | Forms and input | Calendar | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| FORM-023 | Forms and input | ComboBox / Select | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FORM-024 | Forms and input | AutoComplete | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| FORM-025 | Forms and input | MultiSelect / Tags | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| FORM-026 | Forms and input | CheckBox / Radio / Toggle | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FORM-027 | Forms and input | Slider / Range Slider | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| FORM-028 | Forms and input | Color Picker | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| FORM-029 | Forms and input | File / Folder Picker | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FORM-030 | Forms and input | Upload | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: high. |
| FORM-031 | Forms and input | Rating | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: low. |
| FORM-032 | Forms and input | Signature Pad | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: medium. |
| FORM-033 | Forms and input | Form Layout | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FORM-034 | Forms and input | Validation Summary | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FORM-035 | Forms and input | Schema-driven Form | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| NAV-036 | Navigation and layout | Application Shell | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| NAV-037 | Navigation and layout | Menu / Context Menu | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| NAV-038 | Navigation and layout | Toolbar / CommandBar | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| NAV-039 | Navigation and layout | Ribbon | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| NAV-040 | Navigation and layout | Sidebar / Navigation Drawer | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| NAV-041 | Navigation and layout | Navigation View | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| NAV-042 | Navigation and layout | Tabs / Documents | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| NAV-043 | Navigation and layout | Docking / MDI | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: very high. |
| NAV-044 | Navigation and layout | Split Pane | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| NAV-045 | Navigation and layout | Accordion / Expander | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| NAV-046 | Navigation and layout | TreeView | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| NAV-047 | Navigation and layout | Breadcrumb | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| NAV-048 | Navigation and layout | Stepper / Wizard | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| NAV-049 | Navigation and layout | Tile / Dashboard Layout | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| NAV-050 | Navigation and layout | Responsive Layout | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: high. |
| NAV-051 | Navigation and layout | Status Bar | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FEED-052 | Dialogs, feedback and help | Message Dialog | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FEED-053 | Dialogs, feedback and help | Content Dialog / Modal | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FEED-054 | Dialogs, feedback and help | Drawer / Flyout | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: medium. |
| FEED-055 | Dialogs, feedback and help | Toast / Snackbar | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FEED-056 | Dialogs, feedback and help | Desktop Notification | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| FEED-057 | Dialogs, feedback and help | Tooltip / ScreenTip | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FEED-058 | Dialogs, feedback and help | Popover | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: medium. |
| FEED-059 | Dialogs, feedback and help | Progress Bar / Ring | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| FEED-060 | Dialogs, feedback and help | Busy Overlay | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FEED-061 | Dialogs, feedback and help | Skeleton | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: low. |
| FEED-062 | Dialogs, feedback and help | Error Details Dialog | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| FEED-063 | Dialogs, feedback and help | Help / Teaching Tip | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| VIZ-064 | Charts, dashboards and geodata | Cartesian Chart | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| VIZ-065 | Charts, dashboards and geodata | Pie / Donut | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| VIZ-066 | Charts, dashboards and geodata | Financial Chart | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| VIZ-067 | Charts, dashboards and geodata | Heatmap | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| VIZ-068 | Charts, dashboards and geodata | Treemap / Sunburst | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| VIZ-069 | Charts, dashboards and geodata | Sankey | WEB | 10 | Separate Web project | Do not prioritise in the WinForms backlog; address with browser/Blazor-native technology later. Estimated effort: high. |
| VIZ-070 | Charts, dashboards and geodata | Gauge | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| VIZ-071 | Charts, dashboards and geodata | Sparkline / Micro Chart | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| VIZ-072 | Charts, dashboards and geodata | Dashboard | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: very high. |
| VIZ-073 | Charts, dashboards and geodata | Map 2D | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| VIZ-074 | Charts, dashboards and geodata | Map 3D / Globe | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| VIZ-075 | Charts, dashboards and geodata | Network Graph | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| VIZ-076 | Charts, dashboards and geodata | Diagram Editor | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| VIZ-077 | Charts, dashboards and geodata | Text-to-Diagram | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| PLAN-078 | Planning, time and workflow | Scheduler | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: very high. |
| PLAN-079 | Planning, time and workflow | Resource Scheduler | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| PLAN-080 | Planning, time and workflow | Gantt | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: very high. |
| PLAN-081 | Planning, time and workflow | Kanban / TaskBoard | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| PLAN-082 | Planning, time and workflow | Timeline | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: medium. |
| PLAN-083 | Planning, time and workflow | Calendar Heatmap | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: medium. |
| PLAN-084 | Planning, time and workflow | Recurrence Editor | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| PLAN-085 | Planning, time and workflow | Workflow Designer | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: very high. |
| DOC-086 | Documents, editors and media | Rich Text Editor | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: very high. |
| DOC-087 | Documents, editors and media | Document Editor | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| DOC-088 | Documents, editors and media | Markdown Editor | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| DOC-089 | Documents, editors and media | Code Editor | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| DOC-090 | Documents, editors and media | Diff Viewer | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| DOC-091 | Documents, editors and media | PDF Viewer | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| DOC-092 | Documents, editors and media | PDF Generator | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| DOC-093 | Documents, editors and media | Report Engine | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: very high. |
| DOC-094 | Documents, editors and media | Report Designer | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| DOC-095 | Documents, editors and media | Spreadsheet | P3 | 25 | Purchase/adapter | Too complex for the initial product; do not implement without a proven business case. Estimated effort: very high. |
| DOC-096 | Documents, editors and media | Image Viewer | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| DOC-097 | Documents, editors and media | Image Editor | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| DOC-098 | Documents, editors and media | Media Player | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| DOC-099 | Documents, editors and media | Barcode / QR | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| SYS-100 | Files, shell and system integration | File Explorer | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: very high. |
| SYS-101 | Files, shell and system integration | Folder Tree | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| SYS-102 | Files, shell and system integration | Recent Files | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: low. |
| SYS-103 | Files, shell and system integration | Drag and Drop Files | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| SYS-104 | Files, shell and system integration | Clipboard | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| SYS-105 | Files, shell and system integration | System Tray | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| SYS-106 | Files, shell and system integration | Global Hotkeys | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| SYS-107 | Files, shell and system integration | Browser/WebView | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| QUAL-108 | Themes, quality and platform services | Design Tokens | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-109 | Themes, quality and platform services | Light/Dark Theme | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| QUAL-110 | Themes, quality and platform services | High Contrast | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-111 | Themes, quality and platform services | Accessibility | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: very high. |
| QUAL-112 | Themes, quality and platform services | Localization | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-113 | Themes, quality and platform services | RTL Layout | P2 | 50 | Demand-driven | Prototype only for a concrete product need and compare benefit with maintenance cost. Estimated effort: high. |
| QUAL-114 | Themes, quality and platform services | High DPI / Scaling | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-115 | Themes, quality and platform services | Keyboard Navigation | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-116 | Themes, quality and platform services | Command System | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-117 | Themes, quality and platform services | Undo/Redo | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: high. |
| QUAL-118 | Themes, quality and platform services | State Persistence | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-119 | Themes, quality and platform services | Telemetry Hooks | P1 | 72 | After foundation | High reuse value after the foundation; prefer an replaceable adapter or focused SASD façade. Estimated effort: medium. |
| QUAL-120 | Themes, quality and platform services | Visual Regression Tests | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-121 | Themes, quality and platform services | Accessibility Tests | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |
| QUAL-122 | Themes, quality and platform services | Component Gallery | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: medium. |
| QUAL-123 | Themes, quality and platform services | API Documentation | P0 | 90 | Now | Foundation capability used by several current SASD WinForms applications; define an owned contract and automated tests. Estimated effort: high. |


**Parsed component-class count:** 123.

## Functional interpretation by component family

| Family | Included capabilities | SASD decision |
| --- | --- | --- |
| Data display and tables | DataGrid/DataTable, TreeGrid/TreeList, PropertyGrid, ListView, virtual lists, master-detail, pager, column chooser, search, filter, summaries and export. | R1 owns the common business-grid contract, search/filter/state/paging and CSV. TreeGrid, summaries, master-detail and advanced views remain P1/R2. Pivot/OLAP and spreadsheet behaviour are P3. |
| Forms and input | Text, masked, numeric, date/time, choice, autocomplete, tags, switches, sliders, colour, file/folder, form layout and validation. | R1 standardises layout, binding, validation and the common native/Krypton inputs. Upload is web-only; signature, rating and schema-driven forms require concrete demand. |
| Navigation and layout | Application shell, menu, context menu, command bar, sidebar, breadcrumb, tabs, split/layout panels, docking, ribbon, wizard and status. | Shell, commands, navigation and status are R1. Docking is R2. Ribbon/wizard are optional R3 and never required by the core. |
| Dialogs, feedback and help | Information/confirmation/error dialogs, task dialogs, notifications, progress, busy, empty state, tooltip, help and about. | R1 provides service-based feedback with keyboard/accessibility rules. Native Windows notifications are optional. |
| Charts, dashboards and geodata | Technical charts, KPI cards, gauges, maps, heatmaps and dashboard layouts. | ScottPlot line/bar/scatter and accessible KPI cards are R2. Maps, complex gauges and custom dashboards need a real product case. |
| Planning, time and workflow | Scheduler, resource scheduler, Gantt, Kanban, timeline, recurrence and workflow designer. | These classes are expensive specialist products. Keep them in the future/adapter register; no initial own implementation. |
| Documents, editors and media | Rich text, document editor, Markdown, code, diff, PDF viewer/generator, reporting, spreadsheet, image/media and barcode. | R2 implements focused Markdown/code/diff/image/barcode/PDF services. Full word processing, spreadsheet and visual report design are purchased or deferred. |
| Files, shell and system integration | Folder tree, recent files, drag/drop, clipboard, tray, hotkeys and embedded browser. | R1 owns safe file/clipboard/drag/drop/tray/shell behaviour. WebView2 is an isolated R2 security boundary. A complete Explorer and global hotkeys are later needs. |
| Themes, quality and platform services | Tokens, light/dark/high contrast, accessibility, localisation, DPI, keyboard, commands, undo, state, telemetry, visual/accessibility tests, Gallery and API docs. | These are product-defining quality infrastructure, not optional polish. R1 includes the foundations, tests, Gallery and documentation. |
## Templates and reference applications

| Template/source | Value | Target | Link |
| --- | --- | --- | --- |
| DevExpress UI Template Gallery | Commercial reference for forms, navigation, grids and dashboards | Desktop/Web | https://www.devexpress.com/products/net/controls/winforms/ui-templates/ |
| Telerik demo applications | Hotel, CRM, dashboard and control demos with source by product | Desktop/Web | https://www.telerik.com/support/demos |
| Kendo UI building blocks and Figma kits | Page sections, themes and design resources | Web | https://www.telerik.com/kendo-ui |
| MudBlazor templates | Blazor project templates and layout foundations | Blazor | https://github.com/MudBlazor/Templates |
| Ant Design Pro | Enterprise admin shell, navigation and standard pages | React/design reference | https://pro.ant.design/ |
| MUI templates | Dashboard, sign-in, checkout, blog and marketing patterns | React | https://mui.com/store/ |
| HAVIT Blazor templates | Simple and enterprise Bootstrap/Blazor starters | Blazor | https://havit.blazor.eu/getting-started |
| AdminLTE | Open Bootstrap admin dashboard and widgets | Web | https://github.com/ColorlibHQ/AdminLTE |
| Tabler | High-quality open Bootstrap dashboard kit | Web | https://github.com/tabler/tabler |
| CoreUI Free Admin | Open admin templates for Bootstrap, React, Angular and Vue | Web | https://github.com/coreui/coreui-free-bootstrap-admin-template |
## Proposed SASD template catalogue

1. Component Gallery — executable specification for every state, theme, DPI and culture.
2. CRUD application — navigation, grid, forms, validation, dirty state, persistence and CSV.
3. Workbench application — tree, documents, editor, details, commands and optional docking.
4. Utility application — settings, tray, progress, diagnostics and safe file operations.
5. Dashboard application (R2) — filters, KPI cards, charts and saved views.
6. Wizard application (R3) — guided workflow with validation and recovery.

Every template must include loading/error/empty states, accessibility, localisation, tests, example services, upgrade instructions and no hidden dependency on a demo database.

## Target architecture

```text
SASD applications
  -> stable SASD contracts and component packages
       -> WinForms foundations and owned composite behaviour
       -> optional named adapters
            -> Krypton / ScottPlot / ScintillaNET / WebView2 / other approved dependency

Future product lines
  -> WPF implementation
  -> modern Web/Blazor implementation
  -> ASP.NET Web Forms compatibility project
  -> Java/OpenXava project
```

The shared element across future platforms is primarily **semantics and governance**—tokens, component names, behaviour expectations, state concepts, quality gates and documentation—not one renderer or one binary API.

## Desktop development rules

- Keep public visual controls designer-friendly and non-generic.
- Do not wrap primitive controls without reusable behaviour.
- Use composition, services and controllers before deep inheritance.
- Keep business/domain types out of product packages.
- Treat DPI, keyboard, accessibility, localisation and resource ownership as acceptance criteria.
- Use one visible primary suite per application.
- Put heavy/native third-party dependencies in optional packages.

## Web/AJAX development rules for the future register

- Build modern web controls with semantic HTML, ARIA and framework-native lifecycles.
- Separate headless behaviour, tokens and visual styling where useful.
- Prefer browser-native upload, clipboard and navigation models rather than copying WinForms APIs.
- Treat ASP.NET Web Forms as compatibility/migration work, not the basis of the modern web line.
- Preserve open-core and commercial feature boundaries.

## Quality assurance and Definition of Done

A released SASD component has: an approved requirement and owner; public API documentation; executable Gallery example; unit/integration tests; visual state coverage; DPI, keyboard, accessibility and localisation evidence; licence/provenance review; no unexplained warnings; lifecycle/disposal documentation; changelog and migration note.

## Fork, upstream and supplier strategy

Preferred escalation: normal dependency → SASD adapter → upstream issue/pull request → temporary patch fork → permanent fork only with maintenance capacity. A permanent fork requires licence review, active tests, security ownership, documented divergence, upstream monitoring, release process and exit strategy.

## Deferred separate projects

### SASD ASP.NET Web Forms Compatibility

Historical ASPX controls, AJAX extenders, migration aids and compatibility guidance. AJAX Control Toolkit is a feature reference only because it is archived.

### SASD Java Business UI

Separate study of OpenXava/XavaPro, Vaadin and Java enterprise UI patterns. OpenXava is a model-driven Java/JPA application framework rather than a C# control suite.

### WPF, WinUI 3 and Avalonia

Begin only after WinForms R1. Build one identical reference application per platform and compare designer/tooling, accessibility, deployment, performance, ecosystem and maintenance cost.

## Roadmap summary

| Phase | Outcome |
| --- | --- |
| R0 | Architecture and visual supplier pilots; tokens, base form, dialog and grid spike. |
| R1 | Production WinForms foundation, Gallery, CRUD/workbench/utility templates and first real migrations. |
| R2 | Optional charts, editors, media, PDF, WebView2, docking and advanced data adapters. |
| R3 | Only proven optional needs such as wizard or ribbon. |
| Later | Separate WPF, Web/ASPX and Java product decisions. |

## Research limits and maintenance process

The ecosystem changes continuously. This catalogue is broad but does not claim mathematical completeness. Official product pages and repositories take precedence over comparison sites. Revalidate licence, release activity, security posture and package compatibility before every dependency decision. Review core suppliers quarterly, the wider future register twice per year, and archived/legacy sources only when a migration case exists.

## Selected source and discovery register

- DevExpress, Telerik, Syncfusion, MESCIUS, Infragistics and other official product pages listed above.
- AJAX Control Toolkit archive: https://github.com/DevExpress/AjaxControlToolkit
- Krypton Standard Toolkit: https://github.com/Krypton-Suite/Standard-Toolkit
- ReaLTaiizor: https://github.com/Taiizor/ReaLTaiizor
- Avalonia: https://avaloniaui.net/
- OpenXava: https://www.openxava.org/
- Discovery lists retained as secondary sources: SourceForge and Slashdot DevExpress alternatives, Jon Hilton’s Blazor component-library overview, EDUCBA comparison article and curated Awesome lists.

Secondary lists are useful for discovery but do not determine licence, maintenance or technical suitability.

## Change log

| Version | Date | Change |
| --- | --- | --- |
| 2.0 | 23 July 2026 | WinForms-first English edition; commercial references, 50 desktop projects, 84 Web/AJAX projects, all 123 component classes, priorities, templates, architecture and future registers consolidated. |
