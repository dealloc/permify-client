using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Response for <see cref="IDataService.WriteDataAsync" />
/// </summary>
/// <param name="SnapToken">The snapshot token.</param>
public sealed record WriteDataResponse(string SnapToken);