using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 冊次 Id 序列化器
/// </summary>
public class VolumeIdSerializer : NullableClassSerializerBase<VolumeId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 VolumeId
    /// </summary>
    protected override VolumeId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new VolumeId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 VolumeId");
    }

    /// <summary>
    /// 序列化 VolumeId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, VolumeId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
