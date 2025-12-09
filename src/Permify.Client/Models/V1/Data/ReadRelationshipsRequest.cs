using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Request for <see cref="IDataService.ReadRelationshipsAsync" />
/// </summary>
/// <param name="Metadata">Optional metadata to pass along the request.</param>
/// <param name="Filter">A <see cref="RequestFilter" /> to send along with the request.</param>
/// <param name="PageSize">
/// The number of results to return in a single page.
/// If more results are available, a continuous_token is included in the response.
/// </param>
/// <param name="ContinuousToken">Used for pagination, should be the value received in the previous response.</param>
public sealed record ReadRelationshipsRequest(
    ReadRelationshipsRequest.RequestMetadata Metadata,
    ReadRelationshipsRequest.RequestFilter Filter,
    uint PageSize,
    string? ContinuousToken = null
)
{
    /// <summary>
    /// Allows passing in additional request metadata to the <see cref="ReadRelationshipsRequest" />.
    /// </summary>
    /// <param name="SnapToken">The snapshot token.</param>
    public sealed record RequestMetadata(string? SnapToken = null);

    /// <summary>
    /// Used to filter tuples based on the entity, relation and the subject.
    /// </summary>
    /// <param name="Entity">Used to filter entities based on the type and ids.</param>
    /// <param name="Relation">Filter the relations.</param>
    /// <param name="Subject">Used to filter subjects based on the type, ids and relation.</param>
    public sealed record RequestFilter(
        EntityFilter? Entity,
        string? Relation,
        SubjectFilter? Subject
    );

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
};