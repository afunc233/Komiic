using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface IMangaDetailDataService
{
    Task<MangaInfo?> GetMangaInfoById(string comicId);
    
    Task<int> GetMessageCountByComicId(string comicId);
    
    Task<LastMessageByComicId?> GetLastMessageByComicId(string comicId);
    
    Task<List<ChaptersByComicId>> GetChapterByComicId(string comicId);
    
    Task<List<string>> GetRecommendComicById(string comicId);
    
    Task<List<MangaInfo>> GetRecommendMangaInfosById(string comicId);
    
    Task<List<MangaInfo>> GetMangaInfoByIds(List<string> comicIdList);
    
}