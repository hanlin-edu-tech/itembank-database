using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ProductSectionId(string Value) : IConvertibleId<ProductSectionId, string>
{
    public string ToValue() => Value;
    public static ProductSectionId FromValue(string value) => new(value);
}
