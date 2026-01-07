using ItemBank.Database.Core.Schema.Extensions;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Interfaces;

/// <summary>
/// 用於定義集合索引的介面
/// </summary>
/// <typeparam name="T">MongoDB 集合的文件類型</typeparam>
public interface IIndexable<T> where T : class, IIndexable<T>
{
    /// <summary>
    /// 為集合建立索引
    /// </summary>
    static IReadOnlyList<CreateIndexModel<T>> CreateIndexModels =>
        [CreateIndexModel<T>.Default(), ..T.CreateIndexModelsWithoutDefault];

    protected static virtual IReadOnlyList<CreateIndexModel<T>> CreateIndexModelsWithoutDefault => [];
}