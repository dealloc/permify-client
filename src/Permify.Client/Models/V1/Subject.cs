namespace Permify.Client.Models.V1;

/// <summary>
/// A subject is an <see cref="Entity" /> with a <see cref="Relation" />.
/// </summary>
/// <param name="Type"></param>
/// <param name="Id"></param>
/// <param name="Relation"></param>
public sealed record Subject(
    string Type,
    string Id,
    string? Relation = null
);