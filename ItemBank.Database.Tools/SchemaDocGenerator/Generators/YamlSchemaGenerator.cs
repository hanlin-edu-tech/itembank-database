using System.Text;
using ItemBank.Database.Tools.SchemaDocGenerator.Models;

namespace ItemBank.Database.Tools.SchemaDocGenerator.Generators;

/// <summary>
/// YAML Schema 文件生成器
/// </summary>
public sealed class YamlSchemaGenerator : ISchemaDocGenerator
{
    public string Generate(SchemaDocument document)
    {
        var sb = new StringBuilder();

        // 先輸出全局 Enum 定義
        if (document.Enums.Any())
        {
            sb.AppendLine("enums:");
            foreach (var (enumName, enumValues) in document.Enums.OrderBy(kvp => kvp.Key))
            {
                sb.AppendLine($"  {enumName}:");
                foreach (var (name, value) in enumValues)
                {
                    sb.AppendLine($"    {name}: {value}");
                }
            }
            sb.AppendLine();
        }

        // 再輸出集合定義
        sb.AppendLine("collections:");
        foreach (var schema in document.Collections)
        {
            GenerateCollection(sb, schema, indent: 1);
        }

        return sb.ToString();
    }

    private void GenerateCollection(StringBuilder sb, CollectionSchema schema, int indent)
    {
        var indentStr = new string(' ', indent * 2);

        sb.AppendLine($"{indentStr}{schema.CollectionName}:");
        sb.AppendLine($"{indentStr}  description: \"{EscapeYaml(schema.Description)}\"");

        // Indices
        if (schema.Indices.Any())
        {
            sb.AppendLine($"{indentStr}  indices:");
            foreach (var index in schema.Indices)
            {
                sb.AppendLine($"{indentStr}    \"{EscapeYaml(index.Name)}\":");
                if (index.Options.Any())
                {
                    sb.AppendLine($"{indentStr}      options:");
                    foreach (var option in index.Options)
                    {
                        sb.AppendLine($"{indentStr}        {option.Key}: {EscapeYaml(option.Value)}");
                    }
                }
                if (index.Fields.Any())
                {
                    sb.AppendLine($"{indentStr}      fields:");
                    foreach (var field in index.Fields)
                    {
                        sb.AppendLine($"{indentStr}        - field: {field.FieldName}");
                        sb.AppendLine($"{indentStr}          direction: {field.Direction}");
                    }
                }
            }
        }
        else
        {
            sb.AppendLine($"{indentStr}  indices: {{}}");
        }

        // Fields
        sb.AppendLine($"{indentStr}  fields:");
        foreach (var (fieldName, fieldSchema) in schema.Fields)
        {
            GenerateField(sb, fieldName, fieldSchema, indent + 2);
        }
    }

    private void GenerateField(StringBuilder sb, string fieldName, FieldSchema field, int indent)
    {
        var indentStr = new string(' ', indent * 2);

        sb.AppendLine($"{indentStr}{ToCamelCase(fieldName)}:");
        sb.AppendLine($"{indentStr}  type: {field.Type}");

        // 如果有 IdType，輸出 id_type
        if (field.IdType != null)
        {
            sb.AppendLine($"{indentStr}  id_type: {field.IdType}");
        }

        // 如果有 EnumType，輸出 enum_type
        if (field.EnumType != null)
        {
            sb.AppendLine($"{indentStr}  enum_type: {field.EnumType}");
        }

        sb.AppendLine($"{indentStr}  description: \"{EscapeYaml(field.Description)}\"");

        if (field.Nullable)
        {
            sb.AppendLine($"{indentStr}  nullable: true");
        }

        // 如果有嵌套欄位
        if (field.Fields != null && field.Fields.Any())
        {
            sb.AppendLine($"{indentStr}  fields:");
            foreach (var (nestedFieldName, nestedField) in field.Fields)
            {
                GenerateField(sb, nestedFieldName, nestedField, indent + 2);
            }
        }
    }

    /// <summary>
    /// 轉義 YAML 字串中的特殊字元
    /// </summary>
    private string EscapeYaml(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Replace("\"", "\\\"");
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
