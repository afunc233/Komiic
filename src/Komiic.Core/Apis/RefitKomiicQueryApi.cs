using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.Core.Apis;

internal class RefitKomiicQueryApi(IServiceProvider serviceProvider) : IKomiicQueryApi
{
    async Task<ResponseData<ImagesByChapterIdData>> IKomiicQueryApi.GetImagesByChapterId(
        QueryData<ChapterIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetImagesByChapterId(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<RecommendComicIdsData>> IKomiicQueryApi.GetRecommendComicIds(
        QueryData<RecommendComicIdsPaginationVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetRecommendComicIds(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<RecentUpdateData>> IKomiicQueryApi.GetRecentUpdate(QueryData<PaginationVariables> queryData,
        CancellationToken? cancellationToken)
    {
        var client = GetClient();
        return await client.GetRecentUpdate(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<HotComicsData>> IKomiicQueryApi.GetHotComics(QueryData<PaginationVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetHotComics(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ComicByIdData>> IKomiicQueryApi.GetMangaInfoById(QueryData<ComicIdVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetMangaInfoById(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ComicByIdsData>> IKomiicQueryApi.GetMangaInfoByIds(QueryData<ComicIdsVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetMangaInfoByIds(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<RecommendComicByIdData>> IKomiicQueryApi.GetRecommendComicById(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetRecommendComicById(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ChaptersByComicIdData>> IKomiicQueryApi.GetChapterByComicId(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetChapterByComicId(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AllCategoryData>> IKomiicQueryApi.GetAllCategory(QueryData queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetAllCategory(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ComicByCategoryData>> IKomiicQueryApi.GetComicByCategory(
        QueryData<CategoryIdPaginationVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetComicByCategory(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AllAuthorsData>> IKomiicQueryApi.GetAllAuthors(QueryData<PaginationVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetAllAuthors(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ComicsByAuthorData>> IKomiicQueryApi.GetComicsByAuthor(
        QueryData<AuthorIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetComicsByAuthor(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AddFavoriteData>> IKomiicQueryApi.AddFavorite(QueryData<ComicIdVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.AddFavorite(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<RemoveFavoriteData>> IKomiicQueryApi.RemoveFavorite(QueryData<ComicIdVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.RemoveFavorite(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<FavoriteData>> IKomiicQueryApi.GetFavorites(
        QueryData<FavoritePaginationVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetFavorites(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<LastReadByComicIdData>> IKomiicQueryApi.GetComicsLastRead(
        QueryData<ComicIdsVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetComicsLastRead(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<FolderData>> IKomiicQueryApi.GetMyFolder(QueryData queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetMyFolder(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<FolderByKeyData>> IKomiicQueryApi.GetFolderByKey(QueryData<KeyVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetFolderByKey(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<FolderComicIdsData>> IKomiicQueryApi.GetFolderComicIds(
        QueryData<FolderComicIdsVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetFolderComicIds(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<UpdateFolderNameData>> IKomiicQueryApi.UpdateFolderName(
        QueryData<UpdateFolderNameVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.UpdateFolderName(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<RemoveFolderData>> IKomiicQueryApi.RemoveFolder(QueryData<FolderIdVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.RemoveFolder(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<RemoveComicToFolderData>> IKomiicQueryApi.RemoveComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.RemoveComicToFolder(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AddComicToFolderData>> IKomiicQueryApi.AddComicToFolder(
        QueryData<FolderIdAndComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.AddComicToFolder(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ComicInAccountFoldersData>> IKomiicQueryApi.ComicInAccountFolders(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.ComicInAccountFolders(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AddMessageToComicData>> IKomiicQueryApi.AddMessageToComic(
        QueryData<AddMessageToComicVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.AddMessageToComic(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<MessageChanData>> IKomiicQueryApi.GetMessageChan(QueryData<MessageIdVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetMessageChan(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<MessageCountByComicIdData>> IKomiicQueryApi.GetMessageCountByComicId(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetMessageCountByComicId(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<LastMessageByComicIdData>> IKomiicQueryApi.GetLastMessageByComicId(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetLastMessageByComicId(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<MessagesByComicIdData>> IKomiicQueryApi.GetMessagesByComicId(
        QueryData<ComicIdPaginationVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetMessagesByComicId(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<VoteMessageData>> IKomiicQueryApi.VoteMessage(QueryData<VoteMessageVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.VoteMessage(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<MessageVotesByComicIdData>> IKomiicQueryApi.MessageVotesByComicId(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.MessageVotesByComicId(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<DeleteMessageData>> IKomiicQueryApi.DeleteMessage(QueryData<MessageIdVariables> queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.DeleteMessage(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ReadComicHistoryData>> IKomiicQueryApi.ReadComicHistory(
        QueryData<PaginationVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.ReadComicHistory(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<ReadComicHistoryByIdData>> IKomiicQueryApi.ReadComicHistoryById(
        QueryData<ComicIdVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.ReadComicHistoryById(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AddReadComicHistoryData>> IKomiicQueryApi.AddReadComicHistory(
        QueryData<AddReadComicHistoryVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.AddReadComicHistory(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AddReadComicHistoryData>> IKomiicQueryApi.DeleteComicReadHistory(
        QueryData<AddReadComicHistoryVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.DeleteComicReadHistory(queryData, cancellationToken ?? CancellationToken.None);
    }

    private IKomiicQueryClient GetClient()
    {
        return serviceProvider.GetRequiredService<IKomiicQueryClient>();
    }
}