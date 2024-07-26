using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class CategoryDataService(IKomiicQueryApi komiicQueryApi) : ICategoryDataService
{
    public async Task<ApiResponseData<List<Category>>> GetAllCategory()
    {
        List<Category> defaultCategoryArr = [new() { Id = "0", Name = "全部" }];
        var allCategoryData = await komiicQueryApi.GetAllCategory(QueryDataEnum.AllCategory.GetQueryData());
        if (allCategoryData is { Data.AllCategories.Count: > 0 })
        {
            return new() { Data = [..defaultCategoryArr, .. allCategoryData.Data.AllCategories] };
        }

        return new() { Data = defaultCategoryArr };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetComicByCategory(string categoryId, int pageIndex,
        string orderBy = "DATE_UPDATED", string status = "")
    {
        var variables = QueryDataEnum.ComicByCategory.GetQueryDataWithVariables(new CategoryIdPaginationVariables
        {
            CategoryId = categoryId,
            Pagination = new()
            {
                Offset = pageIndex * 20,
                Limit = 20,
                OrderBy = orderBy,
                Status = status
            }
        });

        var comicByCategoryData = await komiicQueryApi.GetComicByCategory(variables);
        if (comicByCategoryData is { Data.ComicByCategories.Count: > 0 })
        {
            return new() { Data = comicByCategoryData.Data.ComicByCategories };
        }

        return new() { Data = [] };
    }
}