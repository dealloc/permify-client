namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Used to filter attributes based on the entity and attribute names.
/// </summary>
/// <param name="Entity">Used to filter entities based on the type and ids.</param>
/// <param name="Attributes">A list of attributes to include in the results.</param>
public sealed record AttributeFilter(
    EntityFilter? Entity,
    IReadOnlyList<string>? Attributes
);