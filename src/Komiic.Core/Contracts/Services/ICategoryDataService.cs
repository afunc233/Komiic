using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface ICategoryDataService
{
    Task<ApiResponseData<List<Category>>> GetAllCategory();

    Task<ApiResponseData<List<MangaInfo>>> GetComicByCategory(string categoryId, int pageIndex,
        string orderBy = "DATE_UPDATED",
        string status = "");
}