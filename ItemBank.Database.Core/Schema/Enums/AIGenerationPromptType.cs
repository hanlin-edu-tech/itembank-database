using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("AI 提示詞類型")]
public enum AIGenerationPromptType
{
    [Description("樣板")]
    Template,

    [Description("片段")]
    Partial
}
