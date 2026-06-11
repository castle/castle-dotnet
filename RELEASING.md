Releasing
=========

1. Create branch `release-X.Y.Z` from `develop`.
2. Update the package version for the `Castle.Sdk` project (`Version` in `src/Castle.Sdk/Castle.Sdk.csproj`).
3. Update `CHANGELOG.md` for the impending release.
4. `git commit -am "release X.Y.Z."` (where X.Y.Z is the new version).
5. Push to GitHub, open a PR to `develop`, and when approved, merge.
6. Merge `develop` into `master` and push.
7. Create a GitHub release from `master`, specifying the tag `vX.Y.Z`. Copy the changelog entry into the release description.
8. Obtain the `.nupkg` to publish. The package multi-targets `net48`, so a complete
   package can only be built on Windows. The `Test` workflow's Windows job already
   runs `dotnet pack -c Release` on every push to `develop`/`master` and uploads the
   result as an artifact named `nupkg`, so just download that artifact.

   **Use the helper script** (requires the [GitHub CLI](https://cli.github.com/),
   authenticated via `gh auth login`):

   ```bash
   bin/fetch-nupkg.sh vX.Y.Z   # the run for the commit the release tag points at
   bin/fetch-nupkg.sh          # or: latest successful run on master
   bin/fetch-nupkg.sh 1a2b3c4  # or: a specific commit SHA
   ```

   It downloads the package into `./artifacts/`.

   > Note: GitHub keeps workflow artifacts for a limited time (90 days by default). If
   > the artifact has expired, re-run the workflow, or build on Windows with
   > `dotnet pack -c Release src/Castle.Sdk/Castle.Sdk.csproj`.
9. Push the package to NuGet (the `.nupkg` is safe to publish — it contains only the
   compiled, MIT-licensed SDK and no secrets). Either let the script download and
   publish in one step:

   ```bash
   NUGET_API_KEY=[nuget API key] bin/fetch-nupkg.sh vX.Y.Z --push
   ```

   …or push a package you already downloaded:

   ```bash
   dotnet nuget push ./artifacts/Castle.Sdk.X.Y.Z.nupkg \
     -k [nuget API key] -s https://api.nuget.org/v3/index.json
   ```
