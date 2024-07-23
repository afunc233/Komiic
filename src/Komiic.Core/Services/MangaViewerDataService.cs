using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MangaViewerDataService(IKomiicQueryApi komiicQueryApi) : IMangaViewerDataService
{
    public Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
        QueryData<AddReadComicHistoryVariables> queryData)
    {
        return komiicQueryApi.AddReadComicHistory(queryData);
    }

    public async Task<List<ImagesByChapterId>> GetImagesByChapterId(string chapterId)
    {
        var variables = QueryDataEnum.ImagesByChapterId.GetQueryDataWithVariables(
            new ChapterIdVariables
            {
                ChapterId = chapterId
            });
        var imagesByChapterIdData = await komiicQueryApi.GetImagesByChapterId(variables);
        if (imagesByChapterIdData is { Data.ImagesByChapterIdList.Count: > 0 })
        {
            return imagesByChapterIdData.Data.ImagesByChapterIdList;
        }

        return [];
    }

    public async Task<ComicHistory?> ReadComicHistoryById(string comicId)
    {
        var variables = QueryDataEnum.ReadComicHistoryById.GetQueryDataWithVariables(new ComicIdVariables
        {
            ComicId = comicId
        });

        var readComicHistoryByIdData = await komiicQueryApi.ReadComicHistoryById(variables);

        if (readComicHistoryByIdData is { Data.ReadComicHistoryById: not null })
        {
            return readComicHistoryByIdData.Data.ReadComicHistoryById;
        }

        return default;
    }

    public async Task<ComicHistory?> AddReadComicHistory(string comicId, string chapterId, int page)
    {
        var variables = QueryDataEnum.AddReadComicHistory.GetQueryDataWithVariables(
            new AddReadComicHistoryVariables
            {
                ComicId = comicId,
                ChapterId = chapterId,
                Page = page,
            });
        var addReadComicHistoryData = await komiicQueryApi.AddReadComicHistory(variables);
        if (addReadComicHistoryData is { Data.AddReadComicHistory: not null })
        {
            return addReadComicHistoryData.Data.AddReadComicHistory;
        }

        return default;
    }
}