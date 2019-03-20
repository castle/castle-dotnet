# Development

## Prerequisites
- IDE of your choice
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- Make sure `dotnet.exe` (from the SDK) is available in your PATH.

## Build
Build using your IDE, or with `dotnet build src/Castle.Sdk/Castle.Sdk.csproj`.

## Test
Use an xUnit-compatible test runner, or `dotnet test src/Tests/Tests.csproj`.

## Pack
The goal of the SDK project is to produce a NuGet package. Packages are created and pushed to a package feed (repository) with the following commands:
1. `dotnet pack src/Castle.Sdk/Castle.Sdk.csproj`:  
 Creates `Castle.Sdk.[version].nupkg` in src/Castle.Sdk/bin/Debug or Release, depending on the value of the optional `-c` switch ("Debug" is default). `version` is read from the .csproj file.
2. `dotnet nuget push [path to package] -s [package source]`:
Pushes the package to the designated source. If you're pushing to `nuget.org` you also need to supply a key using the `-k` switch.

### Development
When developing, it can be useful to create packages for local use in separate test applications. In that scenario you can use a local folder as NuGet feed, e.g. push using:  
`dotnet nuget push [path to package] -s c:\packages`.

#### Alternative 1

1. Pack the SDK. The package version will be the same as the current release.
2. Push the package to your local feed.
3. **[update only]** Clear the NuGet cache, so you know the project will get the latest build of this package version, with:  
`dotnet nuget locals all --clear`
4. **[update only]** In your dependent project: `dotnet remove package Castle.Sdk`, and...
5. `dotnet add package Castle.Sdk -s [path to local folder]`

#### Alternative 2
1. Pack the SDK, and specify a package version independent of the `version` in the .csproj file like so:   
`dotnet pack [path to project file] /p:Version=[a higher version than current]`
2. Push the package to your local feed.
3. In your dependent project, run `dotnet add package Castle.Sdk -s [path to local folder]`. This will add the dependency if it doesn't exist, or update it if it does and there's a higher version available. 


### Releasing
For details on packing in a release scenario, see the [Releasing](/RELEASING.md) doc.