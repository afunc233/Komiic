using System.Threading.Tasks;
using Komiic.Contracts;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class LoadCookiesActivationHandler(ICookieService cookieService) : IActivationHandler
{
    public int Order => 0;

    public async Task HandleAsync()
    {
        await cookieService.LoadCookies();
    }
}