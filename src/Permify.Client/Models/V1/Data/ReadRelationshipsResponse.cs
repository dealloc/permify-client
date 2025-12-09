using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Response for <see cref="IDataService.ReadRelationshipsAsync" />.
/// </summary>
/// <param name="Tuples">List of the relationships retrieved in the read operation, represented as entity-relation-entity triples.</param>
/// <param name="ContinuousToken">Used in the case of paginated reads to retrieve the next page of results.</param>
public record ReadRelationshipsResponse(
    IReadOnlyList<Tuple> Tuples,
    string? ContinuousToken
);