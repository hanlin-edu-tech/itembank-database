using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 版本 Id 序列化器
/// </summary>
public class VersionIdSerializer : NullableClassSerializerBase<VersionId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 VersionId
    /// </summary>
    protected override VersionId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new VersionId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 VersionId");
    }

    /// <summary>
    /// 序列化 VersionId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, VersionId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
