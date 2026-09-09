using Sasd.Ui.Core;
using Sasd.Ui.WinForms.Commands;

namespace Sasd.Ui.WinForms.Shell;

/// <summary>Event data raised after a command-bar command completes.</summary>
public sealed class SasdCommandBarCommandCompletedEventArgs : EventArgs
{
    /// <summary>Initialises the event data.</summary>
    public SasdCommandBarCommandCompletedEventArgs(SasdCommand command, UiOperationResult result)
    {
        Command = command ?? throw new ArgumentNullException(nameof(command));
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>Gets the command that completed.</summary>
    public SasdCommand Command { get; }

    /// <summary>Gets the recoverable execution result.</summary>
    public UiOperationResult Result { get; }
}

/// <summary>
/// ToolStrip-based command surface that keeps button state synchronized with
/// <see cref="SasdCommand"/> instances.
/// </summary>
/// <remarks>
/// <para>
/// The command bar owns the bindings and ToolStrip items it creates. The
/// application continues to own command instances and business execution logic.
/// </para>
/// <para>
/// Images are intentionally not assigned automatically here. A semantic icon
/// service or the application can choose an image without coupling commands to a
/// particular icon set.
/// </para>
/// </remarks>
public class SasdCommandBar : ToolStrip
{
    private readonly SasdCommandRunner runner;
    private readonly Dictionary<string, CommandEntry> commands = new(StringComparer.OrdinalIgnoreCase);
    private bool disposed;

    /// <summary>Initialises a command bar with its own command runner.</summary>
    public SasdCommandBar()
        : this(new SasdCommandRunner())
    {
    }

    /// <summary>Initialises a command bar with a caller-provided command runner.</summary>
    public SasdCommandBar(SasdCommandRunner runner)
    {
        this.runner = runner ?? throw new ArgumentNullException(nameof(runner));
        GripStyle = ToolStripGripStyle.Hidden;
        RenderMode = ToolStripRenderMode.System;
        AccessibleName = "Command bar";
    }

    /// <summary>Occurs after a command started from this bar has completed.</summary>
    public event EventHandler<SasdCommandBarCommandCompletedEventArgs>? CommandCompleted;

    /// <summary>Gets the number of commands currently represented by the bar.</summary>
    public int CommandCount => commands.Count;

    /// <summary>Adds one command and returns the generated ToolStrip button.</summary>
    public ToolStripButton AddCommand(SasdCommand command)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentNullException.ThrowIfNull(command);

        if (commands.ContainsKey(command.Id))
        {
            throw new ArgumentException($"Command id '{command.Id}' is already present in the command bar.", nameof(command));
        }

        var button = new ToolStripButton
        {
            AccessibleName = command.Text,
            DisplayStyle = ToolStripItemDisplayStyle.Text,
            ToolTipText = command.Description ?? command.Text,
        };

        SasdCommandBinding binding = SasdCommandBinding.Bind(button, command, runner);
        EventHandler<SasdCommandCompletedEventArgs> completionHandler = (_, args) =>
            CommandCompleted?.Invoke(this, new SasdCommandBarCommandCompletedEventArgs(command, args.Result));
        binding.CommandCompleted += completionHandler;

        var entry = new CommandEntry(command, button, binding, completionHandler);
        try
        {
            commands.Add(command.Id, entry);
            Items.Add(button);
            return button;
        }
        catch
        {
            binding.CommandCompleted -= completionHandler;
            binding.Dispose();
            button.Dispose();
            commands.Remove(command.Id);
            throw;
        }
    }

    /// <summary>Adds a normal visual separator and returns it for optional configuration.</summary>
    public ToolStripSeparator AddSeparator()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        var separator = new ToolStripSeparator();
        Items.Add(separator);
        return separator;
    }

    /// <summary>Removes one command and disposes its generated UI/binding resources.</summary>
    public bool RemoveCommand(string commandId)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);

        if (!commands.Remove(commandId, out CommandEntry? entry))
        {
            return false;
        }

        DisposeEntry(entry);
        return true;
    }

    /// <summary>Removes all commands. Separators added separately are retained.</summary>
    public void ClearCommands()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        foreach (CommandEntry entry in commands.Values.ToArray())
        {
            DisposeEntry(entry);
        }

        commands.Clear();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            // Do not call the public ClearCommands method after setting the disposed
            // flag because that method deliberately rejects use-after-dispose.
            foreach (CommandEntry entry in commands.Values.ToArray())
            {
                DisposeEntry(entry);
            }

            commands.Clear();
            disposed = true;
        }

        base.Dispose(disposing);
    }

    private void DisposeEntry(CommandEntry entry)
    {
        entry.Binding.CommandCompleted -= entry.CompletionHandler;
        entry.Binding.Dispose();
        Items.Remove(entry.Item);
        entry.Item.Dispose();
    }

    private sealed record CommandEntry(
        SasdCommand Command,
        ToolStripButton Item,
        SasdCommandBinding Binding,
        EventHandler<SasdCommandCompletedEventArgs> CompletionHandler);
}
