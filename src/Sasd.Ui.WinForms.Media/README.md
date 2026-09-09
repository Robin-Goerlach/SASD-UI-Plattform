# Sasd.Ui.WinForms.Media

Native media-presentation components for SASD WinForms applications.

## Implemented R2 foundation

- `SasdImageViewer` for application-supplied images;
- fit, actual-size and bounded custom zoom modes;
- scrolling for images larger than the viewport;
- explicit image ownership: the viewer clones input images and disposes only its own copy.

## Boundaries

The module deliberately does not open files, decode untrusted formats on behalf of applications, edit images or provide an annotation framework. Applications remain responsible for validating and loading source content before passing an `Image` to the viewer.

Advanced image processing, metadata editing and specialist viewers should be added only when a concrete SASD application demonstrates the need.
