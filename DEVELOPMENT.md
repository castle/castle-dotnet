# Development

## Prerequisites

1. [.NET Core SDK](https://dotnet.microsoft.com/download), as a Microsoft installation. It is available as a **Homebrew** cask, but that installation option doesn't work (out of the box, at least) with Visual Studio Code, probably due to PATH issues.
2. Visual Studio Code.
3. The VS Code **C#** extension (the one from Microsoft). Make sure all its dependencies (i.e. OmniSharp) download and initialize successfully.

## Build
Build using your IDE, or with the `dotnet` command. Without .NET Framework, you won't be able to build for all target frameworks, so for `Castle.Sdk` you must specifically build for .NET Standard using the `-f` switch

`dotnet build src/Castle.Sdk/Castle.Sdk.csproj -f netstandard2.0`  

## Test
### Everything
Use an xUnit-compatible test runner (e.g. the VS Code extension **.NET Core Test Explorer**), or

`dotnet test src/Tests/Tests.csproj`

### Individual tests
There should be options above each test in Visual Studio Code: `Run Test` and `Debug Test`. If there aren't, and you just installed VS Code and/or the dotnet SDK, try reopening the project folder.

## Pack
The goal of the SDK project is to produce a NuGet package. With a .NET Standard library, like `Castle.Sdk`, packing is done with the command

`dotnet pack [path to project]`

However, since the library targets both .NET and Framework, `pack` requires .NET Framework to be installed, which means it only works on Windows machines. So, we do the packing in the [AppVeyor](https://www.appveyor.com/) build process. The finished package can be downloaded from the **Artifacts** tab for a build. 

Packages are pushed to a package feed (repository) like this

`dotnet nuget push [path to package] -s [package source]`  

If you're pushing to nuget.org you also need to supply a key using the `-k` switch.

### Development
When developing, it can be useful to add development packages (i.e. not available on nuget.org) for use in test applications. In that scenario you can use a local folder as NuGet feed

1. Download the package from AppVeyor
2. Push to a local folder, e.g. 
 
`dotnet nuget push [path to package] -s ~/Code/packages`

3. **[update only]** Clear the NuGet cache, so you know your project will get the latest build of this package version

`dotnet nuget locals all --clear`

4. **[update only]** Remove the package from your project

 `dotnet remove package Castle.Sdk`

5. Add the new package build

`dotnet add package Castle.Sdk -s [path to local folder]`

### Releasing
For details on packing in a release scenario, see the [Releasing](/RELEASING.md) doc.
