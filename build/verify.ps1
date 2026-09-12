[CmdletBinding()]
param(
    [switch]$SkipRestore,
    [switch]$CompileOnly,
    [switch]$IncludePackDryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$referenceProjects = @(
    @{
        Name = 'CRUD reference application'
        Project = 'samples/Sasd.Ui.Sample.Crud/Sasd.Ui.Sample.Crud.csproj'
    },
    @{
        Name = 'Workbench reference application'
        Project = 'samples/Sasd.Ui.Sample.Workbench/Sasd.Ui.Sample.Workbench.csproj'
    },
    @{
        Name = 'Utility reference application'
        Project = 'samples/Sasd.Ui.Sample.Utility/Sasd.Ui.Sample.Utility.csproj'
    },
    @{
        Name = 'Integrated platform showcase'
        Project = 'examples/Sasd.Ui.PlatformShowcase/Sasd.Ui.PlatformShowcase.csproj'
    }
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

Push-Location $repositoryRoot
try {
    if (-not $SkipRestore) {
        Invoke-DotNetStep -Name 'Restore solution' -Arguments @(
            'restore',
            'SASD.Ui.Platform.sln'
        )

        # Keep the executable Visual Studio entry point itself under CI. This catches malformed
        # solution/project paths while the independent project restores below continue proving
        # that reference consumers do not depend on solution-only state or copied binaries.
        Invoke-DotNetStep -Name 'Restore executable samples solution' -Arguments @(
            'restore',
            'SASD.Ui.Samples.sln'
        )

        foreach ($reference in $referenceProjects) {
            Invoke-DotNetStep -Name "Restore $($reference.Name)" -Arguments @(
                'restore',
                $reference.Project
            )
        }
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

    Invoke-DotNetStep -Name 'Build executable samples solution' -Arguments @(
        'build',
        'SASD.Ui.Samples.sln',
        '--configuration', 'Release',
        '--no-restore',
        '-p:TreatWarningsAsErrors=true'
    )

    # Reference consumers are also built independently with the same analyzer policy. This
    # catches awkward public APIs and missing references that a solution-only build can mask.
    foreach ($reference in $referenceProjects) {
        Invoke-DotNetStep -Name "Build $($reference.Name)" -Arguments @(
            'build',
            $reference.Project,
            '--configuration', 'Release',
            '--no-restore',
            '-p:TreatWarningsAsErrors=true'
        )
    }

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

        # Packaging net8.0-windows projects from a non-Windows host is supported by
        # EnableWindowsTargeting, but this verification path intentionally remains a
        # compile-only contract. CI exercises packaging on the normal Windows runner.
        if ($IncludePackDryRun) {
            Write-Warning 'NuGet pack dry-run is skipped in CompileOnly/non-Windows verification. Run build/pack-dry-run.ps1 explicitly if packaging is required on this host.'
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

    Invoke-DotNetStep -Name 'Forms smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.FormsSmokeChecks/Sasd.Ui.FormsSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Composite feedback smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.CompositeFeedbackSmokeChecks/Sasd.Ui.CompositeFeedbackSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
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

    Invoke-DotNetStep -Name 'Keyboard acceptance smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.KeyboardSmokeChecks/Sasd.Ui.KeyboardSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Dialog threading smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.DialogSmokeChecks/Sasd.Ui.DialogSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    Invoke-DotNetStep -Name 'Lifecycle endurance smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.LifecycleSmokeChecks/Sasd.Ui.LifecycleSmokeChecks.csproj',
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

    Invoke-DotNetStep -Name 'Data interaction smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.DataInteractionSmokeChecks/Sasd.Ui.DataInteractionSmokeChecks.csproj',
        '--configuration', 'Release',
        '-p:TreatWarningsAsErrors=true'
    )

    # Build-only coverage did not catch page-constructor/layout failures because showcase pages
    # are created lazily during navigation. Exercise every registered demonstration page in a
    # normal off-screen WinForms host so consumer-level regressions fail the same shared gate.
    Invoke-DotNetStep -Name 'Integrated showcase navigation smoke checks' -Arguments @(
        'run',
        '--project', 'examples/Sasd.Ui.PlatformShowcase/Sasd.Ui.PlatformShowcase.csproj',
        '--configuration', 'Release',
        '--no-build',
        '--', '--smoke'
    )

    Invoke-DotNetStep -Name 'Krypton adapter smoke checks' -Arguments @(
        'run',
        '--project', 'tests/smoke/Sasd.Ui.KryptonSmokeChecks/Sasd.Ui.KryptonSmokeChecks.csproj',
        '--configuration', 'Release'
    )

    if ($IncludePackDryRun) {
        Write-Host ""
        Write-Host '==> NuGet packaging dry-run' -ForegroundColor Cyan
        # The solution was already restored at the start of this gate. Keeping restore
        # out of the packaging step makes the extra CI evidence deterministic without
        # making ordinary local verification pay the packaging cost unless requested.
        & (Join-Path $PSScriptRoot 'pack-dry-run.ps1') -SkipRestore
    }

    Write-Host ""
    Write-Host 'Full SASD UI Platform verification completed successfully.' -ForegroundColor Green
}
finally {
    Pop-Location
}
