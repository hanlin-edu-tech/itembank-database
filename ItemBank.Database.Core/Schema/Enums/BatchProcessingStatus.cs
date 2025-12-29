using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("批次處理狀態")]
public enum BatchProcessingStatus
{
    [Description("待處理")]
    Pending,

    [Description("處理中")]
    Running,

    [Description("已完成")]
    Completed,

    [Description("失敗")]
    Failed,

    [Description("已取消")]
    Cancelled
}
