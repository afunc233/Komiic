using System.Collections.Generic;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;

namespace Komiic.Services;

public class RecentUpdateDataService(IKomiicQueryApi komiicQueryApi) : IRecentUpdateDataService
{
    private const int PerPageCount = 20;

    public async Task<List<MangaInfo>> LoadMore(int pageIndex, string? orderBy, bool asc)
    {
        var pagination = new Pagination()
        {
            Asc = asc,
            OrderBy = orderBy ?? "DATE_UPDATED",
            Limit = PerPageCount,
            Offset = pageIndex * PerPageCount
        };
        var recentUpdateData = await komiicQueryApi.GetRecentUpdate(
            QueryDataEnum.RecentUpdate.GetQueryDataWithVariables(
                new PaginationVariables()
                {
                    Pagination = pagination
                })).ConfigureAwait(false);

        return recentUpdateData is { Data.RecentUpdate.Count: > 0 } ? recentUpdateData.Data.RecentUpdate : [];
    }
}