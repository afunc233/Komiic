using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Apis;

internal interface IKomiicQueryApi 
{
    #region 漫画内容

    /// <summary>
    ///     根据章节获取图片
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<ImagesByChapterIdData>> GetImagesByChapterId(QueryData<ChapterIdVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 推薦

    Task<ResponseData<RecommendComicIdsData>> GetRecommendComicIds(
        QueryData<RecommendComicIdsPaginationVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 首页

    Task<ResponseData<RecentUpdateData>> GetRecentUpdate(QueryData<PaginationVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<HotComicsData>> GetHotComics(QueryData<PaginationVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 漫画详情

    /// <summary>
    ///     根据id 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<ComicByIdData>> GetMangaInfoById(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);

    /// <summary>
    ///     根据id 列表 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<ComicByIdsData>> GetMangaInfoByIds(QueryData<ComicIdsVariables> queryData,CancellationToken? cancellationToken = null);

    /// <summary>
    ///     根据id 获取推荐漫画id
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<RecommendComicByIdData>> GetRecommendComicById(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);

    /// <summary>
    ///     根据ID 获取章节信息
    /// </summary>
    /// <returns></returns>
    Task<ResponseData<ChaptersByComicIdData>> GetChapterByComicId(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 分类

    /// <summary>
    ///     獲取分類
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<AllCategoryData>> GetAllCategory(QueryData queryData,CancellationToken? cancellationToken = null);

    /// <summary>
    ///     根據分類獲取漫画
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<ComicByCategoryData>> GetComicByCategory(
        QueryData<CategoryIdPaginationVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 作者

    Task<ResponseData<AllAuthorsData>> GetAllAuthors(QueryData<PaginationVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<ComicsByAuthorData>> GetComicsByAuthor(QueryData<AuthorIdVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 喜愛

    Task<ResponseData<AddFavoriteData>> AddFavorite(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<RemoveFavoriteData>> RemoveFavorite(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<FavoriteData>> GetFavorites(QueryData<FavoritePaginationVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<LastReadByComicIdData>> GetComicsLastRead(QueryData<ComicIdsVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 書櫃

    /// <summary>
    ///     獲取書櫃圖片 https://komiic.com/api/folder/image/IzSzen1J
    ///     IzSzen1J : folderKey
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<FolderData>> GetMyFolder(QueryData queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<FolderByKeyData>> GetFolderByKey(QueryData<KeyVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<FolderComicIdsData>> GetFolderComicIds(QueryData<FolderComicIdsVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<UpdateFolderNameData>> UpdateFolderName(QueryData<UpdateFolderNameVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<RemoveFolderData>> RemoveFolder(QueryData<FolderIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<RemoveComicToFolderData>> RemoveComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<AddComicToFolderData>> AddComicToFolder(QueryData<FolderIdAndComicIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<ComicInAccountFoldersData>> ComicInAccountFolders(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 評論/回復

    Task<ResponseData<AddMessageToComicData>> AddMessageToComic(QueryData<AddMessageToComicVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<MessageChanData>> GetMessageChan(QueryData<MessageIdVariables> queryData,CancellationToken? cancellationToken = null);

    /// <summary>
    ///     获取留言数量
    /// </summary>
    /// <returns></returns>
    Task<ResponseData<MessageCountByComicIdData>> GetMessageCountByComicId(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);

    /// <summary>
    ///     最后一条留言
    /// </summary>
    /// <param name="queryData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponseData<LastMessageByComicIdData>> GetLastMessageByComicId(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<MessagesByComicIdData>> GetMessagesByComicId(QueryData<ComicIdPaginationVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<VoteMessageData>> VoteMessage(QueryData<VoteMessageVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<MessageVotesByComicIdData>> MessageVotesByComicId(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<DeleteMessageData>> DeleteMessage(QueryData<MessageIdVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion

    #region 歷史

    Task<ResponseData<ReadComicHistoryData>> ReadComicHistory(QueryData<PaginationVariables> queryData,CancellationToken? cancellationToken = null);

    Task<ResponseData<ReadComicHistoryByIdData>> ReadComicHistoryById(QueryData<ComicIdVariables> queryData,CancellationToken? cancellationToken = null);

    Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
        QueryData<AddReadComicHistoryVariables> queryData,CancellationToken? cancellationToken = null);


    Task<ResponseData<AddReadComicHistoryData>> DeleteComicReadHistory(
        QueryData<AddReadComicHistoryVariables> queryData,CancellationToken? cancellationToken = null);

    #endregion
}