using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MangaDetailDataService(IKomiicQueryApi komiicQueryApi) : IMangaDetailDataService
{
    public async Task<ApiResponseData<MangaInfo?>> GetMangaInfoById(string comicId)
    {
        var variables =
            QueryDataEnum.ComicById.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });
        var mangaInfoByIdData = await komiicQueryApi.GetMangaInfoById(variables);

        if (mangaInfoByIdData is { Data.ComicById: not null })
        {
            return new() { Data = mangaInfoByIdData.Data.ComicById };
        }

        return new() { Data = default };
    }

    public async Task<ApiResponseData<int?>> GetMessageCountByComicId(string comicId)
    {
        var variables =
            QueryDataEnum.MessageCountByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var messageCountByComicIdData = await komiicQueryApi.GetMessageCountByComicId(variables);

        if (messageCountByComicIdData is { Data.MessageCountByComicIdCount: not null })
        {
            return new() { Data = messageCountByComicIdData.Data.MessageCountByComicIdCount };
        }

        return new() { Data = default };
    }

    public async Task<ApiResponseData<LastMessageByComicId?>> GetLastMessageByComicId(string comicId)
    {
        var variables =
            QueryDataEnum.LastMessageByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });


        var lastMessageByComicIdData = await komiicQueryApi.GetLastMessageByComicId(variables);
        if (lastMessageByComicIdData is { Data.LastMessageByComicId: not null })
        {
            return new()
                { Data = lastMessageByComicIdData.Data.LastMessageByComicId };
        }

        return new() { Data = default };
    }

    public async Task<ApiResponseData<List<ChaptersByComicId>>> GetChapterByComicId(string comicId)
    {
        var variables =
            QueryDataEnum.ChapterByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var chapterByComicIdData = await komiicQueryApi.GetChapterByComicId(variables);
        if (chapterByComicIdData is { Data.ChaptersByComicIdList.Count: > 0 })
        {
            return new()
                { Data = chapterByComicIdData.Data.ChaptersByComicIdList };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<List<string>>> GetRecommendComicById(string comicId)
    {
        var variables =
            QueryDataEnum.RecommendComicById.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var recommendComicByIdData = await komiicQueryApi.GetRecommendComicById(variables);
        if (recommendComicByIdData is { Data.RecommendComicList.Count: > 0 })
        {
            return new() { Data = recommendComicByIdData.Data.RecommendComicList };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetRecommendMangaInfosById(string comicId)
    {
        var recommendComicData =
            await GetRecommendComicById(comicId);
        if (recommendComicData.Data is { Count: > 0 })
        {
            var mangaInfoByIdsData = await GetMangaInfoByIds(recommendComicData.Data.ToArray());

            if (mangaInfoByIdsData.Data is { Count: > 0 })
            {
                return mangaInfoByIdsData;
            }
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetMangaInfoByIds(params string[] comicIds)
    {
        var variables = QueryDataEnum.ComicByIds.GetQueryDataWithVariables(new ComicIdsVariables
            { ComicIdList = comicIds.ToList() });
        var mangaInfoByIdsData = await komiicQueryApi.GetMangaInfoByIds(variables);
        if (mangaInfoByIdsData is { Data.ComicByIds.Count: > 0 })
        {
            return new() { Data = mangaInfoByIdsData.Data.ComicByIds };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<List<Folder>>> GetMyFolders()
    {
        var queryData = QueryDataEnum.MyFolder.GetQueryData();

        var folderData = await komiicQueryApi.GetMyFolder(queryData);
        if (folderData is { Data.Folders.Count: > 0 })
        {
            return new() { Data = folderData.Data.Folders };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<bool?>> RemoveComicToFolder(string folderId, string comicId)
    {
        var queryData = QueryDataEnum.RemoveComicToFolder.GetQueryDataWithVariables(new FolderIdAndComicIdVariables()
        {
            FolderId = folderId,
            ComicId = comicId
        });

        var removeComicToFolderData = await komiicQueryApi.RemoveComicToFolder(queryData);

        if (removeComicToFolderData is { Data: not null })
        {
            return new() { Data = removeComicToFolderData.Data.RemoveComicToFolder };
        }

        return new() { Data = default };
    }

    public async Task<ApiResponseData<bool?>> AddComicToFolder(string folderId, string comicId)
    {
        var queryData = QueryDataEnum.AddComicToFolder.GetQueryDataWithVariables(new FolderIdAndComicIdVariables()
        {
            FolderId = folderId,
            ComicId = comicId
        });

        var removeComicToFolderData = await komiicQueryApi.AddComicToFolder(queryData);

        if (removeComicToFolderData is { Data: not null })
        {
            return new() { Data = removeComicToFolderData.Data.AddComicToFolder };
        }

        return new() { Data = default };
    }

    public async Task<ApiResponseData<List<string>>> ComicInAccountFolders(string comicId)
    {
        var queryData = QueryDataEnum.ComicInAccountFolders.GetQueryDataWithVariables(new ComicIdVariables()
        {
            ComicId = comicId
        });

        var comicInAccountFoldersData = await komiicQueryApi.ComicInAccountFolders(queryData);

        if (comicInAccountFoldersData is { Data.ComicInAccountFolders.Count: > 0 })
        {
            return new() { Data = comicInAccountFoldersData.Data.ComicInAccountFolders };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<List<LastReadByComicId>>> GetComicsLastRead(params string[] comicIds)
    {
        var queryData =
            QueryDataEnum.ComicsLastRead.GetQueryDataWithVariables(
                new ComicIdsVariables() { ComicIdList = comicIds.ToList() });

        var comicsLastReadData = await komiicQueryApi.GetComicsLastRead(queryData);
        if (comicsLastReadData is { Data.LastReadByComicIds.Count: > 0 })
        {
            return new() { Data = comicsLastReadData.Data.LastReadByComicIds };
        }

        return new() { Data = [] };
    }
}