using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Difficulties")]
[Description("難度")]
public class Difficulty
{
    [BsonId]
    [Description("Id")]
    public required DifficultyId Id { get; init; }

    [Description("難度值")]
    public required int Value { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("備註")]
    public required string Remark { get; init; }
}
