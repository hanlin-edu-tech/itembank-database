using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 使用者 Id 序列化器
/// </summary>
public class UserIdSerializer : NullableClassSerializerBase<UserId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 UserId
    /// </summary>
    protected override UserId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new UserId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 UserId");
    }

    /// <summary>
    /// 序列化 UserId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, UserId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
