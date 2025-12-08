using Permify.Client.Contracts;

namespace Permify.Client.Models.Tenancy;

/// <summary>
/// Response for <see cref="ITenancyService.DeleteTenantAsync" />
/// </summary>
/// <param name="TenantId">The identifier of the deleted tenant.</param>
public sealed record DeleteTenantResponse(string TenantId);