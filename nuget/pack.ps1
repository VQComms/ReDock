$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\Src\ReDock\bin\Release\ReDock.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\nuget\ReDock.nuspec)
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\nuget\ReDock.compiled.nuspec

& nuget pack $root\nuget\ReDock.compiled.nuspec
