#!/usr/bin/env bash
set -euo pipefail

# Bundle a published Komiic.Desktop into a macOS .app and DMG.
# Usage: ./scripts/macos_app_bundle.sh [Release|Debug] [osx-arm64|osx-x64]

CONFIG=${1:-Release}
RID=${2:-osx-arm64}
PROJECT="src/Komiic.Desktop"
APP_NAME="Komiic"
PUBLISH_DIR="$PROJECT/bin/$CONFIG/net8.0/$RID/publish"
DIST_DIR="dist"
BUNDLE_DIR="$DIST_DIR/$APP_NAME-$RID.app"

echo "[1/4] Publish self-contained single-file for $RID ($CONFIG)"
dotnet publish "$PROJECT" -c "$CONFIG" -r "$RID" -p:SelfContained=true -p:PublishSingleFile=true

echo "[2/4] Create .app bundle structure"
rm -rf "$BUNDLE_DIR"
mkdir -p "$BUNDLE_DIR/Contents/MacOS" "$BUNDLE_DIR/Contents/Resources"

cat > "$BUNDLE_DIR/Contents/Info.plist" <<'PLIST'
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key><string>Komiic</string>
    <key>CFBundleDisplayName</key><string>Komiic</string>
    <key>CFBundleIdentifier</key><string>com.komiic.app</string>
    <key>CFBundleVersion</key><string>1.0</string>
    <key>CFBundleShortVersionString</key><string>1.0</string>
    <key>CFBundleExecutable</key><string>Komiic.Desktop</string>
    <key>LSMinimumSystemVersion</key><string>11.0</string>
    <key>LSApplicationCategoryType</key><string>public.app-category.entertainment</string>
    <key>NSHighResolutionCapable</key><true/>
</dict>
</plist>
PLIST

echo "[3/4] Copy publish output into .app bundle"
cp -R "$PUBLISH_DIR"/* "$BUNDLE_DIR/Contents/MacOS/"
chmod +x "$BUNDLE_DIR/Contents/MacOS/Komiic.Desktop" || true

echo "[4/4] Create DMG"
mkdir -p "$DIST_DIR"
DMG_PATH="$DIST_DIR/$APP_NAME-$RID.dmg"
rm -f "$DMG_PATH"
hdiutil create -fs HFS+ -volname "$APP_NAME" -srcfolder "$BUNDLE_DIR" -ov "$DMG_PATH" >/dev/null

echo "Bundle: $BUNDLE_DIR"
echo "DMG:    $DMG_PATH"

