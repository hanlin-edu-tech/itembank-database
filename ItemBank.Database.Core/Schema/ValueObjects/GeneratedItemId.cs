using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record GeneratedItemId(string Value) : IConvertibleId<GeneratedItemId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static GeneratedItemId FromValue(ObjectId value) => new(value.ToString());
}
