. ".\shared.ps1"

$solutionAbsPath = (Join-Path $buildFolder "../")
Write-Host "Build solution : $solutionAbsPath"
Set-Location $solutionAbsPath

dotnet build -c Release
if (-Not $?) {
    Write-Host ("Build failed for the solution: " + $solutionPath) -ForegroundColor red
    exit $LASTEXITCODE
}
Set-Location $buildFolder


