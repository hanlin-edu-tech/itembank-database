using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Search.Items")]
[Description("搜尋題目")]
public sealed class SearchItem : IIndexable<SearchItem>
{
[BsonId]
    [Description("Id")]
    public required ItemId Id { get; init; }

    [Description("內容")]
    public required SearchItemContent Content { get; init; }

    [Description("內容同步時間")]
    public required DateTime ContentSyncedAt { get; init; }

    [Description("版權")]
    public string? Copyright { get; init; }

    [Description("正確性")]
    public required string Correctness { get; init; }

    [Description("文件清單")]
    public required IReadOnlyList<SearchItemDocument> Documents { get; init; }

    [Description("編輯備註清單")]
    public string? EditorialNotes { get; init; }

    [Description("嵌入資源清單")]
    public IReadOnlyList<BsonValue>? EmbedResources { get; init; }

    [Description("是否有詳解")]
    public required bool HasSolution { get; init; }

    [Description("是否有影片")]
    public required bool HasVideo { get; init; }

    [Description("是否為題組")]
    public required bool IsSet { get; init; }

    [Description("題目年度清單")]
    public required IReadOnlyList<SearchItemItemYear> ItemYears { get; init; }

    [Description("素養")]
    public required bool Literacy { get; init; }

    [Description("中繼資料清單")]
    public required IReadOnlyList<SearchItemMetadataList> MetadataList { get; init; }

    [Description("上線準備度")]
    public required OnlineReadinessStatus OnlineReadiness { get; init; }

    [Description("上線 狀態")]
    public required bool OnlineStatus { get; init; }

    [Description("資源連結清單")]
    public required IReadOnlyList<SearchItemResourceLink> ResourceLinks { get; init; }

    [Description("科目 Id 清單")]
    public required IReadOnlyList<SubjectId> SubjectIds { get; init; }

    [Description("主題")]
    public string? Topic { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    static IReadOnlyList<CreateIndexModel<SearchItem>> IIndexable<SearchItem>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<SearchItem>.IndexKeys.Ascending("contentSyncedAt"),
                new CreateIndexOptions<SearchItem>
                {
                    Name = "contentSyncedAt_1"
                }
        ),
        new(
            Builders<SearchItem>.IndexKeys.Descending("updatedOn"),
                new CreateIndexOptions<SearchItem>
                {
                    Name = "updatedOn_-1"
                }
        )
    ];
}

[Description("內容")]
public sealed class SearchItemContent
{
    [Description("前言")]
    public string? Preamble { get; init; }

    [Description("題目清單")]
    public required IReadOnlyList<SearchItemContentQuestion> Questions { get; init; }

    [Description("詳解")]
    public required string Solution { get; init; }
}

[Description("內容題目")]
public sealed class SearchItemContentQuestion
{
    [Description("作答方式")]
    public required string AnsweringMethod { get; init; }

    [Description("答案")]
    public required IReadOnlyList<BsonArray> Answers { get; init; }

    [Description("難度")]
    public double? Difficulty { get; init; }

    [Description("是否依序選項")]
    public required bool IsSequenceOption { get; init; }

    [Description("LaTeX 答案清單")]
    public IReadOnlyList<bool>? LatexAnswers { get; init; }

    [Description("選項首字母")]
    public string? OptionFirstLetter { get; init; }

    [Description("選項清單")]
    public required IReadOnlyList<string> Options { get; init; }

    [Description("建議答案清單")]
    public required IReadOnlyList<BsonArray> ProposeAnswers { get; init; }

    [Description("詳解")]
    public string? Solution { get; init; }

    [Description("題幹")]
    public required string Stem { get; init; }
}

[Description("文件")]
public sealed class SearchItemDocument
{
[BsonElement("_id")]
    [Description("Id")]
    public required DocumentId Id { get; init; }

    [Description("學程 Id")]
    public BodyOfKnowledgeId? BodyOfKnowledgeId { get; init; }

    [Description("目錄 Id")]
    public required CatalogId CatalogId { get; init; }

    [Description("儲存庫 Id")]
    public required RepositoryId RepositoryId { get; init; }

    [Description("年度")]
    public required int Year { get; init; }
}

[Description("題目年度")]
public sealed class SearchItemItemYear
{
    [Description("學程 Id")]
    public required BodyOfKnowledgeId BodyOfKnowledgeId { get; init; }

    [Description("向度值 Id")]
    public required DimensionValueId DimensionValueId { get; init; }

    [Description("使用類型")]
    public required UsageType UsageType { get; init; }

    [Description("年度")]
    public required int Year { get; init; }
}

[Description("中繼資料 List")]
public sealed class SearchItemMetadataList
{
[BsonElement("_id")]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("類型")]
    public required MetadataType Type { get; init; }
}

[Description("資源連結")]
public sealed class SearchItemResourceLink
{
    [Description("內容 類型")]
    public required string ContentType { get; init; }

    [Description("連結")]
    public required string Href { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("關聯")]
    public required string Rel { get; init; }
}
