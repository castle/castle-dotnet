Releasing
=========

1. Create branch `release-X.Y.Z` from `develop`.
2. Update the package version for the `Castle.Sdk` project (`Version` in `src/Castle.Sdk/Castle.Sdk.csproj`).
3. Update `CHANGELOG.md` for the impending release.
4. `git commit -am "release X.Y.Z."` (where X.Y.Z is the new version).
5. Push to GitHub, open a PR to `develop`, and when approved, merge.
6. Merge `develop` into `master` and push.
7. Create a GitHub release from `master`, specifying the tag `vX.Y.Z`. Copy the changelog entry into the release description.
8. Build the package on Windows (required for the `net48` target):

   `dotnet pack -c Release src/Castle.Sdk/Castle.Sdk.csproj`

   The Windows GitHub Actions build also produces the package as a downloadable `nupkg` artifact.
9. Push the package to NuGet:

   `dotnet nuget push src/Castle.Sdk/bin/Release/Castle.Sdk.X.Y.Z.nupkg -k [nuget API key] -s https://api.nuget.org/v3/index.json`
