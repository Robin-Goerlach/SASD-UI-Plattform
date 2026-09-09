[CmdletBinding()]
param(
    [switch]$SkipRestore,
    [switch]$CompileOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$crudSampleProject = 'samples/Sasd.Ui.Sample.Crud/Sasd.Ui.Sample.Crud.csproj'

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

Push-Location $repositoryRoot
try {
    if (-not $SkipRestore) {
        Invoke-DotNetStep -Name 'Restore solution' -Arguments @(
            'restore',
            'SASD.Ui.Platform.sln'
        )

        # Reference applications are intentionally kept outside the hand-maintained
        # classic solution while they are still small pilots. Restore them explicitly
        # so verification proves that each sample is independently consumable.
        Invoke-DotNetStep -Name 'Restore CRUD reference application' -Arguments @(
            'restore',
            $crudSampleProject
        )
    }

    # The solution build is intentionally strict. The repository treats analyzer
    # warnings as defects so agents and developers do not silently accumulate a
    # backlog of warnings that later obscures real problems.
    Invoke-DotNetStep -Name 'Strict Release build' -Arguments @(
        'build',
        'SASD.Ui.Platform.sln',
        '--configuration', 'Release',
        '--no-restore',
        '-p:TreatWarningsAsErrors=true'
    )

    # Build the first real consumer/reference application with the same analyzer
    # policy. This catches awkward public APIs and missing package/project references
    # that component-level tests alone cannot reveal.
    Invoke-DotNetStep -Name 'Build CRUD reference application' -Arguments @(
        'build',
        $crudSampleProject,
        '--configuration', 'Release',
        '--no-restore',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Architecture checks' -Arguments @(
        'run',
        '--project', 'tests/architecture/Sasd.Ui.ArchitectureChecks/Sasd.Ui.ArchitectureChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true',
        '--', '.'
    )

    Invoke-DotNetStep -Name 'Core smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.SmokeChecks/Sasd.Ui.SmokeChecks.csproj',
        '--configuration', 'Release',
        '--no-build'
    )

    # WinForms runtime checks require Windows. A Linux/macOS Codex environment can
    # still prove restore/build/architecture/core correctness, but it must not claim
    # that Windows UI behavior was executed.
    if ($CompileOnly -or -not $IsWindows) {
        if (-not $IsWindows -and -not $CompileOnly) {
            Write-Warning 'Non-Windows environment detected. WinForms runtime smoke checks are skipped.'
        }

        Write-Host ""
        Write-Host 'Compile/architecture verification completed successfully.' -ForegroundColor Green
        return
    }

    Invoke-DotNetStep -Name 'UI-state smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.StateSmokeChecks/Sasd.Ui.StateSmokeChecks.csproj',
        '--configuration', 'Release'
    )

    Invoke-DotNetStep -Name 'WinForms foundation smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.WinFormsSmokeChecks/Sasd.Ui.WinFormsSmokeChecks.csproj',
        '--configuration', 'Release'
    )

    Invoke-DotNetStep -Name 'Windows integration smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.WindowsSmokeChecks/Sasd.Ui.WindowsSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Shell integration smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.ShellSmokeChecks/Sasd.Ui.ShellSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Native R2 smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.NativeR2SmokeChecks/Sasd.Ui.NativeR2SmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Data/dashboard smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.DataDashboardSmokeChecks/Sasd.Ui.DataDashboardSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Krypton adapter smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.KryptonSmokeChecks/Sasd.Ui.KryptonSmokeChecks.csproj',
        '--configuration', 'Release'
    )

    Write-Host ""
    Write-Host 'Full SASD UI Platform verification completed successfully.' -ForegroundColor Green
}
finally {
    Pop-Location
}
