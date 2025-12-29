using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 驗證目標 Id 序列化器
/// </summary>
public class ValidationTargetIdSerializer : NullableClassSerializerBase<ValidationTargetId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 ValidationTargetId
    /// </summary>
    protected override ValidationTargetId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new ValidationTargetId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ValidationTargetId");
    }

    /// <summary>
    /// 序列化 ValidationTargetId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ValidationTargetId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
