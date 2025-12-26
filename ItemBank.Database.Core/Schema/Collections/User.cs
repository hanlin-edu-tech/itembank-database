using System.ComponentModel;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("使用者")]
public class User
{
    [BsonId]
    [Description("Id")]
    public required UserId Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }
}
