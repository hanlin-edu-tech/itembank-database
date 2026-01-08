using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace ItemBank.Database.Generators;

[Generator]
public sealed class SchemaRegistryGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider,
            static (spc, compilation) => Execute(compilation, spc));
    }

    private static void Execute(Compilation compilation, SourceProductionContext context)
    {
        if (!string.Equals(compilation.AssemblyName, "ItemBank.Database.Tools", StringComparison.Ordinal))
            return;

        var collectionNameAttributeSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.Attributes.CollectionNameAttribute");
        var descriptionAttributeSymbol = compilation.GetTypeByMetadataName(
            "System.ComponentModel.DescriptionAttribute");
        var obsoleteAttributeSymbol = compilation.GetTypeByMetadataName(
            "System.ObsoleteAttribute");
        var bsonIdAttributeSymbol = compilation.GetTypeByMetadataName(
            "MongoDB.Bson.Serialization.Attributes.BsonIdAttribute");
        var iAuditableSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.Interfaces.IAuditable");
        var iFinalizableSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.Interfaces.IFinalizable");
        var iIndexableSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.Interfaces.IIndexable`1");
        var convertibleIdSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.ValueObjects.Abstractions.IConvertibleId`2");
        var objectIdSymbol = compilation.GetTypeByMetadataName("MongoDB.Bson.ObjectId");
        var dateTimeOffsetSymbol = compilation.GetTypeByMetadataName("System.DateTimeOffset");
        var nullableSymbol = compilation.GetTypeByMetadataName("System.Nullable`1");

        var listSymbols = new[]
        {
            compilation.GetTypeByMetadataName("System.Collections.Generic.List`1"),
            compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyList`1"),
            compilation.GetTypeByMetadataName("System.Collections.Generic.IList`1"),
            compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1"),
            compilation.GetTypeByMetadataName("System.Collections.Generic.ICollection`1")
        }.Where(symbol => symbol is not null).Cast<INamedTypeSymbol>().ToImmutableArray();

        var dictionarySymbols = new[]
        {
            compilation.GetTypeByMetadataName("System.Collections.Generic.Dictionary`2"),
            compilation.GetTypeByMetadataName("System.Collections.Generic.IDictionary`2"),
            compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyDictionary`2")
        }.Where(symbol => symbol is not null).Cast<INamedTypeSymbol>().ToImmutableArray();

        if (collectionNameAttributeSymbol is null ||
            descriptionAttributeSymbol is null ||
            obsoleteAttributeSymbol is null ||
            bsonIdAttributeSymbol is null ||
            iAuditableSymbol is null ||
            iFinalizableSymbol is null ||
            iIndexableSymbol is null ||
            convertibleIdSymbol is null ||
            nullableSymbol is null)
        {
            return;
        }

        var coreAssembly = compilation.References
            .Select(compilation.GetAssemblyOrModuleSymbol)
            .OfType<IAssemblySymbol>()
            .FirstOrDefault(assembly =>
                string.Equals(assembly.Name, "ItemBank.Database.Core", StringComparison.Ordinal));

        if (coreAssembly is null)
            return;

        var allTypes = GetAllTypes(coreAssembly.GlobalNamespace).ToList();

        var collectionTypes = allTypes
            .Where(type => HasAttribute(type, collectionNameAttributeSymbol))
            .OrderBy(type => type.Name, StringComparer.Ordinal)
            .ToList();

        if (collectionTypes.Count == 0)
            return;

        var processedTypes = new Dictionary<INamedTypeSymbol, TypeDescriptor>(SymbolEqualityComparer.Default);
        var fieldsByOwner = new Dictionary<INamedTypeSymbol, List<TypeDescriptor>>(SymbolEqualityComparer.Default);

        foreach (var collectionType in collectionTypes)
        {
            ProcessType(collectionType, collectionType, processedTypes, fieldsByOwner, descriptionAttributeSymbol,
                obsoleteAttributeSymbol, bsonIdAttributeSymbol, listSymbols, dictionarySymbols, nullableSymbol,
                objectIdSymbol, dateTimeOffsetSymbol, convertibleIdSymbol);
        }

        var collectionMethodMap = collectionTypes.ToDictionary(
            type => type,
            type => GenerateCollectionMethod(type, processedTypes, collectionNameAttributeSymbol,
                descriptionAttributeSymbol, iAuditableSymbol, iFinalizableSymbol, iIndexableSymbol),
            SymbolEqualityComparer.Default);

        var collectionsList = string.Join("\n", collectionTypes.Select(type =>
        {
            var methodName = "BuildCollection_" +
                             SanitizeIdentifier(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            return $$"""            {{methodName}}(),""";
        }));

        var mainSource = $$"""
                           // <auto-generated/>
                           #nullable enable
                           namespace ItemBank.Database.Tools.SchemaDocGenerator
                           {
                               internal static partial class SchemaRegistry
                               {
                                   public static global::ItemBank.Database.Tools.SchemaDocGenerator.Models.SchemaDocument Build()
                                   {
                                       var collections = new global::System.Collections.Generic.List<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.CollectionSchema>
                                       {
                           {{collectionsList}}
                                       };

                                       var enums = CollectEnums(collections);
                                       return new global::ItemBank.Database.Tools.SchemaDocGenerator.Models.SchemaDocument
                                       {
                                           Enums = enums,
                                           Collections = collections
                                       };
                                   }

                                   private static global::System.Collections.Generic.IReadOnlyDictionary<string, global::System.Collections.Generic.IReadOnlyDictionary<string, string>> CollectEnums(
                                       global::System.Collections.Generic.IReadOnlyList<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.CollectionSchema> collections)
                                   {
                                       var result = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.IReadOnlyDictionary<string, string>>(global::System.StringComparer.Ordinal);
                                       foreach (var collection in collections)
                                       {
                                           CollectEnumsFromFields(collection.Fields, result);
                                       }

                                       return result;
                                   }

                                   private static void CollectEnumsFromFields(
                                       global::System.Collections.Generic.IReadOnlyDictionary<string, global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema> fields,
                                       global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.IReadOnlyDictionary<string, string>> enums)
                                   {
                                       foreach (var field in fields.Values)
                                       {
                                           if (field is { EnumValues: not null, EnumType: not null } && !enums.ContainsKey(field.EnumType))
                                           {
                                               enums[field.EnumType] = field.EnumValues;
                                           }

                                           if (field.Fields != null)
                                           {
                                               CollectEnumsFromFields(field.Fields, enums);
                                           }
                                       }
                                   }

                                   private static global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema CreateField(
                                       string type,
                                       string description,
                                       string? idType = null,
                                       string? enumType = null,
                                       bool nullable = false,
                                       global::System.Collections.Generic.IReadOnlyDictionary<string, string>? enumValues = null,
                                       global::System.Collections.Generic.IReadOnlyDictionary<string, global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema>? fields = null)
                                   {
                                       return new global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema
                                       {
                                           Type = type,
                                           Description = description,
                                           IdType = idType,
                                           EnumType = enumType,
                                           EnumValues = enumValues,
                                           Fields = fields,
                                           Nullable = nullable
                                       };
                                   }

                                   private static global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema CreateEnumField<TEnum>(string description, bool isArray, bool isNullable)
                                       where TEnum : struct, global::System.Enum
                                   {
                                       var info = GetEnumInfo<TEnum>();
                                       var type = isArray ? $"array<{info.BaseType}>" : info.BaseType;
                                       return CreateField(type, description, enumType: info.EnumName, nullable: isNullable, enumValues: info.Values);
                                   }

                                   private static EnumInfo GetEnumInfo<TEnum>()
                                       where TEnum : struct, global::System.Enum
                                   {
                                       var serializer = global::MongoDB.Bson.Serialization.BsonSerializer.LookupSerializer<TEnum>();
                                       if (serializer is global::ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions.IEnumSerializerMetadata metadata)
                                       {
                                           var baseType = metadata.SerializationType == global::ItemBank.Database.Core.Configuration.BsonSerializers.EnumSerializationType.Integer
                                               ? "number"
                                               : "string";
                                           return new EnumInfo(typeof(TEnum).Name, baseType, new global::System.Collections.Generic.Dictionary<string, string>(metadata.SerializedValues));
                                       }

                                       var values = new global::System.Collections.Generic.Dictionary<string, string>();
                                       foreach (var value in global::System.Enum.GetValues<TEnum>())
                                       {
                                           var name = value.ToString();
                                           values[name] = name;
                                       }

                                       return new EnumInfo(typeof(TEnum).Name, "string", values);
                                   }

                                   private static global::System.Collections.Generic.IReadOnlyList<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexSchema> BuildIndices<T>()
                                       where T : class, global::ItemBank.Database.Core.Schema.Interfaces.IIndexable<T>
                                   {
                                       var serializer = global::MongoDB.Bson.Serialization.BsonSerializer.LookupSerializer<T>();
                                       var registry = global::MongoDB.Bson.Serialization.BsonSerializer.SerializerRegistry;
                                       var indexModels = global::ItemBank.Database.Core.Schema.Interfaces.IIndexable<T>.CreateIndexModels;

                                       var result = new global::System.Collections.Generic.List<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexSchema>(indexModels.Count);
                                       foreach (var indexModel in indexModels)
                                       {
                                           var keysDoc = indexModel.Keys.Render(new global::MongoDB.Driver.RenderArgs<T>
                                           {
                                               SerializerRegistry = registry,
                                               DocumentSerializer = serializer
                                           });
                                           var fields = BuildIndexFields(keysDoc);
                                           var options = BuildIndexOptions(indexModel.Options, serializer, registry);
                                           var name = GetIndexName(indexModel.Options, keysDoc);

                                           result.Add(new global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexSchema
                                           {
                                               Name = name,
                                               Fields = fields,
                                               Options = options
                                           });
                                       }

                                       return result;
                                   }

                                   private static string GetIndexName<T>(global::MongoDB.Driver.CreateIndexOptions<T>? options, global::MongoDB.Bson.BsonDocument keysDoc)
                                       where T : class
                                   {
                                       if (!string.IsNullOrWhiteSpace(options?.Name))
                                           return options.Name!;

                                       if (keysDoc.ElementCount == 0)
                                           return "(未命名索引)";

                                       var parts = new global::System.Collections.Generic.List<string>(keysDoc.ElementCount);
                                       foreach (var element in keysDoc.Elements)
                                       {
                                           parts.Add($"{element.Name}_{element.Value}");
                                       }

                                       return string.Join("_", parts);
                                   }

                                   private static global::System.Collections.Generic.IReadOnlyList<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexField> BuildIndexFields(
                                       global::MongoDB.Bson.BsonDocument keysDoc)
                                   {
                                       var fields = new global::System.Collections.Generic.List<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexField>(keysDoc.ElementCount);

                                       foreach (var element in keysDoc.Elements)
                                       {
                                           var direction = element.Value switch
                                           {
                                               global::MongoDB.Bson.BsonInt32 { Value: 1 } => "ascending",
                                               global::MongoDB.Bson.BsonInt32 { Value: -1 } => "descending",
                                               global::MongoDB.Bson.BsonInt64 { Value: 1 } => "ascending",
                                               global::MongoDB.Bson.BsonInt64 { Value: -1 } => "descending",
                                               _ => element.Value.ToString()
                                           };

                                           fields.Add(new global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexField
                                           {
                                               FieldName = element.Name,
                                               Direction = direction
                                           });
                                       }

                                       return fields;
                                   }

                                   private static global::System.Collections.Generic.IReadOnlyDictionary<string, string> BuildIndexOptions<T>(
                                       global::MongoDB.Driver.CreateIndexOptions<T>? options,
                                       global::MongoDB.Bson.Serialization.IBsonSerializer<T> serializer,
                                       global::MongoDB.Bson.Serialization.IBsonSerializerRegistry registry)
                                       where T : class
                                   {
                                       var result = new global::System.Collections.Generic.Dictionary<string, string>();

                                       if (options is null)
                                           return result;

                                       var renderArgs = new global::MongoDB.Driver.RenderArgs<T>
                                       {
                                           SerializerRegistry = registry,
                                           DocumentSerializer = serializer
                                       };

                                       AddOption(result, "Background", options.Background);
                                       AddOption(result, "Bits", options.Bits);
                                       AddOption(result, "DefaultLanguage", options.DefaultLanguage);
                                       AddOption(result, "ExpireAfter", options.ExpireAfter);
                                       AddOption(result, "Hidden", options.Hidden);
                                       AddOption(result, "LanguageOverride", options.LanguageOverride);
                                       AddOption(result, "Max", options.Max);
                                       AddOption(result, "Min", options.Min);
                                       AddOption(result, "Sparse", options.Sparse);
                                       AddOption(result, "SphereIndexVersion", options.SphereIndexVersion);
                                       AddOption(result, "TextIndexVersion", options.TextIndexVersion);
                                       AddOption(result, "Unique", options.Unique);
                                       AddOption(result, "Version", options.Version);

                                       if (options.Collation != null)
                                           result["Collation"] = options.Collation.ToString();

                                       if (options.StorageEngine != null)
                                           result["StorageEngine"] = options.StorageEngine.ToString();

                                       if (options.Weights != null)
                                           result["Weights"] = options.Weights.ToString();

                                       if (options.PartialFilterExpression != null)
                                       {
                                           var filterDoc = options.PartialFilterExpression.Render(renderArgs);
                                           result["PartialFilterExpression"] = filterDoc.ToString();
                                       }

                                       if (options.WildcardProjection != null)
                                       {
                                           var projectionDoc = options.WildcardProjection.Render(renderArgs);
                                           result["WildcardProjection"] = projectionDoc.ToString();
                                       }

                                       return result;
                                   }

                                   private static void AddOption(global::System.Collections.Generic.Dictionary<string, string> options, string name, string? value)
                                   {
                                       if (!string.IsNullOrWhiteSpace(value))
                                           options[name] = value;
                                   }

                                   private static void AddOption(global::System.Collections.Generic.Dictionary<string, string> options, string name, bool? value)
                                   {
                                       if (value.HasValue)
                                           options[name] = value.Value.ToString();
                                   }

                                   private static void AddOption(global::System.Collections.Generic.Dictionary<string, string> options, string name, int? value)
                                   {
                                       if (value.HasValue)
                                           options[name] = value.Value.ToString();
                                   }

                                   private static void AddOption(global::System.Collections.Generic.Dictionary<string, string> options, string name, double? value)
                                   {
                                       if (value.HasValue)
                                           options[name] = value.Value.ToString(global::System.Globalization.CultureInfo.InvariantCulture);
                                   }

                                   private static void AddOption(global::System.Collections.Generic.Dictionary<string, string> options, string name, global::System.TimeSpan? value)
                                   {
                                       if (value.HasValue)
                                           options[name] = value.Value.ToString();
                                   }

                                   private sealed class EnumInfo
                                   {
                                       public EnumInfo(string enumName, string baseType, global::System.Collections.Generic.IReadOnlyDictionary<string, string> values)
                                       {
                                           EnumName = enumName;
                                           BaseType = baseType;
                                           Values = values;
                                       }

                                       public string EnumName { get; }

                                       public string BaseType { get; }

                                       public global::System.Collections.Generic.IReadOnlyDictionary<string, string> Values { get; }
                                   }
                               }
                           }
                           """;

        context.AddSource("SchemaRegistry.g.cs", SourceText.From(mainSource, Encoding.UTF8));

        var usedFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var collectionType in collectionTypes)
        {
            var collectionName =
                GetCollectionName(collectionType, collectionNameAttributeSymbol) ?? collectionType.Name;
            var fileName = GetUniqueFileName(collectionName, usedFileNames);

            var fieldMethods = fieldsByOwner.TryGetValue(collectionType, out var descriptorList)
                ? descriptorList.OrderBy(descriptor => descriptor.MethodName, StringComparer.Ordinal)
                    .Select(GenerateFieldMethod).ToList()
                : new List<string>();

            var collectionMethod = collectionMethodMap[collectionType];
            var sectionContent = string.Join("\n\n", new[] { collectionMethod }.Concat(fieldMethods));

            var collectionSource = $$"""
                                     // <auto-generated/>
                                     #nullable enable
                                     namespace ItemBank.Database.Tools.SchemaDocGenerator
                                     {
                                         internal static partial class SchemaRegistry
                                         {
                                     {{sectionContent}}
                                         }
                                     }
                                     """;

            context.AddSource(fileName, SourceText.From(collectionSource, Encoding.UTF8));
        }
    }

    private static string GenerateCollectionMethod(
        INamedTypeSymbol typeSymbol,
        Dictionary<INamedTypeSymbol, TypeDescriptor> typeMap,
        INamedTypeSymbol collectionNameAttributeSymbol,
        INamedTypeSymbol descriptionAttributeSymbol,
        INamedTypeSymbol iAuditableSymbol,
        INamedTypeSymbol iFinalizableSymbol,
        INamedTypeSymbol iIndexableSymbol)
    {
        var methodName = "BuildCollection_" +
                         SanitizeIdentifier(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        var fieldsMethod = typeMap[typeSymbol].MethodName;
        var collectionName = GetCollectionName(typeSymbol, collectionNameAttributeSymbol) ?? typeSymbol.Name;
        var description = GetDescription(typeSymbol, descriptionAttributeSymbol);
        var clrTypeName = typeSymbol.Name;
        var isAuditable = ImplementsInterface(typeSymbol, iAuditableSymbol);
        var isFinalizable = ImplementsInterface(typeSymbol, iFinalizableSymbol);

        var typeDisplay = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var indicesExpression = ImplementsInterface(typeSymbol, iIndexableSymbol)
            ? $"BuildIndices<{typeDisplay}>()"
            : "global::System.Array.Empty<global::ItemBank.Database.Tools.SchemaDocGenerator.Models.IndexSchema>()";

        return $$"""
                         private static global::ItemBank.Database.Tools.SchemaDocGenerator.Models.CollectionSchema {{methodName}}()
                         {
                             return new global::ItemBank.Database.Tools.SchemaDocGenerator.Models.CollectionSchema
                             {
                                 CollectionName = "{{EscapeStringLiteral(collectionName)}}",
                                 Description = "{{EscapeStringLiteral(description)}}",
                                 ClrTypeName = "{{EscapeStringLiteral(clrTypeName)}}",
                                 IsAuditable = {{isAuditable.ToString().ToLowerInvariant()}},
                                 IsFinalizable = {{isFinalizable.ToString().ToLowerInvariant()}},
                                 Indices = {{indicesExpression}},
                                 Fields = {{fieldsMethod}}()
                             };
                         }
                 """.TrimEnd();
    }

    private static string GenerateFieldMethod(TypeDescriptor descriptor)
    {
        var fieldLines = string.Join("\n", descriptor.Fields.Select(field =>
        {
            var expression = RenderFieldExpression(field);
            return $$"""            ["{{EscapeStringLiteral(field.FieldName)}}"] = {{expression}},""";
        }));

        return $$"""
                         private static global::System.Collections.Generic.IReadOnlyDictionary<string, global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema> {{descriptor.MethodName}}()
                         {
                             return new global::System.Collections.Generic.Dictionary<string, global::ItemBank.Database.Tools.SchemaDocGenerator.Models.FieldSchema>
                             {
                 {{fieldLines}}
                             };
                         }
                 """.TrimEnd();
    }

    private static string RenderFieldExpression(FieldDescriptor field)
    {
        var description = EscapeStringLiteral(field.Description);

        var nullableLiteral = field.IsNullable.ToString().ToLowerInvariant();

        return field.Kind switch
        {
            FieldKind.Enum =>
                $$"""CreateEnumField<{{field.TypeDisplay}}>("{{description}}", isArray: {{field.IsArray.ToString().ToLowerInvariant()}}, isNullable: {{nullableLiteral}})""",
            FieldKind.Object =>
                $$"""CreateField("{{field.TypeString}}", "{{description}}", nullable: {{nullableLiteral}}, fields: {{field.NestedFieldsMethod}}())""",
            FieldKind.ValueObject => field.IdType is null
                ? $$"""CreateField("{{field.TypeString}}", "{{description}}", nullable: {{nullableLiteral}})"""
                : $$"""CreateField("{{field.TypeString}}", "{{description}}", idType: "{{EscapeStringLiteral(field.IdType)}}", nullable: {{nullableLiteral}})""",
            _ => $$"""CreateField("{{field.TypeString}}", "{{description}}", nullable: {{nullableLiteral}})"""
        };
    }

    private static void ProcessType(
        INamedTypeSymbol typeSymbol,
        INamedTypeSymbol ownerCollection,
        Dictionary<INamedTypeSymbol, TypeDescriptor> processedTypes,
        Dictionary<INamedTypeSymbol, List<TypeDescriptor>> fieldsByOwner,
        INamedTypeSymbol descriptionAttributeSymbol,
        INamedTypeSymbol obsoleteAttributeSymbol,
        INamedTypeSymbol bsonIdAttributeSymbol,
        ImmutableArray<INamedTypeSymbol> listSymbols,
        ImmutableArray<INamedTypeSymbol> dictionarySymbols,
        INamedTypeSymbol nullableSymbol,
        INamedTypeSymbol? objectIdSymbol,
        INamedTypeSymbol? dateTimeOffsetSymbol,
        INamedTypeSymbol convertibleIdSymbol)
    {
        if (processedTypes.ContainsKey(typeSymbol))
            return;

        var methodName = "BuildFields_" +
                         SanitizeIdentifier(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        var fields = new List<FieldDescriptor>();
        var descriptor = new TypeDescriptor(typeSymbol, methodName, fields);
        processedTypes[typeSymbol] = descriptor;

        if (!fieldsByOwner.TryGetValue(ownerCollection, out var ownerList))
        {
            ownerList = new List<TypeDescriptor>();
            fieldsByOwner[ownerCollection] = ownerList;
        }

        ownerList.Add(descriptor);

        foreach (var property in typeSymbol.GetMembers().OfType<IPropertySymbol>()
                     .Where(property => property is { IsStatic: false, DeclaredAccessibility: Accessibility.Public }))
        {
            var fieldName = GetFieldName(property, bsonIdAttributeSymbol);
            var description = GetDescription(property, descriptionAttributeSymbol);
            var (isObsolete, obsoleteMessage) = GetObsoleteInfo(property, obsoleteAttributeSymbol);
            if (isObsolete)
            {
                fieldName = $"{fieldName} (obsolete)";
            }

            if (!string.IsNullOrWhiteSpace(obsoleteMessage))
            {
                var obsoleteText = obsoleteMessage!;
                description = string.IsNullOrWhiteSpace(description)
                    ? obsoleteText
                    : $"{description} {obsoleteText}";
            }

            var isNullableReference = property.NullableAnnotation == NullableAnnotation.Annotated;
            var fieldDescriptor = BuildFieldDescriptor(property.Type, fieldName, description, isNullableReference,
                listSymbols, dictionarySymbols, nullableSymbol, objectIdSymbol, dateTimeOffsetSymbol, convertibleIdSymbol);

            fields.Add(fieldDescriptor);

            if (fieldDescriptor.Kind == FieldKind.Object && fieldDescriptor.NestedType != null)
            {
                ProcessType(fieldDescriptor.NestedType, ownerCollection, processedTypes, fieldsByOwner,
                    descriptionAttributeSymbol, obsoleteAttributeSymbol, bsonIdAttributeSymbol, listSymbols,
                    dictionarySymbols, nullableSymbol, objectIdSymbol, dateTimeOffsetSymbol, convertibleIdSymbol);
            }
        }

        // populated via shared list
    }

    private static FieldDescriptor BuildFieldDescriptor(
        ITypeSymbol typeSymbol,
        string fieldName,
        string description,
        bool isNullableReference,
        ImmutableArray<INamedTypeSymbol> listSymbols,
        ImmutableArray<INamedTypeSymbol> dictionarySymbols,
        INamedTypeSymbol nullableSymbol,
        INamedTypeSymbol? objectIdSymbol,
        INamedTypeSymbol? dateTimeOffsetSymbol,
        INamedTypeSymbol convertibleIdSymbol)
    {
        var (elementType, isArray) = UnwrapEnumerable(typeSymbol, listSymbols);
        var (baseType, isNullableValue) = UnwrapNullable(elementType, nullableSymbol);
        var isNullable = isNullableReference || isNullableValue;

        if (baseType is INamedTypeSymbol namedType)
        {
            if (TryGetConvertibleIdBaseType(namedType, convertibleIdSymbol, objectIdSymbol, out var idBaseType))
            {
                var typeString = isArray ? $"array<{idBaseType}>" : idBaseType;
                return new FieldDescriptor(fieldName, description, FieldKind.ValueObject, typeString, namedType.Name,
                    isArray, isNullable);
            }

            if (namedType.TypeKind == TypeKind.Enum)
            {
                var typeDisplay = namedType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                return new FieldDescriptor(fieldName, description, FieldKind.Enum, "enum", null, isArray, isNullable,
                    enumType: typeDisplay);
            }

            if (IsDictionaryType(namedType, dictionarySymbols))
            {
                var typeString = isArray ? "array<object>" : "object";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (namedType.SpecialType == SpecialType.System_String)
            {
                var typeString = isArray ? "array<string>" : "string";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (namedType.SpecialType == SpecialType.System_Boolean)
            {
                var typeString = isArray ? "array<boolean>" : "boolean";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (namedType.SpecialType is SpecialType.System_Int32 or SpecialType.System_Int64
                or SpecialType.System_Decimal
                or SpecialType.System_Double or SpecialType.System_Single)
            {
                var typeString = isArray ? "array<number>" : "number";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (namedType.SpecialType == SpecialType.System_DateTime ||
                (dateTimeOffsetSymbol != null &&
                 SymbolEqualityComparer.Default.Equals(namedType, dateTimeOffsetSymbol)))
            {
                var typeString = isArray ? "array<datetime>" : "datetime";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (objectIdSymbol != null && SymbolEqualityComparer.Default.Equals(namedType, objectIdSymbol))
            {
                var typeString = isArray ? "array<objectId>" : "objectId";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (namedType.SpecialType == SpecialType.System_Object)
            {
                var typeString = isArray ? "array<object>" : "object";
                return new FieldDescriptor(fieldName, description, FieldKind.Simple, typeString, null, isArray, isNullable);
            }

            if (namedType.TypeKind == TypeKind.Class)
            {
                var typeString = isArray ? "array<object>" : "object";
                var nestedType = string.Equals(namedType.ContainingAssembly?.Name, "ItemBank.Database.Core",
                    StringComparison.Ordinal)
                    ? namedType
                    : null;
                return new FieldDescriptor(fieldName, description,
                    nestedType != null ? FieldKind.Object : FieldKind.Simple, typeString, null, isArray, isNullable,
                    nestedType);
            }
        }

        var fallbackType = isArray ? "array<unknown>" : "unknown";
        return new FieldDescriptor(fieldName, description, FieldKind.Simple, fallbackType, null, isArray, isNullable);
    }

    private static (ITypeSymbol ElementType, bool IsArray) UnwrapEnumerable(ITypeSymbol typeSymbol,
        ImmutableArray<INamedTypeSymbol> listSymbols)
    {
        if (typeSymbol is IArrayTypeSymbol arrayType)
        {
            return (arrayType.ElementType, true);
        }

        if (typeSymbol is INamedTypeSymbol namedType && namedType.IsGenericType)
        {
            var definition = namedType.OriginalDefinition;
            if (listSymbols.Any(list => SymbolEqualityComparer.Default.Equals(definition, list)))
            {
                return (namedType.TypeArguments[0], true);
            }
        }

        return (typeSymbol, false);
    }

    private static (ITypeSymbol Type, bool IsNullable) UnwrapNullable(ITypeSymbol typeSymbol, INamedTypeSymbol nullableSymbol)
    {
        if (typeSymbol is INamedTypeSymbol namedType &&
            namedType.IsGenericType &&
            SymbolEqualityComparer.Default.Equals(namedType.OriginalDefinition, nullableSymbol))
        {
            return (namedType.TypeArguments[0], true);
        }

        return (typeSymbol, false);
    }

    private static bool TryGetConvertibleIdBaseType(
        INamedTypeSymbol typeSymbol,
        INamedTypeSymbol convertibleIdSymbol,
        INamedTypeSymbol? objectIdSymbol,
        out string baseType)
    {
        baseType = string.Empty;

        foreach (var iface in typeSymbol.AllInterfaces)
        {
            if (!SymbolEqualityComparer.Default.Equals(iface.OriginalDefinition, convertibleIdSymbol))
                continue;

            if (iface.TypeArguments.Length != 2)
                continue;

            var underlying = iface.TypeArguments[1];
            if (underlying.SpecialType == SpecialType.System_String)
            {
                baseType = "string";
                return true;
            }

            if (objectIdSymbol != null && SymbolEqualityComparer.Default.Equals(underlying, objectIdSymbol))
            {
                baseType = "objectId";
                return true;
            }
        }

        return false;
    }

    private static bool IsDictionaryType(INamedTypeSymbol typeSymbol,
        ImmutableArray<INamedTypeSymbol> dictionarySymbols)
    {
        if (!typeSymbol.IsGenericType)
            return false;

        var definition = typeSymbol.OriginalDefinition;
        return dictionarySymbols.Any(dict => SymbolEqualityComparer.Default.Equals(definition, dict));
    }

    private static IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceSymbol ns)
    {
        foreach (var member in ns.GetMembers())
        {
            if (member is INamespaceSymbol nestedNs)
            {
                foreach (var type in GetAllTypes(nestedNs))
                    yield return type;
            }
            else if (member is INamedTypeSymbol type)
            {
                yield return type;
                foreach (var nested in GetNestedTypes(type))
                    yield return nested;
            }
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetNestedTypes(INamedTypeSymbol type)
    {
        foreach (var nested in type.GetTypeMembers())
        {
            yield return nested;
            foreach (var deeper in GetNestedTypes(nested))
                yield return deeper;
        }
    }

    private static bool HasAttribute(ISymbol symbol, INamedTypeSymbol attributeSymbol)
    {
        return symbol.GetAttributes().Any(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, attributeSymbol));
    }

    private static string? GetCollectionName(INamedTypeSymbol typeSymbol, INamedTypeSymbol attributeSymbol)
    {
        foreach (var attribute in typeSymbol.GetAttributes())
        {
            if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol))
                continue;

            if (attribute.ConstructorArguments.Length > 0 &&
                attribute.ConstructorArguments[0].Value is string ctorName)
            {
                return ctorName;
            }

            foreach (var named in attribute.NamedArguments)
            {
                if (named.Key == "Name" && named.Value.Value is string namedName)
                    return namedName;
            }
        }

        return null;
    }

    private static string GetDescription(ISymbol symbol, INamedTypeSymbol attributeSymbol)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol))
                continue;

            if (attribute.ConstructorArguments.Length > 0 &&
                attribute.ConstructorArguments[0].Value is string ctorName)
            {
                return ctorName;
            }

            foreach (var named in attribute.NamedArguments)
            {
                if (named.Key == "Description" && named.Value.Value is string namedDescription)
                    return namedDescription;
            }
        }

        return string.Empty;
    }

    private static (bool IsObsolete, string? Message) GetObsoleteInfo(ISymbol symbol, INamedTypeSymbol attributeSymbol)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol))
                continue;

            string? message = null;

            if (attribute.ConstructorArguments.Length > 0 &&
                attribute.ConstructorArguments[0].Value is string ctorMessage)
            {
                message = ctorMessage;
            }

            foreach (var named in attribute.NamedArguments)
            {
                if (named.Key == "Message" && named.Value.Value is string namedMessage)
                {
                    message = namedMessage;
                    break;
                }
            }

            return (true, message);
        }

        return (false, null);
    }

    private static string GetFieldName(IPropertySymbol property, INamedTypeSymbol bsonIdAttributeSymbol)
    {
        if (HasAttribute(property, bsonIdAttributeSymbol))
            return "_id";

        return ToCamelCase(property.Name);
    }

    private static bool ImplementsInterface(INamedTypeSymbol typeSymbol, INamedTypeSymbol interfaceSymbol)
    {
        return typeSymbol.AllInterfaces.Any(i =>
            SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, interfaceSymbol) ||
            SymbolEqualityComparer.Default.Equals(i, interfaceSymbol));
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
            return value;

        if (value.Length == 1)
            return value.ToLowerInvariant();

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    private static string SanitizeIdentifier(string fullyQualifiedName)
    {
        var name = fullyQualifiedName.Replace("global::", string.Empty);
        var builder = new StringBuilder(name.Length);

        foreach (var ch in name)
        {
            builder.Append(char.IsLetterOrDigit(ch) ? ch : '_');
        }

        if (builder.Length == 0)
            return "_";

        if (!SyntaxFacts.IsIdentifierStartCharacter(builder[0]))
            builder.Insert(0, '_');

        return builder.ToString();
    }

    private static string EscapeStringLiteral(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"");
    }

    private static string GetUniqueFileName(string collectionName, HashSet<string> usedFileNames)
    {
        var sanitized = SanitizeFileName(collectionName);
        var baseName = $"SchemaRegistry.{sanitized}.g.cs";
        var fileName = baseName;
        var counter = 1;

        while (!usedFileNames.Add(fileName))
        {
            counter++;
            fileName = $"SchemaRegistry.{sanitized}_{counter}.g.cs";
        }

        return fileName;
    }

    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Unknown";

        var builder = new StringBuilder(name.Length);
        foreach (var ch in name)
        {
            builder.Append(char.IsLetterOrDigit(ch) ? ch : '_');
        }

        return builder.ToString();
    }

    private sealed class TypeDescriptor
    {
        public TypeDescriptor(INamedTypeSymbol typeSymbol, string methodName, IReadOnlyList<FieldDescriptor> fields)
        {
            TypeSymbol = typeSymbol;
            MethodName = methodName;
            Fields = fields;
        }

        public INamedTypeSymbol TypeSymbol { get; }

        public string MethodName { get; }

        public IReadOnlyList<FieldDescriptor> Fields { get; }
    }

    private sealed class FieldDescriptor
    {
        public FieldDescriptor(
            string fieldName,
            string description,
            FieldKind kind,
            string typeString,
            string? idType,
            bool isArray,
            bool isNullable,
            INamedTypeSymbol? nestedType = null,
            string? enumType = null)
        {
            FieldName = fieldName;
            Description = description;
            Kind = kind;
            TypeString = typeString;
            IdType = idType;
            IsArray = isArray;
            IsNullable = isNullable;
            NestedType = nestedType;
            TypeDisplay = enumType ?? typeString;
            NestedFieldsMethod = nestedType != null
                ? "BuildFields_" +
                  SanitizeIdentifier(nestedType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                : string.Empty;
        }

        public string FieldName { get; }

        public string Description { get; }

        public FieldKind Kind { get; }

        public string TypeString { get; }

        public string? IdType { get; }

        public bool IsArray { get; }

        public bool IsNullable { get; }

        public INamedTypeSymbol? NestedType { get; }

        public string TypeDisplay { get; }

        public string NestedFieldsMethod { get; }
    }

    private enum FieldKind
    {
        Simple,
        ValueObject,
        Enum,
        Object
    }
}
