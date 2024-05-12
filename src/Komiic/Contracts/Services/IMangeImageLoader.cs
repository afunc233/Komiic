﻿using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Komiic.Data;

namespace Komiic.Contracts.Services;

public interface IMangeImageLoader
{
    Task<Bitmap?> ProvideImageAsync(MangeImageData imageData);
    
    event EventHandler<MangeImageData>? ImageLoaded;
}