using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Request for <see cref="IDataService.RunBundleAsync" />
/// </summary>
/// <param name="Name">Name of the bundle to be executed.</param>
/// <param name="Attributes">Additional key-value pairs for execution arguments.</param>
public record RunBundleRequest(
    string Name,
    Dictionary<string, string> Attributes
);