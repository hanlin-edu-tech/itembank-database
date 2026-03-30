using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record PromptId(string Value) : IConvertibleId<PromptId, string>
{
    public string ToValue() => Value;
    public static PromptId FromValue(string value) => new(value);
}
