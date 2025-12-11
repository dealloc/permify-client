using Permify.Client.Exceptions;
using Permify.Client.Models.V1.Watch;

namespace Permify.Client.Contracts.V1;

/// <summary>
/// The Permify Watch API acts as a real-time broadcaster that shows changes in the relation tuples.
/// </summary>
/// <remarks>
/// The Watch API exclusively supports gRPC and works with PostgreSQL, given the track_commit_timestamp option is enabled.
/// Please note, it doesn’t support in-memory databases or HTTP communication.
/// </remarks>
public interface IWatchService
{
    /// <summary>
    /// Start watching for changes.
    /// </summary>
    /// <param name="request">The <see cref="WatchRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while watching.</exception>
    IAsyncEnumerable<WatchResponse> WatchAsync(WatchRequest request, CancellationToken cancellationToken = default);
}