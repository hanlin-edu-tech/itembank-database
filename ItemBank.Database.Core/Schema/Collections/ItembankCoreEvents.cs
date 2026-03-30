using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItembankCoreEvents")]
[CollectionSchemaKind(CollectionSchemaKind.EnvelopedCollection)]
[CollectionSchemaDiscriminator("eventType")]
[CollectionSchemaVariantField("payload")]
[CollectionSchemaNote("payload 結構依 eventType 改變")]
[CollectionSchemaNote("目前先保守以動態物件表示 payload")]
[Description("核心事件外殼")]
public sealed class ItembankCoreEvent : IIndexable<ItembankCoreEvent>
{
    [BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("聚合名稱")]
    public required string Aggregate { get; init; }

    [Description("聚合 Id")]
    public required string AggregateId { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("事件 Id")]
    public required ObjectId EventId { get; init; }

    [Description("事件類型")]
    public required string EventType { get; init; }

    [Description("事件版本")]
    public required int EventVersion { get; init; }

    [Description("事件內容；結構依 EventType 變化")]
    public required BsonDocument Payload { get; init; }

    [Description("處理時間")]
    public DateTime? ProcessedAt { get; init; }

    static IReadOnlyList<CreateIndexModel<ItembankCoreEvent>> IIndexable<ItembankCoreEvent>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ItembankCoreEvent>.IndexKeys.Combine(
                    Builders<ItembankCoreEvent>.IndexKeys.Ascending("aggregate"),
                    Builders<ItembankCoreEvent>.IndexKeys.Ascending("aggregateId")
                ),
                new CreateIndexOptions<ItembankCoreEvent>
                {
                    Name = "aggregate_1_aggregateId_1"
                }
        ),
        new(
            Builders<ItembankCoreEvent>.IndexKeys.Descending("createdAt"),
                new CreateIndexOptions<ItembankCoreEvent>
                {
                    Name = "createdAt_-1"
                }
        ),
        new(
            Builders<ItembankCoreEvent>.IndexKeys.Combine(
                    Builders<ItembankCoreEvent>.IndexKeys.Ascending("eventType"),
                    Builders<ItembankCoreEvent>.IndexKeys.Ascending("processedAt")
                ),
                new CreateIndexOptions<ItembankCoreEvent>
                {
                    Name = "idx_eventType_processedAt"
                }
        )
    ];
}
