using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 使用者對話 Id
/// </summary>
public record UserConversationId(string Value) : IConvertibleId<UserConversationId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static UserConversationId FromValue(ObjectId value) => new(value.ToString());
}
