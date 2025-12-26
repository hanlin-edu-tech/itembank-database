using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 課本章節表 Id 序列化器
/// </summary>
public class TextbookContentIdSerializer : NullableClassSerializerBase<TextbookContentId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 TextbookContentId
    /// </summary>
    protected override TextbookContentId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new TextbookContentId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 TextbookContentId");
    }

    /// <summary>
    /// 序列化 TextbookContentId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, TextbookContentId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
