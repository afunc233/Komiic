using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MangaDetailDataService(IKomiicQueryApi komiicQueryApi) : IMangaDetailDataService
{
    public async Task<MangaInfo?> GetMangaInfoById(string comicId)
    {
        var variables =
            QueryDataEnum.ComicById.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });
        var mangaInfoByIdData = await komiicQueryApi.GetMangaInfoById(variables);

        if (mangaInfoByIdData is { Data.ComicById: not null })
        {
            return mangaInfoByIdData.Data.ComicById;
        }

        return default;
    }

    public async Task<int> GetMessageCountByComicId(string comicId)
    {
        var variables =
            QueryDataEnum.MessageCountByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var messageCountByComicIdData = await komiicQueryApi.GetMessageCountByComicId(variables);

        if (messageCountByComicIdData is { Data.MessageCountByComicIdCount: not null })
        {
            return messageCountByComicIdData.Data.MessageCountByComicIdCount ?? 0;
        }

        return 0;
    }

    public async Task<LastMessageByComicId?> GetLastMessageByComicId(string comicId)
    {
        var variables =
            QueryDataEnum.LastMessageByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });


        var lastMessageByComicIdData = await komiicQueryApi.GetLastMessageByComicId(variables);
        if (lastMessageByComicIdData is { Data.LastMessageByComicId: not null })
        {
            return lastMessageByComicIdData.Data.LastMessageByComicId;
        }

        return default;
    }

    public async Task<List<ChaptersByComicId>> GetChapterByComicId(string comicId)
    {
        var variables =
            QueryDataEnum.ChapterByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var chapterByComicIdData = await komiicQueryApi.GetChapterByComicId(variables);
        if (chapterByComicIdData is { Data.ChaptersByComicIdList.Count: > 0 })
        {
            return chapterByComicIdData.Data.ChaptersByComicIdList;
        }

        return [];
    }

    public async Task<List<string>> GetRecommendComicById(string comicId)
    {
        var variables =
            QueryDataEnum.RecommendComicById.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var recommendComicByIdData = await komiicQueryApi.GetRecommendComicById(variables);
        if (recommendComicByIdData is { Data.RecommendComicList.Count: > 0 })
        {
            return recommendComicByIdData.Data.RecommendComicList;
        }

        return [];
    }

    public async Task<List<MangaInfo>> GetRecommendMangaInfosById(string comicId)
    {
        var recommendComicData =
            await GetRecommendComicById(comicId);
        if (recommendComicData is { Count: > 0 })
        {
            var mangaInfoByIdsData = await GetMangaInfoByIds(recommendComicData.ToArray());

            if (mangaInfoByIdsData is { Count: > 0 })
            {
                return mangaInfoByIdsData;
            }
        }

        return [];
    }

    public async Task<List<MangaInfo>> GetMangaInfoByIds(params string[] comicIds)
    {
        var variables = QueryDataEnum.ComicByIds.GetQueryDataWithVariables(new ComicIdsVariables
            { ComicIdList = comicIds.ToList() });
        var mangaInfoByIdsData = await komiicQueryApi.GetMangaInfoByIds(variables);
        if (mangaInfoByIdsData is { Data.ComicByIds.Count: > 0 })
        {
            return mangaInfoByIdsData.Data.ComicByIds;
        }

        return [];
    }

    public async Task<List<Folder>> GetMyFolders()
    {
        var queryData = QueryDataEnum.MyFolder.GetQueryData();

        var folderData = await komiicQueryApi.GetMyFolder(queryData);
        if (folderData is { Data.Folders.Count: > 0 })
        {
            return folderData.Data.Folders;
        }

        return [];
    }

    public async Task<bool> RemoveComicToFolder(string folderId, string comicId)
    {
        var queryData = QueryDataEnum.RemoveComicToFolder.GetQueryDataWithVariables(new FolderIdAndComicIdVariables()
        {
            FolderId = folderId,
            ComicId = comicId
        });

        var removeComicToFolderData = await komiicQueryApi.RemoveComicToFolder(queryData);

        if (removeComicToFolderData is { Data: not null })
        {
            return removeComicToFolderData.Data.RemoveComicToFolder;
        }

        return false;
    }

    public async Task<bool> AddComicToFolder(string folderId, string comicId)
    {
        var queryData = QueryDataEnum.AddComicToFolder.GetQueryDataWithVariables(new FolderIdAndComicIdVariables()
        {
            FolderId = folderId,
            ComicId = comicId
        });

        var removeComicToFolderData = await komiicQueryApi.AddComicToFolder(queryData);

        if (removeComicToFolderData is { Data: not null })
        {
            return removeComicToFolderData.Data.AddComicToFolder;
        }

        return false;
    }

    public async Task<List<string>> ComicInAccountFolders(string comicId)
    {
        var queryData = QueryDataEnum.ComicInAccountFolders.GetQueryDataWithVariables(new ComicIdVariables()
        {
            ComicId = comicId
        });

        var comicInAccountFoldersData = await komiicQueryApi.ComicInAccountFolders(queryData);

        if (comicInAccountFoldersData is { Data.ComicInAccountFolders.Count: > 0 })
        {
            return comicInAccountFoldersData.Data.ComicInAccountFolders;
        }

        return [];
    }

    public async Task<List<LastReadByComicId>> GetComicsLastRead(params string[] comicIds)
    {
        var queryData =
            QueryDataEnum.ComicsLastRead.GetQueryDataWithVariables(
                new ComicIdsVariables() { ComicIdList = comicIds.ToList() });

        var comicsLastReadData = await komiicQueryApi.GetComicsLastRead(queryData);
        if (comicsLastReadData is { Data.LastReadByComicIds.Count: > 0 })
        {
            return comicsLastReadData.Data.LastReadByComicIds;
        }

        return [];
    }
}