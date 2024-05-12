using System.Collections.Generic;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;

namespace Komiic.Services;

public class HotComicsDataService(IKomiicQueryApi komiicQueryApi) : IHotComicsDataService
{
    private const int PerPageCount = 20;

    public async Task<List<MangaInfo>> LoadMore(int pageIndex, string? orderBy = null, string? status = null,
        bool aes = true)
    {
        var pagination = new Pagination()
        {
            Limit = PerPageCount,
            Offset = pageIndex * PerPageCount,
            OrderBy = orderBy ?? "MONTH_VIEWS",
            Status = status ?? "",
            Asc = aes
        };

        var hotComicsData = await komiicQueryApi.GetHotComics(QueryDataEnum.HotComics.GetQueryDataWithVariables(
            new PaginationVariables()
            {
                Pagination = pagination
            }));
        return hotComicsData is { Data.HotComics.Count: > 0 } ? hotComicsData.Data.HotComics : [];
    }
}