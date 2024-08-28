using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IMangaViewerDataService
{
    Task<ApiResponseData<List<ImagesByChapterId>>> GetImagesByChapterId(string chapterId);

    Task<ApiResponseData<ComicHistory?>> ReadComicHistoryById(string comicId);

    Task<ApiResponseData<ComicHistory?>> AddReadComicHistory(string comicId, string chapterId, int page);
}