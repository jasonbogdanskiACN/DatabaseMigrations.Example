param(
    [string]$ConnectionString,
    [string]$Version
)

if (-not (Get-Command grate -ErrorAction SilentlyContinue)) {
    Write-Host "Grate CLI not found. Installing local tool manifest and Grate..."
    if (-not (Test-Path .\.config\dotnet-tools.json)) {
        dotnet new tool-manifest
    }
    dotnet tool install grate
}

if (-not $ConnectionString) {
    $ConnectionString = "Server=localhost,1433;Database=grateExample;User Id=sa;Password=YourStrong!Passw0rd123;TrustServerCertificate=True;"
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Push-Location $scriptDir

# Run grate CLI to apply migrations from ./migrations
$args = @("--connectionstring", "$ConnectionString", "--sqlfilesdirectory", ".", "--noninteractive")
if ($Version) {
    $args += "--version", "$Version"
}
dotnet grate @args

Pop-Location
