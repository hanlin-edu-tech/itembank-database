using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 題目合併歷史 Id
/// </summary>
public record ItemMergeHistoryId(string Value) : IConvertibleId<ItemMergeHistoryId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static ItemMergeHistoryId FromValue(ObjectId value) => new(value.ToString());
}
