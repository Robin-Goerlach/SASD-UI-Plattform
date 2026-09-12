# Sasd.Ui.WinForms.Forms

Reusable form-layout, validation and property-editing components for SASD WinForms applications.

## Implemented R1 foundation

- `SasdSectionPanel` for consistent form sections and explicit accessible grouping semantics;
- `SasdFieldLayout` for simple two-column label/editor forms with required markers;
- field-layout accessibility defaults derive missing editor names from field labels while preserving application-supplied accessible text;
- required fields expose a textual accessible indication in addition to the visible `*` marker;
- `FieldGap` is non-negative and updates rows created earlier by `AddField` without rewriting application-inserted exceptional rows;
- editors passed to `SasdFieldLayout.AddField` become normal WinForms child controls and follow parent-control disposal ownership;
- `SasdValidationCoordinator` for reusable synchronous/asynchronous validation rules and stable field-key focus lookup;
- `SasdValidationSummary` for persistent validation feedback, severity-aware accessible text and Enter/double-click navigation back to the affected field;
- required-field helpers and standard validation messages.

## Validation-summary binding and ownership

`SasdValidationSummary.Bind(...)` is an optional convenience for normal forms. A bound summary subscribes to `SasdValidationCoordinator.ValidationCompleted`, displays each completed result and uses the coordinator's existing `FieldKey -> Control` registrations when a user activates a message. The summary retains the complete `SasdValidationMessage` rather than maintaining a second lookup table.

The coordinator remains application-owned. Binding transfers no disposal ownership: the summary owns only its event subscription and detaches it through `Unbind()` or `Dispose()`. `TryFocusField(...)` never enables, reveals or otherwise mutates an unavailable editor merely to make navigation succeed; application workflow policy remains application-owned.

Applications that need custom navigation can leave the summary unbound and handle `MessageInvoked`. The event exposes the original validation message, including its stable field key and severity. When a coordinator is bound, `FocusMoved` reports whether the native WinForms focus request actually succeeded.

## Native R2 foundation

- `SasdPropertyEditor` wraps the native `PropertyGrid` with a searchable descriptor projection;
- filtering covers property/display name, category and description;
- `ForceReadOnly` can project all visible properties as read-only without modifying the application type;
- selected application objects remain caller-owned.

## Boundaries

The Forms module does not introduce a binding framework or domain model. Applications retain ownership of their view models/entities and decide when validation or persistence occurs. The property editor deliberately uses native WinForms descriptors and adds no third-party dependency.

`SasdFieldLayout` owns the visual controls parented into it in the normal WinForms sense. This is intentionally different from `SasdPropertyEditor`, which displays but does not own the selected application object. The distinction is documented because disposal ownership is part of the public lifecycle contract, not an implementation detail.
