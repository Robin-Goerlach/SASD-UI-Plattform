# Sasd.Ui.WinForms.App

`Sasd.Ui.WinForms.App` is the curated R1 convenience package for typical SASD WinForms applications.

It is a **metapackage**: it contains no runtime assembly of its own. Instead, one package reference brings in the lightweight R1 application modules for commands, forms/validation, dialogs/feedback, shell/navigation, UI state, theming and constrained Windows integration.

## Included R1 application modules

- `Sasd.Ui.WinForms.Commands`
- `Sasd.Ui.WinForms.Dialogs`
- `Sasd.Ui.WinForms.Forms`
- `Sasd.Ui.WinForms.Shell`
- `Sasd.Ui.WinForms.State`
- `Sasd.Ui.WinForms.Theming`
- `Sasd.Ui.WinForms.Windows`

Their normal dependencies also provide `Sasd.Ui.Core` and `Sasd.Ui.WinForms`.

## Deliberately not included

- `Sasd.Ui.WinForms.Data` — opt in when the application needs grids, search, filtering, paging, lists or trees.
- `Sasd.Ui.WinForms.Krypton` — optional visual implementation; the application chooses its visual system explicitly.
- `Sasd.Ui.WinForms.Media` and later R2 adapters — specialist/R2 capabilities must not enter every application transitively.

This boundary keeps a normal application's project file small without collapsing the repository's internal module boundaries or forcing optional dependencies into unrelated applications.

## Typical use

```xml
<ItemGroup>
  <PackageReference Include="Sasd.Ui.WinForms.App" Version="<approved-version>" />
  <PackageReference Include="Sasd.Ui.WinForms.Data" Version="<approved-version>" />
</ItemGroup>
```

Add the Data package only when required. Add Krypton only after the application has deliberately selected it as its visual implementation.
