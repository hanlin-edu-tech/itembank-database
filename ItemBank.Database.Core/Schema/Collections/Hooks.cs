using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Hooks")]
[CollectionSchemaKind(CollectionSchemaKind.FlexibleCollection)]
[CollectionSchemaNote("callbackContent.filter 為整合導向的寬鬆結構")]
[Description("Webhook")]
public sealed class Hook : IIndexable<Hook>
{
[BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("回呼 內容")]
    public required HookCallbackContent CallbackContent { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("類型")]
    public string? Type { get; init; }
}

[Description("回呼 內容")]
public sealed class HookCallbackContent
{
    [Description("過濾條件")]
    public required HookCallbackContentFilter Filter { get; init; }

    [Description("方法")]
    public required string Method { get; init; }

    [Description("URL")]
    public required string Url { get; init; }
}

[Description("回呼 內容 過濾條件")]
public sealed class HookCallbackContentFilter
{
    [Description("目錄 Id 清單")]
    public IReadOnlyList<CatalogId>? CatalogIds { get; init; }

    [Description("文件儲存庫 Id 清單")]
    public required IReadOnlyList<DocumentRepoId> DocumentRepositoryIds { get; init; }

    [Description("標籤清單")]
    public IReadOnlyList<string>? Labels { get; init; }

    [Description("產品代碼清單")]
    public IReadOnlyList<string>? ProductCodes { get; init; }
}
