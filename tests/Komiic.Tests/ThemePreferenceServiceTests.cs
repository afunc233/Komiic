using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Services;
using Xunit;

namespace Komiic.Tests;

public class ThemePreferenceServiceTests
{
    [Fact]
    public async Task Roundtrip_Save_And_Load_Mode()
    {
        var svc = new ThemePreferenceService();
        await svc.SetAsync(ThemeMode.Dark);

        var svc2 = new ThemePreferenceService();
        var loaded = await svc2.LoadAsync();

        Assert.Equal(ThemeMode.Dark, loaded);

        // cleanup back to Auto to avoid surprising future runs
        await svc2.SetAsync(ThemeMode.Auto);
    }
}

