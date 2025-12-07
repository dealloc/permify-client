using Permify.Client.Contracts;

namespace Permify.Client.Models.Tenancy;

/// <summary>
/// Response of <see cref="ITenancyService.ListTenantsAsync" />
/// </summary>
/// <param name="ContinuousToken">A string that can be sent in a new <see cref="ListTenantRequest" /> to fetch the next page.</param>
/// <param name="Tenants">A list of <see cref="Tenant" /> objects.</param>
public sealed record ListTenantResponse(
    string ContinuousToken,
    IReadOnlyList<Tenant> Tenants
);