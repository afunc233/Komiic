using CommunityToolkit.Mvvm.ComponentModel;
using Komiic.Core.Contracts.Models;

namespace Komiic.Contracts.VO;

public partial class MangaInfoVO(MangaInfo mangaInfo) : ObservableObject
{
    [ObservableProperty] private int _favoriteCount = mangaInfo.FavoriteCount;
    [ObservableProperty] private bool _isFavourite;

    public MangaInfo MangaInfo { get; } = mangaInfo;
}