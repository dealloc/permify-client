namespace Permify.Client.Models.V1.Permission;

/// <summary>
/// Context encapsulates the information related to a single operation,
/// including the tuples involved and the associated attributes.
/// </summary>
/// <param name="Tuples">A repeated field of tuples involved in the operation.</param>
/// <param name="Attributes">A repeated field of attributes associated with the operation.</param>
/// <param name="Data">Additional data associated with the context.</param>
public record Context(
    IReadOnlyList<Tuple> Tuples,
    IReadOnlyList<AttributeEntity> Attributes,
    Dictionary<string, string> Data
);