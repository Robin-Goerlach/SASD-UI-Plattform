namespace Sasd.Ui.WinForms.Krypton;

/// <summary>
/// Identifies the current state of the Krypton R0 evaluation. The project does
/// not reference Krypton until the licence, designer, DPI, accessibility and
/// API-boundary checks have been approved.
/// </summary>
public static class KryptonAdapterStatus
{
    /// <summary>Gets the architecture decision record governing the pilot.</summary>
    public const string DecisionRecord = "ADR-0004";

    /// <summary>Gets a value indicating whether the real adapter is implemented.</summary>
    public static bool IsImplemented => false;
}
