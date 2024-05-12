using System.Collections.Generic;
using System.Threading.Tasks;
using Komiic.Core.Contracts.Model;

namespace Komiic.Contracts.Services;

public interface IRecentUpdateDataService
{
    Task<List<MangaInfo>> LoadMore(int pageIndex, string? orderBy = null, bool asc = true);
}