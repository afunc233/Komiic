using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Komiic.Contracts.VO;

namespace Komiic.Controls;

public class MangaCard : TemplatedControl
{
    public static readonly StyledProperty<MangaInfoVO> MangaInfoVOProperty =
        AvaloniaProperty.Register<MangaCard, MangaInfoVO>(nameof(MangaInfoVO));

    /// <summary>
    ///     Defines the <see cref="OpenMangaCommand" /> property.
    /// </summary>
    public static readonly StyledProperty<ICommand> OpenMangaCommandProperty =
        AvaloniaProperty.Register<MangaCard, ICommand>(nameof(OpenMangaCommand), enableDataValidation: true);

    /// <summary>
    ///     Defines the <see cref="ToggleFavouriteCommand" /> property.
    /// </summary>
    public static readonly StyledProperty<ICommand> ToggleFavouriteCommandProperty =
        AvaloniaProperty.Register<MangaCard, ICommand>(nameof(ToggleFavouriteCommand), enableDataValidation: true);

    public MangaInfoVO MangaInfoVO
    {
        get => GetValue(MangaInfoVOProperty);
        set => SetValue(MangaInfoVOProperty, value);
    }

    /// <summary>
    ///     Gets or sets an <see cref="ICommand" /> to be invoked when the button is clicked.
    /// </summary>
    public ICommand OpenMangaCommand
    {
        get => GetValue(OpenMangaCommandProperty);
        set => SetValue(OpenMangaCommandProperty, value);
    }

    /// <summary>
    ///     Gets or sets an <see cref="ICommand" /> to be invoked when the button is clicked.
    /// </summary>
    public ICommand ToggleFavouriteCommand
    {
        get => GetValue(ToggleFavouriteCommandProperty);
        set => SetValue(ToggleFavouriteCommandProperty, value);
    }
}