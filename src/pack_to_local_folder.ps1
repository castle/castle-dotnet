param (
    [Parameter(Mandatory=$true)][string]$packages,
    [Parameter(Mandatory=$true)][string]$version
)

$packagePath = Join-Path $packages "Castle.Sdk"

if (Test-Path $packagePath) {
    Write-Host "Clearing existing package"
    Remove-Item -path $packagePath -recurse
}

dotnet pack (Join-Path $PSScriptRoot "Castle.Sdk")
nuget add (Join-Path $PSScriptRoot "Castle.Sdk\bin\Debug\Castle.Sdk.$($version).nupkg") -source $packages
dotnet nuget locals all --clear
