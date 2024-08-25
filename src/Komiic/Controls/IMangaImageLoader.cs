using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Data;

namespace Komiic.Controls;

public interface IMangaImageLoader
{
    Task<Bitmap?> ProvideImageAsync(MangaImageData imageData);

    event EventHandler<KvValue<MangaImageData, bool>>? ImageLoaded;
}