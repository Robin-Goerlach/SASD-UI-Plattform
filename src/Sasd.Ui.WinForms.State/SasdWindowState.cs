namespace Sasd.Ui.WinForms.State;

/// <summary>Serializable, machine-local window placement state.</summary>
public sealed record SasdWindowState(
    int X,
    int Y,
    int Width,
    int Height,
    FormWindowState WindowState)
{
    /// <summary>Captures safe placement information from a form.</summary>
    public static SasdWindowState Capture(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);
        var bounds = form.WindowState == FormWindowState.Normal ? form.Bounds : form.RestoreBounds;
        var state = form.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : form.WindowState;
        return new(bounds.X, bounds.Y, bounds.Width, bounds.Height, state);
    }

    /// <summary>Restores the placement while ensuring that the window remains visible.</summary>
    public void Restore(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var minimumWidth = Math.Max(form.MinimumSize.Width, 320);
        var minimumHeight = Math.Max(form.MinimumSize.Height, 200);
        var requested = new Rectangle(X, Y, Math.Max(Width, minimumWidth), Math.Max(Height, minimumHeight));
        var target = EnsureVisible(requested);

        form.StartPosition = FormStartPosition.Manual;
        form.Bounds = target;
        form.WindowState = WindowState == FormWindowState.Maximized
            ? FormWindowState.Maximized
            : FormWindowState.Normal;
    }

    private static Rectangle EnsureVisible(Rectangle requested)
    {
        var screens = Screen.AllScreens;
        if (screens.Any(screen => Rectangle.Intersect(screen.WorkingArea, requested).Width >= 80
            && Rectangle.Intersect(screen.WorkingArea, requested).Height >= 40))
        {
            return requested;
        }

        var workingArea = Screen.PrimaryScreen?.WorkingArea ?? SystemInformation.WorkingArea;
        var width = Math.Min(requested.Width, workingArea.Width);
        var height = Math.Min(requested.Height, workingArea.Height);
        return new Rectangle(
            workingArea.Left + Math.Max(0, (workingArea.Width - width) / 2),
            workingArea.Top + Math.Max(0, (workingArea.Height - height) / 2),
            width,
            height);
    }
}
