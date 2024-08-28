using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.Core.Apis;

internal class RefitKomiicQueryApi(IServiceProvider serviceProvider) : IKomiicQueryApi
{
    private IKomiicQueryClient KomiicQueryClientImplementation =>
        serviceProvider.GetRequiredService<IKomiicQueryClient>();

    Task<ResponseData<ImagesByChapterIdData>> IKomiicQueryApi.GetImagesByChapterId(
        QueryData<ChapterIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetImagesByChapterId(queryData);
    }

    Task<ResponseData<RecommendComicIdsData>> IKomiicQueryApi.GetRecommendComicIds(
        QueryData<RecommendComicIdsPaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetRecommendComicIds(queryData);
    }

    Task<ResponseData<RecentUpdateData>> IKomiicQueryApi.GetRecentUpdate(QueryData<PaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetRecentUpdate(queryData);
    }

    Task<ResponseData<HotComicsData>> IKomiicQueryApi.GetHotComics(QueryData<PaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetHotComics(queryData);
    }

    Task<ResponseData<ComicByIdData>> IKomiicQueryApi.GetMangaInfoById(QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetMangaInfoById(queryData);
    }

    Task<ResponseData<ComicByIdsData>> IKomiicQueryApi.GetMangaInfoByIds(QueryData<ComicIdsVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetMangaInfoByIds(queryData);
    }

    Task<ResponseData<RecommendComicByIdData>> IKomiicQueryApi.GetRecommendComicById(
        QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetRecommendComicById(queryData);
    }

    Task<ResponseData<ChaptersByComicIdData>> IKomiicQueryApi.GetChapterByComicId(QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetChapterByComicId(queryData);
    }

    Task<ResponseData<AllCategoryData>> IKomiicQueryApi.GetAllCategory(QueryData queryData)
    {
        return KomiicQueryClientImplementation.GetAllCategory(queryData);
    }

    Task<ResponseData<ComicByCategoryData>> IKomiicQueryApi.GetComicByCategory(
        QueryData<CategoryIdPaginationVariables> getQueryDataWithVariables)
    {
        return KomiicQueryClientImplementation.GetComicByCategory(getQueryDataWithVariables);
    }

    Task<ResponseData<AllAuthorsData>> IKomiicQueryApi.GetAllAuthors(QueryData<PaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetAllAuthors(queryData);
    }

    Task<ResponseData<ComicsByAuthorData>> IKomiicQueryApi.GetComicsByAuthor(QueryData<AuthorIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetComicsByAuthor(queryData);
    }

    Task<ResponseData<AddFavoriteData>> IKomiicQueryApi.AddFavorite(QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.AddFavorite(queryData);
    }

    Task<ResponseData<RemoveFavoriteData>> IKomiicQueryApi.RemoveFavorite(QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.RemoveFavorite(queryData);
    }

    Task<ResponseData<FavoriteData>> IKomiicQueryApi.GetFavorites(QueryData<FavoritePaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetFavorites(queryData);
    }

    Task<ResponseData<LastReadByComicIdData>> IKomiicQueryApi.GetComicsLastRead(QueryData<ComicIdsVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetComicsLastRead(queryData);
    }

    Task<ResponseData<FolderData>> IKomiicQueryApi.GetMyFolder(QueryData queryData)
    {
        return KomiicQueryClientImplementation.GetMyFolder(queryData);
    }

    Task<ResponseData<FolderByKeyData>> IKomiicQueryApi.GetFolderByKey(QueryData<KeyVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetFolderByKey(queryData);
    }

    Task<ResponseData<FolderComicIdsData>> IKomiicQueryApi.GetFolderComicIds(
        QueryData<FolderComicIdsVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetFolderComicIds(queryData);
    }

    Task<ResponseData<UpdateFolderNameData>> IKomiicQueryApi.UpdateFolderName(
        QueryData<UpdateFolderNameVariables> queryData)
    {
        return KomiicQueryClientImplementation.UpdateFolderName(queryData);
    }

    Task<ResponseData<RemoveFolderData>> IKomiicQueryApi.RemoveFolder(QueryData<FolderIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.RemoveFolder(queryData);
    }

    Task<ResponseData<RemoveComicToFolderData>> IKomiicQueryApi.RemoveComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.RemoveComicToFolder(queryData);
    }

    Task<ResponseData<AddComicToFolderData>> IKomiicQueryApi.AddComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.AddComicToFolder(queryData);
    }

    Task<ResponseData<ComicInAccountFoldersData>> IKomiicQueryApi.ComicInAccountFolders(
        QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.ComicInAccountFolders(queryData);
    }

    Task<ResponseData<AddMessageToComicData>> IKomiicQueryApi.AddMessageToComic(
        QueryData<AddMessageToComicVariables> queryData)
    {
        return KomiicQueryClientImplementation.AddMessageToComic(queryData);
    }

    Task<ResponseData<MessageChanData>> IKomiicQueryApi.GetMessageChan(QueryData<MessageIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetMessageChan(queryData);
    }

    Task<ResponseData<MessageCountByComicIdData>> IKomiicQueryApi.GetMessageCountByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetMessageCountByComicId(queryData);
    }

    Task<ResponseData<LastMessageByComicIdData>> IKomiicQueryApi.GetLastMessageByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetLastMessageByComicId(queryData);
    }

    Task<ResponseData<MessagesByComicIdData>> IKomiicQueryApi.GetMessagesByComicId(
        QueryData<ComicIdPaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.GetMessagesByComicId(queryData);
    }

    Task<ResponseData<VoteMessageData>> IKomiicQueryApi.VoteMessage(QueryData<VoteMessageVariables> queryData)
    {
        return KomiicQueryClientImplementation.VoteMessage(queryData);
    }

    Task<ResponseData<MessageVotesByComicIdData>> IKomiicQueryApi.MessageVotesByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.MessageVotesByComicId(queryData);
    }

    Task<ResponseData<DeleteMessageData>> IKomiicQueryApi.DeleteMessage(QueryData<MessageIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.DeleteMessage(queryData);
    }

    Task<ResponseData<ReadComicHistoryData>> IKomiicQueryApi.ReadComicHistory(QueryData<PaginationVariables> queryData)
    {
        return KomiicQueryClientImplementation.ReadComicHistory(queryData);
    }

    Task<ResponseData<ReadComicHistoryByIdData>> IKomiicQueryApi.ReadComicHistoryById(
        QueryData<ComicIdVariables> queryData)
    {
        return KomiicQueryClientImplementation.ReadComicHistoryById(queryData);
    }

    Task<ResponseData<AddReadComicHistoryData>> IKomiicQueryApi.AddReadComicHistory(
        QueryData<AddReadComicHistoryVariables> queryData)
    {
        return KomiicQueryClientImplementation.AddReadComicHistory(queryData);
    }

    Task<ResponseData<AddReadComicHistoryData>> IKomiicQueryApi.DeleteComicReadHistory(
        QueryData<AddReadComicHistoryVariables> queryData)
    {
        return KomiicQueryClientImplementation.DeleteComicReadHistory(queryData);
    }
}