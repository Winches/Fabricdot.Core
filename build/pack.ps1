. ".\shared.ps1"

if(Test-Path $packageFolder){
    Remove-Item $packageFolder -Force -Recurse
}

$solutionAbsPath = $rootFolder 
Write-Host "Pack solution : $solutionAbsPath"
Set-Location $solutionAbsPath

dotnet clean
dotnet pack -c Release -o $packageFolder
Write-Host "Output of packages : $packageFolder"

if (-Not $?) {
    Write-Host ("Pack failed for the solution: " + $solutionPath)
    exit $LASTEXITCODE
}
Set-Location $buildFolder


