using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ContentSections")]
[Description("內容章節")]
public sealed class ContentSection : IIndexable<ContentSection>
{
[BsonId]
    [Description("Id")]
    public required ContentSectionId Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("內容 Id")]
    public required ContentId ContentId { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("描述")]
    public string? Description { get; init; }

    [Description("向度值 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DimensionValueIds { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("路徑")]
    public required IReadOnlyList<ContentSectionId> Path { get; init; }

    [Description("段考清單")]
    public required IReadOnlyList<string> TermTests { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    [Description("冊次名稱")]
    public required string VolumeName { get; init; }

    static IReadOnlyList<CreateIndexModel<ContentSection>> IIndexable<ContentSection>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ContentSection>.IndexKeys.Ascending("dimensionValueIds"),
                new CreateIndexOptions<ContentSection>
                {
                    Name = "dimensionValueIds_1"
                }
        )
    ];
}
