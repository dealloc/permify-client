using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Permission;

/// <summary>
/// Response for <see cref="IPermissionService.BulkCheckAccessControlAsync" />
/// </summary>
/// <param name="Results">A list of <see cref="CheckAccessControlResponse" /> for each of the requested checks.</param>
public sealed record BulkCheckAccessControlResponse(
    IReadOnlyList<CheckAccessControlResponse> Results
);
