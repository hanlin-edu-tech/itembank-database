using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Volumes")]
[Description("冊次")]
public class Volume : IIndexable<Volume>
{
    [BsonId]
    [Description("Id")]
    public required VolumeId Id { get; init; }

    [Description("科目 Id")]
    public required SubjectId SubjectId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("學程 Id")]
    public required BodyOfKnowledgeId BodyOfKnowledgeId { get; init; }
}
