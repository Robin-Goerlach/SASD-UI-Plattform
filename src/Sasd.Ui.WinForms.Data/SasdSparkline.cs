using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Renders a compact trend line for a small sequence of numeric values.
/// </summary>
/// <remarks>
/// <para>
/// The sparkline is intentionally not a charting engine. It has no axes, legends, interaction,
/// aggregation or data-source abstraction. It is suitable for compact dashboard context where the
/// exact value is presented separately by a KPI or normal text control.
/// </para>
/// <para>
/// Values are copied on assignment. NaN and infinity are rejected because they make visual scaling
/// ambiguous. The component never interprets whether an upward or downward trend is good or bad.
/// </para>
/// </remarks>
public sealed class SasdSparkline : Control
{
    private double[] values = [];
    private float lineWidth = 2F;

    /// <summary>Initialises an empty sparkline using the current Windows highlight color.</summary>
    public SasdSparkline()
    {
        AccessibleName = "Trend";
        BackColor = SystemColors.Window;
        ForeColor = SystemColors.Highlight;
        MinimumSize = new Size(80, 24);
        Size = new Size(160, 42);
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint,
            true);
        UpdateAccessibilityDescription();
    }

    /// <summary>Occurs after the value sequence is replaced or cleared.</summary>
    public event EventHandler? ValuesChanged;

    /// <summary>Gets a snapshot of the current values.</summary>
    [Browsable(false)]
    public IReadOnlyList<double> Values => values.ToArray();

    /// <summary>Gets the number of values currently displayed.</summary>
    [Browsable(false)]
    public int ValueCount => values.Length;

    /// <summary>Gets or sets the rendered line width in logical pixels.</summary>
    [Category("SASD")]
    [DefaultValue(2F)]
    public float LineWidth
    {
        get => lineWidth;
        set
        {
            if (!float.IsFinite(value) || value is < 1F or > 12F)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Line width must be between 1 and 12 pixels.");
            }

            if (Math.Abs(lineWidth - value) < float.Epsilon)
            {
                return;
            }

            lineWidth = value;
            Invalidate();
        }
    }

    /// <summary>Copies and displays a sequence of finite numeric values.</summary>
    public void SetValues(IEnumerable<double> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        double[] replacement = source.ToArray();
        if (replacement.Length > 10_000)
        {
            throw new ArgumentOutOfRangeException(nameof(source), "A sparkline may contain at most 10000 values.");
        }

        if (replacement.Any(value => !double.IsFinite(value)))
        {
            throw new ArgumentException("Sparkline values must be finite numbers.", nameof(source));
        }

        values = replacement;
        UpdateAccessibilityDescription();
        Invalidate();
        ValuesChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Removes all trend values.</summary>
    public void ClearValues()
    {
        if (values.Length == 0)
        {
            return;
        }

        values = [];
        UpdateAccessibilityDescription();
        Invalidate();
        ValuesChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (values.Length == 0)
        {
            return;
        }

        Rectangle client = ClientRectangle;
        if (client.Width < 2 || client.Height < 2)
        {
            return;
        }

        const float padding = 2F;
        float plotWidth = Math.Max(1F, client.Width - (padding * 2F));
        float plotHeight = Math.Max(1F, client.Height - (padding * 2F));
        double minimum = values.Min();
        double maximum = values.Max();
        double range = maximum - minimum;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(ForeColor, lineWidth)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round,
        };

        if (values.Length == 1)
        {
            float radius = Math.Max(1.5F, lineWidth);
            float x = padding + (plotWidth / 2F);
            float y = padding + (plotHeight / 2F);
            using var brush = new SolidBrush(ForeColor);
            e.Graphics.FillEllipse(brush, x - radius, y - radius, radius * 2F, radius * 2F);
            return;
        }

        var points = new PointF[values.Length];
        for (int index = 0; index < values.Length; index++)
        {
            float x = padding + ((plotWidth * index) / (values.Length - 1));
            float y = range <= double.Epsilon
                ? padding + (plotHeight / 2F)
                : padding + (float)((maximum - values[index]) / range * plotHeight);
            points[index] = new PointF(x, y);
        }

        e.Graphics.DrawLines(pen, points);
    }

    private void UpdateAccessibilityDescription()
    {
        if (values.Length == 0)
        {
            AccessibleDescription = "No trend values.";
            return;
        }

        string first = values[0].ToString("G", CultureInfo.CurrentCulture);
        string last = values[^1].ToString("G", CultureInfo.CurrentCulture);
        string direction = values[^1].CompareTo(values[0]) switch
        {
            > 0 => "rising",
            < 0 => "falling",
            _ => "unchanged",
        };

        AccessibleDescription = $"Trend with {values.Length} values, {direction}; first {first}, last {last}.";
    }
}
