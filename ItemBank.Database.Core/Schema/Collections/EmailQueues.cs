using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("EmailQueues")]
[Description("Email 佇列")]
public sealed class EmailQueue : IIndexable<EmailQueue>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("應用程式")]
    public required string App { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("錯誤訊息")]
    public string? ErrorMessage { get; init; }

    [Description("事件")]
    public required string Event { get; init; }

    [Description("HTML 內容")]
    public required string HtmlContent { get; init; }

    [Description("是否Sent")]
    public required bool IsSent { get; init; }

    [Description("純文字 內容")]
    public required string PlainTextContent { get; init; }

    [Description("寄送時間")]
    public required DateTime SentAt { get; init; }

    [Description("主旨")]
    public required string Subject { get; init; }

    [Description("收件人清單")]
    public required IReadOnlyList<string> To { get; init; }
}
