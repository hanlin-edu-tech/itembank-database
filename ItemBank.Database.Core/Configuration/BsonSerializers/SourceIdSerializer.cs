using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 出處 Id 序列化器
/// </summary>
public class SourceIdSerializer : NullableClassSerializerBase<SourceId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 SourceId
    /// </summary>
    protected override SourceId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new SourceId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 SourceId");
    }

    /// <summary>
    /// 序列化 SourceId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, SourceId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
