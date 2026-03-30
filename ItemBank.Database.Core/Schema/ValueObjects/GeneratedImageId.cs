using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record GeneratedImageId(string Value) : IConvertibleId<GeneratedImageId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static GeneratedImageId FromValue(ObjectId value) => new(value.ToString());
}
