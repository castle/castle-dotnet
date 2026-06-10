# Development

## Prerequisites

1. The [.NET SDK](https://dotnet.microsoft.com/download) (8.0 and 10.0 are used by CI).
2. An editor such as Visual Studio, Visual Studio Code (with the C# Dev Kit), or Rider.
3. To build or test the `net48` target you need Windows (it references `System.Web`). The modern targets (`net10.0`, `net8.0`, `netstandard2.0`) build and test on any platform.

## Build

Build the whole project with:

`dotnet build src/Castle.Sdk/Castle.Sdk.csproj`

On non-Windows machines, build a single modern target framework with the `-f` switch:

`dotnet build src/Castle.Sdk/Castle.Sdk.csproj -f net8.0`

## Test

Run the full test suite (all target frameworks, including `net48`) on Windows:

`dotnet test src/Tests/Tests.csproj`

On non-Windows machines, run a single modern target framework:

`dotnet test src/Tests/Tests.csproj -f net8.0`

CI runs the modern frameworks on Linux and the full matrix (including `net48`) on Windows.

## Formatting

Code style is enforced with `dotnet format` against the `.editorconfig`:

`dotnet format src/Castle.Sdk.sln`

Run with `--verify-no-changes` to check without writing (this is what CI runs).

## Pack

The SDK is published as a NuGet package. Because it targets `net48`, packing must be done on Windows:

`dotnet pack -c Release src/Castle.Sdk/Castle.Sdk.csproj`

See [RELEASING.md](RELEASING.md) for the full release flow.
