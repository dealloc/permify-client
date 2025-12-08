using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Bundles;

/// <summary>
/// Request for <see cref="IBundleService.ReadBundleAsync" />
/// </summary>
/// <param name="Name">The name of the bundle to read (must match <see cref="Bundle.Name" />).</param>
public record ReadBundleRequest(
    string Name
);