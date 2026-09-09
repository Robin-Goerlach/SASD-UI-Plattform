namespace Sasd.Ui.WinForms.Commands;

/// <summary>
/// Provides a compact ToolStrip surface for <see cref="SasdCommand"/> instances.
/// Command execution and state remain in the command model rather than being
/// duplicated in toolbar click handlers.
/// </summary>
public sealed class SasdCommandBar : ToolStrip
{
    private readonly List<SasdCommandBinding> bindings = [];

    /// <summary>Initialises a command bar.</summary>
    public SasdCommandBar()
    {
        GripStyle = ToolStripGripStyle.Hidden;
        RenderMode = ToolStripRenderMode.System;
        ShowItemToolTips = true;
        CanOverflow = true;
    }

    /// <summary>Adds and binds a command button.</summary>
    public ToolStripButton AddCommand(
        SasdCommand command,
        SasdCommandRunner runner,
        Image? image = null,
        bool showText = true)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(runner);

        var button = new ToolStripButton
        {
            Image = image,
            DisplayStyle = image is null
                ? ToolStripItemDisplayStyle.Text
                : showText ? ToolStripItemDisplayStyle.ImageAndText : ToolStripItemDisplayStyle.Image,
            AccessibleName = command.Text,
            ToolTipText = command.Description ?? command.Text,
            CheckOnClick = false,
        };

        Items.Add(button);
        bindings.Add(SasdCommandBinding.Bind(button, command, runner));
        return button;
    }

    /// <summary>Adds a visual separator between command groups.</summary>
    public ToolStripSeparator AddCommandSeparator()
    {
        var separator = new ToolStripSeparator();
        Items.Add(separator);
        return separator;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (SasdCommandBinding binding in bindings)
            {
                binding.Dispose();
            }

            bindings.Clear();
        }

        base.Dispose(disposing);
    }
}
