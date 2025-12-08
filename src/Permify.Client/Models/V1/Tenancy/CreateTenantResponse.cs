using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Tenancy;

/// <summary>
/// Response for <see cref="ITenancyService.CreateTenantAsync" />
/// </summary>
/// <param name="Tenant">The tenant as it was created in Permify.</param>
public record CreateTenantResponse(
    Tenant Tenant
);