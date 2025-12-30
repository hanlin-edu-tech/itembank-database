using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;

/// <summary>
/// 基於 ObjectId 的 Id 序列化器
/// </summary>
/// <typeparam name="TId">Id 型別</typeparam>
public class ObjectIdBasedIdSerializer<TId> : NullableClassSerializerBase<TId>
    where TId : class, IConvertibleId<TId, ObjectId>
{
    protected override TId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType != BsonType.ObjectId)
            throw new BsonSerializationException($"無法從 {bsonType} 反序列化 {typeof(TId).Name}");

        var value = context.Reader.ReadObjectId();
        return TId.FromValue(value);
    }

    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, TId value)
    {
        context.Writer.WriteObjectId(value.ToValue());
    }
}
