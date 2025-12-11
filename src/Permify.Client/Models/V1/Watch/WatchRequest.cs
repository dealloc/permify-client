using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Watch;

/// <summary>
/// Request for <see cref="IWatchService.WatchAsync" />.
/// </summary>
/// <param name="SnapToken"></param>
public sealed record WatchRequest(
    string? SnapToken = null
);