namespace Sasd.Ui.WinForms.Windows;

/// <summary>
/// Configures the safety boundary applied before dropped file-system paths are
/// handed to application code.
/// </summary>
public sealed record SasdFileDropOptions(
    bool AllowFiles = true,
    bool AllowDirectories = false,
    IReadOnlyCollection<string>? AllowedExtensions = null,
    int MaxItems = 20,
    long? MaxFileBytes = 50L * 1024L * 1024L,
    bool RequireAllItemsAccepted = true)
{
    /// <summary>Validates configuration values before a drop target is attached.</summary>
    internal void Validate()
    {
        if (!AllowFiles && !AllowDirectories)
        {
            throw new ArgumentException("At least files or directories must be allowed.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxItems);
        if (MaxFileBytes is <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxFileBytes), "Maximum file size must be positive when configured.");
        }
    }
}

/// <summary>Describes why a dropped path was rejected by the platform boundary.</summary>
public sealed record SasdRejectedDropItem(string Path, string ErrorCode, string Message);

/// <summary>Contains the result of validating one drag-and-drop payload.</summary>
public sealed record SasdFileDropEvaluation(
    IReadOnlyList<string> AcceptedPaths,
    IReadOnlyList<SasdRejectedDropItem> RejectedItems,
    bool IsAccepted);

/// <summary>Event data delivered after a validated drop is accepted.</summary>
public sealed class SasdFilesDroppedEventArgs : EventArgs
{
    /// <summary>Initialises drop event data.</summary>
    public SasdFilesDroppedEventArgs(IReadOnlyList<string> paths) => Paths = paths;

    /// <summary>Gets absolute paths that passed the configured boundary checks.</summary>
    public IReadOnlyList<string> Paths { get; }
}

/// <summary>Abstraction for attaching constrained file-system drag-and-drop behavior.</summary>
public interface ISasdDragDropService
{
    /// <summary>Attaches a validated file-system drop target to a WinForms control.</summary>
    SasdFileDropBinding Attach(Control control, SasdFileDropOptions? options = null);
}

/// <summary>
/// Adds file-system drag-and-drop to application controls without making raw
/// <see cref="DragEventArgs"/> the normal application integration surface.
/// </summary>
public sealed class SasdDragDropService : ISasdDragDropService
{
    /// <inheritdoc />
    public SasdFileDropBinding Attach(Control control, SasdFileDropOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(control);
        return new SasdFileDropBinding(control, options ?? new SasdFileDropOptions());
    }

    /// <summary>
    /// Validates paths independently from a UI event. This is public so consuming
    /// applications can apply the same boundary to paste/import scenarios and tests.
    /// </summary>
    public static SasdFileDropEvaluation Evaluate(IEnumerable<string> paths, SasdFileDropOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(paths);
        SasdFileDropOptions effectiveOptions = options ?? new SasdFileDropOptions();
        effectiveOptions.Validate();

        string[] supplied = paths.ToArray();
        var accepted = new List<string>();
        var rejected = new List<SasdRejectedDropItem>();
        HashSet<string>? extensions = BuildExtensionSet(effectiveOptions.AllowedExtensions);

        for (int index = 0; index < supplied.Length; index++)
        {
            string suppliedPath = supplied[index] ?? string.Empty;
            if (index >= effectiveOptions.MaxItems)
            {
                rejected.Add(new SasdRejectedDropItem(
                    suppliedPath,
                    "DROP_ITEM_LIMIT",
                    $"At most {effectiveOptions.MaxItems} items may be dropped at once."));
                continue;
            }

            if (string.IsNullOrWhiteSpace(suppliedPath))
            {
                rejected.Add(new SasdRejectedDropItem(suppliedPath, "DROP_PATH_EMPTY", "A dropped path is empty."));
                continue;
            }

            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(suppliedPath);
            }
            catch (Exception exception) when (
                exception is ArgumentException or IOException or NotSupportedException or UnauthorizedAccessException)
            {
                rejected.Add(new SasdRejectedDropItem(suppliedPath, "DROP_PATH_INVALID", "The dropped path is invalid."));
                continue;
            }

            if (File.Exists(fullPath))
            {
                if (!effectiveOptions.AllowFiles)
                {
                    rejected.Add(new SasdRejectedDropItem(fullPath, "DROP_FILE_NOT_ALLOWED", "Files are not allowed here."));
                    continue;
                }

                if (extensions is not null && !extensions.Contains(Path.GetExtension(fullPath)))
                {
                    rejected.Add(new SasdRejectedDropItem(
                        fullPath,
                        "DROP_EXTENSION_NOT_ALLOWED",
                        "The file extension is not allowed for this drop target."));
                    continue;
                }

                if (effectiveOptions.MaxFileBytes is long maxBytes)
                {
                    try
                    {
                        var fileInfo = new FileInfo(fullPath);
                        if (fileInfo.Length > maxBytes)
                        {
                            rejected.Add(new SasdRejectedDropItem(
                                fullPath,
                                "DROP_FILE_TOO_LARGE",
                                $"The file exceeds the configured {maxBytes} byte limit."));
                            continue;
                        }
                    }
                    catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
                    {
                        rejected.Add(new SasdRejectedDropItem(
                            fullPath,
                            "DROP_FILE_METADATA_FAILED",
                            "The file could not be inspected safely."));
                        continue;
                    }
                }

                // Extension and size checks are only admission filters. The application
                // must still validate the actual file format/content before parsing it.
                accepted.Add(fullPath);
                continue;
            }

            if (Directory.Exists(fullPath))
            {
                if (!effectiveOptions.AllowDirectories)
                {
                    rejected.Add(new SasdRejectedDropItem(
                        fullPath,
                        "DROP_DIRECTORY_NOT_ALLOWED",
                        "Directories are not allowed here."));
                    continue;
                }

                accepted.Add(fullPath);
                continue;
            }

            rejected.Add(new SasdRejectedDropItem(fullPath, "DROP_PATH_NOT_FOUND", "The dropped path does not exist."));
        }

        bool isAccepted = accepted.Count > 0 &&
            (!effectiveOptions.RequireAllItemsAccepted || rejected.Count == 0);
        return new SasdFileDropEvaluation(accepted, rejected, isAccepted);
    }

    private static HashSet<string>? BuildExtensionSet(IReadOnlyCollection<string>? extensions)
    {
        if (extensions is null || extensions.Count == 0)
        {
            return null;
        }

        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string extension in extensions)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(extension);
            result.Add(extension.StartsWith(".", StringComparison.Ordinal) ? extension : $".{extension}");
        }

        return result;
    }
}

/// <summary>
/// Represents the lifetime of one attached drop target. Disposing the binding
/// removes event handlers and restores the control's previous AllowDrop value.
/// </summary>
public sealed class SasdFileDropBinding : IDisposable
{
    private readonly Control control;
    private readonly SasdFileDropOptions options;
    private readonly bool previousAllowDrop;
    private bool disposed;

    internal SasdFileDropBinding(Control control, SasdFileDropOptions options)
    {
        this.control = control;
        this.options = options;
        options.Validate();

        previousAllowDrop = control.AllowDrop;
        control.AllowDrop = true;
        control.DragEnter += OnDragEnter;
        control.DragDrop += OnDragDrop;
    }

    /// <summary>Raised only after the payload satisfies the configured boundary policy.</summary>
    public event EventHandler<SasdFilesDroppedEventArgs>? FilesDropped;

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        control.DragEnter -= OnDragEnter;
        control.DragDrop -= OnDragDrop;
        if (!control.IsDisposed)
        {
            control.AllowDrop = previousAllowDrop;
        }
    }

    private void OnDragEnter(object? sender, DragEventArgs e)
    {
        SasdFileDropEvaluation evaluation = EvaluateEvent(e);
        e.Effect = evaluation.IsAccepted ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void OnDragDrop(object? sender, DragEventArgs e)
    {
        SasdFileDropEvaluation evaluation = EvaluateEvent(e);
        if (!evaluation.IsAccepted)
        {
            return;
        }

        FilesDropped?.Invoke(this, new SasdFilesDroppedEventArgs(evaluation.AcceptedPaths));
    }

    private SasdFileDropEvaluation EvaluateEvent(DragEventArgs e)
    {
        IDataObject? data = e.Data;
        if (data is null ||
            !data.GetDataPresent(DataFormats.FileDrop) ||
            data.GetData(DataFormats.FileDrop) is not string[] paths)
        {
            return new SasdFileDropEvaluation(
                Array.Empty<string>(),
                [new SasdRejectedDropItem(string.Empty, "DROP_FORMAT_NOT_SUPPORTED", "Only file-system drops are supported.")],
                false);
        }

        return SasdDragDropService.Evaluate(paths, options);
    }
}
