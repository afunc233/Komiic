Komiic — Cross‑Platform Manga Client (Avalonia)

極簡、快、好看。這是一個基於 C# + Avalonia 的跨平臺漫畫客戶端，支持 macOS/Windows/Linux/Android/iOS（Wasm 受跨域限制暫不支持）。本倉庫默認使用繁體中文。

**Screenshots**

桌面端效果

<img width="600px" src="https://github.com/afunc233/Komiic/blob/master/Images/desktop.png" alt="桌面端效果" />

移動端效果

<img width="600px" src="https://github.com/afunc233/Komiic/blob/master/Images/mobile.jpg" alt="移動端效果" />

**Features**
- 深色模式：默認「跟隨系統」，可在頂欄切換「跟隨系統 / 淺色 / 深色」，偏好持久化
- 高品質圖片：縮略圖與閱讀器均為 HighQuality 插值，視口內延遲加載 + 並發限流，畫質與流暢兼得
- 穩健網絡：所有 API 帶超時（10s）+ 重試；閱讀進度上報去重/限頻/免崩潰
- 本地緩存：圖片落盤緩存、JSON 緩存；macOS 上 Token 儲存於 Keychain（安全優先）
- 打包省心：自帶運行時單文件發佈；一鍵生成 `.app` 與 `.dmg`；GitHub Actions 釋出

**Quick Start（測試最新構建）**
- 一行發佈（macOS Apple Silicon）
  - `dotnet publish src/Komiic.Desktop -c Release -r osx-arm64 -p:SelfContained=true -p:PublishSingleFile=true`
  - 運行：`src/Komiic.Desktop/bin/Release/net8.0/osx-arm64/publish/Komiic.Desktop`
- Intel Mac 請將 `-r osx-arm64` 改為 `-r osx-x64`

**Run From Source（開發調試）**
- 依賴：.NET 8 SDK（建議）
- 調試運行：`dotnet run --project src/Komiic.Desktop -c Debug`

**Build macOS .app / .dmg**
- 本地：
  - `chmod +x scripts/macos_app_bundle.sh`
  - `./scripts/macos_app_bundle.sh Release osx-arm64`
  - 輸出：`dist/Komiic-osx-arm64.app`、`dist/Komiic-osx-arm64.dmg`
- CI：推送 `v*` 標籤會自動產生 `.dmg` 附件（arm64/x64）

**Developer Notes**
- 主題偏好：`src/Komiic/Services/ThemePreferenceService.cs`（JSON），啓動激活：`src/Komiic/Services/LoadThemePreferenceActivationHandler.cs`
- 深/淺色資源：`src/Komiic/Styles/KomiicStyles.axaml`
- Keychain 儲存：`src/Komiic.Core/Services/MacKeychainSecureStorage.cs` + `TokenService`
- 影像延遲加載：`src/Komiic/Controls/MangaImageView.axaml.cs`（EffectiveViewport），下載限流：`src/Komiic/Controls/MangaImageLoader.cs`
- 發佈腳本：`scripts/publish_macos.sh`、`scripts/macos_app_bundle.sh`

**Run Tests**
- 需要 .NET 8 运行时（與 .NET 9 可共存）
- `dotnet test tests/Komiic.Tests`

—

致謝：Komiic 原站點內容及設計靈感；Avalonia 社區。歡迎提交 Issue / PR，一起把體驗打磨到絲滑。
