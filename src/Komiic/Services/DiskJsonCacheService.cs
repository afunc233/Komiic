using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class DiskJsonCacheService : IJsonCacheService
{
    private readonly string _jsonCacheFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
        "Cache", "Jsons");

    string? IJsonCacheService.GetLocalJsonUrl(string key, TimeSpan? timeSpan)
    {
        string path = Path.Combine(_jsonCacheFolder, CreateMD5(key));

        if (!File.Exists(path)) return default;

        if (timeSpan.HasValue)
        {
            var fileInfo = new FileInfo(path);
            return fileInfo.LastWriteTime.Add(timeSpan.GetValueOrDefault(TimeSpan.Zero)) > DateTime.Now
                ? path
                : default;
        }

        return path;
    }

    async Task IJsonCacheService.SetLocalJson(string key, string json)
    {
        try
        {
            string path = Path.Combine(_jsonCacheFolder, CreateMD5(key));
            Directory.CreateDirectory(_jsonCacheFolder);
            await File.WriteAllTextAsync(path, json).ConfigureAwait(false);
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