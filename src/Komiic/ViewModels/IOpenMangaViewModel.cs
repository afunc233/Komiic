using CommunityToolkit.Mvvm.Input;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Model;

namespace Komiic.ViewModels;

public interface IOpenMangaViewModel
{
    /// <summary>
    ///     只是统一 一下 方便 MangaInfoTemplate 里面 绑定 command
    ///     或许应该写一个 控件 来处理
    /// </summary>
    IAsyncRelayCommand<MangaInfo> OpenMangaCommand { get; }

    IAsyncRelayCommand<MangaInfoVO> ToggleFavouriteCommand { get; }
}