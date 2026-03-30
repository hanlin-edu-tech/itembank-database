using System.Text.Json;
using ItemBank.Database.Tools.SchemaDocGenerator.Models;

namespace ItemBank.Database.Tools.SchemaDocGenerator.Generators;

/// <summary>
/// JSON Schema 文件生成器（供 AI 工具查詢使用）
/// </summary>
public sealed class JsonSchemaGenerator : ISchemaDocGenerator
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public string Generate(SchemaDocument document)
    {
        var output = new
        {
            enums = document.Enums,
            collections = document.Collections.ToDictionary(
                c => c.CollectionName,
                c => new
                {
                    description = c.Description,
                    kind = c.Kind,
                    discriminator = c.Discriminator,
                    variantField = c.VariantField,
                    notes = c.Notes,
                    variants = c.Variants.Select(v => new
                    {
                        name = v.Name,
                        typeName = v.TypeName,
                        description = v.Description,
                        discriminatorValue = v.DiscriminatorValue
                    }).ToList(),
                    typeName = c.TypeName,
                    isAuditable = c.IsAuditable,
                    isFinalizable = c.IsFinalizable,
                    indices = c.Indices.Select(i => new
                    {
                        name = i.Name,
                        options = i.Options,
                        fields = i.Fields.Select(f => new
                        {
                            fieldName = f.FieldName,
                            direction = f.Direction
                        }).ToList()
                    }).ToList(),
                    fields = ConvertFields(c.Fields)
                })
        };

        return JsonSerializer.Serialize(output, Options);
    }

    private Dictionary<string, object?> ConvertFields(IReadOnlyDictionary<string, FieldSchema> fields)
    {
        return fields.ToDictionary(
            kvp => ToCamelCase(kvp.Key),
            kvp => (object?)ConvertField(kvp.Value));
    }

    private Dictionary<string, object?> ConvertField(FieldSchema field)
    {
        var dict = new Dictionary<string, object?>
        {
            ["type"] = field.Type,
            ["description"] = field.Description
        };

        if (field.IdType != null) dict["idType"] = field.IdType;
        if (field.EnumType != null) dict["enumType"] = field.EnumType;
        if (field.Nullable) dict["nullable"] = true;
        if (field.Fields is { Count: > 0 })
            dict["fields"] = ConvertFields(field.Fields);

        return dict;
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length == 1)
            return value.ToLowerInvariant();

        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}
