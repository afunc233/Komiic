using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface IComicDataService
{
    Task<List<MangaInfo>> GetRecentUpdateComic(int pageIndex, string? orderBy = null, bool asc = true);


    Task<List<MangaInfo>> GetHotComic(int pageIndex, string? orderBy = null, string? status = null, bool aes = true);
}