using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

public class MockImageCacheService : IImageCacheService
{
    public string? GetLocalImageUrl(string url)
    {
        return default;
    }

    public Task SetLocalImage(string url, byte[] imageBytes)
    {
        return Task.CompletedTask;
    }
}