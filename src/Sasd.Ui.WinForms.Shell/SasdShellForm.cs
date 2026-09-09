using Sasd.Ui.WinForms.Commands;

namespace Sasd.Ui.WinForms.Shell;

/// <summary>
/// Provides a ready-to-use R1 application shell composed from the normal SASD WinForms building blocks.
/// </summary>
/// <remarks>
/// <para>
/// The shell deliberately contains no business navigation, dependency injection or persistence policy.
/// It only wires the common command bar, navigation host, status bar, command registry and global
/// shortcut routing together so applications do not repeat this plumbing on every main window.
/// </para>
/// <para>
/// Commands remain application-owned. The shell registers them and creates command-bar bindings, but
/// it never invents application commands or disposes their business state.
/// </para>
/// </remarks>
public class SasdShellForm : SasdForm
{
    private readonly SasdStatusBinding statusBinding;
    private readonly SasdCommandShortcutBinding shortcutBinding;

    /// <summary>Creates a shell with default command and status services.</summary>
    public SasdShellForm()
        : this(new SasdCommandManager(), new SasdStatusService())
    {
    }

    /// <summary>
    /// Creates a shell using caller-provided command and status services.
    /// </summary>
    /// <remarks>
    /// The shell owns only the WinForms controls and event bindings it creates. The supplied service
    /// instances remain caller-owned and are not disposed by the form.
    /// </remarks>
    public SasdShellForm(SasdCommandManager commandManager, ISasdStatusService statusService)
    {
        CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
        StatusService = statusService ?? throw new ArgumentNullException(nameof(statusService));

        Text = "SASD Application";
        MinimumSize = new Size(640, 480);

        CommandBar = new SasdCommandBar(CommandManager.Runner)
        {
            Dock = DockStyle.Fill,
        };

        NavigationHost = new SasdNavigationHost
        {
            Dock = DockStyle.Fill,
        };

        StatusBar = new SasdStatusBar
        {
            Dock = DockStyle.Fill,
        };

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(CommandBar, 0, 0);
        layout.Controls.Add(NavigationHost, 0, 1);
        layout.Controls.Add(StatusBar, 0, 2);
        Controls.Add(layout);

        statusBinding = new SasdStatusBinding(StatusService, StatusBar);
        shortcutBinding = new SasdCommandShortcutBinding(this, CommandManager);
    }

    /// <summary>Gets the global command registry used by the shell.</summary>
    public SasdCommandManager CommandManager { get; }

    /// <summary>Gets the presentation-neutral status publisher wired to <see cref="StatusBar"/>.</summary>
    public ISasdStatusService StatusService { get; }

    /// <summary>Gets the shell command bar.</summary>
    public SasdCommandBar CommandBar { get; }

    /// <summary>Gets the shell navigation/content host.</summary>
    public SasdNavigationHost NavigationHost { get; }

    /// <summary>Gets the shell status bar.</summary>
    public SasdStatusBar StatusBar { get; }

    /// <summary>
    /// Registers a global command and optionally adds it to the command bar.
    /// </summary>
    /// <returns>The generated toolbar button, or <see langword="null"/> for shortcut/programmatic-only commands.</returns>
    public ToolStripButton? RegisterCommand(SasdCommand command, bool showOnCommandBar = true)
    {
        ArgumentNullException.ThrowIfNull(command);
        CommandManager.Register(command);

        if (!showOnCommandBar)
        {
            return null;
        }

        try
        {
            return CommandBar.AddCommand(command);
        }
        catch
        {
            // Registration and visual binding should behave transactionally. If the
            // command bar rejects the item, do not leave a hidden global command behind.
            CommandManager.Remove(command.Id);
            throw;
        }
    }

    /// <summary>
    /// Removes a command from both the global registry and command bar when present.
    /// </summary>
    public bool UnregisterCommand(string commandId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);

        // A shortcut-only command has no command-bar entry, therefore a false result
        // from RemoveCommand is expected and does not make unregistration fail.
        CommandBar.RemoveCommand(commandId);
        return CommandManager.Remove(commandId);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Dispose subscriptions before child controls. This prevents transient
            // status/shortcut events from racing with WinForms control teardown.
            shortcutBinding.Dispose();
            statusBinding.Dispose();
        }

        base.Dispose(disposing);
    }
}
