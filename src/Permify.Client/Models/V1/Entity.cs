namespace Permify.Client.Models.V1;

/// <summary>
/// Represents an entity in Permify with a type and an identifier.
/// </summary>
/// <param name="Type">The type of the entity, this will usually be the name of an entity you defined in your schema.</param>
/// <param name="Id">The unique identifier of this entity, this will usually be the identifier you defined when writing the data.</param>
public sealed record Entity(string Type, string Id);