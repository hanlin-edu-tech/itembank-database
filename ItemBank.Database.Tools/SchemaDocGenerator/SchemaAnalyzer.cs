using System.ComponentModel;
using System.Reflection;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Tools.SchemaDocGenerator.Models;
using ItemBank.Database.Tools.SchemaDocGenerator.TypeMappers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Tools.SchemaDocGenerator;

/// <summary>
/// Schema 分析器 - 掃描並分析所有集合類別
/// </summary>
public sealed class SchemaAnalyzer
{
    private readonly MongoTypeMapper _typeMapper = new();

    private readonly Dictionary<string, Dictionary<string, string>> _globalEnums = new();

    /// <summary>
    /// 分析所有集合定義
    /// </summary>
    /// <returns>Schema 文件（包含全局 Enum 和集合清單）</returns>
    public SchemaDocument AnalyzeCollections()
    {
        // 載入 ItemBank.Database.Core 組件
        var coreAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "ItemBank.Database.Core");

        if (coreAssembly == null)
            throw new InvalidOperationException("找不到 ItemBank.Database.Core 組件");

        // 掃描所有帶有 [CollectionName] 的類別
        var collectionTypes = coreAssembly.GetTypes()
            .Where(t => t.GetCustomAttribute<CollectionNameAttribute>() != null)
            .OrderBy(t => t.Name)
            .ToList();

        var schemas = new List<CollectionSchema>();
        _globalEnums.Clear();

        foreach (var collectionType in collectionTypes)
        {
            var schema = AnalyzeCollection(collectionType);
            schemas.Add(schema);
        }

        return new SchemaDocument
        {
            Enums = _globalEnums.ToDictionary(
                kvp => kvp.Key, IReadOnlyDictionary<string, string> (kvp) => kvp.Value
            ),
            Collections = schemas
        };
    }

    /// <summary>
    /// 分析單一集合類別
    /// </summary>
    private CollectionSchema AnalyzeCollection(Type collectionType)
    {
        var collectionNameAttr = collectionType.GetCustomAttribute<CollectionNameAttribute>()!;
        var descriptionAttr = collectionType.GetCustomAttribute<DescriptionAttribute>();

        var fields = new Dictionary<string, FieldSchema>();
        var primaryKeys = new List<string>();

        // 分析所有 public properties
        foreach (var property in collectionType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var fieldSchema = AnalyzeField(property);
            var fieldName = GetBsonFieldName(collectionType, property);
            fields[fieldName] = fieldSchema;

            // 識別主鍵
            if (property.GetCustomAttribute<BsonIdAttribute>() != null)
            {
                primaryKeys.Add(fieldName);
            }
        }

        return new CollectionSchema
        {
            CollectionName = collectionNameAttr.Name,
            Description = descriptionAttr?.Description ?? "",
            ClrTypeName = collectionType.Name,
            IsAuditable = typeof(IAuditable).IsAssignableFrom(collectionType),
            IsFinalizable = typeof(IFinalizable).IsAssignableFrom(collectionType),
            PrimaryKeys = primaryKeys,
            BusinessKeys = [], // 暫時留空
            Fields = fields
        };
    }

    /// <summary>
    /// 分析欄位
    /// </summary>
    private FieldSchema AnalyzeField(PropertyInfo property)
    {
        var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
        var type = _typeMapper.MapType(property.PropertyType, out var nestedFields, out var enumValues, out var idType, out var enumType);

        // 如果有 enum 值，加入全局 enum 字典
        if (enumValues != null && enumType != null)
        {
            _globalEnums.TryAdd(enumType, enumValues);
        }

        // 遞迴處理嵌套欄位中的 enum
        if (nestedFields != null)
        {
            CollectEnumsFromFields(nestedFields);
        }

        return new FieldSchema
        {
            Type = type,
            Description = description,
            IdType = idType,
            EnumType = enumType,
            Fields = nestedFields,
            EnumValues = null // 不再在欄位中儲存 enum 值
        };
    }

    /// <summary>
    /// 從欄位字典中遞迴收集所有 enum 定義
    /// </summary>
    private void CollectEnumsFromFields(Dictionary<string, FieldSchema> fields)
    {
        foreach (var field in fields.Values)
        {
            // 檢查此欄位是否為 enum
            if (field is { EnumValues: not null, EnumType: not null })
            {
                if (!_globalEnums.ContainsKey(field.EnumType))
                {
                    _globalEnums[field.EnumType] = new Dictionary<string, string>(field.EnumValues);
                }
            }

            // 遞迴處理嵌套欄位
            if (field.Fields != null)
            {
                CollectEnumsFromFields((Dictionary<string, FieldSchema>)field.Fields);
            }
        }
    }

    /// <summary>
    /// 取得欄位在 MongoDB 中的真實名稱（考慮 BsonElement 屬性和序列化規則）
    /// </summary>
    private static string GetBsonFieldName(Type declaringType, PropertyInfo property)
    {
        try
        {
            var classMap = BsonClassMap.LookupClassMap(declaringType);
            var memberMap = classMap.GetMemberMap(property.Name);
            if (memberMap != null)
            {
                return memberMap.ElementName;
            }
        }
        catch
        {
            // 如果無法取得 ClassMap，使用 fallback
        }

        // Fallback: 使用 camelCase 轉換
        return ToCamelCase(property.Name);
    }

    /// <summary>
    /// 將字串轉換為 camelCase
    /// </summary>
    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0])) return str;
        return $"{char.ToLowerInvariant(str[0])}{str[1..]}";
    }
}
