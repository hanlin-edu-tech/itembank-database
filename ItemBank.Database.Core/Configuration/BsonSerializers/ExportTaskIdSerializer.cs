using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 匯出任務 Id 序列化器
/// </summary>
public class ExportTaskIdSerializer : NullableClassSerializerBase<ExportTaskId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 ExportTaskId
    /// </summary>
    protected override ExportTaskId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new ExportTaskId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ExportTaskId");
    }

    /// <summary>
    /// 序列化 ExportTaskId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ExportTaskId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
