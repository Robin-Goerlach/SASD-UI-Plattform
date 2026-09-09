namespace Sasd.Ui.WinForms.State;

/// <summary>Configures the file-backed SASD UI-state store.</summary>
public sealed record SasdStateStoreOptions(
    string CompanyName,
    string ProductName,
    string? RootPath = null,
    int SchemaVersion = 1,
    IReadOnlyList<ISasdStateMigration>? Migrations = null)
{
    /// <summary>Returns the directory used for UI-state files.</summary>
    public string ResolveRootPath()
    {
        ValidatePathSegment(CompanyName, nameof(CompanyName));
        ValidatePathSegment(ProductName, nameof(ProductName));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(SchemaVersion);

        if (!string.IsNullOrWhiteSpace(RootPath))
        {
            return Path.GetFullPath(RootPath);
        }

        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, CompanyName, ProductName);
    }

    private static void ValidatePathSegment(string value, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        if (value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Value contains characters that are not valid in a path segment.", parameterName);
        }
    }
}
