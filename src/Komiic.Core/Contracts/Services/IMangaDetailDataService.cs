using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IMangaDetailDataService
{
    Task<ApiResponseData<MangaInfo?>> GetMangaInfoById(string comicId, CancellationToken? cancellationToken = null);

    Task<ApiResponseData<int?>> GetMessageCountByComicId(string comicId, CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<MessagesByComicId>>> GetMessagesByComicId(string comicId, int pageIndex,
        string orderBy = "DATE_UPDATED", CancellationToken? cancellationToken = null);

    Task<ApiResponseData<LastMessageByComicId?>> GetLastMessageByComicId(string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<ChaptersByComicId>>> GetChapterByComicId(string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<string>>> GetRecommendComicById(string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<MangaInfo>>> GetRecommendMangaInfosById(string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<MangaInfo>>> GetMangaInfoByIds(CancellationToken? cancellationToken = null,
        params List<string> comicIds);

    Task<ApiResponseData<List<Folder>>> GetMyFolders(CancellationToken? cancellationToken = null);

    Task<ApiResponseData<bool?>> RemoveComicToFolder(string folderId, string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<bool?>> AddComicToFolder(string folderId, string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<string>>> ComicInAccountFolders(string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<LastReadByComicId>>> GetComicsLastRead(CancellationToken? cancellationToken = null,
        params List<string> comicIds);

    Task<ApiResponseData<bool?>> RemoveFavorite(string comicId, CancellationToken? cancellationToken = null);

    Task<ApiResponseData<Favorite>> AddFavorite(string comicId, CancellationToken? cancellationToken = null);

    Task<ApiResponseData<bool?>> VoteMessage(string messageId, bool isUp, CancellationToken? cancellationToken = null);

    Task<ApiResponseData<List<MessageVotesByComicId>>> MessageVotesByComicId(string comicId,
        CancellationToken? cancellationToken = null);

    Task<ApiResponseData<MessagesByComicId>> AddMessageToComic(string comicId, string sendMessageText,
        string? replyToId = "0", CancellationToken? cancellationToken = null);

    Task<ApiResponseData<bool?>> DeleteMessage(string messageId, CancellationToken? cancellationToken = null);
}