using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Bundles;

/// <summary>
/// Response for <see cref="IBundleService.ReadBundleAsync" />.
/// </summary>
/// <param name="Bundle">The <see cref="Bundle" /> returned.</param>
public record ReadBundleResponse(
    Bundle Bundle
);