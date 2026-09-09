using System.Runtime.InteropServices;
using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Windows;

/// <summary>Provides explicit text clipboard operations with recoverable failure results.</summary>
public sealed class SasdClipboardService
{
    /// <summary>Copies non-null text to the Windows clipboard.</summary>
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

    /// <summary>Returns clipboard text when available.</summary>
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
