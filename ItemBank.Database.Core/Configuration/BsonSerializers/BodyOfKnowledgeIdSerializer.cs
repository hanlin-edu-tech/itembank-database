using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 學程 Id 序列化器
/// </summary>
public class BodyOfKnowledgeIdSerializer : NullableClassSerializerBase<BodyOfKnowledgeId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 BodyOfKnowledgeId
    /// </summary>
    protected override BodyOfKnowledgeId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new BodyOfKnowledgeId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 BodyOfKnowledgeId");
    }

    /// <summary>
    /// 序列化 BodyOfKnowledgeId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, BodyOfKnowledgeId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
