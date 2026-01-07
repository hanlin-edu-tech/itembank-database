using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ItemBank.Database.Core.Configuration;

/// <summary>
/// 索引初始化後台服務
/// </summary>
internal sealed class IndexInitializationService(DbContext dbContext, ILogger<IndexInitializationService> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("開始初始化 MongoDB 索引...");

        try
        {
            await dbContext.InitializeIndexesAsync(cancellationToken);
            logger.LogInformation("MongoDB 索引初始化完成");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "MongoDB 索引初始化失敗");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

/// <summary>
/// DbContext 擴展方法
/// </summary>
file static class DbContextExtensions
{
    /// <summary>
    /// 初始化所有 IIndexable 集合的索引
    /// </summary>
    public static async Task InitializeIndexesAsync(this DbContext dbContext, CancellationToken cancellationToken = default)
    {
        foreach (var initializer in IndexInitializationRegistry.All)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await initializer.InitializeAsync(dbContext, cancellationToken);
        }
    }
}
