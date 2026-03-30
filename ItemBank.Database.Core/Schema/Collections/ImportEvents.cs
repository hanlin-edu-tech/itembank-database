using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ImportEvents")]
[CollectionSchemaKind(CollectionSchemaKind.DiscriminatedCollection)]
[CollectionSchemaDiscriminator("_t")]
[CollectionSchemaNote("目前先保守建成共通欄位，額外欄位以動態結構保留")]
[Description("匯入事件")]
public sealed class ImportEvent : IIndexable<ImportEvent>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

[BsonElement("_t")]
    [Description("判別型別")]
    public required string T { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("任務 Id")]
    public required TaskId TaskId { get; init; }

    [BsonExtraElements]
    [Description("事件特定欄位；結構依判別型別變化")]
    public BsonDocument? ExtraElements { get; init; }
}
