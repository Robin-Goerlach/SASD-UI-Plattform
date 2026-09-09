using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Displays one compact key-performance-indicator value together with optional detail text and trend data.
/// </summary>
/// <remarks>
/// <para>
/// The card deliberately does not interpret whether a value or trend is good or bad. A rising value can be
/// desirable for throughput and undesirable for error rate. The consuming application therefore remains
/// responsible for wording and any semantic status presentation.
/// </para>
/// <para>
/// The embedded <see cref="SasdSparkline"/> provides visual context only. The textual KPI value and detail
/// remain the primary information so the control does not rely on color or a miniature chart alone.
/// </para>
/// </remarks>
public sealed class SasdKpiCard : UserControl
{
    private readonly Label titleLabel;
    private readonly Label valueLabel;
    private readonly Label detailLabel;
    private readonly SasdSparkline sparkline;

    /// <summary>Initialises an empty KPI card with DPI-aware layout and system colors.</summary>
    public SasdKpiCard()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = SystemColors.Window;
        ForeColor = SystemColors.WindowText;
        Padding = new Padding(12);
        MinimumSize = new Size(180, 128);
        Size = new Size(260, 150);
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "KPI";

        titleLabel = new Label
        {
            AutoEllipsis = true,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font(Font, FontStyle.Bold),
            Text = "KPI",
            TextAlign = ContentAlignment.MiddleLeft,
        };

        valueLabel = new Label
        {
            AutoEllipsis = true,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font(Font.FontFamily, Font.Size * 1.8F, FontStyle.Bold),
            Text = "—",
            TextAlign = ContentAlignment.MiddleLeft,
        };

        detailLabel = new Label
        {
            AutoEllipsis = true,
            AutoSize = false,
            Dock = DockStyle.Fill,
            ForeColor = SystemColors.GrayText,
            Text = string.Empty,
            TextAlign = ContentAlignment.MiddleLeft,
        };

        sparkline = new SasdSparkline
        {
            AccessibleName = "KPI trend",
            BackColor = BackColor,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
        };

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            RowCount = 4,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(titleLabel, 0, 0);
        layout.Controls.Add(valueLabel, 0, 1);
        layout.Controls.Add(detailLabel, 0, 2);
        layout.Controls.Add(sparkline, 0, 3);

        Controls.Add(layout);
        UpdateAccessibilityDescription();
    }

    /// <summary>Gets or sets the short title that identifies the KPI.</summary>
    [Category("SASD")]
    [DefaultValue("KPI")]
    public string TitleText
    {
        get => titleLabel.Text;
        set
        {
            titleLabel.Text = value ?? string.Empty;
            AccessibleName = string.IsNullOrWhiteSpace(titleLabel.Text) ? "KPI" : titleLabel.Text;
            UpdateAccessibilityDescription();
        }
    }

    /// <summary>Gets or sets the primary value text displayed prominently in the card.</summary>
    [Category("SASD")]
    [DefaultValue("—")]
    public string ValueText
    {
        get => valueLabel.Text;
        set
        {
            valueLabel.Text = value ?? string.Empty;
            UpdateAccessibilityDescription();
        }
    }

    /// <summary>Gets or sets optional explanatory text such as a period comparison or unit.</summary>
    [Category("SASD")]
    [DefaultValue("")]
    public string DetailText
    {
        get => detailLabel.Text;
        set
        {
            detailLabel.Text = value ?? string.Empty;
            detailLabel.Visible = detailLabel.Text.Length > 0;
            UpdateAccessibilityDescription();
        }
    }

    /// <summary>Gets or sets whether the compact trend line is shown.</summary>
    [Category("SASD")]
    [DefaultValue(true)]
    public bool ShowSparkline
    {
        get => sparkline.Visible;
        set => sparkline.Visible = value;
    }

    /// <summary>Gets the number of trend values currently displayed by the card.</summary>
    [Browsable(false)]
    public int TrendValueCount => sparkline.ValueCount;

    /// <summary>
    /// Copies a sequence of trend values into the embedded sparkline.
    /// </summary>
    /// <remarks>
    /// The same finite-value and maximum-count validation as <see cref="SasdSparkline.SetValues"/> applies.
    /// </remarks>
    public void SetTrendValues(IEnumerable<double> values)
    {
        sparkline.SetValues(values);
        UpdateAccessibilityDescription();
    }

    /// <summary>Clears all trend values while retaining title, value and detail text.</summary>
    public void ClearTrendValues()
    {
        sparkline.ClearValues();
        UpdateAccessibilityDescription();
    }

    /// <inheritdoc />
    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (sparkline is not null)
        {
            sparkline.BackColor = BackColor;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // The fonts below were created by this composite control and therefore belong to it.
            // Labels do not own custom Font instances in a way that guarantees disposal when replacing
            // or disposing the label, so dispose them explicitly with the control lifecycle.
            titleLabel.Font.Dispose();
            valueLabel.Font.Dispose();
        }

        base.Dispose(disposing);
    }

    private void UpdateAccessibilityDescription()
    {
        string title = string.IsNullOrWhiteSpace(titleLabel.Text) ? "KPI" : titleLabel.Text.Trim();
        string value = string.IsNullOrWhiteSpace(valueLabel.Text) ? "no value" : valueLabel.Text.Trim();
        string detail = string.IsNullOrWhiteSpace(detailLabel.Text) ? string.Empty : $" {detailLabel.Text.Trim()}.";
        string trend = sparkline.ValueCount == 0 ? " No trend data." : $" {sparkline.AccessibleDescription}";

        AccessibleDescription = $"{title}: {value}.{detail}{trend}".Trim();
    }
}
