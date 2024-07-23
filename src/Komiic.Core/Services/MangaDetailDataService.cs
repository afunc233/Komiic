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
            var mangaInfoByIdsData = await GetMangaInfoByIds(recommendComicData);

            if (mangaInfoByIdsData is { Count:>0 })
            {
                return mangaInfoByIdsData;
            }
        }
        return  [];
    }

    public async Task<List<MangaInfo>> GetMangaInfoByIds(List<string> comicIdList)
    {

       var variables= QueryDataEnum.ComicByIds.GetQueryDataWithVariables(new ComicIdsVariables
            { ComicIdList = comicIdList });
       var mangaInfoByIdsData = await komiicQueryApi.GetMangaInfoByIds(variables);
       if (mangaInfoByIdsData is {Data.ComicByIds.Count:>0})
       {
           return mangaInfoByIdsData.Data.ComicByIds;
       }

       return [];
    }
}