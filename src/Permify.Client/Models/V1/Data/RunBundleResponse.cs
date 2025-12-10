using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Response of <see cref="IDataService.RunBundleAsync"/>.
/// </summary>
/// <param name="SnapToken">The snapshot token.</param>
public record RunBundleResponse(string SnapToken);