using System.Threading.Tasks;
using Komiic.Contracts;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class LoadAccountActivationHandler(IAccountService accountService) : IActivationHandler
{
    int IActivationHandler.Order => 10;

    async Task IActivationHandler.HandleAsync()
    {
        await Task.CompletedTask;
        
        await accountService.LoadAccount();
    }
}