using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("圖片工作狀態")]
public enum ItemImageJobStatus
{
    [Description("等待中")]
    Pending,

    [Description("已完成")]
    Completed,

    [Description("失敗")]
    Failed
}
