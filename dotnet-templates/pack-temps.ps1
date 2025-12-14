Set-StrictMode -Version Latest
$templatePath =     "./container/mtr"
$contentDirectory = "./container/mtr/content"
$nugetPath = "./nuget.exe"
$nugetOut =  "./templates-packages"


Write-Output "Copy CleanWebApiTemplate"
Copy-Item -Path "./CleanWebApiTemplate" -Recurse -Destination "$contentDirectory/CleanWebApiTemplate" -Container -Force

Write-Output "Copy SimpleWebApiTemplate"
Copy-Item -Path "./SimpleWebApiTemplate" -Recurse -Destination "$contentDirectory/SimpleWebApiTemplate" -Container -Force


Write-Output "Pack nuget"
$cmdArgList = @( "pack", "$templatePath/Ramsha.Templates.nuspec",
				 "-OutputDirectory", "$nugetOut")
& $nugetPath $cmdArgList 