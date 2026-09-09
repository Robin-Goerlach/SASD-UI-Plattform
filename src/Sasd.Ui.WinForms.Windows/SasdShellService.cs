using System.Diagnostics;
using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Windows;

/// <summary>Provides deliberately constrained Windows shell operations.</summary>
public sealed class SasdShellService
{
    private static readonly HashSet<string> AllowedUriSchemes = new(StringComparer.OrdinalIgnoreCase)
    {
        Uri.UriSchemeHttp,
        Uri.UriSchemeHttps,
        "mailto",
    };

    /// <summary>Opens an existing file or directory using the registered Windows handler.</summary>
    public UiOperationResult OpenPath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var fullPath = Path.GetFullPath(path);
        if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
        {
            return UiOperationResult.Failure("The requested file or folder does not exist.", errorCode: "PATH_NOT_FOUND");
        }

        return Start(fullPath);
    }

    /// <summary>Opens an HTTP, HTTPS or mailto URI using the registered Windows handler.</summary>
    public UiOperationResult OpenUri(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        if (!uri.IsAbsoluteUri || !AllowedUriSchemes.Contains(uri.Scheme))
        {
            return UiOperationResult.Failure("The requested link uses an unsupported scheme.", errorCode: "URI_SCHEME_NOT_ALLOWED");
        }

        return Start(uri.AbsoluteUri);
    }

    private static UiOperationResult Start(string target)
    {
        try
        {
            Process.Start(new ProcessStartInfo(target)
            {
                UseShellExecute = true,
            });
            return UiOperationResult.Success();
        }
        catch (Exception exception) when (exception is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return UiOperationResult.Failure(
                "Windows could not open the requested item.",
                exception.Message,
                "SHELL_OPEN_FAILED",
                exception);
        }
    }
}
