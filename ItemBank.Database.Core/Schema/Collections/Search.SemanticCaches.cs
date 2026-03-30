using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Search.SemanticCaches")]
[Description("搜尋語意快取")]
public sealed class SearchSemanticCache : IIndexable<SearchSemanticCache>
{
[BsonId]
    [Description("Id")]
    public required SemanticCacheId Id { get; init; }

    [Description("是否自動拆解")]
    public required bool AutoDecompose { get; init; }

    [Description("向度")]
    public required int Dimensions { get; init; }

    [Description("嵌入清單")]
    public required IReadOnlyList<double> Embedding { get; init; }

    [Description("生成時間")]
    public required DateTime GeneratedAt { get; init; }

    [Description("關鍵字清單")]
    public IReadOnlyList<string>? Keywords { get; init; }

    [Description("最後存取時間")]
    public required DateTime LastAccessedAt { get; init; }

    [Description("模型")]
    public required string Model { get; init; }

    [Description("查詢雜湊")]
    public required string QueryHash { get; init; }

    [Description("查詢文字")]
    public required string QueryText { get; init; }

    static IReadOnlyList<CreateIndexModel<SearchSemanticCache>> IIndexable<SearchSemanticCache>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<SearchSemanticCache>.IndexKeys.Ascending("queryHash"),
                new CreateIndexOptions<SearchSemanticCache>
                {
                    Name = "queryHash_unique",
                    Unique = true
                }
        )
    ];
}
