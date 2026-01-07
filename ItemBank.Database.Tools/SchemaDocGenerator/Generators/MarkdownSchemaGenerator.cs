using System.Text;
using ItemBank.Database.Tools.SchemaDocGenerator.Models;

namespace ItemBank.Database.Tools.SchemaDocGenerator.Generators;

/// <summary>
/// Markdown Schema 文件生成器
/// </summary>
public sealed class MarkdownSchemaGenerator : ISchemaDocGenerator
{
    public string Generate(SchemaDocument document)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# ItemBank Schema Documentation");
        sb.AppendLine();

        // 先輸出全局 Enum 定義
        if (document.Enums.Any())
        {
            sb.AppendLine("## Enums");
            sb.AppendLine();

            foreach (var (enumName, enumValues) in document.Enums.OrderBy(kvp => kvp.Key))
            {
                sb.AppendLine($"### {enumName}");
                sb.AppendLine();
                foreach (var (name, value) in enumValues)
                {
                    sb.AppendLine($"- {name}: `{value}`");
                }
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine();
        }

        // 再輸出集合定義
        for (int i = 0; i < document.Collections.Count; i++)
        {
            var schema = document.Collections[i];
            GenerateCollection(sb, schema);

            // 如果不是最後一個集合，加入分隔線
            if (i < document.Collections.Count - 1)
            {
                sb.AppendLine("---");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    private void GenerateCollection(StringBuilder sb, CollectionSchema schema)
    {
        sb.AppendLine($"## {schema.CollectionName}");
        sb.AppendLine();
        sb.AppendLine($"**描述**: {schema.Description}");
        sb.AppendLine();

        // 索引
        if (schema.Indices.Any())
        {
            sb.AppendLine("### 索引");
            sb.AppendLine();
            sb.AppendLine("| 索引名稱 | 欄位 | 方向 |");
            sb.AppendLine("|---------|------|------|");

            foreach (var index in schema.Indices)
            {
                if (index.Fields.Any())
                {
                    // 顯示第一個欄位
                    var firstField = index.Fields[0];
                    sb.AppendLine($"| {EscapeMarkdown(index.Name)} | {EscapeMarkdown(firstField.FieldName)} | {firstField.Direction} |");

                    // 如果有多個欄位，繼續顯示
                    for (int i = 1; i < index.Fields.Count; i++)
                    {
                        var field = index.Fields[i];
                        sb.AppendLine($"|  | {EscapeMarkdown(field.FieldName)} | {field.Direction} |");
                    }
                }
                else
                {
                    sb.AppendLine($"| {EscapeMarkdown(index.Name)} | - | - |");
                }
            }

            sb.AppendLine();
        }

        sb.AppendLine("### 欄位");
        sb.AppendLine();
        sb.AppendLine("| 欄位名稱 | 型別 | 描述 |");
        sb.AppendLine("|---------|------|------|");

        // 扁平化所有欄位
        var flattenedFields = FlattenFields(schema.Fields);
        foreach (var (fieldName, type, description) in flattenedFields)
        {
            sb.AppendLine($"| {EscapeMarkdown(fieldName)} | `{EscapeMarkdown(type)}` | {EscapeMarkdown(description)} |");
        }

        sb.AppendLine();
    }

    /// <summary>
    /// 扁平化欄位結構
    /// </summary>
    /// <returns>欄位清單 (欄位名稱, 型別, 描述)</returns>
    private List<(string FieldName, string Type, string Description)> FlattenFields(
        IReadOnlyDictionary<string, FieldSchema> fields,
        string prefix = "")
    {
        var result = new List<(string, string, string)>();

        foreach (var (fieldName, field) in fields)
        {
            var camelCaseFieldName = ToCamelCase(fieldName);
            var fullFieldName = string.IsNullOrEmpty(prefix) ? camelCaseFieldName : $"{prefix}.{camelCaseFieldName}";

            // 建構型別字串（包含 id_type 和 enum_type）
            var typeStr = field.Type;
            if (field.IdType != null)
            {
                typeStr += $" (IdType: {field.IdType})";
            }
            if (field.EnumType != null)
            {
                typeStr += $" (EnumType: {field.EnumType})";
            }

            // 加入當前欄位
            result.Add((fullFieldName, typeStr, field.Description));

            // 如果有嵌套欄位，遞迴處理
            if (field.Fields != null && field.Fields.Any())
            {
                // 判斷是否為陣列型別
                var nestedPrefix = field.Type.StartsWith("array<")
                    ? $"{fullFieldName}[]"
                    : fullFieldName;

                var nestedFields = FlattenFields(field.Fields, nestedPrefix);
                result.AddRange(nestedFields);
            }
        }

        return result;
    }

    /// <summary>
    /// 轉義 Markdown 特殊字元
    /// </summary>
    private string EscapeMarkdown(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Replace("|", "\\|");
    }

    /// <summary>
    /// 將 PascalCase 轉換為 camelCase
    /// </summary>
    private string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (value.Length == 1)
            return value.ToLowerInvariant();

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}
