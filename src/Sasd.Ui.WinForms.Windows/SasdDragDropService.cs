namespace Sasd.Ui.WinForms.Windows;

/// <summary>Policy used when validating files dropped onto an application surface.</summary>
public sealed record SasdFileDropPolicy(
    IReadOnlyCollection<string>? AllowedExtensions = null,
    bool AllowDirectories = false,
    int MaxItems = 20,
    long MaxFileBytes = 100L * 1024L * 1024L)
{
    /// <summary>Validates policy bounds before a drop is inspected.</summary>
    public SasdFileDropPolicy Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxItems);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxFileBytes);

        if (MaxItems > 10_000)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxItems), "A file drop may contain at most 10000 items.");
        }

        if (AllowedExtensions is not null)
        {
            foreach (string extension in AllowedExtensions)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(extension);
                if (extension.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || extension.Contains(Path.DirectorySeparatorChar))
                {
                    throw new ArgumentException($"Invalid file extension '{extension}'.", nameof(AllowedExtensions));
                }
            }
        }

        return this;
    }
}

/// <summary>One file-system item accepted from a drag-and-drop operation.</summary>
public sealed record SasdFileDropItem(string Path, bool IsDirectory, long? FileSizeBytes);

/// <summary>One rejected file-system item and its stable reason code.</summary>
public sealed record SasdFileDropRejection(string? Path, string ReasonCode, string Message);

/// <summary>Result returned after validating a drag-and-drop payload.</summary>
public sealed record SasdFileDropResult(
    IReadOnlyList<SasdFileDropItem> Accepted,
    IReadOnlyList<SasdFileDropRejection> Rejected)
{
    /// <summary>Gets whether at least one item is safe enough to hand to application code.</summary>
    public bool HasAcceptedItems => Accepted.Count > 0;
}

/// <summary>Extracts and validates file-drop payloads without opening their contents.</summary>
public interface ISasdDragDropService
{
    /// <summary>Returns whether the payload contains at least one item accepted by the supplied policy.</summary>
    bool CanAccept(IDataObject? dataObject, SasdFileDropPolicy? policy = null);

    /// <summary>Validates file-system paths contained in a standard Windows file-drop payload.</summary>
    SasdFileDropResult ReadFiles(IDataObject? dataObject, SasdFileDropPolicy? policy = null);
}

/// <summary>
/// Validates Windows <see cref="DataFormats.FileDrop"/> payloads before application code sees them.
/// </summary>
/// <remarks>
/// <para>
/// This service validates metadata only. A permitted extension is not proof of file content,
/// safety or trustworthiness. Consuming applications must still validate/parses file content
/// appropriate to their domain before importing it.
/// </para>
/// <para>
/// The service never opens or executes a dropped file. It normalises paths, verifies existence,
/// applies count/extension/size rules and reports rejected entries with stable reason codes.
/// </para>
/// </remarks>
public sealed class SasdDragDropService : ISasdDragDropService
{
    /// <inheritdoc />
    public bool CanAccept(IDataObject? dataObject, SasdFileDropPolicy? policy = null) =>
        ReadFiles(dataObject, policy).HasAcceptedItems;

    /// <inheritdoc />
    public SasdFileDropResult ReadFiles(IDataObject? dataObject, SasdFileDropPolicy? policy = null)
    {
        policy = (policy ?? new SasdFileDropPolicy()).Validate();

        if (dataObject is null || !dataObject.GetDataPresent(DataFormats.FileDrop, autoConvert: false))
        {
            return new SasdFileDropResult(
                Array.Empty<SasdFileDropItem>(),
                [new SasdFileDropRejection(null, "DROP_FORMAT_UNSUPPORTED", "The drag payload does not contain Windows file-drop data.")]);
        }

        if (dataObject.GetData(DataFormats.FileDrop, autoConvert: false) is not string[] paths)
        {
            return new SasdFileDropResult(
                Array.Empty<SasdFileDropItem>(),
                [new SasdFileDropRejection(null, "DROP_FORMAT_INVALID", "The file-drop payload could not be read as file-system paths.")]);
        }

        if (paths.Length > policy.MaxItems)
        {
            return new SasdFileDropResult(
                Array.Empty<SasdFileDropItem>(),
                [new SasdFileDropRejection(null, "DROP_TOO_MANY_ITEMS", $"The drop contains {paths.Length} items; at most {policy.MaxItems} are allowed.")]);
        }

        HashSet<string>? allowedExtensions = CreateExtensionSet(policy.AllowedExtensions);
        var accepted = new List<SasdFileDropItem>(paths.Length);
        var rejected = new List<SasdFileDropRejection>();
        var seenPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string rawPath in paths)
        {
            ValidatePath(rawPath, policy, allowedExtensions, seenPaths, accepted, rejected);
        }

        return new SasdFileDropResult(accepted.ToArray(), rejected.ToArray());
    }

    private static void ValidatePath(
        string rawPath,
        SasdFileDropPolicy policy,
        HashSet<string>? allowedExtensions,
        HashSet<string> seenPaths,
        List<SasdFileDropItem> accepted,
        List<SasdFileDropRejection> rejected)
    {
        if (string.IsNullOrWhiteSpace(rawPath))
        {
            rejected.Add(new SasdFileDropRejection(rawPath, "DROP_PATH_INVALID", "The drop contains an empty file-system path."));
            return;
        }

        string fullPath;
        try
        {
            fullPath = Path.GetFullPath(rawPath);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            rejected.Add(new SasdFileDropRejection(rawPath, "DROP_PATH_INVALID", "The dropped path is not a valid local file-system path."));
            return;
        }

        if (!seenPaths.Add(fullPath))
        {
            rejected.Add(new SasdFileDropRejection(fullPath, "DROP_DUPLICATE", "The same file-system path was supplied more than once."));
            return;
        }

        if (Directory.Exists(fullPath))
        {
            if (!policy.AllowDirectories)
            {
                rejected.Add(new SasdFileDropRejection(fullPath, "DROP_DIRECTORY_NOT_ALLOWED", "Directories are not allowed by this drop policy."));
                return;
            }

            accepted.Add(new SasdFileDropItem(fullPath, IsDirectory: true, FileSizeBytes: null));
            return;
        }

        if (!File.Exists(fullPath))
        {
            rejected.Add(new SasdFileDropRejection(fullPath, "DROP_PATH_NOT_FOUND", "The dropped file-system item no longer exists."));
            return;
        }

        string extension = Path.GetExtension(fullPath);
        if (allowedExtensions is not null && !allowedExtensions.Contains(extension))
        {
            rejected.Add(new SasdFileDropRejection(fullPath, "DROP_EXTENSION_NOT_ALLOWED", $"Files with extension '{extension}' are not allowed by this drop policy."));
            return;
        }

        long fileSize;
        try
        {
            fileSize = new FileInfo(fullPath).Length;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            rejected.Add(new SasdFileDropRejection(fullPath, "DROP_METADATA_UNAVAILABLE", "File metadata could not be read safely."));
            return;
        }

        if (fileSize > policy.MaxFileBytes)
        {
            rejected.Add(new SasdFileDropRejection(fullPath, "DROP_FILE_TOO_LARGE", $"The file exceeds the allowed size of {policy.MaxFileBytes} bytes."));
            return;
        }

        accepted.Add(new SasdFileDropItem(fullPath, IsDirectory: false, FileSizeBytes: fileSize));
    }

    private static HashSet<string>? CreateExtensionSet(IReadOnlyCollection<string>? extensions)
    {
        if (extensions is null || extensions.Count == 0)
        {
            return null;
        }

        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string extension in extensions)
        {
            string normalized = extension.StartsWith('.', StringComparison.Ordinal) ? extension : $".{extension}";
            result.Add(normalized);
        }

        return result;
    }
}
