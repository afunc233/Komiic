Komiic — Release Notes & Dev Log

Phase A – Avalonia improvements (Desktop/Android/iOS/Browser)
- [completed] Default to system theme: set `RequestedThemeVariant="Default"` (src/Komiic/App.axaml:1)
- [completed] Theme preference service: add `IThemePreferenceService` and implementation with local JSON persistence
- [completed] Apply theme at startup via activation handler (runs earliest and applies preference)
- [completed] Header UI: replace toggle with dropdown (Auto/Light/Dark), bind to ViewModel commands
- [completed] Persist and reflect selection in UI; icon shows current mode
- [completed] Refactor key hard-coded colors to theme resources (Overlay, Hover, Muted, Backdrop, Accent)
- [completed] Shorten page transition to 200ms for smoother navigation
- [completed] macOS M4: disable AOT on osx runtime to avoid networking issues, keep single-file publish guidance
- [completed] Minimal docs update in this file; full README sections next

Next suggested steps
- Extend theme resource coverage across all XAML views.
- Optional: tests for ThemePreferenceService (read/write/apply) and VM command wiring.

New features (tooltips + language)
- [completed] Navbar tooltips: show Chinese description on hover (binds to `NavBar.Description`).
- [completed] Localization service with Simplified/Traditional support and persistence.
- [completed] Language switcher in header (简体中文/繁體中文).
- [completed] Localized navbar names and header texts (donate/login/register/my account/logout).

September 21 – Follow‑up Requests Plan
- [completed] Remove language switching; default to Traditional Chinese UI.
- [completed] Ensure all left navbar items show Traditional tooltips.
- [completed] Remove Python client (pyclient) and related docs from repo.

September 21 – New Tasks (Build + Perf + i18n + CI)
- [completed] A: Add publish script and update instructions; prepare new Release build. (scripts/publish_macos.sh)
- [revised] B: Image quality default to HighQuality (thumbnails and reader). Performance handled via throttling rather than lower interpolation. (MangaCard.axaml, MangaImageView.axaml)
- [completed] C: Review and unify Traditional Chinese copy across views (no Simplified residue).
- [completed] D: Add GitHub Actions workflow to build macOS (osx-arm64) release on tag. (.github/workflows/release-macos.yml)

September 21 – Bugfix
- [completed] Improve navbar tooltip reliability without layout changes: keep template root as StackPanel, set `Background=Transparent` and `ToolTip.ShowDelay=0`; remove container stretch. (MainView.axaml)
- [completed] Theme menu checkmarks: only current mode shows a check; updates live on change. (HeaderViewModel.cs, HeaderView.axaml)
- [completed] Fix checkmark visibility reliably by binding to booleans (`IsThemeAuto/Light/Dark`) and defaulting to Auto on first run. (HeaderViewModel.cs, HeaderView.axaml)
- [completed] Build fix: add missing `using System.Threading` for image loader SemaphoreSlim. (MangaImageLoader.cs)
- [completed] Prevent crash on network timeout from reader history update (wrap async void with try/catch, throttle, dedupe). (MangaViewerPageViewModel.cs)

Roadmap – Phase Next (Performance, Security, Packaging)
- [rolled_back] 1) True virtualization via ItemsRepeater postponed: `UniformGridLayout` not available in stable 11.1.x; keeping current WrapPanel + deferred image loading + throttled downloads. Will re-enable when Avalonia 11.2 stable lands or if pre-release is acceptable.
- [completed] 1a) Viewport-based deferred image loading (no new package) — images load when entering view; keeps layout identical. (MangaImageView)
- [completed] 2) Image loader optimizations — throttle concurrent downloads, ensure success checks, safer streaming; improve cache hashing to UTF‑8. (MangaImageLoader.cs, DiskImageCacheService.cs)
- [completed] 3) HTTP timeouts — add Polly TimeoutPolicy and wrap with retry for all HttpClients (incl. images). (KomiicCoreExtensions.cs)
- [completed] 4) Secure storage (phase 1+2) — stop persisting password; keep username only; token stored in macOS Keychain with fallback. (AccountService.cs, TokenService.cs, ISecureStorage, MacKeychainSecureStorage)
- [completed] 5) Theme resource coverage (first pass) — replace remaining hard-coded colors with theme resources: favorite, tag, accent usage in header/login and comic message. (KomiicStyles.axaml, MangaCard.axaml, HeaderView.axaml, ComicMessagePage.axaml)
- [pending] 6) macOS .app bundle + signing — add local script and extend CI to build .app/DMG.

September 22 – macOS Packaging Plan
- [completed] Add local bundling script to create `Komiic.app` and DMG from publish output. (scripts/macos_app_bundle.sh)
- [completed] Extend GitHub Actions to build .app + .dmg for `osx-arm64` and `osx-x64` on tag. (.github/workflows/release-macos-app.yml)
- [pending] Optional: codesign + notarize steps (require Apple developer credentials; to be wired via secrets if desired).
- [pending] 7) Code hygiene — unify wording (繁體), trim `Task.CompletedTask`, ensure UI‑thread updates around collections.
- [completed] 8) Basic tests — ThemePreferenceService round‑trip; DiskImageCacheService set/get. (tests/)

Run Latest Build (macOS, no .NET install)
- Self-contained publish (includes runtime):
  - `dotnet publish src/Komiic.Desktop -c Release -r osx-arm64 -p:SelfContained=true -p:PublishSingleFile=true`
  - Run: `src/Komiic.Desktop/bin/Release/net8.0/osx-arm64/publish/Komiic.Desktop`
  - Note: Intel Mac use `-r osx-x64`.

Alternatively: Install .NET 8 and run
- Install .NET 8 SDK via Microsoft PKG (recommended) or script:
  - Script quick install: `curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh && chmod +x dotnet-install.sh && ./dotnet-install.sh --channel 8.0`
  - Add to PATH (temporary): `export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"`
- Then run debug: `dotnet run --project src/Komiic.Desktop -c Debug`
