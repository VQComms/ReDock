$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\Src\ReDock\bin\Release\ReDock.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\ReDock.nuspec)
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\ReDock.compiled.nuspec

& $root\NuGet\NuGet.exe pack $root\ReDock.compiled.nuspec
