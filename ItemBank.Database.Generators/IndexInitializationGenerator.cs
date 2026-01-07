using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ItemBank.Database.Generators;

[Generator]
public sealed class IndexInitializationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidateClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax { BaseList: not null },
                static (ctx, _) => GetCandidateClassSymbol(ctx))
            .Where(static symbol => symbol is not null);

        var compilationAndCandidates = context.CompilationProvider.Combine(candidateClasses.Collect());

        context.RegisterSourceOutput(compilationAndCandidates,
            static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static INamedTypeSymbol? GetCandidateClassSymbol(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

        if (symbol is null)
            return null;

        if (symbol.TypeKind != TypeKind.Class || symbol.IsAbstract)
            return null;

        if (symbol.TypeParameters.Length > 0)
            return null;

        return symbol;
    }

    private static void Execute(Compilation compilation, ImmutableArray<INamedTypeSymbol?> candidates,
        SourceProductionContext context)
    {
        var indexableInterfaceSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.Interfaces.IIndexable`1");

        if (indexableInterfaceSymbol is null)
            return;

        var collectionNameAttributeSymbol = compilation.GetTypeByMetadataName(
            "ItemBank.Database.Core.Schema.Attributes.CollectionNameAttribute");

        var items = new List<IndexableInfo>();

        var seen = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        foreach (var candidate in candidates)
        {
            if (candidate is null)
                continue;

            if (!seen.Add(candidate))
                continue;

            var typeSymbol = candidate;

            var indexableInterface = typeSymbol.AllInterfaces.FirstOrDefault(i =>
                SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, indexableInterfaceSymbol));

            if (indexableInterface is null)
                continue;

            if (indexableInterface.TypeArguments.Length != 1)
                continue;

            if (indexableInterface.TypeArguments[0] is not INamedTypeSymbol documentType)
                continue;

            var documentTypeName = documentType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var collectionName = GetCollectionName(documentType, collectionNameAttributeSymbol) ?? documentType.Name;

            items.Add(new IndexableInfo(documentTypeName, collectionName));
        }

        if (items.Count == 0)
            return;

        items.Sort(static (left, right) =>
            string.CompareOrdinal(left.FullyQualifiedTypeName, right.FullyQualifiedTypeName));

        var source = GenerateSource(items);
        context.AddSource("IndexInitializationRegistry.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static string? GetCollectionName(INamedTypeSymbol documentType, INamedTypeSymbol? attributeSymbol)
    {
        if (attributeSymbol is null)
            return null;

        foreach (var attribute in documentType.GetAttributes())
        {
            if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol))
                continue;

            if (attribute.ConstructorArguments.Length > 0 &&
                attribute.ConstructorArguments[0].Value is string constructorName)
            {
                return constructorName;
            }

            foreach (var namedArgument in attribute.NamedArguments)
            {
                if (namedArgument is { Key: "Name", Value.Value: string namedName })
                    return namedName;
            }
        }

        return null;
    }

    private static string EscapeStringLiteral(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"");
    }

    private static string GenerateSource(List<IndexableInfo> items)
    {
        var initializerLines = string.Join("\n", items.Select(item =>
            $$"""            new IndexInitializer("{{EscapeStringLiteral(item.CollectionName)}}", static (db, ct) => Initialize<{{item.FullyQualifiedTypeName}}>(db, ct)),"""));

        var creationLines = string.Join("\n", items.Select(item =>
            $$"""            new IndexCreationEntry("{{EscapeStringLiteral(item.CollectionName)}}", static (db, ct) => CreateIndexes<{{item.FullyQualifiedTypeName}}>(db, "{{EscapeStringLiteral(item.CollectionName)}}", ct)),"""));

        var initMethodBlock = $$"""
            private static global::System.Threading.Tasks.Task Initialize<T>(global::ItemBank.Database.Core.Configuration.DbContext db, global::System.Threading.CancellationToken ct)
                where T : class, global::ItemBank.Database.Core.Schema.Interfaces.IIndexable<T>
            {
                var collection = db.GetCollection<T>();
                return collection.Indexes.CreateManyAsync(global::ItemBank.Database.Core.Schema.Interfaces.IIndexable<T>.CreateIndexModels, ct);
            }
            """.TrimStart('\n', '\r').TrimEnd();

        var creationMethodBlock = $$"""
            private static async global::System.Threading.Tasks.Task<int> CreateIndexes<T>(global::MongoDB.Driver.IMongoDatabase db, string collectionName, global::System.Threading.CancellationToken ct)
                where T : class, global::ItemBank.Database.Core.Schema.Interfaces.IIndexable<T>
            {
                var collection = db.GetCollection<T>(collectionName);
                var indexModels = global::ItemBank.Database.Core.Schema.Interfaces.IIndexable<T>.CreateIndexModels;
                await collection.Indexes.CreateManyAsync(indexModels, ct);
                return indexModels.Count;
            }
            """.TrimStart('\n', '\r').TrimEnd();

        return $$"""
                 // <auto-generated/>
                 #nullable enable
                 namespace ItemBank.Database.Core.Configuration
                 {
                     internal readonly record struct IndexInitializer(
                         string CollectionName,
                         global::System.Func<global::ItemBank.Database.Core.Configuration.DbContext, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> InitializeAsync);

                     public readonly record struct IndexCreationEntry(
                         string CollectionName,
                         global::System.Func<global::MongoDB.Driver.IMongoDatabase, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<int>> CreateAsync);

                     internal static class IndexInitializationRegistry
                     {
                         internal static readonly IndexInitializer[] All = new IndexInitializer[]
                         {
                 {{initializerLines}}
                         };

                 {{initMethodBlock}}
                     }

                     public static class IndexCreationRegistry
                     {
                         public static readonly IndexCreationEntry[] All = new IndexCreationEntry[]
                         {
                 {{creationLines}}
                         };

                 {{creationMethodBlock}}
                     }
                 }
                 """;
    }

    private sealed class IndexableInfo(string fullyQualifiedTypeName, string collectionName)
    {
        public string FullyQualifiedTypeName { get; } = fullyQualifiedTypeName;

        public string CollectionName { get; } = collectionName;
    }
}
