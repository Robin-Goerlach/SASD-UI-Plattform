using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>
/// Hosts application-owned filter controls in a consistent horizontal bar.
/// The component deliberately does not interpret filter values; query semantics
/// remain the responsibility of the consuming application or grid controller.
/// </summary>
[DefaultEvent(nameof(ClearRequested))]
public sealed class SasdFilterBar : UserControl
{
    private readonly FlowLayoutPanel filterHost;
    private readonly Label stateLabel;
    private readonly Button clearButton;
    private int activeFilterCount;

    /// <summary>Initialises the filter bar.</summary>
    public SasdFilterBar()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(0, 4, 0, 4);

        filterHost = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Margin = Padding.Empty,
        };

        stateLabel = new Label
        {
            AutoSize = true,
            Margin = new Padding(8, 8, 4, 0),
            AccessibleName = "Active filters",
        };

        clearButton = new Button
        {
            AutoSize = true,
            Text = "Clear filters",
            Margin = new Padding(8, 2, 0, 0),
            AccessibleName = "Clear filters",
        };
        clearButton.Click += (_, _) => ClearRequested?.Invoke(this, EventArgs.Empty);

        var layout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 3,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.Controls.Add(filterHost, 0, 0);
        layout.Controls.Add(stateLabel, 1, 0);
        layout.Controls.Add(clearButton, 2, 0);
        Controls.Add(layout);

        UpdateStateLabel();
    }

    /// <summary>
    /// Raised when the user asks the application to clear its filters. The bar does
    /// not modify editor values automatically because their types and semantics are
    /// application-owned.
    /// </summary>
    public event EventHandler? ClearRequested;

    /// <summary>Gets or sets the number of filters currently affecting the query.</summary>
    [DefaultValue(0)]
    public int ActiveFilterCount
    {
        get => activeFilterCount;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            if (activeFilterCount == value)
            {
                return;
            }

            activeFilterCount = value;
            UpdateStateLabel();
        }
    }

    /// <summary>Adds an application-owned editor or filter control to the bar.</summary>
    public void AddFilter(Control control)
    {
        ArgumentNullException.ThrowIfNull(control);
        control.Margin = control.Margin == Padding.Empty ? new Padding(0, 2, 8, 2) : control.Margin;
        filterHost.Controls.Add(control);
    }

    /// <summary>Removes an editor from the bar without disposing it.</summary>
    public bool RemoveFilter(Control control)
    {
        ArgumentNullException.ThrowIfNull(control);
        if (!filterHost.Controls.Contains(control))
        {
            return false;
        }

        filterHost.Controls.Remove(control);
        return true;
    }

    private void UpdateStateLabel()
    {
        stateLabel.Text = activeFilterCount switch
        {
            0 => "No active filters",
            1 => "1 active filter",
            _ => $"{activeFilterCount} active filters",
        };
        clearButton.Enabled = activeFilterCount > 0;
    }
}
