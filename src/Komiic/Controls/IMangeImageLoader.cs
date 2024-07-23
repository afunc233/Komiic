using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Data;

namespace Komiic.Controls;

public interface IMangeImageLoader
{
    Task<Bitmap?> ProvideImageAsync(MangeImageData imageData);

    event EventHandler<KvValue<MangeImageData, bool>>? ImageLoaded;
}