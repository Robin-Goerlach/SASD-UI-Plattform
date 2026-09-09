# Source Projects

The `src` directory mirrors the approved product-module architecture. Projects are separated early to make ownership and dependency direction visible. Public NuGet packaging may initially combine several internal projects until the R1 API is stable.

## Initial executable scaffold

- `Sasd.Ui.Core` — platform-neutral contracts and design-token records.
- `Sasd.Ui.WinForms` — WinForms base types and UI-thread helper.
- `Sasd.Ui.WinForms.Krypton` — isolated R0 adapter placeholder; no Krypton dependency is committed before the pilot and licence/API review.

## Reserved R1 modules

- Theming
- Commands
- Shell
- Forms
- Data
- Dialogs
- Windows integration
- UI state

## Reserved adapters

Specialist R2/R3 integrations belong under `adapters`. Each adapter needs an ADR, separate package, security/lifecycle review and exit strategy.

The scaffold deliberately does not claim implementation of the complete component catalogue.
