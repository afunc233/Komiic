using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class AuthorDataService(IKomiicQueryApi komiicQueryApi) : IAuthorDataService
{
    public async Task<ApiResponseData<List<MangaInfo>>> GetComicsByAuthor(string authorId)
    {
        var variables = QueryDataEnum.ComicsByAuthor.GetQueryDataWithVariables(
            new AuthorIdVariables
            {
                AuthorId = authorId
            });
        var comicsByAuthorData = await komiicQueryApi.GetComicsByAuthor(variables);

        if (comicsByAuthorData is { Data.ComicsByAuthorList.Count: > 0 })
        {
            return new ApiResponseData<List<MangaInfo>>
            {
                Data = comicsByAuthorData.Data.ComicsByAuthorList,
                ErrorMessage = comicsByAuthorData.GetMessage()
            };
        }

        return new ApiResponseData<List<MangaInfo>>
        {
            Data = [],
            ErrorMessage = comicsByAuthorData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<Author>>> GetAllAuthors(int pageIndex, string orderBy = "DATE_UPDATED")
    {
        var variables = QueryDataEnum.Authors.GetQueryDataWithVariables(
            new PaginationVariables
            {
                Pagination = new Pagination
                {
                    Limit = 50,
                    Offset = pageIndex * 50,
                    OrderBy = orderBy
                }
            });
        var allAuthorsData = await komiicQueryApi.GetAllAuthors(variables);
        if (allAuthorsData is { Data.AuthorList.Count: > 0 })
        {
            return new ApiResponseData<List<Author>>
            {
                Data = allAuthorsData.Data.AuthorList,
                ErrorMessage = allAuthorsData.GetMessage()
            };
        }

        return new ApiResponseData<List<Author>>
        {
            Data = [],
            ErrorMessage = allAuthorsData.GetMessage()
        };
    }
}