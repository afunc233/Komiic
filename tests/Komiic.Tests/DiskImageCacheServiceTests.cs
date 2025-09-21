using System;
using System.IO;
using System.Threading.Tasks;
using Komiic.Core.Services;
using Xunit;

namespace Komiic.Tests;

public class DiskImageCacheServiceTests
{
    [Fact]
    public async Task Set_Then_Get_Returns_Local_Path()
    {
        var svc = new DiskImageCacheService();
        var url = "test://" + Guid.NewGuid();
        var bytes = new byte[] {1, 2, 3, 4, 5};

        await ((Komiic.Core.Contracts.Services.IImageCacheService)svc).SetLocalImage(url, bytes);
        var path = ((Komiic.Core.Contracts.Services.IImageCacheService)svc).GetLocalImageUrl(url);

        Assert.False(string.IsNullOrWhiteSpace(path));
        Assert.True(File.Exists(path!));
    }
}

