using Komiic.Core.Contracts.Model;
using Refit;

namespace Komiic.Core.Contracts.Api;

[Headers("Authorization: Bearer", KomiicConst.EnableCacheHeader + ":" + KomiicConst.Minute15)]
public interface IKomiicQueryApi
{
    #region 首页

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RecentUpdateData>> GetRecentUpdate([Body] QueryData<PaginationVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<HotComicsData>> GetHotComics([Body] QueryData<PaginationVariables> queryData);

    #endregion

    #region 漫画详情

    /// <summary>
    /// 根据id 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicByIdData>> GetMangaInfoById([Body] QueryData<ComicIdVariables> queryData);

    /// <summary>
    /// 根据id 列表 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicByIdsData>> GetMangaInfoByIds([Body] QueryData<ComicIdsVariables> queryData);

    /// <summary>
    /// 根据id 获取推荐漫画id
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RecommendComicByIdData>> GetRecommendComicById([Body] QueryData<ComicIdVariables> queryData);

    /// <summary>
    /// 根据ID 获取章节信息
    /// </summary>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ChaptersByComicIdData>> GetChapterByComicId([Body] QueryData<ComicIdVariables> queryData);

    #endregion

    #region 漫画内容

    /// <summary>
    /// 根据章节获取图片
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ImagesByChapterIdData>> GetImagesByChapterId([Body] QueryData<ChapterIdVariables> queryData);

    #endregion

    #region 分类

    /// <summary>
    /// 獲取分類
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AllCategoryData>> GetAllCategory([Body] QueryData queryData);

    /// <summary>
    /// 根據分類獲取漫画
    /// </summary>
    /// <param name="getQueryDataWithVariables"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicByCategoryData>> GetComicByCategory(
        [Body] QueryData<CategoryIdPaginationVariables> getQueryDataWithVariables);

    #endregion

    #region 作者

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AllAuthorsData>> GetAllAuthors([Body] QueryData<PaginationVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicsByAuthorData>> GetComicsByAuthor([Body] QueryData<AuthorIdVariables> queryData);

    #endregion

    #region 喜愛

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AddFavoriteData>> AddFavorite([Body] QueryData<ComicIdVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RemoveFavoriteData>> RemoveFavorite([Body] QueryData<ComicIdVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FavoriteData>> GetFavorites([Body] QueryData<FavoritePaginationVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<LastReadByComicIdData>> GetComicsLastRead([Body] QueryData<ComicIdsVariables> queryData);

    #endregion

    #region 推薦

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RecommendComicIdsData>> GetRecommendComicIds(
        [Body] QueryData<RecommendComicIdsPaginationVariables> queryData);

    #endregion

    #region 書櫃

    /// <summary>
    /// 獲取書櫃圖片 https://komiic.com/api/folder/image/IzSzen1J
    /// IzSzen1J : folderKey
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FolderData>> GetMyFolder([Body] QueryData queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FolderByKeyData>> GetFolderByKey([Body] QueryData<KeyVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FolderComicIdsData>> GetFolderComicIds([Body] QueryData<FolderComicIdsVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<UpdateFolderNameData>> UpdateFolderName([Body] QueryData<UpdateFolderNameVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RemoveFolderData>> RemoveFolder([Body] QueryData<FolderIdVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AddComicToFolderData>> AddComicToFolder([Body] QueryData<FolderIdAndComicIdVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicInAccountFoldersData>> ComicInAccountFolders([Body] QueryData<ComicIdVariables> queryData);

    #endregion

    #region 評論/回復

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AddMessageToComicData>> AddMessageToComic([Body] QueryData<AddMessageToComicVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<MessageChanData>> GetMessageChan([Body] QueryData<MessageIdVariables> queryData);

    /// <summary>
    /// 获取留言数量
    /// </summary>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<MessageCountByComicIdData>>
        GetMessageCountByComicId([Body] QueryData<ComicIdVariables> queryData);

    /// <summary>
    /// 最后一条留言
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<LastMessageByComicIdData>> GetLastMessageByComicId([Body] QueryData<ComicIdVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<MessagesByComicIdData>> GetMessagesByComicId(
        [Body] QueryData<ComicIdPaginationVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<VoteMessageData>> VoteMessage([Body] QueryData<VoteMessageVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<MessageVotesByComicIdData>> MessageVotesByComicId([Body] QueryData<ComicIdsVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<DeleteMessageData>> DeleteMessage([Body] QueryData<MessageIdVariables> queryData);

    #endregion

    #region 歷史

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ReadComicHistoryData>> ReadComicHistory([Body] QueryData<PaginationVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second10)]
    Task<ResponseData<ReadComicHistoryByIdData>> ReadComicHistoryById([Body] QueryData<ComicIdVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second5)]
    Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
        [Body] QueryData<AddReadComicHistoryVariables> queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AddReadComicHistoryData>> DeleteComicReadHistory(
        [Body] QueryData<AddReadComicHistoryVariables> queryData);

    #endregion
}