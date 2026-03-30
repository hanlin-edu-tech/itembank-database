using ItemBank.Database.Tools.DbSchemaExtractor.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemBank.Database.Tools.DbSchemaExtractor;

public sealed class DbSchemaExtractor
{
    public async Task<ExtractedDatabaseSchema> ExtractAsync(
        string connectionString,
        string databaseName,
        int sampleSize,
        IReadOnlyCollection<string>? collectionNames = null,
        CancellationToken cancellationToken = default)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var includedCollections = collectionNames is { Count: > 0 }
            ? new HashSet<string>(collectionNames, StringComparer.Ordinal)
            : null;

        var collectionDescriptors = await GetCollectionDescriptorsAsync(database, includedCollections, cancellationToken)
            .ConfigureAwait(false);

        var collections = new List<ExtractedCollectionSchema>(collectionDescriptors.Count);

        foreach (var descriptor in collectionDescriptors)
        {
            collections.Add(await ExtractCollectionAsync(database, descriptor.Name, sampleSize, cancellationToken)
                .ConfigureAwait(false));
        }

        return new ExtractedDatabaseSchema
        {
            DatabaseName = databaseName,
            GeneratedAtUtc = DateTime.UtcNow,
            SampleSizePerCollection = sampleSize,
            Collections = collections
        };
    }

    private static async Task<List<CollectionDescriptor>> GetCollectionDescriptorsAsync(
        IMongoDatabase database,
        HashSet<string>? includedCollections,
        CancellationToken cancellationToken)
    {
        using var cursor = await database.ListCollectionsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        var collections = await cursor.ToListAsync(cancellationToken).ConfigureAwait(false);

        return collections
            .Select(collection => new CollectionDescriptor(
                collection.GetValue("name").AsString,
                collection.TryGetValue("type", out var typeValue) ? typeValue.AsString : null))
            .Where(collection => string.Equals(collection.Type, "collection", StringComparison.OrdinalIgnoreCase))
            .Where(collection => includedCollections is null || includedCollections.Contains(collection.Name))
            .OrderBy(collection => collection.Name, StringComparer.Ordinal)
            .ToList();
    }

    private static async Task<ExtractedCollectionSchema> ExtractCollectionAsync(
        IMongoDatabase database,
        string collectionName,
        int sampleSize,
        CancellationToken cancellationToken)
    {
        var collection = database.GetCollection<BsonDocument>(collectionName);
        var estimatedDocumentCount = await collection.EstimatedDocumentCountAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var sampledDocuments = await collection
            .Find(FilterDefinition<BsonDocument>.Empty)
            .Limit(sampleSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var rootContainer = new ObjectContainerAccumulator();

        foreach (var sampledDocument in sampledDocuments)
        {
            ProcessObject(sampledDocument, rootContainer, pathPrefix: string.Empty);
        }

        var indexes = await ExtractIndexesAsync(collection, cancellationToken).ConfigureAwait(false);

        return new ExtractedCollectionSchema
        {
            Name = collectionName,
            EstimatedDocumentCount = estimatedDocumentCount,
            SampledDocumentCount = sampledDocuments.Count,
            Fields = rootContainer.MaterializeFields(),
            Indexes = indexes
        };
    }

    private static void ProcessObject(BsonDocument document, ObjectContainerAccumulator container, string pathPrefix)
    {
        container.BeginObservation();

        foreach (var element in document.Elements)
        {
            var fieldPath = string.IsNullOrEmpty(pathPrefix) ? element.Name : $"{pathPrefix}.{element.Name}";
            var child = container.GetOrCreateChild(element.Name, fieldPath);
            child.MarkPresent();
            ObserveValue(child, element.Value);
        }
    }

    private static void ObserveValue(FieldAccumulator accumulator, BsonValue value)
    {
        accumulator.ObserveBsonType(MapBsonType(value.BsonType));

        if (value.BsonType == BsonType.Null)
        {
            accumulator.MarkNull();
            return;
        }

        switch (value.BsonType)
        {
            case BsonType.Document:
                accumulator.MarkObjectObservation();
                ProcessObject(value.AsBsonDocument, accumulator.ObjectContainer, accumulator.Path);
                break;

            case BsonType.Array:
                accumulator.MarkArrayObservation();
                ProcessArray(accumulator, value.AsBsonArray);
                break;
        }
    }

    private static void ProcessArray(FieldAccumulator accumulator, BsonArray array)
    {
        foreach (var item in array)
        {
            accumulator.MarkArrayElement(MapBsonType(item.BsonType));

            if (item.BsonType == BsonType.Document)
            {
                ProcessObject(item.AsBsonDocument, accumulator.ArrayObjectContainer, $"{accumulator.Path}[]");
            }
        }
    }

    private static async Task<IReadOnlyList<ExtractedIndexSchema>> ExtractIndexesAsync(
        IMongoCollection<BsonDocument> collection,
        CancellationToken cancellationToken)
    {
        using var cursor = await collection.Indexes.ListAsync(cancellationToken).ConfigureAwait(false);
        var indexDocuments = await cursor.ToListAsync(cancellationToken).ConfigureAwait(false);

        return indexDocuments
            .Select(MaterializeIndex)
            .OrderBy(index => index.Name, StringComparer.Ordinal)
            .ToArray();
    }

    private static ExtractedIndexSchema MaterializeIndex(BsonDocument indexDocument)
    {
        var indexName = indexDocument.TryGetValue("name", out var nameValue)
            ? nameValue.AsString
            : string.Empty;
        var keyDocument = indexDocument.TryGetValue("key", out var keyValue)
            ? keyValue.AsBsonDocument
            : [];

        var fields = keyDocument.Elements
            .Select(element => new ExtractedIndexField
            {
                FieldName = element.Name,
                Value = ConvertBsonValue(element.Value)
            })
            .ToArray();

        var options = indexDocument.Elements
            .Where(element => element.Name is not "v" and not "ns" and not "key" and not "name")
            .OrderBy(element => element.Name, StringComparer.Ordinal)
            .ToDictionary(
                element => element.Name,
                element => ConvertBsonValue(element.Value),
                StringComparer.Ordinal);

        return new ExtractedIndexSchema
        {
            Name = indexName,
            Fields = fields,
            Options = options
        };
    }

    private static object? ConvertBsonValue(BsonValue value)
    {
        return value.BsonType switch
        {
            BsonType.Null => null,
            BsonType.Boolean => value.AsBoolean,
            BsonType.String => value.AsString,
            BsonType.Int32 => value.AsInt32,
            BsonType.Int64 => value.AsInt64,
            BsonType.Double => value.AsDouble,
            BsonType.Decimal128 => value.AsDecimal128.ToString(),
            BsonType.DateTime => value.ToUniversalTime(),
            BsonType.ObjectId => value.AsObjectId.ToString(),
            BsonType.Array => value.AsBsonArray.Select(ConvertBsonValue).ToArray(),
            BsonType.Document => value.AsBsonDocument.Elements
                .OrderBy(element => element.Name, StringComparer.Ordinal)
                .ToDictionary(
                    element => element.Name,
                    element => ConvertBsonValue(element.Value),
                    StringComparer.Ordinal),
            _ => value.ToString()
        };
    }

    private static string MapBsonType(BsonType bsonType)
    {
        return bsonType switch
        {
            BsonType.Double => "double",
            BsonType.String => "string",
            BsonType.Document => "document",
            BsonType.Array => "array",
            BsonType.Binary => "binary",
            BsonType.ObjectId => "objectId",
            BsonType.Boolean => "boolean",
            BsonType.DateTime => "datetime",
            BsonType.Null => "null",
            BsonType.RegularExpression => "regex",
            BsonType.JavaScript => "javascript",
            BsonType.Symbol => "symbol",
            BsonType.JavaScriptWithScope => "javascriptWithScope",
            BsonType.Int32 => "int32",
            BsonType.Timestamp => "timestamp",
            BsonType.Int64 => "int64",
            BsonType.Decimal128 => "decimal128",
            BsonType.MinKey => "minKey",
            BsonType.MaxKey => "maxKey",
            BsonType.Undefined => "undefined",
            _ => bsonType.ToString()
        };
    }

    private sealed record CollectionDescriptor(string Name, string? Type);
}
