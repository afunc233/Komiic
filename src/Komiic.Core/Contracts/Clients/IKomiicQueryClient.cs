using Komiic.Core.Contracts.Models;
using Refit;

namespace Komiic.Core.Contracts.Clients;

[Headers("Authorization: Bearer", KomiicConst.EnableCacheHeader + ":" + KomiicConst.Minute30)]
internal interface IKomiicQueryClient : IDisposable
{
    #region 漫画内容

    /// <summary>
    ///     根据章节获取图片
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ImagesByChapterIdData>> GetImagesByChapterId([Body] QueryData<ChapterIdVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 推薦

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RecommendComicIdsData>> GetRecommendComicIds(
        [Body] QueryData<RecommendComicIdsPaginationVariables> queryData, CancellationToken cancellationToken);

    #endregion

    #region 首页

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RecentUpdateData>> GetRecentUpdate([Body] QueryData<PaginationVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<HotComicsData>> GetHotComics([Body] QueryData<PaginationVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 漫画详情

    /// <summary>
    ///     根据id 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicByIdData>> GetMangaInfoById([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    /// <summary>
    ///     根据id 列表 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicByIdsData>> GetMangaInfoByIds([Body] QueryData<ComicIdsVariables> queryData,
        CancellationToken cancellationToken);

    /// <summary>
    ///     根据id 获取推荐漫画id
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RecommendComicByIdData>> GetRecommendComicById([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    /// <summary>
    ///     根据ID 获取章节信息
    /// </summary>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ChaptersByComicIdData>> GetChapterByComicId([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 分类

    /// <summary>
    ///     獲取分類
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AllCategoryData>> GetAllCategory([Body] QueryData queryData,
        CancellationToken cancellationToken);

    /// <summary>
    ///     根據分類獲取漫画
    /// </summary>
    /// <param name="getQueryDataWithVariables"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicByCategoryData>> GetComicByCategory(
        [Body] QueryData<CategoryIdPaginationVariables> getQueryDataWithVariables,
        CancellationToken cancellationToken);

    #endregion

    #region 作者

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AllAuthorsData>> GetAllAuthors([Body] QueryData<PaginationVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ComicsByAuthorData>> GetComicsByAuthor([Body] QueryData<AuthorIdVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 喜愛

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AddFavoriteData>> AddFavorite([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RemoveFavoriteData>> RemoveFavorite([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FavoriteData>> GetFavorites([Body] QueryData<FavoritePaginationVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers("Authorization: Bearer", KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<LastReadByComicIdData>> GetComicsLastRead([Body] QueryData<ComicIdsVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 書櫃

    /// <summary>
    ///     獲取書櫃圖片 https://komiic.com/api/folder/image/IzSzen1J
    ///     IzSzen1J : folderKey
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<FolderData>> GetMyFolder([Body] QueryData queryData, CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FolderByKeyData>> GetFolderByKey([Body] QueryData<KeyVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FolderComicIdsData>> GetFolderComicIds([Body] QueryData<FolderComicIdsVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<UpdateFolderNameData>> UpdateFolderName([Body] QueryData<UpdateFolderNameVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<RemoveFolderData>> RemoveFolder([Body] QueryData<FolderIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<RemoveComicToFolderData>> RemoveComicToFolder(
        [Body] QueryData<FolderIdAndComicIdVariables> queryData, CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<AddComicToFolderData>> AddComicToFolder([Body] QueryData<FolderIdAndComicIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<ComicInAccountFoldersData>> ComicInAccountFolders([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 評論/回復

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<AddMessageToComicData>> AddMessageToComic([Body] QueryData<AddMessageToComicVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<MessageChanData>> GetMessageChan([Body] QueryData<MessageIdVariables> queryData,
        CancellationToken cancellationToken);

    /// <summary>
    ///     获取留言数量
    /// </summary>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<MessageCountByComicIdData>>
        GetMessageCountByComicId([Body] QueryData<ComicIdVariables> queryData,
            CancellationToken cancellationToken);

    /// <summary>
    ///     最后一条留言
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<LastMessageByComicIdData>> GetLastMessageByComicId([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second10)]
    Task<ResponseData<MessagesByComicIdData>> GetMessagesByComicId(
        [Body] QueryData<ComicIdPaginationVariables> queryData, CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.DisableCache)]
    Task<ResponseData<VoteMessageData>> VoteMessage([Body] QueryData<VoteMessageVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second10)]
    Task<ResponseData<MessageVotesByComicIdData>> MessageVotesByComicId([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<DeleteMessageData>> DeleteMessage([Body] QueryData<MessageIdVariables> queryData,
        CancellationToken cancellationToken);

    #endregion

    #region 歷史

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<ReadComicHistoryData>> ReadComicHistory([Body] QueryData<PaginationVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second10)]
    Task<ResponseData<ReadComicHistoryByIdData>> ReadComicHistoryById([Body] QueryData<ComicIdVariables> queryData,
        CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second5)]
    Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
        [Body] QueryData<AddReadComicHistoryVariables> queryData, CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AddReadComicHistoryData>> DeleteComicReadHistory(
        [Body] QueryData<AddReadComicHistoryVariables> queryData, CancellationToken cancellationToken);

    #endregion
}