using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 目錄群組 Id 序列化器
/// </summary>
public class CatalogGroupIdSerializer : NullableClassSerializerBase<CatalogGroupId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 CatalogGroupId
    /// </summary>
    protected override CatalogGroupId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new CatalogGroupId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 CatalogGroupId");
    }

    /// <summary>
    /// 序列化 CatalogGroupId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, CatalogGroupId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
