# Component Gallery – Concept

## 1. Role

The Component Gallery is an executable specification, test host, documentation example, and visual reference. It is not merely a marketing demo.

## 2. Information Architecture

- Foundations: tokens, themes, typography, icons;
- Base controls: form, dialog, user control, section, field layout;
- States: busy, empty, error, validation;
- Shell: navigation, breadcrumb, tabs, commands, status;
- Data: grid, list, tree, search, filter, paging;
- Dialogs/Windows: file, clipboard, drag-and-drop, tray;
- R2 adapters: charts, editors, media, PDF, WebView, docking.

## 3. Every Gallery Page Includes

- purpose and suitable use cases;
- non-goals and known boundaries;
- minimal example;
- advanced example;
- Normal/Hover/Focus/Disabled/ReadOnly/Busy/Empty/Error states;
- theme and High Contrast switching;
- DPI and accessibility guidance;
- `AutomationId` and test references;
- relevant requirements and functional-specification IDs;
- packages and licenses used by adapters.

## 4. Interactive Review Tools

- culture switching among German, English, and pseudo-localization;
- DPI/size simulation where technically meaningful;
- theme switching;
- expanded text;
- error simulation;
- slow and cancellable operation;
- large, small, and empty data sets;
- reset of stored state.

## 5. Screenshot Baselines

Baselines are created only on a defined runner/VM configuration. Updating them requires review; accepting every new image without root-cause analysis is prohibited.

## 6. Gallery as a Consumer

The Gallery references publishable package boundaries exactly as real applications do. It may not use prohibited internal dependencies merely to simplify examples.

## 7. Example Code

Code blocks should be copyable and compilable. Complex pages identify the full source path. Examples include failure and disposal paths, not only the happy path.
