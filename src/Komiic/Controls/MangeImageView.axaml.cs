using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Komiic.Data;

namespace Komiic.Controls;

public class MangeImageView : TemplatedControl
{
    // ReSharper disable once InconsistentNaming
    private const string PART_Image = nameof(PART_Image);

    public static readonly StyledProperty<MangeImageData?> MangeImageDataProperty =
        AvaloniaProperty.Register<MangeImageView, MangeImageData?>(nameof(MangeImageData));

    public MangeImageData? MangeImageData
    {
        get => GetValue(MangeImageDataProperty);
        set => SetValue(MangeImageDataProperty, value);
    }

    public static readonly StyledProperty<IMangeImageLoader?> LoaderProperty =
        AvaloniaProperty.Register<MangeImageView, IMangeImageLoader?>(nameof(Loader));

    public IMangeImageLoader? Loader
    {
        get => GetValue(LoaderProperty);
        set => SetValue(LoaderProperty, value);
    }


    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<MangeImageView, bool>(nameof(IsLoading));

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        private set => SetValue(IsLoadingProperty, value);
    }


    static MangeImageView()
    {
        MangeImageDataProperty.Changed.AddClassHandler<MangeImageView>((x, _) =>
            x.SetSize());

        // LoaderProperty.Changed.AddClassHandler<MangeImageView>((x, e) =>
        //     x.LoadImage());
    }

    private void SetSize()
    {
        if (MangeImageData == null)
        {
            return;
        }

        if (_image == null)
        {
            return;
        }

        _image.Width = MangeImageData.ImagesByChapterId.Width;
        _image.Height = MangeImageData.ImagesByChapterId.Height;
    }

    private async void LoadImage()
    {
        if (Loader == null)
        {
            return;
        }

        if (MangeImageData == null)
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
            var bitmap = await Loader.ProvideImageAsync(MangeImageData);

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

    private Image? _image;
}