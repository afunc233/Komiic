using System.Diagnostics;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MacKeychainSecureStorage : ISecureStorage
{
    private const string Account = "Komiic";

    public async Task<string?> GetAsync(string key)
    {
        try
        {
            var (ok, output) = await RunSecurity(["find-generic-password", "-a", Account, "-s", key, "-w", "-q"]);
            return ok ? output?.TrimEnd('\n', '\r') : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task SetAsync(string key, string value)
    {
        try
        {
            // -U to update if exists
            var (ok, _) = await RunSecurity(["add-generic-password", "-a", Account, "-s", key, "-w", value, "-U", "-q"]);
            if (!ok)
            {
                // try delete then add
                await RemoveAsync(key);
                await RunSecurity(["add-generic-password", "-a", Account, "-s", key, "-w", value, "-q"]);
            }
        }
        catch
        {
            // ignore
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await RunSecurity(["delete-generic-password", "-a", Account, "-s", key, "-q"]);
        }
        catch
        {
            // ignore
        }
    }

    private static async Task<(bool ok, string? output)> RunSecurity(string[] args)
    {
        var start = new ProcessStartInfo
        {
            FileName = "/usr/bin/security",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var a in args)
        {
            start.ArgumentList.Add(a);
        }

        using var p = Process.Start(start)!;
        var output = await p.StandardOutput.ReadToEndAsync();
        var error = await p.StandardError.ReadToEndAsync();
        await p.WaitForExitAsync();
        var ok = p.ExitCode == 0;
        return (ok, ok ? output : null);
    }
}

