using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 包裹 Id 序列化器
/// </summary>
public class PackageIdSerializer : NullableClassSerializerBase<PackageId>
{
    /// <summary>
    /// 反序列化 BSON ObjectId 為 PackageId
    /// </summary>
    protected override PackageId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.ObjectId)
            return new PackageId(context.Reader.ReadObjectId().ToString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 PackageId");
    }

    /// <summary>
    /// 序列化 PackageId 為 BSON ObjectId
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, PackageId value)
    {
        context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
    }
}
