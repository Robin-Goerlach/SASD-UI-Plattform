using System.Drawing;
using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Theming;

/// <summary>
/// Applies vendor-neutral SASD theme definitions to native WinForms controls.
/// Applications decide which control trees are themed; the service does not use
/// a global service locator.
/// </summary>
public sealed class SasdThemeService
{
    /// <summary>Initialises the service with the SASD light theme.</summary>
    public SasdThemeService()
        : this(SasdThemeCatalog.Light)
    {
    }

    /// <summary>Initialises the service with a supplied theme.</summary>
    public SasdThemeService(SasdThemeDefinition initialTheme)
    {
        ArgumentNullException.ThrowIfNull(initialTheme);
        Current = initialTheme;
    }

    /// <summary>Gets the currently active theme definition.</summary>
    public SasdThemeDefinition Current { get; private set; }

    /// <summary>Occurs after the active definition changed.</summary>
    public event EventHandler<SasdThemeChangedEventArgs>? ThemeChanged;

    /// <summary>Changes the active theme without automatically traversing forms.</summary>
    public void SetTheme(SasdThemeDefinition theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        if (Equals(Current, theme))
        {
            return;
        }

        SasdThemeDefinition previous = Current;
        Current = theme;
        ThemeChanged?.Invoke(this, new SasdThemeChangedEventArgs(previous, theme));
    }

    /// <summary>Applies the active theme recursively to a control tree.</summary>
    public void Apply(Control root)
    {
        ArgumentNullException.ThrowIfNull(root);
        ApplyControl(root, ResolvePalette(Current));
    }

    /// <summary>Changes the active theme and applies it to the supplied roots.</summary>
    public void SetThemeAndApply(SasdThemeDefinition theme, params Control[] roots)
    {
        ArgumentNullException.ThrowIfNull(theme);
        ArgumentNullException.ThrowIfNull(roots);

        SetTheme(theme);
        foreach (Control root in roots)
        {
            if (root is not null && !root.IsDisposed)
            {
                Apply(root);
            }
        }
    }

    private static void ApplyControl(Control control, ThemePalette palette)
    {
        switch (control)
        {
            case DataGridView grid:
                ApplyGrid(grid, palette);
                break;
            case TextBoxBase:
            case ComboBox:
            case ListBox:
            case ListView:
            case TreeView:
                control.BackColor = palette.Surface;
                control.ForeColor = palette.Text;
                break;
            case Button button:
                button.BackColor = palette.SurfaceVariant;
                button.ForeColor = palette.Text;
                button.UseVisualStyleBackColor = false;
                break;
            case Label label:
                label.ForeColor = palette.Text;
                if (label.Parent is not null)
                {
                    label.BackColor = label.Parent.BackColor;
                }
                break;
            case GroupBox:
                control.BackColor = palette.Background;
                control.ForeColor = palette.Text;
                break;
            default:
                control.BackColor = control is Form ? palette.Background : palette.Surface;
                control.ForeColor = palette.Text;
                break;
        }

        foreach (Control child in control.Controls)
        {
            ApplyControl(child, palette);
        }
    }

    private static void ApplyGrid(DataGridView grid, ThemePalette palette)
    {
        grid.BackgroundColor = palette.Surface;
        grid.GridColor = palette.Border;
        grid.ForeColor = palette.Text;
        grid.DefaultCellStyle.BackColor = palette.Surface;
        grid.DefaultCellStyle.ForeColor = palette.Text;
        grid.DefaultCellStyle.SelectionBackColor = palette.Selection;
        grid.DefaultCellStyle.SelectionForeColor = palette.SelectionText;
        grid.AlternatingRowsDefaultCellStyle.BackColor = palette.SurfaceVariant;
        grid.ColumnHeadersDefaultCellStyle.BackColor = palette.SurfaceVariant;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = palette.Text;
        grid.RowHeadersDefaultCellStyle.BackColor = palette.SurfaceVariant;
        grid.RowHeadersDefaultCellStyle.ForeColor = palette.Text;
        grid.EnableHeadersVisualStyles = false;
    }

    private static ThemePalette ResolvePalette(SasdThemeDefinition theme)
    {
        if (theme.Mode == ThemeMode.HighContrast || SystemInformation.HighContrast)
        {
            return new ThemePalette(
                SystemColors.Window,
                SystemColors.Window,
                SystemColors.Control,
                SystemColors.WindowText,
                SystemColors.Highlight,
                SystemColors.HighlightText,
                SystemColors.ActiveBorder);
        }

        return new ThemePalette(
            Parse(theme.Colors.Background, SystemColors.Control),
            Parse(theme.Colors.Surface, SystemColors.Window),
            Parse(theme.Colors.SurfaceVariant, SystemColors.Control),
            Parse(theme.Colors.Text, SystemColors.WindowText),
            Parse(theme.Colors.Selection, SystemColors.Highlight),
            Parse(theme.Colors.Text, SystemColors.HighlightText),
            Parse(theme.Colors.Border, SystemColors.ActiveBorder));
    }

    private static Color Parse(string value, Color fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return fallback;
        }

        try
        {
            return ColorTranslator.FromHtml(value);
        }
        catch (Exception)
        {
            return fallback;
        }
    }

    private sealed record ThemePalette(
        Color Background,
        Color Surface,
        Color SurfaceVariant,
        Color Text,
        Color Selection,
        Color SelectionText,
        Color Border);
}
