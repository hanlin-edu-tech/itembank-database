using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 使用者對話 Id 序列化器
/// </summary>
public class UserConversationIdSerializer : NullableClassSerializerBase<UserConversationId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 UserConversationId
    /// </summary>
    protected override UserConversationId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new UserConversationId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 UserConversationId");
    }

    /// <summary>
    /// 序列化 UserConversationId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, UserConversationId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
