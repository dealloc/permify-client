using Permify.Client.Contracts;

namespace Permify.Client.Models.Bundles;

/// <summary>
/// Response for <see cref="IBundleService.ReadBundleAsync" />.
/// </summary>
/// <param name="Bundle">The <see cref="Bundle" /> returned.</param>
public record ReadBundleResponse(
    Bundle Bundle
);