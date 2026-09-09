[CmdletBinding()]
param(
    [switch]$RunVerification,
    [switch]$CompileOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
$errors = [System.Collections.Generic.List[string]]::new()
$warnings = [System.Collections.Generic.List[string]]::new()

function Write-Check {
    param(
        [Parameter(Mandatory)]
        [string]$Label,

        [Parameter(Mandatory)]
        [string]$Value,

        [ValidateSet('Ok', 'Warn', 'Fail')]
        [string]$State = 'Ok'
    )

    $color = switch ($State) {
        'Ok' { 'Green' }
        'Warn' { 'Yellow' }
        'Fail' { 'Red' }
    }

    Write-Host ('{0,-28} {1}' -f $Label, $Value) -ForegroundColor $color
}

function Test-RequiredFile {
    param(
        [Parameter(Mandatory)]
        [string]$RelativePath
    )

    $fullPath = Join-Path $repositoryRoot $RelativePath
    if (Test-Path -LiteralPath $fullPath -PathType Leaf) {
        Write-Check -Label $RelativePath -Value 'present'
        return
    }

    $errors.Add("Required repository file is missing: $RelativePath")
    Write-Check -Label $RelativePath -Value 'missing' -State Fail
}

Write-Host ''
Write-Host 'SASD UI Platform - Codex preflight' -ForegroundColor Cyan
Write-Host "Repository: $repositoryRoot" -ForegroundColor DarkGray
Write-Host ''

Test-RequiredFile 'AGENTS.md'
Test-RequiredFile 'ROADMAP.md'
Test-RequiredFile 'build/verify.ps1'

$gitCommand = Get-Command git -ErrorAction SilentlyContinue
if ($null -eq $gitCommand) {
    $errors.Add('Git was not found on PATH.')
    Write-Check -Label 'Git' -Value 'not found' -State Fail
}
else {
    $gitVersion = (& git --version) -join ' '
    Write-Check -Label 'Git' -Value $gitVersion
}

$dotnetCommand = Get-Command dotnet -ErrorAction SilentlyContinue
if ($null -eq $dotnetCommand) {
    $errors.Add('.NET SDK was not found on PATH.')
    Write-Check -Label '.NET SDK' -Value 'not found' -State Fail
}
else {
    $sdkLines = @(& dotnet --list-sdks)
    $net8Sdk = $sdkLines | Where-Object { $_ -match '^8\.' } | Select-Object -Last 1
    if ($null -eq $net8Sdk) {
        $errors.Add('.NET 8 SDK was not found. The repository currently targets .NET 8.')
        Write-Check -Label '.NET 8 SDK' -Value 'not found' -State Fail
    }
    else {
        Write-Check -Label '.NET 8 SDK' -Value $net8Sdk
    }
}

Write-Check -Label 'PowerShell' -Value $PSVersionTable.PSVersion.ToString()

if ($IsWindows) {
    Write-Check -Label 'Operating system' -Value 'Windows - full WinForms verification available'
}
else {
    $warnings.Add('Non-Windows environment: WinForms runtime smoke checks cannot be executed locally.')
    Write-Check -Label 'Operating system' -Value 'non-Windows - compile/architecture verification only' -State Warn
}

if ($null -ne $gitCommand) {
    Push-Location $repositoryRoot
    try {
        $insideWorkTree = (& git rev-parse --is-inside-work-tree 2>$null) -eq 'true'
        if (-not $insideWorkTree) {
            $errors.Add('Repository directory is not a Git worktree.')
            Write-Check -Label 'Git worktree' -Value 'not detected' -State Fail
        }
        else {
            Write-Check -Label 'Git worktree' -Value 'detected'

            $branch = (& git branch --show-current).Trim()
            if ([string]::IsNullOrWhiteSpace($branch)) {
                Write-Check -Label 'Current branch' -Value 'detached HEAD' -State Warn
                $warnings.Add('Detached HEAD detected. This can be valid in managed tasks, but commits may need an explicit branch/ref.')
            }
            else {
                Write-Check -Label 'Current branch' -Value $branch
            }

            $status = @(& git status --porcelain)
            if ($status.Count -eq 0) {
                Write-Check -Label 'Working tree' -Value 'clean'
            }
            else {
                Write-Check -Label 'Working tree' -Value "$($status.Count) pending change(s)" -State Warn
                $warnings.Add('Working tree is not clean. Codex must preserve unrelated user work and avoid destructive reset/checkout operations.')
            }

            $origin = (& git remote get-url origin 2>$null).Trim()
            if ([string]::IsNullOrWhiteSpace($origin)) {
                Write-Check -Label 'Origin remote' -Value 'not configured' -State Warn
                $warnings.Add('No origin remote is configured. Local coding can continue, but push/PR workflow is unavailable until a remote is configured.')
            }
            else {
                Write-Check -Label 'Origin remote' -Value $origin
            }
        }
    }
    finally {
        Pop-Location
    }
}

Write-Host ''
if ($warnings.Count -gt 0) {
    Write-Host 'Warnings:' -ForegroundColor Yellow
    foreach ($warning in $warnings) {
        Write-Host "- $warning" -ForegroundColor Yellow
    }
    Write-Host ''
}

if ($errors.Count -gt 0) {
    Write-Host 'Preflight failed:' -ForegroundColor Red
    foreach ($item in $errors) {
        Write-Host "- $item" -ForegroundColor Red
    }

    exit 1
}

$verifyScript = Join-Path $repositoryRoot 'build/verify.ps1'
$useCompileOnly = $CompileOnly -or -not $IsWindows
$verificationCommand = if ($useCompileOnly) {
    'pwsh ./build/verify.ps1 -CompileOnly'
}
else {
    'pwsh ./build/verify.ps1'
}

Write-Check -Label 'Recommended verification' -Value $verificationCommand
Write-Host ''

if ($RunVerification) {
    Write-Host 'Running repository verification...' -ForegroundColor Cyan
    if ($useCompileOnly) {
        & $verifyScript -CompileOnly
    }
    else {
        & $verifyScript
    }

    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

Write-Host 'Codex preflight completed successfully.' -ForegroundColor Green
