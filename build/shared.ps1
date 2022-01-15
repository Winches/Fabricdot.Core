$rootFolder = (Get-Item -Path "../" -Verbose).FullName
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$packageFolder = (Join-Path $buildFolder "/packages")