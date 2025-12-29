using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 難度 Id 序列化器
/// </summary>
public class DifficultyIdSerializer : NullableClassSerializerBase<DifficultyId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 DifficultyId
    /// </summary>
    protected override DifficultyId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new DifficultyId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 DifficultyId");
    }

    /// <summary>
    /// 序列化 DifficultyId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, DifficultyId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
