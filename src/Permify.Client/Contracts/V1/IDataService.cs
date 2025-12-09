using Permify.Client.Exceptions;
using Permify.Client.Models.V1.Data;

namespace Permify.Client.Contracts.V1;

/// <summary>
/// In Permify, attributes and relations between your entities, objects and users represents your authorization data.
/// These data stored as tuples in a preferred database.
///
/// Since these attributes and relations are live instances, meaning they can be affected by specific user actions within the application,
/// they can be created/deleted with a simple Permify API call at runtime.
/// </summary>
public interface IDataService
{
    /// <summary>
    /// Writes authorization data to Permify.
    /// </summary>
    /// <param name="request">The <see cref="WriteDataRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to write the data.</exception>
    Task<WriteDataResponse> WriteDataAsync(WriteDataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read API allows for directly querying the stored graph data to display and filter stored relational tuples.
    /// </summary>
    /// <param name="request">The <see cref="ReadRelationshipsRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to read the relationships.</exception>
    Task<ReadRelationshipsResponse> ReadRelationshipsAsync(
        ReadRelationshipsRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Read API allows for directly querying the stored graph data to display and filter stored attributes.
    /// </summary>
    /// <param name="request">The <see cref="ReadAttributesRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to read the attributes.</exception>
    Task<ReadAttributesResponse> ReadAttributesAsync(
        ReadAttributesRequest request,
        CancellationToken cancellationToken = default
    );
}