using System.Threading.Tasks;
using Komiic.Contracts;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class RefreshAuthActivationHandler(ITokenService tokenService) : IActivationHandler
{
    public int Order => 9;
    public async Task HandleAsync()
    {
        await tokenService.RefreshToken();
    }
}