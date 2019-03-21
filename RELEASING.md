Releasing
=========

1. Create branch `release-X.Y.Z`.
2. Update package version for the `Castle.Sdk` project to the new version ("Version" in .csproj or Properties -> Package -> Package version in Visual Studio)
3. Update `CHANGELOG.md` for the impending release
4. `git commit -am "release X.Y.Z."` (where X.Y.Z is the new version)
5. Push to Github, make PR, and when ok, merge.
6. Make a release on Github, specify tag as `vX.Y.Z` to create a tag. Copy the Changelog entry to the release description.
7. Find the Castle.Sdk NuGet package in AppVeyor under the **Artifacts** tab for the new master merge build, download it and go to the download folder.
8. Run `dotnet nuget push Castle.Sdk.X.Y.Z.nupkg -k [nuget API key] -s https://api.nuget.org/v3/index.json`