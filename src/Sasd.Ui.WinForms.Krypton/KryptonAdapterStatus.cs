using Krypton.Toolkit;

namespace Sasd.Ui.WinForms.Krypton;

/// <summary>Reports the reviewed dependency and implementation state of the Krypton R0.2 pilot.</summary>
public static class KryptonAdapterStatus
{
    /// <summary>Gets the architecture decision record governing the pilot.</summary>
    public const string DecisionRecord = "ADR-0004";

    /// <summary>Gets the centrally pinned stable Krypton.Toolkit version.</summary>
    public const string ReviewedPackageVersion = "105.26.7.201";

    /// <summary>Gets a value indicating that the first real adapter types are present.</summary>
    public static bool IsImplemented => true;

    /// <summary>
    /// Gets the runtime assembly version. This is diagnostic information only;
    /// package selection remains controlled by Directory.Packages.props.
    /// </summary>
    public static string RuntimeAssemblyVersion =>
        typeof(KryptonForm).Assembly.GetName().Version?.ToString() ?? "unknown";
}
