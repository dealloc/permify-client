using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Request for <see cref="IDataService.ReadRelationshipsAsync" />
/// </summary>
/// <param name="Metadata">Optional metadata to pass along the request.</param>
/// <param name="Filter">A <see cref="TupleFilter" /> to send along with the request.</param>
/// <param name="PageSize">
/// The number of results to return in a single page.
/// If more results are available, a continuous_token is included in the response.
/// </param>
/// <param name="ContinuousToken">Used for pagination, should be the value received in the previous response.</param>
public sealed record ReadRelationshipsRequest(
    ReadRelationshipsRequest.RequestMetadata Metadata,
    TupleFilter Filter,
    uint PageSize,
    string? ContinuousToken = null
)
{
    /// <summary>
    /// Allows passing in additional request metadata to the <see cref="ReadRelationshipsRequest" />.
    /// </summary>
    /// <param name="SnapToken">The snapshot token.</param>
    public sealed record RequestMetadata(string? SnapToken = null);
};