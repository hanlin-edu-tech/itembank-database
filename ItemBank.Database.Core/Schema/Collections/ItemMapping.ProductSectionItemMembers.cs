using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemMapping.ProductSectionItemMembers")]
[Description("產品單元章節題目成員")]
public class ProductSectionItemMember : IIndexable<ProductSectionItemMember>
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("產品單元章節 Id")]
    public required ProductSectionId SectionId { get; init; }

    [Description("產品單元章節代碼")]
    public required string SectionCode { get; init; }

    [Description("產品單元章節名稱")]
    public required string SectionName { get; init; }

    [Description("產品單元 Id")]
    public required ProductContentId ProductContentId { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("五欄檔案 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("五欄檔案名稱")]
    public required string DocumentName { get; init; }

    [Description("五欄檔案儲存庫 Id")]
    public required DocumentRepoId DocumentRepoId { get; init; }

    [Description("五欄檔案題目排序")]
    public required int DocumentItemOrder { get; init; }

    [Description("五欄檔案排序")]
    public required int DocumentOrder { get; init; }

    [Description("命中清單")]
    public required IReadOnlyList<ProjectSectionHit> Hits { get; init; }

    [Description("五欄檔案連結")]
    public required bool DocumentLink { get; init; }

    [Description("族譜產品單元 Id 清單")]
    public required IReadOnlyList<ProductContentId> PedigreeContentIds { get; init; }

    [Description("族譜產品單元章節 Id 清單")]
    public required IReadOnlyList<ProductSectionId> PedigreeSectionIds { get; init; }

    [Description("群組 Id")]
    public required string GroupingId { get; init; }

    [Description("冊次代碼")]
    public required string VolumeCode { get; init; }

    [Description("冊次名稱")]
    public required string VolumeName { get; init; }

    [Description("年級代碼")]
    public required string GradeCode { get; init; }

    [Description("期")]
    public required string Terms { get; init; }

    static IReadOnlyList<CreateIndexModel<ProductSectionItemMember>> IIndexable<ProductSectionItemMember>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ProductSectionItemMember>.IndexKeys
                .Ascending(x => x.ProductContentId)
                .Ascending(x => x.DocumentRepoId)
        )
    ];
}

[Description("產品單元章節儲存庫題目成員")]
public class ProductSectionRepoItemMember
{
    [Description("五欄檔案儲存庫 Id")]
    public required DocumentRepoId DocumentRepoId { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("產品單元 Id")]
    public required ProductContentId ProductContentId { get; init; }
}

[Description("產品單元章節命中")]
public class ProjectSectionHit
{
    [Description("向度資訊清單")]
    public required IReadOnlyList<ProjectSectionItemHitDimensionValue> DimValues { get; init; }
}

[Description("產品單元章節題目命中向度資訊")]
public class ProjectSectionItemHitDimensionValue
{
    [Description("向度資訊 Id")]
    public required DimensionValueId Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("類型")]
    public required DimensionType Type { get; init; }

    [Description("使用類型")]
    public required UsageType UsageType { get; init; }
}
