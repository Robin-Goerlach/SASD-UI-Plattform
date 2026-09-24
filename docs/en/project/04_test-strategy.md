# Test Strategy

## 1. Goal

The strategy proves not only functional correctness but also designer compatibility, DPI behavior, accessibility, resource cleanup, and consumer usability.

## 2. Test Pyramid

| Level | Purpose | Examples |
| --- | --- | --- |
| Unit | Pure logic | Tokens, commands, filters, migration, validators |
| Architecture | Structural rules | Dependency graph, third-party types in public APIs, namespaces |
| Integration | File system and services | StateStore, PDF, barcode, clipboard fakes |
| Component | Control in a test host | States, events, theme changes, disposal |
| UI Automation | User workflows | Navigation, dialogs, grid, keyboard |
| Visual Regression | Visual deviations | Theme, DPI, long text, High Contrast |
| Manual | Hard-to-automate quality | Designer, multi-monitor behavior, Accessibility Insights |

## 3. Binding Test Matrix

### Operating System and Runtime

- Windows 11 as the primary reference;
- Windows 10 while it remains a supported target and a runner is available;
- .NET 8 Release build;
- an additional current-LTS compatibility check without unnecessary multi-targeting.

### DPI

- 100%, 125%, 150%, and 200%;
- movement between monitors with different scaling;
- window restoration after DPI or monitor changes;
- no clipped labels or invisible buttons.

### Themes

- Light;
- Dark;
- High Contrast or a system-aligned contrast mode;
- runtime switching without restart where supported.

### Culture and Text

- German;
- English;
- pseudo-localization and expanded text;
- dates, numbers, currency, and sorting.

## 4. Component Acceptance

Every visual component demonstrates and tests, where applicable:

- Normal;
- Hover;
- Focus;
- Disabled;
- ReadOnly;
- Busy;
- Empty;
- Error/Warning;
- long text;
- High Contrast;
- keyboard operation.

## 5. Grid Test Cases

- column sorting and multi-sort according to scope;
- combined search and filtering;
- selection changes and selection persistence;
- editing and validation errors;
- CSV export with culture and escaping checks;
- stored column order and width;
- paging with cancellation and race-condition protection;
- empty, small, and larger data sets;
- in-memory and simulated remote data sources.

## 6. StateStore Test Cases

- first start without a file;
- successful atomic write;
- process interruption between temporary file and replace;
- corrupted JSON;
- unknown fields;
- older `SchemaVersion`;
- failed migration;
- backup and reset;
- no secrets in known state types.

## 7. Resource and Endurance Tests

- 100 open/close cycles for relevant windows;
- GDI/USER handles before and after the cycle;
- GC collection of closed views;
- cancellation of timers, events, and tasks;
- WebView2/editor processes after R2;
- bounded image and icon caches.

## 8. UI Automation Rules

- stable `AutomationId` values instead of visible text as the primary selector;
- explicit wait conditions instead of sleeps;
- small, business-critical core flows;
- reproducible test state;
- flaky tests are not silently ignored but documented with cause and quarantine deadline.

## 9. Release Rule

A release is not approved when a critical test is red, a new visual component has no Gallery coverage, or known handle/designer problems remain without an accepted exception.
