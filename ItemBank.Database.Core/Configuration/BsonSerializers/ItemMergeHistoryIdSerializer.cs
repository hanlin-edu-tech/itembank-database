using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 題目合併歷史 Id 序列化器
/// </summary>
public class ItemMergeHistoryIdSerializer : NullableClassSerializerBase<ItemMergeHistoryId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 ItemMergeHistoryId
    /// </summary>
    protected override ItemMergeHistoryId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new ItemMergeHistoryId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ItemMergeHistoryId");
    }

    /// <summary>
    /// 序列化 ItemMergeHistoryId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ItemMergeHistoryId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
