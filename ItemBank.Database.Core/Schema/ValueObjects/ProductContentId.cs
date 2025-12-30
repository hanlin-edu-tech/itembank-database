using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ProductContentId(string Value) : IConvertibleId<ProductContentId, string>
{
    public string ToValue() => Value;
    public static ProductContentId FromValue(string value) => new(value);
}
