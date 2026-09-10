# Examples

This directory contains integrated, application-like examples of the SASD UI Platform.

Unlike `samples/`, which contains focused reference consumers and the Component Gallery, `examples/` is intended for larger demonstrations that combine several platform modules in one executable application.

## Integrated platform showcase

`Sasd.Ui.PlatformShowcase` demonstrates the current WinForms foundation as one small desktop application. It is designed for two purposes:

1. show how a consuming SASD application can compose the platform without hidden framework magic;
2. provide a manual test surface for important controls, services and interaction patterns.

The showcase intentionally keeps its own business model small and in-memory. It is not an architecture template for domain logic or persistence.