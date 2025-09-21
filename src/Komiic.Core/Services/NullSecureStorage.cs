using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class NullSecureStorage : ISecureStorage
{
    public Task<string?> GetAsync(string key) => Task.FromResult<string?>(null);
    public Task SetAsync(string key, string value) => Task.CompletedTask;
    public Task RemoveAsync(string key) => Task.CompletedTask;
}

