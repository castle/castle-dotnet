#!/usr/bin/env bash
#
# Download the Castle.Sdk .nupkg that CI already built, and optionally push it
# to NuGet.
#
# The package multi-targets net48, so a complete package can only be built on
# Windows. Instead of building locally, this fetches the artifact produced by
# the Windows job of the "Test" workflow (uploaded as an artifact named "nupkg").
# By default it only downloads the package so you can inspect it; pass --push to
# publish it to NuGet.
#
# Usage:
#   bin/fetch-nupkg.sh                  # latest successful run on master
#   bin/fetch-nupkg.sh develop          # latest successful run on a branch
#   bin/fetch-nupkg.sh vX.Y.Z           # the run for the commit a tag points at
#   bin/fetch-nupkg.sh 1a2b3c4          # the run for a specific commit SHA
#   NUGET_API_KEY=xxx bin/fetch-nupkg.sh vX.Y.Z --push   # download and publish
#
# Requirements:
#   - gh (GitHub CLI, authenticated: `gh auth login`)
#   - dotnet (only when using --push)
#
set -euo pipefail

REPO="castle/castle-dotnet"
WORKFLOW="Test"
ARTIFACT_NAME="nupkg"
OUT_DIR="./artifacts"
NUGET_SOURCE="https://api.nuget.org/v3/index.json"

die() { echo "error: $*" >&2; exit 1; }

REF="master"
PUSH="false"
for arg in "$@"; do
  case "$arg" in
    --push) PUSH="true" ;;
    -*) die "unknown option: $arg" ;;
    *) REF="$arg" ;;
  esac
done

command -v gh >/dev/null 2>&1 || die "gh (GitHub CLI) is required: https://cli.github.com/"

# Resolve the ref to the workflow run that built it. Order:
#   1. a tag        -> find the run for the commit it points at
#   2. a commit SHA -> find the run for that commit
#   3. otherwise    -> treat it as a branch name (latest successful run)
SHA="$(gh api "repos/${REPO}/git/refs/tags/${REF}" --jq '.object.sha' 2>/dev/null || true)"

if [ -n "$SHA" ]; then
  echo "==> Tag ${REF} -> commit ${SHA}"
  RUN_ID="$(gh run list --repo "$REPO" --workflow "$WORKFLOW" \
    --commit "$SHA" --status success --limit 1 --json databaseId --jq '.[0].databaseId' || true)"
elif [[ "$REF" =~ ^[0-9a-fA-F]{7,40}$ ]]; then
  echo "==> Commit ${REF}"
  RUN_ID="$(gh run list --repo "$REPO" --workflow "$WORKFLOW" \
    --commit "$REF" --status success --limit 1 --json databaseId --jq '.[0].databaseId' || true)"
else
  echo "==> Latest successful '${WORKFLOW}' run on branch ${REF}"
  RUN_ID="$(gh run list --repo "$REPO" --workflow "$WORKFLOW" \
    --branch "$REF" --status success --limit 1 --json databaseId --jq '.[0].databaseId' || true)"
fi

[ -n "$RUN_ID" ] || die "no successful '${WORKFLOW}' run found for '${REF}' (artifacts expire after ~90 days; re-run the workflow if needed)"

echo "==> Downloading '${ARTIFACT_NAME}' artifact from run ${RUN_ID}"
rm -rf "$OUT_DIR"
gh run download --repo "$REPO" --name "$ARTIFACT_NAME" --dir "$OUT_DIR" "$RUN_ID"

echo "==> Package(s):"
find "$OUT_DIR" -name '*.nupkg' -print

if [ "$PUSH" != "true" ]; then
  echo "==> Done (download only). Re-run with --push to publish to NuGet."
  exit 0
fi

[ -n "${NUGET_API_KEY:-}" ] || die "NUGET_API_KEY env var is required to push"
command -v dotnet >/dev/null 2>&1 || die "dotnet is required to push"

# The .nupkg is safe to publish: it contains only the compiled, MIT-licensed SDK
# and no secrets.
echo "==> Pushing to NuGet (${NUGET_SOURCE})"
find "$OUT_DIR" -name '*.nupkg' -print0 | while IFS= read -r -d '' pkg; do
  dotnet nuget push "$pkg" -k "$NUGET_API_KEY" -s "$NUGET_SOURCE"
done

echo "==> Done."
