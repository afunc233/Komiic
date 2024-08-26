using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Komiic.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Komiic.Services;

public class KomiicHostedService(
    IEnumerable<IActivationHandler> activationHandlers,
    ILogger<KomiicHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        await HandleByGroupAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    /// <summary>
    ///     分组并行执行
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async Task HandleByGroupAsync(CancellationToken cancellationToken)
    {
        foreach (var groupActivationHandlers in activationHandlers.GroupBy(it => it.Order).OrderBy(it => it.Key))
        {
            var tasks = groupActivationHandlers.Select(it => it.HandleAsync());
            try
            {
                await Parallel.ForEachAsync(tasks, cancellationToken, async (task, token) =>
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    await task;
                });
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex,
                        "Error occurred while activating services. taskGroup Key:{groupActivationHandlers.Key}",
                        groupActivationHandlers.Key);
                }
            }
        }
    }
}