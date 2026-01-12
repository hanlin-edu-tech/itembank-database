using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("DocumentRepositories")]
[Description("五欄檔案儲存庫")]
public class DocumentRepository : IIndexable<DocumentRepository>
{
    [BsonId]
    [Description("Id")]
    public required DocumentRepoId Id { get; init; }

    [Description("目錄 Id")]
    public required CatalogId CatalogId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("排序")]
    public required int Order { get; init; }

    [Description("屬性")]
    public DocumentRepoProperty? Properties { get; init; }

    [Description("是否曾被鎖定")]
    public required bool LockedBefore { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    [Description("第一階段")]
    public DocumentRepoStage? StageOne { get; init; }

    [Description("第二階段")]
    public DocumentRepoStage? StageTwo { get; init; }

    [Description("學程 Id")]
    public BodyOfKnowledgeId? BodyOfKnowledgeId { get; init; }

    [Description("標記題目數量記錄清單")]
    public required IReadOnlyList<TaggedQuestionCountRecord> TaggedQuestionCountRecords { get; init; }

    [Description("產品單元表 Id 清單")]
    public required IReadOnlyList<ProductContentId> ProductContentIds { get; init; }

    [Description("包裹 Id")]
    public PackageId? PackageId { get; init; }

    static IReadOnlyList<CreateIndexModel<DocumentRepository>> IIndexable<DocumentRepository>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<DocumentRepository>.IndexKeys.Ascending(x => x.CatalogId)
        ),
        new(
            Builders<DocumentRepository>.IndexKeys.Ascending(x => x.ProductContentIds)
        )
    ];
}

[Description("標記題目數量記錄")]
public class TaggedQuestionCountRecord
{
    [Description("產品單元表 Id")]
    public required ProductContentId? ProductContentId { get; init; }

    [Description("使用者 Id")]
    public required UserId? UserId { get; init; }

    [Description("標記時間")]
    public required DateTime TaggedOn { get; init; }
}

[Description("五欄檔案儲存階段")]
public class DocumentRepoStage
{
    [Description("擁有者")]
    public required UserId Owner { get; init; }

    [Description("部門")]
    public required string Department { get; init; }

    [Description("完成時間")]
    public DateTime? AtcDate { get; init; }

    [Description("預計完成時間")]
    public DateTime? EtcDate { get; init; }
}

[Description("五欄檔案儲存庫屬性")]
public class DocumentRepoProperty : Dictionary<string, object>;
