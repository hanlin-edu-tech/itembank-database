using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("UserConversations")]
[Description("使用者對話")]
public class UserConversation
{
    [BsonId]
    [Description("Id")]
    public required UserConversationId Id { get; init; }

    [Description("使用者 Id")]
    public required UserId UserId { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }

    [Description("最後活動時間")]
    public required DateTime LastActivity { get; init; }

    [Description("是否啟用")]
    public required bool IsActive { get; init; }

    [Description("訊息清單")]
    public required IReadOnlyList<ConversationMessage> Messages { get; init; }

    [Description("總 Token 數")]
    public required int TotalTokens { get; init; }

    [Description("是否需要清理")]
    public required bool RequiresCleanup { get; init; }

    [Description("清理原因")]
    public string? CleanupReason { get; init; }
}

[Description("對話訊息")]
public class ConversationMessage
{
    [BsonId]
    [Description("Id")]
    public required ConversationMessageId Id { get; init; }

    [Description("角色")]
    public required ConversationRole Role { get; init; }

    [Description("內容")]
    public required string Content { get; init; }

    [Description("時間戳記")]
    public required DateTime Timestamp { get; init; }

    [Description("Token 數")]
    public required int TokenCount { get; init; }

    [Description("使用的工具清單")]
    public IReadOnlyList<string>? UsedTools { get; init; }

    [Description("工具結果")]
    public Dictionary<string, object>? ToolResults { get; init; }

    [Description("處理時間（毫秒）")]
    public required int ProcessingTimeMs { get; init; }
}