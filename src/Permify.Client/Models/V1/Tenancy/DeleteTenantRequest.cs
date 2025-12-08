using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Tenancy;

/// <summary>
/// Request for <see cref="ITenancyService.DeleteTenantAsync" />
/// </summary>
/// <param name="Id">The identifier of the tenant to delete (see <see cref="CreateTenantRequest.Id" />).</param>
public sealed record DeleteTenantRequest(string Id);