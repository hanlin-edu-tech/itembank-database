using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("Word 匯出狀態")]
public enum WordExportStatus
{
    [Description("等待中")]
    Pending,

    [Description("已匯出")]
    Exported,

    [Description("失敗")]
    Failed
}
