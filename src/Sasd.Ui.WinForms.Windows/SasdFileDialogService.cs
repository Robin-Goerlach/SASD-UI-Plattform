namespace Sasd.Ui.WinForms.Windows;

/// <summary>Options shared by open and save file dialogs.</summary>
public sealed record SasdFileDialogOptions(
    string? Title = null,
    string? Filter = null,
    string? InitialDirectory = null,
    string? DefaultExtension = null);

/// <summary>Provides application-neutral access to native Windows file and folder dialogs.</summary>
public sealed class SasdFileDialogService
{
    /// <summary>Prompts for one existing file.</summary>
    public string? OpenFile(IWin32Window? owner, SasdFileDialogOptions? options = null)
    {
        options ??= new SasdFileDialogOptions();
        using var dialog = CreateOpenDialog(options);
        dialog.Multiselect = false;
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.FileName : null;
    }

    /// <summary>Prompts for zero or more existing files.</summary>
    public IReadOnlyList<string> OpenFiles(IWin32Window? owner, SasdFileDialogOptions? options = null)
    {
        options ??= new SasdFileDialogOptions();
        using var dialog = CreateOpenDialog(options);
        dialog.Multiselect = true;
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.FileNames : Array.Empty<string>();
    }

    /// <summary>Prompts for a destination file path.</summary>
    public string? SaveFile(IWin32Window? owner, SasdFileDialogOptions? options = null, string? suggestedFileName = null)
    {
        options ??= new SasdFileDialogOptions();
        using var dialog = new SaveFileDialog
        {
            AddExtension = true,
            CheckPathExists = true,
            DefaultExt = options.DefaultExtension ?? string.Empty,
            FileName = suggestedFileName ?? string.Empty,
            Filter = string.IsNullOrWhiteSpace(options.Filter) ? "All files (*.*)|*.*" : options.Filter,
            InitialDirectory = options.InitialDirectory ?? string.Empty,
            OverwritePrompt = true,
            RestoreDirectory = true,
            Title = options.Title ?? string.Empty,
        };

        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.FileName : null;
    }

    /// <summary>Prompts for an existing folder.</summary>
    public string? PickFolder(IWin32Window? owner, string? description = null, string? initialDirectory = null)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = description ?? string.Empty,
            InitialDirectory = initialDirectory ?? string.Empty,
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true,
        };

        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.SelectedPath : null;
    }

    private static OpenFileDialog CreateOpenDialog(SasdFileDialogOptions options) => new()
    {
        CheckFileExists = true,
        CheckPathExists = true,
        DefaultExt = options.DefaultExtension ?? string.Empty,
        Filter = string.IsNullOrWhiteSpace(options.Filter) ? "All files (*.*)|*.*" : options.Filter,
        InitialDirectory = options.InitialDirectory ?? string.Empty,
        RestoreDirectory = true,
        Title = options.Title ?? string.Empty,
    };
}
