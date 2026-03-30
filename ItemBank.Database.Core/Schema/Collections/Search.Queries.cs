using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Search.Queries")]
[CollectionSchemaKind(CollectionSchemaKind.FlexibleCollection)]
[CollectionSchemaNote("include 與 exclude 結構不對稱")]
[CollectionSchemaNote("部分關鍵字與版權欄位保留動態型別，以反映異質結構")]
[Description("搜尋查詢")]
public sealed class SearchQuery : IIndexable<SearchQuery>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("查詢")]
    public required SearchQueryQuery Query { get; init; }

    [Description("語意快取 Id")]
    public SemanticCacheId? SemanticCacheId { get; init; }
}

[Description("查詢")]
public sealed class SearchQueryQuery
{
    [Description("排除條件")]
    public required SearchQueryQueryExclude Exclude { get; init; }

    [Description("包含條件")]
    public required SearchQueryQueryInclude Include { get; init; }
}

[Description("查詢排除")]
public sealed class SearchQueryQueryExclude
{
    [Description("作答方式清單")]
    public required IReadOnlyList<string> AnsweringMethods { get; init; }

    [Description("目錄清單")]
    public required IReadOnlyList<CatalogId> Catalogs { get; init; }

    [Description("版權")]
    public IReadOnlyList<BsonValue>? Copyright { get; init; }

    [Description("向度知識 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DimensionKnowledgeIds { get; init; }

    [Description("向度課次 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DimensionLessonIds { get; init; }

    [Description("離散 向度知識 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DiscreteDimensionKnowledgeIds { get; init; }

    [Description("離散 向度 課次 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DiscreteDimensionLessonIds { get; init; }

    [Description("文件 Id 清單")]
    public required IReadOnlyList<DocumentId> DocumentIds { get; init; }

    [Description("文件儲存庫 Id 清單")]
    public required IReadOnlyList<DocumentRepoId> DocumentRepositoryIds { get; init; }

    [Description("編輯備註清單")]
    public required IReadOnlyList<string> EditorialNotes { get; init; }

    [Description("交集關鍵字")]
    public required IReadOnlyList<BsonValue> IntersectionKeywords { get; init; }

    [Description("題目 Id 清單")]
    public required IReadOnlyList<ItemId> ItemIds { get; init; }

    [Description("出處 Id 清單")]
    public required IReadOnlyList<SourceId> SourceIds { get; init; }

    [Description("聯集關鍵字")]
    public required IReadOnlyList<BsonValue> UnionKeywords { get; init; }

    [Description("使用者類型 Id 清單")]
    public required IReadOnlyList<UserTypeId> UserTypeIds { get; init; }

    [Description("年度清單")]
    public required IReadOnlyList<int> Years { get; init; }
}

[Description("查詢包含")]
public sealed class SearchQueryQueryInclude
{
    [Description("作答方式清單")]
    public required IReadOnlyList<string> AnsweringMethods { get; init; }

    [Description("學程 Id")]
    public BodyOfKnowledgeId? BodyOfKnowledgeId { get; init; }

    [Description("目錄清單")]
    public required IReadOnlyList<CatalogId> Catalogs { get; init; }

    [Description("版權")]
    public IReadOnlyList<BsonValue>? Copyright { get; init; }

    [Description("向度知識 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DimensionKnowledgeIds { get; init; }

    [Description("向度課次 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DimensionLessonIds { get; init; }

    [Description("離散 向度知識 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DiscreteDimensionKnowledgeIds { get; init; }

    [Description("離散 向度 課次 Id 清單")]
    public required IReadOnlyList<DimensionValueId> DiscreteDimensionLessonIds { get; init; }

    [Description("文件 Id 清單")]
    public required IReadOnlyList<DocumentId> DocumentIds { get; init; }

    [Description("文件儲存庫 Id 清單")]
    public required IReadOnlyList<DocumentRepoId> DocumentRepositoryIds { get; init; }

    [Description("編輯備註清單")]
    public required IReadOnlyList<string> EditorialNotes { get; init; }

    [Description("交集關鍵字")]
    public required IReadOnlyList<SearchQueryQueryIncludeIntersectionKeyword> IntersectionKeywords { get; init; }

    [Description("是否為素養題")]
    public bool? IsLiteracy { get; init; }

    [Description("是否為題組")]
    public bool? IsSet { get; init; }

    [Description("題目 Id 清單")]
    public required IReadOnlyList<ItemId> ItemIds { get; init; }

    [Description("上線準備度")]
    public bool? OnlineReadiness { get; init; }

    [Description("語意搜尋")]
    public SearchQueryQueryIncludeSemanticSearch? SemanticSearch { get; init; }

    [Description("詳解")]
    public IReadOnlyList<BsonValue>? Solutions { get; init; }

    [Description("出處 Id 清單")]
    public required IReadOnlyList<SourceId> SourceIds { get; init; }

    [Description("科目 Id")]
    public SubjectId? SubjectId { get; init; }

    [Description("主題清單")]
    public required IReadOnlyList<string> Topics { get; init; }

    [Description("聯集關鍵字")]
    public required IReadOnlyList<SearchQueryQueryIncludeUnionKeyword> UnionKeywords { get; init; }

    [Description("使用者類型 Id 清單")]
    public required IReadOnlyList<UserTypeId> UserTypeIds { get; init; }

    [Description("影片連結")]
    public bool? VideoLink { get; init; }

    [Description("年度清單")]
    public required IReadOnlyList<int> Years { get; init; }
}

[Description("查詢包含交集關鍵字")]
public sealed class SearchQueryQueryIncludeIntersectionKeyword
{
    [Description("類型")]
    public required string Type { get; init; }

    [Description("值")]
    public required string Value { get; init; }
}

[Description("查詢包含語意搜尋")]
public sealed class SearchQueryQueryIncludeSemanticSearch
{
    [Description("是否自動拆解")]
    public required bool AutoDecompose { get; init; }

    [Description("查詢")]
    public required string Query { get; init; }
}

[Description("查詢包含聯集關鍵字")]
public sealed class SearchQueryQueryIncludeUnionKeyword
{
    [Description("類型")]
    public required string Type { get; init; }

    [Description("值")]
    public required string Value { get; init; }
}
