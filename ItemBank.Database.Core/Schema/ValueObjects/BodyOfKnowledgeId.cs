using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record BodyOfKnowledgeId(string Value) : IConvertibleId<BodyOfKnowledgeId, string>
{
    public string ToValue() => Value;
    public static BodyOfKnowledgeId FromValue(string value) => new(value);
}
