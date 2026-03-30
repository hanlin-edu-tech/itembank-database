using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ChatBot.AiResultCaches")]
[Description("聊天 AI 結果快取")]
public sealed class ChatBotAiResultCache : IIndexable<ChatBotAiResultCache>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("內容雜湊")]
    public required string ContentHash { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("模型 Id")]
    public required string ModelId { get; init; }

    [Description("結果")]
    public required ChatBotAiResultCacheResult Result { get; init; }

    [Description("類型")]
    public required ChatBotAiResultCacheType Type { get; init; }

    static IReadOnlyList<CreateIndexModel<ChatBotAiResultCache>> IIndexable<ChatBotAiResultCache>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ChatBotAiResultCache>.IndexKeys.Combine(
                    Builders<ChatBotAiResultCache>.IndexKeys.Ascending("contentHash"),
                    Builders<ChatBotAiResultCache>.IndexKeys.Ascending("modelId"),
                    Builders<ChatBotAiResultCache>.IndexKeys.Ascending("type")
                ),
                new CreateIndexOptions<ChatBotAiResultCache>
                {
                    Name = "contentHash_1_modelId_1_type_1",
                    Unique = true
                }
        )
    ];
}

[Description("結果")]
public sealed class ChatBotAiResultCacheResult
{
    [Description("片段清單")]
    public IReadOnlyList<ChatBotAiResultCacheResultChunk>? Chunks { get; init; }

    [Description("向量")]
    public IReadOnlyList<double>? Vector { get; init; }
}

[Description("結果片段")]
public sealed class ChatBotAiResultCacheResultChunk
{
    [Description("內容")]
    public required string Content { get; init; }

    [Description("標題")]
    public required string Title { get; init; }
}
