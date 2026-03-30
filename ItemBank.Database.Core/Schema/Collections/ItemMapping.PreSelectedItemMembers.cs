using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemMapping.PreSelectedItemMembers")]
[Description("題目對應預先選取題目成員")]
public sealed class ItemMappingPreSelectedItemMember : IIndexable<ItemMappingPreSelectedItemMember>
{
[BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("文件 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("文件題目 Order")]
    public required int DocumentItemOrder { get; init; }

    [Description("文件 Name")]
    public required string DocumentName { get; init; }

    [Description("文件 Order")]
    public required int DocumentOrder { get; init; }

    [Description("文件儲存庫 Id")]
    public DocumentRepoId? DocumentRepoId { get; init; }

    [Description("年級代碼")]
    public string? GradeCode { get; init; }

    [Description("群組 Id")]
    public required string GroupingId { get; init; }

    [Description("命中清單")]
    public required IReadOnlyList<ItemMappingPreSelectedItemMemberHit> Hits { get; init; }

    [Description("題目向度 Values清單")]
    public required IReadOnlyList<ItemMappingPreSelectedItemMemberItemDimensionValue> ItemDimensionValues { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("系譜內容 Id 清單")]
    public required IReadOnlyList<ProductContentId> PedigreeContentIds { get; init; }

    [Description("系譜章節 Id 清單")]
    public required IReadOnlyList<ProductSectionId> PedigreeSectionIds { get; init; }

    [Description("產品單元表 Id")]
    public required ProductContentId ProductContentId { get; init; }

    [Description("章節 Code")]
    public required string SectionCode { get; init; }

    [Description("章節 Id")]
    public required ProductSectionId SectionId { get; init; }

    [Description("章節 Name")]
    public required string SectionName { get; init; }

    [Description("來源產品內容 Id")]
    public ProductContentId? SourceProductContentId { get; init; }

    [Description("來源年度")]
    public required int SourceYear { get; init; }

    [Description("學期")]
    public required string Terms { get; init; }

    [Description("使用者 類型")]
    public required string UserType { get; init; }

    [Description("冊次代碼")]
    public string? VolumeCode { get; init; }

    [Description("冊次名稱")]
    public required string VolumeName { get; init; }
}

[Description("命中")]
public sealed class ItemMappingPreSelectedItemMemberHit
{
    [Description("向度值清單")]
    public required IReadOnlyList<ItemMappingPreSelectedItemMemberHitDimValue> DimValues { get; init; }
}

[Description("命中向度值")]
public sealed class ItemMappingPreSelectedItemMemberHitDimValue
{
[BsonElement("_id")]
    [Description("Id")]
    public required DimensionValueId Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("類型")]
    public required DimensionType Type { get; init; }

    [Description("使用類型")]
    public required UsageType UsageType { get; init; }
}

[Description("題目向度值")]
public sealed class ItemMappingPreSelectedItemMemberItemDimensionValue
{
[BsonElement("_id")]
    [Description("Id")]
    public required DimensionValueId Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("類型")]
    public required DimensionType Type { get; init; }

    [Description("使用類型")]
    public required UsageType UsageType { get; init; }
}
