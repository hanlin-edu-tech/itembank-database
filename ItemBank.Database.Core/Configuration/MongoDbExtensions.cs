using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;

namespace ItemBank.Database.Core.Configuration;

/// <summary>
/// MongoDB 相關的 DI 擴展方法
/// </summary>
public static class MongoDbExtensions
{
    /// <summary>
    /// 將 DbContext 及索引初始化服務註冊到 DI 容器
    /// </summary>
    /// <param name="services">服務集合</param>
    /// <param name="configure">配置委派</param>
    /// <returns>服務集合</returns>
    public static IServiceCollection AddMongoDbContext(
        this IServiceCollection services,
        Action<MongoDbContextOptionsBuilder> configure)
    {
        RegisterSerializers();
        var builder = new MongoDbContextOptionsBuilder();
        configure(builder);
        var options = builder.Build();

        services.AddSingleton(options);
        services.AddSingleton<DbContext>();

        // 如果啟用自動建立索引，註冊後台服務
        if (options.AutoCreateIndexes)
        {
            services.AddHostedService<IndexInitializationService>();
        }

        return services;
    }

    private static void RegisterSerializers()
    {
        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", conventionPack, _ => true);
        var pack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register(
            "Ignore Extra Elements for Models",
            pack,
            t => t.FullName?.StartsWith("ItemBank.Database.Core.Schema.Collections") is true
        );
    }
}

/// <summary>
/// MongoDbContextOptions 的建構器
/// </summary>
public sealed class MongoDbContextOptionsBuilder
{
    private MongoClientSettings? _clientSettings;
    private string? _databaseName;
    private bool _autoCreateIndexes;

    /// <summary>
    /// 設定 MongoDB 客戶端設定
    /// </summary>
    public MongoDbContextOptionsBuilder WithClientSettings(MongoClientSettings settings)
    {
        _clientSettings = settings;
        return this;
    }

    /// <summary>
    /// 設定 MongoDB 資料庫名稱
    /// </summary>
    public MongoDbContextOptionsBuilder WithDatabaseName(string databaseName)
    {
        _databaseName = databaseName;
        return this;
    }

    /// <summary>
    /// 設定是否自動建立索引
    /// </summary>
    public MongoDbContextOptionsBuilder WithAutoCreateIndexes(bool enabled = true)
    {
        _autoCreateIndexes = enabled;
        return this;
    }

    /// <summary>
    /// 建構 MongoDbContextOptions
    /// </summary>
    public MongoDbContextOptions Build()
    {
        if (_clientSettings == null)
            throw new InvalidOperationException("MongoClientSettings 必須被設定");
        if (string.IsNullOrEmpty(_databaseName))
            throw new InvalidOperationException("DatabaseName 必須被設定");

        return new MongoDbContextOptions
        {
            ClientSettings = _clientSettings,
            DatabaseName = _databaseName,
            AutoCreateIndexes = _autoCreateIndexes
        };
    }
}