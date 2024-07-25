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
    
    Task<List<MangaInfo>> GetMangaInfoByIds(params string[] comicIds);

    Task<List<Folder>> GetMyFolders();

    Task<bool> RemoveComicToFolder(string folderId,string comicId);
    
    Task<bool> AddComicToFolder(string folderId,string comicId);
    
    Task<List<string>> ComicInAccountFolders(string comicId);
    
    Task<List<LastReadByComicId>> GetComicsLastRead(params string[] comicIds);
}