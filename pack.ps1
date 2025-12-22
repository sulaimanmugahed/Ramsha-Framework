param (
    [string]$OutputDir = "./src-packages",
    [string]$ModulesPath = "./src"
)

$propsPath = "Directory.Build.props"

$propsContent = Get-Content $propsPath -Raw
if ($propsContent -match '<RamshaVersion>(.*?)</RamshaVersion>') {
    $currentVersion = $matches[1]
} else {
    Write-Error "Could not find RamshaVersion in $propsPath"
    exit 1
}

Write-Host "Current version is: $currentVersion"



Write-Host "`nChoose version update method:"
Write-Host "1- Keep current version ($currentVersion) and rebuild"
Write-Host "2- Auto-bump (major / minor / patch)"
Write-Host "3- Enter version manually"
$updateMethod = Read-Host "Select (1, 2, or 3)"

switch ($updateMethod) {
    "1" {
        $newVersion = $currentVersion
        Write-Host "Keeping version $currentVersion and rebuilding packages..."
    }
    "2" {
        $choice = Read-Host "What do you want to bump? (major / minor / patch)"
        $parts = $currentVersion -split '\.'

        [int]$major = $parts[0]
        [int]$minor = $parts[1]
        [int]$patch = $parts[2]

        switch ($choice.ToLower()) {
            "patch" { $patch++ }
            "minor" { $minor++; $patch = 0 }
            "major" { $major++; $minor = 0; $patch = 0 }
            default {
                Write-Error "Invalid option. Please enter: patch, minor, or major."
                exit 1
            }
        }
        $newVersion = "$major.$minor.$patch"
    }
    "3" {
        $newVersion = Read-Host "Enter the new version (e.g., 1.2.3)"
        if (-not ($newVersion -match '^\d+\.\d+\.\d+$')) {
            Write-Error "Invalid format. Use 'X.Y.Z' (e.g., 1.2.3)"
            exit 1
        }
    }

    default {
        Write-Error "Invalid choice. Select 1 (auto-bump), 2 (manual), or 3 (keep current)."
        exit 1
    }
}

if ($newVersion -ne $currentVersion) {
    Write-Host "Updating version to: $newVersion ..."
    $updatedContent = $propsContent -replace "<RamshaVersion>.*?</RamshaVersion>", "<RamshaVersion>$newVersion</RamshaVersion>"
    $updatedContent | Set-Content $propsPath
}

if (!(Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

Write-Host "Packing all modules to $OutputDir ..."
$projects = Get-ChildItem -Path $ModulesPath -Recurse -Filter *.csproj
foreach ($proj in $projects) {
    $projectName = $proj.BaseName
    $nupkgPattern = "$projectName.$newVersion*.nupkg"
    

    Get-ChildItem $OutputDir -Filter $nupkgPattern | Remove-Item -Force
    
    Write-Host " → $($proj.Name)"
    dotnet pack $proj.FullName -c Release -o $OutputDir
}

Write-Host "Done. Version: $newVersion Packages are in: $OutputDir"