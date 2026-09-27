[CmdletBinding()]
param(
    [string]$Version = '0.0.0-local',
    [string]$OutputDirectory = 'artifacts/nuget-dry-run',
    [switch]$SkipRestore
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
. (Join-Path $PSScriptRoot 'product-projects.ps1')
$productProjects = @(Get-SasdProductProjects)
$metapackageProjects = @(Get-SasdMetapackageProjects)
$appMetapackageDependencyIds = @(Get-SasdAppMetapackageDependencyIds)

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
        [string]$PackageId,

        [switch]$Metapackage,

        [string[]]$ExpectedDependencyIds = @()
    )

    # NuGet packages are ZIP files. Inspecting the archive directly keeps this dry-run
    # dependency-free and proves the actual consumer payload rather than merely proving
    # that `dotnet pack` returned zero.
    $archive = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        $entryNames = @($archive.Entries | ForEach-Object { $_.FullName })
        $readmeName = 'README.md'
        $hasReadme = $entryNames -contains $readmeName

        if (-not $hasReadme) {
            throw "Package '$PackageId' does not contain its root README.md."
        }

        if ($Metapackage) {
            # A metapackage must remain dependency-only. Accidentally shipping a generated
            # marker DLL would create a public assembly/API that serves no runtime purpose.
            $runtimeEntries = @($entryNames | Where-Object { $_ -like 'lib/*' -or $_ -like 'ref/*' })
            if ($runtimeEntries.Count -gt 0) {
                throw "Metapackage '$PackageId' unexpectedly contains runtime/reference assets: $($runtimeEntries -join ', ')"
            }

            $nuspecEntry = $archive.Entries |
                Where-Object { $_.FullName -like '*.nuspec' } |
                Select-Object -First 1
            if ($null -eq $nuspecEntry) {
                throw "Metapackage '$PackageId' does not contain a NuGet manifest."
            }

            $reader = [System.IO.StreamReader]::new($nuspecEntry.Open())
            try {
                [xml]$nuspec = $reader.ReadToEnd()
            }
            finally {
                $reader.Dispose()
            }

            $actualDependencyIds = @(
                $nuspec.SelectNodes("//*[local-name()='dependency']") |
                    ForEach-Object { [string]$_.id } |
                    Sort-Object -Unique
            )
            $expectedIds = @($ExpectedDependencyIds | Sort-Object -Unique)

            $missing = @($expectedIds | Where-Object { $_ -notin $actualDependencyIds })
            $unexpected = @($actualDependencyIds | Where-Object { $_ -notin $expectedIds })
            if ($missing.Count -gt 0 -or $unexpected.Count -gt 0) {
                throw (
                    "Metapackage '$PackageId' dependency contract changed. " +
                    "Missing: $($missing -join ', '); unexpected: $($unexpected -join ', ').")
            }

            return
        }

        $assemblyName = "$PackageId.dll"
        $documentationName = "$PackageId.xml"
        $hasAssembly = $entryNames | Where-Object {
            $_ -like "lib/*/$assemblyName"
        }
        $hasDocumentation = $entryNames | Where-Object {
            $_ -like "lib/*/$documentationName"
        }

        if (-not $hasAssembly) {
            throw "Package '$PackageId' does not contain its product assembly."
        }

        if (-not $hasDocumentation) {
            throw "Package '$PackageId' does not contain XML documentation."
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
        $isMetapackage = $project -in $metapackageProjects

        $packArguments = @(
            'pack',
            $project,
            '--configuration', 'Release',
            '--output', $resolvedOutput,
            '--no-restore',
            "-p:PackageVersion=$Version",
            '-p:SymbolPackageFormat=snupkg',
            '-p:DebugType=portable',
            '-p:ContinuousIntegrationBuild=true',
            '-p:EnablePackageValidation=true',
            '-p:TreatWarningsAsErrors=true'
        )
        if (-not $isMetapackage) {
            # A dependency-only metapackage has no PDB to publish. Concrete assembly
            # packages continue to prove portable symbol-package generation.
            $packArguments += '--include-symbols'
        }

        Invoke-DotNetStep -Name "Pack $packageId" -Arguments $packArguments

        $packagePath = Join-Path $resolvedOutput "$packageId.$Version.nupkg"
        $symbolPackagePath = Join-Path $resolvedOutput "$packageId.$Version.snupkg"

        if (-not (Test-Path $packagePath -PathType Leaf)) {
            throw "Expected package was not produced: $packagePath"
        }

        if ($isMetapackage) {
            if (Test-Path $symbolPackagePath -PathType Leaf) {
                throw "Metapackage '$packageId' unexpectedly produced a symbol package."
            }

            $expectedDependencyIds = if ($packageId -eq 'Sasd.Ui.WinForms.App') {
                $appMetapackageDependencyIds
            }
            else {
                @()
            }
            Assert-PackageContents -PackagePath $packagePath -PackageId $packageId -Metapackage -ExpectedDependencyIds $expectedDependencyIds
        }
        else {
            if (-not (Test-Path $symbolPackagePath -PathType Leaf)) {
                throw "Expected symbol package was not produced: $symbolPackagePath"
            }

            Assert-PackageContents -PackagePath $packagePath -PackageId $packageId
            Assert-SymbolPackageContents -PackagePath $symbolPackagePath -PackageId $packageId
        }
    }

    $packages = @(Get-ChildItem $resolvedOutput -Filter '*.nupkg' -File)
    $symbolPackages = @(Get-ChildItem $resolvedOutput -Filter '*.snupkg' -File)
    $expectedSymbolPackageCount = $productProjects.Count - $metapackageProjects.Count
    if ($packages.Count -ne $productProjects.Count -or $symbolPackages.Count -ne $expectedSymbolPackageCount) {
        throw (
            "Unexpected package count. Expected $($productProjects.Count) normal/metapackages " +
            "and $expectedSymbolPackageCount symbol packages; found $($packages.Count) and $($symbolPackages.Count).")
    }

    Write-Host ""
    Write-Host '==> SHA-256 release checksums' -ForegroundColor Cyan
    # Checksums are generated from the completed artifact set, not incrementally while
    # packages are still being written. The helper verifies the manifest immediately.
    & (Join-Path $PSScriptRoot 'write-checksums.ps1') -InputDirectory $resolvedOutput

    Write-Host ""
    Write-Host "NuGet dry-run succeeded for $($productProjects.Count) product projects." -ForegroundColor Green
    Write-Host "Version: $Version" -ForegroundColor DarkGray
    Write-Host "Artifacts: $resolvedOutput" -ForegroundColor DarkGray
    Write-Host 'No package was published.' -ForegroundColor Green
}
finally {
    Pop-Location
}
