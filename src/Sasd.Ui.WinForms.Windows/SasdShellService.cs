using System.Diagnostics;
using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Windows;

/// <summary>Abstraction for deliberately constrained Windows shell operations.</summary>
public interface ISasdShellService
{
    /// <summary>Opens an existing file or directory using the registered Windows handler.</summary>
    UiOperationResult OpenPath(string path);

    /// <summary>Opens an HTTP, HTTPS or mailto URI using the registered Windows handler.</summary>
    UiOperationResult OpenUri(Uri uri);
}

/// <summary>
/// Provides deliberately constrained Windows shell operations. Only existing local
/// paths and explicitly allowed URI schemes are accepted before Windows is invoked.
/// </summary>
public sealed class SasdShellService : ISasdShellService
{
    private static readonly HashSet<string> AllowedUriSchemes = new(StringComparer.OrdinalIgnoreCase)
    {
        Uri.UriSchemeHttp,
        Uri.UriSchemeHttps,
        "mailto",
    };

    /// <inheritdoc />
    public UiOperationResult OpenPath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        string fullPath;
        try
        {
            // Resolve to an absolute path before existence checks. Invalid user input
            // is reported as a recoverable result rather than escaping as an exception.
            fullPath = Path.GetFullPath(path);
        }
        catch (Exception exception) when (
            exception is ArgumentException or IOException or NotSupportedException or UnauthorizedAccessException)
        {
            return UiOperationResult.Failure(
                "The requested file or folder path is invalid.",
                exception.Message,
                "PATH_INVALID",
                exception);
        }

        if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
        {
            return UiOperationResult.Failure(
                "The requested file or folder does not exist.",
                errorCode: "PATH_NOT_FOUND");
        }

        return Start(fullPath);
    }

    /// <inheritdoc />
    public UiOperationResult OpenUri(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        if (!uri.IsAbsoluteUri || !AllowedUriSchemes.Contains(uri.Scheme))
        {
            return UiOperationResult.Failure(
                "The requested link uses an unsupported scheme.",
                errorCode: "URI_SCHEME_NOT_ALLOWED");
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
