# R1 Manual Acceptance Record

**Purpose:** release evidence for checks that cannot be inferred from compilation or headless smoke tests  
**Initial status:** every check is `NOT RUN` until a human executes and records it  
**Related:** [R1.0 closing plan](r1-closing-plan.md) · [Accessibility/DPI checklist](16_accessibility-and-dpi-checklist.md) · [Integrated Showcase](../../../examples/Sasd.Ui.PlatformShowcase/README.md)

## 1. Evidence header

Fill this section for each acceptance session.

| Field | Recorded value |
| --- | --- |
| Repository commit SHA | NOT RUN |
| Tester | NOT RUN |
| Date/time | NOT RUN |
| Windows edition/build | NOT RUN |
| .NET SDK | NOT RUN |
| Visual Studio version | NOT RUN |
| Display/monitor arrangement | NOT RUN |
| Display scales | NOT RUN |
| Windows High Contrast state/theme | NOT RUN |
| Screen-reader/UIA tool used | NOT RUN |
| Notes/evidence location | NOT RUN |

Allowed result values are `PASS`, `FAIL`, `NOT RUN`, and `N/A`. A failure must reference a reproducible issue or note. Do not replace a failed observation with a later pass without retaining the earlier defect history in the issue/PR record.

## 2. Visual Studio Designer matrix

Use a clean checkout/build before the session. For each representative designer-supported surface, open the containing form/control in the Visual Studio WinForms Designer, make a harmless reversible property/layout edit, save, close the designer, reopen it and verify that serialization remains stable.

| Surface | Open | Edit/save | Reopen | No unexpected generated changes | Result/notes |
| --- | --- | --- | --- | --- | --- |
| Native `SasdForm` host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| `SasdDialogForm` host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| `SasdUserControl` host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| `SasdSectionPanel` + `SasdFieldLayout` | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| Representative Data controls | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| Representative Shell controls | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| Krypton pilot host | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |

The Krypton row is evidence for the later native-vs-Krypton decision; it does not imply that Krypton is the default.

## 3. DPI and layout matrix

Run the Integrated Showcase and use its Acceptance Lab. At every available scale, inspect startup, resize behaviour, text clipping, control overlap, focus visibility, icon rendering and dialog button visibility.

| Scale | Overview | Forms | Data/grid | Controls | Feedback/dialogs | Acceptance Lab snapshot recorded | Result/notes |
| ---: | --- | --- | --- | --- | --- | --- | --- |
| 100% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| 125% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| 150% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |
| 200% | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | NOT RUN | |

### Mixed-DPI movement

| Scenario | Result | Notes |
| --- | --- | --- |
| Move Showcase from lower-DPI to higher-DPI monitor | NOT RUN | |
| Move Showcase from higher-DPI to lower-DPI monitor | NOT RUN | |
| Restore stored window state after monitor/scale change | NOT RUN | |

Use `N/A` only when the available hardware genuinely cannot provide the scenario, and record the hardware limitation.

## 4. Real Windows High Contrast

Enable a real Windows High Contrast/Contrast theme. Do not treat selecting the SASD High Contrast theme alone as equivalent evidence.

| Check | Result | Notes |
| --- | --- | --- |
| Text and backgrounds remain readable | NOT RUN | |
| Focus indicators remain visible | NOT RUN | |
| Validation/status meaning is not conveyed by color alone | NOT RUN | |
| Data/list/tree selection remains visible | NOT RUN | |
| Dialog buttons and cancellation remain discoverable | NOT RUN | |
| Native-vs-Krypton comparison observations recorded | NOT RUN | |

## 5. Keyboard-only representative flows

Do not use the mouse during each flow.

| Flow | Expected outcome | Result | Notes |
| --- | --- | --- | --- |
| Acceptance Lab Tab / Shift+Tab route | Focus follows task order in both directions | NOT RUN | |
| SearchBox edit → Enter → Escape/clear | Search request and clear behavior match documented contract | NOT RUN | |
| Validation Summary → invalid field | Selected validation message moves focus to the registered field | NOT RUN | |
| Breadcrumb overflow | Ellipsis and hidden ancestors are reachable and invokable | NOT RUN | |
| Document tabs | Representative select/close workflow remains keyboard reachable | NOT RUN | |
| BusyOverlay cancellation | Focus remains inside overlay and cancellation is available | NOT RUN | |
| Representative dialog | Enter/Escape and owner/focus return behave correctly | NOT RUN | |

## 6. Accessibility / UI Automation review

Use Windows Narrator and/or an explicitly approved inspection tool. Record the tool and version in the evidence header.

| Surface | Name/role/state exposed | Focus/order understandable | Dynamic/error information understandable | Result/notes |
| --- | --- | --- | --- | --- |
| Forms and validation | NOT RUN | NOT RUN | NOT RUN | |
| Search/filter/pager | NOT RUN | NOT RUN | NOT RUN | |
| List/tree/grid | NOT RUN | NOT RUN | NOT RUN | |
| Breadcrumb/document tabs/shell | NOT RUN | NOT RUN | NOT RUN | |
| Busy/progress/notifications | NOT RUN | NOT RUN | NOT RUN | |

Automated accessible-property checks support this review but cannot be substituted for it.

## 7. Localization and long-label review

| Scenario | Result | Notes |
| --- | --- | --- |
| Representative German labels/content | NOT RUN | |
| Representative English labels/content | NOT RUN | |
| Intentionally expanded/long labels | NOT RUN | |
| Dates/numbers in representative consumer content | NOT RUN | |
| No critical clipping or inaccessible overflow | NOT RUN | |

## 8. Session conclusion

| Field | Value |
| --- | --- |
| Overall session result | NOT RUN |
| Blocking defects | NOT RUN |
| Non-blocking observations | NOT RUN |
| Follow-up issue/PR references | NOT RUN |
| Re-test required | NOT RUN |

A session does not close an R1 gate merely because most rows pass. Any critical keyboard/focus defect, unrecoverable Designer problem or material DPI/High-Contrast failure remains an R1 release blocker until fixed or explicitly accepted through the project decision process.
