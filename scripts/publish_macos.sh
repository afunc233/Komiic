#!/usr/bin/env bash
set -euo pipefail

# Build self-contained single-file app for macOS Apple Silicon (arm64)
# Usage: ./scripts/publish_macos.sh [Debug|Release]

CONFIG="${1:-Release}"
RID="osx-arm64"
PROJECT="src/Komiic.Desktop"

echo "Building $PROJECT ($CONFIG) for $RID ..."
dotnet publish "$PROJECT" -c "$CONFIG" -r "$RID" -p:SelfContained=true -p:PublishSingleFile=true

OUT_DIR="$PROJECT/bin/$CONFIG/net8.0/$RID/publish"
echo "Output: $OUT_DIR"
ls -la "$OUT_DIR"
echo "Run: $OUT_DIR/Komiic.Desktop"

