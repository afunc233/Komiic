using System;
using Komiic.Core;
using Komiic.Core.Contracts.Model;

namespace Komiic.Data;

public record MangeImageData(MangaInfo MangaInfo, ChaptersByComicId Chapter, ImagesByChapterId ImagesByChapterId);

public static class MangeImageDataExt
{
    public static string GetImageUrl(this MangeImageData imageData)
    {
        return $"/api/image/{imageData.ImagesByChapterId.Kid}";
    }

    public static Uri GetReferrer(this MangeImageData mangeImageData)
    {
        return new Uri(
            $"{KomiicConst.KomiicApiUrl}/comic/{mangeImageData.MangaInfo.Id}/chapter/{mangeImageData.Chapter.Id}");
    }
}