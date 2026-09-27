Set-StrictMode -Version Latest

function Get-SasdProductProjects {
    <#
    .SYNOPSIS
    Returns the repository-relative product projects that participate in release evidence.

    .DESCRIPTION
    Keep this list explicit. Adding a project here is a reviewable acknowledgement that
    its assembly or metapackage belongs to the current product line. The list is shared by
    packaging and dependency-evidence scripts so release checks cannot drift silently apart.

    This is an internal release-engineering inventory. It is intentionally more granular
    than the supported R1 consumer-entry package set.
    #>
    return @(
        'src/Sasd.Ui.Core/Sasd.Ui.Core.csproj',
        'src/Sasd.Ui.WinForms/Sasd.Ui.WinForms.csproj',
        'src/Sasd.Ui.WinForms.App/Sasd.Ui.WinForms.App.csproj',
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

function Get-SasdMetapackageProjects {
    <#
    .SYNOPSIS
    Returns product projects whose NuGet package intentionally has no runtime assembly.
    #>
    return @(
        'src/Sasd.Ui.WinForms.App/Sasd.Ui.WinForms.App.csproj'
    )
}

function Get-SasdAppMetapackageDependencyIds {
    <#
    .SYNOPSIS
    Returns the direct R1 module dependencies that define Sasd.Ui.WinForms.App.

    .DESCRIPTION
    Keeping this contract explicit lets package verification detect accidental additions
    such as Data, Krypton or future R2 adapters. Those capabilities require an explicit
    consumer choice and must never drift into the general application metapackage.
    #>
    return @(
        'Sasd.Ui.WinForms.Commands',
        'Sasd.Ui.WinForms.Dialogs',
        'Sasd.Ui.WinForms.Forms',
        'Sasd.Ui.WinForms.Shell',
        'Sasd.Ui.WinForms.State',
        'Sasd.Ui.WinForms.Theming',
        'Sasd.Ui.WinForms.Windows'
    )
}

function Get-SasdR1ConsumerEntryPackageIds {
    <#
    .SYNOPSIS
    Returns the deliberately small R1 package set documented as consumer entry points.

    .DESCRIPTION
    Internal implementation packages remain real NuGet dependencies because the platform
    keeps modular assemblies. Consumers should normally start from one of these packages
    rather than assembling the internal module graph themselves.
    #>
    return @(
        'Sasd.Ui.Core',
        'Sasd.Ui.WinForms',
        'Sasd.Ui.WinForms.App',
        'Sasd.Ui.WinForms.Data',
        'Sasd.Ui.WinForms.Krypton'
    )
}
