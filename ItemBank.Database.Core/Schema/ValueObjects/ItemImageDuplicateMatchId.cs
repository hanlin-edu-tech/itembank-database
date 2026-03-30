using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ItemImageDuplicateMatchId(string Value) : IConvertibleId<ItemImageDuplicateMatchId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static ItemImageDuplicateMatchId FromValue(ObjectId value) => new(value.ToString());
}
