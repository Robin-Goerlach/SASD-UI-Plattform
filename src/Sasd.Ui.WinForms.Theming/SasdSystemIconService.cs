namespace Sasd.Ui.WinForms.Theming;

/// <summary>Semantic icon roles used by common SASD UI surfaces.</summary>
public enum SasdSemanticIcon
{
    /// <summary>Generic application/window icon.</summary>
    Application,

    /// <summary>Informational message or action.</summary>
    Information,

    /// <summary>Warning requiring user attention.</summary>
    Warning,

    /// <summary>Error or failed operation.</summary>
    Error,

    /// <summary>Question or confirmation prompt.</summary>
    Question,
}

/// <summary>Provides semantic images without exposing an icon-file convention to consumers.</summary>
public interface ISasdIconService : IDisposable
{
    /// <summary>
    /// Gets a service-owned image for the requested role. Callers must not dispose
    /// the returned image; the service owns its lifetime.
    /// </summary>
    Image GetImage(SasdSemanticIcon icon);
}

/// <summary>
/// Provides a dependency-free baseline icon implementation backed by Windows
/// system icons.
/// </summary>
/// <remarks>
/// This is intentionally a small R1 baseline rather than a permanent icon-pack
/// decision. A future adapter can implement <see cref="ISasdIconService"/> while
/// callers continue to request semantic roles instead of concrete file names.
/// </remarks>
public sealed class SasdSystemIconService : ISasdIconService
{
    private readonly Dictionary<SasdSemanticIcon, Image> cache = [];
    private bool disposed;

    /// <inheritdoc />
    public Image GetImage(SasdSemanticIcon icon)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        if (cache.TryGetValue(icon, out Image? existing))
        {
            return existing;
        }

        Icon systemIcon = icon switch
        {
            SasdSemanticIcon.Application => SystemIcons.Application,
            SasdSemanticIcon.Information => SystemIcons.Information,
            SasdSemanticIcon.Warning => SystemIcons.Exclamation,
            SasdSemanticIcon.Error => SystemIcons.Error,
            SasdSemanticIcon.Question => SystemIcons.Question,
            _ => throw new ArgumentOutOfRangeException(nameof(icon), icon, "Unknown semantic icon role."),
        };

        // ToBitmap creates an independently owned image. The cache keeps exactly
        // one image per semantic role so repeated toolbar/dialog use does not create
        // additional GDI objects throughout the application lifetime.
        Image created = systemIcon.ToBitmap();
        cache.Add(icon, created);
        return created;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        foreach (Image image in cache.Values)
        {
            image.Dispose();
        }

        cache.Clear();
        disposed = true;
        GC.SuppressFinalize(this);
    }
}
