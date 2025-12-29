using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 對話訊息 Id 序列化器
/// </summary>
public class ConversationMessageIdSerializer : NullableClassSerializerBase<ConversationMessageId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 ConversationMessageId
    /// </summary>
    protected override ConversationMessageId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new ConversationMessageId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ConversationMessageId");
    }

    /// <summary>
    /// 序列化 ConversationMessageId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ConversationMessageId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
