using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Commands;

/// <summary>
/// Registers application commands by stable identifier and provides one shared execution path.
/// </summary>
/// <remarks>
/// The manager does not own or dispose command instances; <see cref="SasdCommand"/> contains no
/// unmanaged resources. Non-empty keyboard shortcuts are required to be unique inside one manager
/// so global routing remains deterministic. Context-specific duplicate shortcuts can still be bound
/// directly to local controls instead of being registered globally.
/// </remarks>
public sealed class SasdCommandManager
{
    private readonly Dictionary<string, SasdCommand> commands = new(StringComparer.OrdinalIgnoreCase);
    private readonly SasdCommandRunner runner;

    /// <summary>Creates a manager with a new command runner.</summary>
    public SasdCommandManager()
        : this(new SasdCommandRunner())
    {
    }

    /// <summary>Creates a manager with a caller-supplied runner.</summary>
    public SasdCommandManager(SasdCommandRunner runner) =>
        this.runner = runner ?? throw new ArgumentNullException(nameof(runner));

    /// <summary>Gets a snapshot of all registered commands.</summary>
    public IReadOnlyList<SasdCommand> Commands => commands.Values.ToArray();

    /// <summary>Gets the number of registered commands.</summary>
    public int Count => commands.Count;

    /// <summary>Registers one command.</summary>
    public void Register(SasdCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (commands.ContainsKey(command.Id))
        {
            throw new ArgumentException($"Command id '{command.Id}' is already registered.", nameof(command));
        }

        if (command.ShortcutKeys != Keys.None)
        {
            SasdCommand? existingShortcut = commands.Values.FirstOrDefault(item => item.ShortcutKeys == command.ShortcutKeys);
            if (existingShortcut is not null)
            {
                throw new ArgumentException(
                    $"Shortcut '{command.ShortcutKeys}' is already assigned to command '{existingShortcut.Id}'.",
                    nameof(command));
            }
        }

        commands.Add(command.Id, command);
    }

    /// <summary>Removes a registered command by id.</summary>
    public bool Remove(string commandId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        return commands.Remove(commandId);
    }

    /// <summary>Removes all registered commands without disposing application-owned command instances.</summary>
    public void Clear() => commands.Clear();

    /// <summary>Returns a registered command or <see langword="null"/>.</summary>
    public SasdCommand? Find(string commandId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        return commands.GetValueOrDefault(commandId);
    }

    /// <summary>Returns the command assigned to an exact shortcut or <see langword="null"/>.</summary>
    public SasdCommand? FindByShortcut(Keys shortcutKeys)
    {
        if (shortcutKeys == Keys.None)
        {
            return null;
        }

        return commands.Values.FirstOrDefault(command => command.ShortcutKeys == shortcutKeys);
    }

    /// <summary>Executes a registered command and reports an unknown id as a recoverable result.</summary>
    public Task<UiOperationResult> ExecuteAsync(string commandId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        if (!commands.TryGetValue(commandId, out SasdCommand? command))
        {
            return Task.FromResult(UiOperationResult.Failure(
                "The requested command is not registered.",
                errorCode: "COMMAND_NOT_REGISTERED"));
        }

        return runner.ExecuteAsync(command, cancellationToken: cancellationToken);
    }
}

/// <summary>Event data raised after a command executed through a form shortcut.</summary>
public sealed class SasdShortcutCommandCompletedEventArgs : EventArgs
{
    /// <summary>Initialises shortcut completion data.</summary>
    public SasdShortcutCommandCompletedEventArgs(SasdCommand command, UiOperationResult result)
    {
        Command = command ?? throw new ArgumentNullException(nameof(command));
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>Gets the command selected by the shortcut.</summary>
    public SasdCommand Command { get; }

    /// <summary>Gets the execution result.</summary>
    public UiOperationResult Result { get; }
}

/// <summary>
/// Routes exact <see cref="SasdCommand.ShortcutKeys"/> combinations from one form to a command manager.
/// </summary>
public sealed class SasdCommandShortcutBinding : IDisposable
{
    private readonly Form form;
    private readonly SasdCommandManager manager;
    private readonly bool changedKeyPreview;
    private bool disposed;

    /// <summary>Attaches shortcut routing to a form.</summary>
    public SasdCommandShortcutBinding(Form form, SasdCommandManager manager)
    {
        this.form = form ?? throw new ArgumentNullException(nameof(form));
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));

        // KeyPreview is necessary for form-level shortcuts while focus is inside a
        // child editor. Remember whether this binding changed it so disposal can
        // restore the caller's original configuration.
        changedKeyPreview = !form.KeyPreview;
        if (changedKeyPreview)
        {
            form.KeyPreview = true;
        }

        form.KeyDown += OnKeyDown;
    }

    /// <summary>Occurs after a shortcut command finishes, including recoverable failures.</summary>
    public event EventHandler<SasdShortcutCommandCompletedEventArgs>? CommandCompleted;

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        form.KeyDown -= OnKeyDown;
        if (changedKeyPreview && !form.IsDisposed && !form.Disposing)
        {
            form.KeyPreview = false;
        }

        disposed = true;
        GC.SuppressFinalize(this);
    }

    private async void OnKeyDown(object? sender, KeyEventArgs e)
    {
        SasdCommand? command = manager.FindByShortcut(e.KeyData);
        if (command is null || !command.Enabled || !command.Visible)
        {
            return;
        }

        // Once a live command claims the shortcut, prevent a focused child control
        // from also processing the same keystroke (for example Ctrl+S inserting a
        // control character in an editor).
        e.Handled = true;
        e.SuppressKeyPress = true;

        UiOperationResult result = await manager.ExecuteAsync(command.Id).ConfigureAwait(true);
        CommandCompleted?.Invoke(this, new SasdShortcutCommandCompletedEventArgs(command, result));
    }
}
