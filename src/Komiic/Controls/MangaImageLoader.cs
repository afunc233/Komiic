using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Core;
using Komiic.Core.Contracts.Services;
using Komiic.Data;

namespace Komiic.Controls;

internal class MangaImageLoader(IHttpClientFactory clientFactory, IImageCacheService cacheService) : IMangaImageLoader
{
    async Task<Bitmap?> IMangaImageLoader.ProvideImageAsync(MangaImageData imageData)
    {
        var bitmap = await LoadAsync(imageData).ConfigureAwait(false);
        return bitmap;
    }

    public event EventHandler<KvValue<MangaImageData, bool>>? ImageLoaded;

    private async Task<Bitmap?> LoadAsync(MangaImageData mangaImageData)
    {
        await Task.CompletedTask;
        string url = mangaImageData.GetImageUrl();
        string? localUrl = cacheService.GetLocalImageUrl(url);

        if (string.IsNullOrWhiteSpace(localUrl))
        {
            using var httpClient = clientFactory.CreateClient(KomiicConst.Komiic);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Referrer = mangaImageData.GetReferrer();

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            byte[] dataArr = await response.Content.ReadAsByteArrayAsync();

            using var memoryStream = new MemoryStream(dataArr);
            var bitmap = new Bitmap(memoryStream);

            await cacheService.SetLocalImage(url, dataArr);

            ImageLoaded?.Invoke(this, new(mangaImageData, true));
            return bitmap;
        }

        ImageLoaded?.Invoke(this, new(mangaImageData, false));

        return new(localUrl);
    }
}