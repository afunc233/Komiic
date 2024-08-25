using System;
using Komiic.Core;
using Komiic.Core.Contracts.Model;

namespace Komiic.Data;

public record MangaImageData(MangaInfo MangaInfo, ChaptersByComicId Chapter, ImagesByChapterId ImagesByChapterId);

public static class MangaImageDataExt
{
    public static string GetImageUrl(this MangaImageData imageData)
    {
        return $"/api/image/{imageData.ImagesByChapterId.Kid}";
    }

    public static Uri GetReferrer(this MangaImageData mangaImageData)
    {
        return new(
            $"{KomiicConst.KomiicApiUrl}/comic/{mangaImageData.MangaInfo.Id}/chapter/{mangaImageData.Chapter.Id}");
    }
}