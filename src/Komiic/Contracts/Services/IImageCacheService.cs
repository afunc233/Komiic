using System.Threading.Tasks;

namespace Komiic.Contracts.Services;

public interface IImageCacheService
{
    string? GetLocalImageUrl(string url);

    Task SetLocalImage(string url, byte[] imageBytes);
}