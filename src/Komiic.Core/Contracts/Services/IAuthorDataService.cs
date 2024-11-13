using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IAuthorDataService
{
    Task<ApiResponseData<List<MangaInfo>>> GetComicsByAuthor(string authorId,CancellationToken? cancellationToken = null);


    Task<ApiResponseData<List<Author>>> GetAllAuthors(int pageIndex, string orderBy = "DATE_UPDATED",CancellationToken? cancellationToken = null);
}