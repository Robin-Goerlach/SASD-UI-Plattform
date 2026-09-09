namespace Sasd.Ui.WinForms.Commands;

/// <summary>
/// Describes one application or infrastructure command independently of a concrete button or menu item.
/// </summary>
public sealed class SasdCommand
{
    private readonly Func<CancellationToken, Task> executeAsync;
    private bool enabled = true;
    private bool visible = true;
    private bool isChecked;

    /// <summary>Initialises a command.</summary>
    public SasdCommand(
        string id,
        string text,
        Func<CancellationToken, Task> executeAsync,
        string? description = null,
        Keys shortcutKeys = Keys.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        Id = id;
        Text = text;
        Description = description;
        ShortcutKeys = shortcutKeys;
        this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
    }

    /// <summary>Raised whenever enabled, visible or checked state changes.</summary>
    public event EventHandler? StateChanged;

    /// <summary>Gets the stable command identifier.</summary>
    public string Id { get; }

    /// <summary>Gets the user-facing command text.</summary>
    public string Text { get; }

    /// <summary>Gets the optional user-facing description.</summary>
    public string? Description { get; }

    /// <summary>Gets the optional keyboard shortcut.</summary>
    public Keys ShortcutKeys { get; }

    /// <summary>Gets whether the command can currently execute.</summary>
    public bool Enabled
    {
        get => enabled;
        set => SetField(ref enabled, value);
    }

    /// <summary>Gets whether command surfaces should be visible.</summary>
    public bool Visible
    {
        get => visible;
        set => SetField(ref visible, value);
    }

    /// <summary>Gets whether checkable command surfaces should be checked.</summary>
    public bool IsChecked
    {
        get => isChecked;
        set => SetField(ref isChecked, value);
    }

    internal Task ExecuteCoreAsync(CancellationToken cancellationToken) => executeAsync(cancellationToken);

    private void SetField(ref bool field, bool value)
    {
        if (field == value)
        {
            return;
        }

        field = value;
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}
