using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Catalogs")]
[Description("目錄")]
public class Catalog
{
    [BsonId]
    [Description("Id")]
    public required CatalogId Id { get; init; }

    [Description("產品 Id")]
    public required CatalogGroupId CatalogGroupId { get; init; }

    [Description("群組名稱")]
    public required string GroupingName { get; init; }

    [Description("群組代碼")]
    public required string GroupingCode { get; init; }

    [Description("基礎名稱")]
    public required string BaseName { get; init; }

    [Description("科目 Id")]
    public required SubjectId SubjectId { get; init; }

    [Description("科目名稱")]
    public required string SubjectName { get; init; }

    [Description("版本")]
    public required string Version { get; init; }

    [Description("版本 Id")]
    public VersionId? VersionId { get; init; }

    [Description("年度")]
    public required int Year { get; init; }

    [Description("標籤清單")]
    public required IReadOnlyList<string> Tags { get; init; }

    [Description("是否可設定")]
    public required bool CanConfigure { get; init; }

    [Description("備註")]
    public string? Description { get; init; }

    [Description("是否為主科")]
    public required bool IsCoreSubject { get; init; }

    [Description("擁有者")]
    public required string Owner { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    [Description("驗證規則清單")]
    public required IReadOnlyList<CatalogValidationRule> ValidationRules { get; init; }
}

[Description("目錄驗證規則，從 ValidationTarget 衍生而來")]
public class CatalogValidationRule
{
    [Description("目標名稱")]
    public required string TargetName { get; init; }

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

    [Description("備註")]
    public string? Description { get; init; }
}
