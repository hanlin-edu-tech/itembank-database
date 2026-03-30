namespace ItemBank.Database.Core.Schema.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class CollectionSchemaKindAttribute(CollectionSchemaKind kind) : Attribute
{
    public CollectionSchemaKind Kind { get; } = kind;
}
