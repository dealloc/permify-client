using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Tenancy;

/// <summary>
/// Request for <see cref="ITenancyService.ListTenantsAsync" />
/// </summary>
/// <param name="PageSize">The (maximum) amount of results per page.</param>
/// <param name="ContinuousToken">
/// A string that can be passed to a new <see cref="ListTenantRequest" /> to request the next page.
///
/// An empty string (or <c>null</c> indicates there are no more pages).
/// </param>
public sealed record ListTenantRequest(
    uint PageSize,
    string? ContinuousToken = null
);