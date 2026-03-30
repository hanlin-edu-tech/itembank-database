using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record SemanticCacheId(string Value) : IConvertibleId<SemanticCacheId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static SemanticCacheId FromValue(ObjectId value) => new(value.ToString());
}
