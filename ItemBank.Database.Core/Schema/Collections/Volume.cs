using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("冊次")]
public class Volume
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("科目 Id")]
    public required string SubjectId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("學程 Id")]
    public required string BodyOfKnowledgeId { get; init; }
}
