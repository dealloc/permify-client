namespace Permify.Client.Models.V1;

/// <summary>
/// Attribute represents an attribute of an entity with a specific type and value.
/// </summary>
public sealed record AttributeEntity(
    Entity Entity,
    string Attribute,
    object Value
);