using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.Core.Apis;

internal class RefitKomiicQueryApi(IServiceProvider serviceProvider) : IKomiicQueryApi
{
    async Task<ResponseData<ImagesByChapterIdData>> IKomiicQueryApi.GetImagesByChapterId(
        QueryData<ChapterIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetImagesByChapterId(queryData);
    }

    async Task<ResponseData<RecommendComicIdsData>> IKomiicQueryApi.GetRecommendComicIds(
        QueryData<RecommendComicIdsPaginationVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetRecommendComicIds(queryData);
    }

    async Task<ResponseData<RecentUpdateData>> IKomiicQueryApi.GetRecentUpdate(QueryData<PaginationVariables> queryData)
    {
        var client = GetClient();
        return await client.GetRecentUpdate(queryData);
    }

    async Task<ResponseData<HotComicsData>> IKomiicQueryApi.GetHotComics(QueryData<PaginationVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetHotComics(queryData);
    }

    async Task<ResponseData<ComicByIdData>> IKomiicQueryApi.GetMangaInfoById(QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetMangaInfoById(queryData);
    }

    async Task<ResponseData<ComicByIdsData>> IKomiicQueryApi.GetMangaInfoByIds(QueryData<ComicIdsVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetMangaInfoByIds(queryData);
    }

    async Task<ResponseData<RecommendComicByIdData>> IKomiicQueryApi.GetRecommendComicById(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetRecommendComicById(queryData);
    }

    async Task<ResponseData<ChaptersByComicIdData>> IKomiicQueryApi.GetChapterByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetChapterByComicId(queryData);
    }

    async Task<ResponseData<AllCategoryData>> IKomiicQueryApi.GetAllCategory(QueryData queryData)
    {
        using var client = GetClient();
        return await client.GetAllCategory(queryData);
    }

    async Task<ResponseData<ComicByCategoryData>> IKomiicQueryApi.GetComicByCategory(
        QueryData<CategoryIdPaginationVariables> getQueryDataWithVariables)
    {
        using var client = GetClient();
        return await client.GetComicByCategory(getQueryDataWithVariables);
    }

    async Task<ResponseData<AllAuthorsData>> IKomiicQueryApi.GetAllAuthors(QueryData<PaginationVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetAllAuthors(queryData);
    }

    async Task<ResponseData<ComicsByAuthorData>> IKomiicQueryApi.GetComicsByAuthor(
        QueryData<AuthorIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetComicsByAuthor(queryData);
    }

    async Task<ResponseData<AddFavoriteData>> IKomiicQueryApi.AddFavorite(QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.AddFavorite(queryData);
    }

    async Task<ResponseData<RemoveFavoriteData>> IKomiicQueryApi.RemoveFavorite(QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.RemoveFavorite(queryData);
    }

    async Task<ResponseData<FavoriteData>> IKomiicQueryApi.GetFavorites(
        QueryData<FavoritePaginationVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetFavorites(queryData);
    }

    async Task<ResponseData<LastReadByComicIdData>> IKomiicQueryApi.GetComicsLastRead(
        QueryData<ComicIdsVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetComicsLastRead(queryData);
    }

    async Task<ResponseData<FolderData>> IKomiicQueryApi.GetMyFolder(QueryData queryData)
    {
        using var client = GetClient();
        return await client.GetMyFolder(queryData);
    }

    async Task<ResponseData<FolderByKeyData>> IKomiicQueryApi.GetFolderByKey(QueryData<KeyVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetFolderByKey(queryData);
    }

    async Task<ResponseData<FolderComicIdsData>> IKomiicQueryApi.GetFolderComicIds(
        QueryData<FolderComicIdsVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetFolderComicIds(queryData);
    }

    async Task<ResponseData<UpdateFolderNameData>> IKomiicQueryApi.UpdateFolderName(
        QueryData<UpdateFolderNameVariables> queryData)
    {
        using var client = GetClient();
        return await client.UpdateFolderName(queryData);
    }

    async Task<ResponseData<RemoveFolderData>> IKomiicQueryApi.RemoveFolder(QueryData<FolderIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.RemoveFolder(queryData);
    }

    async Task<ResponseData<RemoveComicToFolderData>> IKomiicQueryApi.RemoveComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.RemoveComicToFolder(queryData);
    }

    async Task<ResponseData<AddComicToFolderData>> IKomiicQueryApi.AddComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.AddComicToFolder(queryData);
    }

    async Task<ResponseData<ComicInAccountFoldersData>> IKomiicQueryApi.ComicInAccountFolders(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.ComicInAccountFolders(queryData);
    }

    async Task<ResponseData<AddMessageToComicData>> IKomiicQueryApi.AddMessageToComic(
        QueryData<AddMessageToComicVariables> queryData)
    {
        using var client = GetClient();
        return await client.AddMessageToComic(queryData);
    }

    async Task<ResponseData<MessageChanData>> IKomiicQueryApi.GetMessageChan(QueryData<MessageIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetMessageChan(queryData);
    }

    async Task<ResponseData<MessageCountByComicIdData>> IKomiicQueryApi.GetMessageCountByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetMessageCountByComicId(queryData);
    }

    async Task<ResponseData<LastMessageByComicIdData>> IKomiicQueryApi.GetLastMessageByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetLastMessageByComicId(queryData);
    }

    async Task<ResponseData<MessagesByComicIdData>> IKomiicQueryApi.GetMessagesByComicId(
        QueryData<ComicIdPaginationVariables> queryData)
    {
        using var client = GetClient();
        return await client.GetMessagesByComicId(queryData);
    }

    async Task<ResponseData<VoteMessageData>> IKomiicQueryApi.VoteMessage(QueryData<VoteMessageVariables> queryData)
    {
        using var client = GetClient();
        return await client.VoteMessage(queryData);
    }

    async Task<ResponseData<MessageVotesByComicIdData>> IKomiicQueryApi.MessageVotesByComicId(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.MessageVotesByComicId(queryData);
    }

    async Task<ResponseData<DeleteMessageData>> IKomiicQueryApi.DeleteMessage(QueryData<MessageIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.DeleteMessage(queryData);
    }

    async Task<ResponseData<ReadComicHistoryData>> IKomiicQueryApi.ReadComicHistory(
        QueryData<PaginationVariables> queryData)
    {
        using var client = GetClient();
        return await client.ReadComicHistory(queryData);
    }

    async Task<ResponseData<ReadComicHistoryByIdData>> IKomiicQueryApi.ReadComicHistoryById(
        QueryData<ComicIdVariables> queryData)
    {
        using var client = GetClient();
        return await client.ReadComicHistoryById(queryData);
    }

    async Task<ResponseData<AddReadComicHistoryData>> IKomiicQueryApi.AddReadComicHistory(
        QueryData<AddReadComicHistoryVariables> queryData)
    {
        using var client = GetClient();
        return await client.AddReadComicHistory(queryData);
    }

    async Task<ResponseData<AddReadComicHistoryData>> IKomiicQueryApi.DeleteComicReadHistory(
        QueryData<AddReadComicHistoryVariables> queryData)
    {
        using var client = GetClient();
        return await client.DeleteComicReadHistory(queryData);
    }

    private IKomiicQueryClient GetClient()
    {
        return serviceProvider.GetRequiredService<IKomiicQueryClient>();
    }
}