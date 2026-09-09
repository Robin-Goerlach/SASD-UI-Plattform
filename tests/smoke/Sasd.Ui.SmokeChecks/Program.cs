using Sasd.Ui.Core;

namespace Sasd.Ui.SmokeChecks;

internal static class Program
{
    private static int Main()
    {
        try
        {
            ValidateTheme(SasdThemeCatalog.Light, ThemeMode.Light);
            ValidateTheme(SasdThemeCatalog.Dark, ThemeMode.Dark);
            ValidateTheme(SasdThemeCatalog.HighContrast, ThemeMode.HighContrast);

            Ensure(SasdThemeCatalog.Get(ThemeMode.Light) == SasdThemeCatalog.Light, "Light theme lookup failed.");
            Ensure(SasdThemeCatalog.Get(ThemeMode.Dark) == SasdThemeCatalog.Dark, "Dark theme lookup failed.");
            Ensure(SasdThemeCatalog.Get(ThemeMode.HighContrast) == SasdThemeCatalog.HighContrast,
                "High-contrast theme lookup failed.");

            Console.WriteLine("SASD UI smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateTheme(SasdThemeDefinition theme, ThemeMode expectedMode)
    {
        Ensure(!string.IsNullOrWhiteSpace(theme.Id), "Theme ID must not be empty.");
        Ensure(theme.SchemaVersion > 0, "Theme schema version must be positive.");
        Ensure(theme.Mode == expectedMode, $"Unexpected mode for {theme.Id}.");
        Ensure(theme.Spacing.Medium > 0, "Medium spacing must be positive.");
        Ensure(theme.Sizes.InputMinimumHeight >= 24, "Input minimum height is too small.");
        Ensure(theme.Sizes.ButtonMinimumHeight >= 24, "Button minimum height is too small.");
        Ensure(theme.Colors.Background.StartsWith('#'), "Background colour must use a stable HTML value.");
        Ensure(theme.Colors.Text.StartsWith('#'), "Text colour must use a stable HTML value.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
