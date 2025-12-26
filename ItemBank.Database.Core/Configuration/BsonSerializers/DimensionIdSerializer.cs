using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 向度資訊表 Id 序列化器
/// </summary>
public class DimensionIdSerializer : NullableClassSerializerBase<DimensionId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 DimensionId
    /// </summary>
    protected override DimensionId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new DimensionId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 DimensionId");
    }

    /// <summary>
    /// 序列化 DimensionId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, DimensionId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
