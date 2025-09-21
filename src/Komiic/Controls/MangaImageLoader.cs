using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Core;
using Komiic.Core.Contracts.Services;
using Komiic.Data;

namespace Komiic.Controls;

internal class MangaImageLoader(IHttpClientFactory clientFactory, IImageCacheService cacheService) : IMangaImageLoader
{
    private static readonly SemaphoreSlim Throttle = new(6, 6);

    async Task<Bitmap?> IMangaImageLoader.ProvideImageAsync(MangaImageData imageData)
    {
        var bitmap = await LoadAsync(imageData).ConfigureAwait(false);
        return bitmap;
    }

    public event EventHandler<KvValue<MangaImageData, bool>>? ImageLoaded;

    private async Task<Bitmap?> LoadAsync(MangaImageData mangaImageData)
    {
        await Task.CompletedTask;
        var url = mangaImageData.GetImageUrl();
        var localUrl = cacheService.GetLocalImageUrl(url);

        if (string.IsNullOrWhiteSpace(localUrl))
        {
            await Throttle.WaitAsync().ConfigureAwait(false);
            try
            {
                using var httpClient = clientFactory.CreateClient(KomiicConst.Komiic);

                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Referrer = mangaImageData.GetReferrer();

                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                await using var ms = new MemoryStream();
                await stream.CopyToAsync(ms).ConfigureAwait(false);
                var dataArr = ms.ToArray();

                using var memoryStream = new MemoryStream(dataArr);
                var bitmap = new Bitmap(memoryStream);

                await cacheService.SetLocalImage(url, dataArr);

                ImageLoaded?.Invoke(this, new KvValue<MangaImageData, bool>(mangaImageData, true));
                return bitmap;
            }
            finally
            {
                Throttle.Release();
            }
        }

        ImageLoaded?.Invoke(this, new KvValue<MangaImageData, bool>(mangaImageData, false));

        return new Bitmap(localUrl);
    }
}
