using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Media;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Demonstrates native R2 controls that do not require third-party packages.</summary>
internal sealed class AdvancedPage : UserControl
{
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly DemoSettings settings = new();
    private readonly SasdPropertyEditor propertyEditor = new();
    private readonly SasdImageViewer imageViewer = new();
    private int imageSequence;

    public AdvancedPage(Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        propertyEditor.Dock = DockStyle.Fill;
        propertyEditor.SelectedObject = settings;

        imageViewer.Dock = DockStyle.Fill;
        ReplaceDemoImage();

        var imageActions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 0, 0, 8),
        };
        imageActions.Controls.Add(CreateButton("Fit", () => imageViewer.ZoomMode = SasdImageZoomMode.Fit));
        imageActions.Controls.Add(CreateButton("Actual size", () => imageViewer.ZoomMode = SasdImageZoomMode.ActualSize));
        imageActions.Controls.Add(CreateButton("Zoom +", () => imageViewer.ZoomIn(25)));
        imageActions.Controls.Add(CreateButton("Zoom -", () => imageViewer.ZoomOut(25)));
        imageActions.Controls.Add(CreateButton("Reset zoom", imageViewer.ResetZoom));
        imageActions.Controls.Add(CreateButton("New generated image", ReplaceDemoImage));
        imageActions.Controls.Add(CreateButton("Clear image", imageViewer.ClearImage));

        var readOnlyCheckBox = new CheckBox
        {
            AutoSize = true,
            Text = "Force property editor read-only",
        };
        readOnlyCheckBox.CheckedChanged += (_, _) => propertyEditor.ForceReadOnly = readOnlyCheckBox.Checked;

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "Native R2 controls\r\n\r\n" +
                "The property editor adds filtering/read-only projection without changing the selected object. " +
                "The image viewer clones caller-owned images and offers predictable fit/actual/custom zoom. " +
                "The KPI card keeps its textual value primary and uses the sparkline only as supplemental context.",
        };

        var kpi = new SasdKpiCard
        {
            Dock = DockStyle.Top,
            Height = 150,
            TitleText = "Example throughput",
            ValueText = "1,248/min",
            DetailText = "Generated demonstration values",
        };
        kpi.SetTrendValues([820, 910, 940, 1020, 980, 1100, 1175, 1248]);

        var left = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(0, 8, 8, 0),
        };
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        left.Controls.Add(new Label { AutoSize = true, Text = "Searchable property editor" }, 0, 0);
        left.Controls.Add(readOnlyCheckBox, 0, 1);
        left.Controls.Add(propertyEditor, 0, 2);

        var right = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(8, 8, 0, 0),
        };
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        right.Controls.Add(new Label { AutoSize = true, Text = "Image viewer" }, 0, 0);
        right.Controls.Add(imageActions, 0, 1);
        right.Controls.Add(imageViewer, 0, 2);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 470,
        };
        split.Panel1.Controls.Add(left);
        split.Panel2.Controls.Add(right);

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 165F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(kpi, 0, 1);
        layout.Controls.Add(split, 0, 2);
        Controls.Add(layout);
    }

    private void ReplaceDemoImage()
    {
        imageSequence++;
        using Bitmap source = CreateDemoImage(imageSequence);

        // SetImage clones the supplied bitmap. Disposing the source immediately is an intentional
        // demonstration that the application and the viewer have independent ownership lifetimes.
        imageViewer.SetImage(source);
        publishStatus(
            $"Generated image {imageSequence} assigned; caller-owned bitmap disposed.",
            SasdStatusSeverity.Success,
            TimeSpan.FromSeconds(4));
    }

    private static Bitmap CreateDemoImage(int sequence)
    {
        var bitmap = new Bitmap(720, 420);
        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.Clear(SystemColors.Window);

        using var borderPen = new Pen(SystemColors.Highlight, 6F);
        using var accentBrush = new SolidBrush(SystemColors.Highlight);
        using var textBrush = new SolidBrush(SystemColors.WindowText);
        using var font = new Font(SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont, FontStyle.Bold);

        graphics.DrawRectangle(borderPen, 24, 24, 672, 372);
        graphics.FillEllipse(accentBrush, 70, 95, 150, 150);
        graphics.DrawLine(borderPen, 270, 115, 620, 115);
        graphics.DrawLine(borderPen, 270, 180, 570, 180);
        graphics.DrawLine(borderPen, 270, 245, 640, 245);
        graphics.DrawString($"SASD UI Platform — generated image {sequence}", font, textBrush, 70, 305);
        return bitmap;
    }

    private static Button CreateButton(string text, Action action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 8, 8),
            Text = text,
        };
        button.Click += (_, _) => action();
        return button;
    }
}
