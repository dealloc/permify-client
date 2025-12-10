using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Response for <see cref="IDataService.DeleteDataAsync" />
/// </summary>
/// <param name="SnapToken">The snapshot token.</param>
public sealed record DeleteDataResponse(
    string SnapToken
);