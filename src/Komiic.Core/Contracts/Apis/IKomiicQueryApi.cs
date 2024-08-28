using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Apis;

internal interface IKomiicQueryApi
{
    #region 漫画内容

    /// <summary>
    ///     根据章节获取图片
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<ImagesByChapterIdData>> GetImagesByChapterId(QueryData<ChapterIdVariables> queryData);

    #endregion

    #region 推薦

    Task<ResponseData<RecommendComicIdsData>> GetRecommendComicIds(
        QueryData<RecommendComicIdsPaginationVariables> queryData);

    #endregion

    #region 首页

    Task<ResponseData<RecentUpdateData>> GetRecentUpdate(QueryData<PaginationVariables> queryData);


    Task<ResponseData<HotComicsData>> GetHotComics(QueryData<PaginationVariables> queryData);

    #endregion

    #region 漫画详情

    /// <summary>
    ///     根据id 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<ComicByIdData>> GetMangaInfoById(QueryData<ComicIdVariables> queryData);

    /// <summary>
    ///     根据id 列表 获取漫画详情
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<ComicByIdsData>> GetMangaInfoByIds(QueryData<ComicIdsVariables> queryData);

    /// <summary>
    ///     根据id 获取推荐漫画id
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<RecommendComicByIdData>> GetRecommendComicById(QueryData<ComicIdVariables> queryData);

    /// <summary>
    ///     根据ID 获取章节信息
    /// </summary>
    /// <returns></returns>
    Task<ResponseData<ChaptersByComicIdData>> GetChapterByComicId(QueryData<ComicIdVariables> queryData);

    #endregion

    #region 分类

    /// <summary>
    ///     獲取分類
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<AllCategoryData>> GetAllCategory(QueryData queryData);

    /// <summary>
    ///     根據分類獲取漫画
    /// </summary>
    /// <param name="getQueryDataWithVariables"></param>
    /// <returns></returns>
    Task<ResponseData<ComicByCategoryData>> GetComicByCategory(
        QueryData<CategoryIdPaginationVariables> getQueryDataWithVariables);

    #endregion

    #region 作者

    Task<ResponseData<AllAuthorsData>> GetAllAuthors(QueryData<PaginationVariables> queryData);


    Task<ResponseData<ComicsByAuthorData>> GetComicsByAuthor(QueryData<AuthorIdVariables> queryData);

    #endregion

    #region 喜愛

    Task<ResponseData<AddFavoriteData>> AddFavorite(QueryData<ComicIdVariables> queryData);


    Task<ResponseData<RemoveFavoriteData>> RemoveFavorite(QueryData<ComicIdVariables> queryData);


    Task<ResponseData<FavoriteData>> GetFavorites(QueryData<FavoritePaginationVariables> queryData);


    Task<ResponseData<LastReadByComicIdData>> GetComicsLastRead(QueryData<ComicIdsVariables> queryData);

    #endregion

    #region 書櫃

    /// <summary>
    ///     獲取書櫃圖片 https://komiic.com/api/folder/image/IzSzen1J
    ///     IzSzen1J : folderKey
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<FolderData>> GetMyFolder(QueryData queryData);


    Task<ResponseData<FolderByKeyData>> GetFolderByKey(QueryData<KeyVariables> queryData);


    Task<ResponseData<FolderComicIdsData>> GetFolderComicIds(QueryData<FolderComicIdsVariables> queryData);


    Task<ResponseData<UpdateFolderNameData>> UpdateFolderName(QueryData<UpdateFolderNameVariables> queryData);


    Task<ResponseData<RemoveFolderData>> RemoveFolder(QueryData<FolderIdVariables> queryData);


    Task<ResponseData<RemoveComicToFolderData>> RemoveComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData);


    Task<ResponseData<AddComicToFolderData>> AddComicToFolder(QueryData<FolderIdAndComicIdVariables> queryData);


    Task<ResponseData<ComicInAccountFoldersData>> ComicInAccountFolders(QueryData<ComicIdVariables> queryData);

    #endregion

    #region 評論/回復

    Task<ResponseData<AddMessageToComicData>> AddMessageToComic(QueryData<AddMessageToComicVariables> queryData);


    Task<ResponseData<MessageChanData>> GetMessageChan(QueryData<MessageIdVariables> queryData);

    /// <summary>
    ///     获取留言数量
    /// </summary>
    /// <returns></returns>
    Task<ResponseData<MessageCountByComicIdData>> GetMessageCountByComicId(QueryData<ComicIdVariables> queryData);

    /// <summary>
    ///     最后一条留言
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    Task<ResponseData<LastMessageByComicIdData>> GetLastMessageByComicId(QueryData<ComicIdVariables> queryData);


    Task<ResponseData<MessagesByComicIdData>> GetMessagesByComicId(QueryData<ComicIdPaginationVariables> queryData);


    Task<ResponseData<VoteMessageData>> VoteMessage(QueryData<VoteMessageVariables> queryData);


    Task<ResponseData<MessageVotesByComicIdData>> MessageVotesByComicId(QueryData<ComicIdVariables> queryData);


    Task<ResponseData<DeleteMessageData>> DeleteMessage(QueryData<MessageIdVariables> queryData);

    #endregion

    #region 歷史

    Task<ResponseData<ReadComicHistoryData>> ReadComicHistory(QueryData<PaginationVariables> queryData);

    Task<ResponseData<ReadComicHistoryByIdData>> ReadComicHistoryById(QueryData<ComicIdVariables> queryData);

    Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
        QueryData<AddReadComicHistoryVariables> queryData);


    Task<ResponseData<AddReadComicHistoryData>> DeleteComicReadHistory(
        QueryData<AddReadComicHistoryVariables> queryData);

    #endregion
}