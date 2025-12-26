using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("使用者")]
public class User
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; set; }

    [Description("名稱")]
    public required string Name { get; set; }
}
