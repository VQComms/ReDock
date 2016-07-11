Function Install-Dotnet()
{
    # Prepare the dotnet CLI folder
    $env:DOTNET_INSTALL_DIR="$(Convert-Path "$PSScriptRoot")\.dotnet\win7-x64"
    if (!(Test-Path $env:DOTNET_INSTALL_DIR))
    {
      mkdir $env:DOTNET_INSTALL_DIR | Out-Null
    }

	# Download the dotnet CLI install script
    if (!(Test-Path .\dotnet\install.ps1))
    {
      Write-Output "Downloading version 1.0.0-preview2 of Dotnet CLI installer..."
      Invoke-WebRequest "https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview2/scripts/obtain/dotnet-install.ps1" -OutFile ".\.dotnet\dotnet-install.ps1"
    }

    # Run the dotnet CLI install
    Write-Output "Installing Dotnet CLI version 1.0.0-preview2-003121..."
    & .\.dotnet\dotnet-install.ps1 -Channel "preview" -Version "1.0.0-preview2-003121" -InstallDir "$env:DOTNET_INSTALL_DIR"

    # Add the dotnet folder path to the process. This gets skipped
    # by Install-DotNetCli if it's already installed.
    Remove-PathVariable $env:DOTNET_INSTALL_DIR
    $env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"

    dotnet --info
}

Function Remove-PathVariable([string]$VariableToRemove)
{
  $path = [Environment]::GetEnvironmentVariable("PATH", "User")
  $newItems = $path.Split(';') | Where-Object { $_.ToString() -inotlike $VariableToRemove }
  [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "User")
  $path = [Environment]::GetEnvironmentVariable("PATH", "Process")
  $newItems = $path.Split(';') | Where-Object { $_.ToString() -inotlike $VariableToRemove }
  [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "Process")
}

Function Update-ProjectJson()
{
    Write-Output ("Setting project.json version to " + $env:APPVEYOR_BUILD_VERSION)
    $a = Get-Content '.\src\ReDock\project.json' -raw | ConvertFrom-Json
    $a.version = $env:APPVEYOR_BUILD_VERSION
    $a | ConvertTo-Json -Depth 10000 | set-content '.\src\ReDock\project.json' 
    $a = Get-Content '.\src\ReDock\project.json' -raw
    Write-Output $a
}

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition;


# Install Dotnet CLI.
Install-Dotnet
Update-ProjectJson