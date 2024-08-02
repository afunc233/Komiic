using System.Text;
using System.Text.Json.Serialization;

namespace Komiic.Core.Contracts.Model;

internal class ErrorInfo
{
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("path")] public List<string>? PathList { get; set; }
}

internal class ResponseData
{
    [JsonPropertyName("message")] public string? Message { get; set; }

    [JsonPropertyName("errors")] public List<ErrorInfo>? Errors { get; set; }
}

internal class ResponseData<T> : ResponseData
{
    [JsonPropertyName("data")] public T? Data { get; set; }
}

internal static class ResponseDataExt
{
    public static string? GetMessage(this ResponseData responseData)
    {
        if (string.IsNullOrWhiteSpace(responseData.Message) && responseData is { Errors.Count : > 0 })
        {
            return default;
        }

        var sb = new StringBuilder();

        if (responseData is { Message: not null })
        {
            sb.Append(responseData.Message);
            sb.Append('\n');
        }

        if (responseData is { Errors.Count : > 0 })
        {
            sb.Append(responseData.Message);
            sb.Append('\n');
        }

        return sb.ToString();
    }
}

#region 账户信息

internal class TokenResponseData : ResponseData
{
    [JsonPropertyName("token")] public string? Token { get; set; }

    [JsonPropertyName("code")] public long? Code { get; set; }

    [JsonPropertyName("expire")] public DateTime? Expire { get; set; }
}

internal class LogoutResponseData : ResponseData
{
    [JsonPropertyName("code")] public long? Code { get; set; }
}

public class AccountData
{
    [JsonPropertyName("account")] public Account Account { get; set; } = null!;
}

public class Account
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("email")] public string Email { get; set; } = null!;
    [JsonPropertyName("nickname")] public string Nickname { get; set; } = null!;
    [JsonPropertyName("dateCreated")] public string DateCreated { get; set; } = null!;
    [JsonPropertyName("favoriteComicIds")] public List<string> FavoriteComicIds { get; set; } = [];
    [JsonPropertyName("profileText")] public string ProfileText { get; set; } = null!;

    public string ProfileTextCalc
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ProfileText))
            {
                return Nickname.Length > 2 ? Nickname[..2] : Nickname;
            }

            return ProfileText;
        }
    }

    [JsonPropertyName("profileTextColor")] public string ProfileTextColor { get; set; } = null!;

    [JsonPropertyName("profileBackgroundColor")]
    public string ProfileBackgroundColor { get; set; } = null!;

    [JsonPropertyName("totalDonateAmount")]
    public int TotalDonateAmount { get; set; }

    [JsonPropertyName("monthDonateAmount")]
    public int MonthDonateAmount { get; set; }

    [JsonPropertyName("profileImageUrl")] public string ProfileImageUrl { get; set; } = null!;
    [JsonPropertyName("nextChapterMode")] public string NextChapterMode { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class FavoriteNewUpdatedData
{
    [JsonPropertyName("getLatestUpdatedDateInFavorite")]
    public DateTime GetLatestUpdatedDateInFavorite { get; set; }
}

#endregion

#region 首页

public class MangaInfo
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("title")] public string Title { get; set; } = null!;
    [JsonPropertyName("status")] public string Status { get; set; } = null!;
    [JsonPropertyName("year")] public int Year { get; set; }
    [JsonPropertyName("imageUrl")] public string ImageUrl { get; set; } = null!;
    [JsonPropertyName("authors")] public List<Author> Authors { get; set; } = null!;
    [JsonPropertyName("categories")] public List<Category> Categories { get; set; } = null!;
    [JsonPropertyName("dateUpdated")] public string DateUpdated { get; set; } = null!;
    [JsonPropertyName("monthViews")] public int MonthViews { get; set; }
    [JsonPropertyName("views")] public int Views { get; set; }
    [JsonPropertyName("favoriteCount")] public int FavoriteCount { get; set; }
    [JsonPropertyName("lastBookUpdate")] public string LastBookUpdate { get; set; } = null!;

    [JsonPropertyName("lastChapterUpdate")]
    public string LastChapterUpdate { get; set; } = null!;

    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
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
    [JsonPropertyName("limit")] public int Limit { get; set; }
    [JsonPropertyName("usage")] public int Usage { get; set; }
    [JsonPropertyName("resetInSeconds")] public string ResetInSeconds { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
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
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("serial")] public string Serial { get; set; } = null!;
    [JsonPropertyName("type")] public string Type { get; set; } = null!;
    [JsonPropertyName("dateCreated")] public string DateCreated { get; set; } = null!;
    [JsonPropertyName("dateUpdated")] public string DateUpdated { get; set; } = null!;
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
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
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("kid")] public string Kid { get; set; } = null!;
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

#endregion

#region 分类

public class AllCategoryData
{
    [JsonPropertyName("allCategory")] public List<Category>? AllCategories { get; set; }
}

public class Category
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
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
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("chName")] public string ChName { get; set; } = null!;
    [JsonPropertyName("enName")] public string EnName { get; set; } = null!;
    [JsonPropertyName("wikiLink")] public string WikiLink { get; set; } = null!;
    [JsonPropertyName("comicCount")] public int ComicCount { get; set; }
    [JsonPropertyName("views")] public int Views { get; set; }
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
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
    [JsonPropertyName("addFavorite")]
    // "removeFavorite": true
    public Favorite AddFavorite { get; set; } = null!;
}

public class RemoveFavoriteData
{
    [JsonPropertyName("removeFavorite")]
    // "removeFavorite": true
    public bool RemoveFavorite { get; set; }
}

public class FavoriteData
{
    [JsonPropertyName("getLatestUpdatedDateInFavorite")]
    public string GetLatestUpdatedDateInFavorite { get; set; } = null!;

    [JsonPropertyName("favoritesV2")] public List<Favorite> FavoritesV2 { get; set; } = null!;
}

public class Favorite
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = null!;
    [JsonPropertyName("dateAdded")] public string DateAdded { get; set; } = null!;
    [JsonPropertyName("lastAccess")] public string LastAccess { get; set; } = null!;
    [JsonPropertyName("bookReadProgress")] public string BookReadProgress { get; set; } = null!;

    [JsonPropertyName("chapterReadProgress")]
    public string ChapterReadProgress { get; set; } = null!;

    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class LastReadByComicIdData
{
    [JsonPropertyName("lastReadByComicIds")]
    public List<LastReadByComicId> LastReadByComicIds { get; set; } = null!;
}

public class LastReadByComicId
{
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = null!;
    [JsonPropertyName("book")] public BookOrChapter? Book { get; set; }
    [JsonPropertyName("chapter")] public BookOrChapter? Chapter { get; set; }

    public BookOrChapter BookOrChapter => Book ?? Chapter ?? new();

    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class BookOrChapter
{
// "page": 3,
    [JsonPropertyName("page")] public int Page { get; set; }

// "chapterId": "35162",
    [JsonPropertyName("chapterId")] public string ChapterId { get; set; } = null!;

// "serial": "02",
    [JsonPropertyName("serial")] public string Serial { get; set; } = null!;

// "__typename": "ComicLastRead"
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

#endregion

#region 推薦

public class RecommendComicIdsData
{
    [JsonPropertyName("recommendComicIds")]
    public List<string> RecommendComicIds { get; set; } = null!;
}

#endregion

#region 書櫃

public class FolderData
{
    [JsonPropertyName("folders")] public List<Folder> Folders { get; set; } = null!;
}

public class Folder
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("key")] public string Key { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("views")] public int Views { get; set; }
    [JsonPropertyName("comicCount")] public int ComicCount { get; set; }
    [JsonPropertyName("dateCreated")] public string DateCreated { get; set; } = null!;
    [JsonPropertyName("dateUpdated")] public string DateUpdated { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class FolderByKeyData
{
    [JsonPropertyName("folder")] public FolderByKey Folder { get; set; } = null!;
}

public class FolderByKey
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("key")] public string Key { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("views")] public int Views { get; set; }
    [JsonPropertyName("dateCreated")] public string DateCreated { get; set; } = null!;
    [JsonPropertyName("dateUpdated")] public string DateUpdated { get; set; } = null!;
    [JsonPropertyName("comicCount")] public int ComicCount { get; set; }
    [JsonPropertyName("account")] public FolderAccount Account { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class FolderAccount
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("nickname")] public string Nickname { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class FolderComicIdsData
{
    [JsonPropertyName("folderId")] public string FolderId { get; set; } = null!;
    [JsonPropertyName("key")] public string Key { get; set; } = null!;
    [JsonPropertyName("comicIds")] public List<string> ComicIds { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class UpdateFolderNameData
{
    [JsonPropertyName("updateFolderName")]
    // "updateFolderName": true
    public bool UpdateFolderName { get; set; }
}

public class RemoveFolderData
{
    [JsonPropertyName("removeFolder")]
    // "updateFolderName": true
    public bool RemoveFolder { get; set; }
}

public class RemoveComicToFolderData
{
    [JsonPropertyName("removeComicToFolder")]
    // "updateFolderName": true
    public bool RemoveComicToFolder { get; set; }
}

public class AddComicToFolderData
{
    [JsonPropertyName("addComicToFolder")]
    // "updateFolderName": true
    public bool AddComicToFolder { get; set; }
}

public class ComicInAccountFoldersData
{
    [JsonPropertyName("comicInAccountFolders")]
    // "updateFolderName": true
    public List<string> ComicInAccountFolders { get; set; } = null!;
}

public class AddMessageToComicData
{
    [JsonPropertyName("addMessageToComic")]
    public MessagesByComicId AddMessageToComic { get; set; } = null!;
}



public class MessageChanData
{
    [JsonPropertyName("messageChan")] public List<MessageChan> MessageChan { get; set; } = null!;
}

public class MessageChan
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = null!;
    [JsonPropertyName("account")] public Account Account { get; set; } = null!;
    [JsonPropertyName("message")] public string Message { get; set; } = null!;
    [JsonPropertyName("replyTo")] public object ReplyTo { get; set; } = null!;
    [JsonPropertyName("upCount")] public int UpCount { get; set; }
    [JsonPropertyName("downCount")] public int DownCount { get; set; }
    [JsonPropertyName("dateUpdated")] public string DateUpdated { get; set; } = null!;
    [JsonPropertyName("dateCreated")] public string DateCreated { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class MessageCountByComicIdData
{
    [JsonPropertyName("messageCountByComicId")]
    public int? MessageCountByComicIdCount { get; set; }
}

public class LastMessageByComicIdData
{
    [JsonPropertyName("lastMessageByComicId")]
    public LastMessageByComicId LastMessageByComicId { get; set; } = null!;
}

public class LastMessageByComicId
{
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = null!;
    [JsonPropertyName("message")] public string Message { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class MessagesByComicIdData
{
    [JsonPropertyName("getMessagesByComicId")]
    public List<MessagesByComicId> GetMessagesByComicId { get; set; } = null!;
}

public class MessagesByComicId
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = null!;
    [JsonPropertyName("account")] public Account Account { get; set; } = null!;
    [JsonPropertyName("message")] public string Message { get; set; } = null!;
    [JsonPropertyName("replyTo")] public ReplyTo ReplyTo { get; set; } = null!;
    [JsonPropertyName("upCount")] public int UpCount { get; set; }
    [JsonPropertyName("downCount")] public int DownCount { get; set; }
    [JsonPropertyName("dateUpdated")] public string DateUpdated { get; set; } = null!;
    [JsonPropertyName("dateCreated")] public string DateCreated { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class ReplyTo
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("message")] public string Message { get; set; } = null!;
    [JsonPropertyName("account")] public Account Account { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class VoteMessageData
{
    [JsonPropertyName("voteMessage")] public bool VoteMessage { get; set; }
}

public class MessageVotesByComicIdData
{
    [JsonPropertyName("messageVotesByComicId")]
    public List<MessageVotesByComicId> MessageVotesByComicId { get; set; } = null!;
}

public class MessageVotesByComicId
{
    [JsonPropertyName("messageId")] public string MessageId { get; set; } = null!;
    [JsonPropertyName("up")] public bool Up { get; set; }
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class DeleteMessageData
{
    [JsonPropertyName("deleteMessage")] public bool DeleteMessage { get; set; }
}

#endregion

#region 歷史

public class ReadComicHistoryData
{
    [JsonPropertyName("readComicHistory")] public List<ComicHistory> ReadComicHistory { get; set; } = null!;
}

public class ComicHistory
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("comicId")] public string ComicId { get; set; } = null!;
    [JsonPropertyName("chapters")] public List<Chapters> Chapters { get; set; } = null!;
    [JsonPropertyName("startDate")] public string StartDate { get; set; } = null!;
    [JsonPropertyName("lastDate")] public string LastDate { get; set; } = null!;
    [JsonPropertyName("chapterType")] public string ChapterType { get; set; } = null!;
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class Chapters
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("chapterId")] public string ChapterId { get; set; } = null!;
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("__typename")] public string Typename { get; set; } = null!;
}

public class ReadComicHistoryByIdData
{
    [JsonPropertyName("readComicHistoryById")]
    public ComicHistory ReadComicHistoryById { get; set; } = null!;
}

public class AddReadComicHistoryData
{
    [JsonPropertyName("addReadComicHistory")]
    public ComicHistory AddReadComicHistory { get; set; } = null!;
}

#endregion