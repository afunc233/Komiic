using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Core;
using Komiic.Core.Contracts.Services;
using Komiic.Data;

namespace Komiic.Controls;

internal class MangeImageLoader(IHttpClientFactory clientFactory, IImageCacheService cacheService) : IMangeImageLoader
{
    async Task<Bitmap?> IMangeImageLoader.ProvideImageAsync(MangeImageData imageData)
    {
        var bitmap = await LoadAsync(imageData).ConfigureAwait(false);
        return bitmap;
    }

    public event EventHandler<KvValue<MangeImageData, bool>>? ImageLoaded;

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

            ImageLoaded?.Invoke(this, new(mangeImageData, true));
            return bitmap;
        }

        ImageLoaded?.Invoke(this, new(mangeImageData, false));

        return new Bitmap(localUrl);
    }
}