using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ItemYearDimensionValueId(string Value) : IConvertibleId<ItemYearDimensionValueId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static ItemYearDimensionValueId FromValue(ObjectId value) => new(value.ToString());
}
