param (
    [Parameter(Mandatory=$true)][string]$packages,
    [Parameter(Mandatory=$true)][string]$version
)

$packagePath = Join-Path $packages "Castle.Sdk"

dotnet pack (Join-Path $PSScriptRoot "Castle.Sdk")
dotnet nuget push (Join-Path $PSScriptRoot "Castle.Sdk\bin\Debug\Castle.Sdk.$($version).nupkg") -s $packages
dotnet nuget locals all --clear
