using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using ItemBank.Database.Core.Schema.Interfaces;
using System.Reflection;
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
        var indexableTypes = FindAllIndexableTypes();

        foreach (var type in indexableTypes)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await CreateIndexesAsync(dbContext, type);
        }
    }

    /// <summary>
    /// 為指定類型建立索引
    /// </summary>
    private static async Task CreateIndexesAsync(DbContext dbContext, Type documentType)
    {
        // 獲取 IIndexable<T> 接口
        var indexableInterface = typeof(IIndexable<>).MakeGenericType(documentType);

        if (!documentType.GetInterfaces().Contains(indexableInterface))
            return;

        // 獲取 GetCollection<T> 方法並呼叫
        var getCollectionMethod = typeof(DbContext)
            .GetMethod(nameof(DbContext.GetCollection), Type.EmptyTypes)!
            .MakeGenericMethod(documentType);

        var collection = getCollectionMethod.Invoke(dbContext, null);

        // 獲取靜態方法 CreateIndexesAsync
        var createIndexMethod = documentType.GetMethod(
            nameof(IIndexable<>.CreateIndexesAsync),
            BindingFlags.Public | BindingFlags.Static,
            null,
            [typeof(IMongoCollection<>).MakeGenericType(documentType)],
            null);

        // 呼叫靜態方法（第一個參數為 null，因為是靜態方法）
        if (createIndexMethod?.Invoke(null, [collection]) is Task task)
        {
            await task;
        }
    }

    /// <summary>
    /// 掃描並尋找所有實作 IIndexable&lt;T&gt; 的類型
    /// </summary>
    private static IEnumerable<Type> FindAllIndexableTypes()
    {
        var assembly = typeof(DbContext).Assembly;
        var indexableGenericInterface = typeof(IIndexable<>);

        return assembly.GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == indexableGenericInterface));
    }
}
