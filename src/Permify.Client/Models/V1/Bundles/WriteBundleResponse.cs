using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Bundles;

/// <summary>
/// Response for <see cref="IBundleService.WriteBundleAsync" />
/// </summary>
/// <param name="Names">A list of the bundle names created.</param>
public sealed record WriteBundleResponse(IReadOnlyList<string> Names);