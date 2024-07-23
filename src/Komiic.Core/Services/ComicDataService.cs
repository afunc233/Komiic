using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class ComicDataService(IKomiicQueryApi komiicQueryApi) : IComicDataService
{
    private const int PerPageCount = 20;

    public async Task<List<MangaInfo>> GetRecentUpdateComic(int pageIndex, string? orderBy, bool asc)
    {
        var pagination = new Pagination
        {
            Asc = asc,
            OrderBy = orderBy ?? "DATE_UPDATED",
            Limit = PerPageCount,
            Offset = pageIndex * PerPageCount
        };
        var recentUpdateData = await komiicQueryApi.GetRecentUpdate(
            QueryDataEnum.RecentUpdate.GetQueryDataWithVariables(
                new PaginationVariables
                {
                    Pagination = pagination
                }));

        return recentUpdateData is { Data.RecentUpdate.Count: > 0 } ? recentUpdateData.Data.RecentUpdate : [];
    }
    
    public async Task<List<MangaInfo>> GetHotComic(int pageIndex, string? orderBy = null, string? status = null,
        bool aes = true)
    {
        var pagination = new Pagination
        {
            Limit = PerPageCount,
            Offset = pageIndex * PerPageCount,
            OrderBy = orderBy ?? "MONTH_VIEWS",
            Status = status ?? "",
            Asc = aes
        };

        var hotComicsData = await komiicQueryApi.GetHotComics(QueryDataEnum.HotComics.GetQueryDataWithVariables(
            new PaginationVariables
            {
                Pagination = pagination
            }));
        return hotComicsData is { Data.HotComics.Count: > 0 } ? hotComicsData.Data.HotComics : [];
    }
}