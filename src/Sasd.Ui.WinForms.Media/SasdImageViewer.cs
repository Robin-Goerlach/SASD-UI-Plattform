using System.ComponentModel;

namespace Sasd.Ui.WinForms.Media;

/// <summary>Defines how <see cref="SasdImageViewer"/> sizes an image inside its viewport.</summary>
public enum SasdImageZoomMode
{
    /// <summary>Scale the image to fit the available viewport while preserving aspect ratio.</summary>
    Fit,

    /// <summary>Display the image at its native pixel size and use scrolling when necessary.</summary>
    ActualSize,

    /// <summary>Display the image using <see cref="SasdImageViewer.ZoomPercent"/>.</summary>
    Custom,
}

/// <summary>
/// Displays an application-supplied image with conservative zoom and ownership semantics.
/// </summary>
/// <remarks>
/// <para>
/// The viewer never opens files itself. Applications remain responsible for validating and loading
/// image content. When an image is assigned, the viewer clones it and owns only that clone. The
/// caller therefore remains free to dispose its original image immediately after assignment.
/// </para>
/// <para>
/// This first native R2 implementation deliberately provides only the common behaviours: fit,
/// actual-size and custom zoom. Annotation, editing, rotation and advanced image processing belong
/// in later feature modules if real applications require them.
/// </para>
/// </remarks>
public sealed class SasdImageViewer : UserControl
{
    private const int MinimumZoomPercent = 10;
    private const int MaximumZoomPercent = 800;

    private readonly Panel viewport;
    private readonly PictureBox pictureBox;
    private Image? ownedImage;
    private SasdImageZoomMode zoomMode = SasdImageZoomMode.Fit;
    private int zoomPercent = 100;

    /// <summary>Initialises an empty image viewer.</summary>
    public SasdImageViewer()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AccessibleName = "Image viewer";
        BackColor = SystemColors.AppWorkspace;
        MinimumSize = new Size(160, 120);

        viewport = new Panel
        {
            AutoScroll = false,
            BackColor = SystemColors.AppWorkspace,
            Dock = DockStyle.Fill,
        };
        viewport.Resize += OnViewportResize;

        pictureBox = new PictureBox
        {
            BackColor = SystemColors.AppWorkspace,
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            TabStop = false,
            Visible = false,
        };

        viewport.Controls.Add(pictureBox);
        Controls.Add(viewport);
    }

    /// <summary>Occurs after the viewer replaces or clears its owned image copy.</summary>
    public event EventHandler? ImageChanged;

    /// <summary>Occurs after the effective zoom mode or percentage changes.</summary>
    public event EventHandler? ZoomChanged;

    /// <summary>
    /// Gets or sets the source image.
    /// </summary>
    /// <remarks>
    /// Setting this property clones the supplied image. The returned getter value is the viewer's
    /// owned copy and must not be disposed by callers. Prefer <see cref="SetImage"/> when ownership
    /// semantics should be explicit in application code.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Image? Image
    {
        get => ownedImage;
        set => SetImage(value);
    }

    /// <summary>Gets whether the viewer currently contains an image.</summary>
    [Browsable(false)]
    public bool HasImage => ownedImage is not null;

    /// <summary>Gets or sets the active zoom mode.</summary>
    [Category("SASD")]
    [DefaultValue(SasdImageZoomMode.Fit)]
    public SasdImageZoomMode ZoomMode
    {
        get => zoomMode;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(SasdImageZoomMode));
            }

            if (zoomMode == value)
            {
                return;
            }

            zoomMode = value;
            UpdateImageLayout();
            ZoomChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets or sets the custom zoom percentage. Valid values are between 10 and 800 percent.
    /// </summary>
    [Category("SASD")]
    [DefaultValue(100)]
    public int ZoomPercent
    {
        get => zoomPercent;
        set
        {
            if (value is < MinimumZoomPercent or > MaximumZoomPercent)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    $"Zoom must be between {MinimumZoomPercent} and {MaximumZoomPercent} percent.");
            }

            if (zoomPercent == value)
            {
                return;
            }

            zoomPercent = value;
            if (zoomMode == SasdImageZoomMode.Custom)
            {
                UpdateImageLayout();
            }

            ZoomChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Replaces the current image with a clone of <paramref name="image"/>.
    /// </summary>
    /// <remarks>The caller retains ownership of <paramref name="image"/>.</remarks>
    public void SetImage(Image? image)
    {
        // Clone before disposing the old image. This also makes self-assignment safe when a caller
        // intentionally reassigns the value returned by the Image getter.
        Image? replacement = image is null ? null : (Image)image.Clone();

        pictureBox.Image = null;
        ownedImage?.Dispose();
        ownedImage = replacement;
        pictureBox.Image = ownedImage;
        pictureBox.Visible = ownedImage is not null;

        UpdateImageLayout();
        ImageChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Clears and disposes the viewer-owned image copy.</summary>
    public void ClearImage() => SetImage(null);

    /// <summary>Switches to custom zoom and increases the percentage by the requested step.</summary>
    public void ZoomIn(int stepPercent = 10)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stepPercent);
        ZoomMode = SasdImageZoomMode.Custom;
        ZoomPercent = Math.Min(MaximumZoomPercent, ZoomPercent + stepPercent);
    }

    /// <summary>Switches to custom zoom and decreases the percentage by the requested step.</summary>
    public void ZoomOut(int stepPercent = 10)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stepPercent);
        ZoomMode = SasdImageZoomMode.Custom;
        ZoomPercent = Math.Max(MinimumZoomPercent, ZoomPercent - stepPercent);
    }

    /// <summary>Restores the default fit-to-viewport mode and a 100 percent custom value.</summary>
    public void ResetZoom()
    {
        bool percentageChanged = zoomPercent != 100;
        zoomPercent = 100;

        if (zoomMode != SasdImageZoomMode.Fit)
        {
            zoomMode = SasdImageZoomMode.Fit;
            UpdateImageLayout();
            ZoomChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (percentageChanged)
        {
            ZoomChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            viewport.Resize -= OnViewportResize;
            pictureBox.Image = null;
            ownedImage?.Dispose();
            ownedImage = null;
        }

        base.Dispose(disposing);
    }

    private void OnViewportResize(object? sender, EventArgs e)
    {
        if (zoomMode == SasdImageZoomMode.Fit)
        {
            UpdateImageLayout();
        }
    }

    private void UpdateImageLayout()
    {
        if (ownedImage is null)
        {
            viewport.AutoScroll = false;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            return;
        }

        if (zoomMode == SasdImageZoomMode.Fit)
        {
            viewport.AutoScroll = false;
            viewport.AutoScrollPosition = Point.Empty;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            return;
        }

        viewport.AutoScroll = true;
        pictureBox.Dock = DockStyle.None;
        pictureBox.Location = Point.Empty;
        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

        double scale = zoomMode == SasdImageZoomMode.ActualSize ? 1D : zoomPercent / 100D;
        pictureBox.Size = new Size(
            ScaleDimension(ownedImage.Width, scale),
            ScaleDimension(ownedImage.Height, scale));
    }

    private static int ScaleDimension(int source, double scale)
    {
        double scaled = Math.Max(1D, source * scale);
        return scaled >= int.MaxValue ? int.MaxValue : (int)Math.Round(scaled, MidpointRounding.AwayFromZero);
    }
}
