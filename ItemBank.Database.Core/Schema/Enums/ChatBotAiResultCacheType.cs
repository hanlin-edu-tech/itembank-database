using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("聊天 AI 快取類型")]
public enum ChatBotAiResultCacheType
{
    [Description("向量")]
    Embedding,

    [Description("切段")]
    Split
}
