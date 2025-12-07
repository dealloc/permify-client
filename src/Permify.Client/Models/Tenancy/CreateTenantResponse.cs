using Permify.Client.Contracts;

namespace Permify.Client.Models.Tenancy;

/// <summary>
/// Response for <see cref="ITenancyService.CreateTenantAsync" />
/// </summary>
/// <param name="Tenant">The tenant as it was created in Permify.</param>
public record CreateTenantResponse(
    Tenant Tenant
);