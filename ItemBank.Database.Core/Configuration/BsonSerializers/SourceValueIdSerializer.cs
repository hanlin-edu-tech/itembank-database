using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 出處值 Id 序列化器
/// </summary>
public class SourceValueIdSerializer : NullableClassSerializerBase<SourceValueId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 SourceValueId
    /// </summary>
    protected override SourceValueId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new SourceValueId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 SourceValueId");
    }

    /// <summary>
    /// 序列化 SourceValueId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, SourceValueId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
