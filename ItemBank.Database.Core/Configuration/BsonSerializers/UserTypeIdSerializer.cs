using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 使用者類型 Id 序列化器
/// </summary>
public class UserTypeIdSerializer : NullableClassSerializerBase<UserTypeId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 UserTypeId
    /// </summary>
    protected override UserTypeId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new UserTypeId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 UserTypeId");
    }

    /// <summary>
    /// 序列化 UserTypeId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, UserTypeId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
