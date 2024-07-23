using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface ICategoryDataService
{
    Task<List<Category>> GetAllCategory();

    Task<List<MangaInfo>> GetComicByCategory(string categoryId, int pageIndex, string orderBy = "DATE_UPDATED",
        string status = "");
}