using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 課本章節 Id 序列化器
/// </summary>
public class TextbookSectionIdSerializer : NullableClassSerializerBase<TextbookSectionId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 TextbookSectionId
    /// </summary>
    protected override TextbookSectionId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new TextbookSectionId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 TextbookSectionId");
    }

    /// <summary>
    /// 序列化 TextbookSectionId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, TextbookSectionId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
