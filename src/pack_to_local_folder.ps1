param (
    [Parameter(Mandatory=$true)][string]$packages
)

$packagePath = "$($packages)\castle.net"
Write-host $packagePathpo
if (Test-Path $packagePath) {
    Write-Host "Clearing existing package"
    Remove-Item -path $packagePath -recurse
}

dotnet pack Castle.Net
nuget add .\Castle.Net\bin\Debug\Castle.Net.1.0.0.nupkg -source $packages
dotnet nuget locals all --clear