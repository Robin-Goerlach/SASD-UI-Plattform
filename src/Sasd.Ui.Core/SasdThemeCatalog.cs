namespace Sasd.Ui.Core;

/// <summary>
/// Provides the built-in SASD theme definitions used as safe defaults by the
/// WinForms implementation and by applications that do not supply branding.
/// </summary>
public static class SasdThemeCatalog
{
    /// <summary>Gets the built-in light theme.</summary>
    public static SasdThemeDefinition Light { get; } = new(
        Id: "sasd.light",
        SchemaVersion: 1,
        Mode: ThemeMode.Light,
        Colors: new SasdColorTokens(
            Background: "#F5F7FA",
            Surface: "#FFFFFF",
            SurfaceVariant: "#EEF2F7",
            Primary: "#0B5CAD",
            Secondary: "#4B6478",
            Text: "#17212B",
            MutedText: "#5D6B78",
            Border: "#D5DCE5",
            Focus: "#0B5CAD",
            Success: "#16803C",
            Warning: "#A35A00",
            Error: "#B42318",
            Info: "#175CD3",
            Selection: "#D8E9FA"),
        Typography: new SasdTypographyTokens(
            DefaultFontFamily: "Segoe UI",
            MonospaceFontFamily: "Cascadia Mono",
            BodySize: 9f,
            CaptionSize: 8f,
            LabelSize: 9f,
            TitleSize: 18f,
            HeadingSize: 12f),
        Spacing: new SasdSpacingTokens(),
        Sizes: new SasdSizeTokens(
            InputMinimumHeight: 28,
            ButtonMinimumHeight: 30,
            IconSmall: 16,
            IconMedium: 20,
            IconLarge: 32,
            DialogMinimumWidth: 420),
        IconThemeId: "sasd.standard");

    /// <summary>Gets the built-in dark theme.</summary>
    public static SasdThemeDefinition Dark { get; } = new(
        Id: "sasd.dark",
        SchemaVersion: 1,
        Mode: ThemeMode.Dark,
        Colors: new SasdColorTokens(
            Background: "#111827",
            Surface: "#182230",
            SurfaceVariant: "#243244",
            Primary: "#6CB6FF",
            Secondary: "#9FB3C8",
            Text: "#F3F4F6",
            MutedText: "#B6C2CF",
            Border: "#36475A",
            Focus: "#8AC7FF",
            Success: "#62D58A",
            Warning: "#F7B955",
            Error: "#FF8A80",
            Info: "#8AB8FF",
            Selection: "#234C73"),
        Typography: Light.Typography,
        Spacing: Light.Spacing,
        Sizes: Light.Sizes,
        IconThemeId: "sasd.standard");

    /// <summary>
    /// Gets a high-contrast fallback definition. The WinForms implementation
    /// replaces these colours with Windows system colours whenever High Contrast
    /// is active.
    /// </summary>
    public static SasdThemeDefinition HighContrast { get; } = new(
        Id: "sasd.high-contrast",
        SchemaVersion: 1,
        Mode: ThemeMode.HighContrast,
        Colors: new SasdColorTokens(
            Background: "#000000",
            Surface: "#000000",
            SurfaceVariant: "#000000",
            Primary: "#FFFFFF",
            Secondary: "#FFFFFF",
            Text: "#FFFFFF",
            MutedText: "#FFFFFF",
            Border: "#FFFFFF",
            Focus: "#FFFF00",
            Success: "#FFFFFF",
            Warning: "#FFFFFF",
            Error: "#FFFFFF",
            Info: "#FFFFFF",
            Selection: "#000080"),
        Typography: Light.Typography,
        Spacing: Light.Spacing,
        Sizes: Light.Sizes,
        IconThemeId: "sasd.standard");

    /// <summary>Resolves a built-in definition for the requested mode.</summary>
    public static SasdThemeDefinition Get(ThemeMode mode) => mode switch
    {
        ThemeMode.Dark => Dark,
        ThemeMode.HighContrast => HighContrast,
        ThemeMode.Light => Light,
        ThemeMode.System => Light,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
    };
}
