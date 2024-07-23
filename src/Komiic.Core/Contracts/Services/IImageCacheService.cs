namespace Komiic.Core.Contracts.Services;

public interface IImageCacheService
{
    string? GetLocalImageUrl(string url);

    Task SetLocalImage(string url, byte[] imageBytes);
}