using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Documents")]
[Description("五欄檔案")]
public class Document : IIndexable<Document>
{
    [BsonId]
    [Description("Id")]
    public required DocumentId Id { get; init; }

    [Description("五欄檔案儲存庫 Id")]
    public required DocumentRepoId DocumentRepoId { get; init; }

    [Description("學程 Id")]
    public required BodyOfKnowledgeId? BodyOfKnowledgeId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("排序")]
    public required int Order { get; init; }

    [Description("檔案 Id")]
    public required string? FileId { get; init; }

    [Description("檔案名稱")]
    public required string FileName { get; init; }

    [Description("檔案大小")]
    public required long? FileSize { get; init; }

    [Description("檔案內容類型")]
    public string? FileContentType { get; init; }

    [Description("檔案副檔名")]
    public string? FileExtension { get; init; }

    [Description("檔案 URL")]
    public string? FileUrl { get; init; }

    [Description("題目數量")]
    public required int ItemCount { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("父五欄檔案 Id")]
    public required DocumentId? ParentId { get; init; }

    [Description("預檢狀態")]
    public required string PreCheckStatus { get; init; }

    [Description("目錄 Id")]
    public required CatalogId CatalogId { get; init; }

    [Description("產品單元章節 Id 清單")]
    public required IReadOnlyList<ProductSectionId> ProductSectionIds { get; init; }

    [Description("年度")]
    public required int Year { get; init; }

    static IReadOnlyList<CreateIndexModel<Document>> IIndexable<Document>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<Document>.IndexKeys.Ascending(x => x.DocumentRepoId)
        ),
        new(
            Builders<Document>.IndexKeys.Ascending(x => x.ProductSectionIds)
        )
    ];
}
