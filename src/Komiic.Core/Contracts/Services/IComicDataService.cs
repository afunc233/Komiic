using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IComicDataService
{
    Task<ApiResponseData<List<MangaInfo>>> GetRecentUpdateComic(int pageIndex, string? orderBy = null, bool asc = true,CancellationToken? cancellationToken = null);


    Task<ApiResponseData<List<MangaInfo>>> GetHotComic(int pageIndex, string? orderBy = null, string? status = null,
        bool aes = true,CancellationToken? cancellationToken = null);
}