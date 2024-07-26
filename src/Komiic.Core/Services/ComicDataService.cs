using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class ComicDataService(IKomiicQueryApi komiicQueryApi) : IComicDataService
{
    private const int PerPageCount = 20;

    public async Task<ApiResponseData<List<MangaInfo>>> GetRecentUpdateComic(int pageIndex, string? orderBy, bool asc)
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

        if (recentUpdateData is { Data.RecentUpdate.Count: > 0 })
        {
            return new() { Data = recentUpdateData.Data.RecentUpdate };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetHotComic(int pageIndex, string? orderBy = null,
        string? status = null,
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

        if (hotComicsData is { Data.HotComics.Count: > 0 })
        {
            return new() { Data = hotComicsData.Data.HotComics };
        }

        return new() { Data = [] };
    }
}