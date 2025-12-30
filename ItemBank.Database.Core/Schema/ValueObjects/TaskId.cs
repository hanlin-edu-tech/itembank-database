using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record TaskId(string Value) : IConvertibleId<TaskId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static TaskId FromValue(ObjectId value) => new(value.ToString());
}
