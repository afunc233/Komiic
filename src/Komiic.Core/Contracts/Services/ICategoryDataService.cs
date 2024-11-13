using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface ICategoryDataService
{
    Task<ApiResponseData<List<Category>>> GetAllCategory(CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<MangaInfo>>> GetComicByCategory(string categoryId, int pageIndex,
        string orderBy = "DATE_UPDATED",
        string status = "",CancellationToken? cancellationToken = null);
}