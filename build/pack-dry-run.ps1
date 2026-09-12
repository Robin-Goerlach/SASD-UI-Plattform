[CmdletBinding()]
param(
    [string]$Version = '0.0.0-local',
    [string]$OutputDirectory = 'artifacts/nuget-dry-run',
    [switch]$SkipRestore
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path

# Keep this list explicit. A new product project should not silently become a published
# package merely because somebody added a .csproj under src/. Updating this list is a
# small, reviewable acknowledgement of the existing package boundary; it does not create
# a new boundary by itself.
$productProjects = @(
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

function Invoke-DotNetStep {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    Write-Host ""
    Write-Host "==> $Name" -ForegroundColor Cyan
    Write-Host "dotnet $($Arguments -join ' ')" -ForegroundColor DarkGray

    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$Name failed with exit code $LASTEXITCODE."
    }
}

function Assert-PackageContents {
    param(
        [Parameter(Mandatory)]
        [string]$PackagePath,

        [Parameter(Mandatory)]
        [string]$PackageId
    )

    # NuGet packages are ZIP files. Inspecting the archive directly keeps this dry-run
    # dependency-free and proves that consumers receive the runtime assembly, XML API docs
    # and a package landing page rather than merely proving that `dotnet pack` returned zero.
    $archive = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        $entryNames = @($archive.Entries | ForEach-Object { $_.FullName })
        $assemblyName = "$PackageId.dll"
        $documentationName = "$PackageId.xml"
        $readmeName = 'README.md'

        $hasAssembly = $entryNames | Where-Object {
            $_ -like "lib/*/$assemblyName"
        }
        $hasDocumentation = $entryNames | Where-Object {
            $_ -like "lib/*/$documentationName"
        }
        $hasReadme = $entryNames -contains $readmeName

        if (-not $hasAssembly) {
            throw "Package '$PackageId' does not contain its product assembly."
        }

        if (-not $hasDocumentation) {
            throw "Package '$PackageId' does not contain XML documentation."
        }

        if (-not $hasReadme) {
            throw "Package '$PackageId' does not contain its root README.md."
        }
    }
    finally {
        $archive.Dispose()
    }
}

function Assert-SymbolPackageContents {
    param(
        [Parameter(Mandatory)]
        [string]$PackagePath,

        [Parameter(Mandatory)]
        [string]$PackageId
    )

    $archive = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        $pdbName = "$PackageId.pdb"
        $hasPortablePdb = @($archive.Entries | ForEach-Object { $_.FullName }) | Where-Object {
            $_ -like "lib/*/$pdbName"
        }

        if (-not $hasPortablePdb) {
            throw "Symbol package '$PackageId' does not contain a portable PDB."
        }
    }
    finally {
        $archive.Dispose()
    }
}

if ([string]::IsNullOrWhiteSpace($Version)) {
    throw 'Package version must not be empty.'
}

Push-Location $repositoryRoot
try {
    # Resolve the output below the repository by default, but also allow callers to use
    # an absolute temporary path. The script never publishes from this directory.
    $resolvedOutput = if ([System.IO.Path]::IsPathRooted($OutputDirectory)) {
        [System.IO.Path]::GetFullPath($OutputDirectory)
    }
    else {
        [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot $OutputDirectory))
    }

    if (Test-Path $resolvedOutput) {
        Remove-Item $resolvedOutput -Recurse -Force
    }
    New-Item -ItemType Directory -Path $resolvedOutput -Force | Out-Null

    if (-not $SkipRestore) {
        Invoke-DotNetStep -Name 'Restore solution for packaging' -Arguments @(
            'restore',
            'SASD.Ui.Platform.sln'
        )
    }

    foreach ($project in $productProjects) {
        if (-not (Test-Path $project -PathType Leaf)) {
            throw "Configured product project was not found: $project"
        }

        $projectReadme = Join-Path (Split-Path $project -Parent) 'README.md'
        if (-not (Test-Path $projectReadme -PathType Leaf)) {
            throw "Configured product project does not provide its package README: $projectReadme"
        }

        $packageId = [System.IO.Path]::GetFileNameWithoutExtension($project)
        Invoke-DotNetStep -Name "Pack $packageId" -Arguments @(
            'pack',
            $project,
            '--configuration', 'Release',
            '--output', $resolvedOutput,
            '--include-symbols',
            '--no-restore',
            "-p:PackageVersion=$Version",
            '-p:SymbolPackageFormat=snupkg',
            '-p:DebugType=portable',
            '-p:ContinuousIntegrationBuild=true',
            '-p:TreatWarningsAsErrors=true'
        )

        $packagePath = Join-Path $resolvedOutput "$packageId.$Version.nupkg"
        $symbolPackagePath = Join-Path $resolvedOutput "$packageId.$Version.snupkg"

        if (-not (Test-Path $packagePath -PathType Leaf)) {
            throw "Expected package was not produced: $packagePath"
        }
        if (-not (Test-Path $symbolPackagePath -PathType Leaf)) {
            throw "Expected symbol package was not produced: $symbolPackagePath"
        }

        Assert-PackageContents -PackagePath $packagePath -PackageId $packageId
        Assert-SymbolPackageContents -PackagePath $symbolPackagePath -PackageId $packageId
    }

    $packages = @(Get-ChildItem $resolvedOutput -Filter '*.nupkg' -File)
    $symbolPackages = @(Get-ChildItem $resolvedOutput -Filter '*.snupkg' -File)
    if ($packages.Count -ne $productProjects.Count -or $symbolPackages.Count -ne $productProjects.Count) {
        throw "Unexpected package count. Expected $($productProjects.Count) packages and symbol packages; found $($packages.Count) and $($symbolPackages.Count)."
    }

    Write-Host ""
    Write-Host "NuGet dry-run succeeded for $($productProjects.Count) product projects." -ForegroundColor Green
    Write-Host "Version: $Version" -ForegroundColor DarkGray
    Write-Host "Artifacts: $resolvedOutput" -ForegroundColor DarkGray
    Write-Host 'No package was published.' -ForegroundColor Green
}
finally {
    Pop-Location
}
