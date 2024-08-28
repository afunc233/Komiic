using System.Collections.Generic;
using System.Threading.Tasks;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Models;

namespace Komiic.Contracts.Services;

public interface IMangaInfoVOService
{
    IEnumerable<MangaInfoVO> GetMangaInfoVOs(IEnumerable<MangaInfo> mangaInfos);

    MangaInfoVO GetMangaInfoVO(MangaInfo mangaInfo);

    void UpdateMangaInfoVO(IEnumerable<MangaInfoVO> mangaVOs);

    Task<bool> ToggleFavorite(MangaInfoVO mangaInfoVO);
}