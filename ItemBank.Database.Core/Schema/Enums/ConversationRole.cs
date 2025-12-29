using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("對話角色")]
public enum ConversationRole
{
    [Description("使用者")]
    User = 0,

    [Description("助理")]
    Assistant = 1,

    [Description("系統")]
    System = 2
}
