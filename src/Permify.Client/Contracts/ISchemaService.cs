using Permify.Client.Exceptions;
using Permify.Client.Models.Schema;

namespace Permify.Client.Contracts;

/// <summary>
/// Modeling and Permify schema-related functionalities including configuration and auditing.
/// </summary>
public interface ISchemaService
{
    /// <summary>
    /// Overwrites the Permify schema with a new version.
    /// </summary>
    /// <param name="request">The <see cref="WriteSchemaRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to write the schema.</exception>
    Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Lists all models written to Permify with the write schema API along with timestamps when they were created.
    /// </summary>
    /// <param name="request">The <see cref="ListSchemaRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to list the schema.</exception>
    Task<ListSchemaResponse> ListSchemaAsync(ListSchemaRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Allows authorized users to make partial updates to the schema by adding or modifying actions within individual entities.
    /// </summary>
    /// <param name="request">The <see cref="PartialSchemaUpdateRequest" /> containing the actions to execute.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to update the schema.</exception>
    Task<PartialSchemaUpdateResponse> PartialUpdateSchemaAsync(
        PartialSchemaUpdateRequest request,
        CancellationToken cancellationToken
    );
}