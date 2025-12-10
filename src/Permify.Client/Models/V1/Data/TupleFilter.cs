namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Used to filter tuples based on the entity, relation and the subject.
/// </summary>
/// <param name="Entity">Used to filter entities based on the type and ids.</param>
/// <param name="Relation">Filter the relations.</param>
/// <param name="Subject">Used to filter subjects based on the type, ids and relation.</param>
public sealed record TupleFilter(
    EntityFilter? Entity,
    string? Relation,
    SubjectFilter? Subject
);