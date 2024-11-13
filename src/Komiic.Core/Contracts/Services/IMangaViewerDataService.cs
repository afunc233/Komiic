using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IMangaViewerDataService
{
    Task<ApiResponseData<List<ImagesByChapterId>>> GetImagesByChapterId(string chapterId,CancellationToken? cancellationToken = null);

    Task<ApiResponseData<ComicHistory?>> ReadComicHistoryById(string comicId,CancellationToken? cancellationToken = null);

    Task<ApiResponseData<ComicHistory?>> AddReadComicHistory(string comicId, string chapterId, int page,CancellationToken? cancellationToken = null);
}