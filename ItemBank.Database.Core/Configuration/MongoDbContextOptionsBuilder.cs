using MongoDB.Driver;

namespace ItemBank.Database.Core.Configuration;

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