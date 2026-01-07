using System.Text;
using ItemBank.Database.Tools.SchemaDocGenerator.Models;
using Spectre.Console;

namespace ItemBank.Database.Tools.SchemaDocGenerator.Generators;

/// <summary>
/// YAML schema console generator (colored output)
/// </summary>
public sealed class YamlSchemaConsoleGenerator
{
    private const string KeyColor = "bold white";
    private static readonly Dictionary<string, string> TypeColors = new(StringComparer.OrdinalIgnoreCase)
    {
        ["string"] = "green",
        ["number"] = "cyan",
        ["boolean"] = "magenta",
        ["datetime"] = "blue",
        ["objectId"] = "red",
        ["object"] = "grey",
        ["unknown"] = "grey"
    };

    public string GenerateMarkup(SchemaDocument document)
    {
        var sb = new StringBuilder();

        // Output global enum definitions first
        if (document.Enums.Any())
        {
            sb.AppendLine("enums:");
            foreach (var (enumName, enumValues) in document.Enums.OrderBy(kvp => kvp.Key))
            {
                sb.AppendLine($"  {FormatKey(enumName)}:");
                foreach (var (name, value) in enumValues)
                {
                    sb.AppendLine($"    {EscapeMarkup(name)}: {EscapeMarkup(value)}");
                }
            }
            sb.AppendLine();
        }

        // Output collection definitions
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

        sb.AppendLine($"{indentStr}{FormatKey(schema.CollectionName)}:");
        sb.AppendLine($"{indentStr}  description: \"{EscapeYamlMarkup(schema.Description)}\"");

        // Indices
        if (schema.Indices.Any())
        {
            sb.AppendLine($"{indentStr}  indices:");
            foreach (var index in schema.Indices)
            {
                sb.AppendLine($"{indentStr}    {FormatQuotedKey(index.Name)}:");
                if (index.Options.Any())
                {
                    sb.AppendLine($"{indentStr}      options:");
                    foreach (var option in index.Options)
                    {
                        sb.AppendLine($"{indentStr}        {EscapeMarkup(option.Key)}: {EscapeYamlMarkup(option.Value)}");
                    }
                }
                if (index.Fields.Any())
                {
                    sb.AppendLine($"{indentStr}      fields:");
                    foreach (var field in index.Fields)
                    {
                        sb.AppendLine($"{indentStr}        - field: {FormatKeyValue(field.FieldName)}");
                        sb.AppendLine($"{indentStr}          direction: {EscapeMarkup(field.Direction)}");
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
        var renderedName = FormatFieldKey(ToCamelCase(fieldName));

        sb.AppendLine($"{indentStr}{renderedName}:");
        sb.AppendLine($"{indentStr}  type: {FormatTypeValue(field.Type)}");

        // Emit id_type when present
        if (field.IdType != null)
        {
            sb.AppendLine($"{indentStr}  id_type: {EscapeMarkup(field.IdType)}");
        }

        // Emit enum_type when present
        if (field.EnumType != null)
        {
            sb.AppendLine($"{indentStr}  enum_type: {EscapeMarkup(field.EnumType)}");
        }

        sb.AppendLine($"{indentStr}  description: \"{EscapeYamlMarkup(field.Description)}\"");

        if (field.Nullable)
        {
            sb.AppendLine($"{indentStr}  nullable: true");
        }

        // Emit nested fields
        if (field.Fields != null && field.Fields.Any())
        {
            sb.AppendLine($"{indentStr}  fields:");
            foreach (var (nestedFieldName, nestedField) in field.Fields)
            {
                GenerateField(sb, nestedFieldName, nestedField, indent + 2);
            }
        }
    }

    private static string FormatTypeValue(string type)
    {
        if (string.IsNullOrEmpty(type))
            return string.Empty;

        if (type.StartsWith("array<", StringComparison.Ordinal) && type.EndsWith(">", StringComparison.Ordinal))
        {
            var innerType = type.Substring(6, type.Length - 7);
            return $"[dim]array<[/]{FormatTypeValue(innerType)}[dim]>[/]";
        }

        if (TypeColors.TryGetValue(type, out var color))
            return $"[{color}]{EscapeMarkup(type)}[/]";

        return EscapeMarkup(type);
    }

    private static string FormatFieldKey(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var marker = " (obsolte)";
        var index = value.IndexOf(marker, StringComparison.Ordinal);
        if (index < 0)
        {
            marker = " (obsolete)";
            index = value.IndexOf(marker, StringComparison.Ordinal);
        }

        if (index < 0)
            return FormatKey(value);

        var prefix = value[..index];
        var suffix = value[index..];
        return $"[{KeyColor}]{EscapeMarkup(prefix)}[/][yellow]{EscapeMarkup(suffix)}[/]";
    }

    private static string FormatKey(string value)
    {
        return $"[{KeyColor}]{EscapeMarkup(value)}[/]";
    }

    private static string FormatQuotedKey(string value)
    {
        return $"[{KeyColor}]\"{EscapeYamlMarkup(value)}\"[/]";
    }

    private static string FormatKeyValue(string value)
    {
        return $"[{KeyColor}]{EscapeMarkup(value)}[/]";
    }

    private static string EscapeYaml(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Replace("\"", "\\\"");
    }

    private static string EscapeMarkup(string value)
    {
        return Markup.Escape(value);
    }

    private static string EscapeYamlMarkup(string value)
    {
        return EscapeMarkup(EscapeYaml(value));
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (value.Length == 1)
            return value.ToLowerInvariant();

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}
