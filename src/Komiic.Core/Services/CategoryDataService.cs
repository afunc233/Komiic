using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class CategoryDataService(IKomiicQueryApi komiicQueryClient) : ICategoryDataService
{
    public async Task<ApiResponseData<List<Category>>> GetAllCategory(CancellationToken? cancellationToken = null)
    {
        List<Category> defaultCategoryArr = [new Category { Id = "0", Name = "全部" }];
        var allCategoryData =
            await komiicQueryClient.GetAllCategory(QueryDataEnum.AllCategory.GetQueryData(), cancellationToken);
        if (allCategoryData is { Data.AllCategories.Count: > 0 })
        {
            return new ApiResponseData<List<Category>>
            {
                Data = [..defaultCategoryArr, .. allCategoryData.Data.AllCategories],
                ErrorMessage = allCategoryData.GetMessage()
            };
        }

        return new ApiResponseData<List<Category>>
        {
            Data = defaultCategoryArr,
            ErrorMessage = allCategoryData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetComicByCategory(string categoryId, int pageIndex,
        string orderBy = "DATE_UPDATED", string status = "", CancellationToken? cancellationToken = null)
    {
        var variables = QueryDataEnum.ComicByCategory.GetQueryDataWithVariables(new CategoryIdPaginationVariables
        {
            CategoryId = categoryId,
            Pagination = new Pagination
            {
                Offset = pageIndex * 20,
                Limit = 20,
                OrderBy = orderBy,
                Status = status
            }
        });

        var comicByCategoryData = await komiicQueryClient.GetComicByCategory(variables, cancellationToken);
        if (comicByCategoryData is { Data.ComicByCategories.Count: > 0 })
        {
            return new ApiResponseData<List<MangaInfo>>
            {
                Data = comicByCategoryData.Data.ComicByCategories,
                ErrorMessage = comicByCategoryData.GetMessage()
            };
        }

        return new ApiResponseData<List<MangaInfo>>
        {
            Data = [],
            ErrorMessage = comicByCategoryData.GetMessage()
        };
    }
}