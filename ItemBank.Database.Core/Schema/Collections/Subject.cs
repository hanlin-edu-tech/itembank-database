using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Subjects")]
[Description("科目")]
public sealed class Subject : IIndexable<Subject>
{
    [BsonId]
    [Description("Id")]
    public required SubjectId Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("父科目 Id")]
    public required SubjectId? ParentSubjectId { get; init; }

    [Description("教育代碼")]
    public required string EducationCode { get; init; }

    [Description("教育名稱")]
    public required string EducationName { get; init; }

    [Description("簡稱")]
    public required string SimpleName { get; init; }

    [Description("教育排序索引")]
    public required int EducationOrderIndex { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("領域")]
    public required string Area { get; init; }
}
