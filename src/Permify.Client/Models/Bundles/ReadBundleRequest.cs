using Permify.Client.Contracts;

namespace Permify.Client.Models.Bundles;

/// <summary>
/// Request for <see cref="IBundleService.ReadBundleAsync" />
/// </summary>
/// <param name="Name">The name of the bundle to read (must match <see cref="Bundle.Name" />).</param>
public record ReadBundleRequest(
    string Name
);