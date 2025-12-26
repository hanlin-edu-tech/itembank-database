using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Interfaces;

/// <summary>
/// 用於定義集合索引的介面
/// </summary>
/// <typeparam name="T">MongoDB 集合的文件類型</typeparam>
public interface IIndexable<T>
{
    /// <summary>
    /// 為集合建立索引
    /// </summary>
    /// <param name="collection">MongoDB 集合</param>
    static abstract Task CreateIndexesAsync(IMongoCollection<T> collection);
}
