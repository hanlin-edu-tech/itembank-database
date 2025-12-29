using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 重複偵測記錄 Id 序列化器
/// </summary>
public class DuplicateDetectionRecordIdSerializer : NullableClassSerializerBase<DuplicateDetectionRecordId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 DuplicateDetectionRecordId
    /// </summary>
    protected override DuplicateDetectionRecordId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new DuplicateDetectionRecordId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 DuplicateDetectionRecordId");
    }

    /// <summary>
    /// 序列化 DuplicateDetectionRecordId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, DuplicateDetectionRecordId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
