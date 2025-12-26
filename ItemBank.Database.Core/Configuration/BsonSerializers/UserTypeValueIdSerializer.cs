using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 使用者類型值 Id 序列化器
/// </summary>
public class UserTypeValueIdSerializer : NullableClassSerializerBase<UserTypeValueId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 UserTypeValueId
    /// </summary>
    protected override UserTypeValueId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new UserTypeValueId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 UserTypeValueId");
    }

    /// <summary>
    /// 序列化 UserTypeValueId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, UserTypeValueId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
