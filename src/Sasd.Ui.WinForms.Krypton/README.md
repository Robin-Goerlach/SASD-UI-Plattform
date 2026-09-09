# Sasd.Ui.WinForms.Krypton

Isolated R0.2 adapter and evaluation project for **Krypton.Toolkit**.

## Current scope

- stable `Krypton.Toolkit` dependency pinned centrally in `Directory.Packages.props`;
- `SasdKryptonForm` as a designer-friendly Krypton-backed SASD window;
- diagnostic adapter status used by smoke checks and the evaluation process.

## Boundary rule

Krypton types may appear inside this project and in Krypton-specific samples. They must not leak into `Sasd.Ui.Core`, the native `Sasd.Ui.WinForms.*` contracts, or application-neutral service interfaces.

This is still a **pilot**, not a decision to make Krypton mandatory. ADR-0004 remains subject to designer, DPI, accessibility, maintenance, and real-application validation.
