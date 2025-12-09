using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Response for <see cref="IDataService.ReadAttributesAsync" />
/// </summary>
/// <param name="Attributes">A list of the attributes retrieved in the read operation.</param>
/// <param name="ContinuousToken">Used in the case of paginated reads to retrieve the next page of results.</param>
public sealed record ReadAttributesResponse(
    IReadOnlyList<AttributeEntity> Attributes,
    string? ContinuousToken
);