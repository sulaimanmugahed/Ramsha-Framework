param (
    [string]$PackagesFolder = "./src-packages",
    [string]$Source = "https://api.nuget.org/v3/index.json",
    [string]$ApiKey = $null
)

if (-not $ApiKey) {
    if ($env:NUGET_API_KEY) {
        Write-Host "using API key from environment variable: NUGET_API_KEY"
        $ApiKey = $env:NUGET_API_KEY
    }
    else {
        $ApiKey = Read-Host "enter your NuGet API key"
    }
}

if (-not (Test-Path $PackagesFolder)) {
    Write-Error "Packages folder not found: $PackagesFolder"
    exit 1
}

Write-Host "pushing packages from: $PackagesFolder"
Write-Host "nuGet Source: $Source"


$packages = Get-ChildItem $PackagesFolder -Filter *.nupkg | 
            Where-Object { $_.Name -notlike "*.snupkg" }

if ($packages.Count -eq 0) {
    Write-Error "no .nupkg packages found in $PackagesFolder"
    exit 1
}

foreach ($pkg in $packages) {
    Write-Host "pushing $($pkg.Name)..."
    
    dotnet nuget push $pkg.FullName `
        --source $Source `
        --api-key $ApiKey `
        --skip-duplicate `
        --force-english-output

    if ($LASTEXITCODE -ne 0) {
        Write-Error "failed to push $($pkg.Name)"
        exit 1
    }
}

Write-Host "`nAll packages pushed successfully"
