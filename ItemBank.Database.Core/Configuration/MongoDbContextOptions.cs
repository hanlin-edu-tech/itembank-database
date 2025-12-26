using MongoDB.Driver;

namespace ItemBank.Database.Core.Configuration;

/// <summary>
/// MongoDbContext 的配置選項
/// </summary>
public sealed class MongoDbContextOptions
{
    /// <summary>
    /// MongoDB 客戶端設定
    /// </summary>
    public required MongoClientSettings ClientSettings { get; init; }

    /// <summary>
    /// MongoDB 資料庫名稱
    /// </summary>
    public required string DatabaseName { get; init; }

    /// <summary>
    /// 是否在取得集合時自動建立索引
    /// 預設值：false
    /// </summary>
    public bool AutoCreateIndexes { get; init; }
}
