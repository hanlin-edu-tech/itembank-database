using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record RepositoryId(string Value) : IConvertibleId<RepositoryId, string>
{
    public string ToValue() => Value;
    public static RepositoryId FromValue(string value) => new(value);
}
