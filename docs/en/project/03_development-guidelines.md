# Development Guidelines

## 1. C# Baseline Rules

- Nullable Reference Types are enabled.
- Public APIs use unambiguous names and XML documentation.
- `async void` is permitted only for genuine UI event handlers.
- Asynchronous operations accept a `CancellationToken` where meaningful.
- Exceptions are not used as normal control flow.
- Result objects represent expected user or operational errors; programming defects may fail visibly.
- Global static state is avoided.

## 2. WinForms and Designer Rules

- public visual controls have parameterless constructors;
- designer paths perform no file, network, database, or dependency-injection operations;
- generic controls are not published directly as designer surfaces;
- designer values survive closing and reopening the designer;
- UI-thread access runs through `SasdUiDispatcher` or controlled control invocation;
- UI controls start no hidden background threads without a documented lifecycle.

## 3. Inheritance and Composition

Inheritance is limited to stable base classes (`SasdForm`, `SasdDialogForm`, `SasdUserControl`). Application behavior belongs in services, controllers, binders, and commands. New deep class hierarchies require an ADR.

## 4. Public APIs

- Core packages return no Krypton, ScottPlot, ScintillaNET, or WebView2 types.
- Vendor-specific extensions live in a clearly named adapter package.
- Optional parameters use secure defaults.
- Events have a clear sender and documented lifetime.
- `IDisposable`/`IAsyncDisposable` ownership is described explicitly.
- New API is used in a consumer sample before merge.

## 5. Commands and User Actions

- the same action is bound through one command instance to menus, toolbars, context menus, and shortcuts;
- `CanExecute` performs no expensive operation;
- parallel execution is explicitly permitted or prevented;
- failures flow to the central error presenter;
- long-running commands support busy/progress and cancellation.

## 6. Error Handling

- users receive understandable, action-oriented messages;
- technical details are separately copyable;
- logs include correlation, component, and operation, but no secrets;
- optional UI functions must not prevent startup;
- corrupted layout or state files are reset in isolation.

## 7. Resource Management

- bitmaps, icons, streams, timers, handles, and third-party controls are disposed;
- global events and services use explicit unsubscription or weak coupling;
- caches have limits and documented ownership;
- closed views must be collectible by the GC.

## 8. Localization and Formatting

- shared text lives in platform resources;
- application text remains in the consumer;
- technical persistence formats are culture-invariant;
- visible numbers, dates, and currency use the active culture;
- layouts are tested with expanded text.

## 9. Comments

Comments explain decisions, risks, or non-obvious constraints. They do not repeat the code. Public XML comments include purpose, important parameters, exceptions or error results, and a short example when usage is not self-explanatory.

## 10. Prohibited Patterns

- service locator inside controls;
- direct database access from UI components;
- uncontrolled `Application.DoEvents()` loops;
- `Thread.Sleep` for UI synchronization;
- absolute paths or machine-specific Registry assumptions;
- hidden telemetry;
- thin wrappers that only rename primitive controls;
- mixing several visible theme systems in one application.
