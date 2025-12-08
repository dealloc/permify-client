namespace Permify.Client.Models.V1.Tenancy;

/// <summary>
/// Represents a tenant in Permify.
/// </summary>
/// <param name="Id">The identifier of the tenant as it should be passed in the URL.</param>
/// <param name="Name">The human-readable name of the tenant.</param>
/// <param name="CreatedAt">When the tenant was created.</param>
public sealed record Tenant(
    string Id,
    string Name,
    DateTimeOffset CreatedAt
);