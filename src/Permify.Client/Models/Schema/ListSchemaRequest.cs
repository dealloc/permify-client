using Permify.Client.Contracts;

namespace Permify.Client.Models.Schema;

/// <summary>
/// Request for <see cref="ISchemaService.ListSchemaAsync" />.
/// </summary>
/// <param name="PageSize">The number of schemas to be returned in the response, should be between 1 and 100.</param>
/// <param name="ContinuousToken">Used for pagination, should be the value received in the previous response.</param>
public sealed record ListSchemaRequest(
    long PageSize = 20,
    string? ContinuousToken = null
);