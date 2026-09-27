[CmdletBinding()]
param(
    [string]$OutputDirectory = 'artifacts/release-evidence'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
. (Join-Path $PSScriptRoot 'product-projects.ps1')
$productProjects = @(Get-SasdProductProjects)

function Invoke-DotNetPackageInventory {
    param(
        [Parameter(Mandatory)]
        [string]$Project
    )

    # Capture stdout and stderr separately. JSON mode must remain machine-readable even
    # when the SDK emits an informational message on stderr.
    $startInfo = [System.Diagnostics.ProcessStartInfo]::new()
    $startInfo.FileName = 'dotnet'
    $startInfo.UseShellExecute = $false
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true

    foreach ($argument in @('list', $Project, 'package', '--include-transitive', '--format', 'json')) {
        $startInfo.ArgumentList.Add($argument)
    }

    using namespace System.Diagnostics
    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $startInfo

    try {
        if (-not $process.Start()) {
            throw "Failed to start dotnet package inventory for '$Project'."
        }

        $standardOutput = $process.StandardOutput.ReadToEnd()
        $standardError = $process.StandardError.ReadToEnd()
        $process.WaitForExit()

        if ($process.ExitCode -ne 0) {
            throw "dotnet list package failed for '$Project' with exit code $($process.ExitCode): $standardError"
        }

        if (-not [string]::IsNullOrWhiteSpace($standardError)) {
            Write-Warning $standardError.Trim()
        }

        try {
            return $standardOutput | ConvertFrom-Json -Depth 32
        }
        catch {
            throw "dotnet list package returned invalid JSON for '$Project': $($_.Exception.Message)"
        }
    }
    finally {
        $process.Dispose()
    }
}

function Add-PackageRecords {
    param(
        [Parameter(Mandatory)]
        [System.Collections.Generic.List[object]]$Destination,

        [Parameter(Mandatory)]
        [string]$Project,

        [Parameter(Mandatory)]
        [string]$Framework,

        [Parameter(Mandatory)]
        [string]$Kind,

        [AllowNull()]
        [object[]]$Packages
    )

    foreach ($package in @($Packages)) {
        if ($null -eq $package) {
            continue
        }

        $Destination.Add([pscustomobject][ordered]@{
            project = $Project
            framework = $Framework
            kind = $Kind
            packageId = [string]$package.id
            requestedVersion = if ($null -ne $package.PSObject.Properties['requestedVersion']) {
                [string]$package.requestedVersion
            }
            else {
                $null
            }
            resolvedVersion = [string]$package.resolvedVersion
        })
    }
}

Push-Location $repositoryRoot
try {
    $resolvedOutput = if ([System.IO.Path]::IsPathRooted($OutputDirectory)) {
        [System.IO.Path]::GetFullPath($OutputDirectory)
    }
    else {
        [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot $OutputDirectory))
    }

    New-Item -ItemType Directory -Path $resolvedOutput -Force | Out-Null

    $records = [System.Collections.Generic.List[object]]::new()

    foreach ($project in $productProjects) {
        if (-not (Test-Path $project -PathType Leaf)) {
            throw "Configured product project was not found: $project"
        }

        $inventory = Invoke-DotNetPackageInventory -Project $project
        foreach ($projectResult in @($inventory.projects)) {
            foreach ($framework in @($projectResult.frameworks)) {
                $frameworkName = [string]$framework.framework
                Add-PackageRecords -Destination $records -Project $project -Framework $frameworkName -Kind 'top-level' -Packages @($framework.topLevelPackages)
                Add-PackageRecords -Destination $records -Project $project -Framework $frameworkName -Kind 'transitive' -Packages @($framework.transitivePackages)
            }
        }
    }

    $orderedRecords = @(
        $records |
            Sort-Object -Property project, framework, kind, packageId, resolvedVersion
    )

    # The central version file remains the reviewed entry point for direct dependencies.
    # A direct package that bypasses it is release-policy drift and should fail the gate.
    [xml]$centralPackages = Get-Content 'Directory.Packages.props' -Raw
    $centralPackageIds = @(
        $centralPackages.Project.ItemGroup.PackageVersion |
            ForEach-Object { [string]$_.Include } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
            Sort-Object -Unique
    )

    $directPackageIds = @(
        $orderedRecords |
            Where-Object { $_.kind -eq 'top-level' } |
            ForEach-Object { $_.packageId } |
            Sort-Object -Unique
    )

    $missingCentralVersions = @(
        $directPackageIds | Where-Object { $_ -notin $centralPackageIds }
    )
    if ($missingCentralVersions.Count -gt 0) {
        throw "Direct package(s) are not centrally versioned: $($missingCentralVersions -join ', ')"
    }

    $unusedCentralVersions = @(
        $centralPackageIds | Where-Object { $_ -notin $directPackageIds }
    )
    if ($unusedCentralVersions.Count -gt 0) {
        throw "Centrally pinned package(s) are not used by a product project: $($unusedCentralVersions -join ', ')"
    }

    # Every NuGet dependency visible in the product graph must at least be named in the
    # human-reviewed notice file. The script deliberately does not invent or infer license
    # terms; legal/license correctness remains a reviewed release responsibility.
    $noticeText = Get-Content 'THIRD-PARTY-NOTICES.md' -Raw
    $allPackageIds = @(
        $orderedRecords |
            ForEach-Object { $_.packageId } |
            Sort-Object -Unique
    )
    $missingNotices = @(
        $allPackageIds |
            Where-Object { $noticeText -notmatch [regex]::Escape($_) }
    )
    if ($missingNotices.Count -gt 0) {
        throw "NuGet package(s) are missing from THIRD-PARTY-NOTICES.md: $($missingNotices -join ', ')"
    }

    $document = [ordered]@{
        schemaVersion = 1
        evidenceKind = 'nuget-dependency-inventory'
        productProjects = $productProjects
        packages = $orderedRecords
    }

    $json = ($document | ConvertTo-Json -Depth 16) -replace "`r`n", "`n"
    $json += "`n"
    $outputPath = Join-Path $resolvedOutput 'nuget-dependencies.json'
    [System.IO.File]::WriteAllText(
        $outputPath,
        $json,
        [System.Text.UTF8Encoding]::new($false))

    Write-Host ""
    Write-Host "NuGet dependency evidence written for $($productProjects.Count) product projects." -ForegroundColor Green
    Write-Host "Unique package IDs: $($allPackageIds.Count)" -ForegroundColor DarkGray
    foreach ($packageId in $allPackageIds) {
        $versions = @(
            $orderedRecords |
                Where-Object { $_.packageId -eq $packageId } |
                ForEach-Object { $_.resolvedVersion } |
                Sort-Object -Unique
        )
        Write-Host "  $packageId $($versions -join ', ')" -ForegroundColor DarkGray
    }
    Write-Host "Evidence: $outputPath" -ForegroundColor DarkGray
    Write-Host 'This inventory is release evidence, not a standards-compliant SBOM.' -ForegroundColor Yellow
}
finally {
    Pop-Location
}
