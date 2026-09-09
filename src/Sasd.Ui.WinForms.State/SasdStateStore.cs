using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sasd.Ui.WinForms.State;

/// <summary>
/// Stores application-owned UI state in a versioned JSON document with backup recovery.
/// </summary>
public sealed class SasdStateStore : IAsyncDisposable
{
    private readonly SasdStateStoreOptions options;
    private readonly SemaphoreSlim gate = new(1, 1);
    private readonly JsonSerializerOptions serializerOptions;
    private readonly string rootPath;
    private readonly string statePath;
    private readonly string backupPath;

    /// <summary>Initialises a state store.</summary>
    public SasdStateStore(SasdStateStoreOptions options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        rootPath = options.ResolveRootPath();
        statePath = Path.Combine(rootPath, "ui-state.json");
        backupPath = Path.Combine(rootPath, "ui-state.backup.json");
        serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };
        serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    /// <summary>Gets the path of the primary UI-state document.</summary>
    public string StatePath => statePath;

    /// <summary>Loads one named state section or returns <see langword="default"/> when it is absent.</summary>
    public async Task<T?> LoadAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        ValidateKey(key);
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var document = await LoadDocumentAsync(cancellationToken).ConfigureAwait(false);
            if (!document.Sections.TryGetValue(key, out var value))
            {
                return default;
            }

            return value.Deserialize<T>(serializerOptions);
        }
        finally
        {
            gate.Release();
        }
    }

    /// <summary>Stores or replaces one named state section.</summary>
    public async Task SaveAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        ValidateKey(key);
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var document = await LoadDocumentAsync(cancellationToken).ConfigureAwait(false);
            document.SchemaVersion = options.SchemaVersion;
            document.UpdatedAtUtc = DateTimeOffset.UtcNow;
            document.Sections[key] = JsonSerializer.SerializeToElement(value, serializerOptions);
            await WriteDocumentAsync(document, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    /// <summary>Removes one state section.</summary>
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        ValidateKey(key);
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var document = await LoadDocumentAsync(cancellationToken).ConfigureAwait(false);
            if (!document.Sections.Remove(key))
            {
                return false;
            }

            document.UpdatedAtUtc = DateTimeOffset.UtcNow;
            await WriteDocumentAsync(document, cancellationToken).ConfigureAwait(false);
            return true;
        }
        finally
        {
            gate.Release();
        }
    }

    /// <summary>Deletes persisted UI state. Business data is never touched.</summary>
    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            DeleteIfExists(statePath);
            DeleteIfExists(backupPath);
        }
        finally
        {
            gate.Release();
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        gate.Dispose();
        return ValueTask.CompletedTask;
    }

    private async Task<StateDocument> LoadDocumentAsync(CancellationToken cancellationToken)
    {
        var primary = await TryLoadAsync(statePath, cancellationToken).ConfigureAwait(false);
        if (primary is not null)
        {
            return primary;
        }

        var backup = await TryLoadAsync(backupPath, cancellationToken).ConfigureAwait(false);
        return backup ?? StateDocument.Create(options.SchemaVersion);
    }

    private async Task<StateDocument?> TryLoadAsync(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            var document = await JsonSerializer.DeserializeAsync<StateDocument>(stream, serializerOptions, cancellationToken)
                .ConfigureAwait(false);

            if (document is null || document.SchemaVersion <= 0 || document.SchemaVersion > options.SchemaVersion)
            {
                return null;
            }

            document.Sections ??= new Dictionary<string, JsonElement>(StringComparer.Ordinal);
            return document;
        }
        catch (JsonException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }

    private async Task WriteDocumentAsync(StateDocument document, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(rootPath);
        var temporaryPath = Path.Combine(rootPath, $"ui-state.{Guid.NewGuid():N}.tmp");

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                4096,
                FileOptions.Asynchronous | FileOptions.WriteThrough))
            {
                await JsonSerializer.SerializeAsync(stream, document, serializerOptions, cancellationToken)
                    .ConfigureAwait(false);
                await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
            }

            if (File.Exists(statePath))
            {
                File.Replace(temporaryPath, statePath, backupPath, true);
            }
            else
            {
                File.Move(temporaryPath, statePath);
            }
        }
        finally
        {
            DeleteIfExists(temporaryPath);
        }
    }

    private static void ValidateKey(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (key.Length > 200)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "State keys may contain at most 200 characters.");
        }
    }

    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private sealed class StateDocument
    {
        public int SchemaVersion { get; set; }

        public DateTimeOffset UpdatedAtUtc { get; set; }

        public Dictionary<string, JsonElement> Sections { get; set; } = new(StringComparer.Ordinal);

        public static StateDocument Create(int schemaVersion) => new()
        {
            SchemaVersion = schemaVersion,
            UpdatedAtUtc = DateTimeOffset.UtcNow,
        };
    }
}
