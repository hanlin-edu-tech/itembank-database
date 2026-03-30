using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("DimensionEvents")]
[CollectionSchemaKind(CollectionSchemaKind.DiscriminatedCollection)]
[CollectionSchemaDiscriminator("_t")]
[CollectionSchemaNote("目前先保守建成共通欄位，額外欄位以動態結構保留")]
[CollectionSchemaNote("nested updateDimensionValueCommands 內仍存在額外 discriminator")]
[Description("向度事件")]
public sealed class DimensionEvent : IIndexable<DimensionEvent>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

[BsonElement("_t")]
    [Description("判別型別")]
    public required string T { get; init; }

    [Description("學程 Id")]
    public BodyOfKnowledgeId? BodyOfKnowledgeId { get; init; }

    [Description("建立時間")]
    public required DimensionEventCreatedAt CreatedAt { get; init; }

    [Description("向度 Id")]
    public required DimensionId DimensionId { get; init; }

    [Description("使用者 Id")]
    public required UserId UserId { get; init; }

    [BsonExtraElements]
    [Description("事件特定欄位；結構依判別型別變化")]
    public BsonDocument? ExtraElements { get; init; }
}

[Description("建立時間")]
public sealed class DimensionEventCreatedAt
{
    [BsonElement("DateTime")]
    [Description("日期時間")]
    public required DateTime DateTime { get; init; }

    [BsonElement("Offset")]
    [Description("位移")]
    public required int Offset { get; init; }

    [BsonElement("Ticks")]
    [Description("刻度")]
    public required long Ticks { get; init; }
}
