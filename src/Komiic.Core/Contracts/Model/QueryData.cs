using System.Text;
using System.Text.Json.Serialization;

namespace Komiic.Core.Contracts.Model;

public class QueryData(string operationName, string query)
{
    [JsonPropertyName("operationName")] public string OperationName { get; } = operationName;

    [JsonPropertyName("query")] public string Query { get; set; } = query;
}

public class QueryData<T>(string operationName, string query) : QueryData(operationName, query)
{
    [JsonPropertyName("variables")] public T? Variables { get; set; }
}

public enum QueryDataEnum
{
    RecentUpdate,
    HotComics,
    GetImageLimit,
    AllCategory,
    ComicByCategory,
    Authors,
    ComicsByAuthor,
    ComicByIds,
    ComicById,
    RecommendComicById,
    ChapterByComicId,
    GetMessagesByComicId,
    ImagesByChapterId,
    AccountQuery,
    FavoriteNewUpdatedQuery,
    UpdateProfileImage,
    SetNextChapterMode,
    
    AddReadComicHistory,
    ReadComicHistory,
    ReadComicHistoryById,
    DeleteComicReadHistory,
    
    AddFavorite,
    RemoveFavorite,
    FavoritesQuery,
    ComicsLastRead,

    RecommendComicIds,

    MyFolder,
    FolderByKey,
    FolderComicIds,
    UpdateFolderName,
    RemoveFolder,
    AddComicToFolder,
    ComicInAccountFolders,

    AddMessageToComic,
    MessageChan,
    MessageCountByComicId,
    LastMessageByComicId,
    VoteMessage,
    MessageVotesByComicId,
    DeleteMessage,
}

public static class QueryDataExt
{
    #region Methods

    private static readonly Dictionary<QueryDataEnum, string> OperationQueryDic =
        new()
        {
            {
                QueryDataEnum.RecentUpdate,
                "query recentUpdate($pagination: Pagination!) {\n  recentUpdate(pagination: $pagination) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.HotComics,
                "query hotComics($pagination: Pagination!) {\n  hotComics(pagination: $pagination) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },

            {
                QueryDataEnum.GetImageLimit,
                "query getImageLimit {\n  getImageLimit {\n    limit\n    usage\n    resetInSeconds\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.AllCategory,
                "query allCategory {\n  allCategory {\n    id\n    name\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicByCategory,
                "query comicByCategory($categoryId: ID!, $pagination: Pagination!) {\n  comicByCategory(categoryId: $categoryId, pagination: $pagination) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.Authors,
                "query authors($pagination: Pagination!) {\n  authors(pagination: $pagination) {\n    id\n    name\n    chName\n    enName\n    wikiLink\n    comicCount\n    views\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicsByAuthor,
                "query comicsByAuthor($authorId: ID!) {\n  getComicsByAuthor(authorId: $authorId) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicByIds,
                "query comicByIds($comicIds: [ID]!) {\n  comicByIds(comicIds: $comicIds) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicById,
                "query comicById($comicId: ID!) {\n  comicById(comicId: $comicId) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateCreated\n    dateUpdated\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.RecommendComicById,
                "query recommendComicById($comicId: ID!) {\n  recommendComicById(comicId: $comicId)\n}"
            },
            {
                QueryDataEnum.MessageCountByComicId,
                "query messageCountByComicId($comicId: ID!) {\n  messageCountByComicId(comicId: $comicId)\n}"
            },
            {
                QueryDataEnum.LastMessageByComicId,
                "query lastMessageByComicId($comicId: ID!) {\n  lastMessageByComicId(comicId: $comicId) {\n    comicId\n    message\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ChapterByComicId,
                "query chapterByComicId($comicId: ID!) {\n  chaptersByComicId(comicId: $comicId) {\n    id\n    serial\n    type\n    dateCreated\n    dateUpdated\n    size\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.GetMessagesByComicId,
                "query getMessagesByComicId($comicId: ID!, $pagination: Pagination!) {\n  getMessagesByComicId(comicId: $comicId, pagination: $pagination) {\n    id\n    comicId\n    account {\n      id\n      nickname\n      profileText\n      profileTextColor\n      profileBackgroundColor\n      profileImageUrl\n      __typename\n    }\n    message\n    replyTo {\n      id\n      message\n      account {\n        id\n        nickname\n        profileText\n        profileTextColor\n        profileBackgroundColor\n        profileImageUrl\n        __typename\n      }\n      __typename\n    }\n    upCount\n    downCount\n    dateUpdated\n    dateCreated\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ImagesByChapterId,
                "query imagesByChapterId($chapterId: ID!) {\n  imagesByChapterId(chapterId: $chapterId) {\n    id\n    kid\n    height\n    width\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.AccountQuery,
                "query accountQuery {\n  account {\n    id\n    email\n    nickname\n    dateCreated\n    favoriteComicIds\n    profileText\n    profileTextColor\n    profileBackgroundColor\n    totalDonateAmount\n    monthDonateAmount\n    profileImageUrl\n    nextChapterMode\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.FavoriteNewUpdatedQuery,
                "query favoriteNewUpdatedQuery {\n  getLatestUpdatedDateInFavorite\n}"
            },
            {
                QueryDataEnum.UpdateProfileImage,
                "mutation updateProfileImage($text: String!, $textColor: String!, $backgroundColor: String!) {\n  updateProfileImage(\n    text: $text\n    textColor: $textColor\n    backgroundColor: $backgroundColor\n  )\n}"
            },
            {
                QueryDataEnum.SetNextChapterMode,
                "mutation setNextChapterMode($mode: String!) {\n  setNextChapterMode(mode: $mode)\n}"
            },
            {
                QueryDataEnum.AddReadComicHistory,
                "mutation addReadComicHistory($comicId: ID!, $chapterId: ID!, $page: Int!) {\n  addReadComicHistory(comicId: $comicId, chapterId: $chapterId, page: $page) {\n    id\n    comicId\n    chapters {\n      id\n      chapterId\n      page\n      __typename\n    }\n    startDate\n    lastDate\n    chapterType\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ReadComicHistory,
                "query readComicHistory($pagination: Pagination!) {\n  readComicHistory(pagination: $pagination) {\n    id\n    comicId\n    chapters {\n      id\n      chapterId\n      page\n      __typename\n    }\n    startDate\n    lastDate\n    chapterType\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ReadComicHistoryById,
                "query readComicHistoryById($comicId: ID!) {\n  readComicHistoryById(comicId: $comicId) {\n    id\n    comicId\n    chapters {\n      id\n      chapterId\n      page\n      __typename\n    }\n    startDate\n    lastDate\n    chapterType\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.DeleteComicReadHistory,
                "mutation deleteComicReadHistory($comicId: ID!) {\n  deleteReadComicHistory(comicId: $comicId)\n}"
            },
            {
                QueryDataEnum.AddFavorite,
                "mutation addFavorite($comicId: ID!) {\n  addFavorite(comicId: $comicId) {\n    id\n    comicId\n    dateAdded\n    lastAccess\n    bookReadProgress\n    chapterReadProgress\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.RemoveFavorite,
                "mutation removeFavorite($comicId: ID!) {\n  removeFavorite(comicId: $comicId)\n}"
            },
            {
                QueryDataEnum.FavoritesQuery,
                "query favoritesQuery($pagination: Pagination!) {\n  getLatestUpdatedDateInFavorite\n  favoritesV2(pagination: $pagination) {\n    id\n    comicId\n    dateAdded\n    lastAccess\n    bookReadProgress\n    chapterReadProgress\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicsLastRead,
                "query comicsLastRead($comicIds: [ID]!) {\n  lastReadByComicIds(comicIds: $comicIds) {\n    comicId\n    book {\n      page\n      chapterId\n      serial\n      __typename\n    }\n    chapter {\n      page\n      chapterId\n      serial\n      __typename\n    }\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.RecommendComicIds,
                "query recommendComicIds($category: String, $pagination: Pagination!) {\n  recommendComicIds(category: $category, pagination: $pagination)\n}"
            },
            {
                QueryDataEnum.MyFolder,
                "query myFolder {\n  folders {\n    id\n    key\n    name\n    views\n    comicCount\n    dateCreated\n    dateUpdated\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.FolderByKey,
                "query folderByKey($key: String!) {\n  folder(folderKey: $key) {\n    id\n    key\n    name\n    views\n    dateCreated\n    dateUpdated\n    comicCount\n    account {\n      id\n      nickname\n      __typename\n    }\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.FolderComicIds,
                "query folderComicIds($folderId: ID!, $pagination: Pagination!) {\n  folderComicIds(folderId: $folderId, pagination: $pagination) {\n    folderId\n    key\n    comicIds\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.UpdateFolderName,
                "mutation updateFolderName($folderId: ID!, $name: String!) {\n  updateFolderName(folderId: $folderId, name: $name)\n}"
            },
            {
                QueryDataEnum.RemoveFolder,
                "mutation removeFolder($folderId: ID!) {\n  removeFolder(folderId: $folderId)\n}"
            },
            {
                QueryDataEnum.AddComicToFolder,
                "mutation addComicToFolder($comicId: ID!, $folderId: ID!) {\n  addComicToFolder(comicId: $comicId, folderId: $folderId)\n}"
            },
            {
                QueryDataEnum.ComicInAccountFolders,
                "query chapterByComicId($comicId: ID!) {\n  chaptersByComicId(comicId: $comicId) {\n    id\n    serial\n    type\n    dateCreated\n    dateUpdated\n    size\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.AddMessageToComic,
                "mutation addMessageToComic($comicId: ID!, $replyToId: ID!, $message: String!) {\n  addMessageToComic(message: $message, comicId: $comicId, replyToId: $replyToId) {\n    id\n    message\n    comicId\n    account {\n      id\n      nickname\n      __typename\n    }\n    replyTo {\n      id\n      message\n      account {\n        id\n        nickname\n        profileText\n        profileTextColor\n        profileBackgroundColor\n        profileImageUrl\n        __typename\n      }\n      __typename\n    }\n    dateCreated\n    dateUpdated\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.MessageChan,
                "query messageChan($messageId: ID!) {\n  messageChan(messageId: $messageId) {\n    id\n    comicId\n    account {\n      id\n      nickname\n      profileText\n      profileTextColor\n      profileBackgroundColor\n      profileImageUrl\n      __typename\n    }\n    message\n    replyTo {\n      id\n      __typename\n    }\n    upCount\n    downCount\n    dateUpdated\n    dateCreated\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.VoteMessage,
                "mutation voteMessage($messageId: ID!, $up: Boolean!) {\n  voteMessage(messageId: $messageId, up: $up)\n}"
            },
            {
                QueryDataEnum.MessageVotesByComicId,
                "query messageVotesByComicId($comicId: ID!) {\n  messageVotesByComicId(comicId: $comicId) {\n    messageId\n    up\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.DeleteMessage,
                "mutation deleteMessage($messageId: ID!) {\n  deleteMessage(messageId: $messageId)\n}"
            },
        };

    private static string FirstCharToLowerStringBuilder(this QueryDataEnum queryData)
    {
        var input = queryData.ToString();
        if (string.IsNullOrEmpty(input))
            return input;

        var sb = new StringBuilder(input.Length);
        sb.Append(char.ToLower(input[0]));
        sb.Append(input[1..]);
        return sb.ToString();
    }

    public static QueryData GetQueryData(this QueryDataEnum operationName)
    {
        if (OperationQueryDic.TryGetValue(operationName, out string? query))
        {
            return new QueryData(operationName.FirstCharToLowerStringBuilder(), query);
        }

        throw new Exception($"Operation :{operationName} not found");
    }

    public static QueryData<T> GetQueryDataWithVariables<T>(this QueryDataEnum operationName, T variables)
    {
        if (OperationQueryDic.TryGetValue(operationName, out string? query))
        {
            return new QueryData<T>(operationName.FirstCharToLowerStringBuilder(), query)
            {
                Variables = variables
            };
        }

        throw new Exception($"Operation :{operationName} not found");
    }

    #endregion
}

public class Pagination
{
    [JsonPropertyName("limit")] public int Limit { get; set; } = 20;

    [JsonPropertyName("offset")] public int Offset { get; set; }

    [JsonPropertyName("orderBy")] public string OrderBy { get; set; } = "";

    [JsonPropertyName("status")] public string Status { get; set; } = "";

    [JsonPropertyName("asc")] public bool Asc { get; set; }
}

public class PaginationVariables
{
    [JsonPropertyName("pagination")] public Pagination Pagination { get; set; } = new();
}

public class ComicIdVariables
{
    [JsonPropertyName("comicId")] public string ComicId { get; init; } = null!;
}

public class ComicIdsVariables
{
    [JsonPropertyName("comicIds")] public List<string> ComicIdList { get; init; } = null!;
}

public class ChapterIdVariables
{
    [JsonPropertyName("chapterId")] public string ChapterId { get; init; } = null!;
}

public class CategoryIdPaginationVariables : PaginationVariables
{
    [JsonPropertyName("categoryId")] public string CategoryId { get; set; } = "";
}

public class AuthorIdVariables
{
    [JsonPropertyName("authorId")] public string AuthorId { get; init; } = null!;
}

public class UpdateProfileImageVariables
{
    //"text": "a",
    [JsonPropertyName("text")] public string Text { get; set; } = "";

    //"textColor": "#F03F3F",
    [JsonPropertyName("textColor")] public string TextColor { get; set; } = "";

    //"backgroundColor": "#98C2EB"
    [JsonPropertyName("backgroundColor")] public string BackgroundColor { get; set; } = "";
}

public class NextChapterModeVariables
{
    [JsonPropertyName("mode")] public string NextChapterMode { get; set; } = "";
}

public class FavoritePagination : Pagination
{
    [JsonPropertyName("readProgress")] public string ReadProgress { get; set; } = "ALL";
}

public class FavoritePaginationVariables
{
    [JsonPropertyName("pagination")]
    public FavoritePagination Pagination { get; set; } = new FavoritePagination()
    {
        OrderBy = "COMIC_DATE_UPDATED",
    };
}

public class RecommendComicIdsPaginationVariables : PaginationVariables
{
    // "category": ""
    [JsonPropertyName("category")] public string Category { get; set; } = "";
}

public class KeyVariables
{
    [JsonPropertyName("key")] public string Key { get; set; }
}

public class FolderComicIdsVariables : PaginationVariables
{
    // "folderId": "8076",
    [JsonPropertyName("folderId")] public string FolderId { get; set; } = "";
}

public class FolderIdVariables
{
    // "folderId": "8076",
    [JsonPropertyName("folderId")] public string FolderId { get; set; } = "";
}

public class UpdateFolderNameVariables
{
    [JsonPropertyName("folderId")] public string FolderId { get; set; } = "";

    [JsonPropertyName("name")] public string Name { get; set; } = "";
}

public class FolderIdAndComicIdVariables
{
    // "folderId": "8076",
    [JsonPropertyName("folderId")] public string FolderId { get; set; } = "";

    // "folderId": "8076",
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = "";
}

public class AddMessageToComicVariables
{
    // "folderId": "8076",
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = "";

    [JsonPropertyName("message")] public string Message { get; set; } = "";

    [JsonPropertyName("replyToId")] public string ReplyToId { get; set; } = "";
}

public class MessageIdVariables
{
// "messageId": "4261"
    [JsonPropertyName("messageId")] public string MessageId { get; set; } = "";
}

public class ComicIdPaginationVariables : PaginationVariables
{
    [JsonPropertyName("comicId")] public string ComicId { get; init; } = null!;
}

public class VoteMessageVariables : MessageIdVariables
{
    [JsonPropertyName("up")] public bool Up { get; init; } = false;
}

public class AddReadComicHistoryVariables
{
    // "comicId": "2806",
    public string comicId { get; set; }
    //  "chapterId": "88432",
    public string chapterId { get; set; }
    //  "page": 0
    public int page { get; set; }
}