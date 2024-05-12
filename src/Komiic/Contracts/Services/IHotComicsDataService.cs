using System.Collections.Generic;
using System.Threading.Tasks;
using Komiic.Core.Contracts.Model;

namespace Komiic.Contracts.Services;

public interface IHotComicsDataService
{
    Task<List<MangaInfo>> LoadMore(int pageIndex, string? orderBy = null,string? status = null, bool aes = true);
}