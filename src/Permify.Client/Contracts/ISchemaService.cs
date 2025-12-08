using Permify.Client.Exceptions;
using Permify.Client.Models.Schema;

namespace Permify.Client.Contracts;

/// <summary>
/// Modeling and Permify schema-related functionalities including configuration and auditing.
/// </summary>
/// <seealso href="https://docs.permify.co/api-reference/schema" />
public interface ISchemaService
{
    /// <summary>
    /// Permify provide its own authorization language to model common patterns easily.
    /// Permify cals the authorization model Permify Schema, and it can be created on our <a href="https://play.permify.co">playground</a>
    /// as well as in any IDE or text editor.
    /// </summary>
    /// <param name="request">The <see cref="WriteSchemaRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to write the schema.</exception>
    Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Models written to Permify using the <see cref="ISchemaService.WriteSchemaAsync" /> can be listed using this API with
    /// the timestamps at which the models were created.
    /// </summary>
    /// <param name="request">The <see cref="ListSchemaRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to list the schema.</exception>
    Task<ListSchemaResponse> ListSchemaAsync(ListSchemaRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// As development teams regularly roll out new features or API endpoints,
    /// features each addition often necessitates corresponding updates to the Permify schema.
    ///
    /// To streamline this process, Permify has published an endpoint allowing authorized users to make partial updates to the
    /// schema by adding or modifying actions within individual entities.
    /// </summary>
    /// <param name="request">The <see cref="PartialSchemaUpdateRequest" /> containing the actions to execute.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to update the schema.</exception>
    Task<PartialSchemaUpdateResponse> PartialUpdateSchemaAsync(
        PartialSchemaUpdateRequest request,
        CancellationToken cancellationToken
    );
}