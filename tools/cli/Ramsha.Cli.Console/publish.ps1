
Write-Host "Publishing ..." -ForegroundColor Green

dotnet publish -c Release -r win-x64     --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64
dotnet publish -c Release -r linux-x64   --self-contained true -p:PublishSingleFile=true -o ./publish/linux-x64
dotnet publish -c Release -r osx-x64     --self-contained true -p:PublishSingleFile=true -o ./publish/osx-x64

Write-Host "Done! Executables are in ./publish/" -ForegroundColor Cyan