using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 倉庫 Id 序列化器
/// </summary>
public class RepositoryIdSerializer : NullableClassSerializerBase<RepositoryId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 RepositoryId
    /// </summary>
    protected override RepositoryId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new RepositoryId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 RepositoryId");
    }

    /// <summary>
    /// 序列化 RepositoryId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, RepositoryId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
