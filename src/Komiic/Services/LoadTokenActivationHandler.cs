using System.Threading.Tasks;
using Komiic.Contracts;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class LoadTokenActivationHandler(ITokenService tokenService)
    : IActivationHandler
{
    public int Order => 0;

    public async Task HandleAsync()
    {
        await tokenService.LoadToken();
    }
}