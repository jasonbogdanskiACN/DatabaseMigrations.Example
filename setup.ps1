<#
.SYNOPSIS
Bootstrap repository developer environment: restores .NET tools, restores NuGet packages and builds projects.

.DESCRIPTION
This script performs common setup tasks for the repository on Windows PowerShell:
 - Verifies `dotnet` is installed
 - Restores local tools from `.config/dotnet-tools.json` (dotnet tool restore)
 - Verifies `dotnet-ef` and `grate` are available via the local tools
 - Restores NuGet packages (dotnet restore)
 - Builds all projects found under the repository

.Notes
Run this from the repository root in an elevated shell if required by your environment.
#>

param()

function ExitWithError($message, $code = 1) {
    Write-Error $message
    exit $code
}

Write-Host "Running repository setup..." -ForegroundColor Cyan

# Ensure dotnet is available
try {
    $dotnetInfo = & dotnet --info 2>&1
} catch {
    ExitWithError "The .NET SDK (dotnet) was not found in PATH. Install .NET SDK 10 or later and re-run this script." 2
}

Write-Host "dotnet is available:" -ForegroundColor Green
Write-Host $dotnetInfo

# Restore local tools if a manifest exists
$toolManifest = Join-Path -Path $PSScriptRoot -ChildPath ".config\dotnet-tools.json"
if (Test-Path $toolManifest) {
    Write-Host "Restoring local dotnet tools from .config/dotnet-tools.json..." -ForegroundColor Cyan
    & dotnet tool restore
    if ($LASTEXITCODE -ne 0) {
        ExitWithError "dotnet tool restore failed." 3
    }
} else {
    Write-Host ".config/dotnet-tools.json not found. Skipping dotnet tool restore." -ForegroundColor Yellow
}

function Ensure-ToolManifestExists {
    if (-not (Test-Path (Join-Path $PSScriptRoot ".config\dotnet-tools.json"))) {
        Write-Host "Creating tool manifest (.config/dotnet-tools.json)..." -ForegroundColor Cyan
        & dotnet new tool-manifest -q
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Failed to create tool manifest, but continuing." -ForegroundColor Yellow
        }
    }
}

# Verify dotnet-ef
Write-Host "Verifying dotnet-ef is available via local tools..." -ForegroundColor Cyan
try {
    & dotnet tool run dotnet-ef --version
    if ($LASTEXITCODE -ne 0) {
        Write-Host "dotnet-ef not available as local tool. Attempting to install as local tool..." -ForegroundColor Yellow
        Ensure-ToolManifestExists
        & dotnet tool install dotnet-ef --version 10.0.2
        if ($LASTEXITCODE -ne 0) {
            ExitWithError "Failed to install dotnet-ef local tool." 4
        }
    }
} catch {
    Write-Host "Attempting to install dotnet-ef as local tool..." -ForegroundColor Yellow
    Ensure-ToolManifestExists
    & dotnet tool install dotnet-ef --version 10.0.2
    if ($LASTEXITCODE -ne 0) {
        ExitWithError "Failed to install dotnet-ef local tool." 4
    }
}

# Verify grate
Write-Host "Verifying grate CLI is available via local tools..." -ForegroundColor Cyan
try {
    & dotnet tool run grate --version
    if ($LASTEXITCODE -ne 0) {
        Write-Host "grate not available as local tool. Attempting to install as local tool..." -ForegroundColor Yellow
        Ensure-ToolManifestExists
        & dotnet tool install grate
        if ($LASTEXITCODE -ne 0) {
            ExitWithError "Failed to install grate local tool." 5
        }
    }
} catch {
    Write-Host "Attempting to install grate as local tool..." -ForegroundColor Yellow
    Ensure-ToolManifestExists
    & dotnet tool install grate
    if ($LASTEXITCODE -ne 0) {
        ExitWithError "Failed to install grate local tool." 5
    }
}

# Restore NuGet packages for repository
Write-Host "Restoring NuGet packages for the repo..." -ForegroundColor Cyan
& dotnet restore
if ($LASTEXITCODE -ne 0) {
    ExitWithError "dotnet restore failed." 6
}

# Build all projects found under the repository
Write-Host "Building all projects found under the repository..." -ForegroundColor Cyan
$csprojFiles = Get-ChildItem -Path $PSScriptRoot -Recurse -Filter *.csproj | Select-Object -ExpandProperty FullName
if (-not $csprojFiles) {
    Write-Host "No .csproj files found." -ForegroundColor Yellow
} else {
    foreach ($proj in $csprojFiles) {
        Write-Host "Building $proj" -ForegroundColor Gray
        & dotnet build "$proj" --nologo
        if ($LASTEXITCODE -ne 0) {
            ExitWithError "dotnet build failed for $proj" 7
        }
    }
}

Write-Host "Setup complete. You can now run migrations with dotnet ef or use the grate scripts in grate.Example." -ForegroundColor Green
exit 0
