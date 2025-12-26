using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 文件項目 Id 序列化器
/// </summary>
public class DocumentItemIdSerializer : NullableClassSerializerBase<DocumentItemId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 DocumentItemId
    /// </summary>
    protected override DocumentItemId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new DocumentItemId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 DocumentItemId");
    }

    /// <summary>
    /// 序列化 DocumentItemId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, DocumentItemId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
