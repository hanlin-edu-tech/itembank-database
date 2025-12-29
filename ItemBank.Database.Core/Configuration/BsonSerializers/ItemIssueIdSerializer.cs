using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 題目問題 Id 序列化器
/// </summary>
public class ItemIssueIdSerializer : NullableClassSerializerBase<ItemIssueId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 ItemIssueId
    /// </summary>
    protected override ItemIssueId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new ItemIssueId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ItemIssueId");
    }

    /// <summary>
    /// 序列化 ItemIssueId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ItemIssueId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
