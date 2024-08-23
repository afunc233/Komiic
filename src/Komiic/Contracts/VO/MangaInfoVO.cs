using CommunityToolkit.Mvvm.ComponentModel;
using Komiic.Core.Contracts.Model;

namespace Komiic.Contracts.VO;

public partial class MangaInfoVO(MangaInfo mangaInfo) : ObservableObject
{
    [ObservableProperty] private bool _isFavourite;

    [ObservableProperty] private int _favoriteCount = mangaInfo.FavoriteCount;

    public MangaInfo MangaInfo { get; } = mangaInfo;
}