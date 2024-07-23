using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface IAuthorDataService
{
    Task<List<MangaInfo>> GetComicsByAuthor(string authorId);


    Task<List<Author>> GetAllAuthors(int pageIndex, string orderBy = "DATE_UPDATED");
}