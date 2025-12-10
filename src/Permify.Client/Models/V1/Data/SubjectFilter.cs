namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Used to filter subjects based on the type, ids and relation.
/// </summary>
/// <param name="Type">The type of entity to filter by.</param>
/// <param name="Ids">The ids to include in the results.</param>
/// <param name="Relation">The relations to filter the subjects by.</param>
public sealed record SubjectFilter(
    string Type,
    IReadOnlyList<string> Ids,
    string Relation
);