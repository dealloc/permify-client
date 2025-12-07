using Permify.Client.Models.Tenancy;

namespace Permify.Client.Contracts;

/// <summary>
/// With Permify Multi Tenancy support you can create custom schemas for tenants and manage them in a single place.
/// </summary>
/// <remarks>
/// Permify has a pre-inserted tenant - <c>t1</c> - by default for the ones that don’t use multi-tenancy.
/// </remarks>
public interface ITenancyService
{
    /// <summary>
    /// Creates or updates a tenant.
    /// If you pass in the ID of an existing tenant, it simply updates it, otherwise it creates a new one.
    /// </summary>
    /// <param name="request">The <see cref="CreateTenantRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    Task<CreateTenantResponse> CreateTenantAsync(CreateTenantRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets (a paginated) list of all tenants.
    /// </summary>
    /// <param name="request">The <see cref="ListTenantRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    Task<ListTenantResponse> ListTenantsAsync(ListTenantRequest request, CancellationToken cancellationToken = default);
}