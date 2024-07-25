using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

public class MockCookieService : ICookieService
{
    public Task LoadCookies()
    {
        return Task.CompletedTask;
    }

    public Task ClearAllCookies(bool save = true)
    {
        return Task.CompletedTask;
    }

    public Task SaveCookies()
    {
        return Task.CompletedTask;
    }
}