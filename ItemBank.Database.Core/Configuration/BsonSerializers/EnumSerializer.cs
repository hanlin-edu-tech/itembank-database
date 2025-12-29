using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// Enum 序列化器，支援多種序列化類型
/// </summary>
/// <typeparam name="TEnum">列舉類型</typeparam>
public sealed class EnumSerializer<TEnum>(EnumSerializationType serializationType = EnumSerializationType.CamelCase)
    : SerializerBase<TEnum?>
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

        if (serializationType == EnumSerializationType.Integer)
        {
            var intValue = context.Reader.ReadInt32();
            if (Enum.IsDefined(typeof(TEnum), intValue))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), intValue);
            }

            throw new BsonSerializationException(
                $"Unable to deserialize int '{intValue}' to Enum '{typeof(TEnum).Name}'."
            );
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

        switch (serializationType)
        {
            case EnumSerializationType.Integer:
                var intValue = Convert.ToInt32(value.Value);
                context.Writer.WriteInt32(intValue);
                break;

            case EnumSerializationType.CamelCase:
                var camelCaseValue = ToCamelCase(value.Value.ToString());
                context.Writer.WriteString(camelCaseValue);
                break;

            case EnumSerializationType.PascalCase:
                context.Writer.WriteString(value.Value.ToString());
                break;

            default:
                throw new BsonSerializationException(
                    $"Unsupported serialization type '{serializationType}'."
                );
        }
    }

    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0])) return str;

        return $"{char.ToLowerInvariant(str[0])}{str[1..]}";
    }
}