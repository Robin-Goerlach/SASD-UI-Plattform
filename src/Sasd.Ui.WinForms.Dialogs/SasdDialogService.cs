namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Native MessageBox implementation of <see cref="ISasdDialogService"/>.</summary>
public sealed class SasdDialogService : ISasdDialogService
{
    /// <inheritdoc />
    public void ShowInformation(IWin32Window? owner, string message, string title) =>
        MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

    /// <inheritdoc />
    public void ShowWarning(IWin32Window? owner, string message, string title) =>
        MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    /// <inheritdoc />
    public void ShowError(IWin32Window? owner, string message, string title) =>
        MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

    /// <inheritdoc />
    public bool Confirm(IWin32Window? owner, string message, string title) =>
        MessageBox.Show(owner, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;
}
