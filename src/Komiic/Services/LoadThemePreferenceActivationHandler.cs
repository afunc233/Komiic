using System.Threading.Tasks;
using Komiic.Contracts;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class LoadThemePreferenceActivationHandler(IThemePreferenceService themePreferenceService)
    : IActivationHandler
{
    public int Order => -100; // apply as early as possible

    public async Task HandleAsync()
    {
        await themePreferenceService.LoadAsync();
        themePreferenceService.ApplyCurrentPreference();
    }
}

