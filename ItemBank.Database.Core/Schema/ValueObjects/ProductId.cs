using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ProductId(string Value) : IConvertibleId<ProductId, string>
{
    public string ToValue() => Value;
    public static ProductId FromValue(string value) => new(value);
}
