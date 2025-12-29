using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("處理類型")]
public enum ProcessingType
{
    [Description("LaTeX")]
    Latex,

    [Description("純文字")]
    Text,

    [Description("HTML")]
    Html,

    [Description("錯誤")]
    Error
}
