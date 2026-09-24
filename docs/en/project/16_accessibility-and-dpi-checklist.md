# Accessibility, Keyboard, and DPI Checklist

## 1. Keyboard

- [ ] Every interactive element is reachable with Tab.
- [ ] Tab order follows the visual and task order.
- [ ] Focus is visible in Light, Dark, and High Contrast.
- [ ] Enter/Escape behavior is correct in dialogs.
- [ ] Shortcuts do not collide.
- [ ] Context actions have a keyboard alternative.
- [ ] Focus returns appropriately after a dialog or overlay.
- [ ] No keyboard trap exists except a deliberate modal busy state with cancellation.

## 2. Accessibility

- [ ] Meaningful `AccessibleName`.
- [ ] Appropriate role/control type.
- [ ] Value and state are available.
- [ ] Errors are not communicated by color alone.
- [ ] Icons have text alternatives where their meaning is not redundant.
- [ ] Labels are associated with input fields.
- [ ] Validation Summary can navigate to the invalid field.
- [ ] Dynamic status changes are announced appropriately.
- [ ] `AutomationId` values are stable and documented.

## 3. Color and Contrast

- [ ] Status is not conveyed exclusively by color.
- [ ] Text/background contrast is sufficient.
- [ ] Disabled state remains recognizable.
- [ ] Focus outline is not hidden by the theme.
- [ ] High Contrast respects system colors or provides a safe fallback.

## 4. DPI and Layout

- [ ] 100%, 125%, 150%, and 200% reviewed.
- [ ] Per-monitor movement reviewed.
- [ ] Minimum sizes are sensible.
- [ ] Text is not clipped.
- [ ] Icons are not blurry or incorrectly scaled.
- [ ] Dialog buttons remain fully visible.
- [ ] Small windows scroll instead of overlapping content.
- [ ] Stored window position is validated against current monitors.

## 5. Localization

- [ ] German and English.
- [ ] Expanded/pseudo-localized text.
- [ ] Date, number, and currency formatting.
- [ ] No hard-coded visible strings in reusable controls.
- [ ] Tooltips and error messages are localized.

## 6. Review Tools

- UI Automation/FlaUI;
- Accessibility Insights for Windows;
- Component Gallery;
- screenshot matrix;
- manual keyboard-only review;
- Windows High Contrast and multiple scaling levels.

## 7. Deviations

A deviation records component, state, user impact, workaround, target release, and approval decision. Critical keyboard or focus defects block an R1 release.
