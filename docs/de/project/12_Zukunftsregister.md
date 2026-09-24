# Zukunftsregister – spätere Produktlinien

## 1. Zweck

Das Zukunftsregister bewahrt gute Ideen, ohne den aktuellen WinForms-Scope aufzublähen. Ein Eintrag ist keine Zusage zur Umsetzung.

## 2. Wiederverwendbare Grundlagen

Plattformübergreifend wiederverwendbar sind vor allem:

- Design Tokens und semantische Namen;
- UX-Regeln für Fokus, Fehler, Busy, Empty, Navigation und Commands;
- Paging-/Filter-/Result-Semantik;
- Qualitäts- und Testkriterien;
- Dokumentation und Architekturentscheidungen.

WinForms-Controlcode wird nicht automatisch übernommen.

## 3. WPF

Späteres separates Projekt mit XAML und MVVM-nahen Mustern. Zu prüfen:

- WPF UI, MahApps.Metro, MaterialDesignInXaml;
- AvalonDock/PropertyTools;
- Mapping der SASD Design Tokens auf Resources;
- gemeinsame fachneutrale Contracts ohne WinForms-Abhängigkeit.

## 4. WinUI 3

Nach Reife von WinForms/WPF als Pilot für moderne Windows-App-SDK-Oberflächen. Getrennt bewerten:

- Packaging und Deployment;
- Designer-/Hot-Reload-Erfahrung;
- Interop mit bestehenden Win32-/WinForms-Modulen;
- Control-Reife und Langzeitwartung.

## 5. Avalonia

Nur bei realem Bedarf für Windows/Linux/macOS-Desktop. Vor Start erneut prüfen:

- OSS-/Pro-Grenzen;
- DataGrid/TreeDataGrid/Docking;
- Designer/Previewer;
- Packaging, Accessibility und Plattformqualität.

Aktuell sind Linux Desktop und macOS nicht geplant.

## 6. ASP.NET Core/Blazor

Eigenes modernes Webprojekt. Kandidaten:

- MudBlazor;
- Fluent UI Blazor;
- daisyUI/Tailwind;
- JavaScript-/Web-Component-Spezialbibliotheken.

Ziel wäre eine gemeinsame Designsemantik, nicht identischer Controlcode.

## 7. ASP.NET Web Forms/ASPX

Eigenes Legacy-/Migrationsprojekt. Das archivierte AJAX Control Toolkit dient als Funktions- und Migrationsreferenz, nicht als neue strategische Basis. Themen:

- bestehende Web-Forms-Anwendungen modernisieren;
- serverseitige Postbacks reduzieren;
- schrittweise Migration zu ASP.NET Core/Blazor;
- Sicherheits- und Browserkompatibilität.

## 8. Java/OpenXava

Eigenes Java-Projekt. OpenXava ist eher modellgetriebene Anwendungsplattform als klassische Control-Sammlung. Zu untersuchen:

- JPA-/Domänenmodell-getriebene CRUD-Oberflächen;
- Listen, Filter, Export, Detailmasken und Dashboards;
- XavaPro versus Open Source;
- Übertragbarkeit von SASD UX- und Dokumentationsstandards.

## 9. Mobile und weitere Desktops

Android, iOS, macOS und Linux Desktop sind derzeit nicht geplant. Sie bleiben als mögliche spätere Untersuchung dokumentiert, erhalten aber keine Projekte, Abhängigkeiten oder versteckten Vorimplementierungen im WinForms-Repository.

## 10. Reaktivierungskriterien

Eine Zukunftslinie wird erst aktiviert, wenn:

- mindestens ein konkretes Produkt-/Kundenszenario besteht;
- Kapazität nicht die WinForms-Stabilisierung gefährdet;
- Zielplattform und Technologieentscheidung separat dokumentiert werden;
- eigener Komponenten- und Lieferantenscope erstellt wird;
- Wiederverwendung und Nichtwiederverwendung klar abgegrenzt sind.
