using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record SubjectId(string Value) : IConvertibleId<SubjectId, string>
{
    public string ToValue() => Value;
    public static SubjectId FromValue(string value) => new(value);
}
