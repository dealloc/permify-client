using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Tenancy;

/// <summary>
/// Request for <see cref="ITenancyService.CreateTenantAsync" />
/// </summary>
/// <param name="Id">
/// The identifier of the tenant, as it should be used in the URL of further calls.
///
/// Note that identifiers have to match the regex <c>[a-zA-Z0-9-,]+</c>
/// </param>
/// <param name="Name">The name of the tenant as it should be shown in the UI.</param>
public sealed record CreateTenantRequest(
    string Id,
    string Name
);