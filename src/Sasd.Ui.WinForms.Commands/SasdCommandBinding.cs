using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Commands;

/// <summary>Synchronises one command with a WinForms button or ToolStrip item.</summary>
public sealed class SasdCommandBinding : IDisposable
{
    private readonly SasdCommand command;
    private readonly SasdCommandRunner runner;
    private readonly ButtonBase? button;
    private readonly ToolStripItem? toolStripItem;
    private bool disposed;

    private SasdCommandBinding(SasdCommand command, SasdCommandRunner runner, ButtonBase? button, ToolStripItem? toolStripItem)
    {
        this.command = command;
        this.runner = runner;
        this.button = button;
        this.toolStripItem = toolStripItem;

        command.StateChanged += OnCommandStateChanged;
        if (button is not null)
        {
            button.Click += OnSurfaceClick;
        }

        if (toolStripItem is not null)
        {
            toolStripItem.Click += OnSurfaceClick;
        }

        UpdateSurface();
    }

    /// <summary>Raised after execution, including recoverable failure information.</summary>
    public event EventHandler<UiOperationResult>? CommandCompleted;

    /// <summary>Creates a binding for a button-like control.</summary>
    public static SasdCommandBinding Bind(ButtonBase button, SasdCommand command, SasdCommandRunner runner)
    {
        ArgumentNullException.ThrowIfNull(button);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(runner);
        return new SasdCommandBinding(command, runner, button, null);
    }

    /// <summary>Creates a binding for a ToolStrip item.</summary>
    public static SasdCommandBinding Bind(ToolStripItem item, SasdCommand command, SasdCommandRunner runner)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(runner);
        return new SasdCommandBinding(command, runner, null, item);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        command.StateChanged -= OnCommandStateChanged;
        if (button is not null)
        {
            button.Click -= OnSurfaceClick;
        }

        if (toolStripItem is not null)
        {
            toolStripItem.Click -= OnSurfaceClick;
        }
    }

    private void OnCommandStateChanged(object? sender, EventArgs e) => UpdateSurface();

    private async void OnSurfaceClick(object? sender, EventArgs e)
    {
        var result = await runner.ExecuteAsync(command).ConfigureAwait(true);
        CommandCompleted?.Invoke(this, result);
    }

    private void UpdateSurface()
    {
        if (button is not null)
        {
            button.Text = command.Text;
            button.Enabled = command.Enabled;
            button.Visible = command.Visible;
        }

        if (toolStripItem is not null)
        {
            toolStripItem.Text = command.Text;
            toolStripItem.Enabled = command.Enabled;
            toolStripItem.Visible = command.Visible;
            toolStripItem.ToolTipText = command.Description ?? command.Text;
            if (toolStripItem is ToolStripMenuItem menuItem)
            {
                menuItem.Checked = command.IsChecked;
                menuItem.ShortcutKeys = command.ShortcutKeys;
            }
        }
    }
}
