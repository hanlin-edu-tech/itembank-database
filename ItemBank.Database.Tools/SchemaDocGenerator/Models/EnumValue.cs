namespace ItemBank.Database.Tools.SchemaDocGenerator.Models;

/// <summary>
/// Enum 值的封裝，包含序列化類型和實際值
/// </summary>
public sealed record EnumValue
{
    /// <summary>序列化類型</summary>
    public required EnumSerializationType SerializationType { get; init; }

    /// <summary>C# Enum 成員名稱（如 "Knowledge"）</summary>
    public required string Name { get; init; }

    /// <summary>整數值（如 0, 1, 2）</summary>
    public required int IntValue { get; init; }

    /// <summary>
    /// 轉換為資料庫中實際儲存的原始字串
    /// </summary>
    /// <returns>
    /// - Integer: "0", "1", "2"
    /// - CamelCase: "knowledge", "lesson"
    /// - PascalCase: "Knowledge", "Lesson"
    /// </returns>
    public string ToRawString()
    {
        return SerializationType switch
        {
            EnumSerializationType.Integer => IntValue.ToString(),
            EnumSerializationType.CamelCase => ToCamelCase(Name),
            EnumSerializationType.PascalCase => Name,
            _ => Name
        };
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        if (value.Length == 1) return value.ToLowerInvariant();
        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}

/// <summary>
/// Enum 序列化類型
/// </summary>
public enum EnumSerializationType
{
    /// <summary>序列化為整數</summary>
    Integer,

    /// <summary>序列化為 camelCase 字串</summary>
    CamelCase,

    /// <summary>序列化為 PascalCase 字串</summary>
    PascalCase,

    /// <summary>預設字串序列化</summary>
    String
}
