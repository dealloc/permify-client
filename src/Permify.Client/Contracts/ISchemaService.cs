using Permify.Client.Models.Schema;

namespace Permify.Client.Contracts;

/// <summary>
/// Modeling and Permify schema related functionalities including configuration and auditing.
/// </summary>
public interface ISchemaService
{
    /// <summary>
    /// Overwrites the Permify schema with a new version.
    /// </summary>
    /// <param name="request">The <see cref="WriteSchemaRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <returns>The <see cref="WriteSchemaResponse" /> returned by the schema service.</returns>
    Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request, CancellationToken cancellationToken);
}