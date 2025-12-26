using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;

/// <summary>
///     可空型別序列化器基類
///     自動處理 null 檢查，子類只需實現 DeserializeValue 和 SerializeValue
/// </summary>
/// <typeparam name="T">實際型別（非 nullable）</typeparam>
public abstract class NullableClassSerializerBase<T> : SerializerBase<T?>
    where T : class 
{
    public sealed override T? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();

        if (type != BsonType.Null) return DeserializeValue(context, args, type);
        context.Reader.ReadNull();
        return null;
    }

    public sealed override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T? value)
    {
        if (value == null)
        {
            context.Writer.WriteNull();
        }
        else
        {
            SerializeValue(context, args, value);
        }
    }

    /// <summary>
    ///     反序列化非 null 值
    /// </summary>
    protected abstract T DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType);

    /// <summary>
    ///     序列化非 null 值
    /// </summary>
    protected abstract void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, T value);
}