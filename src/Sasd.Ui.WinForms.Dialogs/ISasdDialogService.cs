namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Provides the common modal dialogs used by SASD WinForms applications.</summary>
public interface ISasdDialogService
{
    /// <summary>Shows an informational message.</summary>
    void ShowInformation(IWin32Window? owner, string message, string title);

    /// <summary>Shows a warning message.</summary>
    void ShowWarning(IWin32Window? owner, string message, string title);

    /// <summary>Shows a user-safe error message.</summary>
    void ShowError(IWin32Window? owner, string message, string title);

    /// <summary>Asks a yes/no question and returns true only for an explicit yes.</summary>
    bool Confirm(IWin32Window? owner, string message, string title);
}
