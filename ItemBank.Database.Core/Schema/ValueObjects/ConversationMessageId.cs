using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 對話訊息 Id
/// </summary>
public record ConversationMessageId(string Value) : IConvertibleId<ConversationMessageId, ObjectId>
{
    public ObjectId ToValue() => ObjectId.Parse(Value);
    public static ConversationMessageId FromValue(ObjectId value) => new(value.ToString());
}
