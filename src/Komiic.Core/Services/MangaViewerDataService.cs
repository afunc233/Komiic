using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MangaViewerDataService(IKomiicQueryApi komiicQueryClient) : IMangaViewerDataService
{
    public async Task<ApiResponseData<List<ImagesByChapterId>>> GetImagesByChapterId(string chapterId)
    {
        var variables = QueryDataEnum.ImagesByChapterId.GetQueryDataWithVariables(
            new ChapterIdVariables
            {
                ChapterId = chapterId
            });
        var imagesByChapterIdData = await komiicQueryClient.GetImagesByChapterId(variables);
        if (imagesByChapterIdData is { Data.ImagesByChapterIdList.Count: > 0 })
        {
            return new ApiResponseData<List<ImagesByChapterId>>
            {
                Data = imagesByChapterIdData.Data.ImagesByChapterIdList,
                ErrorMessage = imagesByChapterIdData.GetMessage()
            };
        }

        return new ApiResponseData<List<ImagesByChapterId>>
        {
            Data = [],
            ErrorMessage = imagesByChapterIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<ComicHistory?>> ReadComicHistoryById(string comicId)
    {
        var variables = QueryDataEnum.ReadComicHistoryById.GetQueryDataWithVariables(new ComicIdVariables
        {
            ComicId = comicId
        });

        var readComicHistoryByIdData = await komiicQueryClient.ReadComicHistoryById(variables);

        if (readComicHistoryByIdData is { Data.ReadComicHistoryById: not null })
        {
            return new ApiResponseData<ComicHistory?>
            {
                Data = readComicHistoryByIdData.Data.ReadComicHistoryById,
                ErrorMessage = readComicHistoryByIdData.GetMessage()
            };
        }

        return new ApiResponseData<ComicHistory?>
        {
            Data = default,
            ErrorMessage = readComicHistoryByIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<ComicHistory?>> AddReadComicHistory(string comicId, string chapterId, int page)
    {
        var variables = QueryDataEnum.AddReadComicHistory.GetQueryDataWithVariables(
            new AddReadComicHistoryVariables
            {
                ComicId = comicId,
                ChapterId = chapterId,
                Page = page
            });
        var addReadComicHistoryData = await komiicQueryClient.AddReadComicHistory(variables);
        if (addReadComicHistoryData is { Data.AddReadComicHistory: not null })
        {
            return new ApiResponseData<ComicHistory?>
            {
                Data = addReadComicHistoryData.Data.AddReadComicHistory,
                ErrorMessage = addReadComicHistoryData.GetMessage()
            };
        }

        return new ApiResponseData<ComicHistory?>
        {
            Data = default,
            ErrorMessage = addReadComicHistoryData.GetMessage()
        };
    }
}