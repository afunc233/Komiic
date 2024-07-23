using Komiic.Core.Contracts.Model;
using Refit;

namespace Komiic.Core.Contracts.Services;

public interface IMangaViewerDataService
{
    Task<ResponseData<AddReadComicHistoryData>> AddReadComicHistory(
        [Body] QueryData<AddReadComicHistoryVariables> queryData);


    Task<List<ImagesByChapterId>> GetImagesByChapterId(string chapterId);

    Task<ComicHistory?> ReadComicHistoryById(string comicId);


    Task<ComicHistory?> AddReadComicHistory(string comicId, string chapterId, int page);
}