using System.Threading.Tasks;
using Komiic.Contracts.Services;

namespace Komiic.Services;

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