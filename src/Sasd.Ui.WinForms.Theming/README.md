# Sasd.Ui.WinForms.Theming

R0 implementation of the native WinForms theme layer.

Currently included:

- mapping from vendor-neutral `SasdThemeDefinition` tokens to native WinForms controls;
- built-in Light, Dark and High Contrast handling;
- explicit, application-owned theme application with no global service locator;
- safe recursive theming for common controls and `DataGridView`.

Still pending for R1: icon services, richer Windows system-theme detection, visual regression evidence and the Krypton comparison adapter.
