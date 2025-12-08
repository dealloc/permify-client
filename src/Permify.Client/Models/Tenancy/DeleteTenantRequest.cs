using Permify.Client.Contracts;

namespace Permify.Client.Models.Tenancy;

/// <summary>
/// Request for <see cref="ITenancyService.DeleteTenantAsync" />
/// </summary>
/// <param name="Id">The identifier of the tenant to delete (see <see cref="CreateTenantRequest.Id" />).</param>
public sealed record DeleteTenantRequest(string Id);