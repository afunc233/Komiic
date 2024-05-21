using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class DiskCacheService : ICacheService
{
    private readonly string _jsonCacheFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
        "Cache", "Jsons");

    async Task<string?> ICacheService.GetLocalCacheStr(string key, TimeSpan? timeSpan)
    {
        string? path = Path.Combine(_jsonCacheFolder, CreateMD5(key));

        if (!File.Exists(path)) return default;

        if (timeSpan.HasValue)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.LastWriteTime.Add(timeSpan.GetValueOrDefault(TimeSpan.Zero)) < DateTime.Now)
            {
                path = default;
            }
        }

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return default;
        
        
        string json = await File.ReadAllTextAsync(path);
        return json;
    }

    async Task ICacheService.SetLocalCache(string key, string cache)
    {
        try
        {
            string path = Path.Combine(_jsonCacheFolder, CreateMD5(key));
            Directory.CreateDirectory(_jsonCacheFolder);
            await File.WriteAllTextAsync(path, cache).ConfigureAwait(false);
            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task ClearLocalCache(string key)
    {
        await Task.CompletedTask;
        
        string path = Path.Combine(_jsonCacheFolder, CreateMD5(key));

        if (!File.Exists(path)) return;

        File.Delete(path);
    }

    private static string CreateMD5(string input)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        return BitConverter.ToString(MD5.HashData(bytes)).Replace("-", "");
    }
}