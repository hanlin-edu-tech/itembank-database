using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 匯入任務 Id 序列化器
/// </summary>
public class TaskIdSerializer : NullableClassSerializerBase<TaskId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 TaskId
    /// </summary>
    protected override TaskId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new TaskId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 TaskId");
    }

    /// <summary>
    /// 序列化 TaskId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, TaskId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
