Releasing
=========

1. Create branch `release X.Y.Z`.
2. Update package version for the `Castle.Sdk` project to the new version ("Version" in .csproj or Properties -> Package -> Package version in Visual Studio)
3. Update `CHANGELOG.md` for the impending release
4. `git commit -am "release X.Y.Z."` (where X.Y.Z is the new version)
5. Push to Github, make PR, and when ok, merge.
6. Make a release on Github, specify tag as `vX.Y.Z` to create a tag.
8. `git checkout master && git pull`
9. Go to the `src` folder and run `dotnet pack Castle.Sdk -c Release`
10. Run `dotnet nuget push Castle.Sdk\bin\Release\Castle.Sdk.X.Y.Z.nupkg -k [nuget API key] -s https://api.nuget.org/v3/index.json`