using System.Security.Cryptography;
using System.Text;
using Komiic.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Komiic.Core.Services;

public class DiskCacheService : ICacheService
{
    private readonly string _jsonCacheFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
        "Cache", "Jsons");

    private readonly ILogger _logger;

    public DiskCacheService(ILogger<DiskCacheService> logger)
    {
        _logger = logger;
        AesUtil.SetLogger(logger);
    }

    async Task<string?> ICacheService.GetLocalCacheStr(string key, TimeSpan? timeSpan)
    {
        var path = Path.Combine(_jsonCacheFolder, CreateMD5(key));

        if (!File.Exists(path))
        {
            return default;
        }

        if (timeSpan.HasValue)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.LastWriteTime.Add(timeSpan.GetValueOrDefault(TimeSpan.Zero)) < DateTime.Now)
            {
                path = default;
            }
        }

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            return default;
        }

        var json = await File.ReadAllTextAsync(path);

        return AesUtil.AesDecrypt(json);
    }

    async Task ICacheService.SetLocalCache(string key, string cache)
    {
        try
        {
            var path = Path.Combine(_jsonCacheFolder, CreateMD5(key));
            Directory.CreateDirectory(_jsonCacheFolder);
            await File.WriteAllTextAsync(path, AesUtil.AesEncrypt(cache)).ConfigureAwait(false);
            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SetLocalCache error ! {key}-->{cache}", key, cache);
        }
    }

    public async Task ClearLocalCache(string key)
    {
        await Task.CompletedTask;

        var path = Path.Combine(_jsonCacheFolder, CreateMD5(key));

        if (!File.Exists(path))
        {
            return;
        }

        File.Delete(path);
    }

    private static string CreateMD5(string input)
    {
        var bytes = Encoding.ASCII.GetBytes(input);
        return BitConverter.ToString(MD5.HashData(bytes)).Replace("-", "");
    }

    #region Aes

    private static class AesUtil
    {
        private const string KeyString = "45863214759886542365896541236548"; // 32字节的密钥
        private const string IVString = "9568423687152368"; // 16字节的初始化向量

        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes(KeyString.PadRight(32, '\0'));
        private static readonly byte[] AesIv = Encoding.UTF8.GetBytes(IVString.PadRight(16, '\0'));

        private static ILogger? _logger;

        internal static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="decryptStr"></param>
        /// <returns></returns>
        internal static string? AesDecrypt(string decryptStr)
        {
            return AesDecrypt(decryptStr, AesKey, AesIv);
        }

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="decryptStr">要解密的串</param>
        /// <param name="aesKey">密钥</param>
        /// <param name="aesIV">IV</param>
        /// <returns></returns>
        private static string? AesDecrypt(string decryptStr, byte[] aesKey, byte[] aesIV)
        {
            try
            {
                var byteDecrypt = Convert.FromBase64String(decryptStr);

                using var aes = Aes.Create();
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                aes.Key = aesKey;
                aes.IV = aesIV;

                using var decryptor = aes.CreateDecryptor(aesKey, aesIV);
                var decrypted = decryptor.TransformFinalBlock(
                    byteDecrypt, 0, byteDecrypt.Length);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "AesDecrypt error!");
            }

            return default;
        }

        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static string? AesEncrypt(string content)
        {
            return AesEncrypt(content, AesKey, AesIv);
        }

        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="byteKey"></param>
        /// <param name="byteIV"></param>
        /// <returns></returns>
        private static string? AesEncrypt(string content, byte[] byteKey, byte[] byteIV)
        {
            try
            {
                var byteContent = Encoding.UTF8.GetBytes(content);

                using var aes = Aes.Create();
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                aes.Key = byteKey;
                aes.IV = byteIV;

                using var encryptor = aes.CreateEncryptor(byteKey, byteIV);
                var decrypted = encryptor.TransformFinalBlock(byteContent, 0, byteContent.Length);

                return Convert.ToBase64String(decrypted);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "AesEncrypt error!");
            }

            return default;
        }
    }

    #endregion
}