using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class AuthorDataService(IKomiicQueryApi komiicQueryClient) : IAuthorDataService
{
    public async Task<ApiResponseData<List<MangaInfo>>> GetComicsByAuthor(string authorId,
        CancellationToken? cancellationToken = null)
    {
        var variables = QueryDataEnum.ComicsByAuthor.GetQueryDataWithVariables(
            new AuthorIdVariables
            {
                AuthorId = authorId
            });
        var comicsByAuthorData = await komiicQueryClient.GetComicsByAuthor(variables, cancellationToken);

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

    public async Task<ApiResponseData<List<Author>>> GetAllAuthors(int pageIndex, string orderBy = "DATE_UPDATED",
        CancellationToken? cancellationToken = null)
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
        var allAuthorsData = await komiicQueryClient.GetAllAuthors(variables, cancellationToken);
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