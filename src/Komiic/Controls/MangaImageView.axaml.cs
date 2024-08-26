using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Komiic.Data;

namespace Komiic.Controls;

public class MangaImageView : TemplatedControl
{
    // ReSharper disable once InconsistentNaming
    private const string PART_Image = nameof(PART_Image);

    public static readonly StyledProperty<MangaImageData?> MangaImageDataProperty =
        AvaloniaProperty.Register<MangaImageView, MangaImageData?>(nameof(MangaImageData));

    public static readonly StyledProperty<IMangaImageLoader?> LoaderProperty =
        AvaloniaProperty.Register<MangaImageView, IMangaImageLoader?>(nameof(Loader));


    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<MangaImageView, bool>(nameof(IsLoading));

    private Image? _image;


    static MangaImageView()
    {
        MangaImageDataProperty.Changed.AddClassHandler<MangaImageView>((x, _) =>
            x.SetSize());
    }

    public MangaImageData? MangaImageData
    {
        get => GetValue(MangaImageDataProperty);
        set => SetValue(MangaImageDataProperty, value);
    }

    public IMangaImageLoader? Loader
    {
        get => GetValue(LoaderProperty);
        set => SetValue(LoaderProperty, value);
    }

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        private set => SetValue(IsLoadingProperty, value);
    }

    private void SetSize()
    {
        if (MangaImageData == null)
        {
            return;
        }

        if (_image == null)
        {
            return;
        }

        _image.Width = MangaImageData.ImagesByChapterId.Width;
        _image.Height = MangaImageData.ImagesByChapterId.Height;
    }

    private async void LoadImage()
    {
        if (Loader == null)
        {
            return;
        }

        if (MangaImageData == null)
        {
            return;
        }

        if (_image == null)
        {
            return;
        }

        IsLoading = true;
        try
        {
            var bitmap = await Loader.ProvideImageAsync(MangaImageData);

            if (bitmap is not null)
            {
                _image.Source = bitmap;
            }
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _image = e.NameScope.Get<Image>(PART_Image);
        SetSize();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        LoadImage();
        base.OnLoaded(e);
    }
}