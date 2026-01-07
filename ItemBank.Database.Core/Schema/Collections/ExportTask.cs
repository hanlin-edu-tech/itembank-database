using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ExportTasks")]
[Description("匯出任務")]
public class ExportTask : IIndexable<ExportTask>
{
    [BsonId]
    [Description("Id")]
    public required ExportTaskId Id { get; init; }

    [Description("狀態")]
    public required string Status { get; init; }

    [Description("使用者 Id")]
    public required UserId UserId { get; init; }

    [Description("匯出規格")]
    public required ExportSpec Spec { get; init; }

    [Description("接收時間")]
    public required DateTime ReceivedOn { get; init; }

    [Description("開始處理時間")]
    public DateTime? StartedOn { get; init; }

    [Description("完成時間")]
    public DateTime? FinishedOn { get; init; }

    [Description("API 開始時間")]
    public DateTime? ApiStartedOn { get; init; }

    [Description("API 完成時間")]
    public DateTime? ApiFinishedOn { get; init; }

    [Description("渲染開始時間")]
    public DateTime? RenderStartedOn { get; init; }

    [Description("渲染完成時間")]
    public DateTime? RenderFinishedOn { get; init; }

    [Description("總五欄檔案儲存庫數量")]
    public int? TotalDocumentRepositoryCount { get; init; }

    [Description("總檔案數量")]
    public int? TotalFileCount { get; init; }

    [Description("總題目數量")]
    public int? TotalItemCount { get; init; }

    [Description("檔案名稱")]
    public string? Filename { get; init; }

    [Description("檔案 URL")]
    public string? FileUrl { get; init; }

    [Description("訊息清單")]
    public required IReadOnlyList<string> Messages { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    static IReadOnlyList<CreateIndexModel<ExportTask>> IIndexable<ExportTask>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ExportTask>.IndexKeys.Ascending(x => x.UserId)
        ),
        new(
            Builders<ExportTask>.IndexKeys.Ascending(x => x.Status)
        )
    ];
}

[Description("匯出規格")]
public class ExportSpec
{
    [Description("匯出名稱")]
    public string? ExportName { get; init; }

    [Description("匯出格式")]
    public string? ExportFormat { get; init; }

    [Description("五欄檔案儲存庫 Id 清單")]
    public required IReadOnlyList<DocumentRepoId> DocumentRepositoryIds { get; init; }

    [Description("五欄檔案 Id 清單")]
    public required IReadOnlyList<DocumentId> DocumentIds { get; init; }

    [Description("題目 Id 清單")]
    public required IReadOnlyList<ItemId> ItemIds { get; init; }

    [Description("匯出類型")]
    public required ExportType ExportType { get; init; }

    [Description("壓縮模式")]
    public required ExportArchiveMode ArchiveMode { get; init; }
}
