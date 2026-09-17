#!/usr/bin/env bash

set -euo pipefail

if [[ $# -ne 1 ]]; then
    echo "Usage: $0 <version>"
    exit 1
fi

VERSION="${1#v}"
TAG="$VERSION"
PRERELEASE_IDENTIFIER='(0|[1-9][0-9]*|[0-9]*[A-Za-z-][0-9A-Za-z-]*)'
SEMVER_PATTERN="^(0|[1-9][0-9]*)\\.(0|[1-9][0-9]*)\\.(0|[1-9][0-9]*)(-${PRERELEASE_IDENTIFIER}(\\.${PRERELEASE_IDENTIFIER})*)?$"

if [[ ! $VERSION =~ $SEMVER_PATTERN ]]; then
    echo "Invalid version: $1"
    exit 1
fi

REPOSITORY_ROOT="$(git -C "$(dirname "$0")" rev-parse --show-toplevel)"
DOTNET_PROJECT="$REPOSITORY_ROOT/dotnet/Barchart.BinarySerializer/Barchart.BinarySerializer.csproj"
NPM_PACKAGE="$REPOSITORY_ROOT/typescript/package.json"
RELEASE_NOTES="$REPOSITORY_ROOT/.releases/$VERSION.md"

cd "$REPOSITORY_ROOT"

if [[ $(git branch --show-current) != "main" ]]; then
    echo "Releases must be prepared from the main branch."
    exit 1
fi

if [[ ! -f $RELEASE_NOTES ]]; then
    echo "Missing release notes: .releases/$VERSION.md"
    exit 1
fi

if [[ -n $(git status --porcelain --untracked-files=all -- . ":(exclude).releases/$VERSION.md") ]]; then
    echo "Commit or stash changes outside .releases/$VERSION.md before preparing a release."
    exit 1
fi

git fetch origin main --tags

if git rev-parse --verify --quiet "refs/tags/$TAG" >/dev/null; then
    if [[ $(git rev-parse "refs/tags/$TAG^{commit}") != $(git rev-parse HEAD) ]]; then
        echo "Tag already exists on another commit: $TAG"
        exit 1
    fi

    git push --atomic origin main "$TAG"
    echo "Prepared $TAG. Create a GitHub Release from this tag to publish the package."
    exit 0
fi

if [[ $(git rev-parse HEAD) != $(git rev-parse origin/main) ]]; then
    echo "Local main must match origin/main."
    exit 1
fi

RELEASE_VERSION="$VERSION" perl -0pi -e 's|<Version>[^<]+</Version>|<Version>$ENV{RELEASE_VERSION}</Version>|' "$DOTNET_PROJECT"
RELEASE_VERSION="$VERSION" perl -0pi -e 's|("version":\s*")[^"]+("\s*,)|$1$ENV{RELEASE_VERSION}$2|' "$NPM_PACKAGE"

if ! grep -Fq "<Version>$VERSION</Version>" "$DOTNET_PROJECT"; then
    echo "Failed to update the .NET project version."
    exit 1
fi

if [[ $(node -p "require('$NPM_PACKAGE').version") != "$VERSION" ]]; then
    echo "Failed to update the npm package version."
    exit 1
fi

git add "$RELEASE_NOTES" "$DOTNET_PROJECT" "$NPM_PACKAGE"
git commit -m "chore(release): $TAG"
git tag -a "$TAG" -m "$TAG"
git push --atomic origin main "$TAG"

echo "Prepared $TAG. Create a GitHub Release from this tag to publish the package."
