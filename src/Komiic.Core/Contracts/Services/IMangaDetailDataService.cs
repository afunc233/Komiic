using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface IMangaDetailDataService
{
    Task<ApiResponseData<MangaInfo?>> GetMangaInfoById(string comicId);

    Task<ApiResponseData<int?>> GetMessageCountByComicId(string comicId);

    Task<ApiResponseData<List<MessagesByComicId>>> GetMessagesByComicId(string comicId, int pageIndex,
        string orderBy = "DATE_UPDATED");

    Task<ApiResponseData<LastMessageByComicId?>> GetLastMessageByComicId(string comicId);

    Task<ApiResponseData<List<ChaptersByComicId>>> GetChapterByComicId(string comicId);

    Task<ApiResponseData<List<string>>> GetRecommendComicById(string comicId);

    Task<ApiResponseData<List<MangaInfo>>> GetRecommendMangaInfosById(string comicId);

    Task<ApiResponseData<List<MangaInfo>>> GetMangaInfoByIds(params string[] comicIds);

    Task<ApiResponseData<List<Folder>>> GetMyFolders();

    Task<ApiResponseData<bool?>> RemoveComicToFolder(string folderId, string comicId);

    Task<ApiResponseData<bool?>> AddComicToFolder(string folderId, string comicId);

    Task<ApiResponseData<List<string>>> ComicInAccountFolders(string comicId);

    Task<ApiResponseData<List<LastReadByComicId>>> GetComicsLastRead(params string[] comicIds);

    Task<ApiResponseData<bool?>> RemoveFavorite(string comicId);

    Task<ApiResponseData<Favorite>> AddFavorite(string comicId);

    Task<ApiResponseData<bool?>> VoteMessage(string messageId, bool isUp);

    Task<ApiResponseData<List<MessageVotesByComicId>>> MessageVotesByComicId(string comicId);

    Task<ApiResponseData<MessagesByComicId>> AddMessageToComic(string comicId, string sendMessageText,
        string? replyToId = "0");
}