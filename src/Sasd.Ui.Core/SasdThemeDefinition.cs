namespace Sasd.Ui.Core;

/// <summary>Semantic colours used by SASD components.</summary>
public sealed record SasdColorTokens(
    string Background,
    string Surface,
    string SurfaceVariant,
    string Primary,
    string Secondary,
    string Text,
    string MutedText,
    string Border,
    string Focus,
    string Success,
    string Warning,
    string Error,
    string Info,
    string Selection);

/// <summary>Semantic spacing values at 100 percent DPI.</summary>
public sealed record SasdSpacingTokens(
    int XSmall = 2,
    int Small = 4,
    int Medium = 8,
    int Large = 12,
    int XLarge = 16,
    int XXLarge = 24,
    int XXXLarge = 32);

/// <summary>Identifies the fonts and relative roles used by a theme.</summary>
public sealed record SasdTypographyTokens(
    string DefaultFontFamily,
    string MonospaceFontFamily,
    float BodySize,
    float CaptionSize,
    float LabelSize,
    float TitleSize,
    float HeadingSize);

/// <summary>Defines minimum component sizes at 100 percent DPI.</summary>
public sealed record SasdSizeTokens(
    int InputMinimumHeight,
    int ButtonMinimumHeight,
    int IconSmall,
    int IconMedium,
    int IconLarge,
    int DialogMinimumWidth);

/// <summary>
/// Immutable, vendor-neutral visual semantics for one SASD theme.
/// </summary>
public sealed record SasdThemeDefinition(
    string Id,
    int SchemaVersion,
    ThemeMode Mode,
    SasdColorTokens Colors,
    SasdTypographyTokens Typography,
    SasdSpacingTokens Spacing,
    SasdSizeTokens Sizes,
    string IconThemeId);
