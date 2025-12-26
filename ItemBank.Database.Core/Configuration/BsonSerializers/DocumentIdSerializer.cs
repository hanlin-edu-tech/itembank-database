using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 文件 Id 序列化器
/// </summary>
public class DocumentIdSerializer : NullableClassSerializerBase<DocumentId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 DocumentId
    /// </summary>
    protected override DocumentId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new DocumentId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 DocumentId");
    }

    /// <summary>
    /// 序列化 DocumentId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, DocumentId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
