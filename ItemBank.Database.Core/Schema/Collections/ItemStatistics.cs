using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemStatistics")]
[Description("題目統計")]
public class ItemStatistics
{
    [BsonId]
    [Description("Id")]
    public required ItemId Id { get; init; }

    [Description("全部每題時長")]
    public double? AllEachDuration { get; init; }

    [Description("正確率")]
    public double? CorrectRate { get; init; }

    [Description("難度")]
    public int? Difficulty { get; init; }

    [Description("初始難度")]
    public int? InitialDifficulty { get; init; }

    [Description("可靠標記")]
    public int? ReliableFlag { get; init; }

    [Description("樣本數")]
    public int? SampleSize { get; init; }

    [Description("答對每題時長")]
    public double? TEachDuration { get; init; }

    [Description("答錯每題時長")]
    public double? WEachDuration { get; init; }

    [Description("時間戳記")]
    public string? TimeStamp { get; init; }

    [Description("子題統計清單")]
    public IReadOnlyList<ItemStatisticsQuestion>? Questions { get; init; }
}

[Description("子題統計")]
public class ItemStatisticsQuestion
{
    [Description("答案清單")]
    public IReadOnlyList<ItemStatisticsQuestionAnswer>? Answers { get; init; }

    [Description("子題索引")]
    public int? QuestionIndex { get; init; }

    [Description("是否為陷阱題")]
    public bool? TrapItem { get; init; }

    [Description("選項陷阱清單")]
    public IReadOnlyList<string>? OptionTrap { get; init; }

    [Description("是否為基礎題")]
    public bool? BaseItem { get; init; }

    [Description("難度")]
    public int? Difficulty { get; init; }

    [Description("正確率")]
    public double? CorrectRate { get; init; }

    [Description("選項統計清單")]
    public IReadOnlyList<ItemStatisticsOptionStats>? OptionStats { get; init; }
}

[Description("子題答案統計")]
public class ItemStatisticsQuestionAnswer
{
    [Description("答案 Id")]
    public int? AnswerId { get; init; }

    [Description("正確率")]
    public double? CorrectRate { get; init; }

    [Description("難度")]
    public int? Difficulty { get; init; }
}

[Description("選項統計")]
public class ItemStatisticsOptionStats
{
    [Description("選擇率")]
    public double? ChosenRate { get; init; }

    [Description("選項索引")]
    public int? OptionIndex { get; init; }
}
