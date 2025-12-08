using Permify.Client.Models.Bundles;

namespace Permify.Client.Contracts;

/// <summary>
/// Ensuring that authorization data remains in sync with the business model is an important practice when using Permify.
///
/// Prior to Data Bundles, it was the responsibility of the services (such as WriteData API) to structure how relations
/// are created and deleted when actions occur on resources.
///
/// With the Data Bundles, you be able to bundle and model the creation and deletion of relations and attributes when
/// specific actions occur on resources in your applications.
/// </summary>
/// <seealso href="https://docs.permify.co/api-reference/bundle" />
public interface IBundleService
{
    /// <summary>
    /// Write one or more bundles to Permify.
    /// </summary>
    /// <param name="request">The <see cref="WriteBundleRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    Task<WriteBundleResponse> WriteBundleAsync(WriteBundleRequest request, CancellationToken cancellationToken);
}