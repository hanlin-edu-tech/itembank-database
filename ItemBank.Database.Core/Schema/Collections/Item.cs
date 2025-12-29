using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Items")]
[Description("題目")]
public class Item
{
    [BsonId]
    [Description("Id")]
    public required ItemId Id { get; init; }

    [Description("難度")]
    public required double? Difficulty { get; init; }

    [Description("正確性")]
    public required string Correctness { get; init; }

    [Description("上線準備度")]
    public required string OnlineReadiness { get; init; }

    [Description("元資料")]
    public required Dictionary<string, string> Metadata { get; init; }

    [Description("內容")]
    public required ItemContent Content { get; init; }

    [Description("科目 Id 清單")]
    public required IReadOnlyList<SubjectId> SubjectIds { get; init; }

    [Description("資源連結清單")]
    public required IReadOnlyList<ItemResourceLink> ResourceLinks { get; init; }

    [Description("內容資源清單")]
    public required IReadOnlyList<ItemContentResourceManifest> ContentResourceManifest { get; init; }

    [Description("文字識別碼")]
    public string? TextIdentifier { get; init; }

    [Description("識別碼")]
    public string? Identifier { get; init; }

    [Description("圖片位置識別碼")]
    public string? ImagePositionIdentifier { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("版本")]
    public required int Ver { get; init; }

    [Description("版權")]
    public required CopyrightType? Copyright { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    [Description("比對人員")]
    public required string? Matcher { get; init; }

    [Description("議題")]
    public required string? Topic { get; init; }

    [Description("編輯備註")]
    public required string? EditorialNote { get; init; }

    [Description("素養題")]
    public required bool Literacy { get; init; }

    [Description("強制單題")]
    public required bool ForcedSingle { get; init; }

    [Description("答案數")]
    public required int? UserAnswerCount { get; init; }

    [Description("是否有表格")]
    public required bool HasTable { get; init; }
}

[Description("題目內容")]
public class ItemContent
{
    [Description("前言")]
    public string? Preamble { get; init; }

    [Description("解析")]
    public string? Solution { get; init; }

    [Description("完整內容")]
    public string? FullContent { get; init; }

    [Description("子題清單")]
    public required IReadOnlyList<ItemQuestionContent> Questions { get; init; }
}

[Description("子題內容")]
public class ItemQuestionContent
{
    [Description("題幹")]
    public string? Stem { get; init; }

    [Description("子題索引")]
    public required int QuestionIndex { get; init; }

    [Description("選項首字母")]
    public string? OptionFirstLetter { get; init; }

    [Description("解析")]
    public string? Solution { get; init; }

    [Description("選項清單")]
    public required IReadOnlyList<string> Options { get; init; }

    [Description("答案清單")]
    public required IReadOnlyList<IReadOnlyList<string>> Answers { get; init; }

    [Description("答案圖片 Id 清單")]
    public required IReadOnlyList<IReadOnlyList<string>> AnswerImageIds { get; init; }

    [Description("LaTeX 答案清單")]
    public required IReadOnlyList<bool> LatexAnswers { get; init; }

    [Description("建議答案清單")]
    public required IReadOnlyList<IReadOnlyList<string>> ProposeAnswers { get; init; }

    [Description("推薦答案清單")]
    public IReadOnlyList<IReadOnlyList<string>>? SuggestAnswers { get; init; }

    [Description("難度")]
    public double? Difficulty { get; init; }

    [Description("作答方式")]
    public required string AnsweringMethod { get; init; }

    [Description("是否為順序選項")]
    public required bool IsSequenceOption { get; init; }
}

[Description("圖片處理結果")]
public class ImageProcessingResult
{
    [Description("類型")]
    public required string Type { get; init; }

    [Description("內容")]
    public string? Content { get; init; }

    [Description("信心度")]
    public required double Confidence { get; init; }

    [Description("思考過程")]
    public required string Thinking { get; init; }

    [Description("處理時間")]
    public required DateTime ProcessedAt { get; init; }

    [Description("模型版本")]
    public required string ModelVersion { get; init; }
}

[Description("題目資源連結")]
public class ItemResourceLink
{
    [Description("名稱")]
    public required string Name { get; init; }

    [Description("關係")]
    public required string Rel { get; init; }

    [Description("超連結")]
    public required string Href { get; init; }

    [Description("內容類型")]
    public required string ContentType { get; init; }
}

[Description("題目內容資源清單")]
public class ItemContentResourceManifest
{
    [Description("Id")]
    public required string Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("內容類型")]
    public required string ContentType { get; init; }

    [Description("大小")]
    public required int Size { get; init; }

    [Description("圖片 SHA512 雜湊值")]
    public string? ImageSha512Hash { get; init; }
}
