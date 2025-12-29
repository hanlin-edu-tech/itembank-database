using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 目錄 Id 序列化器
/// </summary>
public class CatalogIdSerializer : NullableClassSerializerBase<CatalogId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 CatalogId
    /// </summary>
    protected override CatalogId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new CatalogId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 CatalogId");
    }

    /// <summary>
    /// 序列化 CatalogId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, CatalogId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
