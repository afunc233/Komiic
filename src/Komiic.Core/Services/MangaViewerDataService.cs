using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MangaViewerDataService(IKomiicQueryApi komiicQueryApi) : IMangaViewerDataService
{
    public async Task<ApiResponseData<List<ImagesByChapterId>>> GetImagesByChapterId(string chapterId)
    {
        var variables = QueryDataEnum.ImagesByChapterId.GetQueryDataWithVariables(
            new ChapterIdVariables
            {
                ChapterId = chapterId
            });
        var imagesByChapterIdData = await komiicQueryApi.GetImagesByChapterId(variables);
        if (imagesByChapterIdData is { Data.ImagesByChapterIdList.Count: > 0 })
        {
            return new()
                { Data = imagesByChapterIdData.Data.ImagesByChapterIdList };
        }

        return new() { Data = [] };
    }

    public async Task<ApiResponseData<ComicHistory?>> ReadComicHistoryById(string comicId)
    {
        var variables = QueryDataEnum.ReadComicHistoryById.GetQueryDataWithVariables(new ComicIdVariables
        {
            ComicId = comicId
        });

        var readComicHistoryByIdData = await komiicQueryApi.ReadComicHistoryById(variables);

        if (readComicHistoryByIdData is { Data.ReadComicHistoryById: not null })
        {
            return new() { Data = readComicHistoryByIdData.Data.ReadComicHistoryById };
        }

        return new() { Data = default };
    }

    public async Task<ApiResponseData<ComicHistory?>> AddReadComicHistory(string comicId, string chapterId, int page)
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
            return new() { Data = addReadComicHistoryData.Data.AddReadComicHistory };
        }

        return new() { Data = default };
    }
}