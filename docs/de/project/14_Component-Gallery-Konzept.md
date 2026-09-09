# Component Gallery – Konzept

## 1. Rolle

Die Component Gallery ist ausführbare Spezifikation, Testhost, Dokumentationsbeispiel und visuelle Referenz. Sie ist nicht nur eine Marketingdemo.

## 2. Informationsarchitektur

- Grundlagen: Tokens, Themes, Typografie, Icons;
- Basiscontrols: Form, Dialog, UserControl, Section, FieldLayout;
- Zustände: Busy, Empty, Error, Validation;
- Shell: Navigation, Breadcrumb, Tabs, Commands, Status;
- Data: Grid, Liste, Baum, Search, Filter, Paging;
- Dialogs/Windows: File, Clipboard, DragDrop, Tray;
- R2-Adapter: Charts, Editors, Media, PDF, WebView, Docking.

## 3. Jede Gallery-Seite enthält

- Zweck und geeignete Einsatzfälle;
- Nichtziele und bekannte Grenzen;
- Minimalbeispiel;
- erweitertes Beispiel;
- Normal/Hover/Focus/Disabled/ReadOnly/Busy/Empty/Error;
- Theme- und High-Contrast-Umschaltung;
- DPI- und Accessibility-Hinweise;
- AutomationId und Testreferenzen;
- relevante Lasten-/Pflichtenheft-IDs;
- verwendete Pakete und Lizenzen bei Adaptern.

## 4. Interaktive Prüfwerkzeuge

- Kulturumschaltung Deutsch/Englisch/Pseudo;
- DPI-/Größensimulation soweit technisch sinnvoll;
- Themewechsel;
- lange Texte;
- Fehlersimulation;
- langsame/cancelbare Operation;
- große/kleine/leere Datenmenge;
- Reset gespeicherter Zustände.

## 5. Screenshotbaselines

Baselines werden nur auf definierter Runner-/VM-Konfiguration erzeugt. Aktualisierung benötigt Review; „alles neu akzeptieren“ ohne Ursachenprüfung ist unzulässig.

## 6. Gallery als Consumer

Die Gallery referenziert veröffentlichbare Paketgrenzen genauso wie reale Anwendungen. Sie darf keine intern verbotenen Abhängigkeiten nutzen, nur um Beispiele einfacher zu machen.

## 7. Beispielcode

Codeblöcke sollen kopierbar und kompilierbar sein. Für komplexe Seiten wird der vollständige Sourcepfad angegeben. Beispiele zeigen auch Fehler- und Dispose-Pfade, nicht nur Happy Path.
