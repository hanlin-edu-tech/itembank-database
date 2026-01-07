using System.ComponentModel;
using System.Reflection;
using ItemBank.Database.Core.Configuration.BsonSerializers;
using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;
using ItemBank.Database.Tools.SchemaDocGenerator.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Tools.SchemaDocGenerator.TypeMappers;

/// <summary>
/// C# 型別 → MongoDB 型別對應器
/// </summary>
public sealed class MongoTypeMapper
{
    private readonly HashSet<Type> _visitedTypes = new();
    private const int MaxDepth = 5;

    /// <summary>
    /// 將 C# 型別對應到 MongoDB 型別字串
    /// </summary>
    /// <param name="type">C# 型別</param>
    /// <param name="nestedFields">嵌套欄位定義（如果是 object 或 array&lt;object&gt;）</param>
    /// <param name="enumValues">Enum 的所有可能值（name -> value 對應，如果是 Enum）</param>
    /// <param name="idType">ValueObject 型別名稱（如果是 ValueObject）</param>
    /// <param name="enumType">Enum 型別名稱（如果是 Enum）</param>
    /// <param name="depth">當前遞迴深度</param>
    /// <returns>MongoDB 型別字串</returns>
    public string MapType(Type type, out Dictionary<string, FieldSchema>? nestedFields, out Dictionary<string, string>? enumValues, out string? idType, out string? enumType, int depth = 0)
    {
        nestedFields = null;
        enumValues = null;
        idType = null;
        enumType = null;

        // 防止過深遞迴
        if (depth > MaxDepth)
            return "object";

        // 處理 Nullable<T>
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        // 基本型別
        if (underlyingType == typeof(string)) return "string";
        if (underlyingType == typeof(int) || underlyingType == typeof(long) ||
            underlyingType == typeof(decimal) || underlyingType == typeof(double) ||
            underlyingType == typeof(float)) return "number";
        if (underlyingType == typeof(bool)) return "boolean";
        if (underlyingType == typeof(DateTime) || underlyingType == typeof(DateTimeOffset)) return "datetime";

        // MongoDB ObjectId
        if (underlyingType == typeof(ObjectId)) return "objectId";

        // 值物件 (IConvertibleId<TSelf, TValue>)
        if (IsValueObject(underlyingType, out var baseType))
        {
            idType = underlyingType.Name;
            return baseType; // "string" 或 "objectId"
        }

        // Enum
        if (underlyingType.IsEnum)
        {
            enumValues = GetEnumValues(underlyingType);
            enumType = underlyingType.Name;
            return GetEnumBaseType(underlyingType); // "string" 或 "number"
        }

        // Array/List
        if (underlyingType.IsArray || IsGenericList(underlyingType))
        {
            var elementType = underlyingType.IsArray
                ? underlyingType.GetElementType()!
                : underlyingType.GetGenericArguments()[0];

            var innerType = MapType(elementType, out var innerNestedFields, out var innerEnumValues, out var innerIdType, out var innerEnumType, depth + 1);

            // 如果元素是 object，則需要傳遞嵌套欄位
            if (innerNestedFields != null)
                nestedFields = innerNestedFields;

            // 如果元素是 enum，則需要傳遞 enum 資訊
            if (innerEnumValues != null)
            {
                enumValues = innerEnumValues;
                enumType = innerEnumType;
            }

            // 如果元素是 id type，則需要傳遞 id 資訊
            if (innerIdType != null)
                idType = innerIdType;

            return $"array<{innerType}>";
        }

        // Dictionary
        if (IsGenericDictionary(underlyingType))
            return "object";

        // 嵌入物件（class）
        if (underlyingType.IsClass && underlyingType != typeof(object))
        {
            nestedFields = AnalyzeNestedFields(underlyingType, depth);
            return "object";
        }

        return "unknown";
    }

    /// <summary>
    /// 判斷是否為值物件（實作 IConvertibleId&lt;TSelf, TValue&gt;）
    /// </summary>
    /// <param name="type">要檢查的型別</param>
    /// <param name="baseType">底層型別（"string" 或 "objectId"）</param>
    /// <returns>是否為值物件</returns>
    private static bool IsValueObject(Type type, out string baseType)
    {
        baseType = string.Empty;

        var convertibleIdInterface = type.GetInterfaces().FirstOrDefault(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() == typeof(IConvertibleId<,>));

        if (convertibleIdInterface == null)
            return false;

        // 取得底層型別 (string 或 ObjectId)
        var underlyingType = convertibleIdInterface.GetGenericArguments()[1];

        if (underlyingType == typeof(string))
            baseType = "string";
        else if (underlyingType == typeof(ObjectId))
            baseType = "objectId";
        else
            return false;

        return true;
    }

    /// <summary>
    /// 取得 Enum 的基礎型別（根據序列化類型）
    /// </summary>
    private static string GetEnumBaseType(Type enumType)
    {
        try
        {
            var serializer = BsonSerializer.LookupSerializer(enumType);

            if (serializer is IEnumSerializerMetadata metadata)
            {
                return metadata.SerializationType switch
                {
                    EnumSerializationType.Integer => "number",
                    _ => "string"
                };
            }
        }
        catch
        {
            // 如果無法判斷，預設為 string
        }

        return "string";
    }

    /// <summary>
    /// 取得 Enum 的所有可能值（name -> value 對應）
    /// </summary>
    private Dictionary<string, string> GetEnumValues(Type enumType)
    {
        try
        {
            var serializer = BsonSerializer.LookupSerializer(enumType);

            if (serializer is IEnumSerializerMetadata metadata)
            {
                return new Dictionary<string, string>(metadata.SerializedValues);
            }
        }
        catch
        {
            // 如果無法取得序列化器，使用 fallback
        }

        // Fallback: 使用 enum 名稱
        var result = new Dictionary<string, string>();
        var enumValues = Enum.GetValues(enumType);

        foreach (var enumValue in enumValues)
        {
            var name = Enum.GetName(enumType, enumValue) ?? "";
            result[name] = name;
        }

        return result;
    }

    /// <summary>
    /// 分析嵌入物件的欄位
    /// </summary>
    private Dictionary<string, FieldSchema> AnalyzeNestedFields(Type type, int depth)
    {
        // 防止循環參照
        if (!_visitedTypes.Add(type))
            return new Dictionary<string, FieldSchema>();

        try
        {
            var fields = new Dictionary<string, FieldSchema>();

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
                var fieldType = MapType(property.PropertyType, out var nestedFields, out var enumValues, out var idType, out var enumType, depth + 1);
                var fieldName = GetBsonFieldName(type, property);

                fields[fieldName] = new FieldSchema
                {
                    Type = fieldType,
                    Description = description,
                    IdType = idType,
                    EnumType = enumType,
                    Fields = nestedFields,
                    EnumValues = enumValues,
                    Nullable = false
                };
            }

            return fields;
        }
        finally
        {
            _visitedTypes.Remove(type);
        }
    }

    /// <summary>
    /// 判斷是否為泛型 List
    /// </summary>
    private bool IsGenericList(Type type)
    {
        return type.IsGenericType &&
               (type.GetGenericTypeDefinition() == typeof(List<>) ||
                type.GetGenericTypeDefinition() == typeof(IReadOnlyList<>) ||
                type.GetGenericTypeDefinition() == typeof(IList<>) ||
                type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                type.GetGenericTypeDefinition() == typeof(ICollection<>));
    }

    /// <summary>
    /// 判斷是否為泛型 Dictionary
    /// </summary>
    private bool IsGenericDictionary(Type type)
    {
        return type.IsGenericType &&
               (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
                type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));
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
