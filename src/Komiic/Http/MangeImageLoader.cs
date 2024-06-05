using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Contracts.Services;
using Komiic.Core;
using Komiic.Data;

namespace Komiic.Http;

internal class MangeImageLoader(IHttpClientFactory clientFactory, IImageCacheService cacheService) : IMangeImageLoader
{
    private readonly ConcurrentDictionary<MangeImageData, Task<Bitmap?>> _memoryCache = new();

    async Task<Bitmap?> IMangeImageLoader.ProvideImageAsync(MangeImageData imageData)
    {
        var bitmap = await _memoryCache.GetOrAdd(imageData, LoadAsync).ConfigureAwait(false);
        // If load failed - remove from cache and return
        // Next load attempt will try to load image again
        if (bitmap == null) _memoryCache.TryRemove(imageData, out _);
        return bitmap;
    }

    public event EventHandler<MangeImageData>? ImageLoaded;


    private async Task<Bitmap?> LoadAsync(MangeImageData mangeImageData)
    {
        await Task.CompletedTask;
        string url = mangeImageData.GetImageUrl();
        string? localUrl = cacheService.GetLocalImageUrl(url);

        if (string.IsNullOrWhiteSpace(localUrl))
        {
            using var httpClient = clientFactory.CreateClient(KomiicConst.Komiic);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Referrer = mangeImageData.GetReferrer();

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            byte[] dataArr = await response.Content.ReadAsByteArrayAsync();

            using var memoryStream = new MemoryStream(dataArr);
            var bitmap = new Bitmap(memoryStream);

            await cacheService.SetLocalImage(url, dataArr);

            ImageLoaded?.Invoke(this, mangeImageData);
            return bitmap;
        }
        
        return new Bitmap(localUrl);
    }
}