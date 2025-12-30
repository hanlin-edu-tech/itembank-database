using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 重複偵測記錄 Id
/// </summary>
public record DuplicateDetectionRecordId(string Value) : IConvertibleId<DuplicateDetectionRecordId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static DuplicateDetectionRecordId FromValue(ObjectId value) => new(value.ToString());
}
