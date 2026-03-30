using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AIGeneration.ItemShells")]
[Description("AI 題殼")]
public sealed class AIGenerationItemShell : IIndexable<AIGenerationItemShell>
{
[BsonId]
    [Description("Id")]
    public required ItemShellId Id { get; init; }

    [Description("評量目標")]
    public required AIGenerationItemShellAssessmentGoal AssessmentGoal { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("題型")]
    public required string ItemType { get; init; }

    [Description("LLM 模型")]
    public required string LlmModel { get; init; }

    [Description("數學分析")]
    public AIGenerationItemShellMathAnalysis? MathAnalysis { get; init; }

    [Description("題殼")]
    public required AIGenerationItemShellShell Shell { get; init; }

    [Description("科目 Id")]
    public required SubjectId SubjectId { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }
}

[Description("評量目標")]
public sealed class AIGenerationItemShellAssessmentGoal
{
    [Description("題目清單")]
    public required IReadOnlyList<AIGenerationItemShellAssessmentGoalQuestion> Questions { get; init; }

    [Description("摘要")]
    public required string Summary { get; init; }
}

[Description("評量目標題目")]
public sealed class AIGenerationItemShellAssessmentGoalQuestion
{
    [Description("能力")]
    public required string Ability { get; init; }

    [Description("索引")]
    public required int Index { get; init; }

    [Description("題目樣式")]
    public required string QuestionPattern { get; init; }
}

[Description("數學分析")]
public sealed class AIGenerationItemShellMathAnalysis
{
    [Description("題目解法清單")]
    public required IReadOnlyList<string> QuestionApproach { get; init; }

    [Description("解題概念清單")]
    public required IReadOnlyList<string> SolvingConcept { get; init; }
}

[Description("題殼")]
public sealed class AIGenerationItemShellShell
{
    [Description("描述")]
    public required string Description { get; init; }

    [Description("是否有題幹")]
    public required bool HasShell { get; init; }

    [Description("需求")]
    public required string Required { get; init; }

    [Description("來源")]
    public required string Source { get; init; }

    [Description("類型")]
    public required string Type { get; init; }
}
