[CmdletBinding()]
param(
    [string]$Version = '0.0.0-local',
    [string]$PackageDirectory = 'artifacts/nuget-dry-run'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
. (Join-Path $PSScriptRoot 'product-projects.ps1')

# Keep the consumer outside the repository so it cannot inherit Directory.Build.props,
# Directory.Packages.props or other SASD-only build policy. That makes this a real
# package-consumer test rather than another project in the monorepo.
$smokeRoot = Join-Path ([System.IO.Path]::GetTempPath()) "sasd-ui-r1-metapackage-consumer-$PID"

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

if ([string]::IsNullOrWhiteSpace($Version)) {
    throw 'Package version must not be empty.'
}

Push-Location $repositoryRoot
try {
    $resolvedPackages = if ([System.IO.Path]::IsPathRooted($PackageDirectory)) {
        [System.IO.Path]::GetFullPath($PackageDirectory)
    }
    else {
        [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot $PackageDirectory))
    }

    if (-not (Test-Path $resolvedPackages -PathType Container)) {
        throw "Package directory was not found: $resolvedPackages"
    }

    $appPackage = Join-Path $resolvedPackages "Sasd.Ui.WinForms.App.$Version.nupkg"
    if (-not (Test-Path $appPackage -PathType Leaf)) {
        throw "R1 application metapackage was not found: $appPackage"
    }

    if (Test-Path $smokeRoot) {
        Remove-Item $smokeRoot -Recurse -Force
    }
    New-Item -ItemType Directory -Path $smokeRoot -Force | Out-Null

    # Keep the smoke completely local. Clearing package sources proves that the R1
    # application package and all of its SASD dependencies are present in the dry-run
    # output rather than being satisfied accidentally from a developer/global feed.
    $escapedPackageSource = [System.Security.SecurityElement]::Escape($resolvedPackages)
    $nugetConfig = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="sasd-dry-run" value="$escapedPackageSource" />
  </packageSources>
</configuration>
"@
    [System.IO.File]::WriteAllText(
        (Join-Path $smokeRoot 'NuGet.Config'),
        $nugetConfig,
        [System.Text.UTF8Encoding]::new($false))

    $project = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <EnableWindowsTargeting>true</EnableWindowsTargeting>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Sasd.Ui.WinForms.App" Version="[$Version]" />
  </ItemGroup>
</Project>
"@
    $projectPath = Join-Path $smokeRoot 'R1MetapackageConsumer.csproj'
    [System.IO.File]::WriteAllText(
        $projectPath,
        $project,
        [System.Text.UTF8Encoding]::new($false))

    # Compile-time references intentionally touch one public type from every direct module
    # behind Sasd.Ui.WinForms.App. This catches a metapackage that restores successfully
    # but fails to expose the assemblies an ordinary application expects.
    $program = @'
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.State;
using Sasd.Ui.WinForms.Theming;
using Sasd.Ui.WinForms.Windows;

Type[] requiredR1Types =
[
    typeof(SasdForm),
    typeof(SasdCommand),
    typeof(SasdBusyOverlay),
    typeof(SasdFieldLayout),
    typeof(SasdBreadcrumb),
    typeof(SasdStateStoreOptions),
    typeof(SasdThemeService),
    typeof(SasdClipboardService),
];

Console.WriteLine($"Resolved {requiredR1Types.Length} representative SASD R1 types.");
'@
    [System.IO.File]::WriteAllText(
        (Join-Path $smokeRoot 'Program.cs'),
        $program,
        [System.Text.UTF8Encoding]::new($false))

    Invoke-DotNetStep -Name 'Restore R1 metapackage consumer from local dry-run feed' -Arguments @(
        'restore',
        $projectPath,
        '--configfile', (Join-Path $smokeRoot 'NuGet.Config')
    )

    $assetsPath = Join-Path $smokeRoot 'obj/project.assets.json'
    if (-not (Test-Path $assetsPath -PathType Leaf)) {
        throw "Consumer restore did not produce project.assets.json: $assetsPath"
    }

    $assets = Get-Content $assetsPath -Raw | ConvertFrom-Json -Depth 64
    $resolvedPackageIds = @(
        $assets.libraries.PSObject.Properties.Name |
            ForEach-Object { ($_ -split '/', 2)[0] } |
            Sort-Object -Unique
    )

    $expectedIds = @(
        'Sasd.Ui.Core',
        'Sasd.Ui.WinForms',
        'Sasd.Ui.WinForms.App'
    ) + @(Get-SasdAppMetapackageDependencyIds)

    $missingIds = @($expectedIds | Where-Object { $_ -notin $resolvedPackageIds })
    if ($missingIds.Count -gt 0) {
        throw "R1 metapackage consumer is missing expected package(s): $($missingIds -join ', ')"
    }

    # These packages are intentional opt-ins. Their accidental appearance would make the
    # convenience package heavier and silently change the architecture we just approved.
    $forbiddenIds = @(
        'Sasd.Ui.WinForms.Data',
        'Sasd.Ui.WinForms.Krypton',
        'Sasd.Ui.WinForms.Media'
    )
    $unexpectedIds = @($forbiddenIds | Where-Object { $_ -in $resolvedPackageIds })
    if ($unexpectedIds.Count -gt 0) {
        throw "R1 metapackage unexpectedly pulled opt-in package(s): $($unexpectedIds -join ', ')"
    }

    Invoke-DotNetStep -Name 'Build R1 metapackage consumer' -Arguments @(
        'build',
        $projectPath,
        '--configuration', 'Release',
        '--no-restore',
        '-p:TreatWarningsAsErrors=true'
    )

    Write-Host ""
    Write-Host 'R1 metapackage consumer smoke succeeded.' -ForegroundColor Green
    Write-Host "Resolved SASD packages: $($resolvedPackageIds -join ', ')" -ForegroundColor DarkGray
}
finally {
    Pop-Location

    # The smoke is evidence, not a release artifact. Remove its generated project,
    # restore graph and binaries so developer/CI machines do not accumulate temp state.
    if (Test-Path $smokeRoot) {
        Remove-Item $smokeRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
}
