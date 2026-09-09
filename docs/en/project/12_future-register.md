# Future Register – Later Product Lines

## 1. Purpose

The future register preserves valuable ideas without inflating the current WinForms scope. An entry is not a commitment to implement it.

## 2. Reusable Foundations

The following are particularly reusable across platforms:

- design tokens and semantic names;
- UX rules for focus, errors, busy, empty, navigation, and commands;
- paging, filtering, and result semantics;
- quality and test criteria;
- documentation and architecture decisions.

WinForms control code is not adopted automatically.

## 3. WPF

A later separate project using XAML and MVVM-aligned patterns. Evaluate:

- WPF UI, MahApps.Metro, MaterialDesignInXaml;
- AvalonDock and PropertyTools;
- mapping SASD design tokens to resources;
- shared business-neutral contracts without WinForms dependencies.

## 4. WinUI 3

After WinForms/WPF maturity, use a pilot for modern Windows App SDK interfaces. Evaluate separately:

- packaging and deployment;
- designer and Hot Reload experience;
- interoperability with existing Win32/WinForms modules;
- control maturity and long-term maintenance.

## 5. Avalonia

Only when there is real demand for Windows/Linux/macOS desktop. Before starting, reassess:

- OSS/Pro boundaries;
- DataGrid, TreeDataGrid, and docking;
- designer/previewer;
- packaging, accessibility, and platform quality.

Linux desktop and macOS are not currently planned.

## 6. ASP.NET Core/Blazor

A separate modern web project. Candidates include:

- MudBlazor;
- Fluent UI Blazor;
- daisyUI/Tailwind;
- specialist JavaScript and Web Component libraries.

The goal is shared design semantics, not identical control code.

## 7. ASP.NET Web Forms/ASPX

A separate legacy and migration project. The archived AJAX Control Toolkit is a functional and migration reference, not a new strategic foundation. Topics include:

- modernizing existing Web Forms applications;
- reducing server-side postbacks;
- incremental migration to ASP.NET Core/Blazor;
- security and browser compatibility.

## 8. Java/OpenXava

A separate Java project. OpenXava is closer to a model-driven application platform than a conventional control suite. Evaluate:

- JPA/domain-model-driven CRUD interfaces;
- lists, filters, export, detail forms, and dashboards;
- XavaPro versus open source;
- transfer of SASD UX and documentation standards.

## 9. Mobile and Additional Desktops

Android, iOS, macOS, and Linux desktop are not currently planned. They remain documented as possible future investigations but receive no projects, dependencies, or hidden pre-implementation in the WinForms repository.

## 10. Activation Criteria

A future line is activated only when:

- at least one concrete product or customer scenario exists;
- capacity does not endanger WinForms stabilization;
- target platform and technology decisions are documented separately;
- a dedicated component and vendor scope is created;
- reuse and non-reuse are clearly separated.
