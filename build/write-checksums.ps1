[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$InputDirectory,

    [string]$OutputFileName = 'SHA256SUMS.txt'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($OutputFileName)) {
    throw 'Output file name must not be empty.'
}

$resolvedInput = (Resolve-Path $InputDirectory).Path
if (-not (Test-Path $resolvedInput -PathType Container)) {
    throw "Input directory was not found: $InputDirectory"
}

$outputPath = Join-Path $resolvedInput $OutputFileName

# Release checksums cover the distributable NuGet and symbol-package artifacts only.
# Keeping the set explicit prevents an unrelated temporary/log file from silently
# changing the release manifest.
$artifacts = @(
    Get-ChildItem $resolvedInput -File |
        Where-Object { $_.Extension -in @('.nupkg', '.snupkg') } |
        Sort-Object -Property Name
)

if ($artifacts.Count -eq 0) {
    throw "No NuGet release artifacts were found in '$resolvedInput'."
}

$entries = foreach ($artifact in $artifacts) {
    $hash = (Get-FileHash -LiteralPath $artifact.FullName -Algorithm SHA256).Hash.ToLowerInvariant()

    [pscustomobject]@{
        FileName = $artifact.Name
        Hash = $hash
    }
}

# Use LF and UTF-8 without a BOM so the manifest is byte-stable independent of the
# developer's Windows text-file defaults. The traditional two-space separator remains
# compatible with common SHA256SUMS tooling.
$lines = @($entries | ForEach-Object { "$($_.Hash)  $($_.FileName)" })
$manifestText = ($lines -join "`n") + "`n"
[System.IO.File]::WriteAllText(
    $outputPath,
    $manifestText,
    [System.Text.UTF8Encoding]::new($false))

# Verify what we just wrote from disk instead of assuming the write succeeded. This also
# protects future refactoring from producing a manifest whose names/hashes do not match
# the package directory.
$manifestLines = @(
    [System.IO.File]::ReadAllLines($outputPath) |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
)

if ($manifestLines.Count -ne $entries.Count) {
    throw "Checksum manifest contains $($manifestLines.Count) entries; expected $($entries.Count)."
}

foreach ($line in $manifestLines) {
    if ($line -notmatch '^(?<hash>[0-9a-f]{64})  (?<file>.+)$') {
        throw "Checksum manifest contains an invalid line: $line"
    }

    $artifactPath = Join-Path $resolvedInput $Matches.file
    if (-not (Test-Path $artifactPath -PathType Leaf)) {
        throw "Checksum manifest references a missing artifact: $($Matches.file)"
    }

    $actualHash = (Get-FileHash -LiteralPath $artifactPath -Algorithm SHA256).Hash.ToLowerInvariant()
    if ($actualHash -ne $Matches.hash) {
        throw "Checksum verification failed for '$($Matches.file)'."
    }
}

Write-Host "SHA-256 checksum manifest created and verified for $($entries.Count) artifacts." -ForegroundColor Green
Write-Host "Manifest: $outputPath" -ForegroundColor DarkGray
