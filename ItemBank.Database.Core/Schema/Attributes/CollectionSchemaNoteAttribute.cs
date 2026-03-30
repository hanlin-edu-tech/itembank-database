namespace ItemBank.Database.Core.Schema.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class CollectionSchemaNoteAttribute(string note) : Attribute
{
    public string Note { get; } = !string.IsNullOrWhiteSpace(note)
        ? note
        : throw new ArgumentException("說明不可為空", nameof(note));
}
