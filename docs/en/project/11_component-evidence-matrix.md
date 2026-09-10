# Component Evidence Matrix

**Product line:** SASD UI Platform — .NET 8 / Windows Forms  
**Revision:** 0.2  
**Status:** Current implementation/evidence inventory  
**Reviewed against:** repository `main` after PR #41, 2026-09-10  
**Scope:** implemented public R1 and dependency-free native R2 visual controls plus application-facing UI services

## 1. Purpose

This matrix answers two different questions that must not be confused:

1. **Is the component implemented?** — the public source exists and is part of the current product line.
2. **Is release-gate evidence complete?** — automated and manual evidence is sufficient for the intended quality gate.

An implemented component is **not** automatically release-ready. In particular, a successful build or smoke check does not prove Visual Studio Designer behavior, real Windows High Contrast behavior, complete keyboard-only usability, UI Automation exposure, or operation at all required DPI/display-scale combinations.

The matrix is intentionally repository-oriented. Paths are kept explicit so developers and coding agents can verify evidence instead of relying on this document as a stale claim.

## 2. Evidence legend

| Mark | Meaning |
| --- | --- |
| **Yes** | Direct evidence exists in the named repository path. |
| **Partial** | The surface is exercised indirectly or only part of its contract is covered. |
| **Manual** | The Showcase/Gallery provides an interactive exercise, but CI does not execute that interaction. |
| **No direct** | No focused automated evidence was identified in the current smoke suites. |
| **N/A** | The evidence type is not useful for this non-visual/service-only surface. |
| **Pending** | A release/manual acceptance activity remains open. |

`examples/Sasd.Ui.PlatformShowcase/Pages/SelfTestPage.cs` is useful application-level self-test code, but the normal CI gate **builds** the Showcase rather than launching and clicking through its UI. Therefore this document does not count Showcase self-tests as automated CI evidence unless the same behavior is also covered by a smoke executable.

## 3. Shared evidence locations

The main reusable evidence surfaces are:

- detailed Component Gallery: `samples/Sasd.Ui.ComponentGallery/MainForm.cs`;
- integrated Gallery shell: `samples/Sasd.Ui.ComponentGallery/GalleryShellForm.cs`;
- native R2 Gallery: `samples/Sasd.Ui.ComponentGallery/R2NativeGalleryPage.cs`;
- integrated application Showcase: `examples/Sasd.Ui.PlatformShowcase/`;
- manual DPI/keyboard observation aid: `examples/Sasd.Ui.PlatformShowcase/Pages/AcceptanceLabPage.cs`;
- canonical gate: `build/verify.ps1`;
- foundation smoke: `tests/smoke/Sasd.Ui.WinFormsSmokeChecks/`;
- forms smoke: `tests/smoke/Sasd.Ui.FormsSmokeChecks/`;
- composite-feedback/lifecycle smoke: `tests/smoke/Sasd.Ui.CompositeFeedbackSmokeChecks/`;
- state smoke: `tests/smoke/Sasd.Ui.StateSmokeChecks/`;
- Windows integration smoke: `tests/smoke/Sasd.Ui.WindowsSmokeChecks/`;
- shell smoke: `tests/smoke/Sasd.Ui.ShellSmokeChecks/`;
- keyboard acceptance smoke: `tests/smoke/Sasd.Ui.KeyboardSmokeChecks/`;
- dialog/threading smoke: `tests/smoke/Sasd.Ui.DialogSmokeChecks/`;
- native R2 smoke: `tests/smoke/Sasd.Ui.NativeR2SmokeChecks/`;
- data/dashboard smoke: `tests/smoke/Sasd.Ui.DataDashboardSmokeChecks/`;
- lifecycle/endurance smoke: `tests/smoke/Sasd.Ui.LifecycleSmokeChecks/`;
- Krypton adapter smoke: `tests/smoke/Sasd.Ui.KryptonSmokeChecks/`.

The keyboard smoke is deliberately counted only for the interactions it actually executes: native Tab/Shift+Tab traversal in a bounded form scenario, global command shortcuts and search-text interaction. It is not evidence that every shell, grid, dialog or document control already has complete keyboard-only coverage.

## 4. WinForms foundation

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdForm` | `src/Sasd.Ui.WinForms/SasdForm.cs` | Yes — Gallery main window derives from it | Yes — application windows derive through SASD shell/base types | Partial — consumer builds, form-state checks and related form checks | Designer open/save, DPI matrix and broader accessibility/UIA remain Pending |
| `SasdDialogForm` | `src/Sasd.Ui.WinForms/SasdDialogForm.cs` | Partial | Yes — `ShowcaseInputDialog.cs` | Yes — `Sasd.Ui.WinFormsSmokeChecks` | Manual dialog focus order, scaling and accessibility Pending |
| `SasdUserControl` | `src/Sasd.Ui.WinForms/SasdUserControl.cs` | Partial | Yes — Showcase self-test/reference usage | No direct focused smoke identified | Designer/DPI/accessibility Pending |
| `SasdUiDispatcher` | `src/Sasd.Ui.WinForms/SasdUiDispatcher.cs` | N/A | Indirect through worker/UI scenarios | Yes — `Sasd.Ui.WinFormsSmokeChecks` | Broader shutdown/handle lifecycle remains ongoing hardening rather than a visual gate |

## 5. Forms and validation

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdSectionPanel` | `src/Sasd.Ui.WinForms.Forms/SasdSectionPanel.cs` | Yes — Overview/Forms | Yes — several Showcase pages | Yes — grouping, spacing and title contract in `Sasd.Ui.FormsSmokeChecks` | Designer, DPI clipping and High Contrast Pending |
| `SasdFieldLayout` | `src/Sasd.Ui.WinForms.Forms/SasdFieldLayout.cs` | Yes — Forms | Yes — Forms and Acceptance Lab | Yes — row/layout, accessibility preservation, required-field semantics, gap validation and child ownership in `Sasd.Ui.FormsSmokeChecks` | Designer, real DPI clipping, long-label/localization and broader keyboard traversal Pending |
| `SasdValidationCoordinator` | `src/Sasd.Ui.WinForms.Forms/SasdValidationCoordinator.cs` | Yes | Yes — Forms | Yes — `Sasd.Ui.WinFormsSmokeChecks` | Real application validation UX/manual focus-on-error still Pending |
| `SasdValidationSummary` | `src/Sasd.Ui.WinForms.Forms/SasdValidationSummary.cs` | Yes | Yes — Forms | Yes — show/clear, accessible text and result projection in `Sasd.Ui.CompositeFeedbackSmokeChecks` | Real screen-reader announcement behavior and DPI Pending |
| `SasdPropertyEditor` | `src/Sasd.Ui.WinForms.Forms/SasdPropertyEditor.cs` | Yes — `R2NativeGalleryPage.cs` | Yes — `AdvancedPage.cs` | Yes — `Sasd.Ui.NativeR2SmokeChecks` | Designer behavior, keyboard-only property editing, DPI and accessibility Pending |

## 6. Data, lists and dashboard controls

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdSearchBox` | `src/Sasd.Ui.WinForms.Data/SasdSearchBox.cs` | Yes — Data | Yes — Data/Self Test | Yes — saved-view/data-dashboard behavior plus bounded keyboard-scenario text change | Keyboard clear-action path, DPI and accessibility/UIA Pending |
| `SasdFilterBar` | `src/Sasd.Ui.WinForms.Data/SasdFilterBar.cs` | Partial/indirect | Yes — Data | Yes — foundation and data/dashboard smoke | Keyboard chip/removal UX, overflow/DPI and accessibility Pending |
| `SasdDataGrid` | `src/Sasd.Ui.WinForms.Data/SasdDataGrid.cs` | Yes — Data | Yes — Data | Yes — foundation, native-R2 and dashboard suites use it | Keyboard-only grid operation, large DPI, High Contrast and UIA Pending |
| `SasdDataGridState` | `src/Sasd.Ui.WinForms.Data/SasdDataGridState.cs` | Yes — save/restore layout | Yes — Data | Yes — foundation and dashboard smoke | Real consumer migration/version evidence remains part of adoption work |
| `SasdDataQuery` and descriptors | `src/Sasd.Ui.WinForms.Data/SasdDataQuery.cs` | Partial | Indirect through data scenarios | Yes — `Sasd.Ui.WinFormsSmokeChecks` and controller smoke | N/A for visual evidence; more consumer combinations may be added as needed |
| `SasdGridController<T>` | `src/Sasd.Ui.WinForms.Data/SasdGridController.cs` | Partial | Yes — Data/Self Test and CRUD reference consumer | Yes — load/search/sort/pager synchronization and disposal in `Sasd.Ui.DataDashboardSmokeChecks` | Cancellation, error and empty-page direct cases remain useful before API stabilization |
| `SasdGridColumnChooser` | `src/Sasd.Ui.WinForms.Data/SasdGridColumnChooser.cs` | Yes — native R2 | Yes — Data | Yes — `Sasd.Ui.NativeR2SmokeChecks` | Keyboard-only chooser operation, DPI and accessibility Pending |
| `SasdGridViewDefinition` | `src/Sasd.Ui.WinForms.Data/SasdGridViewDefinition.cs` | Yes — native R2 | Yes — Data | Yes — `Sasd.Ui.DataDashboardSmokeChecks` | Persistence/versioning in a real consumer remains Pending |
| `SasdPager` | `src/Sasd.Ui.WinForms.Data/SasdPager.cs` | Partial | Yes — `ControlsLabPage.cs` | Yes — clamping, requests, custom page sizes, DPI configuration and accessible state in `Sasd.Ui.DataDashboardSmokeChecks` | End-to-end keyboard interaction, real DPI/High Contrast and UIA Pending |
| `SasdListView` | `src/Sasd.Ui.WinForms.Data/SasdListView.cs` | Partial | Yes — `ControlsLabPage.cs` | Yes — default-contract checks in `Sasd.Ui.WinFormsSmokeChecks` | Real item interaction, keyboard and UIA Pending |
| `SasdTreeView` | `src/Sasd.Ui.WinForms.Data/SasdTreeView.cs` | Partial | Yes — `ControlsLabPage.cs` | Yes — default-contract checks in `Sasd.Ui.WinFormsSmokeChecks` | Expand/collapse keyboard path, large trees and UIA Pending |
| `SasdEmptyState` | `src/Sasd.Ui.WinForms.Data/SasdEmptyState.cs` | Yes — Feedback | Yes — Controls/feedback scenarios | Yes — action, accessibility and visibility contract in `Sasd.Ui.CompositeFeedbackSmokeChecks` | Real focus, High Contrast, DPI and UIA Pending |
| `SasdCsvExporter` | `src/Sasd.Ui.WinForms.Data/SasdCsvExporter.cs` | Yes — Data | Yes — Data exports to an in-memory preview | Yes — `Sasd.Ui.WinFormsSmokeChecks` | Larger datasets/cancellation may be exercised when a consumer requires it; no performance target yet |
| `SasdSparkline` | `src/Sasd.Ui.WinForms.Data/SasdSparkline.cs` | Yes — native R2 | Yes — Overview | Yes — `Sasd.Ui.DataDashboardSmokeChecks` | Visual contrast at DPI/High Contrast and UIA/manual accessible-description review Pending |
| `SasdKpiCard` | `src/Sasd.Ui.WinForms.Data/SasdKpiCard.cs` | Yes — native R2 | Yes — Overview | Yes — `Sasd.Ui.DataDashboardSmokeChecks` | DPI wrapping, High Contrast and screen-reader/UIA review Pending |

## 7. Commands

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdCommand` | `src/Sasd.Ui.WinForms.Commands/SasdCommand.cs` | Yes — Commands and integration shell | Yes — global theme/self-test commands | Yes — foundation plus global-shortcut keyboard smoke | Real consumer command catalog growth only |
| `SasdCommandRunner` | `src/Sasd.Ui.WinForms.Commands/SasdCommandRunner.cs` | Yes | Indirect | Yes — execution, disabled state and re-entry | Cancellation/error reporting can be expanded as real consumers demand |
| `SasdCommandBinding` | `src/Sasd.Ui.WinForms.Commands/SasdCommandBinding.cs` | Yes — button and ToolStrip binding | Indirect through shell command bar | Yes — foundation smoke | Focus/accessibility belongs to the bound visual surface |
| `SasdCommandManager` | `src/Sasd.Ui.WinForms.Commands/SasdCommandManager.cs` | Yes — integration shell through `SasdShellForm` | Yes — Showcase shell | Yes — shell/foundation integration plus registered global-shortcut keyboard flow | Larger registration/removal scenarios may be added before API baseline |

## 8. Shell and navigation

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdShellForm` | `src/Sasd.Ui.WinForms.Shell/SasdShellForm.cs` | Yes — `GalleryShellForm.cs` | Yes — `ShowcaseForm.cs` | Yes/Partial — shell integration smoke and consumer builds | Designer, DPI, complete keyboard-only shell path and UIA Pending |
| `SasdCommandBar` | `src/Sasd.Ui.WinForms.Shell/SasdCommandBar.cs` | Yes | Yes — top command bar | Yes — `Sasd.Ui.WinFormsSmokeChecks` | Overflow, keyboard access, High Contrast and UIA Pending |
| `SasdNavigationHost` | `src/Sasd.Ui.WinForms.Shell/SasdNavigationHost.cs` | Yes — detailed and integration galleries | Yes — primary Showcase navigation | Yes — foundation/shell smoke | Keyboard-only navigation, DPI and accessibility Pending |
| `SasdBreadcrumb` | `src/Sasd.Ui.WinForms.Shell/SasdBreadcrumb.cs` | Partial | Yes — `ControlsLabPage.cs`/Workbench reference | Yes — foundation smoke | Click/keyboard path, truncation and UIA Pending |
| `SasdDocumentTabs` | `src/Sasd.Ui.WinForms.Shell/SasdDocumentTabs.cs` | Partial | Yes — `ControlsLabPage.cs` and Workbench | Yes — foundation smoke + lifecycle endurance | Keyboard tab closing/switching, overflow and UIA Pending |
| `SasdStatusBar` | `src/Sasd.Ui.WinForms.Shell/SasdStatusBar.cs` | Yes | Yes | Yes — foundation/shell behavior | Screen-reader announcement policy and High Contrast Pending |
| `SasdStatusService` / `ISasdStatusService` | `src/Sasd.Ui.WinForms.Shell/SasdStatusService.cs` | Yes — integration gallery | Yes — Showcase status publishing | Yes/Partial — shell integration | Priority/lifetime combinations can be widened; visual acceptance belongs to StatusBar |

## 9. Dialogs and feedback

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdDialogService` / `ISasdDialogService` | `src/Sasd.Ui.WinForms.Dialogs/` | Yes — Feedback | Yes — Feedback | No direct modal interaction smoke identified | Button order, owner behavior, keyboard, DPI and accessibility Pending |
| `SasdErrorDialog` | `src/Sasd.Ui.WinForms.Dialogs/SasdErrorDialog.cs` | Yes — detailed error action | Yes — Feedback | No direct focused smoke identified | Long details, copy/select behavior, DPI and accessibility Pending |
| `SasdBusyOverlay` | `src/Sasd.Ui.WinForms.Dialogs/SasdBusyOverlay.cs` | Yes | Yes — Feedback | Yes — busy/idle state and accessible-message synchronization in `Sasd.Ui.CompositeFeedbackSmokeChecks` | Real focus blocking, resize/DPI, High Contrast and UIA Pending |
| `SasdProgressDialog` | `src/Sasd.Ui.WinForms.Dialogs/SasdProgressDialog.cs` | Partial | Yes — cancellable worker demo | Yes — `Sasd.Ui.DialogSmokeChecks`; startup threading hardened by PR #25 | Manual cancellation/focus/DPI/accessibility Pending |
| `SasdNotificationService` | `src/Sasd.Ui.WinForms.Dialogs/SasdNotificationService.cs` | Yes — integration gallery | Yes — Feedback | Yes — foundation smoke publication contract | Service itself N/A visually; host behavior still needs manual accessibility review |
| `SasdNotificationHost` | `src/Sasd.Ui.WinForms.Dialogs/SasdNotificationHost.cs` | Yes — integration gallery | Yes — Feedback | Yes — bind/unbind/dismiss/dispose plus pre-handle worker publication and latest-value hand-off in `Sasd.Ui.CompositeFeedbackSmokeChecks` | Timer expiry, focus policy, DPI/High Contrast and UIA/screen-reader behavior Pending |

## 10. UI state and recent items

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdStateStore` | `src/Sasd.Ui.WinForms.State/SasdStateStore.cs` | Yes — Gallery window/grid persistence | Yes — `StatePage.cs` | Yes — persistence, backup recovery, remove/reset and migrations in `Sasd.Ui.StateSmokeChecks` | Real consumer upgrade/migration evidence Pending |
| `SasdFormStateService` | `src/Sasd.Ui.WinForms.State/SasdFormStateService.cs` | Yes — Gallery window restore | Partial | Yes — keyed/no-key save, restore and off-screen recovery through a real invisible STA message loop in `Sasd.Ui.StateSmokeChecks` | Real multi-monitor movement and changed-DPI migration evidence Pending |
| `SasdRecentItemsService` | `src/Sasd.Ui.WinForms.State/SasdRecentItemsService.cs` | Partial | Yes — `StatePage.cs` / Showcase self-test | Yes — deterministic ordering, limit, deduplication, normalization, remove and clear in `Sasd.Ui.StateSmokeChecks` | Real consumer MRU adoption/upgrade evidence Pending |
| `SasdStateMigration` | `src/Sasd.Ui.WinForms.State/SasdStateMigration.cs` | N/A | Indirect | Yes — incremental migration in `Sasd.Ui.StateSmokeChecks` | Real-version migration chains remain adoption evidence |
| window-state model/helpers | `src/Sasd.Ui.WinForms.State/SasdWindowState.cs` | Yes — form-state usage | Partial | Yes/Partial — explicit impossible/off-screen placement recovery is covered | Multi-monitor and changed-DPI manual evidence Pending |

## 11. Theming and semantic icons

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdThemeService` | `src/Sasd.Ui.WinForms.Theming/SasdThemeService.cs` | Yes — Light/Dark/High Contrast picker | Yes — global theme commands and Acceptance Lab snapshot | Partial — consumer/self-test coverage; no dedicated full visual smoke | Real Windows High Contrast, all target controls, DPI and focus visibility Pending |
| `SasdSystemIconService` / semantic icon contract | `src/Sasd.Ui.WinForms.Theming/SasdSystemIconService.cs` | Partial | Yes — Windows/Showcase | Yes — cache, dimensions and disposal in foundation smoke | Visual meaning/contrast review at target DPI and High Contrast Pending |

## 12. Windows integration

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdFileDialogService` | `src/Sasd.Ui.WinForms.Windows/SasdFileDialogService.cs` | Yes/Partial | Yes — Windows page | No direct dialog automation | Manual owner/filter/cancel/keyboard/DPI checks Pending |
| `SasdClipboardService` | `src/Sasd.Ui.WinForms.Windows/SasdClipboardService.cs` | Partial | Yes — Windows page | No direct focused smoke identified | Manual clipboard availability/failure cases Pending |
| `SasdShellService` | `src/Sasd.Ui.WinForms.Windows/SasdShellService.cs` | Partial | Yes — safe constrained example | Yes — unsafe/missing path and URI-scheme rejection in foundation smoke | Successful OS launch stays a manual/environment-dependent check |
| `SasdDragDropService` | `src/Sasd.Ui.WinForms.Windows/SasdDragDropService.cs` | Yes — integration gallery | Yes — Windows page | Yes — policy, duplicate, count and unsupported-format behavior in Windows smoke | Real Explorer drag/drop, keyboard alternative and accessibility Pending |
| `SasdTrayService` / `ISasdTrayService` | `src/Sasd.Ui.WinForms.Windows/SasdTrayService.cs` | Partial | Yes — explicit Show/Hide exercise | Yes — Windows smoke + lifecycle endurance, without displaying an icon in CI | Actual Explorer notification-area menu/double-click behavior remains Manual |

## 13. Native R2 media

| Public surface | Source | Gallery | Showcase/manual exercise | Automated smoke | Remaining evidence |
| --- | --- | --- | --- | --- | --- |
| `SasdImageViewer` | `src/Sasd.Ui.WinForms.Media/SasdImageViewer.cs` | Yes — native R2 | Yes — `AdvancedPage.cs` | Yes — native R2 smoke + lifecycle endurance | Large-image/manual scrolling, DPI, High Contrast and accessibility Pending |

## 14. Optional Krypton pilot

The optional Krypton adapter remains isolated under `src/Sasd.Ui.WinForms.Krypton/` and has `tests/smoke/Sasd.Ui.KryptonSmokeChecks/`. That proves adapter-level compatibility checks, **not** the final native-versus-Krypton visual-standard decision. The R0.2 decision gate, Visual Studio Designer matrix, DPI/focus behavior and High Contrast review remain open and must not be inferred from a green adapter smoke run.

## 15. Reference-consumer evidence

Three application-like consumers currently add important compile-time and manual integration evidence:

| Consumer | Path | Primary value |
| --- | --- | --- |
| CRUD reference application | `samples/Sasd.Ui.Sample.Crud/` | Realistic in-memory list/form/command composition |
| Workbench reference application | `samples/Sasd.Ui.Sample.Workbench/` | Document-oriented shell/tree/breadcrumb/tab composition and ownership |
| Integrated Platform Showcase | `examples/Sasd.Ui.PlatformShowcase/` | Broad manual exercise, public-API self-test surface and acceptance observation lab |

All three are restored and built by `build/verify.ps1` with warnings treated as errors. A successful consumer build proves public composition and compile/analyzer compatibility; it does not replace interactive acceptance testing.

## 16. Highest-value evidence gaps

The inventory shows that the next quality work should favor **evidence depth rather than more component count**. Several gaps identified in revision 0.1 are now closed: pager/controller state, recent items, form-state restore, core composite feedback and one bounded keyboard acceptance flow all have direct executable evidence. The highest-value remaining gaps are currently:

1. deepen direct edge-case coverage where behavior is still only partial, especially `SasdGridController<T>` cancellation/error/empty-page handling and selected feedback/Windows failure paths that can be exercised without external desktop side effects;
2. extend keyboard-only automated flows beyond the current bounded form/global-shortcut scenario into shell navigation, grids, dialogs and document-tab operations where deterministic WinForms routing is possible;
3. formal Visual Studio Designer open/edit/save evidence for designer-supported controls;
4. recorded 100%, 125%, 150% and 200% DPI/display-scale runs, including mixed-DPI monitor moves where hardware permits;
5. real Windows High Contrast review rather than only selecting the SASD High Contrast theme;
6. UI Automation/screen-reader evidence. Existing accessible names/descriptions are useful implementation work but do not by themselves prove UIA acceptance;
7. first bounded real SASD application migration after release-engineering/API-baseline work is sufficiently stable.

Third-party UI automation, screenshot-regression tooling or a new visual framework still require the project decision boundary defined in `AGENTS.md` and the roadmap.

## 17. Maintenance rule

Update this matrix whenever a public R1/R2 component/service is added or removed, a new Gallery/Showcase scenario lands, a focused smoke suite changes, or a manual acceptance gate is actually completed. Do not change `Pending` to complete based only on code inspection or a successful compilation.

The canonical machine-verifiable gate remains:

```powershell
pwsh ./build/verify.ps1
```

On non-Windows systems, `-CompileOnly` is useful but must not be reported as execution of WinForms runtime smoke checks.
