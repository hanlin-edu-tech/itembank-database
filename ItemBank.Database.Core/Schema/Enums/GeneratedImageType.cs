using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("AI 生成圖片類型")]
public enum GeneratedImageType
{
    [Description("圖像")]
    Figure,

    [Description("數線")]
    Numberline
}
