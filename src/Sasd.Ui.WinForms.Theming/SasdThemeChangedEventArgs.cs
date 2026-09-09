using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Theming;

/// <summary>Describes a change of the active SASD theme.</summary>
public sealed class SasdThemeChangedEventArgs : EventArgs
{
    /// <summary>Initialises a new instance.</summary>
    public SasdThemeChangedEventArgs(SasdThemeDefinition previous, SasdThemeDefinition current)
    {
        Previous = previous;
        Current = current;
    }

    /// <summary>Gets the previously active theme.</summary>
    public SasdThemeDefinition Previous { get; }

    /// <summary>Gets the newly active theme.</summary>
    public SasdThemeDefinition Current { get; }
}
