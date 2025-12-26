using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 產品 Id 序列化器
/// </summary>
public class ProductIdSerializer : NullableClassSerializerBase<ProductId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 ProductId
    /// </summary>
    protected override ProductId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new ProductId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ProductId");
    }

    /// <summary>
    /// 序列化 ProductId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ProductId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
