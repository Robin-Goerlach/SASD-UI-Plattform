using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Landing page that explains the showcase and demonstrates compact dashboard controls.</summary>
internal sealed class OverviewPage : UserControl
{
    public OverviewPage()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        var heading = new Label
        {
            AutoSize = true,
            Text = "Integrated SASD UI Platform showcase",
        };

        var description = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "This program behaves like a small desktop application rather than a control catalogue. " +
                "Use the navigation on the left to exercise forms, grids, dialogs, UI-state persistence, " +
                "Windows integration and the current native R2 controls. The toolbar also demonstrates " +
                "global commands and keyboard shortcuts.",
        };

        var shortcuts = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "Useful shortcuts: Ctrl+Alt+L = Light, Ctrl+Alt+D = Dark, Ctrl+Alt+H = High Contrast, " +
                "Ctrl+T = Self Test.",
        };

        var cards = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 12, 0, 12),
            WrapContents = true,
        };
        cards.Controls.Add(CreateCard("UI foundation", "R1", "Reusable WinForms baseline", [4, 5, 6, 8, 9, 11, 12]));
        cards.Controls.Add(CreateCard("Native R2", "Active", "Property, image and dashboard helpers", [1, 1, 2, 3, 5, 7, 9]));
        cards.Controls.Add(CreateCard("Verification", "Strict", "Warnings are treated as errors", [6, 7, 7, 8, 9, 10, 10]));

        var layout = new TableLayoutPanel
        {
            AutoScroll = true,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 5,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(description, 0, 1);
        layout.Controls.Add(shortcuts, 0, 2);
        layout.Controls.Add(cards, 0, 3);
        Controls.Add(layout);
    }

    private static SasdKpiCard CreateCard(
        string title,
        string value,
        string detail,
        IEnumerable<double> trend)
    {
        var card = new SasdKpiCard
        {
            Margin = new Padding(0, 0, 12, 12),
            TitleText = title,
            ValueText = value,
            DetailText = detail,
        };
        card.SetTrendValues(trend);
        return card;
    }
}
