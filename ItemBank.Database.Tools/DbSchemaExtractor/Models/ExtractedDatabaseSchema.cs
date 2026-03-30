namespace ItemBank.Database.Tools.DbSchemaExtractor.Models;

public sealed record ExtractedDatabaseSchema
{
    public required string DatabaseName { get; init; }

    public required DateTime GeneratedAtUtc { get; init; }

    public required int SampleSizePerCollection { get; init; }

    public required IReadOnlyList<ExtractedCollectionSchema> Collections { get; init; }
}

public sealed record ExtractedCollectionSchema
{
    public required string Name { get; init; }

    public required long EstimatedDocumentCount { get; init; }

    public required int SampledDocumentCount { get; init; }

    public required IReadOnlyList<ExtractedFieldSchemaFact> Fields { get; init; }

    public required IReadOnlyList<ExtractedIndexSchema> Indexes { get; init; }
}

public sealed record ExtractedFieldSchemaFact
{
    public required string Name { get; init; }

    public required string Path { get; init; }

    public required string ObservationScope { get; init; }

    public required int ParentObservationCount { get; init; }

    public required int PresentCount { get; init; }

    public required int MissingCount { get; init; }

    public required int NullCount { get; init; }

    public required decimal PresenceRate { get; init; }

    public required decimal NullRate { get; init; }

    public required IReadOnlyList<string> BsonTypes { get; init; }

    public required IReadOnlyDictionary<string, int> TypeCounts { get; init; }

    public int ObjectObservationCount { get; init; }

    public IReadOnlyList<ExtractedFieldSchemaFact>? ObjectFields { get; init; }

    public int ArrayObservationCount { get; init; }

    public int ArrayElementCount { get; init; }

    public IReadOnlyList<string>? ArrayElementTypes { get; init; }

    public IReadOnlyDictionary<string, int>? ArrayElementTypeCounts { get; init; }

    public int ArrayObjectElementObservationCount { get; init; }

    public IReadOnlyList<ExtractedFieldSchemaFact>? ArrayObjectFields { get; init; }
}

public sealed record ExtractedIndexSchema
{
    public required string Name { get; init; }

    public required IReadOnlyList<ExtractedIndexField> Fields { get; init; }

    public required IReadOnlyDictionary<string, object?> Options { get; init; }
}

public sealed record ExtractedIndexField
{
    public required string FieldName { get; init; }

    public required object? Value { get; init; }
}
