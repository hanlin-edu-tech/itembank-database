using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 匯出任務 Id
/// </summary>
public record ExportTaskId(string Value) : IConvertibleId<ExportTaskId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static ExportTaskId FromValue(ObjectId value) => new(value.ToString());
}
