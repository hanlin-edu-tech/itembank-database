using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("AI 生成任務狀態")]
public enum AIGenerationTaskStatus
{
    [Description("已完成")]
    Completed,

    [Description("處理中")]
    Processing
}
