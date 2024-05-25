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
    MessageCountByComicId,
    LastMessageByComicId,
    ChapterByComicId,
    GetMessagesByComicId,
    ImagesByChapterId,
    AccountQuery,
    FavoriteNewUpdatedQuery,
    UpdateProfileImage,
    SetNextChapterMode,
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
                // "hotComics",    
                QueryDataEnum.HotComics,
                "query hotComics($pagination: Pagination!) {\n  hotComics(pagination: $pagination) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },

            {
                QueryDataEnum.GetImageLimit,
                // "getImageLimit",
                "query getImageLimit {\n  getImageLimit {\n    limit\n    usage\n    resetInSeconds\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.AllCategory,
                //"allCategory",
                "query allCategory {\n  allCategory {\n    id\n    name\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicByCategory,
                // "comicByCategory",
                "query comicByCategory($categoryId: ID!, $pagination: Pagination!) {\n  comicByCategory(categoryId: $categoryId, pagination: $pagination) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.Authors,
                // "authors",
                "query authors($pagination: Pagination!) {\n  authors(pagination: $pagination) {\n    id\n    name\n    chName\n    enName\n    wikiLink\n    comicCount\n    views\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicsByAuthor,
                // "comicsByAuthor",
                "query comicsByAuthor($authorId: ID!) {\n  getComicsByAuthor(authorId: $authorId) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicByIds,
                // "comicByIds",
                "query comicByIds($comicIds: [ID]!) {\n  comicByIds(comicIds: $comicIds) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateUpdated\n    monthViews\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ComicById,
                // "comicById",
                "query comicById($comicId: ID!) {\n  comicById(comicId: $comicId) {\n    id\n    title\n    status\n    year\n    imageUrl\n    authors {\n      id\n      name\n      __typename\n    }\n    categories {\n      id\n      name\n      __typename\n    }\n    dateCreated\n    dateUpdated\n    views\n    favoriteCount\n    lastBookUpdate\n    lastChapterUpdate\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.RecommendComicById,
                // "recommendComicById",
                "query recommendComicById($comicId: ID!) {\n  recommendComicById(comicId: $comicId)\n}"
            },
            {
                QueryDataEnum.MessageCountByComicId,
                // "messageCountByComicId",
                "query messageCountByComicId($comicId: ID!) {\n  messageCountByComicId(comicId: $comicId)\n}"
            },
            {
                QueryDataEnum.LastMessageByComicId,
                // "lastMessageByComicId",
                "query lastMessageByComicId($comicId: ID!) {\n  lastMessageByComicId(comicId: $comicId) {\n    comicId\n    message\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ChapterByComicId,
                // "chapterByComicId",
                "query chapterByComicId($comicId: ID!) {\n  chaptersByComicId(comicId: $comicId) {\n    id\n    serial\n    type\n    dateCreated\n    dateUpdated\n    size\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.GetMessagesByComicId,
                // "getMessagesByComicId",
                "query getMessagesByComicId($comicId: ID!, $pagination: Pagination!) {\n  getMessagesByComicId(comicId: $comicId, pagination: $pagination) {\n    id\n    comicId\n    account {\n      id\n      nickname\n      profileText\n      profileTextColor\n      profileBackgroundColor\n      profileImageUrl\n      __typename\n    }\n    message\n    replyTo {\n      id\n      message\n      account {\n        id\n        nickname\n        profileText\n        profileTextColor\n        profileBackgroundColor\n        profileImageUrl\n        __typename\n      }\n      __typename\n    }\n    upCount\n    downCount\n    dateUpdated\n    dateCreated\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.ImagesByChapterId,
                // "imagesByChapterId",
                "query imagesByChapterId($chapterId: ID!) {\n  imagesByChapterId(chapterId: $chapterId) {\n    id\n    kid\n    height\n    width\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.AccountQuery,
                // "accountQuery",
                "query accountQuery {\n  account {\n    id\n    email\n    nickname\n    dateCreated\n    favoriteComicIds\n    profileText\n    profileTextColor\n    profileBackgroundColor\n    totalDonateAmount\n    monthDonateAmount\n    profileImageUrl\n    nextChapterMode\n    __typename\n  }\n}"
            },
            {
                QueryDataEnum.FavoriteNewUpdatedQuery,
                //"favoriteNewUpdatedQuery",
                "query favoriteNewUpdatedQuery {\n  getLatestUpdatedDateInFavorite\n}"
            },
            {
                QueryDataEnum.UpdateProfileImage,
                //"favoriteNewUpdatedQuery",
                "mutation updateProfileImage($text: String!, $textColor: String!, $backgroundColor: String!) {\n  updateProfileImage(\n    text: $text\n    textColor: $textColor\n    backgroundColor: $backgroundColor\n  )\n}"
            },
            {
                QueryDataEnum.SetNextChapterMode,
                //"favoriteNewUpdatedQuery",
                "mutation setNextChapterMode($mode: String!) {\n  setNextChapterMode(mode: $mode)\n}"
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

public class  NextChapterModeVariables
{
    [JsonPropertyName("mode")] public string NextChapterMode { get; set; } = "";
}