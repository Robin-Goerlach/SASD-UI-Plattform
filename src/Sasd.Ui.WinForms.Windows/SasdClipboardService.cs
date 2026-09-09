using System.Runtime.InteropServices;
using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Windows;

/// <summary>Abstraction for text clipboard operations used by SASD applications.</summary>
public interface ISasdClipboardService
{
    /// <summary>Copies non-null text to the Windows clipboard.</summary>
    UiOperationResult SetText(string text);

    /// <summary>Returns clipboard text when available.</summary>
    UiOperationResult<string?> GetText();
}

/// <summary>
/// Provides explicit text clipboard operations with recoverable failure results.
/// The service is intentionally instance-based so applications can inject, replace,
/// or fake it without static calls scattered through UI code.
/// </summary>
public sealed class SasdClipboardService : ISasdClipboardService
{
    /// <inheritdoc />
    public UiOperationResult SetText(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        try
        {
            Clipboard.SetText(text);
            return UiOperationResult.Success();
        }
        catch (ExternalException exception)
        {
            return UiOperationResult.Failure(
                "The clipboard is currently unavailable.",
                exception.Message,
                "CLIPBOARD_WRITE_FAILED",
                exception);
        }
    }

    /// <inheritdoc />
    public UiOperationResult<string?> GetText()
    {
        try
        {
            return Clipboard.ContainsText()
                ? UiOperationResult<string?>.Success(Clipboard.GetText())
                : UiOperationResult<string?>.Success(null);
        }
        catch (ExternalException exception)
        {
            return UiOperationResult<string?>.Failure(
                "The clipboard is currently unavailable.",
                exception.Message,
                "CLIPBOARD_READ_FAILED");
        }
    }
}
