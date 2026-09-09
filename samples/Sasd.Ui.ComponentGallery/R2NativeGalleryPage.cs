using System.ComponentModel;
using System.Drawing.Drawing2D;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Media;

namespace Sasd.Ui.ComponentGallery;

/// <summary>
/// Demonstrates the dependency-free native R2 controls as one small application surface.
/// </summary>
/// <remarks>
/// <para>
/// The page intentionally avoids implementing fake database/query infrastructure. Search text and filters
/// are shown as UI state because a real consumer remains responsible for translating them into its own
/// repository, API or in-memory query logic.
/// </para>
/// <para>
/// The examples favour clarity over polish. They are meant to expose component contracts, ownership and
/// lifecycle rules before the controls are adopted by real SASD applications.
/// </para>
/// </remarks>
internal sealed class R2NativeGalleryPage : UserControl
{
    private readonly SasdDataGrid grid = new()
    {
        AutoGenerateColumns = false,
        FillAvailableWidth = false,
        Dock = DockStyle.Fill,
    };
    private readonly SasdSearchBox searchBox = new() { Dock = DockStyle.Fill };
    private readonly SasdFilterBar filterBar = new() { Dock = DockStyle.Fill };
    private readonly SasdGridColumnChooser columnChooser = new() { Dock = DockStyle.Fill };
    private readonly Label savedViewStatus = new()
    {
        AutoSize = true,
        ForeColor = SystemColors.GrayText,
        Text = "No saved view captured yet.",
    };
    private SasdGridViewDefinition? savedView;

    /// <summary>Creates the native R2 Gallery page and its deterministic sample state.</summary>
    public R2NativeGalleryPage()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(16);
        AccessibleName = "Native R2 component examples";

        ConfigureGrid();
        columnChooser.Bind(grid);
        searchBox.SearchTextChanged += OnSearchOrFilterStateChanged;
        filterBar.FiltersChanged += OnSearchOrFilterStateChanged;

        var pageLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
        };
        pageLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pageLayout.Controls.Add(CreateIntro(), 0, 0);

        // Give the split container a realistic design-time size before assigning minimum panel
        // sizes/splitter distance. SplitContainer validates these properties immediately, even
        // before it has been laid out by its eventual parent.
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel2,
            Orientation = Orientation.Vertical,
            Size = new Size(1040, 600),
            Panel1MinSize = 480,
            Panel2MinSize = 320,
            SplitterDistance = 690,
        };
        split.Panel1.Padding = new Padding(0, 12, 8, 0);
        split.Panel2.Padding = new Padding(8, 12, 0, 0);
        split.Panel1.Controls.Add(CreateGridWorkspace());
        split.Panel2.Controls.Add(CreateInspectorWorkspace());
        pageLayout.Controls.Add(split, 0, 1);

        Controls.Add(pageLayout);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            searchBox.SearchTextChanged -= OnSearchOrFilterStateChanged;
            filterBar.FiltersChanged -= OnSearchOrFilterStateChanged;
            columnChooser.Unbind();
        }

        base.Dispose(disposing);
    }

    private static Control CreateIntro() => new Label
    {
        AutoSize = true,
        MaximumSize = new Size(1000, 0),
        Text =
            "Native R2 integration — saved grid views, column chooser, property editor, image viewer and dashboard primitives.\r\n" +
            "These helpers intentionally remain dependency-free. Search/filter state is demonstrated, but data-query translation stays application-owned.",
    };

    private Control CreateGridWorkspace()
    {
        var root = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 5,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        root.Controls.Add(CreateSectionTitle("Grid view and column visibility"), 0, 0);

        var searchAndFilter = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 1,
            Dock = DockStyle.Top,
            RowCount = 2,
        };
        searchAndFilter.Controls.Add(searchBox, 0, 0);
        searchAndFilter.Controls.Add(filterBar, 0, 1);
        root.Controls.Add(searchAndFilter, 0, 1);

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 6, 0, 6),
        };
        actions.Controls.Add(CreateButton("Add status filter", () =>
            filterBar.AddOrUpdateFilter("status", "Status", "Active")));
        actions.Controls.Add(CreateButton("Capture view", CaptureView));
        actions.Controls.Add(CreateButton("Disturb UI", DisturbView));
        actions.Controls.Add(CreateButton("Restore view", RestoreView));
        root.Controls.Add(actions, 0, 2);

        var gridSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel2,
            Orientation = Orientation.Vertical,
            Size = new Size(680, 420),
            Panel1MinSize = 320,
            Panel2MinSize = 180,
            SplitterDistance = 470,
        };
        gridSplit.Panel1.Padding = new Padding(0, 0, 6, 0);
        gridSplit.Panel2.Padding = new Padding(6, 0, 0, 0);
        gridSplit.Panel1.Controls.Add(grid);
        gridSplit.Panel2.Controls.Add(columnChooser);
        root.Controls.Add(gridSplit, 0, 3);
        root.Controls.Add(savedViewStatus, 0, 4);

        return root;
    }

    private Control CreateInspectorWorkspace()
    {
        var root = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 6,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));

        root.Controls.Add(CreateSectionTitle("Dashboard primitives"), 0, 0);
        root.Controls.Add(CreateKpiStrip(), 0, 1);
        root.Controls.Add(CreateSectionTitle("Property editor"), 0, 2);

        var propertyEditor = new SasdPropertyEditor
        {
            Dock = DockStyle.Fill,
            SelectedObject = new DemoSettings(),
        };
        root.Controls.Add(propertyEditor, 0, 3);

        root.Controls.Add(CreateSectionTitle("Image viewer"), 0, 4);
        root.Controls.Add(CreateImageViewer(), 0, 5);
        return root;
    }

    private static Control CreateKpiStrip()
    {
        var host = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
        };

        var activeUsers = new SasdKpiCard
        {
            TitleText = "Active users",
            ValueText = "1,248",
            DetailText = "Sample value — trend is descriptive, not a quality judgement",
            Margin = new Padding(0, 0, 8, 8),
            Size = new Size(250, 142),
        };
        activeUsers.SetTrendValues([980, 1040, 1095, 1080, 1160, 1205, 1248]);

        var incidents = new SasdKpiCard
        {
            TitleText = "Open incidents",
            ValueText = "12",
            DetailText = "2 more than yesterday",
            Margin = new Padding(0, 0, 8, 8),
            Size = new Size(250, 142),
        };
        incidents.SetTrendValues([7, 8, 7, 9, 10, 10, 12]);

        host.Controls.Add(activeUsers);
        host.Controls.Add(incidents);
        return host;
    }

    private static Control CreateImageViewer()
    {
        var viewer = new SasdImageViewer
        {
            Dock = DockStyle.Fill,
            MinimumSize = new Size(220, 160),
        };

        // The viewer clones assigned images. The temporary bitmap can therefore be disposed immediately
        // after SetImage; this demonstrates the ownership contract instead of leaking a GDI resource from
        // the sample for the lifetime of the application.
        using Bitmap source = CreateDemoImage();
        viewer.SetImage(source);
        return viewer;
    }

    private static Bitmap CreateDemoImage()
    {
        var bitmap = new Bitmap(640, 360);
        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(SystemColors.Window);

        using var titleFont = new Font(SystemFonts.DefaultFont.FontFamily, 20F, FontStyle.Bold);
        using var detailFont = new Font(SystemFonts.DefaultFont.FontFamily, 11F, FontStyle.Regular);
        using var accentBrush = new SolidBrush(SystemColors.Highlight);
        using var textBrush = new SolidBrush(SystemColors.WindowText);
        using var mutedBrush = new SolidBrush(SystemColors.GrayText);
        using var borderPen = new Pen(SystemColors.ControlDark, 2F);

        // Stay on APIs available in the .NET 8/System.Drawing baseline. The sample does not need a
        // custom rounded-rectangle helper just to look slightly more decorative.
        graphics.FillRectangle(accentBrush, new Rectangle(36, 36, 160, 84));
        graphics.DrawString("SASD", titleFont, Brushes.White, new PointF(62, 58));
        graphics.DrawString("Image Viewer", titleFont, textBrush, new PointF(228, 48));
        graphics.DrawString("Generated in memory — no file-system dependency", detailFont, mutedBrush, new PointF(230, 88));
        graphics.DrawRectangle(borderPen, new Rectangle(36, 156, 566, 154));
        graphics.DrawString("Fit / Actual Size / Custom Zoom\nThe viewer owns a clone of this bitmap.", detailFont, textBrush, new RectangleF(60, 190, 510, 90));
        return bitmap;
    }

    private void ConfigureGrid()
    {
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "name",
            HeaderText = "Name",
            Width = 190,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "status",
            HeaderText = "Status",
            Width = 110,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "owner",
            HeaderText = "Owner",
            Width = 140,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "updated",
            HeaderText = "Updated",
            Width = 120,
        });

        grid.Rows.Add("Prompt Manager", "Active", "Robin", "Today");
        grid.Rows.Add("Mail Workbench", "Planned", "SASD", "Yesterday");
        grid.Rows.Add("Research Notebook", "Active", "SASD", "Today");
        grid.Rows.Add("Finance Control", "Pilot", "Robin", "This week");
    }

    private void CaptureView()
    {
        savedView = SasdGridViewDefinition.Capture("Gallery view", grid, searchBox, filterBar);
        savedViewStatus.Text = $"Captured '{savedView.Name}' — {savedView.Filters.Count} filter(s), search '{savedView.SearchText}'.";
    }

    private void DisturbView()
    {
        if (grid.Columns.Count >= 3)
        {
            grid.Columns[0].Width = 70;
            grid.Columns[1].Visible = false;
            grid.Columns[2].DisplayIndex = 0;
        }

        searchBox.SearchText = "changed locally";
        filterBar.SetFilters([
            SasdActiveFilter.Create("owner", "Owner", "Robin"),
            SasdActiveFilter.Create("status", "Status", "Pilot"),
        ]);
        savedViewStatus.Text = "UI state changed. Restore the captured view to prove the saved-view contract.";
    }

    private void RestoreView()
    {
        if (savedView is null)
        {
            savedViewStatus.Text = "Capture a view before restoring it.";
            return;
        }

        savedView.Restore(grid, searchBox, filterBar);
        savedViewStatus.Text = $"Restored '{savedView.Name}'. Data was not queried; only UI state was restored.";
    }

    private void OnSearchOrFilterStateChanged(object? sender, EventArgs e)
    {
        if (savedView is null)
        {
            savedViewStatus.Text = $"Current UI state — search '{searchBox.SearchText}', {filterBar.FilterCount} filter(s).";
        }
    }

    private static Label CreateSectionTitle(string text)
    {
        var label = new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 6),
            Text = text,
        };

        // The sample explicitly owns the derived Font instance. Keeping ownership visible here
        // demonstrates the same GDI-resource discipline expected from product controls.
        Font boldFont = new(label.Font, FontStyle.Bold);
        label.Font = boldFont;
        label.Disposed += (_, _) => boldFont.Dispose();
        return label;
    }

    private static Button CreateButton(string text, Action action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 8, 4),
            Text = text,
        };
        button.Click += (_, _) => action();
        return button;
    }

    private sealed class DemoSettings
    {
        [Category("General")]
        [Description("Friendly name used by the sample configuration.")]
        public string Name { get; set; } = "Gallery profile";

        [Category("General")]
        [Description("Whether the example feature is enabled.")]
        public bool Enabled { get; set; } = true;

        [Category("Behaviour")]
        [Description("Retry count shown only to demonstrate normal numeric property editing.")]
        public int RetryCount { get; set; } = 3;

        [Category("Behaviour")]
        [Description("Read-only application-managed value.")]
        [ReadOnly(true)]
        public string Environment => "Development";

        [Browsable(false)]
        public Guid InternalId { get; } = Guid.NewGuid();
    }
}
