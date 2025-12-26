using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

public sealed class CamelCaseEnumStringSerializer<TEnum> : SerializerBase<TEnum?>
    where TEnum : struct, Enum
{
    public override TEnum? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();

        if (type == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null;
        }

        var stringValue = context.Reader.ReadString();

        if (Enum.TryParse(stringValue, true, out TEnum result))
        {
            return result;
        }

        throw new BsonSerializationException(
            $"Unable to deserialize string '{stringValue}' to Enum '{typeof(TEnum).Name}'."
        );
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnum? value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }
        context.Writer.WriteString(ToCamelCase(value.Value.ToString()));
    }

    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0])) return str;

        return $"{char.ToLowerInvariant(str[0])}{str[1..]}";
    }
}