using Komiic.Core.Contracts.Model;

namespace Komiic.Core.Contracts.Services;

public interface IMangaViewerDataService
{
    // Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
    //     QueryData<AddReadComicHistoryVariables> queryData);


    Task<List<ImagesByChapterId>> GetImagesByChapterId(string chapterId);

    Task<ComicHistory?> ReadComicHistoryById(string comicId);


    Task<ComicHistory?> AddReadComicHistory(string comicId, string chapterId, int page);
}