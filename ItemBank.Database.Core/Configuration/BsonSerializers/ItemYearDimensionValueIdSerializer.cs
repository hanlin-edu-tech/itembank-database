using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 項目年份向度資訊 Id 序列化器
/// </summary>
public class ItemYearDimensionValueIdSerializer : NullableClassSerializerBase<ItemYearDimensionValueId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 ItemYearDimensionValueId
    /// </summary>
    protected override ItemYearDimensionValueId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new ItemYearDimensionValueId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 ItemYearDimensionValueId");
    }

    /// <summary>
    /// 序列化 ItemYearDimensionValueId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, ItemYearDimensionValueId value)
    {
        context.Writer.WriteString(value.Value);
    }
}
