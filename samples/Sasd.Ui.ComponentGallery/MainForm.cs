using Sasd.Ui.WinForms;

namespace Sasd.Ui.ComponentGallery;

/// <summary>
/// Minimal R0 host. It proves the repository composition only; component pages
/// are added incrementally as executable specifications.
/// </summary>
internal sealed class MainForm : SasdForm
{
    public MainForm()
    {
        StateKey = "ComponentGallery.MainWindow";
        Text = "SASD UI Platform — Component Gallery (R0 Scaffold)";
        MinimumSize = new Size(900, 600);
        ClientSize = new Size(1200, 760);
        StartPosition = FormStartPosition.CenterScreen;

        Controls.Add(CreateLayout());
    }

    private static Control CreateLayout()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(32),
        };

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        layout.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 22, FontStyle.Bold),
            Text = "SASD UI Platform",
        });

        layout.Controls.Add(new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 12, 0, 24),
            Text = "R0 repository scaffold. Component states and quality evidence will be added here as executable specifications.",
        });

        var group = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "Prepared architecture",
            Padding = new Padding(20),
        };

        group.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "Core contracts\r\nWinForms foundation\r\nIsolated Krypton pilot\r\nBilingual documentation\r\nArchitecture and quality gates",
        });

        layout.Controls.Add(group);
        return layout;
    }
}
