using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 包裹 Id
/// </summary>
public record PackageId(string Value) : IConvertibleId<PackageId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static PackageId FromValue(ObjectId value) => new(value.ToString());
}
