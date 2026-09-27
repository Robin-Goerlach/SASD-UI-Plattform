Set-StrictMode -Version Latest

function Get-SasdProductProjects {
    <#
    .SYNOPSIS
    Returns the repository-relative product projects that participate in release evidence.

    .DESCRIPTION
    Keep this list explicit. Adding a project here is a reviewable acknowledgement that
    its assembly/package belongs to the current product line. The list is shared by the
    package dry-run and dependency-evidence scripts so those release checks cannot drift
    silently apart.

    This is an internal release-engineering inventory. It does not decide which projects
    eventually become separate public NuGet packages.
    #>
    return @(
        'src/Sasd.Ui.Core/Sasd.Ui.Core.csproj',
        'src/Sasd.Ui.WinForms/Sasd.Ui.WinForms.csproj',
        'src/Sasd.Ui.WinForms.Commands/Sasd.Ui.WinForms.Commands.csproj',
        'src/Sasd.Ui.WinForms.Data/Sasd.Ui.WinForms.Data.csproj',
        'src/Sasd.Ui.WinForms.Dialogs/Sasd.Ui.WinForms.Dialogs.csproj',
        'src/Sasd.Ui.WinForms.Forms/Sasd.Ui.WinForms.Forms.csproj',
        'src/Sasd.Ui.WinForms.Media/Sasd.Ui.WinForms.Media.csproj',
        'src/Sasd.Ui.WinForms.Shell/Sasd.Ui.WinForms.Shell.csproj',
        'src/Sasd.Ui.WinForms.State/Sasd.Ui.WinForms.State.csproj',
        'src/Sasd.Ui.WinForms.Theming/Sasd.Ui.WinForms.Theming.csproj',
        'src/Sasd.Ui.WinForms.Windows/Sasd.Ui.WinForms.Windows.csproj',
        'src/Sasd.Ui.WinForms.Krypton/Sasd.Ui.WinForms.Krypton.csproj'
    )
}
