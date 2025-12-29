using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("匯出壓縮模式")]
public enum ExportArchiveMode
{
    [Description("總是壓縮")]
    Always,

    [Description("自動判斷")]
    Auto
}
