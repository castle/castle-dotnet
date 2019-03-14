Releasing
=========

1. Create branch `release X.Y.Z`.
2. Update package version for the `Castle.Net` project to the new version ("Version" in .csproj or Properties -> Package -> Package version in Visual Studio)
3. Update the `CHANGELOG.md` for the impending release
4. `git commit -am "release X.Y.Z."` (where X.Y.Z is the new version)
5. Push to Github, make PR, and when ok, merge.
6. Make a release on Github, specify tag as `vX.Y.Z` to create a tag.
8. `git checkout master && git pull`
9. Go to the `src`folder and run `dotnet pack Castle.Net -c Release`
10. Run `dotnet nuget push Castle.Net\bin\Release\Castle.Net.X.Y.Z.nupkg -k [nuget API key]`