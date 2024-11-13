using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;

namespace Komiic.Services;

public class MangaInfoVOService(
    IMessenger messenger,
    IAccountService accountService,
    IMangaDetailDataService mangaDetailDataService) : IMangaInfoVOService
{
    public IEnumerable<MangaInfoVO> GetMangaInfoVOs(IEnumerable<MangaInfo> mangaInfos)
    {
        return mangaInfos.Select(GetMangaInfoVO);
    }

    public MangaInfoVO GetMangaInfoVO(MangaInfo mangaInfo)
    {
        var favoriteComicIds = accountService.AccountData?.FavoriteComicIds;
        return new MangaInfoVO(mangaInfo)
        {
            IsFavourite = favoriteComicIds?.Any(it => string.Equals(it, mangaInfo.Id)) ?? false
        };
    }

    public void UpdateMangaInfoVO(IEnumerable<MangaInfoVO> mangaVOs)
    {
        foreach (var mangaVO in mangaVOs)
        {
            mangaVO.IsFavourite =
                accountService.AccountData?.FavoriteComicIds.Any(it => string.Equals(it, mangaVO.MangaInfo.Id)) ??
                false;
        }
    }

    public async Task<bool> ToggleFavorite(MangaInfoVO mangaInfoVO,CancellationToken? cancellationToken = null)
    {
        await Task.CompletedTask;

        var account = accountService.AccountData;
        if (account is null)
        {
            var result = await messenger.Send(new OpenLoginDialogMessage());
            if (result)
            {
                account = accountService.AccountData;
            }
        }

        var mangaInfo = mangaInfoVO.MangaInfo;
        if (account is not null)
        {
            if (mangaInfoVO.IsFavourite)
            {
                var removeResult = await mangaDetailDataService.RemoveFavorite(mangaInfo.Id,cancellationToken);
                if (removeResult is { ErrorMessage: null } and { Data: not null })
                {
                    if (removeResult.Data ?? false)
                    {
                        mangaInfoVO.IsFavourite = false;
                        mangaInfoVO.FavoriteCount--;
                        account.FavoriteComicIds.Remove(mangaInfo.Id);
                        return true;
                    }
                }
            }
            else
            {
                var favoriteData = await mangaDetailDataService.AddFavorite(mangaInfo.Id,cancellationToken);
                if (favoriteData is { Data: not null })
                {
                    mangaInfoVO.IsFavourite = true;
                    mangaInfoVO.FavoriteCount++;
                    account.FavoriteComicIds.Add(favoriteData.Data.ComicId);
                    return true;
                }
            }
        }

        return false;
    }
}