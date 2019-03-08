param (
    [Parameter(Mandatory=$true)][string]$packages
)

$packagePath = Join-Path $packages "castle.net"

if (Test-Path $packagePath) {
    Write-Host "Clearing existing package"
    Remove-Item -path $packagePath -recurse
}

dotnet pack (Join-Path $PSScriptRoot "Castle.Net")
nuget add (Join-Path $PSScriptRoot Castle.Net\bin\Debug\Castle.Net.1.0.0.nupkg) -source $packages
dotnet nuget locals all --clear
