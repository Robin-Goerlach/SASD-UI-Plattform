[CmdletBinding()]
param(
    [string]$OutputDirectory = 'artifacts/release-evidence'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
. (Join-Path $PSScriptRoot 'product-projects.ps1')
$productProjects = @(Get-SasdProductProjects)

function Invoke-DotNetVulnerabilityInventory {
    param(
        [Parameter(Mandatory)]
        [string]$Project
    )

    # Keep JSON on stdout clean and observe audit-source diagnostics on stderr. A security
    # check must fail closed when vulnerability data cannot be obtained; "no findings"
    # is meaningful only when the audit source itself was available.
    $startInfo = [System.Diagnostics.ProcessStartInfo]::new()
    $startInfo.FileName = 'dotnet'
    $startInfo.UseShellExecute = $false
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true

    foreach ($argument in @(
        'list',
        $Project,
        'package',
        '--vulnerable',
        '--include-transitive',
        '--format',
        'json',
        '--output-version',
        '1'
    )) {
        $startInfo.ArgumentList.Add($argument)
    }

    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $startInfo

    try {
        if (-not $process.Start()) {
            throw "Failed to start NuGet vulnerability audit for '$Project'."
        }

        $standardOutput = $process.StandardOutput.ReadToEnd()
        $standardError = $process.StandardError.ReadToEnd()
        $process.WaitForExit()

        if ($process.ExitCode -ne 0) {
            throw (
                "dotnet list package --vulnerable failed for '$Project' " +
                "with exit code $($process.ExitCode): $standardError")
        }

        # NuGet can report audit-source failures as warnings while the command itself still
        # succeeds. Treat those as an evidence failure so a network/source problem cannot
        # accidentally become a clean vulnerability report.
        if (
            $standardError -match '(?i)NU1900' -or
            $standardError -match '(?i)did not provide any vulnerability data'
        ) {
            throw "NuGet vulnerability data was unavailable for '$Project': $($standardError.Trim())"
        }

        if (-not [string]::IsNullOrWhiteSpace($standardError)) {
            Write-Warning $standardError.Trim()
        }

        try {
            return $standardOutput | ConvertFrom-Json -Depth 64
        }
        catch {
            throw "NuGet vulnerability audit returned invalid JSON for '$Project': $($_.Exception.Message)"
        }
    }
    finally {
        $process.Dispose()
    }
}

function Add-VulnerabilityFindings {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyCollection()]
        [System.Collections.Generic.List[object]]$Destination,

        [Parameter(Mandatory)]
        [string]$Project,

        [Parameter(Mandatory)]
        [string]$Framework,

        [Parameter(Mandatory)]
        [ValidateSet('top-level', 'transitive')]
        [string]$Kind,

        [AllowNull()]
        [object[]]$Packages
    )

    foreach ($package in @($Packages)) {
        if ($null -eq $package) {
            continue
        }

        $packageId = [string]$package.id
        $resolvedVersion = [string]$package.resolvedVersion
        $vulnerabilities = if ($null -ne $package.PSObject.Properties['vulnerabilities']) {
            @($package.vulnerabilities)
        }
        else {
            @()
        }

        if ($vulnerabilities.Count -eq 0) {
            # --vulnerable is a filtered command. If NuGet returns a package here without
            # advisory details, retain it as an unknown-severity finding rather than
            # silently treating a schema/CLI change as safe.
            $Destination.Add([pscustomobject][ordered]@{
                project = $Project
                framework = $Framework
                kind = $Kind
                packageId = $packageId
                resolvedVersion = $resolvedVersion
                severity = 'unknown'
                advisoryUrl = $null
            })
            continue
        }

        foreach ($vulnerability in $vulnerabilities) {
            $severity = if ($null -ne $vulnerability.PSObject.Properties['severity']) {
                [string]$vulnerability.severity
            }
            else {
                'unknown'
            }

            # Version-1 CLI output currently exposes advisoryUrl. Keep a guarded fallback
            # to "url" so the release check remains conservative if NuGet aligns the field
            # name with its VulnerabilityInfo API.
            $advisoryUrl = if ($null -ne $vulnerability.PSObject.Properties['advisoryUrl']) {
                [string]$vulnerability.advisoryUrl
            }
            elseif ($null -ne $vulnerability.PSObject.Properties['url']) {
                [string]$vulnerability.url
            }
            else {
                $null
            }

            $Destination.Add([pscustomobject][ordered]@{
                project = $Project
                framework = $Framework
                kind = $Kind
                packageId = $packageId
                resolvedVersion = $resolvedVersion
                severity = $severity
                advisoryUrl = $advisoryUrl
            })
        }
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

    $findings = [System.Collections.Generic.List[object]]::new()
    $auditedProjects = [System.Collections.Generic.List[object]]::new()

    foreach ($project in $productProjects) {
        if (-not (Test-Path $project -PathType Leaf)) {
            throw "Configured product project was not found: $project"
        }

        $inventory = Invoke-DotNetVulnerabilityInventory -Project $project
        $projectResults = if ($null -ne $inventory.PSObject.Properties['projects']) {
            @($inventory.projects)
        }
        else {
            @()
        }
        if ($projectResults.Count -eq 0) {
            throw "NuGet vulnerability audit returned no project result for '$project'."
        }

        foreach ($projectResult in $projectResults) {
            if ($null -eq $projectResult) {
                throw "NuGet vulnerability audit returned an empty project result for '$project'."
            }

            $frameworkResults = if ($null -ne $projectResult.PSObject.Properties['frameworks']) {
                @($projectResult.frameworks)
            }
            else {
                @()
            }
            if ($frameworkResults.Count -eq 0) {
                throw "NuGet vulnerability audit returned no framework result for '$project'."
            }

            foreach ($framework in $frameworkResults) {
                $frameworkName = [string]$framework.framework
                $topLevelPackages = if ($null -ne $framework.PSObject.Properties['topLevelPackages']) {
                    @($framework.topLevelPackages)
                }
                else {
                    @()
                }
                $transitivePackages = if ($null -ne $framework.PSObject.Properties['transitivePackages']) {
                    @($framework.transitivePackages)
                }
                else {
                    @()
                }

                Add-VulnerabilityFindings -Destination $findings -Project $project -Framework $frameworkName -Kind 'top-level' -Packages $topLevelPackages
                Add-VulnerabilityFindings -Destination $findings -Project $project -Framework $frameworkName -Kind 'transitive' -Packages $transitivePackages

                $auditedProjects.Add([pscustomobject][ordered]@{
                    project = $project
                    framework = $frameworkName
                })
            }
        }
    }

    $orderedProjects = @(
        $auditedProjects |
            Sort-Object -Property project, framework -Unique
    )
    $orderedFindings = @(
        $findings |
            Sort-Object -Property project, framework, kind, packageId, resolvedVersion, severity, advisoryUrl
    )

    $document = [ordered]@{
        schemaVersion = 1
        evidenceKind = 'nuget-vulnerability-audit'
        auditedAtUtc = [DateTimeOffset]::UtcNow.ToString('O')
        auditScope = 'direct-and-transitive-product-project-packages'
        projects = $orderedProjects
        findings = $orderedFindings
    }

    $json = ($document | ConvertTo-Json -Depth 32) -replace "`r`n", "`n"
    $json += "`n"
    $outputPath = Join-Path $resolvedOutput 'nuget-vulnerabilities.json'
    [System.IO.File]::WriteAllText(
        $outputPath,
        $json,
        [System.Text.UTF8Encoding]::new($false))

    Write-Host ""
    Write-Host "NuGet vulnerability audit completed for $($orderedProjects.Count) project/framework combinations." -ForegroundColor Green
    Write-Host "Evidence: $outputPath" -ForegroundColor DarkGray

    if ($orderedFindings.Count -gt 0) {
        Write-Host ""
        Write-Host "Known NuGet vulnerabilities found: $($orderedFindings.Count)" -ForegroundColor Red
        foreach ($finding in $orderedFindings) {
            $advisory = if ([string]::IsNullOrWhiteSpace($finding.advisoryUrl)) {
                'no advisory URL returned'
            }
            else {
                $finding.advisoryUrl
            }

            Write-Host (
                "  $($finding.packageId) $($finding.resolvedVersion) " +
                "[$($finding.severity)] ($($finding.kind)) - $advisory") -ForegroundColor Red
        }

        throw 'NuGet vulnerability audit found one or more known vulnerable package dependencies.'
    }

    Write-Host 'No known vulnerable NuGet package dependencies were reported.' -ForegroundColor Green
}
finally {
    Pop-Location
}
