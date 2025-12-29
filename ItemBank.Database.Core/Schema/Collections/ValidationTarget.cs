using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ValidationTargets")]
[Description("驗證目標")]
public class ValidationTarget
{
    [BsonId]
    [Description("Id")]
    public required ValidationTargetId Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("是否必填")]
    public required bool IsRequired { get; init; }

    [Description("是否檢查值")]
    public required bool ShouldCheckValue { get; init; }

    [Description("資料類型")]
    public required string DataType { get; init; }

    [Description("值正規表達式")]
    public string? ValueRegex { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("描述")]
    public required string? Description { get; init; }

    [Description("是否封存")]
    public required bool Archived { get; init; }
}
