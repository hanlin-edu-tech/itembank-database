using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("版本")]
public class Version
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }
}
