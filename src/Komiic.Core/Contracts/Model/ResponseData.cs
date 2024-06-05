using System.Text.Json.Serialization;

namespace Komiic.Core.Contracts.Model;

// TODO 优化这里，取消 pragma
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
// ReSharper disable InconsistentNaming
public class ErrorInfo
{
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("path")] public List<string>? PathList { get; set; }
}

public class ResponseData
{
    [JsonPropertyName("message")] public string? Message { get; set; }

    [JsonPropertyName("errors")] public List<ErrorInfo>? Errors { get; set; }
}

public class ResponseData<T> : ResponseData
{
    [JsonPropertyName("data")] public T? Data { get; set; }
}

#region 账户信息

public class TokenResponseData : ResponseData
{
    [JsonPropertyName("token")] public string? Token { get; set; }
}

public class AccountData
{
    [JsonPropertyName("account")] public Account Account { get; set; }
}

public class Account
{
    public string id { get; set; }
    public string email { get; set; }
    public string nickname { get; set; }
    public string dateCreated { get; set; }
    public object[] favoriteComicIds { get; set; }
    public string profileText { get; set; }
    public string profileTextColor { get; set; }
    public string profileBackgroundColor { get; set; }
    public int totalDonateAmount { get; set; }
    public int monthDonateAmount { get; set; }
    public string profileImageUrl { get; set; }
    public string nextChapterMode { get; set; }
    public string __typename { get; set; }
}

public class FavoriteNewUpdatedData
{
    [JsonPropertyName("getLatestUpdatedDateInFavorite")]
    public DateTime GetLatestUpdatedDateInFavorite { get; set; }
}

#endregion

#region 首页

public class AuthorSimple
{
    public string id { get; set; }
    public string name { get; set; }
    public string __typename { get; set; }
}

public class Categories
{
    public string id { get; set; }
    public string name { get; set; }
    public string __typename { get; set; }
}

public class MangaInfo
{
    public string id { get; set; }
    public string title { get; set; }
    public string status { get; set; }
    public int year { get; set; }
    public string imageUrl { get; set; }
    public List<AuthorSimple> authors { get; set; }
    public List<Categories> categories { get; set; }
    public string dateUpdated { get; set; }
    public int monthViews { get; set; }
    public int views { get; set; }
    public int favoriteCount { get; set; }
    public string lastBookUpdate { get; set; }
    public string lastChapterUpdate { get; set; }
    public string __typename { get; set; }
}

public class RecentUpdateData
{
    [JsonPropertyName("recentUpdate")] public List<MangaInfo> RecentUpdate { get; set; } = null!;
}

public class HotComicsData
{
    [JsonPropertyName("hotComics")] public List<MangaInfo> HotComics { get; set; } = null!;
}

#endregion

#region 限制

public class ImageLimit
{
    public int limit { get; set; }
    public int usage { get; set; }
    public string resetInSeconds { get; set; }
    public string __typename { get; set; }
}

public class GetImageLimitData
{
    [JsonPropertyName("getImageLimit")] public ImageLimit ImageLimit { get; set; } = null!;
}

#endregion

#region 漫画详情

public class ComicByIdData
{
    [JsonPropertyName("comicById")] public MangaInfo ComicById { get; set; } = null!;
}

public class ComicByIdsData
{
    [JsonPropertyName("comicByIds")] public List<MangaInfo> ComicByIds { get; set; } = null!;
}

public class RecommendComicByIdData
{
    [JsonPropertyName("recommendComicById")]
    public List<string> RecommendComicList { get; set; } = null!;
}

public class ChaptersByComicIdData
{
    [JsonPropertyName("chaptersByComicId")]
    public List<ChaptersByComicId> ChaptersByComicIdList { get; set; } = null!;
}

public class ChaptersByComicId
{
    public string id { get; set; }
    public string serial { get; set; }
    public string type { get; set; }
    public string dateCreated { get; set; }
    public string dateUpdated { get; set; }
    public int size { get; set; }
    public string __typename { get; set; }
}

#endregion

#region MyRegion

public class ImagesByChapterIdData
{
    [JsonPropertyName("imagesByChapterId")]
    public List<ImagesByChapterId> ImagesByChapterIdList { get; set; } = null!;
}

public class ImagesByChapterId
{
    public string id { get; set; }
    public string kid { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public string __typename { get; set; }
}

#endregion

#region 分类

public class AllCategoryData
{
    [JsonPropertyName("allCategory")] public List<Category>? AllCategories { get; set; }
}

public class Category
{
    public string id { get; set; }
    public string name { get; set; }
    public string __typename { get; set; }
}

public class ComicByCategoryData
{
    [JsonPropertyName("comicByCategory")] public List<MangaInfo> ComicByCategories { get; set; } = [];
}

#endregion

#region 作者

public class AllAuthorsData
{
    [JsonPropertyName("authors")] public List<Author> AuthorList { get; set; } = [];
}

public class Author
{
    public string id { get; set; }
    public string name { get; set; }
    public string chName { get; set; }
    public string enName { get; set; }
    public string wikiLink { get; set; }
    public int comicCount { get; set; }
    public int views { get; set; }
    public string __typename { get; set; }
}

public class ComicsByAuthorData
{
    [JsonPropertyName("getComicsByAuthor")]
    public List<MangaInfo> ComicsByAuthorList { get; set; } = [];
}

#endregion

#region updateProfileImage

public class UpdateProfileImageData
{
    [JsonPropertyName("updateProfileImage")]
    public bool UpdateProfileImage { get; set; }
}

#endregion

#region SetNextChapterMode

public class SetNextChapterModeData
{
    // "setNextChapterMode": true
    [JsonPropertyName("setNextChapterMode")]
    public bool SetNextChapterMode { get; set; }
}

#endregion

#region Favorite

public class AddFavoriteData
{
    // "removeFavorite": true
    public Favorite addFavorite { get; set; }
}

public class RemoveFavoriteData
{
    // "removeFavorite": true
    public bool removeFavorite { get; set; }
}

public class FavoriteData
{
    public string getLatestUpdatedDateInFavorite { get; set; }
    public List<Favorite> favoritesV2 { get; set; }
}

public class Favorite
{
    public string id { get; set; }
    public string comicId { get; set; }
    public string dateAdded { get; set; }
    public string lastAccess { get; set; }
    public string bookReadProgress { get; set; }
    public string chapterReadProgress { get; set; }
    public string __typename { get; set; }
}

public class LastReadByComicIdData
{
    public List<LastReadByComicId> lastReadByComicIds { get; set; }
}

public class LastReadByComicId
{
    public string comicId { get; set; }
    public object book { get; set; }
    public object chapter { get; set; }
    public string __typename { get; set; }
}

#endregion

#region 推薦

public class RecommendComicIdsData
{
    [JsonPropertyName("recommendComicIds")]
    public List<string> recommendComicIds { get; set; }
}

#endregion

#region 書櫃

public class FolderData
{
    public List<Folder> folders { get; set; }
}

public class Folder
{
    public string id { get; set; }
    public string key { get; set; }
    public string name { get; set; }
    public int views { get; set; }
    public int comicCount { get; set; }
    public string dateCreated { get; set; }
    public string dateUpdated { get; set; }
    public string __typename { get; set; }
}

public class FolderByKeyData
{
    public FolderByKey folder { get; set; }
}

public class FolderByKey
{
    public string id { get; set; }
    public string key { get; set; }
    public string name { get; set; }
    public int views { get; set; }
    public string dateCreated { get; set; }
    public string dateUpdated { get; set; }
    public int comicCount { get; set; }
    public FolderAccount account { get; set; }
    public string __typename { get; set; }
}

public class FolderAccount
{
    public string id { get; set; }
    public string nickname { get; set; }
    public string __typename { get; set; }
}

public class FolderComicIdsData
{
    public string folderId { get; set; }
    public string key { get; set; }
    public List<string> comicIds { get; set; }
    public string __typename { get; set; }
}

public class UpdateFolderNameData
{
    // "updateFolderName": true
    public bool updateFolderName { get; set; }
}

public class RemoveFolderData
{
    // "updateFolderName": true
    public bool removeFolder { get; set; }
}

public class AddComicToFolderData
{
    // "updateFolderName": true
    public bool addComicToFolder { get; set; }
}

public class ComicInAccountFoldersData
{
    // "updateFolderName": true
    public List<string> comicInAccountFolders { get; set; }
}

public class AddMessageToComicData
{
    public AddMessageToComic addMessageToComic { get; set; }
}

public class AddMessageToComic
{
    public string id { get; set; }
    public string message { get; set; }
    public string comicId { get; set; }
    public Account account { get; set; }
    public ReplyTo replyTo { get; set; }
    public string dateCreated { get; set; }
    public string dateUpdated { get; set; }
    public string __typename { get; set; }
}

public class ReplyTo
{
    public string id { get; set; }
    public string message { get; set; }
    public Account account { get; set; }
    public string __typename { get; set; }
}

public class MessageChanData
{
    public List<MessageChan> messageChan { get; set; }
}

public class MessageChan
{
    public string id { get; set; }
    public string comicId { get; set; }
    public Account account { get; set; }
    public string message { get; set; }
    public object replyTo { get; set; }
    public int upCount { get; set; }
    public int downCount { get; set; }
    public string dateUpdated { get; set; }
    public string dateCreated { get; set; }
    public string __typename { get; set; }
}

public class MessageCountByComicIdData
{
    [JsonPropertyName("messageCountByComicId")]
    public int MessageCountByComicIdCount { get; set; }
}

public class LastMessageByComicIdData
{
    [JsonPropertyName("lastMessageByComicId")]
    public LastMessageByComicId LastMessageByComicId { get; set; }
}

public class LastMessageByComicId
{
    public string comicId { get; set; }
    public string message { get; set; }
    public string __typename { get; set; }
}

public class MessagesByComicIdData
{
    public List<MessagesByComicId> getMessagesByComicId { get; set; }
}

public class MessagesByComicId
{
    public string id { get; set; }
    public string comicId { get; set; }
    public Account account { get; set; }
    public string message { get; set; }
    public ReplyTo replyTo { get; set; }
    public int upCount { get; set; }
    public int downCount { get; set; }
    public string dateUpdated { get; set; }
    public string dateCreated { get; set; }
    public string __typename { get; set; }
}

public class VoteMessageData
{
    public bool voteMessage { get; set; }
}

public class MessageVotesByComicIdData
{
    public List<MessageVotesByComicId> messageVotesByComicId { get; set; }
}

public class MessageVotesByComicId
{
    public string messageId { get; set; }
    public bool up { get; set; }
    public string __typename { get; set; }
}

public class DeleteMessageData
{
    public bool deleteMessage { get; set; }
}
#endregion

#region 歷史

public class ReadComicHistoryData
{
    public List<ComicHistory> readComicHistory { get; set; }
}

public class ComicHistory
{
    public string id { get; set; }
    public string comicId { get; set; }
    public List<Chapters> chapters { get; set; }
    public string startDate { get; set; }
    public string lastDate { get; set; }
    public string chapterType { get; set; }
    public string __typename { get; set; }
}

public class Chapters
{
    public string id { get; set; }
    public string chapterId { get; set; }
    public int page { get; set; }
    public string __typename { get; set; }
}

public class ReadComicHistoryByIdData
{
    public ComicHistory readComicHistoryById { get; set; }
}
public class AddReadComicHistoryData
{
    public ComicHistory addReadComicHistory { get; set; }
}

#endregion

// ReSharper restore InconsistentNaming
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。