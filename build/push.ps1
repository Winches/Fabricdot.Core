. ".\shared.ps1"

$apiKey = $args[0]
if(-not($apiKey)){
    Write-Host "Api key is null or empty" -ForegroundColor red
    Exit
}
$nugetSource = "https://api.nuget.org/v3/index.json"
Write-Host "Use api key : $apiKey"

[xml]$propsXml = Get-Content (Join-Path $rootFolder "./Directory.Build.props")
$version = $propsXml.Project.PropertyGroup.Version
Write-Host "Version : $version" -ForegroundColor green

$packages = Get-ChildItem -Path $packageFolder -Filter "*$version.nupkg"
Set-Location $packageFolder
foreach($package in $packages){
    $packageName  = Split-Path $package -leaf
    Write-Host "---------- Pushing package : $packageName ----------"
    dotnet nuget push $packageName -s $nugetSource -k $apiKey --skip-duplicate
}
Set-Location $buildFolder

