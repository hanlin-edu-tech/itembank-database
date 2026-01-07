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

        // 分析所有 public properties
        foreach (var property in collectionType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var fieldSchema = AnalyzeField(property);
            var fieldName = GetBsonFieldName(collectionType, property);
            fields[fieldName] = fieldSchema;
        }

        // 提取索引資訊
        var indices = ExtractIndices(collectionType);

        return new CollectionSchema
        {
            CollectionName = collectionNameAttr.Name,
            Description = descriptionAttr?.Description ?? "",
            ClrTypeName = collectionType.Name,
            IsAuditable = typeof(IAuditable).IsAssignableFrom(collectionType),
            IsFinalizable = typeof(IFinalizable).IsAssignableFrom(collectionType),
            Indices = indices,
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

    /// <summary>
    /// 從 IIndexable<T> 提取索引資訊
    /// </summary>
    private static List<IndexSchema> ExtractIndices(Type collectionType)
    {
        var result = new List<IndexSchema>();

        // 先檢查此類型是否實作 IIndexable<>（檢查所有介面）
        var indexableInterface = collectionType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIndexable<>));

        if (indexableInterface == null)
        {
            return result;
        }

        try
        {
            // 取得 CreateIndexModels 靜態屬性
            var createIndexModelsProperty = indexableInterface.GetProperty(
                "CreateIndexModels",
                BindingFlags.Public | BindingFlags.Static);

            if (createIndexModelsProperty == null)
            {
                return result;
            }

            // 取得索引模型列表
            var indexModels = createIndexModelsProperty.GetValue(null);
            if (indexModels is not System.Collections.IEnumerable enumerable)
            {
                return result;
            }

            // 遍歷每個 CreateIndexModel<T>
            var indexCounter = 0;
            foreach (var indexModel in enumerable)
            {
                if (indexModel == null) continue;

                var indexModelType = indexModel.GetType();

                // 取得 Options 屬性以獲得索引名稱
                var optionsProperty = indexModelType.GetProperty("Options");
                var options = optionsProperty?.GetValue(indexModel);
                var nameProperty = options?.GetType().GetProperty("Name");
                var indexName = nameProperty?.GetValue(options) as string;

                // 如果沒有明確的索引名稱，嘗試從 Keys 推斷
                if (string.IsNullOrWhiteSpace(indexName))
                {
                    // 取得 Keys 屬性
                    var keysProperty = indexModelType.GetProperty("Keys");
                    var keys = keysProperty?.GetValue(indexModel);

                    // 嘗試從 Keys 生成預設名稱（MongoDB 的命名慣例）
                    indexName = GenerateDefaultIndexName(keys, collectionType);

                    // 如果還是無法生成，使用臨時 ID
                    if (string.IsNullOrWhiteSpace(indexName))
                    {
                        indexName = $"(未命名索引 #{indexCounter})";
                    }
                }

                indexCounter++;

                // 從索引名稱解析欄位和方向
                var indexFields = ParseIndexName(indexName);

                result.Add(new IndexSchema
                {
                    Name = indexName,
                    Fields = indexFields
                });
            }
        }
        catch
        {
            // 如果解析失敗，返回空列表
        }

        return result;
    }

    /// <summary>
    /// 從 IndexKeysDefinition 生成預設索引名稱（模擬 MongoDB 的命名邏輯）
    /// </summary>
    private static string? GenerateDefaultIndexName(object? keys, Type documentType)
    {
        if (keys == null)
        {
            return null;
        }

        try
        {
            // 嘗試使用 Render 方法取得 BsonDocument
            var renderMethod = keys.GetType().GetMethod("Render",
                BindingFlags.Public | BindingFlags.Instance);

            if (renderMethod != null)
            {
                // 需要 IBsonSerializer 和 IBsonSerializerRegistry 參數
                var parameters = renderMethod.GetParameters();
                if (parameters.Length == 2)
                {
                    // 嘗試取得 BsonSerializer
                    var serializerType = typeof(MongoDB.Bson.Serialization.BsonSerializer);
                    var lookupMethod = serializerType.GetMethod("LookupSerializer",
                        BindingFlags.Public | BindingFlags.Static,
                        null,
                        new[] { typeof(Type) },
                        null);

                    if (lookupMethod != null)
                    {
                        var genericMethod = lookupMethod.MakeGenericMethod(documentType);
                        var serializer = genericMethod.Invoke(null, new object[] { });

                        var registryProperty = serializerType.GetProperty("SerializerRegistry",
                            BindingFlags.Public | BindingFlags.Static);
                        var registry = registryProperty?.GetValue(null);

                        if (serializer != null && registry != null)
                        {
                            var bsonDocument = renderMethod.Invoke(keys, new[] { serializer, registry });

                            if (bsonDocument != null)
                            {
                                // 從 BsonDocument 建構索引名稱
                                var elements = new List<string>();
                                var documentType2 = bsonDocument.GetType();
                                var elementsProperty = documentType2.GetProperty("Elements");

                                if (elementsProperty != null)
                                {
                                    var elementsValue = elementsProperty.GetValue(bsonDocument) as System.Collections.IEnumerable;
                                    if (elementsValue != null)
                                    {
                                        foreach (var element in elementsValue)
                                        {
                                            var nameProperty = element.GetType().GetProperty("Name");
                                            var valueProperty = element.GetType().GetProperty("Value");

                                            var name = nameProperty?.GetValue(element) as string;
                                            var value = valueProperty?.GetValue(element);

                                            if (name != null && value != null)
                                            {
                                                // 取得方向（1 或 -1）
                                                var directionMethod = value.GetType().GetMethod("ToInt32");
                                                if (directionMethod != null)
                                                {
                                                    var direction = directionMethod.Invoke(value, null);
                                                    elements.Add($"{name}_{direction}");
                                                }
                                            }
                                        }
                                    }
                                }

                                if (elements.Any())
                                {
                                    return string.Join("_", elements);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // 忽略錯誤，返回 null
        }

        return null;
    }

    /// <summary>
    /// 解析索引名稱取得欄位和方向
    /// MongoDB 索引命名慣例：fieldName_1（ascending）或 fieldName_-1（descending）
    /// </summary>
    private static List<IndexField> ParseIndexName(string indexName)
    {
        var result = new List<IndexField>();

        try
        {
            // 移除常見的後綴（如 _1、_-1）
            // 分割索引名稱（例如 "field1_1_field2_-1" 或 "fieldName_1"）
            var parts = indexName.Split('_');

            for (int i = 0; i < parts.Length; i++)
            {
                // 檢查當前部分是否為方向標記（1 或 -1）
                if (parts[i] == "1" || parts[i] == "-1")
                {
                    // 前一個部分應該是欄位名稱
                    if (i > 0)
                    {
                        var fieldName = parts[i - 1];
                        var direction = parts[i] == "1" ? "ascending" : "descending";

                        // 避免重複加入（如果已經加入過）
                        if (!result.Any(f => f.FieldName == fieldName))
                        {
                            result.Add(new IndexField
                            {
                                FieldName = fieldName,
                                Direction = direction
                            });
                        }
                    }
                }
            }

            // 如果無法解析出任何欄位，嘗試簡單模式（整個名稱就是欄位名，預設 ascending）
            if (result.Count == 0 && !string.IsNullOrWhiteSpace(indexName) && indexName != "_id_")
            {
                // 移除尾部的 _1 或 _-1
                var cleanName = indexName;
                if (indexName.EndsWith("_1"))
                {
                    cleanName = indexName[..^2];
                    result.Add(new IndexField
                    {
                        FieldName = cleanName,
                        Direction = "ascending"
                    });
                }
                else if (indexName.EndsWith("_-1"))
                {
                    cleanName = indexName[..^3];
                    result.Add(new IndexField
                    {
                        FieldName = cleanName,
                        Direction = "descending"
                    });
                }
            }
        }
        catch
        {
            // 解析失敗，返回空列表
        }

        return result;
    }
}
