# Sasd.Ui.WinForms.Forms

Reusable form-layout, validation and property-editing components for SASD WinForms applications.

## Implemented R1 foundation

- `SasdSectionPanel` for consistent form sections;
- `SasdFieldLayout` for simple two-column label/editor forms with required markers;
- `SasdValidationCoordinator` for reusable synchronous/asynchronous validation rules;
- `SasdValidationSummary` for persistent validation feedback;
- required-field helpers and standard validation messages.

## Native R2 foundation

- `SasdPropertyEditor` wraps the native `PropertyGrid` with a searchable descriptor projection;
- filtering covers property/display name, category and description;
- `ForceReadOnly` can project all visible properties as read-only without modifying the application type;
- selected application objects remain caller-owned.

## Boundaries

The Forms module does not introduce a binding framework or domain model. Applications retain ownership of their view models/entities and decide when validation or persistence occurs. The property editor deliberately uses native WinForms descriptors and adds no third-party dependency.
