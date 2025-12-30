using System.Reflection;
using ItemBank.Database.Core.Schema.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Configuration;

/// <summary>
/// MongoDB 資料庫上下文
/// </summary>
public sealed class DbContext(MongoDbContextOptions options)
{
    private readonly IMongoDatabase _database = new MongoClient(options.ClientSettings).GetDatabase(options.DatabaseName);

    /// <summary>
    /// 取得指定類型的集合
    /// </summary>
    /// <typeparam name="T">集合的文件類型</typeparam>
    /// <returns>MongoDB 集合</returns>
    public IMongoCollection<T> GetCollection<T>()
    {
        var collectionName = GetCollectionName<T>();
        return _database.GetCollection<T>(collectionName);
    }

    private static string GetCollectionName<T>()
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<CollectionNameAttribute>();

        return attribute != null ? attribute.Name :
            throw new InvalidOperationException($"類型 {type.FullName} 未標註 {nameof(CollectionNameAttribute)} 屬性。");
    }
}
