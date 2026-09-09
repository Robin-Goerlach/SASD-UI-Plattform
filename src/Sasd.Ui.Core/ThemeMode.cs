namespace Sasd.Ui.Core;

/// <summary>Defines the user-interface colour mode requested by an application.</summary>
public enum ThemeMode
{
    /// <summary>Follow the current Windows preference where supported.</summary>
    System,

    /// <summary>Use the light theme definition.</summary>
    Light,

    /// <summary>Use the dark theme definition.</summary>
    Dark,

    /// <summary>Respect Windows High Contrast semantics and system colours.</summary>
    HighContrast
}
