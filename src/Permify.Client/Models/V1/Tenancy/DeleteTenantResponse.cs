using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Tenancy;

/// <summary>
/// Response for <see cref="ITenancyService.DeleteTenantAsync" />
/// </summary>
/// <param name="TenantId">The identifier of the deleted tenant.</param>
public sealed record DeleteTenantResponse(string TenantId);