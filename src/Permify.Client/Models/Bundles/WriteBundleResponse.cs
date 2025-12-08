using Permify.Client.Contracts;

namespace Permify.Client.Models.Bundles;

/// <summary>
/// Response for <see cref="IBundleService.WriteBundleAsync" />
/// </summary>
/// <param name="Names">A list of the bundle names created.</param>
public sealed record WriteBundleResponse(IReadOnlyList<string> Names);