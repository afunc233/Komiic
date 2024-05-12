using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class DiskImageCacheService : IImageCacheService
{
    private readonly string _imageCacheFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
        "Cache", "Images");

    string? IImageCacheService.GetLocalImageUrl(string url)
    {
        string path = Path.Combine(_imageCacheFolder, CreateMD5(url));

        return File.Exists(path) ? path : default;
    }

    async Task IImageCacheService.SetLocalImage(string url, byte[] imageBytes)
    {
        try
        {
            string path = Path.Combine(_imageCacheFolder, CreateMD5(url));
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
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        return BitConverter.ToString(MD5.HashData(bytes)).Replace("-", "");
    }
}