using System.Text.Json;

namespace Sasd.Ui.WinForms.State;

/// <summary>
/// Describes one incremental migration of the persisted UI-state document.
/// Migrations operate only on serialized UI sections; they never access or modify
/// business data owned by the consuming application.
/// </summary>
public interface ISasdStateMigration
{
    /// <summary>Gets the schema version accepted by this migration.</summary>
    int FromVersion { get; }

    /// <summary>Gets the schema version produced by this migration.</summary>
    int ToVersion { get; }

    /// <summary>
    /// Applies the migration to the mutable section dictionary. Implementations
    /// should be deterministic and idempotent with respect to their source schema.
    /// </summary>
    void Apply(IDictionary<string, JsonElement> sections);
}

/// <summary>
/// Convenience implementation for small application-owned state migrations.
/// </summary>
public sealed class SasdStateMigration : ISasdStateMigration
{
    private readonly Action<IDictionary<string, JsonElement>> migration;

    /// <summary>Initialises one incremental migration.</summary>
    public SasdStateMigration(
        int fromVersion,
        int toVersion,
        Action<IDictionary<string, JsonElement>> migration)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fromVersion);
        if (toVersion != fromVersion + 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(toVersion),
                "UI-state migrations must advance exactly one schema version.");
        }

        this.migration = migration ?? throw new ArgumentNullException(nameof(migration));
        FromVersion = fromVersion;
        ToVersion = toVersion;
    }

    /// <inheritdoc />
    public int FromVersion { get; }

    /// <inheritdoc />
    public int ToVersion { get; }

    /// <inheritdoc />
    public void Apply(IDictionary<string, JsonElement> sections)
    {
        ArgumentNullException.ThrowIfNull(sections);
        migration(sections);
    }
}
