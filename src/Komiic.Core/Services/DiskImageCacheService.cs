using System.Security.Cryptography;
using System.Text;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

public class DiskImageCacheService : IImageCacheService
{
    private readonly string _imageCacheFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
        "Cache", "Images");

    string? IImageCacheService.GetLocalImageUrl(string url)
    {
        var path = Path.Combine(_imageCacheFolder, CreateMD5(url));

        return File.Exists(path) ? path : default;
    }

    async Task IImageCacheService.SetLocalImage(string url, byte[] imageBytes)
    {
        try
        {
            var path = Path.Combine(_imageCacheFolder, CreateMD5(url));
            Directory.CreateDirectory(_imageCacheFolder);
            await File.WriteAllBytesAsync(path, imageBytes).ConfigureAwait(false);
            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static string CreateMD5(string input)
    {
        var bytes = Encoding.ASCII.GetBytes(input);
        return BitConverter.ToString(MD5.HashData(bytes)).Replace("-", "");
    }
}