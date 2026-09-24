# Migration and Adoption Guide

## 1. Principle

Existing SASD applications migrate incrementally using the strangler pattern. There is no big-bang rewrite and no requirement to replace every UI area at once.

## 2. Preparation

- document current screenshots and core workflows;
- inventory local UI helpers, dialogs, theme code, grid state, and error presentation;
- mark technical debt and known usability problems;
- create baseline tests for critical workflows;
- define target packages and migration scope.

## 3. Recommended Order

1. themes and icons;
2. dialog/error/progress;
3. StateStore and recent items;
4. search/filter/grid state;
5. form layout and validation;
6. commands;
7. shell/navigation/tabs;
8. optional R2 modules.

This sequence provides early value with lower structural risk.

## 4. Prompt Manager

Suitable first scope:

- theme and icons;
- `SasdFieldLayout` in clearly bounded dialogs;
- central error presenter;
- SearchBox/FilterBar;
- grid state and CSV export;
- StateStore.

Not in the first step: replacing the entire navigation system or changing the data model at the same time.

## 5. Mail Workbench

- status/progress and error flow;
- recent items;
- files/clipboard/drag-and-drop;
- command contracts;
- gradual shell regions.

Docking and code/Markdown editors remain R2 and are not pulled forward merely because the application may benefit from them later.

## 6. Notes

- tree/list contracts;
- document tabs and dirty state;
- StateStore;
- workbench shell.

Markdown preview/WebView2 follow only after the R2 security and lifecycle pilot.

## 7. Utility/TaskHost

A small application is suitable as an early productive test bed for BaseForm, dialogs, settings, tray support, and progress.

## 8. Compatibility Adapters

Temporary adapters may map old local interfaces to new SASD services. They have:

- a clear name and expiration date;
- no new business logic;
- tests for old and new paths;
- documented removal after migration.

## 9. Rollback

- migrate through small PRs;
- remove the old implementation only after the new version is proven in production;
- back up UI state before schema changes;
- use feature flags only where they provide a real rollback path and do not create permanent complexity.

## 10. Acceptance Criteria

A partial migration is complete when:

- local duplicate logic is removed or clearly marked as residual work;
- workflows and keyboard operation are unchanged or improved;
- no new direct third-party dependency is introduced in the application where a SASD package is intended;
- state/layout migration is tested;
- known deviations are documented.
