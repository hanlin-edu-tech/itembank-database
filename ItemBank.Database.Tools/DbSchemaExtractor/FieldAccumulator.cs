using ItemBank.Database.Tools.DbSchemaExtractor.Models;

namespace ItemBank.Database.Tools.DbSchemaExtractor;

internal sealed class ObjectContainerAccumulator
{
    public int ObservationCount { get; private set; }

    public Dictionary<string, FieldAccumulator> Children { get; } = new(StringComparer.Ordinal);

    public void BeginObservation()
    {
        ObservationCount++;

        foreach (var child in Children.Values)
        {
            child.ParentObservationCount++;
        }
    }

    public FieldAccumulator GetOrCreateChild(string name, string path)
    {
        if (Children.TryGetValue(name, out var existingChild))
        {
            return existingChild;
        }

        var child = new FieldAccumulator(name, path, ObservationCount);
        Children.Add(name, child);
        return child;
    }

    public IReadOnlyList<ExtractedFieldSchemaFact> MaterializeFields()
    {
        return MaterializeFields("document");
    }

    public IReadOnlyList<ExtractedFieldSchemaFact> MaterializeFields(string observationScope)
    {
        return Children.Values
            .OrderBy(child => child.Name, StringComparer.Ordinal)
            .Select(child => child.Materialize(observationScope))
            .ToArray();
    }
}

internal sealed class FieldAccumulator(string name, string path, int parentObservationCount)
{
    private readonly HashSet<string> _bsonTypes = new(StringComparer.Ordinal);
    private readonly HashSet<string> _arrayElementTypes = new(StringComparer.Ordinal);
    private readonly Dictionary<string, int> _typeCounts = new(StringComparer.Ordinal);
    private readonly Dictionary<string, int> _arrayElementTypeCounts = new(StringComparer.Ordinal);

    public string Name { get; } = name;

    public string Path { get; } = path;

    public int ParentObservationCount { get; set; } = parentObservationCount;

    public int PresentCount { get; private set; }

    public int NullCount { get; private set; }

    public ObjectContainerAccumulator ObjectContainer { get; } = new();

    public int ArrayObservationCount { get; private set; }

    public int ArrayElementCount { get; private set; }

    public ObjectContainerAccumulator ArrayObjectContainer { get; } = new();

    public void MarkPresent()
    {
        PresentCount++;
    }

    public void MarkNull()
    {
        NullCount++;
    }

    public void ObserveBsonType(string bsonType)
    {
        _bsonTypes.Add(bsonType);

        if (_typeCounts.TryGetValue(bsonType, out var count))
        {
            _typeCounts[bsonType] = count + 1;
            return;
        }

        _typeCounts[bsonType] = 1;
    }

    public void MarkObjectObservation()
    {
    }

    public void MarkArrayObservation()
    {
        ArrayObservationCount++;
    }

    public void MarkArrayElement(string bsonType)
    {
        ArrayElementCount++;
        _arrayElementTypes.Add(bsonType);

        if (_arrayElementTypeCounts.TryGetValue(bsonType, out var count))
        {
            _arrayElementTypeCounts[bsonType] = count + 1;
            return;
        }

        _arrayElementTypeCounts[bsonType] = 1;
    }

    public ExtractedFieldSchemaFact Materialize(string observationScope)
    {
        var objectFields = ObjectContainer.Children.Count > 0
            ? ObjectContainer.MaterializeFields(observationScope)
            : null;
        var arrayObjectFields = ArrayObjectContainer.Children.Count > 0
            ? ArrayObjectContainer.MaterializeFields("arrayElement")
            : null;
        var missingCount = ParentObservationCount - PresentCount;
        var presenceRate = ParentObservationCount > 0
            ? Math.Round((decimal)PresentCount / ParentObservationCount, 4, MidpointRounding.AwayFromZero)
            : 0m;
        var nullRate = PresentCount > 0
            ? Math.Round((decimal)NullCount / PresentCount, 4, MidpointRounding.AwayFromZero)
            : 0m;

        return new ExtractedFieldSchemaFact
        {
            Name = Name,
            Path = Path,
            ObservationScope = observationScope,
            ParentObservationCount = ParentObservationCount,
            PresentCount = PresentCount,
            MissingCount = missingCount,
            NullCount = NullCount,
            PresenceRate = presenceRate,
            NullRate = nullRate,
            BsonTypes = _bsonTypes.OrderBy(type => type, StringComparer.Ordinal).ToArray(),
            TypeCounts = _typeCounts
                .OrderBy(entry => entry.Key, StringComparer.Ordinal)
                .ToDictionary(entry => entry.Key, entry => entry.Value, StringComparer.Ordinal),
            ObjectObservationCount = ObjectContainer.ObservationCount,
            ObjectFields = objectFields,
            ArrayObservationCount = ArrayObservationCount,
            ArrayElementCount = ArrayElementCount,
            ArrayElementTypes = _arrayElementTypes.Count > 0
                ? _arrayElementTypes.OrderBy(type => type, StringComparer.Ordinal).ToArray()
                : null,
            ArrayElementTypeCounts = _arrayElementTypeCounts.Count > 0
                ? _arrayElementTypeCounts
                    .OrderBy(entry => entry.Key, StringComparer.Ordinal)
                    .ToDictionary(entry => entry.Key, entry => entry.Value, StringComparer.Ordinal)
                : null,
            ArrayObjectElementObservationCount = ArrayObjectContainer.ObservationCount,
            ArrayObjectFields = arrayObjectFields
        };
    }
}
