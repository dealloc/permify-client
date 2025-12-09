namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Used to filter entities based on the type and ids.
/// </summary>
/// <param name="Type">The type of entity to filter.</param>
/// <param name="Ids">The ids to include in the results.</param>
public sealed record EntityFilter(
    string Type,
    IReadOnlyList<string> Ids
);