
# 項目來源

偶然間看到 [[Komiic](http://komiic.com)](https://komiic.com/) 項目 ,感覺做的非常好。注意到站長未提供客戶端，所以有給網站寫客戶端的想法了，鑒於站長使用繁體中文，故而本頁面使用繁體中文以示尊重

# 簡介
本項目使用 C# + Avalonia 編寫，目前支持 Windows Mac Linux Android 和 IOS(目前未做測試)，Wasm 因爲 跨域問題，目前未能支持

由於其他平臺打包複雜，目前只實際嘗試了 Windows 和 Android 的打包，均可正常運行，Mac Linux 則完成了調試狀態下的運行 (Rider 運行項目)

**另外個人認爲本項目也是一個很好的學習 Avalonia 的項目，歡迎大家嘗試和 PR**

# 效果

桌面端效果

<img width="600px" src="https://github.com/afunc233/Komiic/blob/master/Images/desktop.png" alt="桌面端效果" />

移動端效果

<img width="600px" src="https://github.com/afunc233/Komiic/blob/master/Images/mobile.jpg" alt="移動端效果" />

## 最近更新（深色模式與 macOS 改進）
- 默認跟隨系統主題（可在頂欄切換 自動/淺色/深色，並持久化偏好）
- 修正與補充主題資源（覆蓋卡片/對話遮罩/懸停等顏色）
- 桌面端頁面切換動畫縮短至 200ms，更加跟手
- macOS (arm64) 發佈：對 `osx` Runtime 自動禁用 AOT，避免網絡超時

桌面發佈（macOS arm64）
```
dotnet publish src/Komiic.Desktop -c Release -r osx-arm64 -p:SelfContained=true -p:PublishSingleFile=true
```

<!-- Python 客戶端已移除，當前倉庫僅保留 Avalonia 客戶端程式碼與文檔 -->
